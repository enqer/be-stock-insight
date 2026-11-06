using StockInsight.Domain.Aggregates.Common;
using StockInsight.Domain.Aggregates.Instruments.Enums;
using StockInsight.Domain.ValueObjects;

namespace StockInsight.Domain.Aggregates.Instruments;

public sealed class Instrument : IAggregateRoot
{
    private Instrument()
    {
    }

    public Guid Id { get; private init; }

    public Ticker Ticker { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public string? Isin { get; private init; }

    public InstrumentKind Kind { get; private init; }

    public Guid? CompanyId { get; private init; }

    public static Instrument Stock(Guid companyId, Ticker ticker, string name, string isin)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(isin);
        return New(ticker, name, isin, InstrumentKind.Stock, companyId);
    }

    public static Instrument Index(Ticker ticker, string name) => New(ticker, name, null, InstrumentKind.Index, null);

    public void Rename(Ticker ticker, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Ticker = ticker;
        Name = name.Trim();
    }

    private static Instrument New(Ticker ticker, string name, string? isin, InstrumentKind kind, Guid? companyId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new Instrument
        {
            Id = Guid.CreateVersion7(),
            Ticker = ticker,
            Name = name.Trim(),
            Isin = isin?.Trim(),
            Kind = kind,
            CompanyId = companyId,
        };
    }
}
