using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;

namespace StockInsight.Domain.Algorithm.Strategy;

public interface IStrategyBook
{
    double Cash { get; }
    int PositionCount { get; }
    IReadOnlyList<string> HeldTickers { get; }
    bool Holds(string ticker);
    double Units(string ticker);
    double Value(DatePricing pricing);
    void Sell(DateOnly tradeDate, string ticker, double price, GateReason reason);
    void TrimTo(DateOnly tradeDate, string ticker, double targetValue, double price);
    void TopUpTo(DateOnly tradeDate, string ticker, double targetValue, double price);
    void Buy(DateOnly tradeDate, string ticker, double amount, double price);
}
