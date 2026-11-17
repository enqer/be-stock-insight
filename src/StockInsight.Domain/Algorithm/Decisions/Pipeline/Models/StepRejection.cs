using StockInsight.Domain.Algorithm.Decisions.Pipeline.Enums;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

public sealed record StepRejection(RejectionStage Stage, string Reason);
