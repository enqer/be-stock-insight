using StockInsight.Domain.Calculations.Inputs.Statements;
using StockInsight.Domain.Calculations.Metrics.Capital;
using StockInsight.Domain.Calculations.Metrics.CostOfCapital.Enums;
using StockInsight.Domain.Calculations.Numerics;

namespace StockInsight.Domain.Calculations.Metrics.CostOfCapital;

public static class WindowLeverage
{
    public const int WindowYears = 5;
    public const int MinQuarters = 4;

    public static WindowLeverageValue At(IReadOnlyList<Quarter> quarters, int t)
    {
        var values = new List<double>();
        for (var k = Math.Max(0, t - WindowYears * 4 + 1); k <= t; k++)
        {
            if (quarters.At(k)?.Equity is { } equity && equity > 0)
            {
                values.Add(Math.Max(0.0, Debt.At(quarters, k).Amount / equity));
            }
        }

        if (values.Count >= MinQuarters)
        {
            return new WindowLeverageValue(Statistics.Median(values), WindowLeverageSource.Window);
        }

        if (quarters.At(t)?.Equity is { } equityNow && equityNow > 0)
        {
            return new WindowLeverageValue(Math.Max(0.0, Debt.At(quarters, t).Amount / equityNow), WindowLeverageSource.Current);
        }

        return new WindowLeverageValue(0.0, WindowLeverageSource.None);
    }
}
