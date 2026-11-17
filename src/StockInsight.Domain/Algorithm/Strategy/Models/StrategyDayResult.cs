using StockInsight.Domain.Algorithm.Strategy.Ranking.Models;

namespace StockInsight.Domain.Algorithm.Strategy.Models;

public sealed record StrategyDayResult(MarketSnapshot Snapshot, DatePricing Pricing, IReadOnlyList<RankedCandidate>? Ranked);
