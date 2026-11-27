---
title: "Lossless Decomposition and Deliberate Denormalization"
questions:
  - id: "spurious-tuples-count"
    type: "single"
    prompt: "`assignment(emp, dept, office)` holds `('ana', 'ops', 'A1')`, `('bo', 'dev', 'A1')`, `('cy', 'dev', 'B2')`. It is split into `e(emp, office)` and `d(dept, office)` (each with `SELECT DISTINCT`). How many rows does `SELECT * FROM e NATURAL JOIN d` return?"
    correct:
      - "5"
    distractors:
      - "3"
      - "4"
      - "6"
    explanation: |-
      The natural join matches on the only common column, `office`.

      | office | emps in `e` | depts in `d` | rows |
      |---|---|---|---|
      | A1 | ana, bo | ops, dev | 2 × 2 = 4 |
      | B2 | cy | dev | 1 × 1 = 1 |

      Total 5 rows, but the original had 3. The two extra rows, `(ana, dev, A1)` and `(bo, ops, A1)`, are **spurious tuples**: the decomposition is lossy because `office` is not a key of either projection, so the join cannot tell which department went with which employee.

      "3" assumes every decomposition rebuilds the original; that holds only for lossless ones.

      "4" counts only the A1 group.

      "6" multiplies 3 employees by 2 distinct departments, treating the join as a cross product rather than a join on `office`.
    checks:
      - engine: "both"
        setup: "CREATE TABLE assignment(emp TEXT, dept TEXT, office TEXT); INSERT INTO assignment VALUES ('ana','ops','A1'),('bo','dev','A1'),('cy','dev','B2'); CREATE TABLE e AS SELECT DISTINCT emp, office FROM assignment; CREATE TABLE d AS SELECT DISTINCT dept, office FROM assignment;"
        query: "SELECT COUNT(*) FROM e NATURAL JOIN d"
        expect: [[5]]
  - id: "lossless-join-test"
    type: "single"
    prompt: "`R(A, B, C)` has exactly one nontrivial FD, `B → C`. Which binary decomposition is guaranteed to be lossless?"
    correct:
      - "`R1(A, B)` and `R2(B, C)`"
    distractors:
      - "`R1(A, C)` and `R2(B, C)`"
      - "`R1(A, B)` and `R2(A, C)`"
      - "`R1(A)` and `R2(B, C)`"
    explanation: |-
      The binary lossless-join test (Heath's theorem and its generalization): splitting R into R1 and R2 is lossless exactly when the common attributes R1 ∩ R2 functionally determine all of R1 or all of R2.

      | Decomposition | R1 ∩ R2 | Determines R1 or R2? |
      |---|---|---|
      | (A, B) and (B, C) | {B} | B → C, so B determines R2 = (B, C): lossless |
      | (A, C) and (B, C) | {C} | C determines nothing: lossy |
      | (A, B) and (A, C) | {A} | A determines nothing under the given FDs: lossy |
      | (A) and (B, C) | {} | the join is a cross product: lossy |

      The distractors share attributes that are not keys of either side, so joining them back can pair rows that never belonged together (the spurious tuples from the previous question).
  - id: "dependency-not-preserved"
    type: "single"
    prompt: "`addresses(street, city, zip)` with `street, city → zip` and `zip → city` is decomposed into BCNF tables `zip_city(zip PRIMARY KEY, city)` and `street_zip(street, zip, PRIMARY KEY (street, zip))`. They contain `zip_city`: `('10001', 'NYC')`, `('10002', 'NYC')` and `street_zip`: `('Main', '10001')`, `('Main', '10002')`. Joining them on `zip`, how many distinct `zip` values belong to `('Main', 'NYC')`?"
    correct:
      - "2, so the joined data violates `street, city → zip` even though both tables satisfy their keys"
    distractors:
      - "1, because the primary key of `street_zip` prevents a second zip"
      - "2, but the insert of the second `street_zip` row would have been rejected"
      - "0, because a lossless decomposition never produces new combinations"
    explanation: |-
      BCNF decomposition here is lossless but not dependency-preserving: no single table contains all of `street, city, zip`, so no key can enforce `street, city → zip`.

      Trace of the join on `zip`:

      | street | zip | city |
      |---|---|---|
      | Main | 10001 | NYC |
      | Main | 10002 | NYC |

      `('Main', 'NYC')` maps to two zips, which the original FD forbids. Each table individually is valid: `zip_city` has unique zips, and `street_zip` has unique `(street, zip)` pairs.

      "1, because of the primary key" misreads the key; `(street, zip)` allows the same street with different zips.

      "The insert would have been rejected" assumes the engine checks cross-table FDs; only an assertion or trigger could.

      "0 ... lossless never produces new combinations" confuses losslessness (the join reconstructs any *legal* original instance exactly) with constraint enforcement; here the data was never a legal instance of the original schema.
    checks:
      - engine: "both"
        setup: "CREATE TABLE zip_city(zip TEXT PRIMARY KEY, city TEXT); CREATE TABLE street_zip(street TEXT, zip TEXT, PRIMARY KEY (street, zip)); INSERT INTO zip_city VALUES ('10001','NYC'),('10002','NYC'); INSERT INTO street_zip VALUES ('Main','10001'),('Main','10002');"
        query: "SELECT COUNT(DISTINCT s.zip) FROM street_zip s JOIN zip_city z ON z.zip = s.zip WHERE s.street = 'Main' AND z.city = 'NYC'"
        expect: [[2]]
  - id: "denormalization-justified"
    type: "multiple"
    prompt: "Which of these are sound reasons to store data that a normalized design would derive by join? Select all that apply."
    correct:
      - "Storing `unit_price` on each order line, because the price at purchase time is a different fact from the product's current price"
      - "Keeping a summary table maintained by the database itself, after measurement shows the join is the bottleneck"
    distractors:
      - "Copying `customer_name` into `orders` because joins are always slower than reading a wide table"
      - "Copying `customer_name` into `orders` and having every service that renames a customer update both tables"
      - "Dropping foreign keys from a reporting schema because normalized tables cannot be indexed efficiently"
    explanation: |-
      Denormalization trades the single-copy guarantee for read speed, so it is justified only when (1) there is a measured need and (2) something reliable keeps the copies consistent, or (3) the "copy" is in fact a different fact.

      - `unit_price` on an order line is not redundancy: `order_line → unit_price_at_sale` is its own FD. Recomputing it from today's product price would be wrong.
      - A summary maintained by the database (a materialized view, an indexed view, or a trigger-maintained table) keeps one authority for updates and is introduced after measurement.

      "Joins are always slower" is a folk rule; key-to-key joins on indexed columns are cheap, and the copy brings an update anomaly with it.

      "Every service updates both tables" is the update anomaly with extra steps: one forgotten code path leaves copies disagreeing.

      "Normalized tables cannot be indexed efficiently" is false; and dropping foreign keys removes integrity checks without speeding up reads.
  - id: "json-array-integrity"
    type: "single"
    prompt: "A team replaces the `article_tags(article_id, tag_id)` junction table with a JSON array column `articles.tag_ids`. Which consequence follows from the relational model, whatever the engine?"
    correct:
      - "The engine can no longer enforce, with a declared foreign key, that every element refers to an existing tag"
    distractors:
      - "SQL can no longer query individual tags inside the array"
      - "The table becomes BCNF automatically, because a JSON value counts as one atomic value"
      - "Deleting a tag now cascades into the arrays automatically"
    explanation: |-
      A foreign key constrains column values, and a JSON array is one column value containing many identifiers. Referential integrity, uniqueness of a tag per article, and cascades therefore move into application code or triggers. That is the deliberate 1NF trade-off: the array is a repeating group hidden inside an attribute.

      "SQL can no longer query individual tags" is false: SQL/JSON functions (standardized since SQL:2016) and engine-specific operators query array elements; some engines can index them.

      "BCNF automatically" misuses atomicity: normal forms are about dependencies among attributes, and hiding a set inside one attribute just hides the dependency from the engine.

      "Cascades automatically" assumes a foreign key that cannot be declared on array elements.
  - id: "synthesis-guarantee"
    type: "single"
    prompt: "Which combination of properties can a schema designer *always* achieve, for any set of functional dependencies?"
    correct:
      - "A 3NF decomposition that is both lossless and dependency-preserving"
    distractors:
      - "A BCNF decomposition that is both lossless and dependency-preserving"
      - "A BCNF decomposition that is dependency-preserving but possibly lossy"
      - "A 2NF decomposition that is lossless only if every key is a single column"
    explanation: |-
      Two classic results bound the options. The 3NF synthesis algorithm (Bernstein 1976) produces, for any set of FDs, a decomposition into 3NF that is lossless and preserves every dependency. BCNF decomposition can always be made lossless, but for some FD sets no BCNF decomposition preserves all dependencies: `street, city → zip` with `zip → city` is the standard counterexample, because any BCNF split separates `street` from `city`.

      "BCNF with both" is exactly what that counterexample rules out.

      "BCNF dependency-preserving but possibly lossy" reverses the priorities: losslessness is the non-negotiable property (a lossy decomposition invents data) and is always achievable.

      The 2NF option invents a restriction; losslessness depends on common attributes being keys of a side, not on key width.
body_checks:
  - engine: "both"
    setup: "CREATE TABLE assignment(emp TEXT, dept TEXT, office TEXT); INSERT INTO assignment VALUES ('ana','ops','A1'),('bo','dev','B2'),('cy','dev','B2'); CREATE TABLE ed AS SELECT DISTINCT emp, dept FROM assignment; CREATE TABLE dof AS SELECT DISTINCT dept, office FROM assignment;"
    query: "SELECT COUNT(*) FROM ed NATURAL JOIN dof"
    expect: [[3]]
  - engine: "postgres"
    setup: "CREATE TABLE a(id INT, tags JSONB); CREATE INDEX ON a USING GIN (tags); INSERT INTO a VALUES (1, '[1,2]'), (2, '[3]');"
    query: "SELECT id FROM a WHERE tags @> '[2]'"
    expect: [[1]]
---
## Learning objectives

- Decide whether a decomposition is lossless using the common-attribute key test.
- Count the spurious tuples produced by a lossy decomposition.
- Explain why BCNF can cost dependency preservation and what 3NF guarantees instead.
- Judge when denormalization, including JSON columns, is a defensible trade-off.

## The invariant

Normalizing means replacing one relation R by projections R1, ..., Rn. Two properties decide whether the replacement is safe.

**Lossless join.** Joining the projections must give back exactly R for every legal instance. The join always returns *at least* the original tuples; a lossy decomposition returns extra, **spurious** tuples that look like facts but are not. For a binary split, the test is: the decomposition of R into R1 and R2 is lossless if and only if (R1 ∩ R2) → R1 or (R1 ∩ R2) → R2 follows from the FDs. In words: the shared columns must be a key of one side. Heath (1971) proved the "if" direction; the converse holds when only FDs are considered.

**Dependency preservation.** Each FD should be checkable inside one projection, so that a key or constraint on one table enforces it. Otherwise checking it needs a join on every insert.

The two goals interact with the normal forms. A lossless, dependency-preserving decomposition into **3NF** always exists (Bernstein's synthesis, 1976). A lossless decomposition into **BCNF** always exists, but sometimes no BCNF decomposition preserves all dependencies.

**Denormalization** is the reverse move: storing a derivable fact again so reads avoid work. It reintroduces the anomalies from the previous section, so it is a trade-off, not an optimization that comes for free. The standard offers no "denormalize" construct; materialized views and triggers are the engine-level ways to keep a copy consistent.

## Worked example

`assignment(emp, dept, office)` with the true rule `dept → office` and data `('ana', 'ops', 'A1')`, `('bo', 'dev', 'B2')`, `('cy', 'dev', 'B2')`.

Lossless split: `(emp, dept)` and `(dept, office)`. The shared column `dept` determines `(dept, office)`, so joining reproduces exactly three rows.

Lossy split: `(emp, office)` and `(dept, office)` on data where two departments share office A1:

```sql
CREATE TABLE e AS SELECT DISTINCT emp, office FROM assignment;
CREATE TABLE d AS SELECT DISTINCT dept, office FROM assignment;
SELECT * FROM e NATURAL JOIN d;
-- with ('ana','ops','A1'), ('bo','dev','A1'), ('cy','dev','B2'):
-- 5 rows: 3 real plus (ana, dev, A1) and (bo, ops, A1)
```

A quick lossless check in practice: compare `SELECT COUNT(*)` of the original with the count of the join of the projections on representative data. Equal counts on one instance do not prove losslessness (that is a schema property), but a difference proves the split is lossy.

## Where this shows up today

As of November 2026, the SQL standard (ISO/IEC 9075-2:2023) includes a JSON data type (feature T801), and PostgreSQL 19 documents `json` and `jsonb` types with GIN indexing for `jsonb` containment. Storing a list as a JSON array is a deliberate 1NF trade-off: flexible and fast to read whole, but the elements cannot carry foreign keys or per-element uniqueness. Analytics models such as star schemas denormalize dimensions on purpose because they are rebuilt by a pipeline that is the single writer, which is what makes the copies safe.

## Pitfalls checklist

- Splitting tables on a shared column that is not a key of either side, then "fixing" the extra join rows with `DISTINCT`.
- Normalizing to BCNF and silently losing the ability to enforce a business rule; check which FDs moved across tables.
- Denormalizing without measuring and without a single, automatic owner of the copy.
- Recomputing historical values (prices, tax rates) from current reference data.
- Using JSON arrays for relationships that need integrity, cascades or per-element queries at scale.

## Key takeaways

- A binary split is lossless exactly when the shared attributes are a key of one side.
- Lossy decompositions add spurious tuples; they never lose rows.
- 3NF can always be lossless and dependency-preserving; BCNF can always be lossless but sometimes not dependency-preserving.
- Denormalize only with a measured need and an automatic way to keep copies consistent.

## Sources

- I. J. Heath, "Unacceptable File Operations in a Relational Data Base", *ACM SIGFIDET Workshop*, 1971.
- P. A. Bernstein, "Synthesizing Third Normal Form Relations from Functional Dependencies", *ACM TODS* 1(4), 1976.
- ISO/IEC 9075-2:2023 (JSON data type, feature T801); PostgreSQL 19 documentation, "JSON Types".
