
namespace StockInsight.Domain.ValueObjects;

public record Ticker
{
    public string Symbol { get; }

    public Ticker(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            throw new ArgumentException("Ticker cannot be empty");
        }

        Symbol = symbol.ToUpperInvariant().Trim();
    }

    public override string ToString() => Symbol;
}
