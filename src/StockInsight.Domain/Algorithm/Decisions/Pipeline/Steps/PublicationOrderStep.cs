using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Steps;

public static class PublicationOrderStep
{
    public static IReadOnlyList<PeriodMeasures> Run(IReadOnlyList<PeriodMeasures> periods) =>
        [.. periods.OrderBy(x => x.PublicationDate)];
}
