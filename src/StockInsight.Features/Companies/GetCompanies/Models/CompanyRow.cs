using StockInsight.Domain.Statements;

namespace StockInsight.Features.Companies.GetCompanies.Models;

public sealed class CompanyRow
{
    public const string Sql = $"""
            SELECT
                company.id AS [{nameof(CompanyRow.Id)}],
                instrument.name AS [{nameof(CompanyRow.Name)}],
                instrument.ticker AS [{nameof(CompanyRow.Ticker)}],
                company.financial_profile AS [{nameof(CompanyRow.FinancialProfile)}],
                daily_price.close AS [{nameof(CompanyRow.Close)}],
                daily_price.volume AS [{nameof(CompanyRow.Volume)}]
            FROM instruments instrument
            INNER JOIN companies company ON instrument.company_id = company.id
            CROSS JOIN LATERAL (
                SELECT prices.close, prices.volume
                FROM daily_prices prices
                WHERE prices.instrument_id = instrument.id
                ORDER BY prices.date DESC
                LIMIT 1
            ) daily_price
            WHERE instrument.kind = @InstrumentKind
        """;

    public Guid Id { get; init; }
    public string Ticker { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public FinancialProfile FinancialProfile { get; init; }
    public double Close { get; init; }
    public double Volume { get; init; }
}
