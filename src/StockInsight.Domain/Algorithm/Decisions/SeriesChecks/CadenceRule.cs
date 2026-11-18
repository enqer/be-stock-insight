using System.Globalization;
using StockInsight.Domain.Algorithm.Calculations.Numerics;
using StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

namespace StockInsight.Domain.Algorithm.Decisions.SeriesChecks;

public static class CadenceRule
{
    public const double MaxMedianGapDays = 150;

    public static CadenceCheck Check(IReadOnlyList<double> gapDays)
    {
        if (gapDays.Count == 0)
        {
            return new CadenceCheck(MedianGapDays: null, Rejection: "brak szeregu dat publikacji");
        }

        var median = Statistics.Median(gapDays);
        return median > MaxMedianGapDays
            ? new CadenceCheck(median, string.Create(CultureInfo.InvariantCulture, $"raportowanie rzadsze niż kwartalne: mediana odstępu {median:F0} dni"))
            : new CadenceCheck(median, Rejection: null);
    }
}
