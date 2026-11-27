---
title: "Window Frames: ROWS, RANGE and GROUPS"
questions:
  - id: "default-frame-includes-peers"
    type: "single"
    prompt: "`sales(day, amt)` holds `(1, 10)`, `(2, 20)`, `(2, 30)`, `(3, 40)`. What values does `SUM(amt) OVER (ORDER BY day)` produce, listed by `day, amt`?"
    correct:
      - "10, 60, 60, 100"
    distractors:
      - "10, 30, 60, 100"
      - "100, 100, 100, 100"
      - "10, 60, 60, 90"
    explanation: |-
      When a window has `ORDER BY` but no frame clause, the standard's default frame is `RANGE BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW`. In `RANGE` mode, "current row" means the current row *and all its peers* (rows with equal `ORDER BY` values).

      | day | amt | frame (RANGE, up to current peers) | SUM |
      |---|---|---|---|
      | 1 | 10 | day 1 | 10 |
      | 2 | 20 | days 1–2: 10, 20, 30 | 60 |
      | 2 | 30 | days 1–2: 10, 20, 30 | 60 |
      | 3 | 40 | days 1–3 | 100 |

      "10, 30, 60, 100" is the `ROWS` running total that most people expect; it needs an explicit `ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW` and a unique ordering (for example `ORDER BY day, amt`).

      "100, 100, 100, 100" is the result *without* `ORDER BY`, where the whole partition is the frame.

      "10, 60, 60, 90" sums only the current day and the previous one, which is a `RANGE BETWEEN 1 PRECEDING AND CURRENT ROW` or `GROUPS BETWEEN 1 PRECEDING AND CURRENT ROW` frame.
    checks:
      - engine: "both"
        setup: "CREATE TABLE sales(day INT, amt INT); INSERT INTO sales VALUES (1,10),(2,20),(2,30),(3,40);"
        query: "SELECT day, amt, SUM(amt) OVER (ORDER BY day) FROM sales ORDER BY day, amt"
        expect: [[1, 10, 10], [2, 20, 60], [2, 30, 60], [3, 40, 100]]
        ordered: true
  - id: "rows-moving-sum"
    type: "single"
    prompt: "Same data. What does `SUM(amt) OVER (ORDER BY day, amt ROWS BETWEEN 1 PRECEDING AND CURRENT ROW)` produce, in that order?"
    correct:
      - "10, 30, 50, 70"
    distractors:
      - "10, 30, 60, 100"
      - "10, 60, 60, 90"
      - "30, 50, 70, 40"
      - "10, 20, 30, 40"
    explanation: |-
      `ROWS` counts physical rows in the window order: the frame is the previous row (if any) and the current row.

      | position | day, amt | frame rows | SUM |
      |---|---|---|---|
      | 1 | (1, 10) | 10 | 10 |
      | 2 | (2, 20) | 10, 20 | 30 |
      | 3 | (2, 30) | 20, 30 | 50 |
      | 4 | (3, 40) | 30, 40 | 70 |

      "10, 30, 60, 100" uses `UNBOUNDED PRECEDING` instead of `1 PRECEDING`.

      "10, 60, 60, 90" treats `1 PRECEDING` as "the previous `day` value and all peers", which is `GROUPS` or `RANGE` behavior.

      "30, 50, 70, 40" uses `CURRENT ROW AND 1 FOLLOWING`.

      Adding `amt` to the `ORDER BY` makes the order unique; with ties in the window order, a `ROWS` frame would depend on an arbitrary tie order.

      "10, 20, 30, 40" ignores the preceding row, as if the frame were `CURRENT ROW` alone.
    checks:
      - engine: "both"
        setup: "CREATE TABLE sales(day INT, amt INT); INSERT INTO sales VALUES (1,10),(2,20),(2,30),(3,40);"
        query: "SELECT SUM(amt) OVER (ORDER BY day, amt ROWS BETWEEN 1 PRECEDING AND CURRENT ROW) FROM sales ORDER BY day, amt"
        expect: [[10], [30], [50], [70]]
        ordered: true
  - id: "groups-frame-trace"
    type: "single"
    prompt: "Same data. What does `SUM(amt) OVER (ORDER BY day GROUPS BETWEEN 1 PRECEDING AND CURRENT ROW)` produce, listed by `day, amt`?"
    correct:
      - "10, 60, 60, 90"
    distractors:
      - "10, 30, 50, 70"
      - "10, 60, 60, 100"
      - "10, 50, 50, 40"
    explanation: |-
      `GROUPS` counts *peer groups*: all rows with the same `ORDER BY` value form one group. `1 PRECEDING` means "the previous peer group", and `CURRENT ROW` means "the current row's whole peer group".

      | peer group | rows | frame = previous group + current group | SUM |
      |---|---|---|---|
      | day 1 | 10 | (none) + {10} | 10 |
      | day 2 | 20, 30 | {10} + {20, 30} | 60 for both rows |
      | day 3 | 40 | {20, 30} + {40} | 90 |

      "10, 30, 50, 70" is the `ROWS 1 PRECEDING` result, which counts individual rows.

      "10, 60, 60, 100" is the default `RANGE UNBOUNDED PRECEDING` running total.

      "10, 50, 50, 40" sums only the current peer group, as `GROUPS BETWEEN CURRENT ROW AND CURRENT ROW` would; it forgets the preceding group.

      With integer days spaced by exactly 1, `RANGE BETWEEN 1 PRECEDING AND CURRENT ROW` gives the same values here; it differs when values have gaps, because `RANGE` measures distance in values and `GROUPS` in groups.
    checks:
      - engine: "both"
        setup: "CREATE TABLE sales(day INT, amt INT); INSERT INTO sales VALUES (1,10),(2,20),(2,30),(3,40);"
        query: "SELECT day, amt, SUM(amt) OVER (ORDER BY day GROUPS BETWEEN 1 PRECEDING AND CURRENT ROW) FROM sales ORDER BY day, amt"
        expect: [[1, 10, 10], [2, 20, 60], [2, 30, 60], [3, 40, 90]]
        ordered: true
  - id: "last-value-default-frame"
    type: "single"
    prompt: "`scores(day, pts)` holds `(1, 10)`, `(2, 20)`, `(3, 30)`. A developer wants every row to show the last day's points and writes `LAST_VALUE(pts) OVER (ORDER BY day)`. What does it return, by day?"
    correct:
      - "10, 20, 30"
    distractors:
      - "30, 30, 30"
      - "10, 10, 10"
      - "NULL, NULL, 30"
    explanation: |-
      `LAST_VALUE` returns the value from the last row of the *frame*, not of the partition. With `ORDER BY` and no frame clause, the frame ends at the current row (and its peers), so the last row of the frame is the current row itself.

      | day | frame | LAST_VALUE |
      |---|---|---|
      | 1 | day 1 | 10 |
      | 2 | days 1–2 | 20 |
      | 3 | days 1–3 | 30 |

      "30, 30, 30" is what the developer wanted; it requires `ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING`, or `FIRST_VALUE(pts) OVER (ORDER BY day DESC)`.

      "10, 10, 10" is `FIRST_VALUE` with the default frame.

      "NULL, NULL, 30" imagines that rows without a "last" value yield NULL; every frame here is non-empty.
    checks:
      - engine: "both"
        setup: "CREATE TABLE scores(day INT, pts INT); INSERT INTO scores VALUES (1,10),(2,20),(3,30);"
        query: "SELECT LAST_VALUE(pts) OVER (ORDER BY day), LAST_VALUE(pts) OVER (ORDER BY day ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING) FROM scores ORDER BY day"
        expect: [[10, 30], [20, 30], [30, 30]]
        ordered: true
  - id: "order-by-changes-frame"
    type: "multiple"
    prompt: "`sales(region, day, amt)` holds `('a', 1, 10)`, `('a', 2, 20)`, `('b', 1, 5)`. Consider these two expressions:\n\n```sql\nSUM(amt) OVER (PARTITION BY region)               -- E1\nSUM(amt) OVER (PARTITION BY region ORDER BY day)  -- E2\n```\n\nWhich statements are true? Select all that apply."
    correct:
      - "E1 gives 30, 30 for region `a` and 5 for region `b`"
      - "E2 gives 10, 30 for region `a` (by day) and 5 for region `b`"
    distractors:
      - "E1 and E2 always return the same values, because `ORDER BY` only sorts the output"
      - "E2 gives 10, 30 for region `a` and 15 for region `b`, continuing the total across partitions"
      - "E1 is invalid, because a window aggregate needs `ORDER BY`"
    explanation: |-
      Inside `OVER`, `ORDER BY` is not about presentation: it changes the default frame. Without it, every row's frame is the entire partition; with it, the frame runs from the partition start to the current row's peers.

      | region | day | amt | E1 (whole partition) | E2 (running) |
      |---|---|---|---|---|
      | a | 1 | 10 | 30 | 10 |
      | a | 2 | 20 | 30 | 30 |
      | b | 1 | 5 | 5 | 5 |

      "Always the same" treats window `ORDER BY` like the query's `ORDER BY`.

      "15 for region `b`" ignores `PARTITION BY`; frames never cross partitions.

      "E1 is invalid" is false; aggregates over a whole partition are the simplest window functions.
    checks:
      - engine: "both"
        setup: "CREATE TABLE sales(region TEXT, day INT, amt INT); INSERT INTO sales VALUES ('a',1,10),('a',2,20),('b',1,5);"
        query: "SELECT region, day, SUM(amt) OVER (PARTITION BY region), SUM(amt) OVER (PARTITION BY region ORDER BY day) FROM sales ORDER BY region, day"
        expect: [["a", 1, 30, 10], ["a", 2, 30, 30], ["b", 1, 5, 5]]
        ordered: true
  - id: "groups-engine-support"
    type: "single"
    prompt: "A query with `GROUPS BETWEEN 1 PRECEDING AND CURRENT ROW` runs in PostgreSQL 19 but fails in MySQL 8.4. Which explanation is correct?"
    correct:
      - "`GROUPS` frame units are standard SQL (added in SQL:2011), and the MySQL 8.4 manual states that `GROUPS` is parsed but produces an error"
    distractors:
      - "`GROUPS` is a PostgreSQL extension that is not part of the SQL standard"
      - "MySQL 8.4 accepts `GROUPS` only together with `PARTITION BY`"
      - "`GROUPS` was removed from the standard in SQL:2016, and MySQL follows the newer standard"
    explanation: |-
      SQL:2011 added `GROUPS` frame units alongside `ROWS` and `RANGE`. PostgreSQL implemented all SQL:2011 framing options in version 11; SQLite supports `GROUPS` too. The MySQL 8.4 manual's list of window-function restrictions says the `GROUPS` frame units specifier is parsed but produces an error, and only `ROWS` and `RANGE` are supported.

      "A PostgreSQL extension" is false; `GROUPS` is standard.

      "Only with `PARTITION BY`" invents a condition.

      "Removed in SQL:2016" is false.

      Portable workaround: compute a dense rank of the ordering value (`DENSE_RANK() OVER (ORDER BY day)`) in a subquery and use a `RANGE` frame over that rank, which spaces peer groups exactly 1 apart.
