namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Enums;

public enum RejectionStage
{
    OutOfScope = 1,

    TooFewPeriodsInSource = 2,

    SeriesQuality = 4,

    TooFewDecisions = 5,
}
