using System.Globalization;
using StockInsight.Domain.Algorithm.Calculations.Numerics;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Invariants.Models;

namespace StockInsight.Domain.Algorithm.Invariants.Economic;

public static class PublicationOffsetConsistent
{
    public const string Name = "T5 odstep publikacji zgodny z konwencja spolki";
    public const double MaxDeviationDays = 150;
    public const int AbsurdOffsetDays = -365;
    public const int MinPeriods = 4;

    public static IdentityResult Check(IReadOnlyList<AcceptedCompany> universe)
    {
        var tally = new IdentityCheck.Tally(Name);
        foreach (var company in universe.OrderBy(x => x.Ticker, StringComparer.Ordinal))
        {
            var lastOffsetByPeriod = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var point in company.Series)
            {
                lastOffsetByPeriod[point.PeriodLabel] = point.PublicationDate.DayNumber - point.PeriodEnd.DayNumber;
            }

            if (lastOffsetByPeriod.Count < MinPeriods)
            {
                continue;
            }

            var median = Statistics.Median([.. lastOffsetByPeriod.Values.Select(x => (double)x)]);
            foreach (var (period, days) in lastOffsetByPeriod)
            {
                tally.FailIf(Math.Abs(days - median) > MaxDeviationDays || days < AbsurdOffsetDays, () => string.Create(CultureInfo.InvariantCulture, 
                        $"{company.Ticker} {period}: odstep {days:+0;-0} dni, mediana spolki {median:+0;-0}"));
            }
        }

        return tally.Result();
    }
}
