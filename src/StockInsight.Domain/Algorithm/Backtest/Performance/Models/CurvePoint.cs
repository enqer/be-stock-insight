namespace StockInsight.Domain.Algorithm.Backtest.Performance.Models;

public sealed record CurvePoint(DateOnly Date, double Value, int PositionCount);