body_checks:
  - engine: "both"
    setup: "CREATE TABLE sales(day INT, amt INT); INSERT INTO sales VALUES (1,10),(2,20),(2,30),(3,40);"
    query: "SELECT day, amt, SUM(amt) OVER (ORDER BY day), SUM(amt) OVER (ORDER BY day, amt ROWS UNBOUNDED PRECEDING), SUM(amt) OVER (ORDER BY day, amt ROWS BETWEEN 1 PRECEDING AND CURRENT ROW), SUM(amt) OVER (ORDER BY day GROUPS BETWEEN 1 PRECEDING AND CURRENT ROW) FROM sales ORDER BY day, amt"
    expect: [[1, 10, 10, 10, 10, 10], [2, 20, 60, 30, 30, 60], [2, 30, 60, 60, 50, 60], [3, 40, 100, 100, 70, 90]]
    ordered: true
---
## Learning objectives

- Name the three frame units and what each counts.
- Predict the default frame and the results it produces with ties.
- Avoid the `LAST_VALUE` default-frame trap.
- Explain how `ORDER BY` inside `OVER` changes an aggregate's result.

## The invariant

A window function is computed for each row over a **window**: the row's partition (`PARTITION BY`), ordered by the window's `ORDER BY`, and narrowed to a **frame**. SQL:2003 introduced window functions into ISO SQL; SQL:2011 added the `GROUPS` frame unit.

