using CsvHelper.Configuration.Attributes;

namespace StockInsight.Infrastructure.Seeds.Models;

public sealed class StooqRowDto
{
    [Name("<DATE>"), Format("yyyyMMdd")]
    public DateOnly Date { get; set; }

    [Name("<CLOSE>")]
    public double Close { get; set; }

    [Name("<VOL>")]
    public double Volume { get; set; }
}
