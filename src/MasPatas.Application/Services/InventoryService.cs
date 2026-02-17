using AutoMapper;
using MasPatas.Application.DTOs;
using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;

namespace MasPatas.Application.Services;

public class InventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryMovementRepository _inventoryMovementRepository;
    private readonly IMapper _mapper;

    public InventoryService(
        IInventoryRepository inventoryRepository,
        IInventoryMovementRepository inventoryMovementRepository,
        IMapper mapper)
    {
        _inventoryRepository = inventoryRepository;
        _inventoryMovementRepository = inventoryMovementRepository;
        _mapper = mapper;
    }

    public async Task<List<InventoryDto>> GetInventoryAsync(CancellationToken cancellationToken = default)
    {
        var inventory = await _inventoryRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<InventoryDto>>(inventory);
    }

    public async Task<InventoryMovementDto> AddInventoryMovementAsync(AddInventoryMovementRequest request, CancellationToken cancellationToken = default)
    {
        var inventory = await _inventoryRepository.GetByProductIdAsync(request.ProductId, cancellationToken)
            ?? new Inventory { ProductId = request.ProductId, Stock = 0, MinimumStock = 0 };

        var normalizedType = request.Type.ToUpperInvariant();
        if (normalizedType == "IN")
        {
            inventory.Stock += request.Quantity;
        }
        else if (normalizedType == "OUT")
        {
            inventory.Stock -= request.Quantity;
            if (inventory.Stock < 0)
            {
                throw new InvalidOperationException("Not enough stock for OUT movement.");
            }
        }

        await _inventoryRepository.UpsertAsync(inventory, cancellationToken);

        var movement = new InventoryMovement
        {
            ProductId = request.ProductId,
            Type = normalizedType,
            Quantity = request.Quantity,
            Reason = request.Reason,
            CreatedAt = DateTime.UtcNow
        };

        await _inventoryMovementRepository.CreateAsync(movement, cancellationToken);
        return _mapper.Map<InventoryMovementDto>(movement);
    }
}
