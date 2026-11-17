using StockInsight.Domain.Algorithm.Decisions;
using StockInsight.Domain.Algorithm.Decisions.Gate;
using StockInsight.Domain.Algorithm.Strategy.Models;
using StockInsight.Domain.Algorithm.Strategy.Ranking;
using StockInsight.Domain.Algorithm.Strategy.Ranking.Models;

namespace StockInsight.Domain.Algorithm.Strategy.Steps;

public static class RankingStep
{
    public static IReadOnlyList<RankedCandidate>? Run(MarketSnapshot snapshot, IStrategyBook book, StrategyRules rules)
    {
        var candidates = snapshot.Companies
            .Where(x => !book.Holds(x.Company.Ticker)
                        && x.HasEntryPrice
                        && EntryGate.Evaluate(x.LatestDecision).Passed)
            .Select(x => QualityComponents.From(x.LatestDecision) is { } components ? new RankingCandidate(x.Company.Ticker, components) : null)
            .OfType<RankingCandidate>()
            .ToList();

        return candidates.Count >= rules.MinCandidates ? QualityRanking.Rank(candidates) : null;
    }
}
