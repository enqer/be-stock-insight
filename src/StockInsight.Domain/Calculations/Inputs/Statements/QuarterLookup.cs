namespace StockInsight.Domain.Calculations.Inputs.Statements;

public static class QuarterLookup
{
    public static Quarter? At(this IReadOnlyList<Quarter> quarters, int t) => t >= 0 && t < quarters.Count ? quarters[t] : null;
}
