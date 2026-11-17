using System.Globalization;
using StockInsight.Domain.Algorithm.Decisions.Gate.Conditions.Models;
using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;

namespace StockInsight.Domain.Algorithm.Decisions.Gate.Conditions;

public sealed class WaccAvailableCondition : IEntryCondition
{
    public ConditionCheck Check(DecisionPoint point) =>
        point.CostOfCapital.Wacc?.Rate is { } wacc
            ? ConditionCheck.Passed(GateReason.NoWacc, string.Create(CultureInfo.InvariantCulture, $"WACC {wacc * 100:F1}%"))
            : ConditionCheck.Failed(GateReason.NoWacc, "brak WACC");
}
