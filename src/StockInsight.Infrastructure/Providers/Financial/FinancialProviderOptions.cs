namespace StockInsight.Infrastructure.Providers.Financial;

public sealed class FinancialProviderOptions
{
    public const string Section = "FinancialProvider";

    public string CompaniesUrl { get; set; } = string.Empty;

    public string StatementUrl { get; set; } = string.Empty;
}
