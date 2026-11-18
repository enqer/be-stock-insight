using StockInsight.Domain.Algorithm.Backtest.Enums;

namespace StockInsight.Domain.Algorithm.Backtest.Models;

public sealed record Trade(DateOnly Date, TradeSide Side, string Ticker, double Price);
