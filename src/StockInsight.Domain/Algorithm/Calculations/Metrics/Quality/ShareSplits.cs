using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.Enums;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.Models;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Quality;

public static class ShareSplits
{
    private const double UpperJump = 1.5;
    private const double LowerJump = 0.67;
    private const double CapitalGrowthThreshold = 1.05;
    private const double ProportionTolerance = 0.02;
    private const double IntegerTolerance = 0.02;

    public static ShareHistory Normalise(IReadOnlyList<Quarter> quarters)
    {
        var quarterCount = quarters.Count;
        var shares = quarters.Select(x => x.Shares).ToArray();

        var splitFactors = Enumerable.Repeat(1.0, quarterCount).ToArray();
        var unifiedFromIndex = 0;

        for (var i = 1; i < quarterCount; i++)
        {
            if (shares[i - 1] is not { } before || shares[i] is not { } now || before == 0 || now == 0)
            {
                continue;
            }

            var ratio = now / before;
            if (ratio is <= UpperJump and >= LowerJump)
            {
                continue;
            }

            var issued = quarters[i].ShareIssue ?? 0.0;
            var capitalGrew = false;
            var capitalProportional = false;
            if (quarters[i].ShareCapital is { } capitalNow
                && quarters[i - 1].ShareCapital is { } capitalBefore && capitalBefore != 0)
            {
                var capitalRatio = capitalNow / capitalBefore;
                capitalGrew = capitalRatio > CapitalGrowthThreshold;
                capitalProportional = Math.Abs(capitalRatio / ratio - 1) < ProportionTolerance;
            }

            var integerMultiple = IsIntegerMultiple(ratio) || IsIntegerMultiple(1 / ratio);

            if (issued == 0 && integerMultiple && capitalGrew && !capitalProportional)
            {
                unifiedFromIndex = Math.Max(unifiedFromIndex, i);
                continue;
            }

            if (issued == 0 && (!capitalGrew || capitalProportional))
            {
                for (var earlier = 0; earlier < i; earlier++)
                {
                    splitFactors[earlier] *= ratio;
                }
            }
        }

        var adjusted = shares.Select((x, i) => x is { } count && count != 0 ? count * splitFactors[i] : (double?)null).ToArray();
        var bases = Enumerable.Range(0, quarterCount).Select(x => x < unifiedFromIndex ? ShareBase.Ambiguous : ShareBase.Unified).ToArray();
        return new ShareHistory(adjusted, bases);
    }

    private static bool IsIntegerMultiple(double ratio) =>
        Math.Abs(ratio - Math.Round(ratio)) < IntegerTolerance && Math.Round(ratio) >= 2;
}
