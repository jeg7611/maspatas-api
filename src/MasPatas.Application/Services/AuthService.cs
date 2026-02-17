using AutoMapper;
using MasPatas.Application.DTOs;
using MasPatas.Application.Interfaces;
using MasPatas.Domain.Entities;

namespace MasPatas.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService, IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var user = new User
        {
            Username = request.Username,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = request.Role
        };

        await _userRepository.CreateAsync(user, cancellationToken);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var token = _tokenService.GenerateToken(user);
        return new AuthResponse(token, user.Username, user.Role);
    }
}
