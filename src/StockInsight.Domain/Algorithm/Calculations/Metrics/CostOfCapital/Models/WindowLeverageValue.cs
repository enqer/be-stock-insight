using StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.Enums;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.CostOfCapital.Models;

public readonly record struct WindowLeverageValue(double DebtToEquity, WindowLeverageSource Source);
