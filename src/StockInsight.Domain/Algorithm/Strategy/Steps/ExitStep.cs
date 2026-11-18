using StockInsight.Domain.Algorithm.Decisions.Gate;
using StockInsight.Domain.Algorithm.Strategy.Models;

namespace StockInsight.Domain.Algorithm.Strategy.Steps;

public static class ExitStep
{
    public static void Run(IStrategyBook book, MarketSnapshot snapshot, DatePricing pricing, DateOnly tradeDate)
    {
        foreach (var ticker in book.HeldTickers)
        {
            if (ShouldSell(snapshot, ticker) && pricing.TradePrice(ticker) is { } price)
            {
                book.Sell(tradeDate, ticker, price);
            }
        }
    }

    private static bool ShouldSell(MarketSnapshot snapshot, string ticker) =>
        !snapshot.TryGet(ticker, out var state) || !EntryGate.Evaluate(state.LatestDecision).Passed;
}
