using StockInsight.Domain.Algorithm.Calculations.Metrics.Enums;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics;

public readonly record struct Measured<T>(T? Value, MeasureStatus Status, string Reason)
    where T : struct
{
    public T? OkValue => Status == MeasureStatus.Ok ? Value : null;

    public static Measured<T> Ok(T value) => new(value, MeasureStatus.Ok, "");

    public static Measured<T> Missing(string reason) => new(null, MeasureStatus.Missing, reason);

    public static Measured<T> Unreliable(string reason) => new(null, MeasureStatus.Unreliable, reason);

    public static Measured<T> OutOfRange(T value, string reason) => new(value, MeasureStatus.OutOfRange, reason);
}
