---
title: "Outer Joins: WHERE Versus ON"
questions:
  - id: "on-filter-keeps-left-rows"
    type: "single"
    prompt: "`customers(id, name)`: `(1, 'ann')`, `(2, 'bob')`, `(3, 'cy')`. `orders(id, customer_id, status)`: `(10, 1, 'paid')`, `(11, 1, 'open')`, `(12, 2, 'open')`. How many rows does this return?\n\n```sql\nSELECT c.name, o.id\nFROM customers c\nLEFT JOIN orders o\n  ON o.customer_id = c.id AND o.status = 'paid';\n```"
    explanation: |-
      For a `LEFT JOIN`, the whole `ON` condition decides which right rows *match*; a left row with no match is still output once, padded with NULLs. The status test is part of matching, so it cannot remove customers.

      | customer | right rows satisfying the ON condition | output |
      |---|---|---|
      | ann | order 10 (paid) | (ann, 10) |
      | bob | none (order 12 is open) | (bob, NULL) |
      | cy | none | (cy, NULL) |

      Total: 3 rows.

      "1" is the result when the status test sits in `WHERE` instead, which removes NULL-extended rows.

      "4" is the row count of the plain join on `customer_id` without the status test (ann twice, bob, cy).

      "2" drops `cy` only, as if customers without any orders were removed but customers with non-matching orders kept.
    options:
      - text: "3"
        correct: true
      - text: "1"
      - text: "4"
      - text: "2"
  - id: "where-filter-nullifies-outer-join"
    type: "single"
    prompt: "Same data as before. A developer moves the status test into `WHERE`:\n\n```sql\nSELECT c.name, o.id\nFROM customers c\nLEFT JOIN orders o ON o.customer_id = c.id\nWHERE o.status = 'paid';\n```\n\nWhat does it return?"
    explanation: |-
      `FROM ... LEFT JOIN ... ON` (step 1) runs first and produces:

      | name | o.id | o.status |
      |---|---|---|
      | ann | 10 | paid |
      | ann | 11 | open |
      | bob | 12 | open |
      | cy | NULL | NULL |

      `WHERE o.status = 'paid'` (step 2) keeps rows where the predicate is TRUE. For `cy`, `NULL = 'paid'` is UNKNOWN, so the NULL-extended row is discarded along with the open orders. Only `(ann, 10)` survives. A `WHERE` predicate that rejects NULLs on the inner side turns the outer join into an inner join; optimizers perform exactly this rewrite.

      "Three rows" is what the query would return with the test in `ON`.

      "Two rows ... `(cy, NULL)`" assumes NULL rows pass a comparison; under three-valued logic they do not.

      "Four rows" ignores `WHERE` entirely.
    options:
      - text: "Three rows: `(ann, 10)`, `(bob, NULL)`, `(cy, NULL)`"
      - text: "One row: `(ann, 10)`"
        correct: true
      - text: "Two rows: `(ann, 10)`, `(cy, NULL)`"
      - text: "Four rows, because the `LEFT JOIN` keeps every customer"
  - id: "anti-join-no-paid-orders"
    type: "multiple"
    prompt: "Same data. Which queries return exactly the customers with no *paid* order (`bob` and `cy`)? Select all that apply."
    explanation: |-
      An anti-join keeps left rows that have *no* qualifying right row. With an outer join, every qualifying condition must go into `ON`, and `WHERE o.id IS NULL` (testing a non-nullable column of the right table) then keeps exactly the unmatched left rows.

      Trace of the correct outer-join form: matches are ann–10 only, so bob and cy are NULL-extended; `o.id IS NULL` keeps bob and cy.

      `NOT EXISTS` states the same thing directly: for bob and cy no paid order exists.

      Distractor traces (the plain join on `customer_id` gives ann–10, ann–11, bob–12, cy–NULL):

      - `WHERE o.id IS NULL AND o.status = 'paid'`: only cy has `o.id IS NULL`, and its status is NULL, so `NULL = 'paid'` is UNKNOWN: **no rows**.
      - Inner `JOIN ... WHERE o.status <> 'paid'`: ann–11 and bob–12 survive: **ann, bob**. It drops cy (no orders) and wrongly includes ann.
      - `WHERE o.status <> 'paid' OR o.id IS NULL`: ann–11, bob–12, cy–NULL: **ann, bob, cy**. It tests orders one at a time, so ann's open order qualifies her even though she also has a paid one.
    options:
      - text: "`LEFT JOIN orders o ON o.customer_id = c.id AND o.status = 'paid' WHERE o.id IS NULL`"
        correct: true
      - text: "`WHERE NOT EXISTS (SELECT 1 FROM orders o WHERE o.customer_id = c.id AND o.status = 'paid')`"
        correct: true
      - text: "`LEFT JOIN orders o ON o.customer_id = c.id WHERE o.id IS NULL AND o.status = 'paid'`"
      - text: "`JOIN orders o ON o.customer_id = c.id WHERE o.status <> 'paid'`"
      - text: "`LEFT JOIN orders o ON o.customer_id = c.id WHERE o.status <> 'paid' OR o.id IS NULL`"
  - id: "on-filter-on-preserved-side"
    type: "single"
    prompt: "Same data. How many rows does this return?\n\n```sql\nSELECT c.name, o.id\nFROM customers c\nLEFT JOIN orders o\n  ON o.customer_id = c.id AND c.name = 'ann';\n```"
    explanation: |-
      A condition on the *preserved* (left) table inside `ON` does not filter left rows; it only controls whether they find matches. Every customer still appears at least once.

      | customer | ON true for | output rows |
      |---|---|---|
      | ann | orders 10, 11 | (ann, 10), (ann, 11) |
      | bob | none (name is not ann) | (bob, NULL) |
      | cy | none | (cy, NULL) |

      Total: 4.

      "2" treats `c.name = 'ann'` as a filter on customers, which is what it would do in `WHERE`.

      "3" assumes one row per customer, forgetting that ann matches two orders.

      "1" combines both mistakes.

      Rule of thumb: in an outer join, put conditions on the preserved table in `WHERE` and conditions on the optional table in `ON`.
    options:
      - text: "2"
      - text: "3"
      - text: "1"
      - text: "4"
        correct: true
  - id: "inner-join-after-left-join"
    type: "single"
    prompt: "Same customers and orders, plus `payments(order_id, amount)` containing `(10, 50)`. A report should list every customer with their payments, if any:\n\n```sql\nSELECT c.name, p.amount\nFROM customers c\nLEFT JOIN orders o ON o.customer_id = c.id\nJOIN payments p ON p.order_id = o.id;\n```\n\nWhat does it return?"
    explanation: |-
      Joins are evaluated left to right unless parentheses say otherwise. The `LEFT JOIN` produces ann–10, ann–11, bob–12, cy–NULL. The following *inner* `JOIN` keeps only rows whose `o.id` matches a payment:

      | row | o.id | payment match? |
      |---|---|---|
      | ann | 10 | yes (50) |
      | ann | 11 | no |
      | bob | 12 | no |
      | cy | NULL | `NULL = 10` is UNKNOWN: no |

      Result: `(ann, 50)`. The inner join silently discards the NULL-extended rows that the `LEFT JOIN` worked to preserve.

      "Three rows" is the intended report; it needs `LEFT JOIN payments`, or the inner join nested inside the optional side: `LEFT JOIN (orders o JOIN payments p ON p.order_id = o.id) ON o.customer_id = c.id`.

      "Four rows" ignores the second join.

      "`(ann, NULL)`" assumes the inner join pads unmatched rows, which only outer joins do.

      "`(cy, NULL)`" assumes that a customer with no orders at all escapes the inner join; its NULL `o.id` fails the join condition like every other non-match.
    options:
      - text: "Three rows: `(ann, 50)`, `(bob, NULL)`, `(cy, NULL)`"
      - text: "Four rows, one per row of the `LEFT JOIN`"
      - text: "One row: `(ann, 50)`"
        correct: true
      - text: "Two rows: `(ann, 50)`, `(ann, NULL)`"
      - text: "Two rows: `(ann, 50)`, `(cy, NULL)`"
  - id: "full-outer-join-count"
    type: "single"
    prompt: "`a(k)` holds `1`, `2`, `3`. `b(k)` holds `2`, `3`, `4`, `4`. How many rows does `SELECT * FROM a FULL OUTER JOIN b ON a.k = b.k` return?"
    explanation: |-
      A full outer join returns the inner-join rows, plus each unmatched left row padded with NULLs, plus each unmatched right row padded with NULLs.

      | part | rows | count |
      |---|---|---|
      | matched | (2, 2), (3, 3) | 2 |
      | left only | (1, NULL) | 1 |
      | right only | (NULL, 4), (NULL, 4) | 2 |

      Total: 5.

      "4" counts the distinct key values 1–4, forgetting that `b` contains 4 twice and each copy is a separate unmatched row.

      "7" adds the table sizes (3 + 4), counting matched pairs twice.

      "6" counts each matched pair twice but each unmatched value once.

      "3" counts only the rows of `a`, as if the full join were a left join with at most one match per row.
    options:
      - text: "4"
      - text: "7"
      - text: "6"
      - text: "5"
        correct: true
      - text: "3"
