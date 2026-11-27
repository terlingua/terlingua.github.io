---
title: "NOT IN Versus NOT EXISTS"
questions:
  - id: "not-in-with-null-empty"
    type: "single"
    prompt: "`customers(id)` holds `1`, `2`, `3`. `blocked(customer_id)` holds `2` and `NULL`. What does this return?\n\n```sql\nSELECT id FROM customers\nWHERE id NOT IN (SELECT customer_id FROM blocked);\n```"
    explanation: |-
      ISO SQL defines `x NOT IN (subquery)` as `NOT (x IN (subquery))`, and `x IN (v1, v2, ...)` as `x = v1 OR x = v2 OR ...`. So `x NOT IN (v1, v2)` is `x <> v1 AND x <> v2`.

      | id | `id <> 2` | `id <> NULL` | AND | kept? |
      |---|---|---|---|---|
      | 1 | TRUE | UNKNOWN | UNKNOWN | no |
      | 2 | FALSE | UNKNOWN | FALSE | no |
      | 3 | TRUE | UNKNOWN | UNKNOWN | no |

      One NULL in the subquery makes every comparison either FALSE or UNKNOWN, never TRUE, so the query returns nothing, silently.

      "`1`, `3`" is the intended answer; `NOT EXISTS` produces it.

      "`1`, `2`, `3`" ignores the subquery.

      "A NULL row" invents output; `NOT IN` filters rows, it does not add any.

      "`3` only" invents an evaluation order between rows; each row is judged independently, and both 1 and 3 end up UNKNOWN.
    options:
      - text: "`1`, `3`"
      - text: "`1`, `2`, `3`"
      - text: "No rows"
        correct: true
      - text: "`1`, `3`, and one NULL row"
      - text: "`3` only, because `1` is the smallest id and is compared first"
  - id: "not-exists-correct"
    type: "single"
    prompt: "Same data (`blocked` holds `2` and `NULL`). What does this return?\n\n```sql\nSELECT c.id FROM customers c\nWHERE NOT EXISTS (\n  SELECT 1 FROM blocked b WHERE b.customer_id = c.id\n);\n```"
    explanation: |-
      `NOT EXISTS` asks whether the correlated subquery returns zero rows. Inside it, `b.customer_id = c.id` is UNKNOWN for the NULL row, so that row is simply not returned by the subquery's `WHERE`. `EXISTS` itself is never UNKNOWN: it is TRUE or FALSE.

      | c.id | blocked rows where `customer_id = c.id` is TRUE | NOT EXISTS |
      |---|---|---|
      | 1 | none | TRUE |
      | 2 | (2) | FALSE |
      | 3 | none | TRUE |

      Result: 1 and 3.

      "No rows" carries the `NOT IN` behavior over to `NOT EXISTS`; the NULL row only fails to match.

      "`1`, `2`, `3`" ignores the match for 2.

      "`3` only" assumes the NULL row matches customer 1 for some reason; NULL matches nothing.

      "`2` only" is the answer to `EXISTS`, not `NOT EXISTS`.
    options:
      - text: "No rows, because `blocked` contains a NULL"
      - text: "`1`, `2`, `3`"
      - text: "`3` only"
      - text: "`1`, `3`"
        correct: true
      - text: "`2` only, because it is the one customer found in `blocked`"
  - id: "in-with-null-still-matches"
    type: "single"
    prompt: "Same `customers` (1, 2, 3). What does `SELECT id FROM customers WHERE id IN (2, NULL)` return?"
    explanation: |-
      `id IN (2, NULL)` means `id = 2 OR id = NULL`.

      | id | `id = 2` | `id = NULL` | OR |
      |---|---|---|---|
      | 1 | FALSE | UNKNOWN | UNKNOWN |
      | 2 | TRUE | UNKNOWN | TRUE |
      | 3 | FALSE | UNKNOWN | UNKNOWN |

      `OR` is TRUE as soon as one operand is TRUE, so the match for 2 survives. Rows 1 and 3 are UNKNOWN and filtered out. The NULL can never *produce* a match, but it cannot hide one either.

      "No rows" overgeneralizes the `NOT IN` trap: there the NULL poisons the `AND`; here it sits harmlessly inside an `OR`.

      "`1`, `3`" is the `NOT IN` answer people expect, not `IN`.

      "`1`, `2`, `3`" treats UNKNOWN as TRUE.
    options:
      - text: "No rows, because the list contains NULL"
      - text: "`1`, `3`"
      - text: "`2`"
        correct: true
      - text: "`1`, `2`, `3`"
  - id: "fixing-not-in"
    type: "multiple"
    prompt: "Same data (`blocked` holds `2` and `NULL`). Which rewrites return `1` and `3`? Select all that apply."
    explanation: |-
      The fix is to make sure no NULL takes part in the inequality chain, or to use a predicate whose semantics do not involve it.

      - Filtering `customer_id IS NOT NULL` leaves the list (2): `1 <> 2` TRUE, `2 <> 2` FALSE, `3 <> 2` TRUE: rows 1 and 3.
      - `NOT EXISTS` never compares with the NULL as a value: rows 1 and 3.

      The distractors are all the same predicate in different clothes:

      - `<> ALL (subquery)` is the standard definition of `NOT IN`; it is UNKNOWN for 1 and 3: **no rows**.
      - `NOT (id IN (...))` is literally how `NOT IN` is defined: **no rows**.
      - `... OR id IS NULL` adds rows whose *outer* `id` is NULL; there are none, and the inner NULL still poisons the rest: **no rows**.
    options:
      - text: "`WHERE id NOT IN (SELECT customer_id FROM blocked WHERE customer_id IS NOT NULL)`"
        correct: true
      - text: "`WHERE NOT EXISTS (SELECT 1 FROM blocked b WHERE b.customer_id = customers.id)`"
        correct: true
      - text: "`WHERE id <> ALL (SELECT customer_id FROM blocked)`"
      - text: "`WHERE NOT (id IN (SELECT customer_id FROM blocked))`"
      - text: "`WHERE id NOT IN (SELECT customer_id FROM blocked) OR id IS NULL`"
  - id: "null-on-outer-side"
    type: "single"
    prompt: "`a(x)` holds `1`, `NULL`, `5`. `b(y)` holds `1`, `2` (no NULLs). How many rows does each query return?\n\n```sql\n-- Q1\nSELECT x FROM a WHERE x NOT IN (SELECT y FROM b);\n-- Q2\nSELECT x FROM a WHERE NOT EXISTS (SELECT 1 FROM b WHERE b.y = a.x);\n```"
    explanation: |-
      The two predicates also differ when the NULL is on the *outer* side.

      Q1, `NOT IN`: for `x = NULL`, `NULL <> 1 AND NULL <> 2` is UNKNOWN, so the NULL row is dropped. `1` is FALSE (it is in b). `5` is TRUE. **1 row** (5).

      Q2, `NOT EXISTS`: for `x = NULL`, the subquery's `b.y = NULL` is never TRUE, so the subquery is empty and `NOT EXISTS` is TRUE: the NULL row is kept. `1` has a match (dropped), `5` has none (kept). **2 rows** (NULL, 5).

      "2 and 2" assumes `NOT IN` keeps the outer NULL.

      "1 and 1" assumes `NOT EXISTS` drops it.

      "0 and 2" applies the inner-NULL trap, but `b` contains no NULL.

      Neither answer is wrong in itself; they answer different questions. Decide whether "unknown x" should count as "not blocked", and pick the predicate that says so.
    options:
      - text: "Q1: 1 row; Q2: 2 rows"
        correct: true
      - text: "Q1: 2 rows; Q2: 2 rows"
      - text: "Q1: 1 row; Q2: 1 row"
      - text: "Q1: 0 rows; Q2: 2 rows"
  - id: "not-in-empty-subquery"
    type: "single"
    prompt: "`a(x)` holds `1` and `NULL`. `b(y)` is empty. How many rows does `SELECT x FROM a WHERE x NOT IN (SELECT y FROM b)` return?"
    explanation: |-
      `x NOT IN (empty)` is `x <> ALL (empty)`, and a universally quantified comparison over an empty set is TRUE (there is no element to violate it). No comparison is ever evaluated, so the NULL in `x` never gets the chance to produce UNKNOWN.

      | x | comparisons performed | result |
      |---|---|---|
      | 1 | none | TRUE |
      | NULL | none | TRUE |

      Result: 2 rows, including the NULL.

      "1" assumes the NULL row must be UNKNOWN; it would be if `b` had any row.

      "0" treats an empty subquery as if it were NULL.

      "An error" confuses `IN (subquery)` with a scalar subquery, which must return at most one row but may return none.
    options:
      - text: "1"
      - text: "2"
        correct: true
      - text: "0"
      - text: "An error, because the subquery returns no rows"
