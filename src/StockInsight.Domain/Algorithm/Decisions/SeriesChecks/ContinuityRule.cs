using System.Globalization;
using StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

namespace StockInsight.Domain.Algorithm.Decisions.SeriesChecks;

public static class ContinuityRule
{
    public const double MaxGapDays = 200;

    public static ContinuityCheck Apply(IReadOnlyList<ScheduledReport> reports, IReadOnlyList<double> gapDays)
    {
        var largeGapIndices = Enumerable.Range(0, gapDays.Count).Where(x => gapDays[x] > MaxGapDays).ToArray();
        if (largeGapIndices.Length == 0)
        {
            return new ContinuityCheck(true, null, null);
        }

        var firstKeptIndex = largeGapIndices[^1] + 1;
        var remainingPeriods = reports.Count - firstKeptIndex;
        return remainingPeriods >= SeriesQuality.MinPeriods
            ? new ContinuityCheck(true, reports[firstKeptIndex].PeriodLabel, string.Create(CultureInfo.InvariantCulture,
                $"luk: {largeGapIndices.Length}; szereg ucięty za ostatnią ({gapDays[largeGapIndices[^1]]:F0} dni), zostaje {remainingPeriods} okresów od {reports[firstKeptIndex].PeriodLabel}"))
            : new ContinuityCheck(false, null, string.Create(CultureInfo.InvariantCulture,
                $"luk: {largeGapIndices.Length}; po ucięciu za ostatnią ({reports[firstKeptIndex].PeriodLabel}) zostaje {remainingPeriods} okresów, wymagane {SeriesQuality.MinPeriods}"));
    }
}
