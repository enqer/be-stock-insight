using Microsoft.AspNetCore.Http.HttpResults;
using StockInsight.Features.Common.Endpoints;

namespace StockInsight.Features.Instruments.GetInstrumentStatements;

public static partial class GetInstrumentStatements
{
    public sealed record Response(DateOnly Period, DateOnly? PublishedAt, IReadOnlyDictionary<string, double> Items);

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app) =>
            app.MapGet("/instruments/{ticker}/statements", Invoke)
                .WithName(nameof(GetInstrumentStatements))
                .WithTags(nameof(Instruments));

        private static async Task<Ok<IReadOnlyList<Response>>> Invoke(string ticker, Handler handler, CancellationToken cancellationToken) =>
            TypedResults.Ok(await handler.HandleAsync(ticker, cancellationToken));
    }
}
