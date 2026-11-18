namespace StockInsight.Infrastructure.Providers.Financial.Models;

public sealed record CompanyDetailsDto(string Name, string Ticker, string Isin, bool IsBank, SectorDto? Sector, IReadOnlyList<StatementDto> Statements, IReadOnlyList<string> Warnings);
