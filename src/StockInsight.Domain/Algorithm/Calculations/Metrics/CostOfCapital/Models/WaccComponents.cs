namespace StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.Models;

public sealed record WaccComponents(
    double CostOfEquity,
    double CostOfDebt,
    double LeveredBeta,
    double DebtToEquity,
    double DebtWeight);
