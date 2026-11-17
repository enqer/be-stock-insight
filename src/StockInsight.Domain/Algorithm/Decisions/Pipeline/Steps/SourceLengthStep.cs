using System.Globalization;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Enums;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Steps;

public static class SourceLengthStep
{
    public static StepResult<CompanyInputs> Run(CompanyInputs inputs) =>
        inputs.Quarters.Count < CompanyPipeline.MinUniversePeriods
            ? StepResult.Reject<CompanyInputs>(RejectionStage.TooFewPeriodsInSource,
                string.Create(CultureInfo.InvariantCulture, $"za krótki szereg ({inputs.Quarters.Count} okresów)"))
            : StepResult.Next(inputs);
}
