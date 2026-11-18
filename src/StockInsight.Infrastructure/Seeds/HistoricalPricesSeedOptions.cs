namespace StockInsight.Infrastructure.Seeds;

public sealed class HistoricalPricesSeedOptions
{
    public const string Section = "HistoricalPricesSeed";

    public string StocksDirectory { get; set; } = string.Empty;

    public string IndicesDirectory { get; set; } = string.Empty;
}
