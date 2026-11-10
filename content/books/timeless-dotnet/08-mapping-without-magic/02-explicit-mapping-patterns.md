---
title: Explicit mapping patterns
questions:
  - id: where-live
    prompt: Where should a mapping from a domain entity to an API DTO usually live?
    options:
      - text: Inside the domain entity, so the domain knows about the API
      - text: In a global static `Mapper` class shared by all layers
      - text: In the database
      - id: outer-layer
        text: In the API (outer) layer, for example as an extension member on the entity or a static method on the DTO
        correct: true
    explanation: Dependencies point inward. The domain should not know the shape of HTTP responses; the API layer knows both.
  - id: projection-efficiency
    prompt: "Why is `db.Orders.Select(OrderMappings.ToSummary).ToListAsync(ct)` (with `ToSummary` an `Expression<Func<Order, OrderSummary>>`) usually more efficient than loading entities and mapping in memory?"
    options:
      - text: Expressions run on the GPU
      - id: columns
        text: EF Core translates the expression to SQL selecting only the needed columns, and no entities are tracked
        correct: true
      - text: It is not more efficient
      - text: It caches results forever
    explanation: Projection is the single biggest EF performance habit for read endpoints.
  - id: method-in-expression
    prompt: "What happens if you call a regular C# method like `order.ToDto()` inside an EF Core `Select` at the top level?"
    options:
      - text: EF Core translates the method body into SQL
      - text: It always throws at runtime
      - id: client-eval
        text: EF Core materializes the full entity (top-level client evaluation) and then calls the method in memory — it works but loses column pruning
        correct: true
      - text: It does not compile
    explanation: Use an expression for projection queries, and a method for in-memory mapping. Some teams keep both side by side.
  - id: inbound
    prompt: For inbound mapping (request DTO → domain), what extra responsibility does the mapping usually carry?
    options:
      - text: None, it is symmetric with outbound mapping
      - id: validation
        text: Validation and construction of value objects, so invalid input cannot become an invalid domain object
        correct: true
      - text: Serialization to XML
      - text: Logging every property
    explanation: Inbound mapping is a trust boundary. It is where `string` becomes `EmailAddress` and failures become `400 Bad Request` with problem details.
  - id: testing
    prompt: How should explicit mappings be tested?
    options:
      - text: They cannot be tested
      - text: With a reflection test that compares every property name
      - id: light
        text: Lightly — the compiler checks completeness; test only mappings with logic (formatting, calculations, defaults)
        correct: true
      - text: Only via end-to-end UI tests
    explanation: A plain one-to-one constructor call needs no test. A mapping that converts cents to a formatted currency string does.
  - id: mapperly
    prompt: What distinguishes a source-generated mapper like Mapperly from AutoMapper?
    type: multiple
    options:
      - id: build-time
        text: Mapping code is generated at build time as plain C# you can read and debug
        correct: true
      - id: diagnostics
        text: Unmapped or mismatched members are reported as compiler diagnostics
        correct: true
      - id: aot-safe
        text: No runtime reflection, so it is trimming- and AOT-friendly
        correct: true
      - text: It requires a runtime configuration call at startup
    explanation: It keeps the safety of explicit code while removing typing. It is still a dependency — choose it for volume, not by reflex.
---
## Learning objectives

- Write outbound, inbound and projection mappings by hand
- Place mappings so dependencies point the right way
- Decide when generation helps

## Outbound: entity to response

```csharp
public sealed record OrderResponse(Guid Id, string Customer, decimal Total, string Status, IReadOnlyList<OrderLineResponse> Lines);
public sealed record OrderLineResponse(string Sku, int Quantity, decimal LineTotal);

public static class OrderResponseMappings
{
    extension(Order order)
    {
        public OrderResponse ToResponse() => new(
            order.Id.Value,
            order.CustomerName,
            order.Total,
            order.Status.ToString(),
            [.. order.Lines.Select(l => new OrderLineResponse(l.Sku, l.Quantity, l.Quantity * l.UnitPrice))]);
    }
}
```

## Projection: query straight into the DTO

```csharp
public static class OrderProjections
{
    public static readonly Expression<Func<Order, OrderSummary>> ToSummary = o => new OrderSummary(
        o.Id,
        o.Customer.Name,
        o.Lines.Sum(l => l.Quantity * l.UnitPrice),
        o.PlacedAt);
}

var summaries = await db.Orders
    .Where(o => o.Status == OrderStatus.Open)
    .OrderByDescending(o => o.PlacedAt)
    .Select(OrderProjections.ToSummary)
    .ToListAsync(ct);
```

With Dapper, the "projection" is simply the SQL `select` list mapping into a record — no
mapping layer at all (Chapter 9).

## Inbound: request to domain, with validation

```csharp
public sealed record CreateCustomerRequest(string Name, string Email, string? Phone);

public static class CreateCustomerMapping
{
    extension(CreateCustomerRequest request)
    {
        public Result<NewCustomer> ToDomain()
        {
            var errors = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(request.Name)) errors["name"] = ["Name is required"];
            if (!EmailAddress.TryParse(request.Email, out var email)) errors["email"] = ["Email is invalid"];

            return errors.Count == 0
                ? Result<NewCustomer>.Ok(new NewCustomer(request.Name.Trim(), email!, request.Phone))
                : Result<NewCustomer>.Invalid(errors);
        }
    }
}

app.MapPost("/customers", async (CreateCustomerRequest request, CustomerService service, CancellationToken ct) =>
    request.ToDomain() switch
    {
        { IsValid: true, Value: var customer } => Results.Created($"/customers/{(await service.CreateAsync(customer, ct)).Value}", null),
        { Errors: var errors } => Results.ValidationProblem(errors),
    });
```

## Migrating away from AutoMapper incrementally

1. Stop adding new profiles. New code maps explicitly.
2. When you touch a feature, replace its profile with extension members.
3. Replace `ProjectTo<T>()` with `Select` expressions, verifying generated SQL in logs.
4. When no profiles remain, remove the package and its DI registration.

Each step is small and independently shippable — the opposite of a risky rewrite.

## Sources

- Microsoft Learn, "Efficient querying" (EF Core projection) — <https://learn.microsoft.com/en-us/ef/core/performance/efficient-querying>
- Microsoft Learn, "Client vs. server evaluation" — <https://learn.microsoft.com/en-us/ef/core/querying/client-eval>
- Riok.Mapperly — <https://mapperly.riok.app/>
