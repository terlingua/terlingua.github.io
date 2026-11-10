---
title: Errors, results and problem details
questions:
  - id: exceptions-for
    prompt: When are exceptions the right mechanism?
    options:
      - text: For every validation failure on user input
      - id: exceptional
        text: For conditions the immediate caller cannot reasonably handle — broken invariants, unavailable infrastructure, programming errors
        correct: true
      - text: For normal control flow such as "customer not found" in a search
      - text: Never; result types should replace all exceptions
    explanation: Expected outcomes (not found, invalid input, declined payment) are part of the domain and deserve types. Unexpected failures deserve exceptions.
  - id: result-types
    prompt: What is the main advantage of returning an explicit result (such as a closed `PaymentResult` hierarchy) for expected failures?
    options:
      - text: It is faster in all cases
      - text: It removes the need for logging
      - text: It prevents all bugs
      - id: visible
        text: The possible outcomes are visible in the signature, and pattern matching makes callers handle each one
        correct: true
    explanation: With C# 15 unions and closed hierarchies, the compiler can check that every case is handled.
  - id: rfc
    prompt: What is RFC 9457?
    options:
      - id: problem-details
        text: "\"Problem Details for HTTP APIs\" — a standard JSON format (`application/problem+json`) for error responses, superseding RFC 7807"
        correct: true
      - text: The HTTP/3 specification
      - text: A JSON schema for OpenAPI
      - text: An OAuth 2.1 extension
    explanation: ASP.NET Core supports it with `AddProblemDetails()`, `Results.Problem` and `Results.ValidationProblem`.
  - id: catch-all
    prompt: What is wrong with `catch (Exception) { return null; }` in a repository method?
    type: multiple
    options:
      - id: hides
        text: It hides the cause — a timeout, a permission error and a bug all look like "not found"
        correct: true
      - id: cancel
        text: It swallows `OperationCanceledException`, turning cancellation into a fake result
        correct: true
      - id: no-log
        text: Nothing is logged, so the failure is invisible in production
        correct: true
      - text: "`null` cannot be returned from async methods"
    explanation: Catch what you can handle, where you can handle it. Let the rest reach a boundary that logs it and returns a 500 with a correlation id.
  - id: throw-helpers
    prompt: Which guard clauses are built into modern .NET?
    type: multiple
    options:
      - id: null-check
        text: "`ArgumentNullException.ThrowIfNull(x)`"
        correct: true
      - id: empty
        text: "`ArgumentException.ThrowIfNullOrWhiteSpace(s)`"
        correct: true
      - id: range
        text: "`ArgumentOutOfRangeException.ThrowIfNegative(n)` and friends"
        correct: true
      - text: "`Guard.Against.Null(x)` from the BCL"
    explanation: Third-party guard libraries are partly vestigial since .NET 6–8 added these helpers, which also capture the argument name automatically via `CallerArgumentExpression`.
  - id: exception-handler
    prompt: In ASP.NET Core 8+, what is `IExceptionHandler` for?
    options:
      - text: Catching exceptions inside a single controller only
      - text: Replacing `try/catch` in all code
      - text: Logging to the Windows event log
      - id: central
        text: Centrally translating unhandled exceptions into responses (for example problem details), registered with `AddExceptionHandler<T>()` and `UseExceptionHandler()`
        correct: true
    explanation: One place maps exception types to HTTP responses; endpoints stay clean.
---
## Learning objectives

- Choose between exceptions and result types
- Return standard problem details from APIs
- Use built-in guard clauses and central exception handling

## Two kinds of failure

| Kind | Examples | Mechanism |
|---|---|---|
| Expected, part of the domain | Validation failed, not found, payment declined, out of stock | Return a result/case; map to 4xx |
| Unexpected or infrastructural | Database down, bug, broken invariant | Throw; handle centrally; 5xx |

## Expected outcomes as types

```csharp
public abstract record PlaceOrderResult
{
    private PlaceOrderResult() { }
    public sealed record Placed(OrderId Id) : PlaceOrderResult;
    public sealed record OutOfStock(IReadOnlyList<string> Skus) : PlaceOrderResult;
    public sealed record PaymentDeclined(string Reason) : PlaceOrderResult;
}

app.MapPost("/orders", async (PlaceOrder cmd, OrderPlacement placement, CancellationToken ct) =>
    await placement.PlaceAsync(cmd, ct) switch
    {
        PlaceOrderResult.Placed p => Results.Created($"/orders/{p.Id}", new { id = p.Id }),
        PlaceOrderResult.OutOfStock o => Results.Problem(
            title: "Out of stock", statusCode: StatusCodes.Status409Conflict,
            extensions: new Dictionary<string, object?> { ["skus"] = o.Skus }),
        PlaceOrderResult.PaymentDeclined d => Results.Problem(
            title: "Payment declined", detail: d.Reason, statusCode: StatusCodes.Status402PaymentRequired),
        _ => throw new UnreachableException(),
    });
```

## Unexpected failures, handled once

```csharp
builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = ctx =>
        ctx.ProblemDetails.Extensions["traceId"] = Activity.Current?.Id ?? ctx.HttpContext.TraceIdentifier);
builder.Services.AddExceptionHandler<DatabaseUnavailableHandler>();

var app = builder.Build();
app.UseExceptionHandler();
app.UseStatusCodePages();
```

```csharp
public sealed class DatabaseUnavailableHandler(IProblemDetailsService problems) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if (exception is not SqlException { IsTransient: true }) return false;

        context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        context.Response.Headers.RetryAfter = "5";
        return await problems.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            ProblemDetails = { Title = "Temporarily unavailable", Status = 503 },
            Exception = exception,
        });
    }
}
```

## Guards

```csharp
public Invoice(CustomerId customer, IReadOnlyList<InvoiceLine> lines, DateOnly dueDate)
{
    ArgumentNullException.ThrowIfNull(lines);
    ArgumentOutOfRangeException.ThrowIfZero(lines.Count);
    Customer = customer;
    Lines = lines;
    DueDate = dueDate;
}
```

## Sources

- IETF, RFC 9457 "Problem Details for HTTP APIs" — <https://www.rfc-editor.org/rfc/rfc9457>
- Microsoft Learn, "Handle errors in ASP.NET Core" — <https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling>
- Microsoft Learn, "Best practices for exceptions" — <https://learn.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions>
