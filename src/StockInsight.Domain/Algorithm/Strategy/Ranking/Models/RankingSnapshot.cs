namespace StockInsight.Domain.Algorithm.Strategy.Ranking.Models;

public sealed record RankingSnapshot(DateOnly Date, IReadOnlyList<RankedCandidate> Ranking);
