using System.Globalization;
using StockInsight.Domain.Algorithm.Decisions.Gate.Conditions;
using StockInsight.Domain.Algorithm.Decisions.Gate.Conditions.Enums;
using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;
using StockInsight.Domain.Algorithm.Decisions.Gate.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Gate;

public static class EntryGate
{
    private static readonly IReadOnlyList<IEntryCondition> _conditions =
    [
        new WaccAvailableCondition(),
        new DirectionCondition(),
        new RoicAboveWaccCondition(),
        new RoiicNotBelowWaccCondition(),
        new DilutionCondition(),
        new EarningsAnomalyCondition(),
    ];

    public static GateVerdict Evaluate(DecisionPoint point)
    {
        foreach (var condition in _conditions)
        {
            var check = condition.Check(point);
            if (check.Status == ConditionStatus.Failed)
            {
                return new GateVerdict(Passed: false, check.Reason, check.Message);
            }
        }

        var roic = point.Returns.Roic!.Value;
        var wacc = point.CostOfCapital.Wacc!.Rate;
        var headroom = QualityComponents.From(point)!.Headroom;
        return new GateVerdict(Passed: true, GateReason.Passed,
            string.Create(CultureInfo.InvariantCulture, $"ROIC {roic * 100:F0}% vs WACC {wacc * 100:F1}% (zapas {headroom * 100:F0}%)"));
    }
}
