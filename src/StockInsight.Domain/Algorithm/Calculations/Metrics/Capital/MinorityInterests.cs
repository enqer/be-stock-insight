using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Capital;

public static class MinorityInterests
{
    private const double RelativeTolerance = 1e-4;

    public static bool AreInsideEquity(IReadOnlyList<Quarter> quarters)
    {
        var inside = 0;
        var outside = 0;
        foreach (var quarter in quarters)
        {
            if (quarter.TotalAssets is not { } assets
                || quarter.Equity is not { } equity
                || quarter.MinorityInterests is not { } minorities || minorities == 0
                || quarter.NonCurrentLiabilities is not { } nonCurrentLiabilities
                || quarter.CurrentLiabilities is not { } currentLiabilities)
            {
                continue;
            }

            var withoutMinorities = equity + nonCurrentLiabilities + currentLiabilities;
            var tolerance = Math.Max(1.0, Math.Abs(assets) * RelativeTolerance);
            if (Math.Abs(withoutMinorities - assets) <= tolerance)
            {
                inside++;
            }
            else if (Math.Abs(withoutMinorities + minorities - assets) <= tolerance)
            {
                outside++;
            }
        }

        return inside > outside;
    }

    public static double At(IReadOnlyList<Quarter> quarters, int periodIndex, bool insideEquity) =>
        insideEquity ? 0.0 : quarters[periodIndex].MinorityInterests ?? 0.0;
}
