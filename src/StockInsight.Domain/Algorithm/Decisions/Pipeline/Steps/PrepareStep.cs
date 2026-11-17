using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Capital;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Quality;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Steps;

public static class PrepareStep
{
    public static PreparedQuarters Run(IReadOnlyList<Quarter> quarters)
    {
        var withCashFlowGaps = CashFlowGaps.ZeroCashFlowsAsMissing(quarters);
        return new PreparedQuarters(withCashFlowGaps, MinorityInterests.AreInsideEquity(withCashFlowGaps), ShareSplits.Normalise(withCashFlowGaps));
    }
}
