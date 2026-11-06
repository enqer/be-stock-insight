namespace StockInsight.Domain.Calculations.Inputs.Market;

public sealed class PriceSeries
{
    public PriceSeries(IEnumerable<PricePoint> points)
    {
        Points = [.. points.OrderBy(p => p.Date).ThenBy(p => p.Close)];
        foreach (var point in Points)
        {
            if (!double.IsFinite(point.Close) || point.Close <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(points), point, "close must be a positive finite number");
            }
        }
    }

    public static PriceSeries Empty { get; } = new([]);

    public IReadOnlyList<PricePoint> Points { get; }

    public int Count => Points.Count;

    public bool IsEmpty => Points.Count == 0;

    public PricePoint? LastOnOrBefore(DateOnly date)
    {
        var after = FirstIndexWhere(d => d > date);
        return after > 0 ? Points[after - 1] : null;
    }

    public PricePoint? FirstOnOrAfter(DateOnly date)
    {
        var index = FirstIndexWhere(d => d >= date);
        return index < Points.Count ? Points[index] : null;
    }

    private int FirstIndexWhere(Func<DateOnly, bool> condition)
    {
        int lo = 0, hi = Points.Count;
        while (lo < hi)
        {
            var mid = (lo + hi) / 2;
            if (condition(Points[mid].Date))
            {
                hi = mid;
            }
            else
            {
                lo = mid + 1;
            }
        }

        return lo;
    }
}
