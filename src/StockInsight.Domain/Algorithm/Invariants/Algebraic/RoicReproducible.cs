using System.Globalization;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Invariants.Models;

namespace StockInsight.Domain.Algorithm.Invariants.Algebraic;

public static class RoicReproducible
{
    public const string Name = "T1 ROIC = NOPAT / ic_sr";

    public static IdentityResult Check(IReadOnlyList<AcceptedCompany> universe)
    {
        var tally = new IdentityCheck.Tally(Name);
        foreach (var (ticker, point) in IdentityCheck.Observations(universe))
        {
            if (point.Returns.Roic is not { } roic || point.Earnings.TrailingEbit is not { } ebit || point.Capital.AverageInvestedCapital is not { } capital || capital == 0)
            {
                continue;
            }

            var expected = ebit * (1 - MarketConstants.TaxRate) / capital;
            tally.FailIf(IdentityCheck.Differs(expected, roic), () => string.Create(CultureInfo.InvariantCulture, $"{ticker} {point.PeriodLabel}: {roic:F6} vs {expected:F6}"));
        }

        return tally.Result();
    }
}
