---
title: What cancellation is NOT
questions:
  - id: not-abort
    prompt: Your code calls a third-party SDK method `Compute(input)` that accepts no token and takes 10 minutes. You cancel your token after 5 seconds. What happens to `Compute`?
    options:
      - text: It stops immediately
      - id: keeps-running
        text: It keeps running to completion; tokens cannot stop code that does not observe them
        correct: true
      - text: The runtime aborts its thread
      - text: It throws `OperationCanceledException` at its next line
    explanation: Cancellation is a request, not an abort. If you must hard-stop uncooperative work, isolate it in a separate process you can kill.
  - id: waitasync-not-stop
    prompt: "What does `await slowTask.WaitAsync(TimeSpan.FromSeconds(5))` do when the timeout elapses?"
    options:
      - id: stops-waiting
        text: Throws `TimeoutException` to *you*, while `slowTask` continues running in the background
        correct: true
      - text: Cancels `slowTask`
      - text: Blocks for 5 seconds then returns null
      - text: Restarts `slowTask`
    explanation: "`WaitAsync` abandons the wait, not the work. That is sometimes exactly what you want — but understand that the work still consumes resources and may still have side effects."
  - id: not-rollback
    prompt: A method writes three rows and is cancelled after the second write. What does cancellation do to the first two rows?
    options:
      - text: Rolls them back automatically
      - text: Marks them as deleted
      - id: nothing
        text: Nothing — cancellation does not undo side effects; atomicity needs a transaction (or idempotent, compensatable steps)
        correct: true
      - text: Moves them to a dead-letter queue
    explanation: If the three writes must be all-or-nothing, put them in one transaction. A cancelled `await` inside the transaction leads to disposal without commit, which rolls back.
  - id: not-a-guarantee
    prompt: You cancel an HTTP POST that charges a credit card. The `await` throws `OperationCanceledException`. Was the card charged?
    options:
      - id: unknown
        text: Unknown — the request may have reached the server and completed; cancellation only tells you that *you* stopped waiting
        correct: true
      - text: No, cancellation guarantees the server did nothing
      - text: Yes, always
      - text: Only if the response status was 200
    explanation: This is why payment APIs use idempotency keys. Retrying with the same key is safe whether or not the first attempt succeeded.
  - id: point-of-no-return
    prompt: In a checkout flow, after the payment provider has confirmed a charge, which token should the code use to save the order?
    options:
      - text: The request's `HttpContext.RequestAborted` token
      - id: none
        text: "`CancellationToken.None` (or a token tied only to application shutdown) — the user closing the tab must not stop us recording a payment we already took"
        correct: true
      - text: A token that cancels after 1 millisecond
      - text: No token; the method should not be async
    explanation: Past the point of no return, honoring the client's cancellation creates inconsistent state. Choose tokens deliberately per step, not mechanically.
  - id: not-error
    prompt: How should ASP.NET Core log a request that ended because the client disconnected?
    options:
      - text: As an error with a stack trace and an alert to on-call
      - text: It should crash the process
      - id: not-error
        text: As informational or debug at most — a client going away is not a server fault
        correct: true
      - text: As a security incident
    explanation: Treating cancellations as errors floods dashboards and pages people at 3 a.m. for users closing their browser. Filter with `catch (OperationCanceledException) when (ct.IsCancellationRequested)`.
  - id: not-swallow
    prompt: "What is wrong with `catch (OperationCanceledException) { return Array.Empty<Order>(); }` deep inside a data access method?"
    options:
      - text: "`Array.Empty` allocates"
      - text: It cannot compile
      - id: lies
        text: It converts "cancelled" into "no results", so callers proceed as if the query succeeded — possibly caching or acting on empty data
        correct: true
      - text: Nothing is wrong with it
    explanation: Let cancellation propagate. Only the code that *initiated* the cancellation (or a framework boundary) should decide what it means.
  - id: myths
    prompt: Which statements are myths?
    type: multiple
    options:
      - id: myth-timeout
        text: "\"A token is a timeout.\" (A token is a signal; a timeout is one *reason* a source might signal.)"
        correct: true
      - id: myth-threads
        text: "\"Cancelling a token frees threads immediately.\""
        correct: true
      - text: "\"Cancellation is cooperative.\""
      - id: myth-optional
        text: "\"If my method accepts a token, callers are guaranteed fast cancellation.\" (Only if the method actually observes it everywhere.)"
        correct: true
    explanation: Cooperative is the one true statement. The others confuse the signal with its effects.
