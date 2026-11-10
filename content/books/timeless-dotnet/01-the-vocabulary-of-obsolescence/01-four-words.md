---
title: Anachronism, dogma, factoid and vestige
questions:
  - id: anachronism-def
    prompt: A team adds `WITH (NOLOCK)` to every query and splits every report into a cursor loop because "that is how we tuned SQL Server 2000". Which word best describes applying this technique to a 2026 application?
    options:
      - text: Factoid
      - text: Polymorphic
      - id: anachronistic
        text: Anachronistic
        correct: true
      - text: Idempotent
    explanation: An anachronism is something that belongs to a period other than the one in which it exists. The technique may have been reasonable for its era and hardware; transplanted into today's engine, optimizer and workload, it is out of time.
  - id: factoid-origin
    prompt: In its original sense, what is a *factoid*?
    options:
      - text: A small but true and interesting fact
      - text: A fact that has been peer reviewed
      - id: repeated-falsehood
        text: A piece of unreliable or false information repeated so often that it becomes accepted as fact
        correct: true
      - text: A fact that only applies to one version of a product
    explanation: Norman Mailer coined the word in 1973 for "facts which have no existence before appearing in a magazine or newspaper". The later "trivia" meaning is a drift; this book uses the original one.
  - id: dogma-def
    prompt: Which statement is the clearest example of *legacy dogma*?
    options:
      - text: "We measured both options with BenchmarkDotNet and kept the faster one."
      - text: "We use records for DTOs because value equality makes our tests simpler."
      - text: "We target .NET 10 because it is the current LTS release."
      - id: never-question
        text: "Every class must have an interface. That is the rule here; it has always been the rule."
        correct: true
    explanation: Dogma is a rule held as incontrovertible and repeated after the conditions that justified it have gone. "It has always been the rule" is the tell — no reason is offered, and none is allowed to be questioned.
  - id: vestige-def
    prompt: A codebase still calls `GC.Collect()` after every request "to keep memory low", a habit that began during a leak investigation years ago. The leak was fixed long ago. What is the `GC.Collect()` call?
    options:
      - text: An anachronism, because GC did not exist when it was written
      - text: A factoid, because it is a false statement
      - id: vestigial
        text: Vestigial — a surviving remnant of an earlier stage that no longer serves a purpose
        correct: true
      - text: A dogma, because it is written in code
    explanation: Like the appendix, the call survived the conditions that produced it. It now costs something (forced full collections hurt throughput) and buys nothing.
  - id: relationships
    prompt: Which statements about how these four ideas relate are accurate?
    type: multiple
    options:
      - id: factoid-feeds-dogma
        text: A factoid often supplies the "reason" that keeps a dogma alive
        correct: true
      - id: vestige-was-useful
        text: A vestigial practice usually *was* useful once; that is what distinguishes it from a plain mistake
        correct: true
      - text: An anachronism is always wrong in every context
      - id: time-dimension
        text: All four involve time — something that was once true, once believed or once needed is carried past its moment
        correct: true
    explanation: Anachronism is relative to context; a technique can be right in its own era. The four words are useful precisely because they share a time dimension, which tells you where to look for the evidence — at what changed.
  - id: not-everything-old
    prompt: Which of these is *not* an example of a vestige or anachronism?
    options:
      - text: Hungarian notation (`strName`, `iCount`) in C# with an IDE that shows types on hover
      - text: Wrapping `HttpClient` I/O in `Task.Run` in an ASP.NET Core controller
      - text: Using `ArrayList` in new code
      - id: parnas
        text: Hiding design decisions that are likely to change behind a module boundary (Parnas, 1972)
        correct: true
    explanation: Age alone does not make an idea obsolete. Information hiding is over fifty years old and as relevant as ever, because the problem it solves — change — has not gone away.
  - id: test-for-vestige
    prompt: What is the most reliable first question to ask when you suspect a practice is vestigial?
    options:
      - id: what-problem
        text: "What problem did this solve, and does that problem still exist here?"
        correct: true
      - text: "Who on the team wrote it?"
      - text: "Is it older than five years?"
      - text: "Does a blog post say it is bad?"
    explanation: Chesterton's fence applies — find out why it was built. If the problem is gone, the practice is a vestige. If the problem is still there, you have learned something.
---
## Learning objectives

- Define *anachronism*, *dogma*, *factoid* and *vestige* precisely
- Recognize each in real codebases and code reviews
- Use the vocabulary to have calmer, evidence-based design discussions

## Why a book about .NET starts with vocabulary

Software ages unevenly. The C# compiler, the runtime and the libraries change every year;
the advice people repeat about them changes far more slowly. A great deal of friction in
teams is not about code at all — it is about which era's rules everyone is playing by.

