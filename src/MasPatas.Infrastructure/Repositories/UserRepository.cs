using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;
using MasPatas.Infrastructure.Persistence;
using MongoDB.Driver;

namespace MasPatas.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MongoDbContext _context;

    public UserRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
        => await _context.Users.InsertOneAsync(user, cancellationToken: cancellationToken);

     public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Users.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        => await _context.Users.Find(x => x.Username == username).FirstOrDefaultAsync(cancellationToken);
}
