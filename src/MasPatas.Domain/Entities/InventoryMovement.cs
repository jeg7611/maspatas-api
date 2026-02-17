namespace MasPatas.Domain.Entities;

public class InventoryMovement
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public string Type { get; set; } = string.Empty; // IN / OUT
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
