using StockInsight.Domain.Calculations.Metrics.CostOfCapital.Enums;

namespace StockInsight.Domain.Calculations.Metrics.CostOfCapital;

public readonly record struct WindowLeverageValue(double DebtToEquity, WindowLeverageSource Source);
