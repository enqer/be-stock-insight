using StockInsight.Domain.Algorithm.Decisions;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

namespace StockInsight.Domain.Algorithm.Strategy.Models;

public sealed record CompanyState(AcceptedCompany Company, DecisionPoint LatestDecision, bool HasEntryPrice);
