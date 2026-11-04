namespace StockInsight.Domain.Aggregates.Companies;

public abstract class Statement
{
    public Guid CompanyId { get; protected init; }
    public DateOnly Period { get; protected init; }
    public DateOnly FetchedAt { get; protected init; }
    public DateOnly? PublishedAt { get; protected init; }
    public double? OtherOperatingIncome { get; protected init; }
    public double? OtherOperatingExpenses { get; protected init; }
    public double? ProfitBeforeTax { get; protected init; }
    public double? DiscontinuedOperations { get; protected init; }
    public double? NetProfit { get; protected init; }
    public double? OperatingCashFlow { get; protected init; }
    public double? Depreciation { get; protected init; }
    public double? Capex { get; protected init; }
    public double? Dividends { get; protected init; }
    public double? Equity { get; protected init; }
    public double? MinorityInterests { get; protected init; }
    public double? Shares { get; protected init; }
    public double? ShareIssue { get; protected init; }
    public double? TotalAssets { get; protected init; }

    public abstract bool HasSameContent(Statement other);

    protected bool HasSameCommonContent(Statement other) =>
        PublishedAt == other.PublishedAt
        && OtherOperatingIncome == other.OtherOperatingIncome
        && OtherOperatingExpenses == other.OtherOperatingExpenses
        && ProfitBeforeTax == other.ProfitBeforeTax
        && DiscontinuedOperations == other.DiscontinuedOperations
        && NetProfit == other.NetProfit
        && OperatingCashFlow == other.OperatingCashFlow
        && Depreciation == other.Depreciation
        && Capex == other.Capex
        && Dividends == other.Dividends
        && Equity == other.Equity
        && MinorityInterests == other.MinorityInterests
        && Shares == other.Shares
        && ShareIssue == other.ShareIssue
        && TotalAssets == other.TotalAssets;
}
