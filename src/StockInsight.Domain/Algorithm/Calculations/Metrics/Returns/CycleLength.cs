using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Numerics;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Returns;

public static class CycleLength
{
    public const int FallbackYears = 5;
    public const int MinYears = 3;
    public const int MaxYears = 8;
    private const int MinObservations = 20;

    public static int Years(IReadOnlyList<Quarter> quarters, int periodIndex)
    {
        var revenueGrowth = new List<double>();
        for (var periodInWindow = Math.Max(8, periodIndex - 47); periodInWindow <= periodIndex; periodInWindow++)
        {
            if (TrailingTwelveMonths.Sum(quarters, periodInWindow, x => x.Revenue) is { } now
                && TrailingTwelveMonths.Sum(quarters, periodInWindow - 4, x => x.Revenue) is { } before && before > 0)
            {
                revenueGrowth.Add(now / before - 1);
            }
        }

        if (revenueGrowth.Count < MinObservations)
        {
            return FallbackYears;
        }

        var mean = Statistics.SumInOrder(revenueGrowth) / revenueGrowth.Count;
        var mostNegativeCorrelation = 0.0;
        int? halfCycleLag = null;
        for (var lag = 4; lag < Math.Min(21, revenueGrowth.Count / 2); lag++)
        {
            var squaresNow = 0.0;
            var squaresLagged = 0.0;
            var crossProducts = 0.0;
            for (var index = 0; index < revenueGrowth.Count - lag; index++)
            {
                var deviationNow = revenueGrowth[index] - mean;
                var deviationLagged = revenueGrowth[index + lag] - mean;
                squaresNow += deviationNow * deviationNow;
                squaresLagged += deviationLagged * deviationLagged;
                crossProducts += deviationNow * deviationLagged;
            }

            var spreadNow = Math.Sqrt(squaresNow);
            var spreadLagged = Math.Sqrt(squaresLagged);
            if (spreadNow == 0 || spreadLagged == 0)
            {
                continue;
            }

            var correlation = crossProducts / (spreadNow * spreadLagged);
            if (correlation < mostNegativeCorrelation)
            {
                mostNegativeCorrelation = correlation;
                halfCycleLag = lag;
            }
        }

        if (halfCycleLag is not { } halfCycle)
        {
            return FallbackYears;
        }

        var years = (int)Math.Round(2.0 * halfCycle / 4.0, MidpointRounding.ToEven);
        return Math.Clamp(years, MinYears, MaxYears);
    }
}
