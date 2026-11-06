using StockInsight.Domain.Calculations.Metrics.Quality.Enums;

namespace StockInsight.Domain.Calculations.Metrics.Quality;

public sealed record ShareHistory(IReadOnlyList<double?> AdjustedShares, IReadOnlyList<ShareBase> Bases);
