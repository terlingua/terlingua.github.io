---
title: Security and performance
questions:
  - id: interpolation
    prompt: "Is `db.QueryAsync<User>($\"SELECT * FROM Users WHERE Name = '{name}'\")` safe if `name` comes from a query string?"
    options:
      - text: Yes, Dapper escapes interpolated strings automatically
      - id: unsafe
        text: No — the value is concatenated into the SQL text, which allows SQL injection
        correct: true
      - text: Yes, as long as the column is indexed
    explanation: Dapper only protects values passed as parameters. Plain string interpolation produces a raw SQL string.
  - id: least-privilege
    prompt: Which database permission setup follows the principle of least privilege for a typical web app?
    options:
      - id: least
        text: An application login that can only read and write the tables (or execute the procedures) it needs
        correct: true
      - text: The `sa` or database owner account, for simplicity
      - text: A shared admin account used by every service
    explanation: Limited permissions contain the damage of a compromised application or an injection bug.
  - id: perf
    prompt: Which practices improve the performance of Dapper-based data access?
    type: multiple
    options:
      - text: Select only the columns you need
        correct: true
      - text: Keep SQL text stable and parameterised so the database can reuse execution plans
        correct: true
      - text: Match parameter types to column types (for example `DbString` with `IsAnsi` for `VARCHAR`)
        correct: true
      - text: Open a new connection for every row in a loop
    explanation: Fewer bytes, reused plans and sargable predicates matter far more than micro-optimising C#.
  - id: aot
    prompt: What does Dapper.AOT add?
    options:
      - text: Automatic migrations
      - id: source-gen
        text: A source generator that produces the mapping code at build time, avoiding runtime reflection and supporting Native AOT
        correct: true
      - text: A LINQ provider
    explanation: Dapper.AOT uses C# interceptors to replace calls to Dapper's methods with generated code.
---
## Learning objectives

- Prevent SQL injection
- Apply least privilege
- Tune Dapper queries for performance

## SQL injection

```csharp
// Vulnerable
await db.QueryAsync<User>($"SELECT Id, Name FROM Users WHERE Name = '{name}'");

// Safe
await db.QueryAsync<User>("SELECT Id, Name FROM Users WHERE Name = @name", new { name });
```

An attacker sending `' OR 1=1 --` as the name dumps the table in the first version. Analyzers and
code review should flag any SQL built with interpolation of non-constant values.

## Least privilege

```sql
CREATE USER shop_app FOR LOGIN shop_app;
GRANT SELECT, INSERT, UPDATE ON SCHEMA::sales TO shop_app;
GRANT EXECUTE ON OBJECT::sales.PlaceOrder TO shop_app;
```

The application cannot drop tables or read other schemas, even if compromised.

## Performance checklist

- **Columns** — list them; avoid `SELECT *`
- **Parameters** — never inline values; stable SQL text means plan reuse
- **Types** — match `NVARCHAR` vs `VARCHAR`, `DECIMAL` precision, `DATETIME2`
- **Round trips** — use `QueryMultiple` or joins instead of N+1 loops
- **Paging** — use `OFFSET ... FETCH` or keyset pagination (`WHERE Id > @lastId`) for deep pages
- **Indexes** — check actual execution plans for scans on hot queries

## Keyset pagination

```csharp
const string sql = """
    SELECT TOP (@pageSize) Id, Name, Price
    FROM Products
    WHERE Id > @afterId
    ORDER BY Id
    """;
var page = await db.QueryAsync<ProductSummary>(sql, new { afterId, pageSize = 50 });
```

Unlike `OFFSET`, keyset paging stays fast however deep the user goes.

## Dapper.AOT

```xml
<PackageReference Include="Dapper.AOT" />
<PropertyGroup>
  <InterceptorsNamespaces>$(InterceptorsNamespaces);Dapper.AOT</InterceptorsNamespaces>
</PropertyGroup>
```

```csharp
[module: DapperAot]
```

Your Dapper calls stay the same; the generator emits strongly typed code for parameters and
mapping at build time, which suits Native AOT and trimmed apps.
