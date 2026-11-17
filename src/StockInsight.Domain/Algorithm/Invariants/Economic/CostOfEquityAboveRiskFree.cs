using System.Globalization;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Invariants.Models;

namespace StockInsight.Domain.Algorithm.Invariants.Economic;

public static class CostOfEquityAboveRiskFree
{
    public const string Name = "T4 ke >= rf (z daty decyzji)";
    private const double Slack = 1e-12;

    public static IdentityResult Check(IReadOnlyList<AcceptedCompany> universe)
    {
        var tally = new IdentityCheck.Tally(Name);
        foreach (var (ticker, point) in IdentityCheck.Observations(universe))
        {
            if (point.CostOfCapital.Wacc is not { } wacc)
            {
                continue;
            }

            var costOfEquity = wacc.Components.CostOfEquity;
            tally.FailIf(costOfEquity < point.CostOfCapital.RiskFreeRate - Slack, () => string.Create(CultureInfo.InvariantCulture, $"{ticker} {point.PeriodLabel}: ke={costOfEquity:F4} < rf={point.CostOfCapital.RiskFreeRate:F4}"));
        }

        return tally.Result();
    }
}
