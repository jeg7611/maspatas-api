namespace MasPatas.Application.DTOs;

public record InventoryDto(Guid Id, Guid ProductId, int Stock, int MinimumStock);
public record AddInventoryMovementRequest(Guid ProductId, string Type, int Quantity, string Reason);
public record InventoryMovementDto(Guid Id, Guid ProductId, string Type, int Quantity, string Reason, DateTime CreatedAt);
