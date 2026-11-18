using System.Globalization;
using StockInsight.Domain.Algorithm.Calculations.Numerics;
using StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

namespace StockInsight.Domain.Algorithm.Decisions.SeriesChecks;

public static class OffsetQuarantineRule
{
    public const double MaxOffsetDeviationDays = 150;
    public const int AbsurdOffsetDays = -365;

    public static OffsetQuarantine Apply(IReadOnlyList<ScheduledReport> reports)
    {
        var noQuarantinedPeriods = new HashSet<string>(StringComparer.Ordinal);

        var offsets = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var report in reports)
        {
            offsets[report.PeriodLabel] = report.PublicationDate.DayNumber - report.PeriodEnd.DayNumber;
        }

        if (offsets.Count < SeriesQuality.MinPeriods)
        {
            return new OffsetQuarantine(reports, noQuarantinedPeriods, Reason: null);
        }

        var medianOffset = Statistics.Median([.. offsets.Values.Select(x => (double)x)]);
        var suspectPeriods = offsets
            .Where(x => Math.Abs(x.Value - medianOffset) > MaxOffsetDeviationDays || x.Value < AbsurdOffsetDays)
            .Select(x => x.Key)
            .ToHashSet(StringComparer.Ordinal);
        if (suspectPeriods.Count == 0)
        {
            return new OffsetQuarantine(reports, noQuarantinedPeriods, Reason: null);
        }

        return new OffsetQuarantine(
            [.. reports.Where(x => !suspectPeriods.Contains(x.PeriodLabel))],
            suspectPeriods,
            string.Create(CultureInfo.InvariantCulture,
                $"{suspectPeriods.Count} okresów z datą publikacji odstającą od konwencji spółki (mediana {medianOffset:+0;-0} dni) — kwarantanna"));
    }
}
