using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital;
using StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.Enums;
using StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Measures;

public sealed record CostOfCapitalMeasures(
    double? BetaRegression,
    double BetaUnlevered,
    double? WindowLeverage,
    WindowLeverageSource WindowLeverageSource,
    double RiskFreeRate,
    WaccValue? Wacc)
{
    public static CostOfCapitalMeasures At(
        IReadOnlyList<Quarter> quarters, int periodIndex, DateOnly decisionDate, PriceSeries companyPrices, PriceSeries indexPrices, RiskFreeRate riskFreeRate)
    {
        double? betaRegression = null;
        WindowLeverageValue? windowLeverage = null;
        var betaUnlevered = Beta.Fallback;
        if (!indexPrices.IsEmpty)
        {
            betaRegression = Beta.Regression(companyPrices, indexPrices, decisionDate);
            windowLeverage = StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.WindowLeverage.At(quarters, periodIndex);
            if (betaRegression is { } levered)
            {
                betaUnlevered = Beta.Unlever(levered, windowLeverage.Value.DebtToEquity);
            }
        }

        var riskFree = riskFreeRate.On(decisionDate);
        return new CostOfCapitalMeasures(
            betaRegression,
            betaUnlevered,
            windowLeverage?.DebtToEquity,
            windowLeverage?.Source ?? WindowLeverageSource.None,
            riskFree,
            StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.Wacc.At(quarters, periodIndex, riskFree, betaUnlevered));
    }
}
