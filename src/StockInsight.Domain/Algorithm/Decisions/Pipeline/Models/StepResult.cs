using StockInsight.Domain.Algorithm.Decisions.Pipeline.Enums;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

public static class StepResult
{
    public static StepResult<T> Next<T>(T value) => new(value, null);

    public static StepResult<T> Reject<T>(RejectionStage stage, string reason) => new(default, new StepRejection(stage, reason));
}
