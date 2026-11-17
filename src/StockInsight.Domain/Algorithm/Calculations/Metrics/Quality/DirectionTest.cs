using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Quality;

public static class DirectionTest
{
    public const double MarginTolerance = -0.005;

    public static int? CriteriaMet(IReadOnlyList<Quarter> quarters, int periodIndex)
    {
        var revenueNow = TrailingTwelveMonths.Sum(quarters, periodIndex, x => x.Revenue);
        var revenueBefore = TrailingTwelveMonths.Sum(quarters, periodIndex - 4, x => x.Revenue);
        var incomeNow = ContinuingNetIncome(quarters, periodIndex);
        var incomeBefore = ContinuingNetIncome(quarters, periodIndex - 4);

        var profitNow = TrailingTwelveMonths.Sum(quarters, periodIndex, x => x.GrossProfitOnSales);
        var profitBefore = TrailingTwelveMonths.Sum(quarters, periodIndex - 4, x => x.GrossProfitOnSales);
        if (profitNow is null || profitBefore is null)
        {
            profitNow = TrailingTwelveMonths.Sum(quarters, periodIndex, x => x.ProfitOnSales);
            profitBefore = TrailingTwelveMonths.Sum(quarters, periodIndex - 4, x => x.ProfitOnSales);
        }

        if (revenueNow is not { } revenue || revenueBefore is not { } previousRevenue || incomeNow is not { } income
            || incomeBefore is not { } previousIncome || profitNow is not { } profit || profitBefore is not { } previousProfit
            || previousRevenue <= 0 || previousIncome <= 0)
        {
            return null;
        }

        var revenueGrowth = revenue / previousRevenue - 1;
        var incomeGrowth = income / previousIncome - 1;
        var marginChange = profit / revenue - previousProfit / previousRevenue;

        return (revenueGrowth > 0 ? 1 : 0)
               + (marginChange >= MarginTolerance ? 1 : 0)
               + (incomeGrowth >= revenueGrowth ? 1 : 0);
    }

    private static double? ContinuingNetIncome(IReadOnlyList<Quarter> quarters, int periodIndex) =>
        TrailingTwelveMonths.Sum(quarters, periodIndex, x => x.NetProfit - (x.DiscontinuedOperations ?? 0.0));
}
