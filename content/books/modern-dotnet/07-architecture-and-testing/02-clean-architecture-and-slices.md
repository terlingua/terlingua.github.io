---
title: Clean architecture and vertical slices
questions:
  - id: dependency-rule
    prompt: What is the dependency rule in clean architecture?
    options:
      - id: inward
        text: Source-code dependencies point only inward, toward the domain and application core
        correct: true
      - text: Outer layers must never reference inner layers
      - text: Every layer must reference every other layer
      - text: The database layer is the centre of the design
    explanation: The domain knows nothing about the web, databases or frameworks. Infrastructure depends on the core, not the reverse.
  - id: vertical-slice
    prompt: What characterises a vertical slice architecture?
    options:
      - id: feature
        text: Code is organised by feature, with each request's handler, validation and data access kept together
        correct: true
      - text: Code is organised by technical layer — all controllers together, all repositories together
      - text: Every feature is deployed as a separate microservice
    explanation: Slices maximise cohesion per feature; the code that changes together lives together.
  - id: project-refs
    prompt: In a clean architecture solution, which project references are correct?
    type: multiple
    options:
      - text: Infrastructure references Application
        correct: true
      - text: Application references Domain
        correct: true
      - text: The Web project references Infrastructure (to compose it) and Application
        correct: true
      - text: Domain references Infrastructure to save entities
    explanation: The domain is at the centre and references nothing project-specific.
---
## Learning objectives

- Lay out a solution following the dependency rule
- Compare layered clean architecture with vertical slices
- Pick a structure that fits the size of the system

## Clean architecture layout

```text
src/
  Shop.Domain/          entities, value objects, domain events — no dependencies
  Shop.Application/     use cases, interfaces such as IOrderRepository — references Domain
  Shop.Infrastructure/  EF Core, Dapper, email, queues — references Application
  Shop.Web/             endpoints, composition root — references Application and Infrastructure
tests/
  Shop.Domain.Tests/
  Shop.Application.Tests/
  Shop.Web.IntegrationTests/
```

```csharp
// Shop.Infrastructure/DependencyInjection.cs
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ShopDbContext>(o => o.UseSqlServer(config.GetConnectionString("Shop")));
        services.AddScoped<IOrderRepository, EfOrderRepository>();
        return services;
    }
}
```

The compiler enforces the dependency rule: `Shop.Domain` cannot call EF Core because it has no
reference to it.

## Vertical slices

```text
Features/
  Orders/
    PlaceOrder.cs       request, validator, handler, endpoint mapping
    CancelOrder.cs
    GetOrder.cs
```

```csharp
public static class PlaceOrder
{
    public sealed record Request(int CustomerId, IReadOnlyList<LineItem> Lines);
    public sealed record Response(int OrderId);

    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/orders", HandleAsync);

    private static async Task<Created<Response>> HandleAsync(Request request, ShopDbContext db, CancellationToken ct)
    {
        var order = Order.Create(request.CustomerId, request.Lines);
        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);
        return TypedResults.Created($"/orders/{order.Id}", new Response(order.Id));
    }
}
```

## Choosing

| Situation | Good fit |
|-----------|----------|
| Rich domain rules, long-lived system, several delivery mechanisms | Clean architecture |
| Mostly CRUD and queries, features change independently | Vertical slices |
| Small service or prototype | A single project with feature folders |

Many teams combine them: a rich `Domain` project at the centre and vertical slices in the
application layer. Whatever you choose, keep the domain free of infrastructure concerns.
