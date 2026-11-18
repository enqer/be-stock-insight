using StockInsight.Domain.Aggregates.DecisionRuns.Models;

namespace StockInsight.Domain.Aggregates.DecisionRuns;

public sealed class DecisionRankingEntry
{
    private DecisionRankingEntry()
    {
    }

    public Guid DecisionRunId { get; private init; }
    public int Position { get; private init; }
    public Guid InstrumentId { get; private init; }
    public double Score { get; private init; }
    public double HeadroomPercentile { get; private init; }
    public double RoiicPercentile { get; private init; }
    public double CashConversionPercentile { get; private init; }

    internal static DecisionRankingEntry Create(Guid decisionRunId, int position, RankingInput input) =>
        new()
        {
            DecisionRunId = decisionRunId,
            Position = position,
            InstrumentId = input.InstrumentId,
            Score = input.Score,
            HeadroomPercentile = input.HeadroomPercentile,
            RoiicPercentile = input.RoiicPercentile,
            CashConversionPercentile = input.CashConversionPercentile,
        };
}
