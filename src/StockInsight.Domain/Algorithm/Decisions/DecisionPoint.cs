using StockInsight.Domain.Algorithm.Decisions.Measures;

namespace StockInsight.Domain.Algorithm.Decisions;

public sealed record DecisionPoint
{
    public required string PeriodLabel { get; init; }

    public required DateOnly PeriodEnd { get; init; }

    public required DateOnly PublicationDate { get; init; }

    public required int PeriodIndex { get; init; }

    public required ReturnMeasures Returns { get; init; }

    public required CapitalMeasures Capital { get; init; }

    public required CostOfCapitalMeasures CostOfCapital { get; init; }

    public required EarningsMeasures Earnings { get; init; }

    public required QualityMeasures Quality { get; init; }
}
