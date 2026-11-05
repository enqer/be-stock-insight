namespace StockInsight.Domain.Aggregates.Common;

public class AuditableEntity
{
    public int CreatedById { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int UpdatedById { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public int? DeletedById { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
