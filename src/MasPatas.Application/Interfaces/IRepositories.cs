using MasPatas.Domain.Entities;

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
}

public interface IInventoryMovementRepository
{
    Task CreateAsync(InventoryMovement movement, CancellationToken cancellationToken = default);
}

public interface ISaleRepository
{
    Task CreateAsync(Sale sale, CancellationToken cancellationToken = default);
    Task<List<Sale>> GetAllAsync(CancellationToken cancellationToken = default);
}

public interface IUserRepository
{
    Task CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