---
## Learning objectives

- Explain why a condition in `ON` and the same condition in `WHERE` give different results for outer joins.
- Write an anti-join ("rows with no match") correctly with `LEFT JOIN ... IS NULL` or `NOT EXISTS`.
- Predict how an inner join placed after an outer join changes the result.
- Count the rows of a `FULL OUTER JOIN`.

## The invariant

An outer join is defined in two parts. ISO SQL specifies `A LEFT JOIN B ON p` as: the inner join of A and B on `p`, plus every row of A that has no B row for which `p` is TRUE, extended with NULLs for B's columns. A is the **preserved** table, B the **null-supplying** table. `RIGHT JOIN` swaps the roles; `FULL JOIN` preserves both.

Two consequences follow, both direct from the logical order:

1. **`ON` decides matching; it never removes preserved rows.** A condition in `ON` on either table only affects which B rows count as matches. A row of A with no match still appears once.
2. **`WHERE` runs after the join is complete.** It sees NULL-extended rows like any other. A `WHERE` predicate on B's columns that cannot be TRUE when they are NULL (such as `b.status = 'paid'`) removes every NULL-extended row, so the outer join behaves as an inner join. Such predicates are called *null-rejecting*, and optimizers rewrite the join to an inner join when they see one.

Hence the placement rule: conditions that define *which B rows qualify* go in `ON`; conditions that define *which result rows you want* go in `WHERE`. For inner joins the two placements are equivalent, which is why the habit of "putting everything in `WHERE`" works until the join becomes outer.

