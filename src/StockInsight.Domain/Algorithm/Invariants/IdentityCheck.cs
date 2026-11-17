using StockInsight.Domain.Algorithm.Decisions;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Invariants.Algebraic;
using StockInsight.Domain.Algorithm.Invariants.Economic;
using StockInsight.Domain.Algorithm.Invariants.Models;
using StockInsight.Domain.Algorithm.Invariants.Provenance;

namespace StockInsight.Domain.Algorithm.Invariants;

public static class IdentityCheck
{
    public const double Tolerance = 1e-9;

    public const int MaxExamples = 3;

    public static IReadOnlyList<IdentityResult> RunAll(IReadOnlyList<AcceptedCompany> universe) =>
    [
        RoicReproducible.Check(universe),
        AverageCapitalIsMean.Check(universe),
        WaccReproducible.Check(universe),
        CostOfEquityAboveRiskFree.Check(universe),
        PublicationOffsetConsistent.Check(universe),
        DebtSourceRecorded.Check(universe),
        BetaReleveringIdentity.Check(universe),
        WindowLeverageDiffersFromCurrent.Check(universe),
        WindowLeverageSourceRecorded.Check(universe),
        CapitalPositiveWhenRoic.Check(universe),
    ];

    internal static IEnumerable<(string Ticker, DecisionPoint Point)> Observations(IReadOnlyList<AcceptedCompany> universe) =>
        universe
            .OrderBy(x => x.Ticker, StringComparer.Ordinal)
            .SelectMany(x => x.Series.Select(y => (x.Ticker, y)));

    internal static bool Differs(double expected, double actual, double tolerance = Tolerance) =>
        !(Math.Abs(expected - actual) <= tolerance * Math.Max(1, Math.Abs(actual)));

    internal sealed class Tally(string name)
    {
        private readonly List<string> _examples = [];
        private int _brokenCount;
        private int _checkedCount;

        public void FailIf(bool broken, Func<string> example)
        {
            _checkedCount++;
            if (broken && ++_brokenCount <= MaxExamples)
            {
                _examples.Add(example());
            }
        }

        public IdentityResult Result() => new(name, _brokenCount, _checkedCount, _examples);
    }
}
