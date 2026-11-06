namespace StockInsight.Domain.Calculations.Inputs.Statements;

public static class QuarterLookup
{
    // Okres spoza szeregu to brak, nie wyjątek — na tym stoją porównania t−4 na początku szeregu.
    public static Quarter? At(this IReadOnlyList<Quarter> quarters, int t) => t >= 0 && t < quarters.Count ? quarters[t] : null;
}
