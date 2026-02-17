namespace MasPatas.Application.DTOs;

public record CreateProductRequest(string Name, string Description, decimal Price, bool Active);
public record ProductDto(Guid Id, string Name, string Description, decimal Price, bool Active);
