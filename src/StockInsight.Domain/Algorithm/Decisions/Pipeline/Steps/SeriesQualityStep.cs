using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Enums;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Decisions.SeriesChecks;
using StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Steps;

public static class SeriesQualityStep
{
    public static StepResult<SeriesQualityResult> Run(IReadOnlyList<Quarter> quarters)
    {
        var quality = SeriesQuality.Evaluate(quarters);
        return quality.Applicable
            ? StepResult.Next(quality)
            : StepResult.Reject<SeriesQualityResult>(RejectionStage.SeriesQuality, string.Join("; ", quality.Reasons));
    }
}
