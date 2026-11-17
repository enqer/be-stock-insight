using StockInsight.Domain.Algorithm.Calculations.Metrics.Capital.Enums;

namespace StockInsight.Domain.Algorithm.Calculations.Metrics.Capital.Models;

public readonly record struct DebtValue(double Amount, DebtSource Source);
