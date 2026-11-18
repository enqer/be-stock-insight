using StockInsight.Domain.Aggregates.Macro.Enums;

namespace StockInsight.Features.Macro.GetMacroIndicators.Models;

public sealed class MacroIndicatorRow
{
    public const string Sql = """
        SELECT
            indicator.id,
            indicator.country,
            indicator.code,
            indicator.name,
            indicator.kind,
            indicator.unit,
            latest_version.period,
            latest_version.value
        FROM macro_indicators indicator
        LEFT JOIN LATERAL (
            SELECT DISTINCT ON (macro_value.period) macro_value.period, macro_value.value
            FROM macro_indicator_values macro_value
            WHERE macro_value.indicator_id = indicator.id
            ORDER BY macro_value.period, macro_value.fetched_at DESC
        ) latest_version ON true
        WHERE indicator.country = @Country
        ORDER BY indicator.code, latest_version.period
        """;

    public Guid Id { get; init; }
    public string Country { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string? Name { get; init; }
    public MacroIndicatorKind? Kind { get; init; }
    public MacroUnit? Unit { get; init; }
    public DateOnly? Period { get; init; }
    public double? Value { get; init; }
}
