namespace StockInsight.Domain.Aggregates.DailyPrices;

public sealed class DailyPrice
{
    private DailyPrice()
    {
    }

    public Guid InstrumentId { get; private init; }

    public DateOnly Date { get; private init; }

    public double Close { get; private init; }

    public double? Volume { get; private init; }

    public static DailyPrice Create(Guid instrumentId, DateOnly date, double close, double? volume)
    {
        if (!double.IsFinite(close) || close <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(close), close, "close must be a positive finite number");
        }

        if (volume is { } v && (!double.IsFinite(v) || v < 0))
        {
            throw new ArgumentOutOfRangeException(nameof(volume), volume, "volume must be a non-negative finite number");
        }

        return new DailyPrice { InstrumentId = instrumentId, Date = date, Close = close, Volume = volume };
    }
}
