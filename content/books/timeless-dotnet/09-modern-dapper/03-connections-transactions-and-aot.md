---
title: Connections, transactions and Dapper.AOT
questions:
  - id: pooling
    prompt: Why is opening a connection per operation (`await using var c = await db.OpenConnectionAsync(ct)`) not expensive?
    options:
      - text: Because Dapper caches connections in a static field
      - text: Because SQL Server keeps every connection open forever
      - text: It is expensive; use one global connection instead
      - id: pool
        text: ADO.NET providers pool physical connections; "open" usually takes one from the pool and "dispose" returns it
        correct: true
    explanation: Short-lived logical connections are the intended pattern. A single shared global connection is not thread-safe and is a vestige of desktop-era code.
  - id: tx-param
    prompt: You started `var tx = await connection.BeginTransactionAsync(ct)`. What must you do for each Dapper call that should be part of it?
    options:
      - text: Nothing; Dapper detects the open transaction automatically
      - text: Call `SqlMapper.Enlist(tx)` once
      - text: Wrap each call in `TransactionScope`
      - id: pass-tx
        text: Pass `tx` as the transaction (for example in `CommandDefinition`), or the provider throws because the command's transaction is not set
        correct: true
    explanation: ADO.NET requires each command to be associated with the active transaction. Forgetting is a common runtime error with SqlClient.
  - id: transactionscope
    prompt: Which statement about `TransactionScope` with async code is accurate?
    options:
      - text: It works with async by default in every constructor
      - text: It is required for Dapper transactions
      - text: It cannot be used on .NET 10
      - id: async-flow
        text: You must create it with `TransactionScopeAsyncFlowOption.Enabled`, or the ambient transaction does not flow across awaits
        correct: true
    explanation: Explicit `DbTransaction` is simpler and more predictable for single-database work. Distributed transactions across resources are supported on Windows since .NET 7, but are rarely the right design.
  - id: aot-enable
    prompt: How do you turn on Dapper.AOT's build-time code generation for a project?
    type: multiple
    options:
      - id: package
        text: Add the `Dapper.AOT` package
        correct: true
      - id: attribute
        text: "Opt in with `[module: DapperAot]` (or `[DapperAot]` on a type or method)"
        correct: true
      - id: interceptors
        text: Add `Dapper.AOT` to the `InterceptorsNamespaces` MSBuild property (and `InterceptorsPreviewNamespaces` for the .NET 8 SDK)
        correct: true
      - text: Call `SqlMapper.EnableAot()` at startup
    explanation: Dapper.AOT uses C# interceptors to replace Dapper call sites with generated code. Analyzer-only guidance is available separately as `Dapper.Advisor`.
  - id: aot-limits
    prompt: Which are limitations of Dapper.AOT you should know about?
    type: multiple
    options:
      - id: inline-only
        text: Only direct, inline Dapper calls are intercepted; calls through your own generic helper methods are not
        correct: true
      - id: runtime-config
        text: Runtime configuration such as `SqlMapper.AddTypeHandler` is ignored; type handlers are declared with attributes instead
        correct: true
      - id: non-generic
        text: Non-generic overloads like `Query(typeof(Foo), ...)` are not supported
        correct: true
      - text: It only works with PostgreSQL
    explanation: Un-intercepted call sites fall back to normal Dapper, which works under JIT but is not AOT-safe. Test under a real AOT publish.
  - id: outbox
    prompt: An order is saved and an `OrderPlaced` message must be published. Which design avoids "saved but never published" and "published but never saved"?
    options:
      - text: Save, then publish, inside a `try/catch`
      - text: Publish first, then save
      - id: outbox
        text: The transactional outbox — insert the message into an outbox table in the same database transaction, and publish from the outbox afterwards
        correct: true
      - text: Use a `CancellationToken` to roll back the publish
    explanation: One local transaction covers both writes. A background publisher delivers outbox rows at least once; consumers handle duplicates idempotently.
---
## Learning objectives

