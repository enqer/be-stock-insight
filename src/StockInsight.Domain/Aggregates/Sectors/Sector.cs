using StockInsight.Domain.Aggregates.Common;

namespace StockInsight.Domain.Aggregates.Sectors;

public sealed class Sector : IAggregateRoot
{
    private Sector()
    {
    }

    public Guid Id { get; private init; }
    public string MacroSector { get; private init; } = string.Empty;
    public string Name { get; private init; } = string.Empty;

    public static Sector Create(string macroSector, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(macroSector);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new Sector
        {
            Id = Guid.CreateVersion7(),
            MacroSector = macroSector.Trim(),
            Name = name.Trim(),
        };
    }
}
