using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;
using MasPatas.Infrastructure.Persistence;
using MongoDB.Driver;

namespace MasPatas.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly MongoDbContext _context;

    public ProductRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Product product, CancellationToken cancellationToken = default)
        => await _context.Products.InsertOneAsync(product, cancellationToken: cancellationToken);

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Products.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
}
