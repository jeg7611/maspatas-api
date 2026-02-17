namespace MasPatas.Domain.Entities;

public class Inventory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public int Stock { get; set; }
    public int MinimumStock { get; set; }
}
