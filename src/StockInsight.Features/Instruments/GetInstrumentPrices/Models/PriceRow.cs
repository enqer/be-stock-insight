namespace StockInsight.Features.Instruments.GetInstrumentPrices.Models;

public sealed class PriceRow
{
    public const string Sql = """
        SELECT prices.date, prices.close, prices.volume
        FROM daily_prices prices
        INNER JOIN instruments instrument ON prices.instrument_id = instrument.id
        WHERE instrument.ticker = @Ticker
        ORDER BY prices.date
        """;

    public DateOnly Date { get; init; }
    public double Close { get; init; }
    public double? Volume { get; init; }
}
