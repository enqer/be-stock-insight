using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

namespace StockInsight.Domain.Algorithm.Decisions.SeriesChecks;

public static class PublicationSchedule
{
    public static IReadOnlyList<ScheduledReport> From(IReadOnlyList<Quarter> quarters) =>
        [
            .. quarters
                .Where(x => x.PublishedAt is not null)
                .Select(x => new ScheduledReport(x.PublishedAt!.Value, x.Label, x.Period))
                .OrderBy(x => x.PublicationDate)
                .ThenBy(x => x.PeriodLabel, StringComparer.Ordinal),
        ];

    public static IReadOnlyList<double> GapDays(IReadOnlyList<ScheduledReport> reports) =>
        [.. reports.Skip(1).Select((x, i) => (double)(x.PublicationDate.DayNumber - reports[i].PublicationDate.DayNumber))];
}
