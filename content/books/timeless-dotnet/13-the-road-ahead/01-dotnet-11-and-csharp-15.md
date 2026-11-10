---
title: .NET 11 and C# 15
questions:
  - id: dotnet11-type
    prompt: What kind of release is .NET 11, launching at .NET Conf 2026 (10–13 November 2026)?
    options:
      - id: sts
        text: Standard Term Support (STS), supported for 24 months
        correct: true
      - text: Long Term Support (LTS), supported for 5 years
      - text: A preview that will never be supported
      - text: A Windows-only release
    explanation: Odd-numbered releases are STS. Teams on the LTS train stay on .NET 10 and plan for .NET 12; teams that upgrade yearly can adopt 11.
  - id: union-syntax
    prompt: How is a union type declared in C# 15?
    options:
      - text: "`public enum Pet { Cat, Dog, Bird }`"
      - text: "`public Pet = Cat | Dog | Bird;`"
      - id: union-keyword
        text: "`public union Pet(Cat, Dog, Bird);`"
        correct: true
      - text: "`[Union] public interface Pet;`"
    explanation: A union value is exactly one of its case types. Each case type converts implicitly to the union, and a `switch` over the case types is checked for exhaustiveness.
  - id: union-exhaustive
    prompt: "Given `union Pet(Cat, Dog, Bird)` with non-nullable case types, does `pet switch { Cat c => ..., Dog d => ..., Bird b => ... }` need a discard arm?"
    options:
      - text: Yes, always, just like enums
      - text: Only in Release builds
      - text: Unions cannot be used in switch expressions
      - id: no-discard
        text: No — the compiler knows the case types are exhaustive
        correct: true
    explanation: This is the key difference from today's abstract-record idiom, which needs `_ => throw new UnreachableException()`. Adding a case later produces warnings at every non-exhaustive switch — the compiler finds the code you must update.
  - id: union-boxing
    prompt: What is a performance consideration with the compiler-generated union types described in the preview material?
    options:
      - text: Unions always allocate two objects
      - text: Unions cannot contain records
      - id: boxing
        text: The generated union stores its contents in an `object?` `Value`, so value-type cases are boxed; custom union types can avoid this by implementing `HasValue`/`TryGetValue` patterns
        correct: true
      - text: Unions are slower than exceptions
    explanation: For most application code the cost is irrelevant; for hot paths with struct cases, measure and consider a custom union type.
  - id: closed
    prompt: What does the C# 15 `closed` modifier on a class do?
    options:
      - text: Prevents all inheritance, like `sealed`
      - id: closed-hierarchy
        text: Restricts direct derivation to the declaring assembly and makes the class implicitly abstract, so switches over its derived types can be exhaustive
        correct: true
      - text: Makes all members private
      - text: Closes database connections automatically
    explanation: "`public closed record GateState;` with `Open` and `Closed` derived records is the language-supported version of the private-constructor idiom from Chapter 3."
  - id: with-args
    prompt: "What does `List<string> names = [with(capacity: values.Length * 2), .. values];` do?"
    options:
      - id: ctor-args
        text: Passes `capacity` to the `List<string>` constructor, then adds the spread elements — collection expression arguments
        correct: true
      - text: Creates a list of two lists
      - text: Clones `values` with a modified length
      - text: It is invalid syntax
    explanation: "`with(...)` must be the first element. It also enables comparers: `HashSet<string> set = [with(StringComparer.OrdinalIgnoreCase), \"a\", \"A\"];`."
  - id: other-cs15
    prompt: Which are also C# 15 features?
    type: multiple
    options:
      - id: ext-indexers
        text: Extension indexers in extension blocks
        correct: true
      - id: labeled
        text: Labeled `break` and `continue` for nested loops
        correct: true
      - id: memory-safety
        text: Updated memory-safety rules for `unsafe` (as a preview feature)
        correct: true
      - text: Removal of the `goto` statement
    explanation: Labeled `break`/`continue` replace boolean flags and `goto` for escaping nested loops; an IDE analyzer (IDE0410) suggests the change.
  - id: runtime-libs
    prompt: Which are .NET 11 runtime or library additions?
    type: multiple
    options:
      - id: runtime-async
        text: Runtime async — async methods implemented by the runtime, with lower overhead and better stack traces
        correct: true
      - id: zstd
        text: Zstandard compression in `System.IO.Compression`, and zstd in ASP.NET Core response compression
        correct: true
      - id: linq-join
        text: New LINQ operators such as `FullJoin`, and tuple-returning `Join`/`GroupJoin` overloads
        correct: true
      - id: async-validation
        text: Asynchronous validation (`AsyncValidationAttribute`, `IAsyncValidatableObject`) and async options validation
        correct: true
    explanation: Also notable — `System.Text.Json` union support, IEEE decimal types (`Decimal32/64/128`), Process API improvements, and `dotnet test --timeout` and `--maximum-failed-tests`.
