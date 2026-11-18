using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital;
using StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.Enums;
using StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.Models;
using WaccMetric = StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.Wacc;
using WindowLeverageMetric = StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.WindowLeverage;

namespace StockInsight.Domain.Algorithm.Decisions.Measures;

public sealed record CostOfCapitalMeasures(
    double? BetaRegression,
    double BetaUnlevered,
    double WindowLeverage,
    WindowLeverageSource WindowLeverageSource,
    double RiskFreeRate,
    WaccValue? Wacc)
{
    public static CostOfCapitalMeasures At(
        IReadOnlyList<Quarter> quarters, int periodIndex, DateOnly decisionDate, PriceSeries companyPrices, PriceSeries indexPrices, RiskFreeRate riskFreeRate)
    {
        var betaRegression = Beta.Regression(companyPrices, indexPrices, decisionDate);
        var windowLeverage = WindowLeverageMetric.At(quarters, periodIndex);
        var betaUnlevered = betaRegression is { } levered ? Beta.Unlever(levered, windowLeverage.DebtToEquity) : Beta.Fallback;

        var riskFree = riskFreeRate.On(decisionDate);
        return new CostOfCapitalMeasures(
            betaRegression,
            betaUnlevered,
            windowLeverage.DebtToEquity,
            windowLeverage.Source,
            riskFree,
            WaccMetric.At(quarters, periodIndex, riskFree, betaUnlevered));
    }
}
