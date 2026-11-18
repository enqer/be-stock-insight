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
    private const int WindowQuarters = 48;
    private const int EarliestWindowStartIndex = 8;
    private const int MinLagQuarters = 4;
    private const int MaxLagQuarters = 20;

    public static int Years(IReadOnlyList<Quarter> quarters, int periodIndex)
    {
        var revenueGrowth = new List<double>();
        var windowStart = Math.Max(EarliestWindowStartIndex, periodIndex - (WindowQuarters - 1));
        for (var periodInWindow = windowStart; periodInWindow <= periodIndex; periodInWindow++)
        {
            if (TrailingTwelveMonths.Sum(quarters, periodInWindow, x => x.Revenue) is { } now
                && TrailingTwelveMonths.Sum(quarters, periodInWindow - TrailingTwelveMonths.Quarters, x => x.Revenue) is { } before && before > 0)
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
        for (var lag = MinLagQuarters; lag <= MaxLagQuarters && lag < revenueGrowth.Count / 2; lag++)
        {
            if (LaggedCorrelation(revenueGrowth, mean, lag) is { } correlation && correlation < mostNegativeCorrelation)
            {
                mostNegativeCorrelation = correlation;
                halfCycleLag = lag;
            }
        }

        if (halfCycleLag is not { } halfCycle)
        {
            return FallbackYears;
        }

        var cycleQuarters = 2.0 * halfCycle;
        var years = (int)Math.Round(cycleQuarters / TrailingTwelveMonths.Quarters, MidpointRounding.ToEven);
        return Math.Clamp(years, MinYears, MaxYears);
    }

    private static double? LaggedCorrelation(IReadOnlyList<double> growth, double mean, int lag)
    {
        var squaresNow = 0.0;
        var squaresLagged = 0.0;
        var crossProducts = 0.0;
        for (var index = 0; index < growth.Count - lag; index++)
        {
            var deviationNow = growth[index] - mean;
            var deviationLagged = growth[index + lag] - mean;
            squaresNow += deviationNow * deviationNow;
            squaresLagged += deviationLagged * deviationLagged;
            crossProducts += deviationNow * deviationLagged;
        }

        var spreadNow = Math.Sqrt(squaresNow);
        var spreadLagged = Math.Sqrt(squaresLagged);
        if (spreadNow == 0 || spreadLagged == 0)
        {
            return null;
        }

        return crossProducts / (spreadNow * spreadLagged);
    }
}