| Frame unit | Offsets count | `CURRENT ROW` means |
|---|---|---|
| `ROWS` | physical rows | exactly this row |
| `RANGE` | difference in the `ORDER BY` value | this row and all its peers |
| `GROUPS` | peer groups (distinct `ORDER BY` values) | this row's whole peer group |

The default frame depends on `ORDER BY`:

| Window | Default frame |
|---|---|
| no `ORDER BY` | the whole partition |
| with `ORDER BY` | `RANGE BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW` |

So adding `ORDER BY` to an aggregate window turns a partition total into a running total, and because the default unit is `RANGE`, rows that tie on the ordering value get *the same* running total. Ranking functions (`ROW_NUMBER`, `RANK`, ...) and `LAG`/`LEAD` ignore frames; aggregates and `FIRST_VALUE`/`LAST_VALUE`/`NTH_VALUE` use them.

Window functions are computed at step 5 of the logical order, after `WHERE`, `GROUP BY` and `HAVING`, which is why they cannot appear in those clauses.

Unlike `GROUP BY`, a window does not collapse rows: every input row stays in the output and gains a value computed from its window. That is why the frame matters so much. The same aggregate, over the same partition, can mean "total for the group", "total so far" or "total of the last three rows", and only the frame clause says which. When the frame is left implicit, the result depends on whether the window has an `ORDER BY`, and with ties, on the difference between `ROWS` and `RANGE`.

