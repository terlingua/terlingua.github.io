---
title: Transactions
questions:
  - id: pass-tx
    prompt: You start a transaction with `connection.BeginTransaction()`. What must you do on each Dapper call that should participate?
    options:
      - text: Nothing; Dapper detects the transaction automatically
      - id: pass
        text: "Pass the transaction object, for example `transaction: tx`"
        correct: true
      - text: Open a new connection for each call
      - text: Call `tx.Enlist(command)` manually
    explanation: ADO.NET requires each command on that connection to be associated with the active transaction. Forgetting it causes an error.
  - id: open-first
    prompt: Why must the connection be open before calling `BeginTransaction()`?
    options:
      - text: Dapper requires it for logging
      - text: It does not need to be open
      - id: open
        text: A transaction belongs to an open physical connection; ADO.NET cannot begin one on a closed connection
        correct: true
    explanation: Open the connection explicitly, begin the transaction, run commands, then commit.
  - id: facts
    prompt: Which statements about transactions with Dapper are true?
    type: multiple
    options:
      - text: Disposing a transaction that was not committed rolls it back
        correct: true
      - text: Keep transactions short to reduce lock contention
        correct: true
      - text: "`TransactionScope` with `TransactionScopeAsyncFlowOption.Enabled` is needed for async code when using ambient transactions"
        correct: true
      - text: A transaction automatically spans calls made on different connections without any coordination
    explanation: Work across multiple connections requires a distributed transaction, which is best avoided.
---
## Learning objectives

- Run several commands atomically
- Avoid common transaction mistakes
- Encapsulate a unit of work

## Explicit transactions

```csharp
await using var connection = new SqlConnection(connectionString);
await connection.OpenAsync(ct);
await using var tx = await connection.BeginTransactionAsync(ct);

try
{
    var orderId = await connection.ExecuteScalarAsync<int>(
        "INSERT INTO Orders (CustomerId, Total) OUTPUT INSERTED.Id VALUES (@CustomerId, @Total)",
        new { order.CustomerId, order.Total }, tx);

    await connection.ExecuteAsync(
        "INSERT INTO OrderLines (OrderId, Sku, Quantity, UnitPrice) VALUES (@OrderId, @Sku, @Quantity, @UnitPrice)",
        order.Lines.Select(l => new { OrderId = orderId, l.Sku, l.Quantity, l.UnitPrice }), tx);

    await connection.ExecuteAsync(
        "UPDATE Stock SET Quantity = Quantity - @Quantity WHERE Sku = @Sku",
        order.Lines, tx);

    await tx.CommitAsync(ct);
}
catch
{
    await tx.RollbackAsync(ct);
    throw;
}
```

`BeginTransactionAsync` returns a `DbTransaction`; Dapper's `transaction` parameter accepts
`IDbTransaction`, which it implements.

## A unit of work

```csharp
public sealed class UnitOfWork(DbConnection connection) : IAsyncDisposable
{
    private DbTransaction? _tx;
    public DbConnection Connection => connection;
    public DbTransaction Transaction => _tx ?? throw new InvalidOperationException("Call BeginAsync first.");

    public async Task BeginAsync(CancellationToken ct)
    {
        if (connection.State != ConnectionState.Open) await connection.OpenAsync(ct);
        _tx = await connection.BeginTransactionAsync(ct);
    }

    public Task CommitAsync(CancellationToken ct) => Transaction.CommitAsync(ct);

    public async ValueTask DisposeAsync()
    {
        if (_tx is not null) await _tx.DisposeAsync();
        await connection.DisposeAsync();
    }
}
```

Repositories receive the `UnitOfWork` and pass `uow.Transaction` to every call.

## Ambient transactions

```csharp
using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
// open connections and run commands here
scope.Complete();
```

Without `TransactionScopeAsyncFlowOption.Enabled`, the ambient transaction does not flow across
`await`. Opening a second connection inside the scope may escalate to a distributed transaction,
which is not supported everywhere — prefer explicit transactions on one connection.
