using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Invariants.Models;

namespace StockInsight.Domain.Algorithm.Invariants.Provenance;

public static class WindowLeverageSourceRecorded
{
    public const string Name = "T9 de_okno_zrodlo zapisane";

    public static IdentityResult Check(IReadOnlyList<AcceptedCompany> universe) =>
        ProvenanceCheck.Run(universe, Name, x => x.CostOfCapital.WindowLeverageSource);
}
