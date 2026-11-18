using StockInsight.Domain.Algorithm.Backtest.Performance.Models;

namespace StockInsight.Domain.Algorithm.Backtest.Models;

public sealed record SimulationResult(
    IReadOnlyList<CurvePoint> Curve,
    IReadOnlyList<Trade> Trades,
    IReadOnlyList<RankingSnapshot> Rankings,
    int Buys,
    int Sells,
    int Reentries,
    double Turnover);
