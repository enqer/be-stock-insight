using StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.Enums;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

public sealed record PeriodMeasures(
    string PeriodLabel,
    DateOnly PeriodEnd,
    DateOnly PublicationDate,
    int PeriodIndex,
    double? Roic,
    double? Roiic,
    int? DirectionCriteriaMet,
    double? TrailingNetIncome,
    double? Shares,
    ShareBase ShareBase);
