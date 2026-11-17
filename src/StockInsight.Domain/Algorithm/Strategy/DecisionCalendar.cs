namespace StockInsight.Domain.Algorithm.Strategy;

public static class DecisionCalendar
{
    private static readonly int[] _months = [2, 5, 8, 11];

    public static IReadOnlyList<DateOnly> Quarterly(DateOnly from, DateOnly to) =>
        [.. Enumerable.Range(from.Year, to.Year - from.Year + 1)
            .SelectMany(x => _months.Select(y => new DateOnly(x, y, 15)))
            .Where(x => x >= from && x <= to)];
}
