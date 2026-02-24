namespace MasPatas.Domain.Entities;

public enum SaleStatus
{
    PendingPayment = 1,
    Paid = 2,
    Cancelled = 3,
    Refunded = 4
}

public class Sale
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? CustomerId { get; set; }
    public List<SaleItem> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public SaleStatus Status { get; set; } = SaleStatus.PendingPayment;
    public List<Payment> Payments { get; set; } = new();
    public int Version { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class SaleItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class Payment
{
    public Guid PaymentId { get; set; } = Guid.NewGuid();
    public Guid SaleId { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    public string RequestId { get; set; } = string.Empty;
}
