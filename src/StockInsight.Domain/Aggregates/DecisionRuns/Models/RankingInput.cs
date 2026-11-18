namespace StockInsight.Domain.Aggregates.DecisionRuns.Models;

public sealed record RankingInput(Guid InstrumentId, double Score, double HeadroomPercentile, double RoiicPercentile, double CashConversionPercentile);
