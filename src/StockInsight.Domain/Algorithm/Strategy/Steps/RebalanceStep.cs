using StockInsight.Domain.Algorithm.Strategy.Models;

namespace StockInsight.Domain.Algorithm.Strategy.Steps;

public static class RebalanceStep
{
    public static void Run(IStrategyBook book, DatePricing pricing, StrategyRules rules, DateOnly tradeDate)
    {
        if (book.PositionCount == 0)
        {
            return;
        }

        var targetValue = book.Value(pricing) / rules.TargetPositions;
        foreach (var ticker in book.HeldTickers)
        {
            if (pricing.TradePrice(ticker) is not { } price)
            {
                continue;
            }

            var heldValue = book.Units(ticker) * price;
            if (heldValue > targetValue * rules.RebalanceUpperBand)
            {
                book.TrimTo(tradeDate, ticker, targetValue, price);
            }
            else if (heldValue < targetValue * rules.RebalanceLowerBand && book.Cash > 0)
            {
                book.TopUpTo(tradeDate, ticker, targetValue, price);
            }
        }
    }
}
