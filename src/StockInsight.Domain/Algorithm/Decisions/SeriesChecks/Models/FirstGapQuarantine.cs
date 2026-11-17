namespace StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

public sealed record FirstGapQuarantine(IReadOnlyList<ScheduledReport> Remaining, string? QuarantinedPeriod, string? Reason);
