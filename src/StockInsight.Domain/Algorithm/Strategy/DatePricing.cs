using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;

namespace StockInsight.Domain.Algorithm.Strategy;

public sealed class DatePricing(
    DateOnly decisionDate,
    IReadOnlyDictionary<string, PriceSeries> prices,
    DateOnly? executionSession = null)
{
    public double? TradePrice(string ticker) =>
        executionSession is { } session
            ? SessionClose(ticker, session)
            : prices[ticker].FirstOnOrAfter(decisionDate)?.Close ?? LastClose(ticker);

    public bool HasEntryPrice(string ticker, int maxPriceAgeDays)
    {
        if (prices[ticker].LastOnOrBefore(decisionDate) is not { } last)
        {
            return false;
        }

        return executionSession is { } session
            ? SessionClose(ticker, session) is not null
            : decisionDate.DayNumber - last.Date.DayNumber <= maxPriceAgeDays;
    }

    public double ValuationPrice(string ticker) => LastClose(ticker) ?? 0.0;

    private double? LastClose(string ticker) => prices[ticker].LastOnOrBefore(decisionDate)?.Close;

    private double? SessionClose(string ticker, DateOnly session) =>
        prices[ticker].FirstOnOrAfter(session) is { } point && point.Date == session ? point.Close : null;
}
