namespace MasPatas.Application.DTOs;

public record CreateCustomerRequest(string Name, string Phone, string Email);
public record CustomerDto(Guid Id, string Name, string Phone, string Email, DateTime CreatedAt);