- Manage connection lifetimes with pooling in mind
- Use explicit transactions correctly with Dapper
- Enable Dapper.AOT and understand its limits

## Connection lifetime

```csharp
await using var connection = await db.OpenConnectionAsync(ct);
```

Open late, dispose early. The pool does the rest. Keep connections out of fields and DI lifetimes
longer than an operation.

## Explicit transactions

```csharp
public async Task<OrderId> PlaceAsync(NewOrder order, CancellationToken ct)
{
    await using var connection = await db.OpenConnectionAsync(ct);
    await using var tx = await connection.BeginTransactionAsync(ct);

    var id = OrderId.New();
    await connection.ExecuteAsync(new CommandDefinition("""
        insert dbo.Orders (Id, CustomerId, PlacedAt, Status)
        values (@Id, @CustomerId, sysutcdatetime(), 'Placed')
        """, new { Id = id.Value, CustomerId = order.Customer.Value }, tx, cancellationToken: ct));

    await connection.ExecuteAsync(new CommandDefinition("""
        insert dbo.OrderLines (OrderId, Sku, Quantity, UnitPrice)
        values (@OrderId, @Sku, @Quantity, @UnitPrice)
        """, order.Lines.Select(l => new { OrderId = id.Value, l.Sku, l.Quantity, l.UnitPrice }), tx, cancellationToken: ct));

    await connection.ExecuteAsync(new CommandDefinition("""
        insert dbo.Outbox (Id, Type, Payload, CreatedAt)
        values (newid(), 'OrderPlaced', @payload, sysutcdatetime())
        """, new { payload = JsonSerializer.Serialize(new OrderPlaced(id, order.Customer)) }, tx, cancellationToken: ct));

    await tx.CommitAsync(ct);
    return id;
}
```

Passing a sequence as the parameter makes Dapper execute the statement once per element — fine
for tens of lines. For thousands, use a TVP, `SqlBulkCopy`, or PostgreSQL `COPY`.

## Dapper.AOT

```xml
<PropertyGroup>
  <InterceptorsNamespaces>$(InterceptorsNamespaces);Dapper.AOT</InterceptorsNamespaces>
  <InterceptorsPreviewNamespaces>$(InterceptorsPreviewNamespaces);Dapper.AOT</InterceptorsPreviewNamespaces>
</PropertyGroup>
<ItemGroup>
  <PackageReference Include="Dapper" />
  <PackageReference Include="Dapper.AOT" />
</ItemGroup>
```

```csharp
[module: DapperAot]
```

Benefits beyond Native AOT: no runtime IL emission (faster startup), and analyzers that review
your SQL usage at build time (for example DAP219 for `select *`).

Write calls inline so they are intercepted:

```csharp
var rows = await connection.QueryAsync<ProductRow>(
    "select Sku, Name, Price, AvailableFrom from dbo.Products where CategoryId = @categoryId",
    new { categoryId });
```

## Testing the data layer

Test SQL against a real database engine — not mocks of `IDbConnection`. Testcontainers starts SQL
Server or PostgreSQL in Docker for the test run:

```csharp
public sealed class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder().Build();
    public DbDataSource DataSource { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();
        DataSource = SqlClientFactory.Instance.CreateDataSource(_container.GetConnectionString());
        await Migrations.ApplyAsync(DataSource);
    }

    public async ValueTask DisposeAsync() => await _container.DisposeAsync();
}
```

## Sources

- Dapper.AOT, "Getting started" — <https://aot.dapperlib.dev/gettingstarted>
- Dapper.AOT FAQ — <https://aot.dapperlib.dev/faq>
- Microsoft Learn, "SQL Server connection pooling" — <https://learn.microsoft.com/en-us/sql/connect/ado-net/sql-server-connection-pooling>
- Microsoft Learn, "Transactional outbox pattern" — <https://learn.microsoft.com/en-us/azure/architecture/databases/guide/transactional-outbox-cosmos>
- Testcontainers for .NET — <https://dotnet.testcontainers.org/>
