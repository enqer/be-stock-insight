using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Capital;

public static class MinorityInterests
{
    private const double RelativeTolerance = 1e-4;

    public static bool AreInsideEquity(IReadOnlyList<Quarter> quarters)
    {
        var quartersBalancedWithMinoritiesInEquity = 0;
        var quartersBalancedWithMinoritiesSeparate = 0;
        foreach (var quarter in quarters)
        {
            if (quarter.TotalAssets is not { } assets
                || quarter.Equity is not { } equity
                || quarter.MinorityInterests is not { } minorities
                || quarter.NonCurrentLiabilities is not { } nonCurrentLiabilities
                || quarter.CurrentLiabilities is not { } currentLiabilities)
            {
                continue;
            }

            if (minorities == 0)
            {
                continue;
            }

            var liabilitiesAndEquity = equity + nonCurrentLiabilities + currentLiabilities;
            var tolerance = Math.Max(1.0, Math.Abs(assets) * RelativeTolerance);
            if (Math.Abs(liabilitiesAndEquity - assets) <= tolerance)
            {
                quartersBalancedWithMinoritiesInEquity++;
            }
            else if (Math.Abs(liabilitiesAndEquity + minorities - assets) <= tolerance)
            {
                quartersBalancedWithMinoritiesSeparate++;
            }
        }

        return quartersBalancedWithMinoritiesInEquity > quartersBalancedWithMinoritiesSeparate;
    }

    public static double At(IReadOnlyList<Quarter> quarters, int periodIndex, bool insideEquity) =>
        insideEquity ? 0.0 : quarters[periodIndex].MinorityInterests ?? 0.0;
}
