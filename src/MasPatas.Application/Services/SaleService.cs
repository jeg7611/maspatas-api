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
    private readonly IMapper _mapper;

    public SaleService(
        ISaleRepository saleRepository,
        IInventoryRepository inventoryRepository,
        IInventoryMovementRepository inventoryMovementRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _inventoryRepository = inventoryRepository;
        _inventoryMovementRepository = inventoryMovementRepository;
        _mapper = mapper;
    }

    public async Task<SaleDto> RegisterSaleAsync(RegisterSaleRequest request, CancellationToken cancellationToken = default)
    {
        foreach (var item in request.Items)
        {
            var inventory = await _inventoryRepository.GetByProductIdAsync(item.ProductId, cancellationToken)
                ?? throw new InvalidOperationException($"Inventory not found for product {item.ProductId}");

            if (inventory.Stock < item.Quantity)
            {
                throw new InvalidOperationException($"Not enough stock for product {item.ProductId}");
            }

            inventory.Stock -= item.Quantity;
            await _inventoryRepository.UpsertAsync(inventory, cancellationToken);

            await _inventoryMovementRepository.CreateAsync(new InventoryMovement
            {
                ProductId = item.ProductId,
                Type = "OUT",
                Quantity = item.Quantity,
                Reason = "Sale",
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);
        }

        var sale = new Sale
        {
            CustomerId = request.CustomerId,
            Items = request.Items.Select(i => new SaleItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList(),
            TotalAmount = request.Items.Sum(i => i.Quantity * i.UnitPrice),
            CreatedAt = DateTime.UtcNow
        };

        await _saleRepository.CreateAsync(sale, cancellationToken);
        return _mapper.Map<SaleDto>(sale);
    }

    public async Task<List<SaleDto>> GetSalesAsync(CancellationToken cancellationToken = default)
    {
        var sales = await _saleRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<SaleDto>>(sales);
    }
}
