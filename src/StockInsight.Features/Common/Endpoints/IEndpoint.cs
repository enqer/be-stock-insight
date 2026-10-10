namespace StockInsight.Features.Common.Endpoints;

/// <summary>
/// HTTP entry point of a single vertical slice. Implementations are discovered and mapped automatically.
/// </summary>
public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
