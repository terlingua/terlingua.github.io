---
title: "Running Totals, LAG/LEAD, and Gaps and Islands"
questions:
  - id: "partitioned-running-balance"
    type: "single"
    prompt: "`txn(acct, day, amt)` holds `('x', 1, 100)`, `('x', 2, -30)`, `('x', 3, 50)`, `('y', 1, 20)`, `('y', 2, 10)`. With `SUM(amt) OVER (PARTITION BY acct ORDER BY day ROWS UNBOUNDED PRECEDING) AS bal`, what are `bal` for `('x', 3)` and `('y', 2)`?"
    correct:
      - "120 and 30"
    distractors:
      - "120 and 150"
      - "50 and 10"
      - "150 and 30"
    explanation: |-
      `PARTITION BY acct` restarts the computation for each account; `ORDER BY day` with a `ROWS UNBOUNDED PRECEDING` frame sums from the partition's first row to the current row.

      | acct | day | amt | running balance |
      |---|---|---|---|
      | x | 1 | 100 | 100 |
      | x | 2 | -30 | 70 |
      | x | 3 | 50 | 120 |
      | y | 1 | 20 | 20 |
      | y | 2 | 10 | 30 |

      "120 and 150" lets the total run across partitions (120 + 20 + 10).

      "50 and 10" returns the row's own amount, ignoring the frame.

      "150 and 30" sums only the positive amounts of `x` (100 + 50), treating the withdrawal as excluded.
    checks:
      - engine: "both"
        setup: "CREATE TABLE txn(acct TEXT, day INT, amt INT); INSERT INTO txn VALUES ('x',1,100),('x',2,-30),('x',3,50),('y',1,20),('y',2,10);"
        query: "SELECT acct, day, SUM(amt) OVER (PARTITION BY acct ORDER BY day ROWS UNBOUNDED PRECEDING) FROM txn ORDER BY acct, day"
        expect: [["x", 1, 100], ["x", 2, 70], ["x", 3, 120], ["y", 1, 20], ["y", 2, 30]]
        ordered: true
  - id: "lag-difference"
    type: "single"
    prompt: "`readings(day, v)` holds `(1, 10)`, `(2, 15)`, `(3, 12)`. What does `v - LAG(v) OVER (ORDER BY day)` return, by day?"
    correct:
      - "NULL, 5, -3"
    distractors:
      - "0, 5, -3"
      - "5, -3, NULL"
      - "10, 5, -3"
    explanation: |-
      `LAG(v)` returns `v` from the previous row in window order; for the first row there is no previous row, so it returns the default, which is NULL unless a third argument is given. `v - NULL` is NULL.

      | day | v | LAG(v) | v − LAG(v) |
      |---|---|---|---|
      | 1 | 10 | NULL | NULL |
      | 2 | 15 | 10 | 5 |
      | 3 | 12 | 15 | -3 |

      "0, 5, -3" assumes the default is 0; write `LAG(v, 1, v)` or `COALESCE(...)` if that is the desired meaning.

      "5, -3, NULL" uses `LEAD` (the next row) and subtracts in the other direction.

      "10, 5, -3" uses `LAG(v, 1, 0)`, which treats the missing predecessor as zero.
    checks:
      - engine: "both"
        setup: "CREATE TABLE readings(day INT, v INT); INSERT INTO readings VALUES (1,10),(2,15),(3,12);"
        query: "SELECT v - LAG(v) OVER (ORDER BY day) FROM readings ORDER BY day"
        expect: [[null], [5], [-3]]
        ordered: true
  - id: "lead-stops-at-partition"
    type: "single"
    prompt: "Same `txn` table. What is `LEAD(day) OVER (PARTITION BY acct ORDER BY day)` on the row `('x', 3, 50)`?"
    correct:
      - "NULL"
    distractors:
      - "1, the first day of account `y`"
      - "3, the current row's own day"
      - "2, the previous day of account `x`"
    explanation: |-
      `LEAD` and `LAG` look only within the current partition. `('x', 3)` is the last row of partition `x`, so there is no following row and `LEAD` returns its default, NULL.

      | acct | day | LEAD(day) |
      |---|---|---|
      | x | 1 | 2 |
      | x | 2 | 3 |
      | x | 3 | NULL |
      | y | 1 | 2 |
      | y | 2 | NULL |

      "1, the first day of account `y`" would happen without `PARTITION BY`: the windows would then cover the whole table.

      "3" confuses `LEAD` with the current value.

      "2" is `LAG`, the previous row.
    checks:
      - engine: "both"
        setup: "CREATE TABLE txn(acct TEXT, day INT, amt INT); INSERT INTO txn VALUES ('x',1,100),('x',2,-30),('x',3,50),('y',1,20),('y',2,10);"
        query: "SELECT acct, day, LEAD(day) OVER (PARTITION BY acct ORDER BY day) FROM txn ORDER BY acct, day"
        expect: [["x", 1, 2], ["x", 2, 3], ["x", 3, null], ["y", 1, 2], ["y", 2, null]]
        ordered: true
  - id: "island-key-value"
    type: "single"
    prompt: "A user logged in on days `1`, `2`, `3`, `5`, `6`, `9` (one row per day). For the row with `day = 6`, what is `day - ROW_NUMBER() OVER (ORDER BY day)`?"
    correct:
      - "1"
    distractors:
      - "0"
      - "5"
      - "3"
    explanation: |-
      Within a run of consecutive days, both `day` and `ROW_NUMBER()` increase by exactly 1 per row, so their difference is constant; a gap in `day` increases the difference. That constant is the island's grouping key.

      | day | ROW_NUMBER | day − ROW_NUMBER |
      |---|---|---|
      | 1 | 1 | 0 |
      | 2 | 2 | 0 |
      | 3 | 3 | 0 |
      | 5 | 4 | 1 |
      | 6 | 5 | 1 |
      | 9 | 6 | 3 |

      Day 6 is the fifth row: 6 − 5 = 1.

      "0" is the key of the first island (days 1–3).

      "5" is the row number, not the difference.

      "3" is the key of the last island (day 9).
    checks:
      - engine: "both"
        setup: "CREATE TABLE logins(day INT); INSERT INTO logins VALUES (1),(2),(3),(5),(6),(9);"
        query: "SELECT g FROM (SELECT day, day - ROW_NUMBER() OVER (ORDER BY day) AS g FROM logins) t WHERE day = 6"
        expect: [[1]]
  - id: "island-count-and-length"
    type: "single"
    prompt: "Same login days. What does this return?\n\n```sql\nSELECT MIN(day) AS first_day, MAX(day) AS last_day, COUNT(*) AS len\nFROM (SELECT day, day - ROW_NUMBER() OVER (ORDER BY day) AS g\n      FROM logins) AS t\nGROUP BY g;\n```"
    correct:
      - "Three rows: (1, 3, 3), (5, 6, 2), (9, 9, 1)"
    distractors:
      - "Two rows: (1, 3, 3), (5, 9, 3)"
      - "Six rows, one per login day"
      - "Four rows: (1, 3, 3), (4, 4, 0), (5, 6, 2), (9, 9, 1)"
    explanation: |-
      Grouping by the key from the previous question collapses each run of consecutive days into one row.

      | g | days | first_day | last_day | len |
      |---|---|---|---|---|
      | 0 | 1, 2, 3 | 1 | 3 | 3 |
      | 1 | 5, 6 | 5 | 6 | 2 |
      | 3 | 9 | 9 | 9 | 1 |

      So there are three islands and the longest streak is 3 days.

      "(5, 9, 3)" merges two islands across the gap at days 7–8; the key changes there (1 → 3).

      "Six rows" forgets the `GROUP BY`.

      "(4, 4, 0)" invents a row for a gap; gaps are absent rows, and `GROUP BY` produces groups only from rows that exist. (Gaps can be found separately, for example with `LEAD(day) - day > 1`.)
    checks:
      - engine: "both"
        setup: "CREATE TABLE logins(day INT); INSERT INTO logins VALUES (1),(2),(3),(5),(6),(9);"
        query: "SELECT MIN(day), MAX(day), COUNT(*) FROM (SELECT day, day - ROW_NUMBER() OVER (ORDER BY day) AS g FROM logins) t GROUP BY g"
        expect: [[1, 3, 3], [5, 6, 2], [9, 9, 1]]
  - id: "duplicates-break-islands"
    type: "multiple"
    prompt: "Now the login table records each login, so days are `1`, `2`, `2`, `3`. Which statements about the gaps-and-islands key are true? Select all that apply."
    correct:
      - "With `day - ROW_NUMBER()`, the duplicate day 2 splits the single streak 1–3 into two groups"
      - "With `day - DENSE_RANK()`, all four rows get the same key, so days 1–3 form one island"
    distractors:
      - "`ROW_NUMBER()` gives both day-2 rows the same number, so `day - ROW_NUMBER()` is unaffected"
      - "The key should be `day + ROW_NUMBER()`, which is constant within a streak"
      - "Duplicates cannot be handled with window functions; the table must be de-duplicated first"
    explanation: |-
      The trick relies on `day` and the row counter rising in lockstep. A duplicate day adds a row without advancing `day`, so the difference drops.

      | day | ROW_NUMBER | day − ROW_NUMBER | DENSE_RANK | day − DENSE_RANK |
      |---|---|---|---|---|
      | 1 | 1 | 0 | 1 | 0 |
      | 2 | 2 | 0 | 2 | 0 |
      | 2 | 3 | -1 | 2 | 0 |
      | 3 | 4 | -1 | 3 | 0 |

      With `ROW_NUMBER` the keys are 0, 0, −1, −1: two groups for one streak. `DENSE_RANK` advances only on a new day value, so it moves in lockstep with distinct days and the key stays 0.

      Distractors: `ROW_NUMBER` never gives equal numbers; `day + ROW_NUMBER()` grows by 2 per row and is never constant; and de-duplicating first (`SELECT DISTINCT day`) works but is not required, since `DENSE_RANK` handles it.
    checks:
      - engine: "both"
        setup: "CREATE TABLE logins(day INT); INSERT INTO logins VALUES (1),(2),(2),(3);"
        query: "SELECT (SELECT COUNT(DISTINCT g) FROM (SELECT day - ROW_NUMBER() OVER (ORDER BY day) AS g FROM logins) a), (SELECT COUNT(DISTINCT g) FROM (SELECT day - DENSE_RANK() OVER (ORDER BY day) AS g FROM logins) b)"
        expect: [[2, 1]]
