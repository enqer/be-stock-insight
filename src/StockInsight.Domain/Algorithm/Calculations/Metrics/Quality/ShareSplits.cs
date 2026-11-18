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
            var ordinaryChange = ratio is <= UpperJump and >= LowerJump;
            if (ordinaryChange)
            {
                continue;
            }

            var noSharesIssued = (quarters[i].ShareIssue ?? 0.0) == 0;
            var (capitalGrew, capitalProportional) = ShareCapitalChange(quarters[i - 1], quarters[i], ratio);
            var integerMultiple = IsIntegerMultiple(ratio) || IsIntegerMultiple(1 / ratio);

            var compositeOperation = noSharesIssued && integerMultiple && capitalGrew && !capitalProportional;
            if (compositeOperation)
            {
                unifiedFromIndex = Math.Max(unifiedFromIndex, i);
                continue;
            }

            var splitOrBonusIssue = noSharesIssued && (!capitalGrew || capitalProportional);
            if (splitOrBonusIssue)
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

    private static (bool Grew, bool Proportional) ShareCapitalChange(Quarter before, Quarter now, double shareRatio)
    {
        if (now.ShareCapital is not { } capitalNow || before.ShareCapital is not { } capitalBefore || capitalBefore == 0)
        {
            return (false, false);
        }

        var capitalRatio = capitalNow / capitalBefore;
        var grew = capitalRatio > CapitalGrowthThreshold;
        var proportional = Math.Abs(capitalRatio / shareRatio - 1) < ProportionTolerance;
        return (grew, proportional);
    }

    private static bool IsIntegerMultiple(double ratio) =>
        Math.Abs(ratio - Math.Round(ratio)) < IntegerTolerance && Math.Round(ratio) >= 2;
}
