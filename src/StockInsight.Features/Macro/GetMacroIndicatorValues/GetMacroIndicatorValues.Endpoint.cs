using Microsoft.AspNetCore.Http.HttpResults;
using StockInsight.Features.Common.Endpoints;

namespace StockInsight.Features.Macro.GetMacroIndicatorValues;

public static partial class GetMacroIndicatorValues
{
    public sealed record Response(DateOnly Period, double Value);

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app) =>
            app.MapGet("/macro-indicators/{id:guid}/values", Invoke)
                .WithName(nameof(GetMacroIndicatorValues))
                .WithTags(nameof(Macro));

        private static async Task<Ok<IReadOnlyList<Response>>> Invoke(Guid id, Handler handler, CancellationToken cancellationToken) =>
            TypedResults.Ok(await handler.HandleAsync(id, cancellationToken));
    }
}
