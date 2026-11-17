using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;

namespace StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;

public static class QuarterLookup
{
    public static Quarter? At(this IReadOnlyList<Quarter> quarters, int periodIndex) => periodIndex >= 0 && periodIndex < quarters.Count ? quarters[periodIndex] : null;
}
