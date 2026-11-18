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
            return new ContinuityCheck(Applicable: true, ContinuousFromPeriod: null, Reason: null);
        }

        var lastLargeGap = largeGapIndices[^1];
        var firstReportAfterLastGap = lastLargeGap + 1;
        var firstKeptPeriod = reports[firstReportAfterLastGap].PeriodLabel;
        var remainingPeriods = reports.Count - firstReportAfterLastGap;
        var enoughPeriodsRemain = remainingPeriods >= SeriesQuality.MinPeriods;
        if (!enoughPeriodsRemain)
        {
            return new ContinuityCheck(Applicable: false, ContinuousFromPeriod: null, Reason: string.Create(CultureInfo.InvariantCulture,
                $"luk: {largeGapIndices.Length}; po ucięciu za ostatnią ({firstKeptPeriod}) zostaje {remainingPeriods} okresów, wymagane {SeriesQuality.MinPeriods}"));
        }

        return new ContinuityCheck(Applicable: true, ContinuousFromPeriod: firstKeptPeriod, Reason: string.Create(CultureInfo.InvariantCulture,
            $"luk: {largeGapIndices.Length}; szereg ucięty za ostatnią ({gapDays[lastLargeGap]:F0} dni), zostaje {remainingPeriods} okresów od {firstKeptPeriod}"));
    }
}
