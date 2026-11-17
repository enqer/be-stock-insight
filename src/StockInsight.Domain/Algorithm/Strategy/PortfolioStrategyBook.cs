using StockInsight.Domain.Aggregates.Portfolios;
using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;

namespace StockInsight.Domain.Algorithm.Strategy;

public sealed class PortfolioStrategyBook(Portfolio portfolio, IReadOnlyDictionary<string, Guid> instrumentIdByTicker) : IStrategyBook
{
    private readonly Dictionary<Guid, string> _tickerByInstrumentId = instrumentIdByTicker.ToDictionary(x => x.Value, x => x.Key);

    public double Cash => (double)portfolio.Holdings().Cash;

    public int PositionCount => portfolio.Holdings().Positions.Count;

    public IReadOnlyList<string> HeldTickers => [.. portfolio.Holdings().Positions.Keys.Select(x => _tickerByInstrumentId[x])];

    public bool Holds(string ticker) => portfolio.Holdings().Positions.ContainsKey(instrumentIdByTicker[ticker]);

    public double Units(string ticker) => (double)portfolio.Holdings().Positions[instrumentIdByTicker[ticker]];

    public double Value(DatePricing pricing)
    {
        var holdings = portfolio.Holdings();
        var value = (double)holdings.Cash;
        foreach (var (instrument, quantity) in holdings.Positions)
        {
            value += (double)quantity * pricing.ValuationPrice(_tickerByInstrumentId[instrument]);
        }

        return value;
    }

    public void Sell(DateOnly tradeDate, string ticker, double price, GateReason reason)
    {
        var instrument = instrumentIdByTicker[ticker];
        portfolio.Sell(tradeDate, instrument, portfolio.Holdings().Positions[instrument], (decimal)price, 0m);
    }

    public void TrimTo(DateOnly tradeDate, string ticker, double targetValue, double price) =>
        Book(portfolio.Sell, tradeDate, ticker, (Units(ticker) * price - targetValue) / price, price);

    public void TopUpTo(DateOnly tradeDate, string ticker, double targetValue, double price) =>
        Book(portfolio.Buy, tradeDate, ticker, Math.Min(targetValue - Units(ticker) * price, Cash) / price, price);

    public void Buy(DateOnly tradeDate, string ticker, double amount, double price) =>
        Book(portfolio.Buy, tradeDate, ticker, amount / price, price);

    private void Book(Action<DateOnly, Guid, decimal, decimal, decimal> trade, DateOnly tradeDate, string ticker, double units, double price)
    {
        if (Transaction.TradableQuantity((decimal)units, (decimal)price) is { } quantity)
        {
            trade(tradeDate, instrumentIdByTicker[ticker], quantity, (decimal)price, 0m);
        }
    }
}
