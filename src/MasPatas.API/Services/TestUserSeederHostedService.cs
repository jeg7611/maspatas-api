using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;

namespace MasPatas.API.Services;

public class TestUserSeederHostedService : IHostedService
{
    private const string TestUsername = "test.admin";
    private const string TestPassword = "Test12345!";

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TestUserSeederHostedService> _logger;

    public TestUserSeederHostedService(IServiceProvider serviceProvider, ILogger<TestUserSeederHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        var existingUser = await userRepository.GetByUsernameAsync(TestUsername, cancellationToken);
        if (existingUser is not null)
        {
            _logger.LogInformation("Usuario de prueba '{Username}' ya existe.", TestUsername);
            return;
        }

        var user = new User
        {
            Username = TestUsername,
            Email = "test.admin@maspatas.local",
            Role = "Admin",
            PasswordHash = passwordHasher.Hash(TestPassword)
        };

        await userRepository.CreateAsync(user, cancellationToken);
        _logger.LogInformation("Usuario de prueba creado: {Username} / {Password}", TestUsername, TestPassword);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
