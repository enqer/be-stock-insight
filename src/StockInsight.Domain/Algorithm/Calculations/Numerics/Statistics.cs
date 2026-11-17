namespace StockInsight.Domain.Algorithm.Calculations.Numerics;

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
        var middle = sorted.Length / 2;
        return sorted.Length % 2 == 1 ? sorted[middle] : (sorted[middle - 1] + sorted[middle]) / 2;
    }

    public static double SumInOrder(IEnumerable<double> values)
    {
        var sum = 0.0;
        foreach (var value in values)
        {
            sum += value;
        }

        return sum;
    }
}
