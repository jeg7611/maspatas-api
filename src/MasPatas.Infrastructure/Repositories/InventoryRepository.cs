using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;
using MasPatas.Infrastructure.Persistence;
using MongoDB.Driver;

namespace MasPatas.Infrastructure.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly MongoDbContext _context;

    public InventoryRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Inventory>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Inventory.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<Inventory?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        => await _context.Inventory.Find(x => x.ProductId == productId).FirstOrDefaultAsync(cancellationToken);

    public async Task UpsertAsync(Inventory inventory, CancellationToken cancellationToken = default)
    {
        await _context.Inventory.ReplaceOneAsync(
            x => x.ProductId == inventory.ProductId,
            inventory,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken);
    }
}
