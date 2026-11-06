using System.Globalization;
using StockInsight.Domain.Calculations.Inputs.Statements;
using StockInsight.Domain.Calculations.Metrics.Capital;
using StockInsight.Domain.Calculations.Numerics;

namespace StockInsight.Domain.Calculations.Metrics.Quality;

public static class CashConversion
{
    public const int WindowQuarters = 12;
    public const int MinObservations = 8;
    public const double MinProfitShareOfCapital = 0.02;

    public static Measured<double> At(IReadOnlyList<Quarter> quarters, int t, bool minoritiesInsideEquity)
    {
        var ratios = new List<double>();
        for (var k = Math.Max(0, t - (WindowQuarters - 1)); k <= t; k++)
        {
            var capital = InvestedCapital.At(quarters, k, minoritiesInsideEquity).Value;
            var scale = capital > 0 ? capital : null;
            var ratio = SignificantRatio.Of(
                TrailingTwelveMonths.Sum(quarters, k, q => q.OperatingCashFlow),
                TrailingTwelveMonths.Sum(quarters, k, q => q.NetProfit),
                scale,
                MinProfitShareOfCapital);
            if (ratio is { } r)
            {
                ratios.Add(r);
            }
        }

        return ratios.Count >= MinObservations
            ? Measured<double>.Ok(Statistics.Median(ratios))
            : Measured<double>.Missing(
                string.Create(CultureInfo.InvariantCulture, $"tylko {ratios.Count} odczytów z {MinObservations} wymaganych"));
    }
}
