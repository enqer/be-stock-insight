using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Invariants.Models;

namespace StockInsight.Domain.Algorithm.Invariants.Provenance;

public static class DebtSourceRecorded
{
    public const string Name = "T6 debt_zrodlo zapisane";

    public static IdentityResult Check(IReadOnlyList<AcceptedCompany> universe) =>
        ProvenanceCheck.Run(universe, Name, x => x.Capital.Debt.Source);
}
