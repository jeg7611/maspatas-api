using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;
using MasPatas.Infrastructure.Persistence;
using MongoDB.Driver;

namespace MasPatas.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly MongoDbContext _context;

    public CustomerRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Customer customer, CancellationToken cancellationToken = default)
        => await _context.Customers.InsertOneAsync(customer, cancellationToken: cancellationToken);

    public async Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Customers.Find(_ => true).ToListAsync(cancellationToken);
}
