using StockInsight.Domain.Algorithm.Decisions.Gate.Conditions.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Gate.Conditions;

public interface IEntryCondition
{
    ConditionCheck Check(DecisionPoint point);
}
