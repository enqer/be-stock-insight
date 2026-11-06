namespace StockInsight.Domain.Calculations.Inputs.Market;

public sealed class RiskFreeRate
{
    private readonly RiskFreeRateEntry[] _history;

    public RiskFreeRate(IEnumerable<RiskFreeRateEntry> history)
    {
        _history = [.. history];
        if (_history.Length == 0)
        {
            throw new ArgumentException("empty risk-free rate history", nameof(history));
        }

        for (var i = 0; i < _history.Length; i++)
        {
            if (!double.IsFinite(_history[i].Rate))
            {
                throw new ArgumentException($"rate from {_history[i].From:yyyy-MM-dd} is not a finite number", nameof(history));
            }

            if (i > 0 && _history[i].From <= _history[i - 1].From)
            {
                throw new ArgumentException($"risk-free rate history dates must increase ({_history[i].From:yyyy-MM-dd})", nameof(history));
            }
        }
    }

    public double On(DateOnly date) => (_history.LastOrDefault(e => e.From <= date) ?? _history[0]).Rate;
}
