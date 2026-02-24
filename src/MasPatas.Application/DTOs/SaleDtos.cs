using MasPatas.Domain.Entities;

namespace MasPatas.Application.DTOs;

public record CreateSaleItemRequest(Guid ProductId, int Quantity, decimal Price);
public record CreateSaleCommand(Guid? CustomerId, List<CreateSaleItemRequest> Items, string RequestId, Guid UserId);
public record RegisterPaymentCommand(Guid SaleId, string PaymentMethod, decimal Amount, string RequestId, Guid UserId);
public record CancelSaleCommand(Guid SaleId, string RequestId, Guid UserId);

public record SaleItemDto(Guid ProductId, int Quantity, decimal UnitPrice);
public record PaymentDto(Guid PaymentId, Guid SaleId, string PaymentMethod, decimal Amount, DateTime PaidAt, string RequestId);

public record SaleDto(
    Guid Id,
    Guid? CustomerId,
    List<SaleItemDto> Items,
    decimal TotalAmount,
    SaleStatus Status,
    List<PaymentDto> Payments,
    int Version,
    DateTime CreatedAt,
    DateTime UpdatedAt);
