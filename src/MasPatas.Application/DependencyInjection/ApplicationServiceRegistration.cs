using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MasPatas.Application.Mappings;
using MasPatas.Application.Services;

namespace MasPatas.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

        services.AddScoped<ProductService>();
        services.AddScoped<CustomerService>();
        services.AddScoped<InventoryService>();
        services.AddScoped<SaleService>();
        services.AddScoped<AuthService>();

        return services;
    }
}