---
## Learning objectives

- Expand `IN` and `NOT IN` into chains of `=`/`<>` connected by `OR`/`AND`.
- Explain why one NULL in a `NOT IN` subquery empties the result.
- Choose between `NOT IN` and `NOT EXISTS` deliberately, including when the outer value is NULL.
- Predict `NOT IN` over an empty subquery.

## The invariant

ISO SQL defines the quantified predicates in terms of comparisons:

| Predicate | Definition | Over values v1 … vn |
|---|---|---|
| `x IN (S)` | `x = ANY (S)` | `x = v1 OR … OR x = vn` |
| `x NOT IN (S)` | `NOT (x IN (S))` = `x <> ALL (S)` | `x <> v1 AND … AND x <> vn` |
| `EXISTS (S)` | S has at least one row | TRUE or FALSE, never UNKNOWN |

Apply the three-valued truth tables from the previous section:

- A NULL among the v's makes one conjunct of `NOT IN` UNKNOWN. An `AND` chain with one UNKNOWN conjunct can be FALSE or UNKNOWN, but never TRUE. So **`NOT IN` returns no rows as soon as the subquery yields a NULL**, apart from outer values that are not compared at all.
- The same NULL inside `IN` sits in an `OR` chain, where a TRUE elsewhere still wins, so `IN` keeps finding matches.
- If the outer `x` is NULL, every comparison is UNKNOWN; `NOT IN` drops the row (unless S is empty).
- Over an empty S, `ALL` is vacuously TRUE and `ANY` is FALSE, so `x NOT IN (empty)` is TRUE even for NULL `x`.

