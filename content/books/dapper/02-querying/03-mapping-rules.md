---
title: How Dapper maps columns to objects
questions:
  - id: case
    prompt: A query returns a column named `PRODUCTNAME`. Will it map to a property named `ProductName`?
    options:
      - id: yes-case
        text: Yes — Dapper matches names case-insensitively
        correct: true
      - text: No — names must match exactly, including case
      - text: Only if the property has a `[Column]` attribute
    explanation: Matching ignores case. Dapper does not read EF-style `[Column]` attributes by default.
  - id: underscores
    prompt: Your PostgreSQL columns use snake_case (`first_name`). What is the simplest way to map them to `FirstName`?
    options:
      - text: Rename every C# property to snake_case
      - text: Write a type handler for every class
      - id: match-underscores
        text: Set `Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true` once at startup
        correct: true
      - text: Dapper cannot map these columns
    explanation: The global setting ignores underscores when matching names. Column aliases (`first_name AS FirstName`) also work.
  - id: unmatched
    prompt: Which statements about unmatched columns and properties are true?
    type: multiple
    options:
      - text: A column with no matching property is ignored
        correct: true
      - text: A property with no matching column keeps its default value
        correct: true
      - text: Aliasing in SQL (`SELECT p.Name AS ProductName`) is a simple way to fix name mismatches
        correct: true
      - text: Dapper throws when any column is not mapped
    explanation: Silent ignoring is convenient but can hide typos — integration tests that assert on mapped values catch them.
  - id: dynamic
    prompt: What does `connection.QueryAsync(sql)` (without a type argument) return?
    options:
      - id: dynamic-rows
        text: "`dynamic` rows whose columns can be accessed as properties or through `IDictionary<string, object>`"
        correct: true
      - text: Raw `DataTable` objects
      - text: JSON strings
    explanation: Dynamic results are handy for ad-hoc scripts and reporting but lose compile-time safety.
---
## Learning objectives

- Understand Dapper's name-matching rules
- Handle naming-convention differences
- Use dynamic results when appropriate

## Name matching

Dapper matches each result column to a settable property, field or constructor parameter with the
same name, ignoring case. Unmatched columns are ignored; unmatched properties keep their defaults.

```csharp
public sealed class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
}

var customers = await db.QueryAsync<Customer>(
    """
    SELECT c.Id,
           c.FirstName + ' ' + c.LastName AS FullName,
           c.Email
    FROM Customers c
    """);
```

The alias `AS FullName` maps a computed expression.

## Snake case

```csharp
// once, at startup
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var people = await db.QueryAsync<Person>("SELECT person_id, first_name, last_name FROM people");
// person_id → PersonId, first_name → FirstName
```

## Custom maps

For full control, register a `CustomPropertyTypeMap`:

```csharp
SqlMapper.SetTypeMap(typeof(LegacyCustomer), new CustomPropertyTypeMap(
    typeof(LegacyCustomer),
    (type, column) => type.GetProperties().FirstOrDefault(p =>
        string.Equals(p.Name, column.Replace("cust_", ""), StringComparison.OrdinalIgnoreCase))!));
```

Aliases in SQL are usually clearer than global maps — prefer them unless a whole schema follows a
convention.

## Dynamic results

```csharp
var rows = await db.QueryAsync("SELECT Category, COUNT(*) AS Total FROM Products GROUP BY Category");
foreach (var row in rows)
{
    Console.WriteLine($"{row.Category}: {row.Total}");
}
```

Each row is a `DapperRow` that implements `IDictionary<string, object>`. Great for exploration,
but prefer typed results in application code.

## Value tuples

```csharp
var pairs = await db.QueryAsync<(int Id, string Name)>("SELECT Id, Name FROM Products");
```

Tuples map **by position**, not by name — keep the column order aligned with the tuple.
