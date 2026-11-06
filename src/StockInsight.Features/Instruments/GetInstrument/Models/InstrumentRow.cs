using StockInsight.Domain.Aggregates.Companies.Enums;
using StockInsight.Domain.Aggregates.Instruments.Enums;

namespace StockInsight.Features.Instruments.GetInstrument.Models;

public sealed class InstrumentRow
{
    public const string Sql = """
        SELECT
            instrument.id,
            instrument.ticker,
            instrument.name,
            instrument.isin,
            instrument.kind,
            company.financial_profile,
            daily_price.date,
            daily_price.close,
            daily_price.previous_close,
            daily_price.volume
        FROM instruments instrument
        LEFT JOIN companies company ON instrument.company_id = company.id
        LEFT JOIN LATERAL (
            SELECT
                prices.date,
                prices.close,
                prices.volume,
                LEAD(prices.close) OVER (ORDER BY prices.date DESC) AS previous_close
            FROM daily_prices prices
            WHERE prices.instrument_id = instrument.id
            ORDER BY prices.date DESC
            LIMIT 1
        ) daily_price ON true
        WHERE instrument.ticker = @Ticker
        """;

    public Guid Id { get; init; }
    public string Ticker { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Isin { get; init; }
    public InstrumentKind Kind { get; init; }
    public FinancialProfile? FinancialProfile { get; init; }
    public DateOnly? Date { get; init; }
    public double? Close { get; init; }
    public double? PreviousClose { get; init; }
    public double? Volume { get; init; }
}
