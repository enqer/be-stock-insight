using StockInsight.Domain.Aggregates.Instruments.Enums;
using StockInsight.Features.Common.Handlers;
using StockInsight.Features.Companies.GetCompanies.Models;
using StockInsight.Infrastructure.Persistence;
using StockInsight.Infrastructure.Time;

namespace StockInsight.Features.Companies.GetCompanies;

public static partial class GetCompanies
{
    internal sealed class Handler(ISqlRepository sql, TimeProvider time) : IHandler
    {
        public async Task<IReadOnlyList<Response>> HandleAsync(CancellationToken cancellationToken)
        {
            var yearAgo = MarketTime.Today(time).AddYears(-1).ToDateTime(TimeOnly.MinValue);
            var parameters = new { InstrumentKind = InstrumentKind.Stock, YearAgo = yearAgo };
            var rows = await sql.QueryAsync<CompanyRow>(CompanyRow.Sql, parameters, cancellationToken);
            return rows.Select(x => new Response(x.Id, x.Ticker, x.Name, x.MacroSector, x.Sector, x.Close, x.Volume, x.RecentCloses ?? [])).ToList();
        }
    }
}
