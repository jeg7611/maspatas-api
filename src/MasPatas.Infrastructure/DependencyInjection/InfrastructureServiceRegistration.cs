using MasPatas.Application.Interfaces;
using MasPatas.Infrastructure.Configuration;
using MasPatas.Infrastructure.Persistence;
using MasPatas.Infrastructure.Repositories;
using MasPatas.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace MasPatas.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddSingleton<MongoDbContext>();
        services.AddScoped<IMongoDatabase>(sp =>
                                            {
                                                var context = sp.GetRequiredService<MongoDbContext>();
                                                return context.Database;
                                            });

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IInventoryMovementRepository, InventoryMovementRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
