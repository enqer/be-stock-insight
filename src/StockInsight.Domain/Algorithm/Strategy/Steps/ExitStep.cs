using StockInsight.Domain.Algorithm.Decisions.Gate;
using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;
using StockInsight.Domain.Algorithm.Strategy.Models;

namespace StockInsight.Domain.Algorithm.Strategy.Steps;

public static class ExitStep
{
    public static void Run(IStrategyBook book, MarketSnapshot snapshot, DatePricing pricing, DateOnly tradeDate)
    {
        foreach (var ticker in book.HeldTickers)
        {
            if (ExitReason(snapshot, ticker) is { } reason && pricing.TradePrice(ticker) is { } price)
            {
                book.Sell(tradeDate, ticker, price, reason);
            }
        }
    }

    private static GateReason? ExitReason(MarketSnapshot snapshot, string ticker) =>
        !snapshot.TryGet(ticker, out var state) ? GateReason.OutsideUniverse
        : EntryGate.Evaluate(state.LatestDecision) is { Passed: false } verdict ? verdict.Reason
        : null;
}
