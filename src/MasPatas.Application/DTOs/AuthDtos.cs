namespace MasPatas.Application.DTOs;

public record CreateUserRequest(string Username, string Password, string Role);
public record LoginRequest(string Username, string Password);
public record AuthResponse(string Token, string Username, string Role);
public record UserDto(Guid Id, string Username, string Role);
