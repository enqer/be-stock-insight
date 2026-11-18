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
        var parts = new[] { quarter.LongTermBorrowings, quarter.ShortTermBorrowings, quarter.LongTermLeaseLiabilities, quarter.ShortTermLeaseLiabilities, quarter.LongTermBonds, quarter.ShortTermBonds }.OfType<double>().ToList();
        var breakdown = Statistics.SumInOrder(parts);

        if (breakdown > 0)
        {
            return new DebtValue(breakdown, DebtSource.Breakdown);
        }

        if (quarter.NonCurrentLiabilities is { } nonCurrentLiabilities && nonCurrentLiabilities > 0)
        {
            return new DebtValue(nonCurrentLiabilities, DebtSource.Aggregate);
        }

        return parts.Count > 0
            ? new DebtValue(breakdown, DebtSource.Breakdown)
            : new DebtValue(0.0, DebtSource.None);
    }
}
