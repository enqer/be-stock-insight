namespace StockInsight.Domain.Aggregates.Companies;

public sealed class StandardStatement : Statement
{
    private StandardStatement()
    {
    }

    public double? Revenue { get; private init; }
    public double? GrossProfitOnSales { get; private init; }
    public double? SellingExpenses { get; private init; }
    public double? GeneralAndAdministrativeExpenses { get; private init; }
    public double? ProfitOnSales { get; private init; }
    public double? Ebit { get; private init; }
    public double? FinancialIncome { get; private init; }
    public double? FinancialExpenses { get; private init; }
    public double? Ebitda { get; private init; }
    public double? InvestingCashFlow { get; private init; }
    public double? FinancingCashFlow { get; private init; }
    public double? LeasePayments { get; private init; }
    public double? TotalCashFlow { get; private init; }
    public double? NonCurrentAssets { get; private init; }
    public double? CurrentAssets { get; private init; }
    public double? Inventories { get; private init; }
    public double? TradeReceivables { get; private init; }
    public double? OtherCurrentAssets { get; private init; }
    public double? ShortTermInvestments { get; private init; }
    public double? Cash { get; private init; }
    public double? NonCurrentLiabilities { get; private init; }
    public double? CurrentLiabilities { get; private init; }
    public double? LongTermBorrowings { get; private init; }
    public double? ShortTermBorrowings { get; private init; }
    public double? LongTermLeaseLiabilities { get; private init; }
    public double? ShortTermLeaseLiabilities { get; private init; }
    public double? LongTermTradePayables { get; private init; }
    public double? ShortTermTradePayables { get; private init; }
    public double? LongTermBonds { get; private init; }
    public double? ShortTermBonds { get; private init; }
    public double? OtherLongTermLiabilities { get; private init; }
    public double? OtherShortTermLiabilities { get; private init; }
    public double? ShareCapital { get; private init; }

    public static StandardStatement Create(
        Guid companyId,
        DateOnly period,
        DateOnly fetchedAt,
        DateOnly? publishedAt,
        double? revenue,
        double? grossProfitOnSales,
        double? sellingExpenses,
        double? generalAndAdministrativeExpenses,
        double? profitOnSales,
        double? ebit,
        double? financialIncome,
        double? financialExpenses,
        double? ebitda,
        double? investingCashFlow,
        double? financingCashFlow,
        double? leasePayments,
        double? totalCashFlow,
        double? nonCurrentAssets,
        double? currentAssets,
        double? inventories,
        double? tradeReceivables,
        double? otherCurrentAssets,
        double? shortTermInvestments,
        double? cash,
        double? nonCurrentLiabilities,
        double? currentLiabilities,
        double? longTermBorrowings,
        double? shortTermBorrowings,
        double? longTermLeaseLiabilities,
        double? shortTermLeaseLiabilities,
        double? longTermTradePayables,
        double? shortTermTradePayables,
        double? longTermBonds,
        double? shortTermBonds,
        double? otherLongTermLiabilities,
        double? otherShortTermLiabilities,
        double? shareCapital,
        double? otherOperatingIncome,
        double? otherOperatingExpenses,
        double? profitBeforeTax,
        double? discontinuedOperations,
        double? netProfit,
        double? operatingCashFlow,
        double? depreciation,
        double? capex,
        double? dividends,
        double? equity,
        double? minorityInterests,
        double? shares,
        double? shareIssue,
        double? totalAssets)
    {
        return new()
        {
            CompanyId = companyId,
            Period = period,
            FetchedAt = fetchedAt,
            PublishedAt = publishedAt,
            Revenue = revenue,
            GrossProfitOnSales = grossProfitOnSales,
            SellingExpenses = sellingExpenses,
            GeneralAndAdministrativeExpenses = generalAndAdministrativeExpenses,
            ProfitOnSales = profitOnSales,
            Ebit = ebit,
            FinancialIncome = financialIncome,
            FinancialExpenses = financialExpenses,
            Ebitda = ebitda,
            InvestingCashFlow = investingCashFlow,
            FinancingCashFlow = financingCashFlow,
            LeasePayments = leasePayments,
            TotalCashFlow = totalCashFlow,
            NonCurrentAssets = nonCurrentAssets,
            CurrentAssets = currentAssets,
            Inventories = inventories,
            TradeReceivables = tradeReceivables,
            OtherCurrentAssets = otherCurrentAssets,
            ShortTermInvestments = shortTermInvestments,
            Cash = cash,
            NonCurrentLiabilities = nonCurrentLiabilities,
            CurrentLiabilities = currentLiabilities,
            LongTermBorrowings = longTermBorrowings,
            ShortTermBorrowings = shortTermBorrowings,
            LongTermLeaseLiabilities = longTermLeaseLiabilities,
            ShortTermLeaseLiabilities = shortTermLeaseLiabilities,
            LongTermTradePayables = longTermTradePayables,
            ShortTermTradePayables = shortTermTradePayables,
            LongTermBonds = longTermBonds,
            ShortTermBonds = shortTermBonds,
            OtherLongTermLiabilities = otherLongTermLiabilities,
            OtherShortTermLiabilities = otherShortTermLiabilities,
            ShareCapital = shareCapital,
            OtherOperatingIncome = otherOperatingIncome,
            OtherOperatingExpenses = otherOperatingExpenses,
            ProfitBeforeTax = profitBeforeTax,
            DiscontinuedOperations = discontinuedOperations,
            NetProfit = netProfit,
            OperatingCashFlow = operatingCashFlow,
            Depreciation = depreciation,
            Capex = capex,
            Dividends = dividends,
            Equity = equity,
            MinorityInterests = minorityInterests,
            Shares = shares,
            ShareIssue = shareIssue,
            TotalAssets = totalAssets,
        };
    }

