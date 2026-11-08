---
title: Buffering and streaming large results
questions:
  - id: buffered-default
    prompt: By default, what does Dapper's `Query<T>` do with the rows it reads?
    options:
      - id: buffer
        text: Reads them all into a list before returning
        correct: true
      - text: Streams them one at a time while the reader stays open
      - text: Writes them to a temporary file
    explanation: Buffering is the default because it releases the connection quickly and avoids surprises if the caller enumerates slowly.
  - id: unbuffered-async
    prompt: Which method streams rows asynchronously as an `IAsyncEnumerable<T>`?
    options:
      - text: "`QueryMultipleAsync`"
      - text: "`QueryFirstAsync`"
      - text: "`ExecuteReaderAsync`"
      - id: unbuffered
        text: "`QueryUnbufferedAsync`"
        correct: true
    explanation: "`QueryUnbufferedAsync` (on `DbConnection`) yields rows as they arrive, ideal with `await foreach`."
  - id: tradeoffs
    prompt: Which statements about unbuffered queries are true?
    type: multiple
    options:
      - text: Memory use stays flat regardless of the number of rows
        correct: true
      - text: The connection stays busy until enumeration finishes
        correct: true
      - text: You cannot run another command on the same connection while enumerating (without MARS)
        correct: true
      - text: The results can be enumerated twice
    explanation: Streaming trades memory for connection time; the sequence is single-use.
---
## Learning objectives

- Understand buffered versus unbuffered queries
- Stream large result sets with `QueryUnbufferedAsync`

## Buffered by default

```csharp
var all = await db.QueryAsync<Product>("SELECT Id, Name, Price FROM Products");
// all rows are already in memory here
```

Great for typical result sizes: the reader is closed and the connection released immediately.

## Streaming

```csharp
await using var connection = new SqlConnection(connectionString);

await foreach (var row in connection.QueryUnbufferedAsync<AuditRow>(
    "SELECT Id, At, UserId, Action FROM AuditLog WHERE At >= @since", new { since }))
{
    await writer.WriteLineAsync($"{row.At:O},{row.UserId},{row.Action}");
}
```

Memory stays flat even for millions of rows — useful for exports and ETL jobs.

The synchronous API also supports streaming with `buffered: false`:

```csharp
foreach (var row in connection.Query<AuditRow>(sql, param, buffered: false)) { /* ... */ }
```

## Streaming to an HTTP response

```csharp
app.MapGet("/export/audit", (DbConnection db, DateTime since) =>
    db.QueryUnbufferedAsync<AuditRow>("SELECT Id, At, UserId, Action FROM AuditLog WHERE At >= @since", new { since }));
```

This assumes a scoped `DbConnection` registration (see the next chapter). Returning `IAsyncEnumerable<T>` from a minimal API endpoint streams a JSON array to the client as
rows are read, without collecting them first.

## Rules of thumb

| Situation | Choose |
|-----------|--------|
| Up to a few thousand rows | Buffered (default) |
| Exports, reports, ETL with large volumes | Unbuffered/streaming |
| Need to run more commands while iterating | Buffered, or a second connection |
