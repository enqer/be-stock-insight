using StockInsight.Domain.Algorithm.Strategy.Ranking.Models;

namespace StockInsight.Domain.Algorithm.Backtest.Models;

public sealed record RankingSnapshot(DateOnly Date, IReadOnlyList<RankedCandidate> Ranking);
