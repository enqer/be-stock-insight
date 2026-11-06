using StockInsight.Domain.ValueObjects;
using StockInsight.Features.Common.Handlers;
using StockInsight.Features.Instruments.GetInstrumentPrices.Models;
using StockInsight.Infrastructure.Persistence;

namespace StockInsight.Features.Instruments.GetInstrumentPrices;

public static partial class GetInstrumentPrices
{
    internal sealed class Handler(ISqlRepository sql) : IHandler
    {
        public async Task<IReadOnlyList<Response>> HandleAsync(string ticker, CancellationToken cancellationToken)
        {
            var rows = await sql.QueryAsync<PriceRow>(PriceRow.Sql, new { Ticker = new Ticker(ticker).Symbol }, cancellationToken);
            return rows.Select(x => new Response(x.Date, x.Close, x.Volume)).ToList();
        }
    }
}
