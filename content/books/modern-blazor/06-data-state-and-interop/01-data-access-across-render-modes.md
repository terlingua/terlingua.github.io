---
title: Data access across render modes
questions:
  - id: interface
    prompt: A product list component may run on the server or in WebAssembly (Auto mode). What is the cleanest way to load its data?
    options:
      - id: abstraction
        text: Inject an `IProductService` interface — implemented with direct data access on the server and with `HttpClient` calls in the client project
        correct: true
      - text: Inject the EF Core `DbContext` directly into the component
      - text: Always call JavaScript `fetch` through interop
      - text: Hard-code the data into the component
    explanation: Depending on an abstraction lets the same component run in both environments; each host registers its own implementation.
  - id: dbcontext-wasm
    prompt: Why can't a component running on WebAssembly use a `DbContext` that connects to your SQL Server?
    options:
      - text: EF Core does not support .NET 10
      - id: browser
        text: Code runs in the browser sandbox, which cannot open database connections, and any credentials shipped to it would be public
        correct: true
      - text: SQL Server blocks all Blazor apps
    explanation: Browser code talks to the server through HTTP APIs. Secrets must never be shipped to the client.
  - id: circuit-db
    prompt: Which statements about using EF Core in Interactive Server components are true?
    type: multiple
    options:
      - text: A scoped `DbContext` would live for the whole circuit, which can be long and is not thread-safe
        correct: true
      - text: "`IDbContextFactory<T>` lets you create a short-lived context per operation"
        correct: true
      - text: Contexts created from the factory should be disposed, for example with `await using`
        correct: true
      - text: Sharing one `DbContext` across all users as a singleton is recommended
    explanation: Use a factory and short-lived contexts in interactive server components.
---
## Learning objectives

- Design components that load data in any render mode
- Use `IDbContextFactory` in Interactive Server
- Call APIs from WebAssembly with `HttpClient`

## The abstraction

```csharp
// Shared (e.g. Shop.Client or a Contracts library)
public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> ListAsync(string? category, CancellationToken ct = default);
}
```

```csharp
// Server project
public sealed class ServerProductService(IDbContextFactory<ShopDbContext> factory) : IProductService
{
    public async Task<IReadOnlyList<ProductDto>> ListAsync(string? category, CancellationToken ct = default)
    {
        await using var db = await factory.CreateDbContextAsync(ct);
        return await db.Products
            .AsNoTracking()
            .Where(p => category == null || p.Category == category)
            .Select(p => new ProductDto(p.Id, p.Name, p.Price))
            .ToListAsync(ct);
    }
}
```

```csharp
// Client project
public sealed class HttpProductService(HttpClient http) : IProductService
{
    public async Task<IReadOnlyList<ProductDto>> ListAsync(string? category, CancellationToken ct = default) =>
        await http.GetFromJsonAsync<List<ProductDto>>($"api/products?category={Uri.EscapeDataString(category ?? "")}", ct) ?? [];
}
```

## Registration in each host

```csharp
// Server Program.cs
builder.Services.AddDbContextFactory<ShopDbContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddScoped<IProductService, ServerProductService>();
app.MapGet("/api/products", (string? category, IProductService products, CancellationToken ct) =>
    products.ListAsync(category, ct));

// Client Program.cs
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, HttpProductService>();
```

## The component

```razor
@inject IProductService Products

@foreach (var p in products)
{
    <ProductCard Product="p" />
}

@code {
    [SupplyParameterFromQuery] public string? Category { get; set; }
    private IReadOnlyList<ProductDto> products = [];

    protected override async Task OnParametersSetAsync() =>
        products = await Products.ListAsync(Category);
}
```

This is dependency inversion in practice: the component is reusable in every render mode — and in
a Hybrid app — because it never knows where its data comes from.
