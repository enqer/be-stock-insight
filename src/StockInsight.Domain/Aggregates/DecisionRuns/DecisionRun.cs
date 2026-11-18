using StockInsight.Domain.Aggregates.Common;
using StockInsight.Domain.Aggregates.DecisionRuns.Models;

namespace StockInsight.Domain.Aggregates.DecisionRuns;

public sealed class DecisionRun : AuditableEntity, IAggregateRoot
{
    private readonly List<DecisionVerdict> _verdicts = [];
    private readonly List<DecisionRankingEntry> _ranking = [];

    private DecisionRun()
    {
    }

    public Guid Id { get; private init; }
    public Guid PortfolioId { get; private init; }
    public DateOnly DecisionDate { get; private init; }
    public DateOnly ExecutedOn { get; private init; }

    public IReadOnlyCollection<DecisionVerdict> Verdicts => _verdicts.AsReadOnly();
    public IReadOnlyCollection<DecisionRankingEntry> Ranking => _ranking.AsReadOnly();

    public static DecisionRun Record(
        Guid portfolioId,
        DateOnly decisionDate,
        DateOnly executedOn,
        IReadOnlyList<VerdictInput> verdicts,
        IReadOnlyList<RankingInput> ranking)
    {
        if (executedOn < decisionDate)
        {
            throw new ArgumentOutOfRangeException(nameof(executedOn), executedOn, "execution cannot precede the decision date");
        }

        EnsureOncePerInstrument(verdicts.Select(x => x.InstrumentId), nameof(verdicts));
        EnsureOncePerInstrument(ranking.Select(x => x.InstrumentId), nameof(ranking));

        var run = new DecisionRun
        {
            Id = Guid.CreateVersion7(),
            PortfolioId = portfolioId,
            DecisionDate = decisionDate,
            ExecutedOn = executedOn,
        };
        run._verdicts.AddRange(verdicts.Select(x => DecisionVerdict.Create(run.Id, x.InstrumentId, x.Reason, x.PeriodEnd)));
        run._ranking.AddRange(ranking.Select((x, i) => DecisionRankingEntry.Create(run.Id, i + 1, x)));
        return run;
    }

    private static void EnsureOncePerInstrument(IEnumerable<Guid> instruments, string name)
    {
        if (instruments.GroupBy(x => x).Any(x => x.Count() > 1))
        {
            throw new ArgumentException("an instrument appears more than once", name);
        }
    }
}