`NOT EXISTS` with a correlated equality behaves differently because it never treats the NULL as a candidate value: an inner row whose comparison is UNKNOWN just fails to match. This is why `NOT EXISTS` is the anti-join most people actually mean.

## Worked example

`customers(id)`: 1, 2, 3. `blocked(customer_id)`: 2, NULL.

```sql
-- No rows: 1 <> 2 AND 1 <> NULL is UNKNOWN, and similarly for 3
SELECT id FROM customers
WHERE id NOT IN (SELECT customer_id FROM blocked);

-- 1, 3
SELECT c.id FROM customers c
WHERE NOT EXISTS (SELECT 1 FROM blocked b WHERE b.customer_id = c.id);

-- 1, 3: remove the NULLs explicitly
SELECT id FROM customers
WHERE id NOT IN (SELECT customer_id FROM blocked WHERE customer_id IS NOT NULL);
```

The bug is dangerous because it appears only when a NULL arrives in the subquery's column, often long after the query was written and tested, and because an empty result rarely triggers an alert.

## Where this shows up today

Because `NOT IN` must respect these NULL rules, an optimizer can turn it into a plain anti-join only when it can prove both sides non-nullable. As of November 2026 this difference is visible in plans: on PostgreSQL 16.15 (the build used for this book's checks), `NOT IN` over a nullable column runs as a hashed subplan filter, while the equivalent `NOT EXISTS` runs as a hash anti join. Linters and code-review rules that flag `NOT IN (SELECT ...)` exist precisely for this trap.

## Pitfalls checklist

- Using `NOT IN (SELECT col ...)` on a nullable column.
- "Fixing" it with `<> ALL` or `NOT (... IN ...)`, which are the same predicate.
- Testing only with data that has no NULLs in the subquery column.
- Forgetting that a NULL outer value is dropped by `NOT IN` but kept by `NOT EXISTS`.
- Replacing NULLs with a sentinel (`COALESCE(col, 0)`) that might collide with a real key.

## Key takeaways

- `NOT IN` is an `AND` of `<>` comparisons; one NULL makes it never TRUE.
- `IN` is an `OR` of `=` comparisons; a NULL cannot hide a real match.
- `NOT EXISTS` is never UNKNOWN and is the safe default for anti-joins.
- `NOT IN` over an empty subquery is TRUE for every row, including NULLs.

## Sources

- ISO/IEC 9075-2 (SQL/Foundation), `<in predicate>`, `<quantified comparison predicate>`, `<exists predicate>`.
- PostgreSQL 19 documentation, "Subquery Expressions" (`NOT IN` and NULL).
