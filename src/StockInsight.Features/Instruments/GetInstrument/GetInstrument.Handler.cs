using StockInsight.Domain.ValueObjects;
using StockInsight.Features.Common.Handlers;
using StockInsight.Features.Instruments.GetInstrument.Models;
using StockInsight.Infrastructure.Persistence;

namespace StockInsight.Features.Instruments.GetInstrument;

public static partial class GetInstrument
{
    internal sealed class Handler(ISqlRepository sql) : IHandler
    {
        public async Task<Response?> HandleAsync(string ticker, CancellationToken cancellationToken) =>
            await sql.QuerySingleOrDefaultAsync<InstrumentRow>(InstrumentRow.Sql, new { Ticker = new Ticker(ticker).Symbol }, cancellationToken) is { } row
                ? new Response(row.Id, row.Ticker, row.Name, row.Isin, row.Kind, row.FinancialProfile, row.Date, row.Close, row.PreviousClose, row.Volume)
                : null;
    }
}
