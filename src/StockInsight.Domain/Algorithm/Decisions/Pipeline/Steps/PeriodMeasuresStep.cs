using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Quality;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Returns;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Steps;

public static class PeriodMeasuresStep
{
    public static IReadOnlyList<PeriodMeasures> Run(PreparedQuarters prepared)
    {
        var quarters = prepared.Quarters;
        var measures = new List<PeriodMeasures>(quarters.Count);
        for (var i = 0; i < quarters.Count; i++)
        {
            if (quarters[i].PublishedAt is not { } published)
            {
                continue;
            }

            measures.Add(new PeriodMeasures(
                quarters[i].Label,
                quarters[i].Period,
                published,
                i,
                Roic.At(quarters, i, prepared.MinoritiesInsideEquity).OkValue,
                Roiic.WithFallbackLag(quarters, i, prepared.MinoritiesInsideEquity).Value,
                DirectionTest.CriteriaMet(quarters, i),
                TrailingTwelveMonths.Sum(quarters, i, x => x.NetProfit),
                prepared.Shares.AdjustedShares[i],
                prepared.Shares.Bases[i]));
        }

        return measures;
    }
}
