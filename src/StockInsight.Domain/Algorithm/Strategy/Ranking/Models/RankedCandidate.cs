namespace StockInsight.Domain.Algorithm.Strategy.Ranking.Models;

public sealed record RankedCandidate(
    string Ticker,
    double Score,
    double HeadroomPercentile,
    double RoiicPercentile,
    double CashConversionPercentile);
