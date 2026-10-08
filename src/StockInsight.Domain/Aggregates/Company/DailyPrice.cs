namespace StockInsight.Domain.Aggregates.Company;

public class DailyPrice
{
    public long Id { get; private init; }
    public Guid CompanyId { get; private init; }
    public DateOnly Date { get; private init; }
    public decimal Open { get; private init; }
    public decimal High { get; private init; }
    public decimal Low { get; private init; }
    public decimal Close { get; private init; }
    public long Volume { get; private init; }
    public decimal? AdjustedClose { get; private init; }

    private DailyPrice() { }

    public static DailyPrice Create()
    {
        return new DailyPrice();
    }
}
