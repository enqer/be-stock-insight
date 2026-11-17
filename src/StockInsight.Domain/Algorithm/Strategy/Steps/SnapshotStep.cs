using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Strategy.Models;

namespace StockInsight.Domain.Algorithm.Strategy.Steps;

public static class SnapshotStep
{
    public static MarketSnapshot Run(
        IReadOnlyList<AcceptedCompany> universe,
        DateOnly decisionDate,
        StrategyRules rules,
        IReadOnlyDictionary<string, double> lastKnownCloses,
        DatePricing pricing)
    {
        var state = new OrderedDictionary<string, CompanyState>(StringComparer.Ordinal);
        foreach (var company in universe)
        {
            if (company.Series.TakeWhile(x => x.PublicationDate <= decisionDate).LastOrDefault() is not { } latest)
            {
                continue;
            }

            var hasEntryPrice = lastKnownCloses.ContainsKey(company.Ticker) && pricing.HasEntryPrice(company.Ticker, rules.MaxEntryPriceAgeDays);
            state.Add(company.Ticker, new CompanyState(company, latest, hasEntryPrice));
        }

        return new MarketSnapshot(state);
    }
}
