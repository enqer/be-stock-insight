using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;

namespace StockInsight.Domain.Algorithm.Strategy;

public sealed class DatePricing(
    DateOnly decisionDate,
    IReadOnlyDictionary<string, PriceSeries> prices,
    IReadOnlyDictionary<string, double> lastKnownCloses,
    DateOnly? executionSession = null)
{
    public double? TradePrice(string ticker) =>
        executionSession is { } session
            ? SessionClose(ticker, session)
            : prices[ticker].FirstOnOrAfter(decisionDate)?.Close ?? (lastKnownCloses.TryGetValue(ticker, out var known) ? known : null);

    public bool HasEntryPrice(string ticker, int maxPriceAgeDays) =>
        executionSession is { } session
            ? SessionClose(ticker, session) is not null
            : prices[ticker].LastOnOrBefore(decisionDate) is { } last && decisionDate.DayNumber - last.Date.DayNumber <= maxPriceAgeDays;

    public double ValuationPrice(string ticker) => lastKnownCloses.GetValueOrDefault(ticker, 0.0);

    private double? SessionClose(string ticker, DateOnly session) =>
        prices[ticker].FirstOnOrAfter(session) is { } point && point.Date == session ? point.Close : null;
}
