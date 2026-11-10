---
title: Principles that do not age
questions:
  - id: parnas
    prompt: According to Parnas (1972), on what basis should a system be divided into modules?
    options:
      - id: hide-decisions
        text: Each module hides a design decision that is likely to change, behind an interface that does not reveal it
        correct: true
      - text: One module per step of the processing flowchart
      - text: One module per developer
      - text: One module per database table
    explanation: Information hiding is the root of encapsulation, interfaces, bounded contexts and microservice boundaries. It is fifty years old and not vestigial at all.
  - id: coupling-cohesion
    prompt: Which pair describes a well-designed module?
    options:
      - id: high-low
        text: High cohesion (its parts belong together) and low coupling (it depends little on the internals of others)
        correct: true
      - text: Low cohesion and high coupling
      - text: High cohesion and high coupling
      - text: As many dependencies as possible, for reuse
    explanation: Change tends to ripple along coupling. Cohesion keeps related change in one place.
  - id: explicit-magic
    prompt: Why does "explicit over magic" tend to age well in .NET?
    type: multiple
    options:
      - id: tooling
        text: The compiler, IDE navigation and analyzers can see explicit code; they cannot see conventions resolved at runtime
        correct: true
      - id: aot
        text: Explicit code is compatible with trimming and Native AOT; reflection-based magic often is not
        correct: true
      - id: onboarding
        text: New developers can read the code instead of learning a framework's hidden rules
        correct: true
      - text: Explicit code is always shorter
    explanation: Source generators are the modern compromise — the "magic" is generated as readable code at build time.
  - id: functional-core
    prompt: What is "functional core, imperative shell"?
    options:
      - id: pure-core
        text: Keep decisions in pure functions (no I/O) and push I/O to a thin outer layer that gathers inputs, calls the core, and acts on its output
        correct: true
      - text: Write everything in F#
      - text: Use only static classes
      - text: Avoid classes entirely
    explanation: The core becomes trivially testable and reusable; the shell is thin enough that a few integration tests cover it.
  - id: yagni
    prompt: A developer adds a plug-in system, a generic repository and three abstraction layers "in case we need them". Which principle does this violate?
    options:
      - text: DRY
      - id: yagni
        text: YAGNI — "you aren't gonna need it"; speculative generality has a real cost today and an uncertain benefit later
        correct: true
      - text: Liskov substitution
      - text: Nothing; abstractions are free
    explanation: Code you do not write cannot become vestigial. Add abstractions when the second real case arrives.
  - id: dry-misuse
    prompt: Two services have nearly identical `Address` validation today but belong to different business areas with different owners. Should you merge them into one shared library immediately?
    options:
      - text: Yes, duplication is always wrong
      - id: wrong-abstraction
        text: Not necessarily — if they are likely to diverge, coupling them creates the "wrong abstraction"; duplication is cheaper than the wrong abstraction
        correct: true
      - text: Yes, and make it a NuGet package with a major version per change
      - text: Delete one of them
    explanation: DRY is about knowledge, not text. Two pieces of code that look alike but change for different reasons are different knowledge.
  - id: boundaries
    prompt: Where do interfaces deliver the most value in a typical .NET application?
    options:
      - id: io-boundaries
        text: At boundaries with things that are slow, external, non-deterministic or likely to be replaced — payment gateways, message brokers, clocks, file stores
        correct: true
      - text: On every class, uniformly
      - text: On DTOs and records
      - text: On static helper classes
    explanation: This is information hiding again — the boundary hides a volatile decision (which vendor, which protocol).
---
## Learning objectives

- Apply information hiding, cohesion and coupling to modern C#
- Prefer explicit code and pure decision logic
- Resist both speculative generality and premature deduplication

## The test of time

Ask of any principle: was it true before .NET existed, and will it be true after? Information
hiding (1972), cohesion and coupling (Constantine and Yourdon, 1970s), and separating decisions
from effects pass this test. Specific patterns, frameworks and folder layouts do not — they are
*implementations* of principles, valid in a context.

## Information hiding in C#

```csharp
public interface IShippingRates
{
    Task<Money> QuoteAsync(Parcel parcel, Address destination, CancellationToken ct);
}
```

Behind this interface hide: which carrier, which API version, whether results are cached,
currency conversion, retries. Each is a decision likely to change. Callers are untouched when it does.

Use `internal` generously; `public` is a promise.

```xml
<ItemGroup>
  <InternalsVisibleTo Include="Shipping.Tests" />
</ItemGroup>
```

## Functional core, imperative shell

```csharp
public static class LateFeePolicy
{
    public static LateFeeDecision Decide(Invoice invoice, DateOnly today) => invoice switch
    {
        { IsPaid: true } => new LateFeeDecision.None(),
        _ when today <= invoice.DueDate => new LateFeeDecision.None(),
        _ when today <= invoice.DueDate.AddDays(30) => new LateFeeDecision.Charge(invoice.Total * 0.015m),
        _ => new LateFeeDecision.SendToCollections(invoice.Id),
    };
}

public sealed class LateFeeJob(IInvoiceStore invoices, ICollections collections, TimeProvider time)
{
    public async Task RunAsync(CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(time.GetUtcNow().UtcDateTime);
        await foreach (var invoice in invoices.OverdueAsync(today, ct))
        {
            switch (LateFeePolicy.Decide(invoice, today))
            {
                case LateFeeDecision.Charge charge:
                    await invoices.AddFeeAsync(invoice.Id, charge.Amount, ct);
                    break;
                case LateFeeDecision.SendToCollections send:
                    await collections.SubmitAsync(send.InvoiceId, ct);
                    break;
            }
        }
    }
}
```

`LateFeePolicy` is tested with dozens of fast, pure tests. `LateFeeJob` needs only a couple.

## Speculative generality versus the wrong abstraction

Two opposite mistakes, both common:

- **Speculative generality** — building for imagined futures. Cure: YAGNI; wait for the second case.
- **Premature unification** — merging code that looks alike but changes for different reasons.
  Cure: Sandi Metz's rule, "duplication is far cheaper than the wrong abstraction".

The principle underneath both: abstractions should follow *observed* variation.

## Sources

- D. L. Parnas, "On the Criteria To Be Used in Decomposing Systems into Modules" (1972) — <https://dl.acm.org/doi/10.1145/361598.361623>
- Gary Bernhardt, "Functional Core, Imperative Shell" — <https://www.destroyallsoftware.com/screencasts/catalog/functional-core-imperative-shell>
- Sandi Metz, "The Wrong Abstraction" — <https://sandimetz.com/blog/2016/1/20/the-wrong-abstraction>
- Martin Fowler, "Yagni" — <https://martinfowler.com/bliki/Yagni.html>
