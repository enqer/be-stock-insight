using StockInsight.Domain.Algorithm.Strategy.Models;
using StockInsight.Domain.Algorithm.Strategy.Ranking.Models;

namespace StockInsight.Domain.Algorithm.Strategy.Steps;

public static class EntryStep
{
    public static void Run(IStrategyBook book, IReadOnlyList<RankedCandidate> ranked, int freeSlots, DatePricing pricing, StrategyRules rules, DateOnly tradeDate)
    {
        var targetPositionValue = book.Value(pricing) / rules.TargetPositions;
        foreach (var chosen in ranked.Take(freeSlots))
        {
            if (book.Cash < targetPositionValue * rules.MinCashShareOfPosition)
            {
                break;
            }

            if (pricing.TradePrice(chosen.Ticker) is not { } price)
            {
                continue;
            }

            book.Buy(tradeDate, chosen.Ticker, Math.Min(targetPositionValue, book.Cash), price);
        }
    }
}
