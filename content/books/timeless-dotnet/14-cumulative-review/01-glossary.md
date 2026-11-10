---
title: Glossary
questions:
  - id: captive-term
    prompt: Which term names a shorter-lived service captured by a longer-lived one?
    options:
      - text: Service locator
      - text: Composition root
      - text: Keyed service
      - id: captive
        text: Captive dependency
        correct: true
    explanation: The classic case is a scoped `DbContext` injected into a singleton.
  - id: idempotency-term
    prompt: Which term describes an operation that can be safely repeated with the same effect as doing it once?
    options:
      - id: idempotent
        text: Idempotent
        correct: true
      - text: Atomic
      - text: Cooperative
      - text: Vestigial
    explanation: Idempotency (often via an idempotency key) is what makes retries and uncertain cancellations safe across a network.
  - id: outbox-term
    prompt: Which pattern writes outgoing messages to a table in the same transaction as the business data?
    options:
      - text: Saga
      - text: Decorator
      - id: outbox
        text: Transactional outbox
        correct: true
      - text: Repository
    explanation: A separate publisher sends the stored messages afterwards, at least once.
  - id: sniffing-term
    prompt: Which term names the reuse of a plan compiled for the first-seen parameter values?
    options:
      - text: Plan pollution
      - id: sniffing
        text: Parameter sniffing
        correct: true
      - text: Ownership chaining
      - text: Ad hoc caching
    explanation: It affects both procedures and parameterized statements.
---
## Learning objectives

- Review the key terms used throughout this book

## The four time words

| Term | Meaning | .NET example |
|---|---|---|
| **Anachronism** | Belongs to a different period than the one it exists in | 1998 SQL tuning folklore in a 2026 service |
| **Dogma** | A rule held as unquestionable after its reasons are gone | "Always `ConfigureAwait(false)`" in ASP.NET Core apps |
| **Factoid** | False or unreliable information repeated until accepted as fact | "Stored procedures are precompiled, so they are faster" |
| **Vestige** | A remnant that once served a purpose and no longer does | AutoMapper profiles for one-to-one record copies |

## Technical terms

| Term | Meaning |
|---|---|
| **Async state machine** | Compiler- (or, from .NET 11, runtime-) generated mechanism that suspends and resumes an `async` method at `await` points |
| **Buffered query** | Dapper reads all rows into a list before returning |
| **Captive dependency** | A shorter-lived service held by a longer-lived one |
| **Chesterton's fence** | Understand why something exists before removing it |
| **Closed hierarchy** | C# 15: a `closed` base class whose direct subclasses are limited to its assembly, enabling exhaustive switches |
| **Composition root** | The single place where the object graph is assembled |
| **Cooperative cancellation** | Code stops only where it observes a token |
| **Expand/contract** | Evolving a shared contract by adding the new form, migrating consumers, then removing the old |
| **Functional core, imperative shell** | Pure decisions inside, I/O at the edges |
| **Idempotency** | Repeating an operation has the same effect as doing it once |
| **Information hiding** | Modules hide decisions likely to change (Parnas, 1972) |
| **Keyed service** | A DI registration distinguished by a key |
| **MTP** | Microsoft.Testing.Platform, the executable-based test platform replacing VSTest |
| **Ownership chaining** | SQL Server lets a procedure access objects with the same owner without the caller having direct permission |
| **Parameter sniffing** | A cached plan compiled for the first parameter values is reused for others |
| **Plan cache pollution** | Many single-use plans caused by unparameterized or inconsistently typed SQL |
| **Point of no return** | The step after which honoring a caller's cancellation would leave inconsistent state |
| **Problem details** | RFC 9457 JSON format for HTTP API errors |
| **Service locator** | Pulling dependencies from a container inside business code (anti-pattern) |
| **Sync over async** | Blocking on a task with `.Result`/`.Wait()`/`GetResult()` |
| **Transactional outbox** | Storing outgoing messages in the same transaction as the data change |
| **Union type** | C# 15: a value that is exactly one of a fixed set of case types |
