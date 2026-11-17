namespace StockInsight.Domain.Algorithm.Strategy.Models;

public sealed class MarketSnapshot(OrderedDictionary<string, CompanyState> companies)
{
    public IEnumerable<CompanyState> Companies => companies.Values;

    public bool TryGet(string ticker, out CompanyState state) => companies.TryGetValue(ticker, out state!);
}
