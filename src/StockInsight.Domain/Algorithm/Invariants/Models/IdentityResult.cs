namespace StockInsight.Domain.Algorithm.Invariants.Models;

public sealed record IdentityResult(string Name, int BrokenCount, int CheckedCount, IReadOnlyList<string> Examples)
{
    public bool Holds => BrokenCount == 0;
}
