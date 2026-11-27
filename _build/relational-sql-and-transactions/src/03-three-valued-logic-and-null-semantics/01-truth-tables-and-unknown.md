---
title: "TRUE, FALSE and UNKNOWN"
questions:
  - id: "unknown-and-false"
    type: "single"
    prompt: "A row has `x = NULL` and `y = 2`. What is the truth value of `(x > 5) AND (y = 1)`?"
    correct:
      - "FALSE"
    distractors:
      - "UNKNOWN"
      - "TRUE"
      - "NULL, which is a fourth truth value"
    explanation: |-
      Evaluate each comparison, then combine with the three-valued `AND` table.

      - `x > 5` with `x` NULL: any comparison with NULL is UNKNOWN.
      - `y = 1` with `y = 2`: FALSE.
      - `UNKNOWN AND FALSE`: FALSE, because `AND` is FALSE as soon as either operand is FALSE, whatever the other is.

      | AND | TRUE | FALSE | UNKNOWN |
      |---|---|---|---|
      | TRUE | TRUE | FALSE | UNKNOWN |
      | FALSE | FALSE | FALSE | FALSE |
      | UNKNOWN | UNKNOWN | FALSE | UNKNOWN |

      "UNKNOWN" assumes NULL contaminates every expression it touches; that is true for arithmetic and comparisons, not for `AND` and `OR`, whose result can be decided by the other operand.

      "TRUE" is impossible with a FALSE conjunct.

      "A fourth truth value" confuses the NULL *value* with the UNKNOWN *truth value*; SQL's logic has exactly three truth values (a NULL of type BOOLEAN represents UNKNOWN).
    checks:
      - engine: "both"
        setup: "CREATE TABLE t(x INT, y INT); INSERT INTO t VALUES (NULL, 2);"
        query: "SELECT CASE WHEN (x > 5) AND (y = 1) THEN 'T' WHEN NOT ((x > 5) AND (y = 1)) THEN 'F' ELSE 'U' END FROM t"
        expect: [["F"]]
  - id: "which-are-unknown"
    type: "multiple"
    prompt: "Which of these expressions evaluate to UNKNOWN? Select all that apply."
    correct:
      - "`NULL = NULL`"
      - "`NOT (NULL = 1)`"
      - "`NULL <> NULL`"
    distractors:
      - "`(NULL = 1) OR TRUE`"
      - "`(NULL = 1) AND FALSE`"
    explanation: |-
      ISO SQL treats NULL as "value unknown or absent", so any comparison involving NULL yields UNKNOWN, even comparing NULL with NULL: two unknown values are not known to be equal or unequal.

      | Expression | Steps | Result |
      |---|---|---|
      | `NULL = NULL` | comparison with NULL | UNKNOWN |
      | `NULL <> NULL` | comparison with NULL | UNKNOWN |
      | `NOT (NULL = 1)` | NOT UNKNOWN | UNKNOWN |
      | `(NULL = 1) OR TRUE` | UNKNOWN OR TRUE | TRUE |
      | `(NULL = 1) AND FALSE` | UNKNOWN AND FALSE | FALSE |

      `NOT` maps TRUE↔FALSE and leaves UNKNOWN unchanged. `OR` is TRUE if either operand is TRUE; `AND` is FALSE if either operand is FALSE. To compare NULL-safely, use `IS NULL` or `IS [NOT] DISTINCT FROM`.
    checks:
      - engine: "postgres"
        query: "SELECT (NULL::int = NULL::int) IS UNKNOWN, (NOT (NULL::int = 1)) IS UNKNOWN, (NULL::int <> NULL::int) IS UNKNOWN, ((NULL::int = 1) OR TRUE) IS TRUE, ((NULL::int = 1) AND FALSE) IS FALSE"
        expect: [[true, true, true, true, true]]
  - id: "not-equal-drops-null"
    type: "single"
    prompt: "Column `x` holds `5`, `7`, `NULL`, `NULL`, `3`. How many rows does `SELECT * FROM t WHERE x <> 5` return?"
    correct:
      - "2"
    distractors:
      - "4"
      - "3"
      - "5"
    explanation: |-
      `WHERE` keeps a row only when its condition is TRUE; FALSE and UNKNOWN rows are both discarded.

      | x | `x <> 5` | kept? |
      |---|---|---|
      | 5 | FALSE | no |
      | 7 | TRUE | yes |
      | NULL | UNKNOWN | no |
      | NULL | UNKNOWN | no |
      | 3 | TRUE | yes |

      Result: 2 rows (7 and 3).

      "4" is the intuitive answer "everything except the 5", which treats NULL as "some value other than 5".

      "3" keeps one NULL row, which no rule does.

      "5" ignores the filter.

      To include the NULLs, say so: `WHERE x <> 5 OR x IS NULL`, or `WHERE x IS DISTINCT FROM 5`.
    checks:
      - engine: "both"
        setup: "CREATE TABLE t(x INT); INSERT INTO t VALUES (5),(7),(NULL),(NULL),(3);"
        query: "SELECT COUNT(*) FROM t WHERE x <> 5"
        expect: [[2]]
  - id: "check-accepts-unknown"
    type: "single"
    prompt: "A table declares `qty INT CHECK (qty > 0)` and no `NOT NULL`. What happens to `INSERT INTO t (qty) VALUES (NULL)`?"
    correct:
      - "It succeeds, because a `CHECK` constraint is violated only when its condition is FALSE"
    distractors:
      - "It fails, because `NULL > 0` is not TRUE"
      - "It succeeds only if the engine is configured to ignore constraints on NULL"
      - "It stores `0` instead, because NULL is converted to satisfy the type"
    explanation: |-
      SQL applies different rules to queries and constraints. A `WHERE` clause keeps rows whose condition is TRUE. A `CHECK` constraint *rejects* rows whose condition is FALSE. UNKNOWN therefore passes a `CHECK`: `NULL > 0` is UNKNOWN, which is not FALSE, so the row is accepted.

      "It fails, because `NULL > 0` is not TRUE" applies the `WHERE` rule to constraints, the most common misconception here.

      "Configured to ignore constraints" invents a setting; this is the standard rule in every conforming engine.

      "Stores 0" invents a conversion; NULL is stored as NULL.

      If NULL must be rejected, declare `NOT NULL` as well, or write `CHECK (qty IS NOT NULL AND qty > 0)`.
    checks:
      - engine: "both"
        setup: "CREATE TABLE t(qty INT CHECK (qty > 0)); INSERT INTO t VALUES (NULL);"
        query: "SELECT COUNT(*) FROM t WHERE qty IS NULL"
        expect: [[1]]
  - id: "case-unknown-goes-to-else"
    type: "single"
    prompt: "Column `x` holds `1`, `2`, `NULL`. What does this return, in row order?\n\n```sql\nSELECT CASE\n         WHEN x = 1 THEN 'one'\n         WHEN NOT (x = 1) THEN 'other'\n         ELSE 'unknown'\n       END\nFROM t ORDER BY x NULLS LAST;\n```"
    correct:
      - "`one`, `other`, `unknown`"
    distractors:
      - "`one`, `other`, `other`"
      - "`one`, `other`, NULL"
      - "`one`, `other`, and an error for the NULL row"
    explanation: |-
      A searched `CASE` returns the result of the first `WHEN` whose condition is TRUE. UNKNOWN is not TRUE, so it falls through exactly like FALSE.

      | x | `x = 1` | `NOT (x = 1)` | result |
      |---|---|---|---|
      | 1 | TRUE | | one |
      | 2 | FALSE | TRUE | other |
      | NULL | UNKNOWN | UNKNOWN | ELSE: unknown |

      For the NULL row, neither the condition nor its negation is TRUE, which is the signature of three-valued logic: "A or not A" does not cover every row.

      "`other`" assumes `NOT (x = 1)` is TRUE whenever `x = 1` is not TRUE.

      "NULL" would be the result if there were no `ELSE`; here there is one.

      "An error" is wrong; UNKNOWN conditions are normal.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE t(x INT); INSERT INTO t VALUES (1),(2),(NULL);"
        query: "SELECT CASE WHEN x = 1 THEN 'one' WHEN NOT (x = 1) THEN 'other' ELSE 'unknown' END FROM t ORDER BY x NULLS LAST"
        expect: [["one"], ["other"], ["unknown"]]
        ordered: true
  - id: "excluded-middle-fails"
    type: "single"
    prompt: "Column `x` holds `5`, `7`, `NULL`, `NULL`, `3`. How many rows does `SELECT * FROM t WHERE x = 5 OR x <> 5` return?"
    correct:
      - "3"
    distractors:
      - "5"
      - "1"
      - "0"
    explanation: |-
      In two-valued logic, "x = 5 OR x <> 5" is a tautology. In SQL's three-valued logic it is not: for a NULL, both disjuncts are UNKNOWN, and `UNKNOWN OR UNKNOWN` is UNKNOWN.

      | x | `x = 5` | `x <> 5` | OR | kept? |
      |---|---|---|---|---|
      | 5 | TRUE | FALSE | TRUE | yes |
      | 7 | FALSE | TRUE | TRUE | yes |
      | NULL | UNKNOWN | UNKNOWN | UNKNOWN | no |
      | NULL | UNKNOWN | UNKNOWN | UNKNOWN | no |
      | 3 | FALSE | TRUE | TRUE | yes |

      Result: 3 rows.

      "5" applies two-valued logic.

      "1" keeps only the row matching `x = 5`.

      "0" assumes any NULL in the column empties the result; NULLs only affect their own rows here.

      This is why optimizers may not simplify `p OR NOT p` to TRUE unless they know the operands are not nullable.
    checks:
      - engine: "both"
        setup: "CREATE TABLE t(x INT); INSERT INTO t VALUES (5),(7),(NULL),(NULL),(3);"
        query: "SELECT COUNT(*) FROM t WHERE x = 5 OR x <> 5"
        expect: [[3]]
