using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Capital;

public static class InvestedCapital
{
    public static Measured<double> At(IReadOnlyList<Quarter> quarters, int periodIndex, bool minoritiesInsideEquity)
    {
        var quarter = quarters[periodIndex];
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
        var hasYearAgoQuarter = periodIndex >= TrailingTwelveMonths.Quarters;
        var yearAgo = hasYearAgoQuarter
            ? At(quarters, periodIndex - TrailingTwelveMonths.Quarters, minoritiesInsideEquity).Value
            : now.Value;
        if (now.Value is not { } current)
        {
            return (now, yearAgo, null);
        }

        var yearAgoOrCurrent = yearAgo ?? current;
        return (now, yearAgo, (current + yearAgoOrCurrent) / 2);
    }
}
