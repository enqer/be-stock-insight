namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

public sealed class StepResult<T>
{
    private readonly T? _value;

    internal StepResult(T? value, StepRejection? rejection)
    {
        _value = value;
        Rejection = rejection;
    }

    public StepRejection? Rejection { get; }

    public T Value => Rejection is null ? _value! : throw new InvalidOperationException($"step rejected the company: {Rejection.Reason}");
}
