using MasPatas.Domain.Entities;

namespace MasPatas.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}
