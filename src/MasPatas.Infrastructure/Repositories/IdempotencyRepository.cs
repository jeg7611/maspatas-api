using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;
using MasPatas.Infrastructure.Persistence;
using MongoDB.Driver;

namespace MasPatas.Infrastructure.Repositories;

public class IdempotencyRepository : IIdempotencyRepository
{
    private readonly MongoDbContext _context;

    public IdempotencyRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<IdempotencyRecord?> GetByRequestIdAsync(string requestId, string operation, CancellationToken cancellationToken = default)
        => await _context.IdempotencyRecords
            .Find(x => x.RequestId == requestId && x.Operation == operation)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task CreateAsync(IdempotencyRecord record, IClientSessionHandle session, CancellationToken cancellationToken = default)
        => await _context.IdempotencyRecords.InsertOneAsync(session, record, cancellationToken: cancellationToken);
}
