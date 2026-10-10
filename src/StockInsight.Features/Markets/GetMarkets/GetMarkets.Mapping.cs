using StockInsight.Domain.Enums;

namespace StockInsight.Features.Markets;

public static partial class GetMarkets
{
    internal static class Mapping
    {
        public static Response ToResponse(Market market) => new((int)market, market.ToString());
    }
}