body_checks:
  - engine: "both"
    setup: "CREATE TABLE events(minute INT); INSERT INTO events VALUES (0),(10),(50),(55),(120);"
    query: "SELECT minute, SUM(new_session) OVER (ORDER BY minute ROWS UNBOUNDED PRECEDING) AS session_no FROM (SELECT minute, CASE WHEN minute - LAG(minute) OVER (ORDER BY minute) <= 30 THEN 0 ELSE 1 END AS new_session FROM events) AS flagged ORDER BY minute"
    expect: [[0, 1], [10, 1], [50, 2], [55, 2], [120, 3]]
    ordered: true
---
## Learning objectives

- Compute partitioned running totals with an explicit frame.
- Use `LAG` and `LEAD` for row-to-row differences, and handle partition boundaries.
- Find islands (consecutive runs) with the "value minus row number" key.
- Adapt the islands key when the sequence contains duplicates.

## The invariant

Many "sequential" questions (balances, deltas, streaks, sessions) need to relate a row to its neighbors. Before window functions, SQL answered them with self-joins or correlated subqueries, both quadratic in the worst case. Window functions express them directly:

| Need | Window tool | Key detail |
|---|---|---|
| Running total per entity | `SUM(x) OVER (PARTITION BY e ORDER BY t ROWS UNBOUNDED PRECEDING)` | `ROWS` avoids the peer-tie effect of the default `RANGE` frame |
| Change from previous row | `x - LAG(x) OVER (PARTITION BY e ORDER BY t)` | first row of each partition yields NULL (or the default argument) |
| Next event | `LEAD(t) OVER (PARTITION BY e ORDER BY t)` | last row of each partition yields NULL |
| Islands of consecutive values | group by `x - ROW_NUMBER() OVER (ORDER BY x)` | `x` must step by exactly 1 within a run and be unique |

