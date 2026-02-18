using MasPatas.Domain.Entities;
using MasPatas.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MasPatas.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoDatabase Database => _database;

    public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
    public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>("Customers");
    public IMongoCollection<Inventory> Inventory => _database.GetCollection<Inventory>("Inventory");
    public IMongoCollection<InventoryMovement> InventoryMovements => _database.GetCollection<InventoryMovement>("InventoryMovements");
    public IMongoCollection<Sale> Sales => _database.GetCollection<Sale>("Sales");
    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
}
