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
    public const double TaxShield = 1 - MarketConstants.TaxRate;

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
        for (var i = 1; i < months.Length; i++)
        {
            companyReturns[i - 1] = companyCloses[months[i]] / companyCloses[months[i - 1]] - 1;
            indexReturns[i - 1] = indexCloses[months[i]] / indexCloses[months[i - 1]] - 1;
        }

        var meanIndexReturn = Statistics.SumInOrder(indexReturns) / indexReturns.Length;
        var meanCompanyReturn = Statistics.SumInOrder(companyReturns) / companyReturns.Length;
        var variance = 0.0;
        var covariance = 0.0;
        for (var i = 0; i < indexReturns.Length; i++)
        {
            variance += (indexReturns[i] - meanIndexReturn) * (indexReturns[i] - meanIndexReturn);
            covariance += (companyReturns[i] - meanCompanyReturn) * (indexReturns[i] - meanIndexReturn);
        }

        return variance <= 0 ? null : covariance / variance;
    }

    public static double Unlever(double levered, double windowLeverage)
    {
        var leverageNotBelowZero = Math.Max(0.0, windowLeverage);
        var unlevered = levered / (1 + TaxShield * leverageNotBelowZero);
        return Math.Clamp(unlevered, Floor, Ceiling);
    }

    public static double Relever(double unlevered, double currentLeverage) =>
        unlevered * (1 + TaxShield * currentLeverage);

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
