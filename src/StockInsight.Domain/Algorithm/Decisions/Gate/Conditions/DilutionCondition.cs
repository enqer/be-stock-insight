using StockInsight.Domain.Algorithm.Decisions.Gate.Conditions.Models;
using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;

namespace StockInsight.Domain.Algorithm.Decisions.Gate.Conditions;

public sealed class DilutionCondition : IEntryCondition
{
    public const double MaxYearOverYearDilution = 0.05;

    public ConditionCheck Check(DecisionPoint point)
    {
        if (point.Quality.Dilution is not { } dilution)
        {
            return ConditionCheck.Skipped(GateReason.Dilution, "brak rozwodnienia");
        }

        return dilution > MaxYearOverYearDilution
            ? ConditionCheck.Failed(GateReason.Dilution, "rozwodnienie")
            : ConditionCheck.Passed(GateReason.Dilution, "bez rozwodnienia");
    }
}
