namespace MasPatas.Application.DTOs;

public record CreateSaleItemRequest(Guid ProductId, int Quantity, decimal UnitPrice);
public record RegisterSaleRequest(Guid? CustomerId, List<CreateSaleItemRequest> Items);
public record SaleItemDto(Guid ProductId, int Quantity, decimal UnitPrice);
public record SaleDto(Guid Id, Guid? CustomerId, List<SaleItemDto> Items, decimal TotalAmount, DateTime CreatedAt);
