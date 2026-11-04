namespace StockInsight.Domain.Aggregates.Companies;

public sealed class BankStatement : Statement
{
    private BankStatement()
    {
    }

    public double? InterestIncome { get; private init; }
    public double? InterestExpense { get; private init; }
    public double? NetInterestIncome { get; private init; }
    public double? NetFeeIncome { get; private init; }
    public double? DividendIncome { get; private init; }
    public double? TradingResult { get; private init; }
    public double? OtherFinancialInstrumentsResult { get; private init; }
    public double? LoanImpairment { get; private init; }
    public double? AdministrativeExpenses { get; private init; }
    public double? OperatingProfit { get; private init; }
    public double? ShareOfAssociatesProfit { get; private init; }
    public double? CustomerLoans { get; private init; }
    public double? CustomerDeposits { get; private init; }

    public static BankStatement Create(
        Guid companyId,
        DateOnly period,
        DateOnly fetchedAt,
        DateOnly? publishedAt,
        double? interestIncome,
        double? interestExpense,
        double? netInterestIncome,
        double? netFeeIncome,
        double? dividendIncome,
        double? tradingResult,
        double? otherFinancialInstrumentsResult,
        double? loanImpairment,
        double? administrativeExpenses,
        double? operatingProfit,
        double? shareOfAssociatesProfit,
        double? customerLoans,
        double? customerDeposits,
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
            InterestIncome = interestIncome,
            InterestExpense = interestExpense,
            NetInterestIncome = netInterestIncome,
            NetFeeIncome = netFeeIncome,
            DividendIncome = dividendIncome,
            TradingResult = tradingResult,
            OtherFinancialInstrumentsResult = otherFinancialInstrumentsResult,
            LoanImpairment = loanImpairment,
            AdministrativeExpenses = administrativeExpenses,
            OperatingProfit = operatingProfit,
            ShareOfAssociatesProfit = shareOfAssociatesProfit,
            CustomerLoans = customerLoans,
            CustomerDeposits = customerDeposits,
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
        other is BankStatement statement
        && HasSameCommonContent(statement)
        && InterestIncome == statement.InterestIncome
        && InterestExpense == statement.InterestExpense
        && NetInterestIncome == statement.NetInterestIncome
        && NetFeeIncome == statement.NetFeeIncome
        && DividendIncome == statement.DividendIncome
        && TradingResult == statement.TradingResult
        && OtherFinancialInstrumentsResult == statement.OtherFinancialInstrumentsResult
        && LoanImpairment == statement.LoanImpairment
        && AdministrativeExpenses == statement.AdministrativeExpenses
        && OperatingProfit == statement.OperatingProfit
        && ShareOfAssociatesProfit == statement.ShareOfAssociatesProfit
        && CustomerLoans == statement.CustomerLoans
        && CustomerDeposits == statement.CustomerDeposits;
}
