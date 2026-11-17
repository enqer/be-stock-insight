using StockInsight.Domain.Algorithm.Decisions.Pipeline.Enums;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

public sealed record RejectedCompany(string Ticker, RejectionStage Stage, string Reason) : CompanyOutcome(Ticker);
