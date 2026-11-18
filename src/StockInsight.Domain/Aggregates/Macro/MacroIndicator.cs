using StockInsight.Domain.Aggregates.Common;
using StockInsight.Domain.Aggregates.Macro.Enums;

namespace StockInsight.Domain.Aggregates.Macro;

public sealed class MacroIndicator : IAggregateRoot
{
    private MacroIndicator()
    {
    }

    public Guid Id { get; private init; }

    public string Country { get; private init; } = string.Empty;

    public string Code { get; private init; } = string.Empty;

    public string? Name { get; private set; }

    public MacroIndicatorKind? Kind { get; private set; }

    public MacroUnit? Unit { get; private set; }

    public static MacroIndicator Create(string country, string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(country);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        return new MacroIndicator
        {
            Id = Guid.CreateVersion7(),
            Country = country.Trim().ToUpperInvariant(),
            Code = code.Trim().ToUpperInvariant(),
        };
    }

    public void Describe(string? name, MacroIndicatorKind? kind, MacroUnit? unit)
    {
        Name = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
        Kind = kind;
        Unit = unit;
    }
}
