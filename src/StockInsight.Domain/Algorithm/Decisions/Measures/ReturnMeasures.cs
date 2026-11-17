using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Returns;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Measures;

public sealed record ReturnMeasures(
    double? Roic,
    double? Roiic,
    double? NormalisedRoic,
    int CycleYears)
{
    public static ReturnMeasures At(IReadOnlyList<Quarter> quarters, PeriodMeasures period, double? averageCapital)
    {
        var cycleYears = CycleLength.Years(quarters, period.PeriodIndex);
        return new ReturnMeasures(
            period.Roic,
            period.Roiic,
            StockInsight.Domain.Algorithm.Calculations.Metrics.Returns.NormalisedRoic.At(quarters, period.PeriodIndex, cycleYears, averageCapital).OkValue,
            cycleYears);
    }
}
