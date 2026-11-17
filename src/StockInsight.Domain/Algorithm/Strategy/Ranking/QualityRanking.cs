using StockInsight.Domain.Algorithm.Strategy.Ranking.Models;

namespace StockInsight.Domain.Algorithm.Strategy.Ranking;

public static class QualityRanking
{
    public const int MinValuesForPercentile = 5;

    public const double NeutralPercentile = 0.5;

    public static IReadOnlyList<RankedCandidate> Rank(IReadOnlyList<RankingCandidate> candidates)
    {
        var headroomDistribution = Distribution(candidates.Select(x => (double?)x.Components.Headroom));
        var roiicDistribution = Distribution(candidates.Select(x => x.Components.Roiic));
        var cashConversionDistribution = Distribution(candidates.Select(x => x.Components.CashConversion));

        return
        [
            .. candidates
                .Select(x =>
                {
                    var headroomPercentile = Percentile(x.Components.Headroom, headroomDistribution);
                    var roiicPercentile = Percentile(x.Components.Roiic, roiicDistribution);
                    var cashConversionPercentile = Percentile(x.Components.CashConversion, cashConversionDistribution);
                    return new RankedCandidate(x.Ticker, (headroomPercentile + roiicPercentile + cashConversionPercentile) / 3, headroomPercentile, roiicPercentile, cashConversionPercentile);
                })
                .OrderByDescending(x => x.Score),
        ];
    }

    public static double Percentile(double? value, IReadOnlyList<double> sortedDistribution)
    {
        if (value is not { } knownValue || sortedDistribution.Count < MinValuesForPercentile)
        {
            return NeutralPercentile;
        }

        return (double)sortedDistribution.Count(x => x < knownValue) / sortedDistribution.Count;
    }

    private static double[] Distribution(IEnumerable<double?> values) => [.. values.OfType<double>().Order()];
}
