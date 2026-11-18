using Dapper;
using Npgsql;

namespace StockInsight.Infrastructure.Persistence;

public sealed class SqlRepository(NpgsqlDataSource dataSource) : ISqlRepository
{
    static SqlRepository() => DefaultTypeMap.MatchNamesWithUnderscores = true;

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? parameters, CancellationToken cancellationToken)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        return [.. await connection.QueryAsync<T>(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken))];
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters, CancellationToken cancellationToken)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
    }
}
