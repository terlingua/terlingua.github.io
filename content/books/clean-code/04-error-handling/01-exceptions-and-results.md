---
title: Exceptions and result types
questions:
  - id: rethrow
    prompt: Inside a `catch (Exception ex)` block, which statement rethrows while preserving the original stack trace?
    options:
      - text: "`throw ex;`"
      - text: "`throw new Exception(ex.Message);`"
      - id: throw
        text: "`throw;`"
        correct: true
      - text: "`return ex;`"
    explanation: "`throw;` rethrows the current exception unchanged. `throw ex;` resets the stack trace to the current line."
  - id: when-exception
    prompt: When is an exception the most appropriate way to signal a problem?
    options:
      - text: When a user types an invalid e-mail address into a form
      - text: For normal control flow such as ending a loop
      - id: exceptional
        text: When a method cannot fulfil its contract for an unexpected reason, such as a lost database connection
        correct: true
    explanation: Expected validation outcomes are better modelled as return values. Exceptions are for situations the caller does not expect as part of the normal flow.
  - id: good-practice
    prompt: Which are good exception-handling practices in .NET?
    type: multiple
    options:
      - text: Catch only exceptions you can actually handle
        correct: true
      - text: Use exception filters such as `catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)`
        correct: true
      - text: Wrap low-level exceptions in domain exceptions and keep the original as `InnerException`
        correct: true
      - text: Catch `Exception` everywhere and ignore it so the app never crashes
    explanation: Swallowing exceptions hides failures. Catch narrowly, add context and let unknown failures reach a global handler.
---
## Learning objectives

- Use exceptions for exceptional situations
- Model expected failures with result types
- Rethrow and wrap exceptions correctly

## Exceptions are for the unexpected

```csharp
try
{
    await _payments.ChargeAsync(order.Total, ct);
}
catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.ServiceUnavailable)
{
    throw new PaymentProviderUnavailableException(order.Id, ex);
}
```

Catch only what you can handle, add context, preserve the inner exception, and let everything else
bubble up to a global handler (`UseExceptionHandler` in ASP.NET Core) that logs it once.

## Do not lose the stack trace

```csharp
catch (Exception ex)
{
    _logger.LogError(ex, "Import failed for {File}", path);
    throw;      // preserves the stack trace
    // throw ex; would reset it
}
```

## Expected failures as values

A user entering a duplicate username is not exceptional; it is a normal outcome. Model it as data:

```csharp
public abstract record RegistrationResult
{
    public sealed record Success(UserId Id) : RegistrationResult;
    public sealed record UsernameTaken(string Username) : RegistrationResult;
    public sealed record WeakPassword(IReadOnlyList<string> Problems) : RegistrationResult;
}

var message = await registration.RegisterAsync(form, ct) switch
{
    RegistrationResult.Success s => $"Welcome! Your id is {s.Id}",
    RegistrationResult.UsernameTaken t => $"'{t.Username}' is taken",
    RegistrationResult.WeakPassword w => string.Join(", ", w.Problems),
    _ => throw new UnreachableException()
};
```

The compiler and the type system now force callers to consider every outcome, and the happy path
does not pay the cost of exceptions.

## Guard clauses for programmer errors

```csharp
public void Ship(Order order, int quantity)
{
    ArgumentNullException.ThrowIfNull(order);
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
}
```

Throw helpers such as `ThrowIfNull` and `ThrowIfNegativeOrZero` (.NET 8+) keep guards short and
consistent.
