using StockInsight.Domain.Aggregates.Companies.Enums;

namespace StockInsight.Features.Companies.GetCompanies.Models;

public sealed class CompanyRow
{
    public const string Sql = """
        SELECT
            company.id,
            instrument.name,
            instrument.ticker,
            company.financial_profile,
            daily_price.close,
            daily_price.volume
        FROM instruments instrument
        INNER JOIN companies company ON instrument.company_id = company.id
        LEFT JOIN LATERAL (
            SELECT prices.close, prices.volume
            FROM daily_prices prices
            WHERE prices.instrument_id = instrument.id
            ORDER BY prices.date DESC
            LIMIT 1
        ) daily_price ON true
        WHERE instrument.kind = @InstrumentKind
        ORDER BY instrument.ticker
        """;

    public Guid Id { get; init; }
    public string Ticker { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public FinancialProfile FinancialProfile { get; init; }
    public double? Close { get; init; }
    public double? Volume { get; init; }
}
