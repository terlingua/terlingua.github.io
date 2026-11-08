---
title: Connection factories and dependency injection
questions:
  - id: factory
    prompt: Why inject an `IDbConnectionFactory` instead of a connection string into repositories?
    options:
      - text: Connection strings cannot be injected
      - text: It makes queries run faster
      - text: Dapper requires it
      - id: dip
        text: Repositories depend on an abstraction for creating connections, so providers, configuration and test setups can change without touching them
        correct: true
    explanation: This is dependency inversion — the data layer asks for "a connection", and the composition root decides how to build one.
  - id: singleton-connection
    prompt: Is registering a single open `SqlConnection` as a singleton a good idea?
    options:
      - text: Yes, it avoids the cost of opening connections
      - text: Yes, as long as the connection string is in configuration
      - id: not-thread-safe
        text: No — connections are not thread-safe, and pooling already makes opening cheap
        correct: true
    explanation: Create a connection per unit of work and dispose it; the provider's pool reuses physical connections.
  - id: datasource
    prompt: Which statements about `DbDataSource` (.NET 7+) are true?
    type: multiple
    options:
      - text: It is a provider-agnostic factory for connections and commands
        correct: true
      - text: "Npgsql's `NpgsqlDataSource` is a common implementation and is designed to be a singleton"
        correct: true
      - text: "`dataSource.OpenConnectionAsync()` returns an already-open connection"
        correct: true
      - text: It replaces the need for SQL
    explanation: "`DbDataSource` is the modern built-in abstraction for creating connections."
---
## Learning objectives

- Create connections through an abstraction
- Register data access correctly in DI
- Use `DbDataSource` where available

## A small factory

```csharp
public interface IDbConnectionFactory
{
    Task<DbConnection> OpenAsync(CancellationToken ct = default);
}

public sealed class SqlConnectionFactory(IOptions<DatabaseOptions> options) : IDbConnectionFactory
{
    public async Task<DbConnection> OpenAsync(CancellationToken ct = default)
    {
        var connection = new SqlConnection(options.Value.ConnectionString);
        await connection.OpenAsync(ct);
        return connection;
    }
}

builder.Services.AddOptions<DatabaseOptions>().BindConfiguration("Database").ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
```

## DbDataSource

.NET 7 introduced `System.Data.Common.DbDataSource`. Npgsql implements it fully:

```csharp
builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("Shop")!);

public sealed class ProductQueries(NpgsqlDataSource dataSource)
{
    public async Task<IReadOnlyList<ProductSummary>> ListAsync(CancellationToken ct)
    {
        await using var connection = await dataSource.OpenConnectionAsync(ct);
        var rows = await connection.QueryAsync<ProductSummary>(
            new CommandDefinition("SELECT id, name, price FROM products ORDER BY name", cancellationToken: ct));
        return rows.AsList();
    }
}
```

`AddNpgsqlDataSource` comes from the `Npgsql.DependencyInjection` package. `AsList()` is a Dapper
helper that avoids copying when the result is already a `List<T>`.

## Lifetimes

| Thing | Lifetime |
|-------|----------|
| Connection factory / data source | Singleton |
| Repositories / query services (stateless) | Singleton or scoped |
| Connections | Per operation or per unit of work — always disposed |
| Transactions | Per unit of work |

## Identity-based connections

In the cloud, prefer passwordless authentication — for example `Authentication=Active Directory Default`
in an Azure SQL connection string with a managed identity — so no secret appears in configuration at all.
