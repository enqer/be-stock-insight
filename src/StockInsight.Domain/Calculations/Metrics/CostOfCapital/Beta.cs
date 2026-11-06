using StockInsight.Domain.Calculations.Inputs.Market;
using StockInsight.Domain.Calculations.Numerics;

namespace StockInsight.Domain.Calculations.Metrics.CostOfCapital;

public static class Beta
{
    public const int WindowYears = 5;
    public const int MinObservations = 36;
    public const double Floor = 0.3;
    public const double Ceiling = 2.5;

    public const double Fallback = 1.0;

    public static double? Regression(PriceSeries company, PriceSeries index, DateOnly asOf)
    {
        var a = LastCloseOfMonth(company, asOf);
        var b = LastCloseOfMonth(index, asOf);

        var months = a.Keys
            .Where(b.ContainsKey)
            .Where(m => (asOf.Year - m.Year) * 12 + (asOf.Month - m.Month) <= WindowYears * 12)
            .Order()
            .ToArray();
        if (months.Length < MinObservations)
        {
            return null;
        }

        var y = new double[months.Length - 1];
        var x = new double[months.Length - 1];
        for (var j = 1; j < months.Length; j++)
        {
            y[j - 1] = a[months[j]] / a[months[j - 1]] - 1;
            x[j - 1] = b[months[j]] / b[months[j - 1]] - 1;
        }

        var meanX = Statistics.SumInOrder(x) / x.Length;
        var meanY = Statistics.SumInOrder(y) / y.Length;
        double variance = 0, covariance = 0;
        for (var j = 0; j < x.Length; j++)
        {
            variance += (x[j] - meanX) * (x[j] - meanX);
            covariance += (y[j] - meanY) * (x[j] - meanX);
        }

        return variance <= 0 ? null : covariance / variance;
    }

    public static double Unlever(double levered, double windowLeverage) =>
        Math.Clamp(levered / (1 + (1 - MarketConstants.TaxRate) * Math.Max(0.0, windowLeverage)), Floor, Ceiling);

    public static double Relever(double unlevered, double currentLeverage) =>
        unlevered * (1 + (1 - MarketConstants.TaxRate) * currentLeverage);

    private static Dictionary<(int Year, int Month), double> LastCloseOfMonth(PriceSeries series, DateOnly asOf)
    {
        var map = new Dictionary<(int Year, int Month), double>();
        foreach (var p in series.Points)
        {
            if (p.Date <= asOf)
            {
                map[(p.Date.Year, p.Date.Month)] = p.Close;
            }
        }

        return map;
    }
}
