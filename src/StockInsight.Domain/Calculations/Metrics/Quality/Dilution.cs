using StockInsight.Domain.Calculations.Metrics.Quality.Enums;

namespace StockInsight.Domain.Calculations.Metrics.Quality;

public static class Dilution
{
    public static double? YearOverYear(double? sharesNow, ShareBase baseNow, double? sharesBefore, ShareBase baseBefore)
    {
        if (baseNow == ShareBase.Ambiguous || baseBefore == ShareBase.Ambiguous)
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
