---
title: When stored procedures genuinely win
questions:
  - id: set-based
    prompt: A month-end job loads 2 million ledger rows into the app, computes balances in C#, and writes 50,000 summary rows back. Why is this a strong candidate for a stored procedure (or a single set-based SQL batch)?
    options:
      - text: Because C# cannot add decimals correctly
      - text: Because procedures are precompiled
      - id: data-locality
        text: Moving 2 million rows across the network to compute a small result is far slower than computing where the data lives with set-based SQL
        correct: true
      - text: Because the app might be written in a different language next year
    explanation: Data locality is the strongest performance argument. Note that a parameterized multi-statement batch sent from .NET gets the same locality; the procedure adds a named, permissioned, server-side unit.
  - id: security-boundary
    prompt: In which scenario is a procedure the right *security* tool?
    options:
      - text: Any public website, because procedures prevent injection
      - text: When you want to hide SQL from code reviewers
      - text: When the app uses an ORM
      - id: execute-only
        text: A regulated database where the application login must not read or modify tables directly, only call vetted operations granted with `EXECUTE` (ownership chaining provides table access inside the procedure)
        correct: true
    explanation: This is a real least-privilege boundary — even a compromised application cannot run `select * from dbo.Patients`. It is a deliberate architecture decision, not a default.
  - id: shared-db
    prompt: Five applications in three languages (C#, Python, a reporting tool) must perform the same complex "allocate stock" operation against one database. Why might a procedure be appropriate?
    options:
      - text: Because Python cannot send SQL
      - id: contract
        text: It provides one implementation and a stable, versioned contract at the integration point, instead of five copies drifting apart
        correct: true
      - text: Because procedures run faster in Python
      - text: It is never appropriate
    explanation: The better long-term design is often a service owning the data, but when the shared database *is* the integration point, a procedure is the shared implementation.
  - id: native
    prompt: When are stored procedures uniquely capable, with no inline-SQL equivalent?
    options:
      - text: Any `SELECT` with a `JOIN`
      - id: natively-compiled
        text: Natively compiled procedures over memory-optimized (In-Memory OLTP) tables, which are compiled to machine code for very high-throughput OLTP
        correct: true
      - text: Queries with parameters
      - text: Queries that return JSON
    explanation: This is the one place where "compiled" is literally true — and a niche one.
  - id: not-procs
    prompt: Which logic generally should *not* live in stored procedures?
    type: multiple
    options:
      - id: business-rules
        text: Branch-heavy business rules (pricing, eligibility, workflows) that change often and need fast unit tests
        correct: true
      - id: crud
        text: Simple CRUD for a single application that owns its database
        correct: true
      - id: string-heavy
        text: String formatting, HTTP calls, email composition and other non-relational work
        correct: true
      - text: Set-based maintenance over millions of rows
    explanation: T-SQL is excellent at sets and poor at everything else. Splitting business logic between C# and procedures also splits the knowledge, the tests and the deployments.
  - id: queue
    prompt: A table is used as a work queue. Many workers must each claim a different row without blocking each other. Which T-SQL pattern is typical — and must it be in a procedure?
    options:
      - text: "`select * from Queue` then delete in C#"
      - text: It is impossible without a procedure
      - text: "`nolock` on every query"
      - id: readpast
        text: "`update top (1) ... with (rowlock, readpast, updlock) output inserted.*` — it can be sent as parameterized inline SQL or wrapped in a procedure; the hints matter, not the packaging"
        correct: true
    explanation: An atomic single-statement claim is the key. Whether to name it as a procedure is a secondary choice about ownership and permissions.
  - id: ops
    prompt: A DBA team insists on procedures so they can change a query plan without an application release. What is a precise response?
    options:
      - text: They are wrong; this is never valid
      - id: tradeoff
        text: It is a real operational benefit, but modern alternatives exist (Query Store plan forcing and hints apply to inline queries too), and changing logic outside the application's tests and release process carries risk
        correct: true
      - text: Procedures cannot be changed without restarting SQL Server
      - text: Inline queries cannot be tuned
    explanation: Query Store (SQL Server 2016+) lets DBAs force plans and, since 2022, add hints to any query, including parameterized inline SQL.
---
## Learning objectives

- Name the specific situations in which stored procedures are the better choice
- Name the situations in which they are worse
- Express the trade-offs without dogma in either direction

## The precise list: use a stored procedure when…

1. **Set-based work over large data that should not leave the server.** Month-end closes,
   purges, re-aggregations, bulk upserts from a TVP with multiple dependent statements. The win
   is data locality and fewer round trips. (A parameterized batch from .NET can also achieve
   this; the procedure adds a name, a permission and a plan you can inspect.)
2. **A security boundary is required.** The application identity is granted `EXECUTE` only, with
   no table permissions, and ownership chaining lets the procedure touch the tables. Typical in
   finance, health and government systems with strict auditors.
3. **The database is a shared integration point** for several applications or languages that must
   run identical, non-trivial data logic.
4. **In-Memory OLTP natively compiled procedures** are needed for extreme throughput.
5. **An organizational contract** genuinely exists: a DBA team owns, tests, versions and deploys the
   data API, and application teams consume it. This is valid when the ownership is real, staffed
   and in source control — and vestigial when it is a rule nobody resources.

## Prefer parameterized inline SQL (or an ORM) when…

1. **One application owns its database** — the normal case for modern services.
2. **The logic is business logic** — rules, branching, calculations that change with the business.
3. **You need fast, isolated tests.** C# logic tests in milliseconds; procedure tests need a database.
4. **Deployment coupling hurts.** Every procedure change is a schema change that must be released in
   step with the application (expand/contract versioning).
5. **Queries are composed dynamically** — search screens with optional filters, sorting, paging.
   Building that in T-SQL means dynamic SQL inside procedures, with all of its injection and
   maintenance risk.
6. **Portability or developer familiarity** matters more than server-side packaging.

## Side-by-side: the same bulk upsert

The inline version, sent from .NET as one parameterized batch:

```csharp
await connection.ExecuteAsync(new CommandDefinition("""
    merge dbo.Prices as target
    using @prices as source on target.Sku = source.Sku
    when matched and target.Price <> source.Price then
        update set Price = source.Price, UpdatedAt = sysutcdatetime()
    when not matched then
        insert (Sku, Price, UpdatedAt) values (source.Sku, source.Price, sysutcdatetime());
    """,
    new { prices = priceTable.AsTableValuedParameter("dbo.PriceList") },
    tx,
    cancellationToken: ct));
```

The procedure version:

```sql
create or alter procedure dbo.UpsertPrices @prices dbo.PriceList readonly
as
begin
    set nocount on;
    merge dbo.Prices as target
    using @prices as source on target.Sku = source.Sku
    when matched and target.Price <> source.Price then
        update set Price = source.Price, UpdatedAt = sysutcdatetime()
    when not matched then
        insert (Sku, Price, UpdatedAt) values (source.Sku, source.Price, sysutcdatetime());
end
```

```csharp
await connection.ExecuteAsync(new CommandDefinition(
    "dbo.UpsertPrices",
    new { prices = priceTable.AsTableValuedParameter("dbo.PriceList") },
    tx,
    commandType: CommandType.StoredProcedure,
    cancellationToken: ct));
```

Performance: equivalent. The deciding questions are about **ownership, permissions and
deployment**, not speed.

## A real-world scenario that justifies a procedure

An insurer's claims database is accessed by a .NET API, a Python fraud model and a vendor's
reporting tool. Auditors require that no application account can read raw policyholder tables.
The team exposes `dbo.GetClaimSummary`, `dbo.RecordAdjustment` and `dbo.CloseClaim` with
`EXECUTE` grants only. This is a correct, deliberate use.

## A real-world scenario that does not

A three-developer team building a single SaaS application has 400 procedures, many of the form
`dbo.Customer_GetById`, plus pricing rules in a 900-line procedure with nested cursors. Nobody can
unit test pricing; every change needs a coordinated database deployment; the reason given is "procs
are faster". This is legacy dogma resting on a factoid.

## Sources

- Microsoft Learn, "Ownership chains" — <https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql/ownership-chaining-in-sql-server>
- Microsoft Learn, "Natively compiled stored procedures" — <https://learn.microsoft.com/en-us/sql/relational-databases/in-memory-oltp/natively-compiled-stored-procedures>
- Microsoft Learn, "Query Store hints" — <https://learn.microsoft.com/en-us/sql/relational-databases/performance/query-store-hints>
- Microsoft Learn, "MERGE (Transact-SQL)" — <https://learn.microsoft.com/en-us/sql/t-sql/statements/merge-transact-sql>
- Microsoft Learn, "Table hints" (READPAST, UPDLOCK) — <https://learn.microsoft.com/en-us/sql/t-sql/queries/hints-transact-sql-table>
