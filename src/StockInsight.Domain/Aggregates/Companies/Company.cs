using StockInsight.Domain.Abstract;
using StockInsight.Domain.Interfaces;
using StockInsight.Domain.Statements;

namespace StockInsight.Domain.Aggregates.Companies;

public sealed class Company : Entity, IAggregateRoot
{
    private readonly List<Statement> _statements = [];

    public Guid Id { get; private init; }
    public string Slug { get; private init; } = string.Empty;
    public FinancialProfile FinancialProfile { get; private init; }

    public IReadOnlyCollection<Statement> Statements => _statements.AsReadOnly();

    private Company()
    {
    }

    public static Company Create(string slug, FinancialProfile financialProfile)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);
        return new Company
        {
            Id = Guid.CreateVersion7(),
            Slug = slug,
            FinancialProfile = financialProfile,
        };
    }

    public void AddStatements(IEnumerable<Statement> statements)
    {
        var latest = _statements
            .GroupBy(x => x.Period)
            .ToDictionary(x => x.Key, x => x.MaxBy(s => s.FetchedAt)!);

        var added = new List<Statement>();
        foreach (var statement in statements)
        {
            if (latest.TryGetValue(statement.Period, out var stored) && stored.HasSameContent(statement))
            {
                continue;
            }

            if (stored?.FetchedAt == statement.FetchedAt)
            {
                throw new InvalidDataException($"{Slug}: period {statement.Period:yyyy-MM-dd} changed again on the same day {statement.FetchedAt:yyyy-MM-dd}");
            }

            added.Add(statement);
            latest[statement.Period] = statement;
        }

        _statements.AddRange(added);
    }
}
