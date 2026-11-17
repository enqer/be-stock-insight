using StockInsight.Domain.Algorithm.Decisions.Gate.Conditions.Enums;
using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;

namespace StockInsight.Domain.Algorithm.Decisions.Gate.Conditions.Models;

public sealed record ConditionCheck(GateReason Reason, ConditionStatus Status, string Message)
{
    public static ConditionCheck Passed(GateReason reason, string message) => new(reason, ConditionStatus.Passed, message);

    public static ConditionCheck Failed(GateReason reason, string message) => new(reason, ConditionStatus.Failed, message);

    public static ConditionCheck Skipped(GateReason reason, string message) => new(reason, ConditionStatus.Skipped, message);
}
