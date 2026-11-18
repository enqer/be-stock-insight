using StockInsight.Domain.Algorithm.Backtest.Enums;
using StockInsight.Domain.Algorithm.Decisions.Gate.Enums;

namespace StockInsight.Domain.Algorithm.Backtest.Models;

public sealed record Trade(DateOnly Date, TradeSide Side, string Ticker, double Price, GateReason? ExitReason);
