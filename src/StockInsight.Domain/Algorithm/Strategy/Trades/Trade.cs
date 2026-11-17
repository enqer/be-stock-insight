using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;
using StockInsight.Domain.Algorithm.Strategy.Trades.Enums;

namespace StockInsight.Domain.Algorithm.Strategy.Trades;

public sealed record Trade(DateOnly Date, TradeSide Side, string Ticker, double Price, GateReason? ExitReason);
