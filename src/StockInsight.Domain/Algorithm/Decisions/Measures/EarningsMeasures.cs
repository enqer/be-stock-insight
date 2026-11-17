using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Measures;

public sealed record EarningsMeasures(double? TrailingRevenue, double? TrailingEbit, double? TrailingNetIncome)
{
    public static EarningsMeasures At(IReadOnlyList<Quarter> quarters, PeriodMeasures period) =>
        new(
            TrailingTwelveMonths.Sum(quarters, period.PeriodIndex, x => x.Revenue),
            TrailingTwelveMonths.Sum(quarters, period.PeriodIndex, x => x.Ebit),
            period.TrailingNetIncome);
}
