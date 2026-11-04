namespace StockInsight.Domain.Abstract;

public interface IEntity
{
    int CreatedById { get; set; }
    DateTimeOffset CreatedAt { get; set; }
    int UpdatedById { get; set; }
    DateTimeOffset UpdatedAt { get; set; }
    int? DeletedById { get; set; }
    DateTimeOffset? DeletedAt { get; set; }
}