body_checks:
  - engine: "postgres"
    setup: "CREATE TABLE t(x INT); INSERT INTO t VALUES (5),(7),(NULL),(NULL),(3);"
    query: "SELECT (SELECT COUNT(*) FROM t WHERE NOT (x = 5)), (SELECT COUNT(*) FROM t WHERE x <> 5 OR x IS NULL), (SELECT COUNT(*) FROM t WHERE (x = 5) IS NOT TRUE)"
    expect: [[2, 4, 4]]
  - engine: "both"
    setup: "CREATE TABLE t(qty INT CHECK (qty > 0));"
    query: "INSERT INTO t VALUES (0)"
    expect_error: "check"
---
## Learning objectives

- Evaluate `AND`, `OR` and `NOT` over TRUE, FALSE and UNKNOWN.
- Predict which rows `WHERE` keeps when comparisons involve NULL.
- Explain why `CHECK` constraints accept UNKNOWN while `WHERE` rejects it.
- Recognize two-valued tautologies that fail in SQL.

## The invariant

SQL represents a missing or inapplicable value with NULL. Codd proposed in the 1970s that comparisons involving such a "null value" should be neither true nor false, and ISO SQL adopted a three-valued logic: every predicate evaluates to **TRUE**, **FALSE** or **UNKNOWN**.

