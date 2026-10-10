using StockInsight.Domain.ValueObjects;

namespace StockInsight.Domain.Tests.ValueObjects;

public sealed class TickerTests
{
    [Fact]
    public void Normalizes_symbol_to_upper_case_without_whitespace() =>
        new Ticker("  cdr ").Symbol.ShouldBe("CDR");

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Rejects_empty_symbol(string symbol) =>
        Should.Throw<ArgumentException>(() => new Ticker(symbol));

    [Fact]
    public void Tickers_with_same_symbol_are_equal() =>
        new Ticker("pko").ShouldBe(new Ticker("PKO"));
}
