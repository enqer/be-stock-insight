using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Strategy.Models;
using StockInsight.Domain.Algorithm.Strategy.Steps;

namespace StockInsight.Domain.Algorithm.Strategy;

public static class StrategyDay
{
    public static StrategyDayResult Run(
        IReadOnlyList<AcceptedCompany> universe,
        IReadOnlyDictionary<string, PriceSeries> prices,
        IStrategyBook book,
        StrategyRules rules,
        DateOnly decisionDate,
        DateOnly? executionSession = null)
    {
        var pricing = new DatePricing(decisionDate, prices, executionSession);
        var snapshot = SnapshotStep.Run(universe, decisionDate, rules, pricing);

        var tradeDate = executionSession ?? decisionDate;
        ExitStep.Run(book, snapshot, pricing, tradeDate);
        RebalanceStep.Run(book, pricing, rules, tradeDate);

        var freeSlots = rules.TargetPositions - book.PositionCount;
        var ranked = freeSlots > 0 ? RankingStep.Run(snapshot, book, rules) : null;
        if (ranked is not null)
        {
            EntryStep.Run(book, ranked, freeSlots, pricing, rules, tradeDate);
        }

        return new StrategyDayResult(snapshot, pricing, ranked);
    }
}
