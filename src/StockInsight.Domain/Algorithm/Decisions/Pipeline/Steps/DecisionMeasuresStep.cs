using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Decisions.Measures;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Steps;

public static class DecisionMeasuresStep
{
    public static IReadOnlyList<DecisionPoint> Run(
        PreparedQuarters prepared, IReadOnlyList<PeriodMeasures> periodsByPublication, PriceSeries prices, PriceSeries indexPrices, RiskFreeRate riskFreeRate)
    {
        var quarters = prepared.Quarters;
        var series = new List<DecisionPoint>(periodsByPublication.Count);
        for (var position = 0; position < periodsByPublication.Count; position++)
        {
            var period = periodsByPublication[position];
            var fourDecisionsEarlier = periodsByPublication[Math.Max(0, position - 4)];
            var capital = CapitalMeasures.At(quarters, period.PeriodIndex, prepared.MinoritiesInsideEquity);

            series.Add(new DecisionPoint
            {
                PeriodLabel = period.PeriodLabel,
                PeriodEnd = period.PeriodEnd,
                PublicationDate = period.PublicationDate,
                PeriodIndex = period.PeriodIndex,
                Returns = ReturnMeasures.At(quarters, period, capital.InvestedCapital > 0 ? capital.AverageInvestedCapital : null),
                Capital = capital,
                CostOfCapital = CostOfCapitalMeasures.At(quarters, period.PeriodIndex, period.PublicationDate, prices, indexPrices, riskFreeRate),
                Earnings = EarningsMeasures.At(quarters, period),
                Quality = QualityMeasures.At(quarters, prepared.MinoritiesInsideEquity, period, fourDecisionsEarlier),
            });
        }

        return series;
    }
}
