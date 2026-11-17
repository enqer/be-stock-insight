using StockInsight.Domain.Algorithm.Decisions;

namespace StockInsight.Domain.Algorithm.Strategy.Ranking.Models;

public sealed record RankingCandidate(string Ticker, QualityComponents Components);
