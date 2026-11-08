---
title: Validation, OpenAPI and problem details
questions:
  - id: add-validation
    prompt: In .NET 10, what does `builder.Services.AddValidation()` enable for minimal APIs?
    options:
      - text: Validation of JWT tokens
      - text: Validation of `appsettings.json`
      - id: data-annotations
        text: Automatic validation of DataAnnotations on endpoint parameters, returning a 400 problem response when invalid
        correct: true
      - text: HTML form validation in the browser
    explanation: Built-in minimal API validation arrived in .NET 10, using a source generator to discover validatable types.
  - id: openapi
    prompt: Which statements about OpenAPI in ASP.NET Core 10 are true?
    type: multiple
    options:
      - text: "`AddOpenApi()` and `MapOpenApi()` generate an OpenAPI document from `Microsoft.AspNetCore.OpenApi`"
        correct: true
      - text: The generated document uses OpenAPI 3.1 by default
        correct: true
      - text: The default document is served at `/openapi/v1.json`
        correct: true
      - text: Swashbuckle's Swagger UI is included in the default `webapi` template
    explanation: Microsoft's built-in generator replaced Swashbuckle in the templates starting with .NET 9; you can still add a UI such as Scalar or Swagger UI yourself.
  - id: problem-details
    prompt: What is RFC 9457 Problem Details?
    options:
      - text: A logging format for Windows Event Log
      - text: A JSON schema for configuration files
      - id: standard
        text: A standard JSON format for describing HTTP API errors with fields like `type`, `title`, `status` and `detail`
        correct: true
    explanation: Problem Details gives clients one predictable error shape. ASP.NET Core produces it via `AddProblemDetails`.
  - id: exception-handler
    prompt: In .NET 8 and later, which interface lets you map specific exceptions to HTTP responses in the exception-handling middleware?
    options:
      - id: iexceptionhandler
        text: "`IExceptionHandler`"
        correct: true
      - text: "`IExceptionFilter`"
      - text: "`IMiddleware`"
    explanation: Register implementations with `AddExceptionHandler<T>()` and enable them with `UseExceptionHandler()`.
---
## Learning objectives

- Validate requests with .NET 10's built-in minimal API validation
- Generate OpenAPI documents
- Return consistent errors with Problem Details

## Validation

```csharp
builder.Services.AddValidation();

public sealed class CreateProduct
{
    [Required, StringLength(100)]
    public required string Name { get; init; }

    [Range(0.01, 100_000)]
    public decimal Price { get; init; }
}
```

An invalid body now produces a `400 Bad Request` with a validation problem response, and the
handler never runs. Validation also applies to query and header parameters, and you can opt out
per endpoint with `.DisableValidation()`.

## OpenAPI

```csharp
builder.Services.AddOpenApi();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();           // GET /openapi/v1.json
}
```

Typed results, `WithName`, `WithTags`, `WithSummary` and `Produces<T>` enrich the document. In
.NET 10 the default is OpenAPI 3.1, and YAML output is available by mapping a `.yaml` route.

## Problem details everywhere

```csharp
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DomainExceptionHandler>();

app.UseExceptionHandler();
app.UseStatusCodePages();
```

```csharp
public sealed class DomainExceptionHandler(IProblemDetailsService problems) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        var (status, title) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            ConcurrencyException => (StatusCodes.Status409Conflict, "The resource was modified"),
            _ => (0, "")
        };
        if (status == 0) return false;

        context.Response.StatusCode = status;
        return await problems.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            Exception = exception,
            ProblemDetails = { Title = title, Status = status }
        });
    }
}
```

Returning `false` passes unknown exceptions on to the default handler, which returns a generic
500 without leaking stack traces in production.
