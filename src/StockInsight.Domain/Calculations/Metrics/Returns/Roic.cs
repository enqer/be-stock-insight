using System.Globalization;
using StockInsight.Domain.Calculations.Inputs.Market;
using StockInsight.Domain.Calculations.Inputs.Statements;
using StockInsight.Domain.Calculations.Metrics.Capital;

namespace StockInsight.Domain.Calculations.Metrics.Returns;

public static class Roic
{
    public const double CapitalFloorShareOfAssets = 0.10;

    public static Measured<double> At(IReadOnlyList<Quarter> quarters, int t, bool minoritiesInsideEquity)
    {
        if (TrailingTwelveMonths.Sum(quarters, t, q => q.Ebit) is not { } ebit)
        {
            return Measured<double>.Missing("brak EBIT za 12 miesięcy");
        }

        var (now, _, averageCapital) = InvestedCapital.WithAverage(quarters, t, minoritiesInsideEquity);
        if (averageCapital is not { } average)
        {
            return Measured<double>.Missing(now.Reason);
        }

        if (average <= 0)
        {
            return Measured<double>.Unreliable(
                string.Create(CultureInfo.InvariantCulture, $"średni kapitał zainwestowany <= 0 ({average:N0})"));
        }

        if (quarters.At(t)?.TotalAssets is { } assets && average < assets * CapitalFloorShareOfAssets)
        {
            return Measured<double>.Unreliable(
                string.Create(CultureInfo.InvariantCulture, $"średni kapitał {average:N0} < 10% sumy bilansowej"));
        }

        return Measured<double>.Ok(ebit * (1 - MarketConstants.TaxRate) / average);
    }
}
