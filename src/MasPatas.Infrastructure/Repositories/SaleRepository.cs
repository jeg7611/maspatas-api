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

    public async Task CreateAsync(Sale sale, IClientSessionHandle session, CancellationToken cancellationToken = default)
        => await _context.Sales.InsertOneAsync(session, sale, cancellationToken: cancellationToken);

    public async Task<List<Sale>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Sales.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Sales.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

    public async Task<bool> MarkAsPaidAsync(Guid saleId, int version, Payment payment, IClientSessionHandle session, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Sale>.Filter.And(
            Builders<Sale>.Filter.Eq(x => x.Id, saleId),
            Builders<Sale>.Filter.Eq(x => x.Version, version),
            Builders<Sale>.Filter.Eq(x => x.Status, SaleStatus.PendingPayment));

        var update = Builders<Sale>.Update
            .Set(x => x.Status, SaleStatus.Paid)
            .Push(x => x.Payments, payment)
            .Set(x => x.UpdatedAt, DateTime.UtcNow)
            .Inc(x => x.Version, 1);

        var result = await _context.Sales.UpdateOneAsync(session, filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount == 1;
    }

    public async Task<bool> CancelAsync(Guid saleId, int version, IClientSessionHandle session, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Sale>.Filter.And(
            Builders<Sale>.Filter.Eq(x => x.Id, saleId),
            Builders<Sale>.Filter.Eq(x => x.Version, version),
            Builders<Sale>.Filter.Eq(x => x.Status, SaleStatus.PendingPayment));

        var update = Builders<Sale>.Update
            .Set(x => x.Status, SaleStatus.Cancelled)
            .Set(x => x.UpdatedAt, DateTime.UtcNow)
            .Inc(x => x.Version, 1);

        var result = await _context.Sales.UpdateOneAsync(session, filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount == 1;
    }
}
