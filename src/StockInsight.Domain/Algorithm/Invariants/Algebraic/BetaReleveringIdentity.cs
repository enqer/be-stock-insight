using System.Globalization;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Invariants.Models;

namespace StockInsight.Domain.Algorithm.Invariants.Algebraic;

public static class BetaReleveringIdentity
{
    public const string Name = "T8 beta_l = beta_reg*(1+(1-t)de)/(1+(1-t)de_okno)";
    public const double RelativeTolerance = 1e-6;
    private const double ClipMargin = 1e-9;

    public static IdentityResult Check(IReadOnlyList<AcceptedCompany> universe)
    {
        var tally = new IdentityCheck.Tally(Name);
        foreach (var (ticker, point) in IdentityCheck.Observations(universe))
        {
            if (point.CostOfCapital.BetaRegression is not { } betaRegression || point.CostOfCapital.Wacc is not { } wacc)
            {
                continue;
            }

            var betaClipped = point.CostOfCapital.BetaUnlevered <= Beta.Floor + ClipMargin || point.CostOfCapital.BetaUnlevered >= Beta.Ceiling - ClipMargin;
            if (betaClipped)
            {
                continue;
            }

            var components = wacc.Components;
            const double taxShield = 1 - MarketConstants.TaxRate;
            var windowLeverage = point.CostOfCapital.WindowLeverage;
            var expected = betaRegression * (1 + taxShield * components.DebtToEquity) / (1 + taxShield * windowLeverage);
            tally.FailIf(IdentityCheck.Differs(expected, components.LeveredBeta, RelativeTolerance), () => string.Create(CultureInfo.InvariantCulture, 
                    $"{ticker} {point.PeriodLabel}: beta {components.LeveredBeta:F6} vs oczekiwana {expected:F6} (de={components.DebtToEquity:F3}, de_okno={windowLeverage:F3})"));
        }

        return tally.Result();
    }
}
