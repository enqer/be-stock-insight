using StockInsight.Domain.Enums;
using StockInsight.Features.Common.Handlers;

namespace StockInsight.Features.Markets;

public static partial class GetMarkets
{
    internal sealed class Handler : IHandler
    {
        public IReadOnlyList<Response> Handle() =>
            [.. Enum.GetValues<Market>().Select(Mapping.ToResponse)];
    }
}
