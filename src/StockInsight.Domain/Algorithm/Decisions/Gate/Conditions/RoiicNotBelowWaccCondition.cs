using StockInsight.Domain.Algorithm.Decisions.Gate.Conditions.Models;
using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;

namespace StockInsight.Domain.Algorithm.Decisions.Gate.Conditions;

public sealed class RoiicNotBelowWaccCondition : IEntryCondition
{
    public ConditionCheck Check(DecisionPoint point)
    {
        if (point.CostOfCapital.Wacc?.Rate is not { } wacc)
        {
            return ConditionCheck.Skipped(GateReason.RoiicBelowWacc, "brak WACC");
        }

        if (point.Returns.Roiic is not { } roiic)
        {
            return ConditionCheck.Skipped(GateReason.RoiicBelowWacc, "brak ROIIC");
        }

        return roiic < wacc
            ? ConditionCheck.Failed(GateReason.RoiicBelowWacc, "ROIIC < WACC")
            : ConditionCheck.Passed(GateReason.RoiicBelowWacc, "ROIIC >= WACC");
    }
}
