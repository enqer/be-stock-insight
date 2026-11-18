using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;

namespace StockInsight.Domain.Algorithm.Strategy;

public sealed class DatePricing(
    DateOnly decisionDate,
    IReadOnlyDictionary<string, PriceSeries> prices,
    DateOnly? executionSession = null)
{
    public double? TradePrice(string ticker)
    {
        if (executionSession is { } session)
        {
            return SessionClose(ticker, session);
        }

        var firstCloseFromDecisionDate = prices[ticker].FirstOnOrAfter(decisionDate)?.Close;
        return firstCloseFromDecisionDate ?? LastClose(ticker);
    }

    public bool HasEntryPrice(string ticker, int maxPriceAgeDays)
    {
        if (prices[ticker].LastOnOrBefore(decisionDate) is not { } lastBeforeDecision)
        {
            return false;
        }

        if (executionSession is { } session)
        {
            return SessionClose(ticker, session) is not null;
        }

        var priceAgeDays = decisionDate.DayNumber - lastBeforeDecision.Date.DayNumber;
        return priceAgeDays <= maxPriceAgeDays;
    }

    public double ValuationPrice(string ticker) => LastClose(ticker) ?? 0.0;

    private double? LastClose(string ticker) => prices[ticker].LastOnOrBefore(decisionDate)?.Close;

    private double? SessionClose(string ticker, DateOnly session) =>
        prices[ticker].FirstOnOrAfter(session) is { } point && point.Date == session ? point.Close : null;
}
