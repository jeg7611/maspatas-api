using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;
using MasPatas.Infrastructure.Persistence;

namespace MasPatas.Infrastructure.Repositories;

public class InventoryMovementRepository : IInventoryMovementRepository
{
    private readonly MongoDbContext _context;

    public InventoryMovementRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(InventoryMovement movement, CancellationToken cancellationToken = default)
        => await _context.InventoryMovements.InsertOneAsync(movement, cancellationToken: cancellationToken);
}
