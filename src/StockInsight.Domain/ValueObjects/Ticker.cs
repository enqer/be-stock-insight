namespace StockInsight.Domain.ValueObjects;

public sealed record Ticker
{
    public string Symbol { get; }

    public Ticker(string symbol)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(symbol);
        Symbol = symbol.ToUpperInvariant().Trim();
    }

    public override string ToString() => Symbol;
}