Comparisons (`=`, `<>`, `<`, `>`, `LIKE`, `IN` against a NULL, and so on) yield UNKNOWN when either operand is NULL. The connectives follow Kleene's strong three-valued logic:

| p | q | p AND q | p OR q | NOT p |
|---|---|---|---|---|
| TRUE | UNKNOWN | UNKNOWN | TRUE | FALSE |
| FALSE | UNKNOWN | FALSE | UNKNOWN | TRUE |
| UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

(The rows with no UNKNOWN operand are the ordinary two-valued ones.) A handy reading: treat UNKNOWN as "could be either"; the result is decided only if both possibilities give the same answer.

Where truth values are *consumed*, SQL uses two different rules:

| Context | Row or value accepted when the condition is |
|---|---|
| `WHERE`, `HAVING`, `ON`, `CASE WHEN`, the `FILTER` clause | TRUE only |
| `CHECK` constraints | TRUE or UNKNOWN (rejected only when FALSE) |

The standard also offers tests that never return UNKNOWN: `IS NULL`, `IS NOT NULL`, and `IS [NOT] TRUE | FALSE | UNKNOWN`.

Two familiar laws of two-valued logic fail. The law of excluded middle (`p OR NOT p` is always TRUE) fails when p is UNKNOWN. And "`x <> 5` is the complement of `x = 5`" fails: neither keeps NULL rows.

