using StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Steps;

public static class ContinuityCutStep
{
    public static IReadOnlyList<DecisionPoint> Run(IReadOnlyList<DecisionPoint> series, SeriesQualityResult quality)
    {
        if (quality.ContinuousFromPeriod is not { } from)
        {
            return series;
        }

        var cutIndex = series.Select(x => x.PeriodLabel).ToList().LastIndexOf(from);
        return cutIndex >= 0 ? [.. series.Skip(cutIndex)] : series;
    }
}
