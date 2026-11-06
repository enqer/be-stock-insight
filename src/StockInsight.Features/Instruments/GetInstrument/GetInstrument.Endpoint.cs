using Microsoft.AspNetCore.Http.HttpResults;
using StockInsight.Domain.Aggregates.Companies.Enums;
using StockInsight.Domain.Aggregates.Instruments.Enums;
using StockInsight.Features.Common.Endpoints;

namespace StockInsight.Features.Instruments.GetInstrument;

public static partial class GetInstrument
{
    public sealed record Response(
        Guid Id,
        string Ticker,
        string Name,
        string? Isin,
        InstrumentKind Kind,
        FinancialProfile? FinancialProfile,
        DateOnly? Date,
        double? Close,
        double? PreviousClose,
        double? Volume);

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app) =>
            app.MapGet("/instruments/{ticker}", Invoke)
                .WithName(nameof(GetInstrument))
                .WithTags(nameof(Instruments));

        private static async Task<Results<Ok<Response>, NotFound>> Invoke(string ticker, Handler handler, CancellationToken cancellationToken) =>
            await handler.HandleAsync(ticker, cancellationToken) is { } instrument ? TypedResults.Ok(instrument) : TypedResults.NotFound();
    }
}
