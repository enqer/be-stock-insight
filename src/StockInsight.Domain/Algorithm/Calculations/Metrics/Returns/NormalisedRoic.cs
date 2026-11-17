using System.Globalization;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Numerics;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Returns;

public static class NormalisedRoic
{
    public const int MinMarginObservations = 8;

    public static Measured<double> At(IReadOnlyList<Quarter> quarters, int periodIndex, int cycleYears, double? averageCapital)
    {
        if (TrailingTwelveMonths.Sum(quarters, periodIndex, x => x.Revenue) is not { } revenue)
        {
            return Measured<double>.Missing("brak przychodów za 12 miesięcy");
        }

        if (averageCapital is not { } capital || capital <= 0)
        {
            return Measured<double>.Missing("brak dodatniego średniego kapitału");
        }

        var margins = new List<double>();
        for (var index = Math.Max(4, periodIndex - 4 * cycleYears + 1); index <= periodIndex; index++)
        {
            if (TrailingTwelveMonths.Sum(quarters, index, x => x.Ebit) is { } ebit
                && TrailingTwelveMonths.Sum(quarters, index, x => x.Revenue) is { } periodRevenue && periodRevenue > 0)
            {
                margins.Add(ebit / periodRevenue);
            }
        }

        if (margins.Count < MinMarginObservations)
        {
            return Measured<double>.Missing(
                string.Create(CultureInfo.InvariantCulture, $"za mało obserwacji marży ({margins.Count} < {MinMarginObservations})"));
        }

        return Measured<double>.Ok(revenue * Statistics.Median(margins) * (1 - MarketConstants.TaxRate) / capital);
    }
}
