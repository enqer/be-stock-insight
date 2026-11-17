using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.Enums;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

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
            StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.CashConversion.At(quarters, now.PeriodIndex, minoritiesInsideEquity).OkValue,
            now.Shares,
            now.ShareBase,
            StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.Dilution.YearOverYear(now.Shares, now.ShareBase, fourDecisionsEarlier.Shares, fourDecisionsEarlier.ShareBase));
}
