namespace StockInsight.Domain.Calculations.Numerics;

public static class Statistics
{
    public static double Median(IReadOnlyCollection<double> values)
    {
        if (values.Count == 0)
        {
            throw new ArgumentException("median of an empty set", nameof(values));
        }

        var sorted = values.ToArray();
        Array.Sort(sorted);
        var mid = sorted.Length / 2;
        return sorted.Length % 2 == 1 ? sorted[mid] : (sorted[mid - 1] + sorted[mid]) / 2;
    }

    public static double SumInOrder(IEnumerable<double> values)
    {
        var sum = 0.0;
        foreach (var v in values)
        {
            sum += v;
        }

        return sum;
    }
}
