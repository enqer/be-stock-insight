using StockInsight.Domain.Enums;
using StockInsight.Features.Markets;

namespace StockInsight.Features.Tests.Markets;

public sealed class GetMarketsHandlerTests
{
    [Fact]
    public void Returns_every_market_with_its_id_and_name()
    {
        var markets = new GetMarkets.Handler().Handle();

        markets.Count.ShouldBe(Enum.GetValues<Market>().Length);
        markets.ShouldContain(new GetMarkets.Response((int)Market.GPW, nameof(Market.GPW)));
    }
}