An anti-join tests for absence. With an outer join, put every qualifying condition in `ON` and test a column of B that is never NULL in real rows (its primary key) with `IS NULL` in `WHERE`.

## Worked example

`customers`: ann (1), bob (2), cy (3). `orders`: 10 (ann, paid), 11 (ann, open), 12 (bob, open).

```sql
-- A: status test in ON  -> ann 10, bob NULL, cy NULL      (3 rows)
SELECT c.name, o.id
FROM customers c
LEFT JOIN orders o ON o.customer_id = c.id AND o.status = 'paid';

-- B: status test in WHERE -> ann 10                       (1 row)
SELECT c.name, o.id
FROM customers c
LEFT JOIN orders o ON o.customer_id = c.id
WHERE o.status = 'paid';

-- C: anti-join, customers without a paid order -> bob, cy
SELECT c.name
FROM customers c
LEFT JOIN orders o ON o.customer_id = c.id AND o.status = 'paid'
WHERE o.id IS NULL;
```

Trace for B: the join produces ann–10, ann–11, bob–12 and cy–NULL. For cy, `NULL = 'paid'` is UNKNOWN, and `WHERE` keeps only TRUE rows, so cy is gone along with the open orders.

Chained joins follow the same logic. `customers LEFT JOIN orders ... JOIN payments ...` is `(customers LEFT JOIN orders) JOIN payments`: the second, inner join removes every customer without a paid order. Keep it optional with `LEFT JOIN payments`, or nest the inner join on the optional side with parentheses.

## Where this shows up today

As of November 2026, SQLite supports `RIGHT` and `FULL OUTER JOIN` (added in SQLite 3.39.0, 2022), while the MySQL 8.4 `JOIN` grammar still has no `FULL OUTER JOIN`; there it is emulated with a `LEFT JOIN` united with the unmatched rows of a `RIGHT JOIN`. ORMs that generate "include optional children" queries emit `LEFT JOIN`s, and adding a filter on a child property through the ORM's `Where` often lands in SQL `WHERE`, producing exactly the inner-join collapse shown above.

## Pitfalls checklist

- Filtering the optional table in `WHERE` and losing the rows the outer join was meant to keep.
- Filtering the preserved table in `ON` and expecting rows to disappear.
- Testing a nullable column with `IS NULL` in an anti-join; use the right table's key.
- Adding an inner join after an outer join and silently dropping unmatched rows.
- Counting `FULL OUTER JOIN` rows by distinct keys instead of by rows.

## Key takeaways

- `ON` decides what matches; `WHERE` filters the finished join.
- A null-rejecting `WHERE` predicate on the optional side turns an outer join into an inner join.
- Anti-joins need all qualifying conditions in `ON` plus a key `IS NULL` test, or `NOT EXISTS`.
- Join order matters once outer joins are involved; use parentheses to say what you mean.

## Sources

- ISO/IEC 9075-2 (SQL/Foundation), `<joined table>` semantics.
- C. Galindo-Legaria and A. Rosenthal, "Outerjoin Simplification and Reordering for Query Optimization", *ACM TODS* 22(1), 1997.
- SQLite release log 3.39.0 (2022-06-25); MySQL 8.4 Reference Manual, "JOIN Clause".
