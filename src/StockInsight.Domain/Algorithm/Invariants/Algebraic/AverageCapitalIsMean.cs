using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Invariants.Models;

namespace StockInsight.Domain.Algorithm.Invariants.Algebraic;

public static class AverageCapitalIsMean
{
    public const string Name = "T2 ic_sr = srednia(ic, ic_prev)";

    public static IdentityResult Check(IReadOnlyList<AcceptedCompany> universe)
    {
        var tally = new IdentityCheck.Tally(Name);
        foreach (var (ticker, point) in IdentityCheck.Observations(universe))
        {
            if (point.Capital.InvestedCapital is not { } now || point.Capital.AverageInvestedCapital is not { } average)
            {
                continue;
            }

            var expected = (now + (point.Capital.InvestedCapitalYearAgo ?? now)) / 2;
            tally.FailIf(IdentityCheck.Differs(expected, average), () => $"{ticker} {point.PeriodLabel}");
        }

        return tally.Result();
    }
}
