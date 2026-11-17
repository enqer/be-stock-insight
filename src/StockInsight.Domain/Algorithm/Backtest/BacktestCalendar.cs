using StockInsight.Domain.Algorithm.Strategy;

namespace StockInsight.Domain.Algorithm.Backtest;

public static class BacktestCalendar
{
    private static readonly DateOnly _productionStart = new(2012, 2, 15);
    private static readonly DateOnly _productionEnd = new(2026, 8, 15);

    public static IReadOnlyList<DateOnly> Production() => DecisionCalendar.Quarterly(_productionStart, _productionEnd);
}
