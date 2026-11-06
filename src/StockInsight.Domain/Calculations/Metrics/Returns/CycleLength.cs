using StockInsight.Domain.Calculations.Inputs.Statements;
using StockInsight.Domain.Calculations.Numerics;

namespace StockInsight.Domain.Calculations.Metrics.Returns;

public static class CycleLength
{
    public const int FallbackYears = 5;
    public const int MinYears = 3;
    public const int MaxYears = 8;
    private const int MinObservations = 20;

    public static int Years(IReadOnlyList<Quarter> quarters, int t)
    {
        var growth = new List<double>();
        for (var k = Math.Max(8, t - 47); k <= t; k++)
        {
            if (TrailingTwelveMonths.Sum(quarters, k, q => q.Revenue) is { } now
                && TrailingTwelveMonths.Sum(quarters, k - 4, q => q.Revenue) is { } before && before > 0)
            {
                growth.Add(now / before - 1);
            }
        }

        if (growth.Count < MinObservations)
        {
            return FallbackYears;
        }

        var mean = Statistics.SumInOrder(growth) / growth.Count;
        var best = 0.0;
        int? bestLag = null;
        for (var lag = 4; lag < Math.Min(21, growth.Count / 2); lag++)
        {
            double sxx = 0, syy = 0, sxy = 0;
            for (var j = 0; j < growth.Count - lag; j++)
            {
                var x = growth[j] - mean;
                var y = growth[j + lag] - mean;
                sxx += x * x;
                syy += y * y;
                sxy += x * y;
            }

            var sx = Math.Sqrt(sxx);
            var sy = Math.Sqrt(syy);
            if (sx == 0 || sy == 0)
            {
                continue;
            }

            var correlation = sxy / (sx * sy);
            if (correlation < best)
            {
                best = correlation;
                bestLag = lag;
            }
        }

        if (bestLag is not { } halfCycle)
        {
            return FallbackYears;
        }

        var years = (int)Math.Round(2.0 * halfCycle / 4.0, MidpointRounding.ToEven);
        return Math.Clamp(years, MinYears, MaxYears);
    }
}
