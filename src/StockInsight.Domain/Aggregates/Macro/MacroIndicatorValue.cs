namespace StockInsight.Domain.Aggregates.Macro;

public sealed class MacroIndicatorValue
{
    private MacroIndicatorValue()
    {
    }

    public Guid IndicatorId { get; private init; }

    public DateOnly Period { get; private init; }

    public DateOnly FetchedAt { get; private init; }

    public double Value { get; private init; }

    public static MacroIndicatorValue Create(Guid indicatorId, DateOnly period, DateOnly fetchedAt, double value)
    {
        if (!double.IsFinite(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value, "indicator value must be a finite number");
        }

        return new MacroIndicatorValue { IndicatorId = indicatorId, Period = period, FetchedAt = fetchedAt, Value = value };
    }
}
