using System.Globalization;
using StockInsight.Domain.Algorithm.Decisions.Gate.Conditions.Models;
using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;

namespace StockInsight.Domain.Algorithm.Decisions.Gate.Conditions;

public sealed class DirectionCondition : IEntryCondition
{
    public const int MinCriteriaMet = 2;

    public ConditionCheck Check(DecisionPoint point) =>
        point.Quality.DirectionCriteriaMet is { } met && met >= MinCriteriaMet
            ? ConditionCheck.Passed(GateReason.Direction, string.Create(CultureInfo.InvariantCulture, $"kierunek {met}/3"))
            : ConditionCheck.Failed(GateReason.Direction, "kierunek <2/3");
}
