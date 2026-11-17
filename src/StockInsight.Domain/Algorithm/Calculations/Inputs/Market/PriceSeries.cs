using StockInsight.Domain.Algorithm.Calculations.Inputs.Market.Models;

namespace StockInsight.Domain.Algorithm.Calculations.Inputs.Market;

public sealed class PriceSeries
{
    public PriceSeries(IEnumerable<PricePoint> points)
    {
        Points = [.. points.OrderBy(x => x.Date).ThenBy(x => x.Close)];
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

    public bool IsEmpty => Points.Count == 0;

    public PricePoint? LastOnOrBefore(DateOnly date)
    {
        var after = FirstIndexWhere(x => x > date);
        return after > 0 ? Points[after - 1] : null;
    }

    public PricePoint? FirstOnOrAfter(DateOnly date)
    {
        var index = FirstIndexWhere(x => x >= date);
        return index < Points.Count ? Points[index] : null;
    }

    private int FirstIndexWhere(Func<DateOnly, bool> condition)
    {
        var low = 0;
        var high = Points.Count;
        while (low < high)
        {
            var middle = (low + high) / 2;
            if (condition(Points[middle].Date))
            {
                high = middle;
            }
            else
            {
                low = middle + 1;
            }
        }

        return low;
    }
}
