using StockInsight.Domain.Algorithm.Backtest.Performance.Models;
using StockInsight.Domain.Algorithm.Strategy.Ranking.Models;
using StockInsight.Domain.Algorithm.Strategy.Trades;

namespace StockInsight.Domain.Algorithm.Backtest.Models;

public sealed record SimulationResult(
    IReadOnlyList<CurvePoint> Curve,
    IReadOnlyList<Trade> Trades,
    IReadOnlyList<RankingSnapshot> Rankings,
    int Buys,
    int Sells,
    int Reentries,
    double Turnover);
