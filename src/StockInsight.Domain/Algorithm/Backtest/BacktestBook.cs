using StockInsight.Domain.Algorithm.Backtest.Enums;
using StockInsight.Domain.Algorithm.Backtest.Models;
using StockInsight.Domain.Algorithm.Backtest.Performance.Models;
using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;
using StockInsight.Domain.Algorithm.Strategy;
using StockInsight.Domain.Algorithm.Strategy.Models;

namespace StockInsight.Domain.Algorithm.Backtest;

public sealed class BacktestBook : IStrategyBook
{
    private readonly double _buyFactor;
    private readonly double _sellFactor;
    private readonly OrderedDictionary<string, double> _holdingsInPurchaseOrder = new(StringComparer.Ordinal);
    private readonly HashSet<string> _everHeld = new(StringComparer.Ordinal);
    private readonly List<CurvePoint> _curve;
    private readonly List<Trade> _trades = [];
    private readonly List<RankingSnapshot> _rankings = [];
    private int _buys;
    private int _sells;
    private int _reentries;
    private double _turnover;

    public BacktestBook(StrategyRules rules, DateOnly start)
    {
        _buyFactor = 1 + rules.CostBasisPoints / 1e4;
        _sellFactor = 1 - rules.CostBasisPoints / 1e4;
        _curve = [new CurvePoint(start, 1.0, 0)];
    }

    public double Cash { get; private set; } = 1.0;

    public int PositionCount => _holdingsInPurchaseOrder.Count;

    public IReadOnlyList<string> HeldTickers => [.. _holdingsInPurchaseOrder.Keys];

    public bool Holds(string ticker) => _holdingsInPurchaseOrder.ContainsKey(ticker);

    public double Units(string ticker) => _holdingsInPurchaseOrder[ticker];

    public double Value(DatePricing pricing)
    {
        var value = Cash;
        foreach (var (ticker, units) in _holdingsInPurchaseOrder)
        {
            value += units * pricing.ValuationPrice(ticker);
        }

        return value;
    }

    public void Sell(DateOnly tradeDate, string ticker, double price, GateReason reason)
    {
        var units = _holdingsInPurchaseOrder[ticker];
        Cash += units * price * _sellFactor;
        _turnover += units * price;
        _holdingsInPurchaseOrder.Remove(ticker);
        _sells++;
        _trades.Add(new Trade(tradeDate, TradeSide.Sell, ticker, price, reason));
    }

    public void TrimTo(DateOnly tradeDate, string ticker, double targetValue, double price)
    {
        var heldValue = _holdingsInPurchaseOrder[ticker] * price;
        var excessUnits = (heldValue - targetValue) / price;
        _holdingsInPurchaseOrder[ticker] -= excessUnits;
        Cash += excessUnits * price * _sellFactor;
        _turnover += excessUnits * price;
    }

    public void TopUpTo(DateOnly tradeDate, string ticker, double targetValue, double price)
    {
        var heldValue = _holdingsInPurchaseOrder[ticker] * price;
        var addedUnits = Math.Min(targetValue - heldValue, Cash) / (price * _buyFactor);
        _holdingsInPurchaseOrder[ticker] += addedUnits;
        Cash -= addedUnits * price * _buyFactor;
        _turnover += addedUnits * price;
    }

    public void Buy(DateOnly tradeDate, string ticker, double amount, double price)
    {
        _holdingsInPurchaseOrder[ticker] = amount / (price * _buyFactor);
        Cash -= amount;
        _turnover += amount;
        _buys++;
        if (!_everHeld.Add(ticker))
        {
            _reentries++;
        }

        _trades.Add(new Trade(tradeDate, TradeSide.Buy, ticker, price, null));
    }

    public void RecordRanking(RankingSnapshot snapshot) => _rankings.Add(snapshot);

    public void RecordCurvePoint(DateOnly date, DatePricing pricing) => _curve.Add(new CurvePoint(date, Value(pricing), _holdingsInPurchaseOrder.Count));

    public SimulationResult Result() => new(_curve, _trades, _rankings, _buys, _sells, _reentries, _turnover);
}
