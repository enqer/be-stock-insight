using StockInsight.Domain.Calculations.Inputs.Market;
using StockInsight.Domain.Calculations.Inputs.Statements;
using StockInsight.Domain.Calculations.Metrics.Capital;
using StockInsight.Domain.Calculations.Numerics;

namespace StockInsight.Domain.Calculations.Metrics.CostOfCapital;

public static class Wacc
{
    public const double MinPlausibleCostOfDebt = 0.005;
    public const double MaxPlausibleCostOfDebt = 0.35;
    public const double HypotheticalDebtSpread = 0.03;
    private const double DebtSignificanceShareOfRevenue = 0.01;

    public static WaccValue? At(IReadOnlyList<Quarter> quarters, int t, double riskFreeRate, double unleveredBeta)
    {
        var revenue = TrailingTwelveMonths.Sum(quarters, t, q => q.Revenue);
        var debt = Debt.At(quarters, t).Amount;
        var debtYearAgo = Debt.At(quarters, Math.Max(0, t - 4)).Amount;

        if (quarters.At(t)?.Equity is not { } equity || equity <= 0 || revenue is null)
        {
            return null;
        }

        var financeCost = TrailingTwelveMonths.Sum(quarters, t, q => q.FinancialExpenses);
        var measured = SignificantRatio.Of(
            Math.Abs(financeCost ?? 0.0), (debt + debtYearAgo) / 2, revenue, DebtSignificanceShareOfRevenue);
        var costOfDebt = measured is { } kd && kd > MinPlausibleCostOfDebt && kd < MaxPlausibleCostOfDebt
            ? kd
            : riskFreeRate + HypotheticalDebtSpread;

        var debtToEquity = debt / equity;
        var leveredBeta = Beta.Relever(unleveredBeta, debtToEquity);
        var costOfEquity = riskFreeRate + MarketConstants.EquityRiskPremium * leveredBeta;
        var debtWeight = debt / (debt + equity);

        var wacc = (1 - debtWeight) * costOfEquity + debtWeight * costOfDebt * (1 - MarketConstants.TaxRate);
        return new WaccValue(wacc, new WaccComponents(costOfEquity, costOfDebt, leveredBeta, debtToEquity, debtWeight));
    }
}
