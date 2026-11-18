using System.Globalization;

namespace StockInsight.Infrastructure.Providers.Financial.StockWatch;

public static class StockWatchNumber
{
    private const char NonBreakingSpace = ' ';
    private const char MinusSign = '−';

    public static double? Parse(string? raw)
    {
        var normalized = raw?.Replace(NonBreakingSpace, ' ').Replace(" ", string.Empty, StringComparison.Ordinal).Replace(MinusSign, '-').Replace(',', '.');

        if (!double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
        {
            return null;
        }

        return double.IsFinite(value) ? value : throw new InvalidDataException($"value '{raw}' is not a finite number");
    }
}
