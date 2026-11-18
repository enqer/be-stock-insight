using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.Enums;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using CashConversionMetric = StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.CashConversion;
using DilutionMetric = StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.Dilution;

namespace StockInsight.Domain.Algorithm.Decisions.Measures;

public sealed record QualityMeasures(
    int? DirectionCriteriaMet,
    double? CashConversion,
    double? Shares,
    ShareBase ShareBase,
    double? Dilution)
{
    public static QualityMeasures At(IReadOnlyList<Quarter> quarters, bool minoritiesInsideEquity, PeriodMeasures now, PeriodMeasures fourDecisionsEarlier) =>
        new(
            now.DirectionCriteriaMet,
            CashConversionMetric.At(quarters, now.PeriodIndex, minoritiesInsideEquity).OkValue,
            now.Shares,
            now.ShareBase,
            DilutionMetric.YearOverYear(now.Shares, now.ShareBase, fourDecisionsEarlier.Shares, fourDecisionsEarlier.ShareBase));
}
