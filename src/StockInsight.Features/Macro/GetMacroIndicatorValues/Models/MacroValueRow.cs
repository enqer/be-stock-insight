namespace StockInsight.Features.Macro.GetMacroIndicatorValues.Models;

public sealed class MacroValueRow
{
    public const string Sql = """
        SELECT DISTINCT ON (macro_value.period) macro_value.period, macro_value.value
        FROM macro_indicator_values macro_value
        WHERE macro_value.indicator_id = @IndicatorId
        ORDER BY macro_value.period, macro_value.fetched_at DESC
        """;

    public DateOnly Period { get; init; }
    public double Value { get; init; }
}
