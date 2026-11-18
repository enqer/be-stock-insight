using System.Globalization;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Invariants.Models;

namespace StockInsight.Domain.Algorithm.Invariants.Provenance;

public static class WindowLeverageDiffersFromCurrent
{
    public const double MinShareDiffering = 0.30;
    public const double MinDifference = 0.01;

    public static IdentityResult Check(IReadOnlyList<AcceptedCompany> universe)
    {
        var differing = 0;
        var total = 0;
        foreach (var (_, point) in IdentityCheck.Observations(universe))
        {
            if (point.CostOfCapital.Wacc is not { } wacc)
            {
                continue;
            }

            total++;
            if (Math.Abs(wacc.Components.DebtToEquity - point.CostOfCapital.WindowLeverage) > MinDifference)
            {
                differing++;
            }
        }

        var share = total > 0 ? (double)differing / total : 0.0;
        var brokenCount = share >= MinShareDiffering ? 0 : 1;

        var percent = Math.Round(share * 100, MidpointRounding.ToEven).ToString("F0", CultureInfo.InvariantCulture);
        return new IdentityResult(
            $"T10 de_okno != de w >=30% obs. (jest {percent}%)",
            brokenCount,
            total,
            brokenCount == 0 ? [] : ["dzwignia okna pokrywa sie z biezaca — prawdopodobnie podstawiona"]);
    }
}
