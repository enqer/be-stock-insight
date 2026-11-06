using System.Globalization;
using StockInsight.Domain.Calculations.Inputs.Market;
using StockInsight.Domain.Calculations.Inputs.Statements;
using StockInsight.Domain.Calculations.Metrics.Capital;
using StockInsight.Domain.Calculations.Metrics.Enums;

namespace StockInsight.Domain.Calculations.Metrics.Returns;

public static class Roiic
{
    public const int LagQuarters = 12;
    public const int FallbackLagQuarters = 8;
    public const double MinCapitalIncreaseShareOfBase = 0.10;
    public const double ExtrapolationCeiling = 0.40;

    public static Measured<double> At(IReadOnlyList<Quarter> quarters, int t, int lag, bool minoritiesInsideEquity)
    {
        if (t - lag < 3)
        {
            return Measured<double>.Missing(string.Create(CultureInfo.InvariantCulture, $"potrzebne {lag + 4} kwartały historii"));
        }

        var nopatNow = Nopat(quarters, t);
        var nopatBase = Nopat(quarters, t - lag);
        var capitalNow = InvestedCapital.At(quarters, t, minoritiesInsideEquity).Value;
        var capitalBase = InvestedCapital.At(quarters, t - lag, minoritiesInsideEquity).Value;
        if (nopatNow is null || nopatBase is null || capitalNow is null || capitalBase is null)
        {
            return Measured<double>.Missing("luka w szeregu");
        }

        var deltaNopat = nopatNow.Value - nopatBase.Value;
        var deltaCapital = capitalNow.Value - capitalBase.Value;

        if (deltaCapital <= 0)
        {
            return Measured<double>.Unreliable("kapitał zainwestowany nie wzrósł");
        }

        if (deltaCapital < MinCapitalIncreaseShareOfBase * Math.Abs(capitalBase.Value))
        {
            return Measured<double>.Unreliable("przyrost kapitału poniżej 10% bazy");
        }

        var roiic = deltaNopat / deltaCapital;
        return roiic > ExtrapolationCeiling
            ? Measured<double>.OutOfRange(roiic, "ROIIC powyżej 40% — nie do ekstrapolacji")
            : Measured<double>.Ok(roiic);
    }

    public static Measured<double> WithFallbackLag(IReadOnlyList<Quarter> quarters, int t, bool minoritiesInsideEquity)
    {
        var primary = At(quarters, t, LagQuarters, minoritiesInsideEquity);
        if (primary.Status is MeasureStatus.Ok or MeasureStatus.OutOfRange)
        {
            return primary;
        }

        var fallback = At(quarters, t, FallbackLagQuarters, minoritiesInsideEquity);
        if (fallback.Status is MeasureStatus.Ok or MeasureStatus.OutOfRange)
        {
            return fallback;
        }

        return primary.Status == MeasureStatus.Missing ? fallback : primary;
    }

    private static double? Nopat(IReadOnlyList<Quarter> quarters, int t) =>
        TrailingTwelveMonths.Sum(quarters, t, q => q.Ebit) * (1 - MarketConstants.TaxRate);
}