## Worked example

`t(x)` with `5`, `7`, `NULL`, `NULL`, `3`:

| Query | Rows |
|---|---|
| `WHERE x = 5` | 5 |
| `WHERE x <> 5` | 7, 3 |
| `WHERE NOT (x = 5)` | 7, 3 |
| `WHERE x = 5 OR x <> 5` | 5, 7, 3 |
| `WHERE x <> 5 OR x IS NULL` | 7, NULL, NULL, 3 |
| `WHERE (x = 5) IS NOT TRUE` | 7, NULL, NULL, 3 |

The first three show that `=` and `<>` together miss the NULL rows. The last two show the two ways to ask for "everything except 5" explicitly.

Constraints flip the rule:

```sql
CREATE TABLE t (qty INT CHECK (qty > 0));
INSERT INTO t VALUES (NULL);   -- accepted: NULL > 0 is UNKNOWN, not FALSE
INSERT INTO t VALUES (0);      -- rejected: 0 > 0 is FALSE
```

## Where this shows up today

As of November 2026, PostgreSQL 19 and MySQL 8.4 document the standard `IS [NOT] TRUE`, `IS [NOT] FALSE` and `IS [NOT] UNKNOWN` tests; SQLite (3.53.1 in this book's checks) accepts `IS TRUE` and `IS FALSE` but not `IS UNKNOWN`, for which `IS NULL` serves. Application code feels the logic too: an ORM translating `!(x == 5)` into `NOT (x = 5)` silently drops rows where `x` is NULL unless it adds an `OR x IS NULL`, and some ORMs do exactly that for nullable columns.

## Pitfalls checklist

- Writing `x <> value` to mean "anything else" on a nullable column.
- Expecting `CHECK (col > 0)` to reject NULL; add `NOT NULL`.
- Writing `x = NULL` instead of `x IS NULL`; it is UNKNOWN for every row.
- Assuming a `CASE` with complementary `WHEN` branches covers every row; NULL reaches `ELSE`.
- Letting an optimizer or a code reviewer simplify `p OR NOT p` to TRUE on nullable data.

## Key takeaways

- Comparisons with NULL are UNKNOWN; `AND`/`OR` can still decide when the other side is FALSE/TRUE.
- `WHERE` keeps only TRUE; `CHECK` rejects only FALSE.
- `NOT UNKNOWN` is UNKNOWN, so negating a filter does not return the rows it excluded.
- Use `IS NULL`, `IS DISTINCT FROM` or `IS NOT TRUE` when NULLs must be handled explicitly.

## Sources

- E. F. Codd, "Extending the Database Relational Model to Capture More Meaning", *ACM TODS* 4(4), 1979.
- ISO/IEC 9075-2 (SQL/Foundation), `<boolean value expression>` and `<check constraint definition>`.
- PostgreSQL 19 documentation, "Comparison Functions and Operators"; MySQL 8.4 Reference Manual, "Comparison Functions and Operators"; SQLite documentation, "SQL Language Expressions".
