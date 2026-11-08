---
title: Type handlers
questions:
  - id: purpose
    prompt: What is a Dapper type handler for?
    options:
      - text: Handling exceptions thrown by the database
      - text: Choosing which database provider to use
      - id: convert
        text: Converting between a custom .NET type and a database value, in both directions
        correct: true
      - text: Generating SQL from LINQ
    explanation: Implement `SqlMapper.TypeHandler<T>` with `SetValue` (to the database) and `Parse` (from the database).
  - id: register
    prompt: How is a type handler registered?
    options:
      - text: With an attribute on every property
      - text: In the connection string
      - id: add
        text: Once, globally, with `SqlMapper.AddTypeHandler(new MyHandler())` at startup
        correct: true
    explanation: Handlers are global to the process, so register them during application startup.
  - id: uses
    prompt: Which are good uses for type handlers?
    type: multiple
    options:
      - text: Mapping strongly typed IDs such as `record struct OrderId(int Value)`
        correct: true
      - text: Storing a small value object as JSON in a text column
        correct: true
      - text: Mapping a custom `Money` or `EmailAddress` value object to a column
        correct: true
      - text: Replacing SQL joins
    explanation: Type handlers keep domain types in your C# model without manual conversion in every query.
---
## Learning objectives

- Write and register type handlers
- Map strongly typed IDs and JSON columns

## Strongly typed IDs

```csharp
public readonly record struct OrderId(int Value);

public sealed class OrderIdHandler : SqlMapper.TypeHandler<OrderId>
{
    public override void SetValue(IDbDataParameter parameter, OrderId value)
    {
        parameter.DbType = DbType.Int32;
        parameter.Value = value.Value;
    }

    public override OrderId Parse(object value) => new(Convert.ToInt32(value));
}

SqlMapper.AddTypeHandler(new OrderIdHandler());
```

```csharp
public sealed class OrderRow
{
    public OrderId Id { get; init; }
    public DateTime PlacedAt { get; init; }
}

var order = await db.QuerySingleAsync<OrderRow>(
    "SELECT Id, PlacedAt FROM Orders WHERE Id = @id", new { id = new OrderId(42) });
```

Now an `OrderId` cannot be confused with a `CustomerId` anywhere in the codebase.

## JSON columns

```csharp
public sealed record ShippingPreferences(bool LeaveAtDoor, string? Instructions);

public sealed class JsonHandler<T> : SqlMapper.TypeHandler<T?> where T : class
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public override void SetValue(IDbDataParameter parameter, T? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value is null ? DBNull.Value : JsonSerializer.Serialize(value, Options);
    }

    public override T? Parse(object value) =>
        value is string json ? JsonSerializer.Deserialize<T>(json, Options) : default;
}

SqlMapper.AddTypeHandler(new JsonHandler<ShippingPreferences>());
```

## Things to know

- Handlers are global; register each one once at startup
- Handlers are used for parameters and for single-column mapping into properties
- Keep `Parse` tolerant of the provider's actual CLR type (for example `long` vs `int`) by using
  `Convert`
