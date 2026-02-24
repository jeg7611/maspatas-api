using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;
using MasPatas.Infrastructure.Persistence;
using MongoDB.Driver;

namespace MasPatas.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly MongoDbContext _context;

    public AuditLogRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(AuditLog log, IClientSessionHandle session, CancellationToken cancellationToken = default)
        => await _context.AuditLogs.InsertOneAsync(session, log, cancellationToken: cancellationToken);
}
