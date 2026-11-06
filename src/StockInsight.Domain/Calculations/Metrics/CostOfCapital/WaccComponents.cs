namespace StockInsight.Domain.Calculations.Metrics.CostOfCapital;

public sealed record WaccComponents(
    double CostOfEquity,
    double CostOfDebt,
    double LeveredBeta,
    double DebtToEquity,
    double DebtWeight);