    public override bool HasSameContent(Statement other) =>
        other is StandardStatement statement
        && HasSameCommonContent(statement)
        && Revenue == statement.Revenue
        && GrossProfitOnSales == statement.GrossProfitOnSales
        && SellingExpenses == statement.SellingExpenses
        && GeneralAndAdministrativeExpenses == statement.GeneralAndAdministrativeExpenses
        && ProfitOnSales == statement.ProfitOnSales
        && Ebit == statement.Ebit
        && FinancialIncome == statement.FinancialIncome
        && FinancialExpenses == statement.FinancialExpenses
        && Ebitda == statement.Ebitda
        && InvestingCashFlow == statement.InvestingCashFlow
        && FinancingCashFlow == statement.FinancingCashFlow
        && LeasePayments == statement.LeasePayments
        && TotalCashFlow == statement.TotalCashFlow
        && NonCurrentAssets == statement.NonCurrentAssets
        && CurrentAssets == statement.CurrentAssets
        && Inventories == statement.Inventories
        && TradeReceivables == statement.TradeReceivables
        && OtherCurrentAssets == statement.OtherCurrentAssets
        && ShortTermInvestments == statement.ShortTermInvestments
        && Cash == statement.Cash
        && NonCurrentLiabilities == statement.NonCurrentLiabilities
        && CurrentLiabilities == statement.CurrentLiabilities
        && LongTermBorrowings == statement.LongTermBorrowings
        && ShortTermBorrowings == statement.ShortTermBorrowings
        && LongTermLeaseLiabilities == statement.LongTermLeaseLiabilities
        && ShortTermLeaseLiabilities == statement.ShortTermLeaseLiabilities
        && LongTermTradePayables == statement.LongTermTradePayables
        && ShortTermTradePayables == statement.ShortTermTradePayables
        && LongTermBonds == statement.LongTermBonds
        && ShortTermBonds == statement.ShortTermBonds
        && OtherLongTermLiabilities == statement.OtherLongTermLiabilities
        && OtherShortTermLiabilities == statement.OtherShortTermLiabilities
        && ShareCapital == statement.ShareCapital;
}
