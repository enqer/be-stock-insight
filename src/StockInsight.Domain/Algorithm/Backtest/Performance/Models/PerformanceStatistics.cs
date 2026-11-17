using StockInsight.Domain.Algorithm.Calculations.Numerics;

namespace StockInsight.Domain.Algorithm.Backtest.Performance.Models;

public sealed record PerformanceStatistics(
    double? Cagr,
    double MaxDrawdown,
    double MedianPeriodReturn,
    int BadPeriods,
    int Periods,
    double MedianPositions,
    double EndMultiple)
{
    public const double BadPeriodThreshold = -0.10;

    public const double DaysPerYear = 365.25;

    public static PerformanceStatistics From(IReadOnlyList<CurvePoint> curve)
    {
        if (curve.Count < 2)
        {
            throw new ArgumentException("statistics require at least two curve points", nameof(curve));
        }

        var first = curve[0].Value;
        var last = curve[^1].Value;
        var years = (curve[^1].Date.DayNumber - curve[0].Date.DayNumber) / DaysPerYear;
        double? cagr = years > 0 && first > 0 ? Math.Pow(last / first, 1 / years) - 1 : null;

        var peak = first;
        var drawdown = 0.0;
        foreach (var point in curve)
        {
            peak = Math.Max(peak, point.Value);
            drawdown = Math.Min(drawdown, point.Value / peak - 1);
        }

        var returns = curve.Skip(1).Select((x, i) => x.Value / curve[i].Value - 1).ToList();

        return new PerformanceStatistics(
            cagr,
            drawdown,
            Statistics.Median(returns),
            returns.Count(x => x < BadPeriodThreshold),
            returns.Count,
            Statistics.Median([.. curve.Select(x => (double)x.PositionCount)]),
            last / first);
    }
}
