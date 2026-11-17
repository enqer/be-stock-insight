using StockInsight.Domain.Algorithm.Calculations.Metrics.Quality.Enums;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Quality;

public static class Dilution
{
    public static double? YearOverYear(double? sharesNow, ShareBase baseNow, double? sharesBefore, ShareBase baseBefore) =>
        baseNow == ShareBase.Ambiguous || baseBefore == ShareBase.Ambiguous
        || sharesNow is not { } now || now == 0 || sharesBefore is not { } before || before == 0
            ? null
            : now / before - 1;
}
