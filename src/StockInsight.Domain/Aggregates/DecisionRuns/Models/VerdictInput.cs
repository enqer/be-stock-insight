namespace StockInsight.Domain.Aggregates.DecisionRuns.Models;

public sealed record VerdictInput(Guid InstrumentId, string Reason, DateOnly? PeriodEnd);
