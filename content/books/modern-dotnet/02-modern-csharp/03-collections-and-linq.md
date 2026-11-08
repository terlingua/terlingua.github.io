---
title: Collections, collection expressions and LINQ
questions:
  - id: deferred
    prompt: "When does the filtering in `var adults = people.Where(p => p.Age >= 18);` actually run?"
    options:
      - text: Immediately, when the line executes
      - text: When the garbage collector runs
      - id: enumerated
        text: When the result is enumerated, for example by `foreach`, `ToList()` or `Count()`
        correct: true
      - text: Only in Release builds
    explanation: Most LINQ operators use deferred execution. Enumerating the same query twice runs it twice.
  - id: collection-expr
    prompt: "What does `int[] all = [..first, ..second, 99];` do?"
    options:
      - text: Creates a jagged array with three elements
      - text: It is invalid syntax
      - id: spread
        text: Creates an array with the elements of `first`, then `second`, then 99
        correct: true
    explanation: Collection expressions (C# 12) use `..` to spread existing sequences into the new collection.
  - id: new-linq
    prompt: Which LINQ operators were added in .NET 9 or .NET 10?
    type: multiple
    options:
      - text: "`CountBy`"
        correct: true
      - text: "`Index`"
        correct: true
      - text: "`LeftJoin`"
        correct: true
      - text: "`Select`"
    explanation: "`CountBy`, `AggregateBy` and `Index` arrived in .NET 9; `LeftJoin` and `RightJoin` in .NET 10. `Select` has existed since LINQ's introduction."
  - id: frozen
    prompt: When is `FrozenDictionary<TKey, TValue>` a good choice?
    options:
      - text: When the dictionary is modified constantly
      - text: When you need to preserve insertion order
      - id: read-heavy
        text: When a lookup table is built once and then read many times
        correct: true
    explanation: Frozen collections spend more time at creation to make lookups faster, ideal for read-only configuration data.
---
## Learning objectives

- Use collection expressions
- Understand deferred execution
- Use the newest LINQ operators and the right collection type

## Collection expressions

```csharp
int[] primes = [2, 3, 5, 7];
List<string> names = ["Ada", "Grace"];
ReadOnlySpan<byte> header = [0x89, 0x50, 0x4E, 0x47];
IReadOnlyList<int> empty = [];
int[] combined = [..primes, 11, 13];
```

The target type decides what is created, so `[]` replaces `new List<T>()`, `Array.Empty<T>()` and
friends.

## Deferred execution

```csharp
var query = orders.Where(o => o.Total > 100);   // nothing runs yet
orders.Add(new Order(Total: 500));
var count = query.Count();                       // includes the new order
var snapshot = query.ToList();                   // materialise once, reuse
```

Materialise with `ToList()`/`ToArray()` when you will enumerate more than once, or when the source
may change.

## Newer operators

```csharp
var wordsByLength = words.CountBy(w => w.Length);            // .NET 9
var totals = orders.AggregateBy(o => o.CustomerId, 0m, (sum, o) => sum + o.Total);
foreach (var (index, item) in items.Index()) { }             // .NET 9

var withOrders = customers.LeftJoin(                          // .NET 10
    orders,
    c => c.Id,
    o => o.CustomerId,
    (c, o) => new { c.Name, OrderId = o?.Id });
```

Before .NET 10, a left join needed `GroupJoin` plus `SelectMany` plus `DefaultIfEmpty`.

## Choosing collections

| Need | Type |
|------|------|
| Ordered, indexable, growable | `List<T>` |
| Fast key lookup | `Dictionary<TKey, TValue>` |
| Uniqueness | `HashSet<T>` |
| Read-only lookup built once | `FrozenDictionary`, `FrozenSet` |
| Thread-safe shared state | `ConcurrentDictionary<TKey, TValue>` |
| Expose without allowing mutation | `IReadOnlyList<T>`, `IReadOnlyDictionary<TKey, TValue>` |
| Priority-based processing | `PriorityQueue<TElement, TPriority>` |

```csharp
private static readonly FrozenDictionary<string, decimal> VatRates =
    new Dictionary<string, decimal> { ["DE"] = 0.19m, ["FR"] = 0.20m }.ToFrozenDictionary();
```
