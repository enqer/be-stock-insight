using StockInsight.Domain.Aggregates.Instruments.Enums;
using StockInsight.Features.Common.Handlers;
using StockInsight.Features.Companies.GetCompanies.Models;
using StockInsight.Infrastructure.Persistence;

namespace StockInsight.Features.Companies.GetCompanies;

public static partial class GetCompanies
{
    internal sealed class Handler(ISqlRepository sql) : IHandler
    {
        public async Task<IReadOnlyList<Response>> HandleAsync(CancellationToken cancellationToken)
        {
            var rows = await sql.QueryAsync<CompanyRow>(CompanyRow.Sql, new { InstrumentKind = InstrumentKind.Stock }, cancellationToken);
            return rows.Select(x => new Response(x.Id, x.Ticker, x.Name, x.FinancialProfile, x.Close, x.Volume)).ToList();
        }
    }
}