The islands key rests on a simple invariant: if two sequences both increase by 1 per step, their difference is constant; whenever the value sequence jumps by more than 1 (a gap), the difference increases. When values can repeat, use `DENSE_RANK()` instead of `ROW_NUMBER()`, because `DENSE_RANK` advances by 1 per distinct value. For dates, subtract the row number as a number of days (`day - ROW_NUMBER() * INTERVAL '1 day'` in PostgreSQL syntax, or date arithmetic in your dialect).

`LAG(x, n, default)` and `LEAD(x, n, default)` take an offset and a default for "no such row"; they ignore frames but respect partitions.

## Worked example

Session detection: a new session starts when the gap since the previous event exceeds 30 minutes. With event minutes 0, 10, 50, 55, 120 for one user:

```sql
SELECT minute,
       SUM(new_session) OVER (ORDER BY minute ROWS UNBOUNDED PRECEDING) AS session_no
FROM (
  SELECT minute,
         CASE WHEN minute - LAG(minute) OVER (ORDER BY minute) <= 30
              THEN 0 ELSE 1 END AS new_session
  FROM events
) AS flagged
ORDER BY minute;
```

| minute | gap from previous | new_session | session_no |
|---|---|---|---|
| 0 | NULL | 1 | 1 |
| 10 | 10 | 0 | 1 |
| 50 | 40 | 1 | 2 |
| 55 | 5 | 0 | 2 |
| 120 | 65 | 1 | 3 |

