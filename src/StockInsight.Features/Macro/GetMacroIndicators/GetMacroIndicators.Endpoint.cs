using Microsoft.AspNetCore.Http.HttpResults;
using StockInsight.Domain.Aggregates.Macro.Enums;
using StockInsight.Features.Common.Endpoints;

namespace StockInsight.Features.Macro.GetMacroIndicators;

public static partial class GetMacroIndicators
{
    public sealed record Response(
        Guid Id,
        string Country,
        string Code,
        string? Name,
        MacroIndicatorKind? Kind,
        MacroUnit? Unit,
        DateOnly? Period,
        double? Value,
        double? PreviousValue,
        IReadOnlyList<double> RecentValues);

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app) =>
            app.MapGet("/macro-indicators", Invoke)
                .WithName(nameof(GetMacroIndicators))
                .WithTags(nameof(Macro));

        private static async Task<Ok<IReadOnlyList<Response>>> Invoke(string country, Handler handler, CancellationToken cancellationToken) =>
            TypedResults.Ok(await handler.HandleAsync(country, cancellationToken));
    }
}
