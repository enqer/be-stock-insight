using StockInsight.Domain.Calculations.Inputs.Statements;
using StockInsight.Domain.Calculations.Metrics.Quality.Enums;

namespace StockInsight.Domain.Calculations.Metrics.Quality;

public static class ShareSplits
{
    private const double UpperJump = 1.5;
    private const double LowerJump = 0.67;
    private const double CapitalGrowthThreshold = 1.05;
    private const double ProportionTolerance = 0.02;
    private const double IntegerTolerance = 0.02;

    public static ShareHistory Normalise(IReadOnlyList<Quarter> quarters)
    {
        var n = quarters.Count;
        var shares = quarters.Select(q => q.Shares).ToArray();

        var factor = Enumerable.Repeat(1.0, n).ToArray();
        var ambiguousBefore = 0;

        for (var i = 1; i < n; i++)
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
                ambiguousBefore = Math.Max(ambiguousBefore, i);
                continue;
            }

            if (issued == 0 && (!capitalGrew || capitalProportional))
            {
                for (var j = 0; j < i; j++)
                {
                    factor[j] *= ratio;
                }
            }
        }

        var adjusted = shares.Select((s, i) => s is { } v && v != 0 ? v * factor[i] : (double?)null).ToArray();
        var bases = Enumerable.Range(0, n).Select(i => i < ambiguousBefore ? ShareBase.Ambiguous : ShareBase.Unified).ToArray();
        return new ShareHistory(adjusted, bases);
    }

    private static bool IsIntegerMultiple(double ratio) =>
        Math.Abs(ratio - Math.Round(ratio)) < IntegerTolerance && Math.Round(ratio) >= 2;
}
