namespace StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

public sealed record SeriesQualityResult(
    bool Applicable,
    IReadOnlyList<string> Reasons,
    IReadOnlySet<string> QuarantinedPeriods,
    string? ContinuousFromPeriod);