---
## Learning objectives

- Distinguish cancellation from abort, timeout, rollback and failure
- Choose deliberately when *not* to honor a caller's token
- Avoid swallowing or misreporting cancellation

## Five things a token is not

### 1. Not an abort

A token cannot stop code that ignores it. `Thread.Abort` is gone from modern .NET, deliberately:
forcibly unwinding arbitrary code corrupts state. If you need a kill switch for untrusted or
uncooperative work, run it in a separate **process** and kill that.

### 2. Not a timeout by itself

A timeout is one policy that *triggers* cancellation (`CancelAfter`). There are others: user
clicked Cancel, client disconnected, host is shutting down, a sibling operation won a race.

### 3. Not a rollback

```csharp
await db.InsertOrderAsync(order, ct);
await db.InsertLinesAsync(order.Lines, ct);
await db.ReserveStockAsync(order.Lines, ct);
```

Cancellation between lines leaves partial state. Use a transaction:

```csharp
await using var connection = await dataSource.OpenConnectionAsync(ct);
await using var tx = await connection.BeginTransactionAsync(ct);
await connection.ExecuteAsync(new CommandDefinition(InsertOrderSql, order, tx, cancellationToken: ct));
await connection.ExecuteAsync(new CommandDefinition(InsertLinesSql, order.Lines, tx, cancellationToken: ct));
await connection.ExecuteAsync(new CommandDefinition(ReserveStockSql, order.Lines, tx, cancellationToken: ct));
await tx.CommitAsync(ct);
```

If cancellation strikes before `CommitAsync`, disposal rolls back.

### 4. Not a guarantee the other side stopped

Once bytes leave the process, cancellation only means *you stopped listening*. Distributed
operations need **idempotency**:

```csharp
using var request = new HttpRequestMessage(HttpMethod.Post, "v1/charges")
{
    Content = JsonContent.Create(charge),
};
request.Headers.Add("Idempotency-Key", charge.OrderId.ToString());
using var response = await http.SendAsync(request, ct);
```

### 5. Not an error

A cancelled request is an expected outcome. It should not produce error logs, failed-request
alerts or 500 responses. ASP.NET Core logs client disconnects quietly and does not write a
response when the client is gone.

## Choosing *not* to honor a token

```csharp
app.MapPost("/checkout", async (Checkout cmd, IPayments payments, IOrders orders, CancellationToken ct) =>
{
    var charge = await payments.ChargeAsync(cmd.PaymentMethod, cmd.Total, cmd.IdempotencyKey, ct);

    var order = await orders.RecordPaidOrderAsync(cmd, charge, CancellationToken.None);

    return Results.Created($"/orders/{order.Id}", order);
});
```

Before the charge, abandoning is fine — nothing happened, or the idempotency key protects a
retry. After the charge, the user's browser closing must not prevent us from recording that we
took their money. This is a business decision expressed in a token choice.

## Sources

- Microsoft Learn, "Cancellation in managed threads" — <https://learn.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads>
- Microsoft Learn, `Task.WaitAsync` — <https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.waitasync>
- Stripe API, "Idempotent requests" — <https://docs.stripe.com/api/idempotent_requests>
- Microsoft Learn, "Thread.Abort is obsolete" (SYSLIB0006) — <https://learn.microsoft.com/en-us/dotnet/fundamentals/syslib-diagnostics/syslib0006>
