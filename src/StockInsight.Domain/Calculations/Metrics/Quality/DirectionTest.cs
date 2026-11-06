using StockInsight.Domain.Calculations.Inputs.Statements;

namespace StockInsight.Domain.Calculations.Metrics.Quality;

public static class DirectionTest
{
    public const double MarginTolerance = -0.005;

    public static int? CriteriaMet(IReadOnlyList<Quarter> quarters, int t)
    {
        var revenueNow = TrailingTwelveMonths.Sum(quarters, t, q => q.Revenue);
        var revenueBefore = TrailingTwelveMonths.Sum(quarters, t - 4, q => q.Revenue);
        var incomeNow = ContinuingNetIncome(quarters, t);
        var incomeBefore = ContinuingNetIncome(quarters, t - 4);

        var profitNow = TrailingTwelveMonths.Sum(quarters, t, q => q.GrossProfitOnSales);
        var profitBefore = TrailingTwelveMonths.Sum(quarters, t - 4, q => q.GrossProfitOnSales);
        if (profitNow is null || profitBefore is null)
        {
            profitNow = TrailingTwelveMonths.Sum(quarters, t, q => q.ProfitOnSales);
            profitBefore = TrailingTwelveMonths.Sum(quarters, t - 4, q => q.ProfitOnSales);
        }

        if (revenueNow is not { } r1 || revenueBefore is not { } r0 || incomeNow is not { } n1
            || incomeBefore is not { } n0 || profitNow is not { } g1 || profitBefore is not { } g0
            || r0 <= 0 || n0 <= 0)
        {
            return null;
        }

        var revenueGrowth = r1 / r0 - 1;
        var incomeGrowth = n1 / n0 - 1;
        var marginChange = g1 / r1 - g0 / r0;

        return (revenueGrowth > 0 ? 1 : 0)
               + (marginChange >= MarginTolerance ? 1 : 0)
               + (incomeGrowth >= revenueGrowth ? 1 : 0);
    }

    private static double? ContinuingNetIncome(IReadOnlyList<Quarter> quarters, int t) =>
        TrailingTwelveMonths.Sum(quarters, t, q => q.NetProfit - (q.DiscontinuedOperations ?? 0.0));
}
