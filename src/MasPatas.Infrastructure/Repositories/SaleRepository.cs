using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;
using MasPatas.Infrastructure.Persistence;
using MongoDB.Driver;

namespace MasPatas.Infrastructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly MongoDbContext _context;

    public SaleRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        => await _context.Sales.InsertOneAsync(sale, cancellationToken: cancellationToken);

    public async Task<List<Sale>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Sales.Find(_ => true).ToListAsync(cancellationToken);
}
