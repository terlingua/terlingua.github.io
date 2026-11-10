---
title: The "precompiled" factoid
questions:
  - id: precompiled
    prompt: "\"Stored procedures are faster because they are precompiled.\" In modern SQL Server, what is the accurate statement?"
    options:
      - id: both-cached
        text: Procedures are compiled to a plan on first execution and cached; parameterized ad hoc statements (`sp_executesql`, which ADO.NET uses for parameterized commands) are compiled and cached the same way
        correct: true
      - text: True — procedures are compiled to machine code when created
      - text: Ad hoc SQL is never cached
      - text: Only procedures can use indexes
    explanation: Since SQL Server 7.0, the engine's plan caching extends most of the performance benefits once unique to procedures to all parameterized statements. The exception is natively compiled procedures for In-Memory OLTP, which really are compiled to machine code.
  - id: adonet-parameterized
    prompt: How does SqlClient send a command with parameters, such as Dapper's `select ... where Id = @id`?
    options:
      - text: As a literal string with values substituted
      - text: As a temporary stored procedure created and dropped per call
      - text: As a prepared statement only on Windows
      - id: sp-executesql
        text: As a call to `sp_executesql` with the statement text and typed parameter definitions
        correct: true
    explanation: The plan is cached keyed on the statement text and parameter types, and reused for different parameter values — exactly like a procedure's plan.
  - id: sniffing
    prompt: Which statement about parameter sniffing is accurate?
    options:
      - text: It only affects inline SQL
      - text: It only affects stored procedures
      - text: It was removed in SQL Server 2019
      - id: both
        text: It affects both — the plan compiled for the first parameter values is reused for later values, which can be good or bad for skewed data
        correct: true
    explanation: Moving a query into a procedure neither causes nor cures sniffing. Mitigations (`OPTION (RECOMPILE)`, `OPTIMIZE FOR`, Query Store hints, and SQL Server 2022's parameter-sensitive plan optimization) apply to both.
  - id: network
    prompt: "\"Procedures save network bandwidth because `EXEC dbo.GetOrder @id` is shorter than the full SELECT.\" How significant is this today for a typical query?"
    options:
      - text: It is the main reason procedures are faster
      - text: It doubles throughput
      - text: Inline SQL is sent as an uncompressed image
      - id: negligible
        text: Negligible — a few hundred bytes of statement text is tiny next to result sets and round-trip latency
        correct: true
    explanation: The real network argument is about *round trips* and *data moved*, not statement length — and that argument is about where set-based work happens, which Section 2 covers.
  - id: plan-pollution
    prompt: Which practice genuinely does pollute the plan cache?
    type: multiple
    options:
      - id: literals
        text: Concatenating values into SQL text, so every distinct value produces a new statement and a new plan
        correct: true
      - id: varying-lengths
        text: Passing string parameters whose declared length varies with each value, so each length gets its own plan
        correct: true
      - text: Using parameterized inline SQL with consistent types
      - id: in-lists
        text: Expanding large `IN` lists of varying size into different numbers of parameters
        correct: true
    explanation: Plan cache pollution is about *distinct statement text*. Parameterize, keep parameter types and lengths stable, and use TVPs or arrays for large lists.
  - id: factoid-anatomy
    prompt: Which features make "procs are precompiled" a textbook factoid?
    type: multiple
    options:
      - id: kernel
        text: It had a kernel of truth in early SQL Server versions
        correct: true
      - id: no-date
        text: It is repeated without a date or version attached
        correct: true
      - id: mechanism
        text: It offers a plausible-sounding mechanism ("compiled" code is fast)
        correct: true
      - text: It was never true in any version of any database
    explanation: Factoids are rarely pure invention. Their danger is that the kernel of truth makes them resistant to correction.
  - id: security-claim
    prompt: "\"Stored procedures prevent SQL injection.\" Which is accurate?"
    options:
      - text: True in all cases
      - text: False; procedures increase injection risk
      - id: params-prevent
        text: Parameterization prevents injection; a procedure that concatenates its inputs into dynamic SQL and runs `EXEC(@sql)` is fully injectable, and parameterized inline SQL is fully safe
        correct: true
      - text: Only if the procedure is encrypted
    explanation: The protection comes from separating code and data. Procedures are one way to do that; parameterized commands are another.
---
## Learning objectives

- Trace the history of the "precompiled" claim
- Explain plan caching for procedures and parameterized statements
- Separate the real performance issues (parameter sniffing, plan pollution, round trips) from the myth

## Where the factoid came from

In early SQL Server versions (6.x and before), stored procedures received special treatment:
their plans were cached and reused, while ad hoc batches were generally compiled each time.
"Use procedures for performance" was sound advice in 1996.

SQL Server 7.0 (1998) and 2000 reworked query processing. Microsoft's own documentation from that
era explained that changes to statement processing extended many of the performance benefits of
stored procedures to all SQL statements, via plan caching for ad hoc and parameterized queries
and the introduction of `sp_executesql`. The advice was obsolete before most of today's .NET
developers wrote their first line of code. It is a near-perfect example of an anachronism kept
alive as a factoid.

## What actually happens today

```sql
exec sp_executesql
    N'select Id, Status, Total from dbo.Orders where CustomerId = @customerId',
    N'@customerId uniqueidentifier',
    @customerId = '7d4e...';
```

That is what SqlClient sends for a parameterized command. SQL Server:

1. Hashes the statement text and parameter definitions
2. Looks for a cached plan
3. Compiles and caches one if none exists
4. Reuses it for every later execution with any `@customerId`

A stored procedure follows the same steps, keyed by object id instead of text.

## You can verify this yourself

```sql
select cp.usecounts, cp.objtype, st.text
from sys.dm_exec_cached_plans cp
cross apply sys.dm_exec_sql_text(cp.plan_handle) st
where st.text like '%dbo.Orders where CustomerId%'
  and st.text not like '%dm_exec_cached_plans%';
```

Run your parameterized query several times: `objtype = Prepared`, `usecounts` climbing.

## The real performance issues

| Issue | Affects | Fix |
|---|---|---|
| Plan cache pollution | Concatenated SQL, unstable parameter types | Parameterize; stable types and lengths |
| Parameter sniffing | Procedures *and* parameterized SQL | `OPTION (RECOMPILE)`, `OPTIMIZE FOR`, Query Store hints, PSP optimization |
| Chatty round trips | Code that loops over rows from the app | Set-based SQL, batching, sometimes a procedure |
| Moving lots of data to the app to compute a small answer | Any | Compute where the data lives |

The last two are where procedures *can* legitimately help — the subject of the next section.

## Sources

- Microsoft Learn, "Query processing architecture guide — execution plan caching and reuse" — <https://learn.microsoft.com/en-us/sql/relational-databases/query-processing-architecture-guide>
- Microsoft Learn, `sp_executesql` — <https://learn.microsoft.com/en-us/sql/relational-databases/system-stored-procedures/sp-executesql-transact-sql>
- Microsoft Learn, "Parameter Sensitive Plan optimization" — <https://learn.microsoft.com/en-us/sql/relational-databases/performance/parameter-sensitive-plan-optimization>
- Erland Sommarskog, "Slow in the Application, Fast in SSMS?" — <https://www.sommarskog.se/query-plan-mysteries.html>
- Erland Sommarskog, "The Curse and Blessings of Dynamic SQL" — <https://www.sommarskog.se/dynamic_sql.html>
