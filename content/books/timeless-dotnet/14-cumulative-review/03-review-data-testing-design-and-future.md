---
title: "Review II: data, testing, design and the future"
questions:
  - id: r-automapper
    prompt: The strongest technical argument against convention-based runtime mapping in new code is…
    options:
      - text: It is always slower than hand-written code by 10×
      - text: It cannot map records
      - text: Microsoft banned it
      - id: compile-time
        text: Renames and new members are not caught at compile time, so mapping bugs reach production silently
        correct: true
    explanation: Licensing and AOT are real concerns too, but compile-time safety is the core of the case.
  - id: r-projection
    prompt: For a read endpoint with EF Core, the efficient explicit alternative to `ProjectTo<T>()` is…
    options:
      - id: select
        text: "`Select(...)` into the DTO (possibly via a reusable expression)"
        correct: true
      - text: "`ToList()` then map"
      - text: A stored procedure per DTO
      - text: "`AsNoTracking()` alone"
    explanation: EF Core translates it into a narrow SQL select.
  - id: r-dapper-ct
    prompt: To pass a `CancellationToken` to Dapper's `QueryAsync`, use…
    options:
      - text: "`DynamicParameters.Token`"
      - text: An anonymous object property
      - text: "`SqlMapper.Settings`"
      - id: cmddef
        text: "`CommandDefinition`"
        correct: true
    explanation: The simple overloads have no token parameter.
  - id: r-identifier
    prompt: A sort column chosen by the user should reach the SQL via…
    options:
      - text: A parameter `order by @col`
      - id: allowlist
        text: An allow-list mapping to constant column names
        correct: true
      - text: Escaping quotes in the input
      - text: A stored procedure with `exec(@sql)`
    explanation: Identifiers cannot be parameterized; allow-lists are safe.
  - id: r-dbstring
    prompt: Passing a .NET `string` to compare with a large indexed `varchar` column in SQL Server can cause…
    options:
      - text: A syntax error
      - id: scan
        text: An implicit conversion that turns an index seek into a scan; fix with `DbString { IsAnsi = true }`
        correct: true
      - text: Data truncation
      - text: Nothing; types never matter
    explanation: Match parameter types to column types.
  - id: r-precompiled
    prompt: "\"Procedures are precompiled\" is best classified as a…"
    options:
      - text: Timeless principle
      - id: factoid
        text: Factoid with a historical kernel of truth
        correct: true
      - text: Language feature
      - text: Security guarantee
    explanation: Parameterized statements have been plan-cached since SQL Server 7.0.
  - id: r-proc-yes
    prompt: Which situations justify a stored procedure?
    type: multiple
    options:
      - id: execute-only
        text: The application account must have EXECUTE-only access with no table permissions
        correct: true
      - id: shared
        text: Several applications in different languages share one database and need identical complex data logic
        correct: true
      - id: native
        text: Natively compiled In-Memory OLTP operations
        correct: true
      - text: A single-owner app's simple `GetCustomerById`, "for speed"
    explanation: Speed alone is rarely the reason; ownership, permissions and data locality are.
  - id: r-proc-injection
    prompt: A stored procedure that builds `@sql` by concatenating input and calls `exec(@sql)` is…
    options:
      - id: injectable
        text: Injectable; it should use `sp_executesql` with parameters
        correct: true
      - text: Safe, because it is a procedure
      - text: Safe if the procedure is encrypted
      - text: Faster and therefore preferred
    explanation: Separation of code and data is what prevents injection.
  - id: r-mtp
    prompt: "On the .NET 10 SDK, MTP mode for `dotnet test` is enabled by…"
    options:
      - id: global-json
        text: "`\"test\": { \"runner\": \"Microsoft.Testing.Platform\" }` in `global.json`"
        correct: true
      - text: Installing VSTest
      - text: Setting `<IsTestProject>false</IsTestProject>`
      - text: Using .NET Framework
    explanation: Options then pass directly, for example `dotnet test --filter-class ...`.
  - id: r-xunit-skip
    prompt: In xUnit v3, a test that should not run when Docker is unavailable should…
    options:
      - text: Return early
      - id: skip
        text: Call `Assert.SkipUnless(dockerAvailable, "Docker required")`
        correct: true
      - text: Catch all exceptions
      - text: Be deleted
    explanation: Returning early reports a misleading pass.
  - id: r-xunit-lifetime
    prompt: "`IAsyncLifetime.InitializeAsync` in xUnit v3 returns…"
    options:
      - text: "`Task`"
      - text: "`void`"
      - text: "`IAsyncEnumerable<object>`"
      - id: valuetask
        text: "`ValueTask`"
        correct: true
    explanation: Cleanup is `IAsyncDisposable.DisposeAsync`, also returning `ValueTask`.
  - id: r-mock-db
    prompt: The most trustworthy way to test Dapper queries is…
    options:
      - text: Mocking `IDbConnection`
      - text: EF Core's in-memory provider
      - text: Reading the SQL in code review only
      - id: real-engine
        text: Running them against the real database engine, for example in a Testcontainers container
        correct: true
    explanation: Only a real engine exercises the SQL, types and constraints.
  - id: r-parnas
    prompt: Information hiding asks each module to hide…
    options:
      - text: Its source code from other teams
      - text: All public methods
      - text: Its unit tests
      - id: likely-change
        text: A design decision likely to change
        correct: true
    explanation: Parnas, 1972.
  - id: r-problem
    prompt: The standard format for HTTP API error bodies is…
    options:
      - text: RFC 2616 error pages
      - text: SOAP faults
      - id: rfc9457
        text: RFC 9457 problem details
        correct: true
      - text: 'A custom `{ "error": true }` object'
    explanation: ASP.NET Core supports it via `AddProblemDetails()`.
  - id: r-union
    prompt: "In C# 15, `public union Shape(Circle, Square);` — a `switch` covering `Circle` and `Square`…"
    options:
      - id: exhaustive
        text: Is exhaustive and needs no discard arm (for non-nullable case types)
        correct: true
      - text: Always needs a discard arm
      - text: Cannot use type patterns
      - text: Throws at runtime for `Square`
    explanation: Adding a case later produces warnings at non-exhaustive switches.
  - id: r-net11
    prompt: .NET 11 is…
    options:
      - id: sts
        text: An STS release launched in November 2026, supported for 24 months
        correct: true
      - text: LTS, supported until 2031
      - text: A preview only
      - text: The last version of .NET
    explanation: .NET 12 is expected as the next LTS in November 2027.
  - id: r-vocab
    prompt: Match the term to the example — which pairing is correct?
    type: multiple
    options:
      - id: vestige-gc
        text: "Vestige — a `GC.Collect()` left from a long-fixed leak"
        correct: true
      - id: dogma-cfa
        text: "Dogma — \"always `ConfigureAwait(false)`\" enforced in ASP.NET Core app code with no stated reason"
        correct: true
      - id: anachronism-nolock
        text: "Anachronism — SQL Server 2000-era tuning folklore applied unchanged to a 2026 workload"
        correct: true
      - text: "Factoid — \"information hiding reduces the cost of change\""
    explanation: The last statement is a well-supported principle, not a factoid.
  - id: r-adr
    prompt: Why record the reason for an architectural decision?
    options:
      - text: To make it permanent
      - id: recognize-vestige
        text: So that future maintainers can check whether the reason still holds — and retire the decision when it does not
        correct: true
      - text: To satisfy the compiler
      - text: To replace tests
    explanation: Written reasons are how a team avoids creating tomorrow's dogma.
---
## Learning objectives

- Recall the core ideas of Chapters 8–13 and the book's vocabulary

## A closing thought

Every practice in this book will one day be someone's vestige. Records may give way to something
better; xUnit 4 will have a successor; even async/await is getting a new engine underneath in
.NET 11. The habits that do not age are the ones that ask *why*, measure, cite sources, write
reasons down, and let go of rules whose reasons have gone.

## Sources

- See the sources listed at the end of each chapter section.
