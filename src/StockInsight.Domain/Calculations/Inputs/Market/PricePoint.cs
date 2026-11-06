namespace StockInsight.Domain.Calculations.Inputs.Market;

public readonly record struct PricePoint(DateOnly Date, double Close);
