using StockInsight.Domain.Algorithm.Backtest.Models;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Strategy;
using StockInsight.Domain.Algorithm.Strategy.Models;
using StockInsight.Domain.Algorithm.Strategy.Ranking.Models;

namespace StockInsight.Domain.Algorithm.Backtest;

public static class BacktestPipeline
{
    public static SimulationResult Run(IReadOnlyList<AcceptedCompany> universe, IReadOnlyList<DateOnly> dates, StrategyRules rules)
    {
        if (dates.Count == 0)
        {
            throw new ArgumentException("simulation without dates", nameof(dates));
        }

        var prices = universe.ToDictionary(x => x.Ticker, x => x.Prices, StringComparer.Ordinal);
        var book = new BacktestBook(rules, dates[0]);
        var lastKnownCloses = new Dictionary<string, double>(StringComparer.Ordinal);

        foreach (var date in dates)
        {
            var day = StrategyDay.Run(universe, prices, lastKnownCloses, book, rules, date);
            if (day.Ranked is { } ranked)
            {
                book.RecordRanking(new RankingSnapshot(date, ranked));
            }

            book.RecordCurvePoint(date, day.Pricing);
        }

        return book.Result();
    }
}