The first row's `LAG` is NULL, so `NULL <= 30` is UNKNOWN, `CASE` falls to `ELSE`, and the first event starts session 1: three-valued logic doing useful work. A running sum of start flags then numbers the sessions. This "flag, then running-sum the flags" pattern generalizes to most islands problems with irregular steps.

## Where this shows up today

As of November 2026, `LAG`, `LEAD` and running-total frames are supported by PostgreSQL 19, MySQL 8.4, SQL Server 2025 and SQLite alike, which makes window-based sessionization, streak and balance queries portable across them. Event and telemetry pipelines use the same pattern at scale; the logic is identical whether the engine is transactional or columnar.

## Pitfalls checklist

- Writing running totals with the default `RANGE` frame and getting equal totals for tied timestamps.
- Forgetting `PARTITION BY`, so `LAG`/`LEAD` read across entities.
- Treating the NULL from `LAG` on the first row as zero without saying so.
- Applying `x - ROW_NUMBER()` to data with duplicates or non-unit steps.
- Rebuilding these with self-joins, which are slower and easier to get wrong.

## Key takeaways

- Running totals need `PARTITION BY`, `ORDER BY` and preferably an explicit `ROWS` frame.
- `LAG`/`LEAD` stop at partition boundaries and return NULL (or the default) there.
- `value - ROW_NUMBER()` is constant within a run of consecutive values.
- Use `DENSE_RANK()` in the islands key when values repeat, or flag-and-sum for irregular gaps.

## Sources

- ISO/IEC 9075-2:2003 (window aggregates and `ROW_NUMBER`/`DENSE_RANK`) and ISO/IEC 9075-2:2011 (`LAG`, `LEAD`, `FIRST_VALUE`, `LAST_VALUE`, `NTH_VALUE`).
- PostgreSQL 19 documentation, "Window Functions"; SQLite documentation, "Window Functions"; MySQL 8.4 Reference Manual, "Window Function Descriptions".
