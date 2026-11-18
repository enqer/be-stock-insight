using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Capital.Enums;
using StockInsight.Domain.Algorithm.Calculations.Metrics.Capital.Models;
using StockInsight.Domain.Algorithm.Calculations.Numerics;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Capital;

public static class Debt
{
    public static DebtValue At(IReadOnlyList<Quarter> quarters, int periodIndex)
    {
        var quarter = quarters[periodIndex];
        var reportedParts = ReportedDebtParts(quarter);
        var breakdownTotal = Statistics.SumInOrder(reportedParts);
        if (breakdownTotal > 0)
        {
            return new DebtValue(breakdownTotal, DebtSource.Breakdown);
        }

        if (quarter.NonCurrentLiabilities is { } nonCurrentLiabilities && nonCurrentLiabilities > 0)
        {
            return new DebtValue(nonCurrentLiabilities, DebtSource.Aggregate);
        }

        var breakdownReportedButNotPositive = reportedParts.Count > 0;
        if (breakdownReportedButNotPositive)
        {
            return new DebtValue(breakdownTotal, DebtSource.Breakdown);
        }

        return new DebtValue(0.0, DebtSource.None);
    }

    private static List<double> ReportedDebtParts(Quarter quarter) =>
    [
        .. new[]
        {
            quarter.LongTermBorrowings,
            quarter.ShortTermBorrowings,
            quarter.LongTermLeaseLiabilities,
            quarter.ShortTermLeaseLiabilities,
            quarter.LongTermBonds,
            quarter.ShortTermBonds,
        }.OfType<double>(),
    ];
}
