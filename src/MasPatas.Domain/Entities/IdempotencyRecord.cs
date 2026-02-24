namespace MasPatas.Domain.Entities;

public class IdempotencyRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string RequestId { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public string ResourceId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
