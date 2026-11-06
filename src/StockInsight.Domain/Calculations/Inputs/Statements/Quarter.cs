using System.Globalization;
using StockInsight.Domain.Aggregates.Companies;

namespace StockInsight.Domain.Calculations.Inputs.Statements;

public sealed record Quarter
{
    public required DateOnly Period { get; init; }

    public DateOnly? PublishedAt { get; init; }

    public double? Revenue { get; init; }

    public double? GrossProfitOnSales { get; init; }

    public double? ProfitOnSales { get; init; }

    public double? Ebit { get; init; }

    public double? FinancialExpenses { get; init; }

    public double? NetProfit { get; init; }

    public double? DiscontinuedOperations { get; init; }

    public double? OperatingCashFlow { get; init; }

    public double? InvestingCashFlow { get; init; }

    public double? FinancingCashFlow { get; init; }

    public double? TotalCashFlow { get; init; }

    public double? Capex { get; init; }

    public double? LeasePayments { get; init; }

    public double? Dividends { get; init; }

    public double? Cash { get; init; }

    public double? TotalAssets { get; init; }

    public double? Equity { get; init; }

    public double? MinorityInterests { get; init; }

    public double? NonCurrentLiabilities { get; init; }

    public double? CurrentLiabilities { get; init; }

    public double? LongTermBorrowings { get; init; }

    public double? ShortTermBorrowings { get; init; }

    public double? LongTermLeaseLiabilities { get; init; }

    public double? ShortTermLeaseLiabilities { get; init; }

    public double? LongTermBonds { get; init; }

    public double? ShortTermBonds { get; init; }

    public double? Shares { get; init; }

    public double? ShareCapital { get; init; }

    public double? ShareIssue { get; init; }

    public string Label => string.Create(CultureInfo.InvariantCulture, $"Q{Period.Month / 3} {Period.Year}");

    public static Quarter From(StandardStatement statement) => new()
    {
        Period = statement.Period,
        PublishedAt = statement.PublishedAt,
        Revenue = NullIfZero(statement.Revenue),
        GrossProfitOnSales = statement.GrossProfitOnSales,
        ProfitOnSales = statement.ProfitOnSales,
        Ebit = statement.Ebit,
        FinancialExpenses = statement.FinancialExpenses,
        NetProfit = statement.NetProfit,
        DiscontinuedOperations = statement.DiscontinuedOperations,
        OperatingCashFlow = statement.OperatingCashFlow,
        InvestingCashFlow = statement.InvestingCashFlow,
        FinancingCashFlow = statement.FinancingCashFlow,
        TotalCashFlow = statement.TotalCashFlow,
        Capex = statement.Capex,
        LeasePayments = statement.LeasePayments,
        Dividends = statement.Dividends,
        Cash = statement.Cash,
        TotalAssets = NullIfZero(statement.TotalAssets ?? statement.NonCurrentAssets + statement.CurrentAssets),
        Equity = NullIfZero(statement.Equity),
        MinorityInterests = statement.MinorityInterests,
        NonCurrentLiabilities = statement.NonCurrentLiabilities,
        CurrentLiabilities = statement.CurrentLiabilities,
        LongTermBorrowings = statement.LongTermBorrowings,
        ShortTermBorrowings = statement.ShortTermBorrowings,
        LongTermLeaseLiabilities = statement.LongTermLeaseLiabilities,
        ShortTermLeaseLiabilities = statement.ShortTermLeaseLiabilities,
        LongTermBonds = statement.LongTermBonds,
        ShortTermBonds = statement.ShortTermBonds,
        Shares = statement.Shares,
        ShareCapital = statement.ShareCapital,
        ShareIssue = statement.ShareIssue,
    };

    private static double? NullIfZero(double? value) => value == 0 ? null : value;
}
