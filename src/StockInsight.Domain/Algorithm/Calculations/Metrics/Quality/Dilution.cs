using StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.Enums;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Quality;

public static class Dilution
{
    public static double? YearOverYear(double? sharesNow, ShareBase baseNow, double? sharesBefore, ShareBase baseBefore)
    {
        var shareBaseAmbiguous = baseNow == ShareBase.Ambiguous || baseBefore == ShareBase.Ambiguous;
        if (shareBaseAmbiguous)
        {
            return null;
        }

        if (sharesNow is not { } now || now == 0 || sharesBefore is not { } before || before == 0)
        {
            return null;
        }

        return now / before - 1;
    }
}
