namespace StockInsight.Domain.Algorithm.Strategy.Models;

public sealed record StrategyRules(
    int TargetPositions,
    double CostBasisPoints,
    int MaxEntryPriceAgeDays,
    double RebalanceUpperBand,
    double RebalanceLowerBand,
    double MinCashShareOfPosition,
    int MinCandidates)
{
    public static StrategyRules Production { get; } = new(
        TargetPositions: 12,
        CostBasisPoints: 39,
        MaxEntryPriceAgeDays: 90,
        RebalanceUpperBand: 1.25,
        RebalanceLowerBand: 0.75,
        MinCashShareOfPosition: 0.5,
        MinCandidates: 3);
}
