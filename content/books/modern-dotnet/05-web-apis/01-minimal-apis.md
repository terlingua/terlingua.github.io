---
title: Minimal APIs
questions:
  - id: typed-results
    prompt: "Why declare an endpoint's return type as `Results<Ok<Product>, NotFound>`?"
    options:
      - text: It makes the endpoint run faster
      - id: metadata
        text: The possible responses become part of the signature, which improves OpenAPI metadata and lets the compiler check them
        correct: true
      - text: It is required for every minimal API endpoint
      - text: It enables output caching
    explanation: "`TypedResults` plus the `Results<...>` union type describe all outcomes statically, so OpenAPI generation and unit tests can rely on them."
  - id: binding
    prompt: "In `app.MapGet(\"/products/{id:int}\", (int id, IProductStore store, CancellationToken ct) => ...)`, where do the parameters come from?"
    options:
      - id: sources
        text: "`id` from the route, `store` from DI and `ct` from the request's aborted token"
        correct: true
      - text: All three come from the query string
      - text: All three come from the request body
    explanation: Minimal APIs infer binding sources — route values, registered services and special types like `CancellationToken` — without attributes.
  - id: groups
    prompt: "What does `app.MapGroup(\"/api/orders\")` help with?"
    options:
      - id: prefix
        text: Applying a shared route prefix and shared conventions (authorization, filters, tags) to many endpoints
        correct: true
      - text: Grouping database tables
      - text: Load balancing
    explanation: Route groups keep related endpoints together and avoid repeating prefixes and metadata.
  - id: organise
    prompt: Which are good ways to keep minimal API code organised as an app grows?
    type: multiple
    options:
      - text: Put each feature's endpoints in an extension method such as `MapOrderEndpoints`
        correct: true
      - text: Use named handler methods instead of large inline lambdas
        correct: true
      - text: Keep business logic in services the handlers call
        correct: true
      - text: Put every endpoint and all business logic directly in `Program.cs`
    explanation: Minimal does not mean monolithic — group by feature and keep handlers thin.
---
## Learning objectives

- Build endpoints with minimal APIs
- Use typed results and route groups
- Organise endpoints by feature

## A first API

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IProductStore, InMemoryProductStore>();

var app = builder.Build();
app.MapProductEndpoints();
app.Run();
```

## Feature-based endpoint modules

```csharp
public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/products").WithTags("Products");

        group.MapGet("/", ListAsync);
        group.MapGet("/{id:int}", GetAsync).WithName("GetProduct");
        group.MapPost("/", CreateAsync).RequireAuthorization("catalog-admin");

        return routes;
    }

    private static async Task<Ok<IReadOnlyList<Product>>> ListAsync(IProductStore store, CancellationToken ct) =>
        TypedResults.Ok(await store.ListAsync(ct));

    internal static async Task<Results<Ok<Product>, NotFound>> GetAsync(int id, IProductStore store, CancellationToken ct) =>
        await store.FindAsync(id, ct) is { } product
            ? TypedResults.Ok(product)
            : TypedResults.NotFound();

    private static async Task<CreatedAtRoute<Product>> CreateAsync(CreateProduct request, IProductStore store, CancellationToken ct)
    {
        var product = await store.AddAsync(request, ct);
        return TypedResults.CreatedAtRoute(product, "GetProduct", new { id = product.Id });
    }
}

public sealed record CreateProduct(string Name, decimal Price);
```

## Binding sources

| Parameter | Bound from |
|-----------|------------|
| Matches a route segment (`{id}`) | Route |
| Simple type not in the route | Query string |
| Complex type on POST/PUT | JSON body |
| Registered service | DI container |
| `HttpContext`, `CancellationToken`, `ClaimsPrincipal` | Special types |

Use `[FromQuery]`, `[FromHeader]`, `[FromBody]`, `[FromServices]` or `[AsParameters]` when you need
to be explicit.

## Testing handlers

Because handlers are ordinary static methods returning typed results, you can unit test them directly:

```csharp
var result = await ProductEndpoints.GetAsync(42, new InMemoryProductStore(), CancellationToken.None);
Assert.IsType<NotFound>(result.Result);
```

`GetAsync` is `internal`, so the test project needs `<InternalsVisibleTo Include="Shop.Api.Tests" />` in the API project file.
