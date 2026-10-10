using Microsoft.AspNetCore.Http.HttpResults;
using StockInsight.Features.Common.Endpoints;

namespace StockInsight.Features.Markets;

public static partial class GetMarkets
{
    public sealed record Response(int Id, string Name);

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app) =>
            app.MapGet("/markets", Invoke)
                .WithName(nameof(GetMarkets))
                .WithTags(nameof(Markets));

        private static Ok<IReadOnlyList<Response>> Invoke(Handler handler) =>
            TypedResults.Ok(handler.Handle());
    }
}
