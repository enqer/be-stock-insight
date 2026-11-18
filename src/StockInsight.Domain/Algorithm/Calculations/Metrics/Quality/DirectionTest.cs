using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Quality;

public static class DirectionTest
{
    public const double MarginTolerance = -0.005;

    public static int? CriteriaMet(IReadOnlyList<Quarter> quarters, int periodIndex)
    {
        var yearEarlier = periodIndex - TrailingTwelveMonths.Quarters;
        var revenueNow = TrailingTwelveMonths.Sum(quarters, periodIndex, x => x.Revenue);
        var revenueBefore = TrailingTwelveMonths.Sum(quarters, yearEarlier, x => x.Revenue);
        var incomeNow = ContinuingNetIncome(quarters, periodIndex);
        var incomeBefore = ContinuingNetIncome(quarters, yearEarlier);
        var (profitNow, profitBefore) = ComparableProfit(quarters, periodIndex, yearEarlier);

        if (revenueNow is not { } revenue || revenueBefore is not { } previousRevenue || incomeNow is not { } income
            || incomeBefore is not { } previousIncome || profitNow is not { } profit || profitBefore is not { } previousProfit
            || previousRevenue <= 0 || previousIncome <= 0)
        {
            return null;
        }

        var revenueGrowth = revenue / previousRevenue - 1;
        var incomeGrowth = income / previousIncome - 1;
        var marginChange = profit / revenue - previousProfit / previousRevenue;

        var revenueGrew = revenueGrowth > 0;
        var marginHeld = marginChange >= MarginTolerance;
        var incomeKeptPaceWithRevenue = incomeGrowth >= revenueGrowth;
        return new[] { revenueGrew, marginHeld, incomeKeptPaceWithRevenue }.Count(x => x);
    }

    private static (double? Now, double? Before) ComparableProfit(IReadOnlyList<Quarter> quarters, int periodIndex, int yearEarlier)
    {
        var grossProfitNow = TrailingTwelveMonths.Sum(quarters, periodIndex, x => x.GrossProfitOnSales);
        var grossProfitBefore = TrailingTwelveMonths.Sum(quarters, yearEarlier, x => x.GrossProfitOnSales);
        if (grossProfitNow is not null && grossProfitBefore is not null)
        {
            return (grossProfitNow, grossProfitBefore);
        }

        var profitOnSalesNow = TrailingTwelveMonths.Sum(quarters, periodIndex, x => x.ProfitOnSales);
        var profitOnSalesBefore = TrailingTwelveMonths.Sum(quarters, yearEarlier, x => x.ProfitOnSales);
        return (profitOnSalesNow, profitOnSalesBefore);
    }

    private static double? ContinuingNetIncome(IReadOnlyList<Quarter> quarters, int periodIndex) =>
        TrailingTwelveMonths.Sum(quarters, periodIndex, x => x.NetProfit - (x.DiscontinuedOperations ?? 0.0));
}
