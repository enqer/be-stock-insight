using System.Globalization;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Enums;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Steps;

public static class MinDecisionsStep
{
    public static StepResult<IReadOnlyList<DecisionPoint>> Run(IReadOnlyList<DecisionPoint> series) =>
        series.Count < CompanyPipeline.MinUniversePeriods
            ? StepResult.Reject<IReadOnlyList<DecisionPoint>>(RejectionStage.TooFewDecisions,
                string.Create(CultureInfo.InvariantCulture, $"po kwarantannie zostało {series.Count} okresów"))
            : StepResult.Next(series);
}
