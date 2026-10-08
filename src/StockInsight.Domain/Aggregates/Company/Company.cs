using StockInsight.Domain.Enums;
using StockInsight.Domain.Interfaces;
using StockInsight.Domain.ValueObjects;

namespace StockInsight.Domain.Aggregates.Company;

public class Company : IAggregateRoot
{
    private readonly List<DailyPrice> _prices = [];
    public Ticker Ticker { get; private init; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string ISIN { get; private init; } = string.Empty;
    public Market Market { get; private init; }
    public bool IsActive { get; private init; }

    public IReadOnlyCollection<DailyPrice> Prices => _prices.AsReadOnly();


    private Company() { }

    public static Company Create(Ticker ticker, string name, string isin, Market market)
    {
        var company = new Company
        {
            Market = market,
            Ticker = ticker,
            ISIN = isin,
            IsActive = true
        };
        company.SetName(name);

        return company;
    }

    public void SetName(string name)
    {
        Name = name;
    }
}
