using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Strategy.Models;

namespace StockInsight.Domain.Algorithm.Strategy.Steps;

public static class SnapshotStep
{
    public static MarketSnapshot Run(
        IReadOnlyList<AcceptedCompany> universe,
        DateOnly decisionDate,
        StrategyRules rules,
        DatePricing pricing)
    {
        var state = new OrderedDictionary<string, CompanyState>(StringComparer.Ordinal);
        foreach (var company in universe)
        {
            if (company.Series.TakeWhile(x => x.PublicationDate <= decisionDate).LastOrDefault() is not { } latest)
            {
                continue;
            }

            state.Add(company.Ticker, new CompanyState(company.Ticker, latest, pricing.HasEntryPrice(company.Ticker, rules.MaxEntryPriceAgeDays)));
        }

        return new MarketSnapshot(state);
    }
}
