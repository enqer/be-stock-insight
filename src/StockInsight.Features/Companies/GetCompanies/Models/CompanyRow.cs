namespace StockInsight.Features.Companies.GetCompanies.Models;

public sealed class CompanyRow
{
    public const string Sql = """
        SELECT
            company.id,
            instrument.name,
            instrument.ticker,
            sector.macro_sector,
            sector.name AS sector,
            latest_price.close,
            latest_price.volume,
            recent.recent_closes
        FROM instruments instrument
        INNER JOIN companies company ON instrument.company_id = company.id
        LEFT JOIN sectors sector ON company.sector_id = sector.id
        LEFT JOIN LATERAL (
            SELECT prices.close, prices.volume
            FROM daily_prices prices
            WHERE prices.instrument_id = instrument.id
            ORDER BY prices.date DESC
            LIMIT 1
        ) latest_price ON true
        LEFT JOIN LATERAL (
            SELECT array_agg(prices.close ORDER BY prices.date) AS recent_closes
            FROM daily_prices prices
            WHERE prices.instrument_id = instrument.id
                AND prices.date > @YearAgo
        ) recent ON true
        WHERE instrument.kind = @InstrumentKind
        ORDER BY instrument.ticker
        """;

    public Guid Id { get; init; }
    public string Ticker { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? MacroSector { get; init; }
    public string? Sector { get; init; }
    public double? Close { get; init; }
    public double? Volume { get; init; }
    public IReadOnlyList<double>? RecentCloses { get; init; }
}
