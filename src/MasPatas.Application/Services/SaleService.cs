using AutoMapper;
using MasPatas.Application.DTOs;
using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;

namespace MasPatas.Application.Services;

public class SaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryMovementRepository _inventoryMovementRepository;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IIdempotencyRepository _idempotencyRepository;
    private readonly IMongoTransactionManager _transactionManager;
    private readonly IResiliencePolicyExecutor _resilience;
    private readonly IMapper _mapper;

    public SaleService(
        ISaleRepository saleRepository,
        IInventoryRepository inventoryRepository,
        IInventoryMovementRepository inventoryMovementRepository,
        IAuditLogRepository auditLogRepository,
        IIdempotencyRepository idempotencyRepository,
        IMongoTransactionManager transactionManager,
        IResiliencePolicyExecutor resilience,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _inventoryRepository = inventoryRepository;
        _inventoryMovementRepository = inventoryMovementRepository;
        _auditLogRepository = auditLogRepository;
        _idempotencyRepository = idempotencyRepository;
        _transactionManager = transactionManager;
        _resilience = resilience;
        _mapper = mapper;
    }

    public async Task<SaleDto> CreateSaleAsync(CreateSaleCommand command, CancellationToken cancellationToken = default)
    {
        return await _resilience.ExecuteAsync(async ct =>
        {
            var existing = await _idempotencyRepository.GetByRequestIdAsync(command.RequestId, "Sell", ct);
            if (existing is not null && Guid.TryParse(existing.ResourceId, out var existingId))
            {
                var existingSale = await _saleRepository.GetByIdAsync(existingId, ct)
                    ?? throw new InvalidOperationException("Idempotency record references missing sale.");
                return _mapper.Map<SaleDto>(existingSale);
            }

            return await _transactionManager.ExecuteAsync(async session =>
            {
                var sale = new Sale
                {
                    CustomerId = command.CustomerId,
                    Items = command.Items.Select(i => new SaleItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.Price
                    }).ToList(),
                    TotalAmount = command.Items.Sum(i => i.Quantity * i.Price),
                    Status = SaleStatus.PendingPayment,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Version = 1
                };

                foreach (var item in sale.Items)
                {
                    var reserved = await _inventoryRepository.ReserveStockAsync(item.ProductId, item.Quantity, session, ct);
                    if (!reserved)
                    {
                        throw new InvalidOperationException($"Not enough stock for product {item.ProductId}");
                    }

                    await _inventoryMovementRepository.CreateAsync(new InventoryMovement
                    {
                        ProductId = item.ProductId,
                        Type = "OUT",
                        Quantity = item.Quantity,
                        Reason = $"Sale reservation {sale.Id}",
                        CreatedAt = DateTime.UtcNow
                    }, session, ct);
                }

                await _saleRepository.CreateAsync(sale, session, ct);
                await _idempotencyRepository.CreateAsync(new IdempotencyRecord
                {
                    RequestId = command.RequestId,
                    Operation = "Sell",
                    ResourceId = sale.Id.ToString()
                }, session, ct);

                await _auditLogRepository.CreateAsync(new AuditLog
                {
                    EntityType = "Sale",
                    EntityId = sale.Id.ToString(),
                    Action = "Sell",
                    UserId = command.UserId,
                    RequestId = command.RequestId,
                    Timestamp = DateTime.UtcNow,
                    Changes = $"{{\"status\":\"{sale.Status}\",\"totalAmount\":{sale.TotalAmount}}}"
                }, session, ct);

                return _mapper.Map<SaleDto>(sale);
            }, ct);
        }, cancellationToken);
    }

    public async Task<SaleDto> RegisterPaymentAsync(RegisterPaymentCommand command, CancellationToken cancellationToken = default)
    {
        return await _resilience.ExecuteAsync(async ct =>
        {
            var existing = await _idempotencyRepository.GetByRequestIdAsync(command.RequestId, "Pay", ct);
            if (existing is not null && Guid.TryParse(existing.ResourceId, out var existingId))
            {
                var paidSale = await _saleRepository.GetByIdAsync(existingId, ct)
                    ?? throw new InvalidOperationException("Idempotency record references missing sale.");
                return _mapper.Map<SaleDto>(paidSale);
            }

            return await _transactionManager.ExecuteAsync(async session =>
            {
                var sale = await _saleRepository.GetByIdAsync(command.SaleId, ct)
                    ?? throw new KeyNotFoundException("Sale not found.");

                if (sale.Status != SaleStatus.PendingPayment)
                {
                    throw new InvalidOperationException($"Payment not allowed for sale status '{sale.Status}'.");
                }

                if (command.Amount != sale.TotalAmount)
                {
                    throw new InvalidOperationException("Payment amount must match sale total amount.");
                }

                var payment = new Payment
                {
                    SaleId = sale.Id,
                    PaymentMethod = command.PaymentMethod,
                    Amount = command.Amount,
                    PaidAt = DateTime.UtcNow,
                    RequestId = command.RequestId
                };

                var updated = await _saleRepository.MarkAsPaidAsync(sale.Id, sale.Version, payment, session, ct);
                if (!updated)
                {
                    throw new InvalidOperationException("Sale was modified concurrently. Please retry.");
                }

                await _idempotencyRepository.CreateAsync(new IdempotencyRecord
                {
                    RequestId = command.RequestId,
                    Operation = "Pay",
                    ResourceId = sale.Id.ToString()
                }, session, ct);

                await _auditLogRepository.CreateAsync(new AuditLog
                {
                    EntityType = "Payment",
                    EntityId = payment.PaymentId.ToString(),
                    Action = "Pay",
                    UserId = command.UserId,
                    RequestId = command.RequestId,
                    Timestamp = DateTime.UtcNow,
                    Changes = $"{{\"saleId\":\"{sale.Id}\",\"amount\":{payment.Amount},\"method\":\"{payment.PaymentMethod}\"}}"
                }, session, ct);

                var result = await _saleRepository.GetByIdAsync(sale.Id, ct)
                    ?? throw new InvalidOperationException("Unable to read updated sale.");

                return _mapper.Map<SaleDto>(result);
            }, ct);
        }, cancellationToken);
    }

    public async Task<SaleDto> CancelSaleAsync(CancelSaleCommand command, CancellationToken cancellationToken = default)
    {
        return await _resilience.ExecuteAsync(async ct =>
        {
            var existing = await _idempotencyRepository.GetByRequestIdAsync(command.RequestId, "Cancel", ct);
            if (existing is not null && Guid.TryParse(existing.ResourceId, out var existingId))
            {
                var cancelledSale = await _saleRepository.GetByIdAsync(existingId, ct)
                    ?? throw new InvalidOperationException("Idempotency record references missing sale.");
                return _mapper.Map<SaleDto>(cancelledSale);
            }

            return await _transactionManager.ExecuteAsync(async session =>
            {
                var sale = await _saleRepository.GetByIdAsync(command.SaleId, ct)
                    ?? throw new KeyNotFoundException("Sale not found.");

                if (sale.Status != SaleStatus.PendingPayment)
                {
                    throw new InvalidOperationException("Cancellation only allowed for pending payment sales.");
                }

                var updated = await _saleRepository.CancelAsync(sale.Id, sale.Version, session, ct);
                if (!updated)
                {
                    throw new InvalidOperationException("Sale was modified concurrently. Please retry.");
                }

                await _idempotencyRepository.CreateAsync(new IdempotencyRecord
                {
                    RequestId = command.RequestId,
                    Operation = "Cancel",
                    ResourceId = sale.Id.ToString()
                }, session, ct);

                await _auditLogRepository.CreateAsync(new AuditLog
                {
                    EntityType = "Sale",
                    EntityId = sale.Id.ToString(),
                    Action = "Cancel",
                    UserId = command.UserId,
                    RequestId = command.RequestId,
                    Timestamp = DateTime.UtcNow,
                    Changes = "{\"status\":\"Cancelled\"}"
                }, session, ct);

                var result = await _saleRepository.GetByIdAsync(sale.Id, ct)
                    ?? throw new InvalidOperationException("Unable to read cancelled sale.");

                return _mapper.Map<SaleDto>(result);
            }, ct);
        }, cancellationToken);
    }

    public async Task<List<SaleDto>> GetSalesAsync(CancellationToken cancellationToken = default)
    {
        var sales = await _resilience.ExecuteAsync(ct => _saleRepository.GetAllAsync(ct), cancellationToken);
        return _mapper.Map<List<SaleDto>>(sales);
    }
}
