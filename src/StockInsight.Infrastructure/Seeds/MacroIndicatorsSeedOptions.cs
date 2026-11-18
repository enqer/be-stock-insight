namespace StockInsight.Infrastructure.Seeds;

public sealed class MacroIndicatorsSeedOptions
{
    public const string Section = "MacroIndicatorsSeed";

    public string Directory { get; set; } = string.Empty;

    public string[] Countries { get; set; } = [];
}
