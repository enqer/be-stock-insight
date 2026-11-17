using StockInsight.Domain.Algorithm.Decisions;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Invariants.Models;

namespace StockInsight.Domain.Algorithm.Invariants.Provenance;

internal static class ProvenanceCheck
{
    public static IdentityResult Run<TSource>(
        IReadOnlyList<AcceptedCompany> universe, string name, Func<DecisionPoint, TSource> source)
        where TSource : struct, Enum
    {
        var tally = new IdentityCheck.Tally(name);
        foreach (var (ticker, point) in IdentityCheck.Observations(universe))
        {
            var recorded = source(point);
            tally.FailIf(!Enum.IsDefined(recorded), () => $"{ticker} {point.PeriodLabel}: '{recorded}'");
        }

        return tally.Result();
    }
}
