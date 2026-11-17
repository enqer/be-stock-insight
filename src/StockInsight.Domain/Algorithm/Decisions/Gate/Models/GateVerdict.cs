using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;

namespace StockInsight.Domain.Algorithm.Decisions.Gate.Models;

public sealed record GateVerdict(bool Passed, GateReason Reason, string Message);
