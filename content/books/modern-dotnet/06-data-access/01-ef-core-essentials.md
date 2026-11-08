---
title: EF Core essentials
questions:
  - id: dbcontext-lifetime
    prompt: What is the recommended lifetime for a `DbContext` in an ASP.NET Core app?
    options:
      - text: Singleton, shared by all requests
      - text: Transient, created for every method call
      - text: Static field on the controller
      - id: scoped
        text: Scoped — one instance per request (the default of `AddDbContext`)
        correct: true
    explanation: A `DbContext` is a lightweight unit of work and is not thread-safe. Use one per request, or `IDbContextFactory<T>` for other scopes.
  - id: no-tracking
    prompt: When should you use `AsNoTracking()`?
    options:
      - id: read-only
        text: For read-only queries whose results you will not modify and save
        correct: true
      - text: Whenever you plan to call `SaveChanges` on the results
      - text: Only when using SQLite
    explanation: Skipping change tracking reduces memory and CPU for read-only data.
  - id: n-plus-one
    prompt: Which of these help avoid the N+1 query problem?
    type: multiple
    options:
      - text: Projecting with `Select` into a DTO that includes the related data you need
        correct: true
      - text: Using `Include` to load related entities in the same query
        correct: true
      - text: Lazy loading related collections inside a `foreach` over the parent rows
      - text: Logging SQL during development to spot repeated queries
        correct: true
    explanation: Lazy loading inside loops is the classic cause of N+1. Projection and `Include` fetch related data up front; SQL logging reveals the problem.
  - id: migrations
    prompt: Which command creates a new migration from model changes?
    options:
      - text: "`dotnet ef database drop`"
      - id: add
        text: "`dotnet ef migrations add AddOrderStatus`"
        correct: true
      - text: "`dotnet ef dbcontext scaffold`"
    explanation: "`migrations add` generates a migration class; `database update` or a migration bundle applies it."
---
Entity Framework Core is .NET's full ORM. (For hand-written SQL with minimal overhead, see the
Dapper book on this site — many systems use both.)

## Learning objectives

- Configure a `DbContext` and map entities
- Write efficient queries with projection and no-tracking
- Manage schema changes with migrations

## A model and a context

```csharp
public sealed class Order
{
    public int Id { get; private set; }
    public required string CustomerEmail { get; init; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public List<OrderLine> Lines { get; } = [];
}

public sealed class ShopDbContext(DbContextOptions<ShopDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Order>(o =>
        {
            o.Property(x => x.CustomerEmail).HasMaxLength(256);
            o.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            o.HasMany(x => x.Lines).WithOne().OnDelete(DeleteBehavior.Cascade);
        });
    }
}

builder.Services.AddDbContext<ShopDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Shop")));
```

## Query efficiently

```csharp
var summaries = await db.Orders
    .AsNoTracking()
    .Where(o => o.Status == OrderStatus.Paid)
    .OrderByDescending(o => o.Id)
    .Select(o => new OrderSummary(o.Id, o.CustomerEmail, o.Lines.Sum(l => l.Quantity * l.UnitPrice)))
    .Take(50)
    .ToListAsync(ct);
```

Projection fetches only needed columns and computes the sum in SQL — one round trip, no N+1.

## Change tracking

```csharp
var order = await db.Orders.Include(o => o.Lines).SingleAsync(o => o.Id == id, ct);
order.MarkPaid();
await db.SaveChangesAsync(ct);   // EF detects the change and issues one UPDATE
```

## Bulk operations

```csharp
await db.Orders
    .Where(o => o.Status == OrderStatus.Pending && o.CreatedAt < cutoff)
    .ExecuteDeleteAsync(ct);
```

`ExecuteUpdateAsync` and `ExecuteDeleteAsync` run a single SQL statement without loading entities.

## Migrations

```bash
dotnet ef migrations add AddOrderStatus
dotnet ef migrations script --idempotent -o migrate.sql
dotnet ef migrations bundle -o efbundle
```

In production, prefer reviewed SQL scripts or migration bundles in the deployment pipeline over
calling `Database.Migrate()` at application startup.
