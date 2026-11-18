namespace StockInsight.Domain.Aggregates.DecisionRuns;

public sealed class DecisionVerdict
{
    private DecisionVerdict()
    {
    }

    public Guid DecisionRunId { get; private init; }
    public Guid InstrumentId { get; private init; }
    public string Reason { get; private init; } = string.Empty;
    public DateOnly? PeriodEnd { get; private init; }

    internal static DecisionVerdict Create(Guid decisionRunId, Guid instrumentId, string reason, DateOnly? periodEnd)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);
        return new DecisionVerdict { DecisionRunId = decisionRunId, InstrumentId = instrumentId, Reason = reason, PeriodEnd = periodEnd };
    }
}
