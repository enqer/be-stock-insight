namespace StockInsight.Domain.Abstract;

public class Entity : IEntity
{
    public int CreatedById { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int UpdatedById { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public int? DeletedById { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
