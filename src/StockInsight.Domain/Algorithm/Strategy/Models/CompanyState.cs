using StockInsight.Domain.Algorithm.Decisions;

namespace StockInsight.Domain.Algorithm.Strategy.Models;

public sealed record CompanyState(string Ticker, DecisionPoint LatestDecision, bool HasEntryPrice);
