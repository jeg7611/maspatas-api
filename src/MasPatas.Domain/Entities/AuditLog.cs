namespace MasPatas.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string RequestId { get; set; } = string.Empty;
    public string? Changes { get; set; }
}
