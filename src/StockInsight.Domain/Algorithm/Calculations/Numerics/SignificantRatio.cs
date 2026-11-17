namespace StockInsight.Domain.Algorithm.Calculations.Numerics;

public static class SignificantRatio
{
    public static double? Of(double? numerator, double? denominator, double? scale, double minShareOfScale)
    {
        if (numerator is not { } knownNumerator || denominator is not { } knownDenominator || scale is not { } knownScale || knownScale <= 0)
        {
            return null;
        }

        return Math.Abs(knownDenominator) < minShareOfScale * Math.Abs(knownScale) ? null : knownNumerator / knownDenominator;
    }
}
