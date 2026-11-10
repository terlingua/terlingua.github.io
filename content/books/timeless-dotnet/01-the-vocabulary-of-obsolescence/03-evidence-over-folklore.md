---
title: Evidence over folklore
questions:
  - id: benchmark-tool
    prompt: Which tool is the standard way to write reliable micro-benchmarks for .NET code?
    options:
      - text: "`Stopwatch` around a loop in a `Main` method run once in Debug"
      - text: Visual Studio's Output window
      - text: "`DateTime.Now` before and after the call"
      - id: bdn
        text: BenchmarkDotNet
        correct: true
    explanation: BenchmarkDotNet handles warm-up, JIT tiering, outliers, statistical analysis and memory diagnostics. A `Stopwatch` loop in Debug measures the debugger as much as your code.
  - id: release-mode
    prompt: Why are measurements taken in a Debug build misleading?
    options:
      - text: Debug builds use a different garbage collector
      - text: Debug builds run on a different CLR version
      - id: no-opts
        text: The JIT disables most optimizations so code can be stepped through, which distorts relative costs
        correct: true
      - text: They are not misleading
    explanation: Inlining, loop optimizations and register allocation are largely disabled. Always measure Release builds, ideally with BenchmarkDotNet's harness.
  - id: dated-claims
    prompt: "A blog post from 2012 says \"`foreach` over `List<T>` is slower than `for`\". What should you do before applying it?"
    type: multiple
    options:
      - id: check-runtime
        text: Note the runtime it was measured on (.NET Framework 4.5 in 2012) and recognize the JIT has changed dramatically since
        correct: true
      - id: measure-now
        text: Measure on .NET 10 for your actual case if it matters
        correct: true
      - text: Apply it everywhere, since a measured claim is true forever
      - id: readability
        text: Prefer the readable version unless a profiler shows this loop is hot
        correct: true
    explanation: Performance claims have a half-life. Date them, re-measure the ones that matter, and default to clarity.
  - id: profiler-first
    prompt: An endpoint is slow. What is the best first step?
    options:
      - id: measure
        text: Measure where the time goes (tracing, a profiler, database query statistics) before changing anything
        correct: true
      - text: Rewrite the LINQ queries as `for` loops
      - text: Convert all classes to structs
      - text: Add `ConfigureAwait(false)` to every await
    explanation: Most real latency is I/O — a missing index, a chatty N+1 query, a slow downstream service. Micro-optimizations made blind usually change nothing measurable.
  - id: primary-sources
    prompt: Which sources are primary for questions about .NET behavior?
    type: multiple
    options:
      - id: learn
        text: Microsoft Learn documentation and the official release notes
        correct: true
      - id: source
        text: The runtime source code on GitHub (dotnet/runtime) and the language design notes (dotnet/csharplang)
        correct: true
      - text: An undated answer on a Q&A site with many upvotes
      - id: own-measurement
        text: Your own measurement of your own workload
        correct: true
    explanation: Upvotes measure popularity at the time of voting. The source, the docs and your own data are where to resolve disagreements.
  - id: chesterton
    prompt: What does *Chesterton's fence* advise when you find a strange line of code?
    options:
      - text: Delete it immediately; unknown code is dead code
      - text: Never touch it
      - id: find-reason
        text: Find out why it was put there before removing it
        correct: true
      - text: Wrap it in a try/catch
    explanation: The principle cuts both ways. It protects useful oddities from careless deletion, and it is also how you *prove* something is vestigial — you find the reason and show it no longer applies.
---
## Learning objectives

- Turn opinions about code into testable claims
- Measure correctly with BenchmarkDotNet
- Use primary sources to settle disputes

## Every claim has a date

A useful habit: when you hear a rule, silently append "...as of when, on what?"

- "Exceptions are slow" — as of when? Throwing is expensive relative to a return value, which is
  why exceptions are for exceptional cases; .NET 9 also made exception handling itself
  substantially faster. Neither fact justifies returning error codes everywhere.
- "LINQ is slow" — on what? LINQ has been heavily optimized across .NET 7–10; for typical
  business code its cost is noise next to a single database round trip.

## Measuring properly

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<LabelBenchmarks>();

[MemoryDiagnoser]
public class LabelBenchmarks
{
    private readonly string _first = "Ada";
    private readonly string _last = "Lovelace";

    [Benchmark(Baseline = true)]
    public string Interpolation() => $"{_first} {_last}";

    [Benchmark]
    public string Builder() => new StringBuilder().Append(_first).Append(' ').Append(_last).ToString();
}
```

```bash
dotnet run -c Release
```

Read the *Allocated* column as carefully as the *Mean* column; allocations drive GC cost under load.

## The order of evidence

1. **Production telemetry** (OpenTelemetry traces, metrics) — what is actually slow
2. **Profiles** (`dotnet-trace`, Visual Studio profiler, PerfView) — where the time goes
3. **Benchmarks** — whether a specific change helps
4. **Documentation and source** — why it behaves that way
5. **Folklore** — a hypothesis to test, never a conclusion

## A short case study

A team spent a sprint replacing LINQ with hand-written loops in an order export, citing "LINQ is
slow". Export time did not change. A trace later showed 97% of the time in 4,000 individual
`SELECT` statements — one per order line. A single set-based query fixed it in an afternoon. The
factoid cost a sprint; the measurement cost ten minutes.

## Sources

- BenchmarkDotNet — <https://benchmarkdotnet.org/>
- Microsoft Learn, ".NET diagnostic tools" — <https://learn.microsoft.com/en-us/dotnet/core/diagnostics/>
- Stephen Toub, "Performance Improvements in .NET 10" (and earlier annual posts) — <https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-10/>
- dotnet/runtime on GitHub — <https://github.com/dotnet/runtime>
