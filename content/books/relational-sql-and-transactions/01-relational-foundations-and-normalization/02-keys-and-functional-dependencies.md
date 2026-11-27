---
title: "Keys and Functional Dependencies"
questions:
  - id: "attribute-closure"
    type: "single"
    prompt: "Relation `R(A, B, C, D, E)` has the functional dependencies `A → B`, `BC → D`, `D → E` and `E → C`. What is the closure `{A, D}⁺`?"
    explanation: |-
      The closure of a set of attributes X is every attribute that X determines under the given FDs. Compute it by starting with X and repeatedly adding the right side of any FD whose left side is already inside the result, until nothing changes (a direct consequence of Armstrong's axioms).

      | Step | FD applied | Result |
      |---|---|---|
      | start | | {A, D} |
      | 1 | A → B | {A, B, D} |
      | 2 | D → E | {A, B, D, E} |
      | 3 | E → C | {A, B, C, D, E} |
      | 4 | BC → D adds nothing new | {A, B, C, D, E} |

      "`{A, B, D, E}`" stops one step early and misses `E → C`; closure must be iterated until no FD adds anything.

      "`{A, B, D}`" applies only FDs whose left side is in the starting set and never revisits.

      "`{A, D, E}`" forgets `A → B`, which applies immediately because A is in the start set.

      Because the closure is every attribute of R, `{A, D}` is a superkey.
    options:
      - text: "`{A, B, D, E}`"
      - text: "`{A, B, D}`"
      - text: "`{A, D, E}`"
      - text: "`{A, B, C, D, E}`"
        correct: true
  - id: "candidate-keys"
    type: "multiple"
    prompt: "Using the same `R(A, B, C, D, E)` with `A → B`, `BC → D`, `D → E`, `E → C`, which sets are candidate keys? Select all that apply."
    explanation: |-
      A candidate key is a minimal superkey: its closure is all of R, and no proper subset has that property.

      A never appears on the right side of any FD, so nothing else can determine it; every key must contain A. Its closure alone is `{A}⁺ = {A, B}`, which is not all of R, so `{A}` is not a key.

      Closures of the two-attribute sets that contain A:

      | Set | Closure trace | Key? |
      |---|---|---|
      | {A, C} | A → B gives B; BC → D gives D; D → E gives E | all of R: key |
      | {A, D} | A → B; D → E; E → C | all of R: key |
      | {A, E} | A → B; E → C; BC → D | all of R: key |
      | {A, B} | A → B adds nothing | {A, B}: not a key |

      Each of the three keys is minimal because its only proper subsets are `{A}` (closure `{A, B}`) and a single non-A attribute (which cannot determine A).

      "`{A, B, C}`" is a superkey, but not a candidate key: removing B leaves `{A, C}`, which is still a superkey, so it is not minimal.
    options:
      - text: "`{A, D}`"
        correct: true
      - text: "`{A, E}`"
        correct: true
      - text: "`{A}`"
      - text: "`{A, B, C}`"
      - text: "`{A, C}`"
        correct: true
  - id: "instance-refutes-fd"
    type: "single"
    prompt: "This is the current content of `staff`:\n\n| emp | dept | manager | floor |\n|---|---|---|---|\n| ana | ops | lee | 2 |\n| bo | ops | lee | 3 |\n| cy | dev | kim | 3 |\n| di | dev | kim | 3 |\n\nWhich functional dependency does this instance *violate*?"
    explanation: |-
      X → Y is violated by an instance when two rows agree on X but disagree on Y.

      | FD | Rows that agree on the left side | Do they agree on the right side? |
      |---|---|---|
      | dept → floor | ana and bo (ops) | floor 2 vs 3: violated |
      | dept → manager | ana, bo (ops: lee, lee); cy, di (dev: kim, kim) | yes: holds here |
      | manager → dept | lee rows (ops, ops); kim rows (dev, dev) | yes: holds here |
      | emp → floor | no two rows share an emp | holds trivially |

      Note the asymmetry: an instance can *refute* an FD but can never *prove* one. `dept → manager` holds in these four rows, yet whether it is a rule of the schema is a business fact. Inferring FDs from sample data is how designers mistake coincidences for constraints.
    options:
      - text: "`dept → manager`"
      - text: "`dept → floor`"
        correct: true
      - text: "`manager → dept`"
      - text: "`emp → floor`"
  - id: "fk-match-simple-null"
    type: "single"
    prompt: "`regions` has primary key `(country, region)` and contains only `('US', 'CA')`. `stores` declares `FOREIGN KEY (country, region) REFERENCES regions (country, region)` with no `MATCH` clause. What happens to `INSERT INTO stores (id, country, region) VALUES (1, 'FR', NULL)`?"
    explanation: |-
      ISO SQL defines three match types for referential constraints. Under `MATCH SIMPLE` (the default), the constraint is satisfied if *any* referencing column is NULL, or if all are non-NULL and a matching referenced row exists. Under `MATCH FULL`, the referencing columns must be all NULL or all non-NULL and matching. (`MATCH PARTIAL` is in the standard but rarely implemented.)

      Trace for `('FR', NULL)`: region is NULL → `MATCH SIMPLE` treats the row as satisfying the constraint without looking at `regions`. The insert succeeds even though no French region exists.

      "It fails, because the non-NULL part must still match" describes `MATCH PARTIAL`-style checking of the non-NULL parts, which the default does not do. Under `MATCH FULL` the insert would also fail, but for a different reason: mixing NULL and non-NULL is forbidden.

      Foreign key columns are not implicitly `NOT NULL`; nullable foreign keys are how optional relationships are modeled.

      "Only if `('FR', NULL)` exists" is impossible anyway: primary key columns cannot be NULL.
    options:
      - text: "It fails, because the non-NULL part `country = 'FR'` must still match some row of `regions`"
      - text: "It fails, because every column that takes part in a foreign key is implicitly declared `NOT NULL`"
      - text: "It succeeds, because under the default `MATCH SIMPLE` a row with any NULL in the foreign key is not checked"
        correct: true
      - text: "It succeeds only if a row `('FR', NULL)` already exists in `regions` for the partial key to match against"
  - id: "superkey-not-minimal"
    type: "single"
    prompt: "A schema has the candidate key `{order_id}`. A designer proposes declaring `PRIMARY KEY (order_id, created_at)` \"to be safe\". What is wrong with that key?"
    explanation: |-
      A superkey is any attribute set whose values are unique across rows; a candidate key is a superkey with no removable attribute. Adding attributes to a key makes the uniqueness rule *weaker*, not stronger: the constraint only forbids duplicates of the whole combination.

      Trace: rows `(42, '2026-01-01')` and `(42, '2026-01-02')` differ on the pair, so `PRIMARY KEY (order_id, created_at)` accepts both, and the real rule ("one row per order") is no longer enforced.

      "Nothing is wrong ... stricter and safer" has the direction backwards.

      "It is not even a superkey" is wrong: any superset of a key is a superkey, because `order_id` alone already makes rows unique in valid data.

      "Only one column" is false; composite primary keys are standard SQL.
    options:
      - text: "Nothing is wrong; adding more columns to a primary key always makes the uniqueness rule stricter and safer"
      - text: "It is not even a superkey, because two different orders can share the same `created_at` timestamp"
      - text: "It is invalid SQL, because a primary key may have only one column"
      - text: "It is a superkey but not minimal, so two rows with the same `order_id` but different `created_at` are accepted"
        correct: true
  - id: "alternate-key-enforcement"
    type: "single"
    prompt: "`users` has two candidate keys: a surrogate `id` and the natural key `email`. The table declares `id INT PRIMARY KEY` and `email TEXT`. What does the relational model say about `email`?"
    explanation: |-
      Choosing a primary key picks one candidate key for identification; the others (alternate keys) remain keys of the relation because the business rule "one account per email" is still true. Codd's model treats all candidate keys as equally valid; SQL only enforces those you declare. Without `UNIQUE`, two rows with the same `email` and different `id` are accepted, which breaks the FD `email → id`.

      "It stopped being a key" confuses the declaration with the fact; FDs come from the domain, not from DDL.

      "A foreign key to `users.id`" mixes up types and meaning; a foreign key references another key, it does not replace one.

      "Enforced automatically because it determines `id`" is backwards: the engine knows only declared constraints and cannot infer `email → id`.
    options:
      - text: "It is still a candidate key; without `UNIQUE NOT NULL` the engine will not enforce it"
        correct: true
      - text: "It stopped being a key of the relation at the moment `id` was chosen as the primary key"
      - text: "It should become a foreign key to `users.id`, so that each email points at the row that owns it"
      - text: "It is enforced automatically by the engine, because `email` functionally determines `id` anyway"
---
## Learning objectives

- Define superkey, candidate key, primary key, alternate key and foreign key precisely.
- Compute an attribute closure and use it to find every candidate key.
- Distinguish what an instance of data can show about an FD from what the schema asserts.
- Predict how a foreign key treats NULLs under `MATCH SIMPLE` and `MATCH FULL`.

## The invariant

A **functional dependency** (FD) X → Y holds in a relation schema when, in every legal instance, any two tuples that agree on all attributes of X also agree on all attributes of Y. FDs are statements about the domain ("an ISBN identifies one title"), not about today's rows.

From FDs come keys:

| Term | Definition |
|---|---|
| Superkey | A set K with K → (all attributes): no two tuples agree on K |
| Candidate key | A superkey with no proper subset that is also a superkey (minimal) |
| Primary key | The candidate key chosen to identify tuples; SQL adds `NOT NULL` |
| Alternate key | Any other candidate key; declare it `UNIQUE NOT NULL` |
| Foreign key | Attributes whose values must appear as a key value in another (or the same) relation |

Codd (1970) introduced primary keys and foreign keys; the theory of FDs and keys was formalized in the following years, and Armstrong's axioms (reflexivity, augmentation, transitivity) are sound and complete for deriving every FD implied by a given set. The practical tool is the **closure** X⁺: start with X, and while some FD L → R has L ⊆ X⁺, add R. X is a superkey exactly when X⁺ contains every attribute.

Two shortcuts make key-finding fast. An attribute that never appears on any right-hand side must be in every key. An attribute that appears only on right-hand sides is in no key.

Layers: the theory says every candidate key is a key; ISO SQL enforces only declared `PRIMARY KEY` and `UNIQUE` constraints, and its `FOREIGN KEY` has `MATCH SIMPLE` (default), `MATCH FULL` and `MATCH PARTIAL`; each engine decides whether and when it checks them.

## Worked example

`R(A, B, C, D, E)` with `A → B`, `BC → D`, `D → E`, `E → C`.

1. A is never on a right side, so every key contains A.
2. `{A}⁺ = {A, B}`. Not a key.
3. Try A plus one attribute. `{A, C}⁺`: B from A, D from BC, E from D, giving all five. `{A, D}⁺`: B, E, then C from E. `{A, E}⁺`: B, C, then D from BC. `{A, B}⁺ = {A, B}`.
4. Candidate keys: `{A, C}`, `{A, D}`, `{A, E}`. Every superset, such as `{A, B, C}`, is a superkey but not a candidate key.

Instances can disprove FDs. In this table, rows 1 and 2 agree on `dept` but not on `floor`, so `dept → floor` is not an FD of this data:

| emp | dept | floor |
|---|---|---|
| ana | ops | 2 |
| bo | ops | 3 |

A query finds such counterexamples quickly:

```sql
SELECT dept
FROM staff
GROUP BY dept
HAVING COUNT(DISTINCT floor) > 1;   -- any row returned refutes dept -> floor
```

No query can prove an FD; an empty result only means "not refuted yet".

## Where this shows up today

As of November 2026, declared keys still differ in enforcement across engines. SQLite enforces foreign keys only after `PRAGMA foreign_keys = ON` on each connection; the sandbox build used for this book (SQLite 3.53.1) starts with it off. PostgreSQL 19 and MySQL 8.4 (InnoDB) enforce declared foreign keys by default. Schema-inference and data-profiling tools that "discover" keys and FDs from samples are useful hints, but they can only report dependencies not yet refuted.

## Pitfalls checklist

- Declaring only a surrogate `id` and forgetting `UNIQUE` on the natural key, so duplicates of the real entity appear.
- Padding a primary key with extra columns "for safety", which weakens uniqueness.
- Inferring FDs from current data and baking coincidences into the design.
- Assuming a composite foreign key with one NULL column is checked; under `MATCH SIMPLE` it is not.
- Forgetting that some engines need foreign key enforcement switched on.

## Key takeaways

- A candidate key is a minimal superkey; the primary key is just the chosen one.
- Compute closures by iterating FDs to a fixed point; attributes on no right-hand side belong to every key.
- Data can refute an FD but never prove one.
- `MATCH SIMPLE` skips the check when any foreign key column is NULL.

## Sources

- E. F. Codd, "A Relational Model of Data for Large Shared Data Banks", *CACM* 13(6), 1970.
- W. W. Armstrong, "Dependency Structures of Data Base Relationships", *IFIP Congress*, 1974.
- ISO/IEC 9075-2, referential constraint definition (`MATCH SIMPLE | FULL | PARTIAL`).
- SQLite documentation, "SQLite Foreign Key Support"; PostgreSQL 19 documentation, "Constraints".
