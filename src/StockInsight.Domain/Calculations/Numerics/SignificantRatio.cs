namespace StockInsight.Domain.Calculations.Numerics;

public static class SignificantRatio
{
    public static double? Of(double? numerator, double? denominator, double? scale, double minShareOfScale)
    {
        if (numerator is not { } num || denominator is not { } den || scale is not { } s || s <= 0)
        {
            return null;
        }

        return Math.Abs(den) < minShareOfScale * Math.Abs(s) ? null : num / den;
    }
}
