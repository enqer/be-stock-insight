using StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Steps;

public static class QuarantineStep
{
    public static IReadOnlyList<DecisionPoint> Run(IReadOnlyList<DecisionPoint> series, SeriesQualityResult quality) =>
        [.. series.Where(x => !quality.QuarantinedPeriods.Contains(x.PeriodLabel))];
}
