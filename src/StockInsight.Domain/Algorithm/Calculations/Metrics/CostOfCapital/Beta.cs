using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Calculations.Numerics;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital;

public static class Beta
{
    public const int WindowYears = 5;
    public const int MinObservations = 36;
    public const double Floor = 0.3;
    public const double Ceiling = 2.5;

    public const double Fallback = 1.0;

    public static double? Regression(PriceSeries companyPrices, PriceSeries indexPrices, DateOnly asOf)
    {
        var companyCloses = LastCloseOfMonth(companyPrices, asOf);
        var indexCloses = LastCloseOfMonth(indexPrices, asOf);

        var months = companyCloses.Keys
            .Where(indexCloses.ContainsKey)
            .Where(x => (asOf.Year - x.Year) * 12 + (asOf.Month - x.Month) <= WindowYears * 12)
            .Order()
            .ToArray();
        if (months.Length < MinObservations)
        {
            return null;
        }

        var companyReturns = new double[months.Length - 1];
        var indexReturns = new double[months.Length - 1];
        for (var month = 1; month < months.Length; month++)
        {
            companyReturns[month - 1] = companyCloses[months[month]] / companyCloses[months[month - 1]] - 1;
            indexReturns[month - 1] = indexCloses[months[month]] / indexCloses[months[month - 1]] - 1;
        }

        var meanIndexReturn = Statistics.SumInOrder(indexReturns) / indexReturns.Length;
        var meanCompanyReturn = Statistics.SumInOrder(companyReturns) / companyReturns.Length;
        var variance = 0.0;
        var covariance = 0.0;
        for (var month = 0; month < indexReturns.Length; month++)
        {
            variance += (indexReturns[month] - meanIndexReturn) * (indexReturns[month] - meanIndexReturn);
            covariance += (companyReturns[month] - meanCompanyReturn) * (indexReturns[month] - meanIndexReturn);
        }

        return variance <= 0 ? null : covariance / variance;
    }

    public static double Unlever(double levered, double windowLeverage) =>
        Math.Clamp(levered / (1 + (1 - MarketConstants.TaxRate) * Math.Max(0.0, windowLeverage)), Floor, Ceiling);

    public static double Relever(double unlevered, double currentLeverage) =>
        unlevered * (1 + (1 - MarketConstants.TaxRate) * currentLeverage);

    private static Dictionary<(int Year, int Month), double> LastCloseOfMonth(PriceSeries series, DateOnly asOf)
    {
        var closes = new Dictionary<(int Year, int Month), double>();
        foreach (var point in series.Points)
        {
            if (point.Date <= asOf)
            {
                closes[(point.Date.Year, point.Date.Month)] = point.Close;
            }
        }

        return closes;
    }
}
