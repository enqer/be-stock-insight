using System.Globalization;
using StockInsight.Domain.Algorithm.Decisions.Gate.Conditions.Models;
using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;

namespace StockInsight.Domain.Algorithm.Decisions.Gate.Conditions;

public sealed class EarningsAnomalyCondition : IEntryCondition
{
    public const double MaxRoicToNormalisedRoic = 3.0;

    public ConditionCheck Check(DecisionPoint point)
    {
        if (point.Returns.Roic is not { } roic || point.Returns.NormalisedRoic is not { } normalised || normalised <= 0)
        {
            return ConditionCheck.Skipped(GateReason.EarningsAnomaly, "brak ROIC znormalizowanego");
        }

        var ratio = roic / normalised;
        return ratio > MaxRoicToNormalisedRoic
            ? ConditionCheck.Failed(GateReason.EarningsAnomaly, string.Create(CultureInfo.InvariantCulture, $"anomalia zysku {ratio:F1}x"))
            : ConditionCheck.Passed(GateReason.EarningsAnomaly, string.Create(CultureInfo.InvariantCulture, $"ROIC {ratio:F1}x znormalizowanego"));
    }
}
