using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Capital.Models;
using DebtMetric = StockInsight.Domain.Algorithm.Calculations.Metrics.Capital.Debt;
using InvestedCapitalMetric = StockInsight.Domain.Algorithm.Calculations.Metrics.Capital.InvestedCapital;

namespace StockInsight.Domain.Algorithm.Decisions.Measures;

public sealed record CapitalMeasures(
    double? InvestedCapital,
    double? InvestedCapitalYearAgo,
    double? AverageInvestedCapital,
    DebtValue Debt,
    double? Cash)
{
    public static CapitalMeasures At(IReadOnlyList<Quarter> quarters, int periodIndex, bool minoritiesInsideEquity)
    {
        var (capitalNow, capitalYearAgo, averageCapital) = InvestedCapitalMetric.WithAverage(quarters, periodIndex, minoritiesInsideEquity);
        var debt = DebtMetric.At(quarters, periodIndex);
        var cash = quarters[periodIndex].Cash;
        return new CapitalMeasures(capitalNow.Value, capitalYearAgo, averageCapital, debt, cash);
    }
}
