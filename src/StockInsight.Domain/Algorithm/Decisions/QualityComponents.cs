namespace StockInsight.Domain.Algorithm.Decisions;

public sealed record QualityComponents(double Headroom, double? Roiic, double? CashConversion, int? DirectionCriteriaMet)
{
    public static QualityComponents? From(DecisionPoint point)
    {
        if (point.Returns.Roic is not { } roic || point.CostOfCapital.Wacc?.Rate is not { } wacc)
        {
            return null;
        }

        return new QualityComponents(roic / wacc - 1, point.Returns.Roiic, point.Quality.CashConversion, point.Quality.DirectionCriteriaMet);
    }
}
