using MasPatas.Application.Interfaces;
using MongoDB.Driver;

namespace MasPatas.Infrastructure.Persistence;

public class MongoTransactionManager : IMongoTransactionManager
{
    private readonly MongoDbContext _context;

    public MongoTransactionManager(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<T> ExecuteAsync<T>(Func<IClientSessionHandle, Task<T>> action, CancellationToken cancellationToken = default)
    {
        using var session = await _context.Client.StartSessionAsync(cancellationToken: cancellationToken);
        return await session.WithTransactionAsync(async (s, ct) => await action(s), cancellationToken: cancellationToken);
    }
}
