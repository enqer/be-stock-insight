using StockInsight.Domain.Algorithm.Backtest.Performance.Models;
using StockInsight.Domain.Algorithm.Calculations.Numerics;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

namespace StockInsight.Domain.Algorithm.Backtest.Performance;

public static class Benchmark
{
    public static IReadOnlyList<CurvePoint> Run(IReadOnlyList<AcceptedCompany> universe, IReadOnlyList<DateOnly> dates)
    {
        var prices = universe.Select(x => x.Prices).ToList();
        var curve = new List<CurvePoint>(dates.Count);
        var value = 1.0;
        for (var index = 0; index < dates.Count; index++)
        {
            curve.Add(new CurvePoint(dates[index], value, universe.Count));
            if (index + 1 == dates.Count)
            {
                break;
            }

            var returns = new List<double>();
            foreach (var series in prices)
            {
                if (series.LastOnOrBefore(dates[index])?.Close is { } start && series.LastOnOrBefore(dates[index + 1])?.Close is { } end)
                {
                    returns.Add(end / start - 1);
                }
            }

            if (returns.Count > 0)
            {
                value *= 1 + Statistics.SumInOrder(returns) / returns.Count;
            }
        }

        return curve;
    }
}
