using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;

namespace StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;

public static class TrailingTwelveMonths
{
    public const int Quarters = 4;

    public static double? Sum(IReadOnlyList<Quarter> quarters, int periodIndex, Func<Quarter, double?> item)
    {
        if (periodIndex < Quarters - 1 || periodIndex >= quarters.Count)
        {
            return null;
        }

        var sum = 0.0;
        for (var index = periodIndex - Quarters + 1; index <= periodIndex; index++)
        {
            if (item(quarters[index]) is not { } value)
            {
                return null;
            }

            sum += value;
        }

        return sum;
    }
}
