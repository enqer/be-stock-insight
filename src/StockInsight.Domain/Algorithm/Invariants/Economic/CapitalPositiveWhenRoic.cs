using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Invariants.Models;

namespace StockInsight.Domain.Algorithm.Invariants.Economic;

public static class CapitalPositiveWhenRoic
{
    public const string Name = "T7 ic_sr > 0 gdy ROIC policzony";

    public static IdentityResult Check(IReadOnlyList<AcceptedCompany> universe)
    {
        var tally = new IdentityCheck.Tally(Name);
        foreach (var (ticker, point) in IdentityCheck.Observations(universe))
        {
            if (point.Returns.Roic is null)
            {
                continue;
            }

            tally.FailIf(point.Capital.AverageInvestedCapital is not { } capital || capital <= 0, () => $"{ticker} {point.PeriodLabel}");
        }

        return tally.Result();
    }
}
