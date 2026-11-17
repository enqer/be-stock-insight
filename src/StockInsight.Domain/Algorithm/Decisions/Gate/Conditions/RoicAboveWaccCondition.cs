using System.Globalization;
using StockInsight.Domain.Algorithm.Decisions.Gate.Conditions.Models;
using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;

namespace StockInsight.Domain.Algorithm.Decisions.Gate.Conditions;

public sealed class RoicAboveWaccCondition : IEntryCondition
{
    public ConditionCheck Check(DecisionPoint point)
    {
        if (point.CostOfCapital.Wacc?.Rate is not { } wacc)
        {
            return ConditionCheck.Skipped(GateReason.RoicNotAboveWacc, "brak WACC");
        }

        return point.Returns.Roic is { } roic && roic > wacc
            ? ConditionCheck.Passed(GateReason.RoicNotAboveWacc, string.Create(CultureInfo.InvariantCulture, $"ROIC {roic * 100:F1}% > WACC {wacc * 100:F1}%"))
            : ConditionCheck.Failed(GateReason.RoicNotAboveWacc, string.Create(CultureInfo.InvariantCulture, $"ROIC {(point.Returns.Roic ?? 0) * 100:F1}% <= WACC {wacc * 100:F1}%"));
    }
}
