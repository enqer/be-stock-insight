namespace StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

public sealed record CadenceCheck(double? MedianGapDays, string? Rejection);
