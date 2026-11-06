using Microsoft.AspNetCore.Http.HttpResults;
using StockInsight.Features.Common.Endpoints;

namespace StockInsight.Features.Instruments.GetInstrumentPrices;

public static partial class GetInstrumentPrices
{
    public sealed record Response(DateOnly Date, double Close, double? Volume);

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app) =>
            app.MapGet("/instruments/{ticker}/prices", Invoke)
                .WithName(nameof(GetInstrumentPrices))
                .WithTags(nameof(Instruments));

        private static async Task<Ok<IReadOnlyList<Response>>> Invoke(string ticker, Handler handler, CancellationToken cancellationToken) =>
            TypedResults.Ok(await handler.HandleAsync(ticker, cancellationToken));
    }
}
