using System.Globalization;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Capital;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Returns;

public static class Roiic
{
    public const int LagQuarters = 12;
    public const int FallbackLagQuarters = 8;
    public const double MinCapitalIncreaseShareOfBase = 0.10;
    public const double ExtrapolationCeiling = 0.40;

    public static Measured<double> At(IReadOnlyList<Quarter> quarters, int periodIndex, int lagQuarters, bool minoritiesInsideEquity)
    {
        if (periodIndex - lagQuarters < 3)
        {
            return Measured<double>.Missing(string.Create(CultureInfo.InvariantCulture, $"potrzebne {lagQuarters + 4} kwartały historii"));
        }

        if (Nopat(quarters, periodIndex) is not { } nopatNow
            || Nopat(quarters, periodIndex - lagQuarters) is not { } nopatBase
            || InvestedCapital.At(quarters, periodIndex, minoritiesInsideEquity).Value is not { } capitalNow
            || InvestedCapital.At(quarters, periodIndex - lagQuarters, minoritiesInsideEquity).Value is not { } capitalBase)
        {
            return Measured<double>.Missing("luka w szeregu");
        }

        var deltaNopat = nopatNow - nopatBase;
        var deltaCapital = capitalNow - capitalBase;

        if (deltaCapital <= 0)
        {
            return Measured<double>.Unreliable("kapitał zainwestowany nie wzrósł");
        }

        if (deltaCapital < MinCapitalIncreaseShareOfBase * Math.Abs(capitalBase))
        {
            return Measured<double>.Unreliable("przyrost kapitału poniżej 10% bazy");
        }

        var roiic = deltaNopat / deltaCapital;
        return roiic > ExtrapolationCeiling
            ? Measured<double>.OutOfRange(roiic, "ROIIC powyżej 40% — nie do ekstrapolacji")
            : Measured<double>.Ok(roiic);
    }

    public static double? WithFallbackLag(IReadOnlyList<Quarter> quarters, int periodIndex, bool minoritiesInsideEquity) =>
        At(quarters, periodIndex, LagQuarters, minoritiesInsideEquity).Value
        ?? At(quarters, periodIndex, FallbackLagQuarters, minoritiesInsideEquity).Value;

    private static double? Nopat(IReadOnlyList<Quarter> quarters, int periodIndex) =>
        TrailingTwelveMonths.Sum(quarters, periodIndex, x => x.Ebit) * (1 - MarketConstants.TaxRate);
}
