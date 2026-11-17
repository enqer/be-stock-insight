using System.Globalization;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Capital;
using StockInsight.Domain.Algorithm.Calculations.Numerics;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Quality;

public static class CashConversion
{
    public const int WindowQuarters = 12;
    public const int MinObservations = 8;
    public const double MinProfitShareOfCapital = 0.02;

    public static Measured<double> At(IReadOnlyList<Quarter> quarters, int periodIndex, bool minoritiesInsideEquity)
    {
        var ratios = new List<double>();
        for (var index = Math.Max(0, periodIndex - (WindowQuarters - 1)); index <= periodIndex; index++)
        {
            var capital = InvestedCapital.At(quarters, index, minoritiesInsideEquity).Value;
            var scale = capital > 0 ? capital : null;
            var ratio = SignificantRatio.Of(
                TrailingTwelveMonths.Sum(quarters, index, x => x.OperatingCashFlow),
                TrailingTwelveMonths.Sum(quarters, index, x => x.NetProfit),
                scale,
                MinProfitShareOfCapital);
            if (ratio is { } knownRatio)
            {
                ratios.Add(knownRatio);
            }
        }

        return ratios.Count >= MinObservations
            ? Measured<double>.Ok(Statistics.Median(ratios))
            : Measured<double>.Missing(
                string.Create(CultureInfo.InvariantCulture, $"tylko {ratios.Count} odczytów z {MinObservations} wymaganych"));
    }
}
