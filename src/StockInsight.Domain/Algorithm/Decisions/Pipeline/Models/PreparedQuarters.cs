using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

public sealed record PreparedQuarters(IReadOnlyList<Quarter> Quarters, bool MinoritiesInsideEquity, ShareHistory Shares);
