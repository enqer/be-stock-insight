using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Capital.Models;

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
        var (capitalNow, previousCapital, averageCapital) =
            StockInsight.Domain.Algorithm.Calculations.Metrics.Capital.InvestedCapital.WithAverage(quarters, periodIndex, minoritiesInsideEquity);
        return new CapitalMeasures(capitalNow.Value, previousCapital, averageCapital,
            StockInsight.Domain.Algorithm.Calculations.Metrics.Capital.Debt.At(quarters, periodIndex), quarters.At(periodIndex)?.Cash);
    }
}
