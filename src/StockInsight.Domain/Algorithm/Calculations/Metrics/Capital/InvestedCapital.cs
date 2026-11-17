using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Capital;

public static class InvestedCapital
{
    public static Measured<double> At(IReadOnlyList<Quarter> quarters, int periodIndex, bool minoritiesInsideEquity)
    {
        if (quarters.At(periodIndex) is not { } quarter)
        {
            return Measured<double>.Missing("okres poza szeregiem");
        }

        if (quarter.Equity is not { } equity || quarter.Cash is null)
        {
            return Measured<double>.Missing("brak kapitału własnego lub gotówki");
        }

        var minorities = MinorityInterests.At(quarters, periodIndex, minoritiesInsideEquity);
        var debt = Debt.At(quarters, periodIndex).Amount;
        return Measured<double>.Ok(equity + minorities + debt);
    }

    public static (Measured<double> Now, double? YearAgo, double? Average) WithAverage(
        IReadOnlyList<Quarter> quarters, int periodIndex, bool minoritiesInsideEquity)
    {
        var now = At(quarters, periodIndex, minoritiesInsideEquity);
        var yearAgo = periodIndex >= 4 ? At(quarters, periodIndex - 4, minoritiesInsideEquity).Value : now.Value;
        var average = now.Value is { } current ? (current + (yearAgo ?? current)) / 2 : (double?)null;
        return (now, yearAgo, average);
    }
}
