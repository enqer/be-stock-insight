using System.Globalization;
using StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

namespace StockInsight.Domain.Algorithm.Decisions.SeriesChecks;

public static class FirstGapQuarantineRule
{
    public const double FirstGapMultiple = 2.5;

    public static FirstGapQuarantine Apply(IReadOnlyList<ScheduledReport> reports, IReadOnlyList<double> gapDays, double medianGap)
    {
        if (gapDays[0] <= FirstGapMultiple * medianGap)
        {
            return new FirstGapQuarantine(reports, null, null);
        }

        var first = reports[0];
        return new FirstGapQuarantine(
            [.. reports.Skip(1)],
            first.PeriodLabel,
            string.Create(CultureInfo.InvariantCulture, $"okres {first.PeriodLabel} ma błędną datę publikacji ({first.PublicationDate:yyyy-MM-dd}) — kwarantanna"));
    }
}
