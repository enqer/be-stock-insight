namespace StockInsight.Domain.Calculations.Inputs.Statements;

public static class TrailingTwelveMonths
{
    public const int Quarters = 4;

    public static double? Sum(IReadOnlyList<Quarter> quarters, int index, Func<Quarter, double?> item)
    {
        if (index < Quarters - 1 || index >= quarters.Count)
        {
            return null;
        }

        var sum = 0.0;
        for (var k = index - Quarters + 1; k <= index; k++)
        {
            if (item(quarters[k]) is not { } v)
            {
                // Brak choć jednej wartości to brak, nie suma częściowa.
                return null;
            }

            sum += v;
        }

        return sum;
    }
}
