using System.Globalization;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Invariants.Models;

namespace StockInsight.Domain.Algorithm.Invariants.Algebraic;

public static class WaccReproducible
{
    public const string Name = "T3 WACC = (1-w)*ke + w*kd*(1-t)";

    public const double AbsoluteTolerance = 1e-9;

    public static IdentityResult Check(IReadOnlyList<AcceptedCompany> universe)
    {
        var tally = new IdentityCheck.Tally(Name);
        foreach (var (ticker, point) in IdentityCheck.Observations(universe))
        {
            if (point.CostOfCapital.Wacc is not { } wacc)
            {
                continue;
            }

            var components = wacc.Components;
            var expected = (1 - components.DebtWeight) * components.CostOfEquity + components.DebtWeight * components.CostOfDebt * (1 - MarketConstants.TaxRate);
            tally.FailIf(Math.Abs(expected - wacc.Rate) > AbsoluteTolerance, () => string.Create(CultureInfo.InvariantCulture, $"{ticker} {point.PeriodLabel}: {wacc.Rate:F6} vs {expected:F6}"));
        }

        return tally.Result();
    }
}
