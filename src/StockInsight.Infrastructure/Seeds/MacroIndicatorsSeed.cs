using System.Globalization;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockInsight.Infrastructure.Macro;
using StockInsight.Infrastructure.Seeds.Models;

namespace StockInsight.Infrastructure.Seeds;

public sealed class MacroIndicatorsSeed(
    IOptions<MacroIndicatorsSeedOptions> options,
    MacroIndicatorsImport import,
    ILogger<MacroIndicatorsSeed> logger)
{
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        foreach (var country in options.Value.Countries)
        {
            var monthlyFileSuffix = $"{country.ToLowerInvariant()}.m.txt";
            var countryDirectory = Path.Combine(options.Value.Directory, country.ToLowerInvariant());
            var series = Directory.EnumerateFiles(countryDirectory, $"*{monthlyFileSuffix}")
                .ToDictionary(x => IndicatorCodeOf(x, monthlyFileSuffix), ReadSeries);

            var added = await import.ImportAsync(country, series, cancellationToken);
            logger.LogInformation("Macro indicators {Country}: {Series} series, {Added} new values", country, series.Count, added);
        }
    }

    private static string IndicatorCodeOf(string path, string monthlyFileSuffix) => Path.GetFileName(path)[..^monthlyFileSuffix.Length];

    internal static IReadOnlyList<(DateOnly Period, double Value)> ReadSeries(string path)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<StooqRowDto>().Select(x => (x.Date, x.Close)).ToList();
    }
}
