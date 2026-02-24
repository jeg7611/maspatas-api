using MasPatas.Domain.Entities;
using MongoDB.Driver;

namespace MasPatas.Application.Interfaces;

public interface IProductRepository
{
    Task CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface ICustomerRepository
{
    Task CreateAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
}

public interface IInventoryRepository
{
    Task<List<Inventory>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Inventory?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task UpsertAsync(Inventory inventory, CancellationToken cancellationToken = default);
    Task<bool> ReserveStockAsync(Guid productId, int quantity, IClientSessionHandle session, CancellationToken cancellationToken = default);
}

public interface IInventoryMovementRepository
{
    Task CreateAsync(InventoryMovement movement, CancellationToken cancellationToken = default);
    Task CreateAsync(InventoryMovement movement, IClientSessionHandle session, CancellationToken cancellationToken = default);
    Task<List<InventoryMovement>> GetAllAsync(CancellationToken cancellationToken = default);
}

public interface ISaleRepository
{
    Task CreateAsync(Sale sale, CancellationToken cancellationToken = default);
    Task CreateAsync(Sale sale, IClientSessionHandle session, CancellationToken cancellationToken = default);
    Task<List<Sale>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> MarkAsPaidAsync(Guid saleId, int version, Payment payment, IClientSessionHandle session, CancellationToken cancellationToken = default);
    Task<bool> CancelAsync(Guid saleId, int version, IClientSessionHandle session, CancellationToken cancellationToken = default);
}

public interface IAuditLogRepository
{
    Task CreateAsync(AuditLog log, IClientSessionHandle session, CancellationToken cancellationToken = default);
}

public interface IIdempotencyRepository
{
    Task<IdempotencyRecord?> GetByRequestIdAsync(string requestId, string operation, CancellationToken cancellationToken = default);
    Task CreateAsync(IdempotencyRecord record, IClientSessionHandle session, CancellationToken cancellationToken = default);
}

public interface IMongoTransactionManager
{
    Task<T> ExecuteAsync<T>(Func<IClientSessionHandle, Task<T>> action, CancellationToken cancellationToken = default);
}

public interface IUserRepository
{
    Task CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
}
