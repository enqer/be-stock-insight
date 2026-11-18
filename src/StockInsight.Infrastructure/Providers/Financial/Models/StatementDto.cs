namespace StockInsight.Infrastructure.Providers.Financial.Models;

public sealed record StatementDto(string Period, IReadOnlyList<StatementCellDto> Items);
