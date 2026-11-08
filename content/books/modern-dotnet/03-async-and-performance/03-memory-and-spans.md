---
title: Memory, spans and measuring performance
questions:
  - id: span
    prompt: What is `Span<T>`?
    options:
      - id: view
        text: A type-safe view over contiguous memory (arrays, stack memory, native memory) without copying
        correct: true
      - text: A thread-safe list
      - text: A compressed array
      - text: A type that can be stored in a class field
    explanation: "`Span<T>` is a `ref struct`, so it lives on the stack and cannot be a field of a class, but it can slice memory with no allocations."
  - id: benchmark
    prompt: What is the recommended way to compare the performance of two implementations?
    options:
      - text: Wrap each in a `Stopwatch` and run it once in Debug mode
      - text: Count the lines of code
      - id: bdn
        text: Use BenchmarkDotNet in a Release build
        correct: true
    explanation: BenchmarkDotNet handles warm-up, JIT tiering, statistics and memory diagnostics.
  - id: alloc
    prompt: Which techniques reduce allocations on hot paths?
    type: multiple
    options:
      - text: Renting buffers from `ArrayPool<T>.Shared`
        correct: true
      - text: Parsing slices with `ReadOnlySpan<char>` instead of `Substring`
        correct: true
      - text: Using `stackalloc` for small, fixed-size temporary buffers
        correct: true
      - text: Calling `GC.Collect()` after every request
    explanation: Pooling, slicing and stack allocation avoid garbage. Forcing collections usually makes performance worse.
---
## Learning objectives

- Use `Span<T>` and `Memory<T>` to avoid copying
- Pool buffers and use `stackalloc` safely
- Measure before optimising

## The cost of allocations

Every heap allocation eventually costs garbage-collection work. On hot paths — parsers,
serializers, middleware — allocation-free code can make a large difference. Everywhere else,
clarity wins.

## Spans

```csharp
public static class Csv
{
    public static int Sum(ReadOnlySpan<char> line)
    {
        var total = 0;
        foreach (var range in line.Split(','))
        {
            total += int.Parse(line[range]);
        }
        return total;
    }
}

Console.WriteLine(Csv.Sum("1,2,3,4"));
```

`MemoryExtensions.Split` (.NET 9) enumerates ranges without creating substrings, and
`int.Parse` accepts spans directly.

| Type | Lives | Use for |
|------|-------|---------|
| `Span<T>` / `ReadOnlySpan<T>` | Stack only (`ref struct`) | Synchronous slicing and parsing |
| `Memory<T>` / `ReadOnlyMemory<T>` | Heap-storable | Async APIs, fields |

## Pooling and stackalloc

```csharp
var buffer = ArrayPool<byte>.Shared.Rent(64 * 1024);
try
{
    int read;
    while ((read = await stream.ReadAsync(buffer.AsMemory(), ct)) > 0)
        hasher.AppendData(buffer.AsSpan(0, read));
}
finally
{
    ArrayPool<byte>.Shared.Return(buffer);
}

Span<byte> small = stackalloc byte[16];
Random.Shared.NextBytes(small);
```

Keep `stackalloc` small (a few hundred bytes); large stack buffers risk stack overflow.

## Measure

```csharp
[MemoryDiagnoser]
public class ParsingBenchmarks
{
    private const string Line = "10,20,30,40,50";

    [Benchmark(Baseline = true)]
    public int WithSplit() => Line.Split(',').Sum(int.Parse);

    [Benchmark]
    public int WithSpan() => Csv.Sum(Line);
}
```

```bash
dotnet run -c Release
```

Optimise only what a profiler or benchmark shows is slow, and keep the readable version when the
gain does not matter.