## Worked example

`sales(day, amt)`: (1, 10), (2, 20), (2, 30), (3, 40).

```sql
SELECT day, amt,
  SUM(amt) OVER (ORDER BY day)                                     AS default_range,
  SUM(amt) OVER (ORDER BY day, amt ROWS UNBOUNDED PRECEDING)       AS rows_running,
  SUM(amt) OVER (ORDER BY day, amt ROWS BETWEEN 1 PRECEDING AND CURRENT ROW) AS rows_moving,
  SUM(amt) OVER (ORDER BY day GROUPS BETWEEN 1 PRECEDING AND CURRENT ROW)   AS groups_1
FROM sales
ORDER BY day, amt;
```

| day | amt | default_range | rows_running | rows_moving | groups_1 |
|---|---|---|---|---|---|
| 1 | 10 | 10 | 10 | 10 | 10 |
| 2 | 20 | 60 | 30 | 30 | 60 |
| 2 | 30 | 60 | 60 | 50 | 60 |
| 3 | 40 | 100 | 100 | 70 | 90 |

Read the two day-2 rows: the default `RANGE` frame treats them as one step, `ROWS` steps through them one at a time, and `GROUPS` reaches back one whole day.

## Where this shows up today

As of November 2026, window functions with `ROWS` and `RANGE` frames are available in PostgreSQL 19, MySQL 8.4, SQL Server 2025 and SQLite; `GROUPS` frames work in PostgreSQL (since version 11) and SQLite, while the MySQL 8.4 manual lists `GROUPS` as parsed but unsupported. Frames are the standard tool for moving averages and running balances in analytics queries.

## Pitfalls checklist

- Writing a running total with only `ORDER BY` and getting equal totals for tied rows.
- Using `LAST_VALUE` with the default frame and getting the current row's value.
- Using `ROWS` frames over a non-unique ordering; results depend on arbitrary tie order.
- Assuming window `ORDER BY` sorts the query output; add a query-level `ORDER BY`.
- Using `RANGE` with numeric offsets when values have gaps and expecting row counts.

## Key takeaways

- `ROWS` counts rows, `RANGE` measures value distance, `GROUPS` counts peer groups.
- With `ORDER BY` and no frame, the default is `RANGE UNBOUNDED PRECEDING` to the current row's peers.
- Without `ORDER BY`, the frame is the whole partition.
- Spell out the frame whenever ties are possible or `LAST_VALUE` is involved.

## Sources

- ISO/IEC 9075-2:2003 (window functions, OLAP amendment) and ISO/IEC 9075-2:2011 (`GROUPS` frame units).
- PostgreSQL 11 release notes; PostgreSQL 19 documentation, "Window Function Calls".
- MySQL 8.4 Reference Manual, "Window Function Frame Specification" and "Window Function Restrictions"; SQLite documentation, "Window Functions".
