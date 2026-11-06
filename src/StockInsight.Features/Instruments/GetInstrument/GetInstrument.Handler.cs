using StockInsight.Domain.ValueObjects;
using StockInsight.Features.Common.Handlers;
using StockInsight.Features.Instruments.GetInstrument.Models;
using StockInsight.Infrastructure.Persistence;

namespace StockInsight.Features.Instruments.GetInstrument;

public static partial class GetInstrument
{
    internal sealed class Handler(ISqlRepository sql) : IHandler
    {
        public async Task<Response?> HandleAsync(string ticker, CancellationToken cancellationToken)
        {
            var rows = await sql.QueryAsync<InstrumentRow>(InstrumentRow.Sql, new { Ticker = new Ticker(ticker).Symbol }, cancellationToken);
            return rows.SingleOrDefault() is { } x
                ? new Response(x.Id, x.Ticker, x.Name, x.Isin, x.Kind, x.FinancialProfile, x.Date, x.Close, x.PreviousClose, x.Volume)
                : null;
        }
    }
}
