using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Capital;
using StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.Enums;
using StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.Models;
using StockInsight.Domain.Algorithm.Calculations.Numerics;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital;

public static class WindowLeverage
{
    public const int WindowYears = 5;
    public const int MinQuarters = 4;

    public static WindowLeverageValue At(IReadOnlyList<Quarter> quarters, int periodIndex)
    {
        var windowLeverages = new List<double>();
        for (var index = Math.Max(0, periodIndex - WindowYears * 4 + 1); index <= periodIndex; index++)
        {
            if (quarters[index].Equity is { } equity && equity > 0)
            {
                windowLeverages.Add(Math.Max(0.0, Debt.At(quarters, index).Amount / equity));
            }
        }

        if (windowLeverages.Count >= MinQuarters)
        {
            return new WindowLeverageValue(Statistics.Median(windowLeverages), WindowLeverageSource.Window);
        }

        if (quarters[periodIndex].Equity is { } equityNow && equityNow > 0)
        {
            return new WindowLeverageValue(Math.Max(0.0, Debt.At(quarters, periodIndex).Amount / equityNow), WindowLeverageSource.Current);
        }

        return new WindowLeverageValue(0.0, WindowLeverageSource.None);
    }
}
