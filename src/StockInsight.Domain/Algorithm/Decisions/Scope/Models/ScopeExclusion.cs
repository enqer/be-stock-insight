namespace StockInsight.Domain.Algorithm.Decisions.Scope.Models;

public sealed record ScopeExclusion(string Group, string Reason)
{
    public override string ToString() => $"{Group}: {Reason}";
}
