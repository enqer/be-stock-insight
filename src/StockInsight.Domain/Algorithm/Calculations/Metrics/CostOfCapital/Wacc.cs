using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Capital;
using StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.Models;
using StockInsight.Domain.Algorithm.Calculations.Numerics;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital;

public static class Wacc
{
    public const double MinPlausibleCostOfDebt = 0.005;
    public const double MaxPlausibleCostOfDebt = 0.35;
    public const double HypotheticalDebtSpread = 0.03;
    private const double DebtSignificanceShareOfRevenue = 0.01;

    public static WaccValue? At(IReadOnlyList<Quarter> quarters, int periodIndex, double riskFreeRate, double unleveredBeta)
    {
        var revenue = TrailingTwelveMonths.Sum(quarters, periodIndex, x => x.Revenue);
        var debt = Debt.At(quarters, periodIndex).Amount;
        var debtYearAgo = Debt.At(quarters, Math.Max(0, periodIndex - 4)).Amount;

        if (quarters[periodIndex].Equity is not { } equity || equity <= 0 || revenue is null)
        {
            return null;
        }

        var financeCost = TrailingTwelveMonths.Sum(quarters, periodIndex, x => x.FinancialExpenses);
        var impliedCostOfDebt = SignificantRatio.Of(
            Math.Abs(financeCost ?? 0.0), (debt + debtYearAgo) / 2, revenue, DebtSignificanceShareOfRevenue);
        var costOfDebt = impliedCostOfDebt is { } statementCostOfDebt && statementCostOfDebt > MinPlausibleCostOfDebt && statementCostOfDebt < MaxPlausibleCostOfDebt
            ? statementCostOfDebt
            : riskFreeRate + HypotheticalDebtSpread;

        var debtToEquity = debt / equity;
        var leveredBeta = Beta.Relever(unleveredBeta, debtToEquity);
        var costOfEquity = riskFreeRate + MarketConstants.EquityRiskPremium * leveredBeta;
        var debtWeight = debt / (debt + equity);

        var wacc = (1 - debtWeight) * costOfEquity + debtWeight * costOfDebt * (1 - MarketConstants.TaxRate);
        return new WaccValue(wacc, new WaccComponents(costOfEquity, costOfDebt, leveredBeta, debtToEquity, debtWeight));
    }
}