---
## Learning objectives

- Know what ships in .NET 11 and C# 15
- Understand unions and closed hierarchies and how they change domain modeling
- Plan adoption sensibly on LTS and STS trains

## Status as of November 2026

.NET 11 launches at .NET Conf 2026 (10–13 November) after release candidates with a go-live
licence. It is an STS release. C# 15 is the default language version for `net11.0`. Some
features — notably the updated memory-safety rules — remain previews behind `LangVersion preview`.
Union types appeared as a preview in early 2026 builds (Preview 2 required a polyfill for
`UnionAttribute`/`IUnion`); the libraries now ship those types. Verify the exact status in your
SDK's release notes before relying on a feature in production.

## Unions: modeling alternatives with compiler help

```csharp
public sealed record Approved(string AuthorizationCode);
public sealed record Declined(string Reason);
public sealed record RequiresAction(Uri RedirectTo);

public union PaymentResult(Approved, Declined, RequiresAction);

public static IResult ToHttp(PaymentResult result) => result switch
{
    Approved a => Results.Ok(new { a.AuthorizationCode }),
    Declined d => Results.UnprocessableEntity(new { d.Reason }),
    RequiresAction r => Results.Accepted(r.RedirectTo.ToString()),
};

PaymentResult outcome = new Declined("Insufficient funds");
```

Compare with Chapter 3's nested-record idiom: no private constructor trick, no `UnreachableException`
arm, and when a fourth case is added, every incomplete `switch` warns.

Unions can declare members:

```csharp
public union OneOrMore<T>(T, IEnumerable<T>)
{
    public IEnumerable<T> AsEnumerable() => this switch
    {
        T single => [single],
        IEnumerable<T> many => many,
    };
}
```

Libraries such as OneOf filled this gap for years; for new code on .NET 11+, the language feature
is the timeless choice.

## Closed hierarchies

```csharp
public closed record GateState;
public sealed record Closed : GateState;
public sealed record Open(float Percent) : GateState;

string Describe(GateState state) => state switch
{
    Closed => "closed",
    Open(var percent) => $"{percent}% open",
};
```

Choose `closed` when the cases share a base type with common members; choose `union` when the
cases are unrelated types.

## Smaller conveniences

```csharp
HashSet<string> tags = [with(StringComparer.OrdinalIgnoreCase), "dotnet", "DotNet", "csharp"];

public static class SequenceIndexer
{
    extension<T>(IEnumerable<T> sequence)
    {
        public T this[int index] => sequence.ElementAt(index);
    }
}

outer:
for (var row = 0; row < grid.Height; row++)
{
    for (var column = 0; column < grid.Width; column++)
    {
        if (grid[row, column].IsBlocked) continue outer;
        if (grid[row, column].IsGoal) break outer;
    }
}
```

## Adoption advice

- **LTS shops:** stay on .NET 10; prototype unions on 11 in a branch so .NET 12 adoption is quick.
- **Yearly upgraders:** move to 11; take runtime async and library improvements for free.
- **Libraries:** keep targeting `net8.0`/`net10.0`; add `net11.0` only when you need its APIs.

## Sources

- Microsoft Learn, "What's new in C# 15" — <https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-15>
- .NET Blog, "Explore union types in C# 15" — <https://devblogs.microsoft.com/dotnet/csharp-15-union-types/>
- Microsoft Learn, "What's new in .NET 11" — <https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-11/overview>
- .NET Blog, "Get Ready for .NET Conf 2026" — <https://devblogs.microsoft.com/dotnet/get-ready-for-dotnet-conf-2026/>
- .NET support policy — <https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core>
