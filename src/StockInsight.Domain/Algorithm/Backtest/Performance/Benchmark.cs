using StockInsight.Domain.Algorithm.Backtest.Performance.Models;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Calculations.Numerics;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

namespace StockInsight.Domain.Algorithm.Backtest.Performance;

public static class Benchmark
{
    public static IReadOnlyList<CurvePoint> Run(IReadOnlyList<AcceptedCompany> universe, IReadOnlyList<DateOnly> dates)
    {
        if (dates.Count == 0)
        {
            return [];
        }

        var prices = universe.Select(x => x.Prices).ToList();
        var value = 1.0;
        var curve = new List<CurvePoint>(dates.Count) { new(dates[0], value, universe.Count) };
        for (var i = 1; i < dates.Count; i++)
        {
            if (EqualWeightedReturn(prices, dates[i - 1], dates[i]) is { } periodReturn)
            {
                value *= 1 + periodReturn;
            }

            curve.Add(new CurvePoint(dates[i], value, universe.Count));
        }

        return curve;
    }

    private static double? EqualWeightedReturn(IReadOnlyList<PriceSeries> prices, DateOnly from, DateOnly to)
    {
        var returns = new List<double>();
        foreach (var series in prices)
        {
            if (series.LastOnOrBefore(from)?.Close is { } start && series.LastOnOrBefore(to)?.Close is { } end)
            {
                returns.Add(end / start - 1);
            }
        }

        if (returns.Count == 0)
        {
            return null;
        }

        return Statistics.SumInOrder(returns) / returns.Count;
    }
}
