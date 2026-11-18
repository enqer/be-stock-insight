namespace StockInsight.Infrastructure.Providers.Financial.StockWatch;

public sealed class MissingLabelException(string label, string? section)
    : Exception(section is null ? $"missing row '{label}'" : $"missing row '{label}' in section '{section}'");
