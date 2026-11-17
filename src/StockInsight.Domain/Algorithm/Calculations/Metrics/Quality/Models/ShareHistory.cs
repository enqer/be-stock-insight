using StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.Enums;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.Models;

public sealed record ShareHistory(IReadOnlyList<double?> AdjustedShares, IReadOnlyList<ShareBase> Bases);
