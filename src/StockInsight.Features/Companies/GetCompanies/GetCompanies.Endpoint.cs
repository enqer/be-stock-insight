using Microsoft.AspNetCore.Http.HttpResults;
using StockInsight.Features.Common.Endpoints;

namespace StockInsight.Features.Companies.GetCompanies;

public static partial class GetCompanies
{
    public sealed record Response(Guid Id, string Ticker, string Name, string? MacroSector, string? Sector, double? Close, double? Volume, IReadOnlyList<double> RecentCloses);

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app) =>
            app.MapGet("/companies", Invoke)
                .WithName(nameof(GetCompanies))
                .WithTags(nameof(Companies));

        private static async Task<Ok<IReadOnlyList<Response>>> Invoke(Handler handler, CancellationToken cancellationToken) =>
            TypedResults.Ok(await handler.HandleAsync(cancellationToken));
    }
}
