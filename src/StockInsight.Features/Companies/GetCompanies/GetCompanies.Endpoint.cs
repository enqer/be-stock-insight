using Microsoft.AspNetCore.Http.HttpResults;
using StockInsight.Domain.Statements;
using StockInsight.Features.Common.Endpoints;

namespace StockInsight.Features.Companies.GetCompanies;

public static partial class GetCompanies
{
    public sealed record Response(Guid Id, string Ticker, string Name, FinancialProfile FinancialProfile, double Close, double Volume);

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
