using StockInsight.Domain.Algorithm.Decisions.Pipeline.Enums;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Decisions.Scope;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Steps;

public static class ScopeStep
{
    public static StepResult<string> Run(string ticker) =>
        ModelScope.Exclusions.TryGetValue(ticker, out var exclusion)
            ? StepResult.Reject<string>(RejectionStage.OutOfScope, "poza zakresem: " + exclusion)
            : StepResult.Next(ticker);
}