Having precise words for "this was once true" lets us separate the *idea* from the *person*
holding it. "That rule is vestigial" is a claim about history that can be checked. "That rule is
dumb" is an insult that cannot.

The real-world scenarios throughout this book are composites of common incidents, written to
teach; they do not describe specific organizations.

## The four words

**Anachronism** *(adj. anachronistic)* — something that belongs to a period other than that in
which it exists. Applying a 1998 database optimization to a 2026 .NET application is
*anachronistic*: the optimizer, the plan cache, the hardware and the workload are all different.

**Dogma (legacy dogma)** — a rule held as incontrovertible truth, repeated long after the
conditions that made it sensible have vanished. The sign of dogma is not that the rule is wrong;
it is that the rule cannot be questioned.

**Factoid** — in Norman Mailer's original 1973 sense, a piece of unreliable or false information
repeated so often that it becomes accepted as fact. "Stored procedures are faster because they are
precompiled" is a classic .NET-world factoid (Chapter 10 takes it apart).

**Vestige** *(adj. vestigial)* — borrowed from biology, a remnant of an earlier stage of evolution
that serves no current purpose. A vestigial belief or practice was usually useful once.

## How they show up in .NET code

| Symptom in a codebase | Likely category | What changed |
|---|---|---|
| `AutoMapper` profiles for one-to-one DTO copies | Vestige | Records, `with`, extension members, source generators, compile-time safety |
| "Always `ConfigureAwait(false)`" in ASP.NET Core app code | Dogma | ASP.NET Core has no `SynchronizationContext` |
| "Stored procedures are precompiled, so they are faster" | Factoid | Parameterized SQL plans have been cached and reused since SQL Server 7.0 |
| `Newtonsoft.Json` added to a new .NET 10 project by habit | Anachronism | `System.Text.Json` ships in the box, with source generation |
| One interface per class, every class | Dogma | Interfaces exist for substitution; most classes are never substituted |
| `Task.Run(() => httpClient.GetAsync(...))` in a web app | Vestige | Async I/O frees the thread already; `Task.Run` just borrows another |
| `lock (this)` / `lock (typeof(X))` | Anachronism | Private lock objects (and `System.Threading.Lock` since .NET 9) |
| Hand-rolled `IDateTimeProvider` interfaces | Vestige | `TimeProvider` in the BCL since .NET 8 |

None of these is malicious. Each was good advice somewhere, sometime, for someone.

## The anatomy of a factoid

Factoids survive because they are *nearly* true and easy to repeat. They tend to have:

1. **A kernel of historical truth.** SQL Server 6.5 really did treat procedures specially.
2. **A plausible mechanism.** "Precompiled" sounds like how compilers work.
3. **No date attached.** The claim is stated in the present tense forever.
4. **Social reinforcement.** Senior people repeat it, so juniors do too.

The antidote is to attach a date and a mechanism to every performance or design claim:
*when* was this true, *why* was it true, and has that mechanism changed?

## A worked example: the "every class gets an interface" rule

```csharp
public interface IInvoiceTotalCalculator
{
    decimal Calculate(Invoice invoice);
}

public sealed class InvoiceTotalCalculator : IInvoiceTotalCalculator
{
    public decimal Calculate(Invoice invoice) => invoice.Lines.Sum(l => l.Quantity * l.UnitPrice);
}
```

Ask the historical question: *what problem did the interface solve?* In 2008 the answer was
often "Rhino Mocks and Moq can only fake interfaces and virtual members, and we want to mock
this". But a pure calculation needs no mocking — you test it directly with real invoices. The
interface is vestigial for this class. It costs a file, an indirection, and a registration.

```csharp
public static class InvoiceMath
{
    public static decimal Total(this Invoice invoice) => invoice.Lines.Sum(l => l.Quantity * l.UnitPrice);
}
```

Contrast a payment gateway client: it talks to the network, is slow, costs money, and has
several real implementations (sandbox, production, fake). There the interface earns its keep.
The rule is not "never use interfaces" — that would be a new dogma. The rule is "know the reason".

## Language for code review

- "What problem does this solve today?"
- "When was this measured, and on what runtime?"
- "Is this still true on .NET 10?"
- "What would break if we deleted it?"

These questions are hard to take personally and easy to answer with evidence.

## Sources

- Merriam-Webster, "factoid" — <https://www.merriam-webster.com/dictionary/factoid>
- D. L. Parnas, "On the Criteria To Be Used in Decomposing Systems into Modules", *CACM* 1972 — <https://dl.acm.org/doi/10.1145/361598.361623>
- Microsoft Learn, `TimeProvider` — <https://learn.microsoft.com/en-us/dotnet/api/system.timeprovider>
- Stephen Toub, "ConfigureAwait FAQ" — <https://devblogs.microsoft.com/dotnet/configureawait-faq/>
