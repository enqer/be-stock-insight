using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;

public sealed record CompanyInputs(IReadOnlyList<Quarter> Quarters, PriceSeries Prices);
