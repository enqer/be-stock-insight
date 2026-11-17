namespace StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

public sealed record OffsetQuarantine(IReadOnlyList<ScheduledReport> Remaining, IReadOnlySet<string> QuarantinedPeriods, string? Reason);
