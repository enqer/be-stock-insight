using StockInsight.Domain.Calculations.Inputs.Statements;

namespace StockInsight.Domain.Calculations.Metrics.Quality;

public static class CashFlowGaps
{
    public static IReadOnlyList<Quarter> ZeroCashFlowsAsMissing(IReadOnlyList<Quarter> quarters) =>
        [.. quarters.Select(q => q is { OperatingCashFlow: 0.0, InvestingCashFlow: 0.0, FinancingCashFlow: 0.0, TotalCashFlow: 0.0 }
            ? q with { OperatingCashFlow = null, InvestingCashFlow = null, FinancingCashFlow = null, TotalCashFlow = null, Capex = null, LeasePayments = null, Dividends = null }
            : q)];
}
