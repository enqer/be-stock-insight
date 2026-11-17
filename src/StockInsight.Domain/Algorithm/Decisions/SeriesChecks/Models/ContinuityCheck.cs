namespace StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

public sealed record ContinuityCheck(bool Applicable, string? ContinuousFromPeriod, string? Reason);
