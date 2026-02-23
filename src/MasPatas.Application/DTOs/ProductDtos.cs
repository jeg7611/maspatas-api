namespace MasPatas.Application.DTOs;

public record CreateProductRequest(string Name, string sku, string Description, decimal Price, bool Active);
public record ProductDto(Guid Id, string Name, string sku, string Description, decimal Price, bool Active);
