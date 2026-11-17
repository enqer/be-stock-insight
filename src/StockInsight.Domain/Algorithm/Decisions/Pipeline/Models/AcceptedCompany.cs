using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

public sealed record AcceptedCompany(string Ticker, IReadOnlyList<DecisionPoint> Series, PriceSeries Prices) : CompanyOutcome(Ticker);
