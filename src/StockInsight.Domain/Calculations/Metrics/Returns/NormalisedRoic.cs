using System.Globalization;
using StockInsight.Domain.Calculations.Inputs.Market;
using StockInsight.Domain.Calculations.Inputs.Statements;
using StockInsight.Domain.Calculations.Numerics;

namespace StockInsight.Domain.Calculations.Metrics.Returns;

public static class NormalisedRoic
{
    public const int MinMarginObservations = 8;

    public static Measured<double> At(IReadOnlyList<Quarter> quarters, int t, int cycleYears, double? averageCapital)
    {
        if (TrailingTwelveMonths.Sum(quarters, t, q => q.Revenue) is not { } revenue)
        {
            return Measured<double>.Missing("brak przychodów za 12 miesięcy");
        }

        if (averageCapital is not { } capital || capital <= 0)
        {
            return Measured<double>.Missing("brak dodatniego średniego kapitału");
        }

        var margins = new List<double>();
        for (var k = Math.Max(4, t - 4 * cycleYears + 1); k <= t; k++)
        {
            if (TrailingTwelveMonths.Sum(quarters, k, q => q.Ebit) is { } ebit
                && TrailingTwelveMonths.Sum(quarters, k, q => q.Revenue) is { } rev && rev > 0)
            {
                margins.Add(ebit / rev);
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
