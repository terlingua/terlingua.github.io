---
title: "Normal Forms and the Anomalies They Prevent"
questions:
  - id: "partial-dependency-2nf"
    type: "single"
    prompt: "`order_lines(order_id, product_id, quantity, product_name)` has candidate key `(order_id, product_id)` and the FDs `order_id, product_id → quantity` and `product_id → product_name`. Values are atomic. What is the highest normal form it satisfies?"
    explanation: |-
      2NF forbids a non-prime attribute (one that belongs to no candidate key) from depending on a proper subset of a candidate key.

      Trace: the only key is `(order_id, product_id)`. `product_name` is non-prime, and `product_id → product_name` makes it depend on `product_id`, a proper subset of the key. That is a partial dependency, so the table is in 1NF (atomic values) but not 2NF.

      "2NF, but not 3NF" would require the partial dependency to be absent and a transitive one present.

      "3NF, but not BCNF" and "BCNF" are impossible: each normal form includes the previous one, so failing 2NF fails all higher forms.

      The symptom: the product name is stored once per order line, so renaming a product means updating many rows.
    options:
      - text: "1NF, but not 2NF"
        correct: true
      - text: "2NF, but not 3NF"
      - text: "3NF, but not BCNF"
      - text: "BCNF"
  - id: "transitive-dependency-3nf"
    type: "single"
    prompt: "`employees(emp_id, name, dept_id, dept_name)` has key `emp_id` and the FDs `emp_id → name, dept_id` and `dept_id → dept_name`. What is the highest normal form it satisfies?"
    explanation: |-
      With a single-attribute key there can be no partial dependency (a proper subset of `{emp_id}` is the empty set), so the table is in 2NF.

      3NF requires that for every nontrivial FD X → A, either X is a superkey or A is prime. `dept_id → dept_name`: `dept_id` is not a superkey (many employees share a department) and `dept_name` is not prime. That FD violates 3NF. It is a transitive dependency: `emp_id → dept_id → dept_name`.

      "1NF, but not 2NF" mistakes the transitive dependency for a partial one; partial dependencies need a composite key.

      "3NF, but not BCNF" would need every violating FD to have a prime right side, which `dept_name` is not.

      "BCNF" fails for the same FD: its left side is not a superkey.

      The fix is to decompose into `employees(emp_id, name, dept_id)` and `departments(dept_id, dept_name)`.
    options:
      - text: "1NF, but not 2NF"
      - text: "2NF, but not 3NF"
        correct: true
      - text: "3NF, but not BCNF"
      - text: "BCNF"
  - id: "update-anomaly-trace"
    type: "single"
    prompt: "`course_offering(course_id, instructor, office)` has key `course_id`, and the business rule is `instructor → office`. It contains `('db1', 'kim', 'B12')`, `('db2', 'kim', 'B12')`, `('os1', 'lee', 'C3')`. After `UPDATE course_offering SET office = 'B14' WHERE course_id = 'db1'`, what does `SELECT DISTINCT office FROM course_offering WHERE instructor = 'kim'` return?"
    explanation: |-
      This is an update anomaly. The fact "kim's office" is stored once per course kim teaches, and the `UPDATE` changed only the copy in row `db1`.

      | course_id | instructor | office after update |
      |---|---|---|
      | db1 | kim | B14 |
      | db2 | kim | B12 |
      | os1 | lee | C3 |

      Filtering on `instructor = 'kim'` keeps the first two rows; `DISTINCT office` yields `B12` and `B14`.

      "One row: `B14`" assumes the update propagated to every copy; nothing makes it do so.

      "One row: `B12`" ignores the updated row.

      "An error" assumes the engine knows `instructor → office`. It only enforces declared constraints, and an FD whose left side is not a key cannot be declared as `UNIQUE`. The cure is decomposition so that the fact is stored once (BCNF here).
    options:
      - text: "One row: `B14`"
      - text: "One row: `B12`"
      - text: "Two rows: `B12` and `B14`"
        correct: true
      - text: "An error, because the update violates `instructor → office`"
  - id: "anomaly-kinds"
    type: "multiple"
    prompt: "`course_offering(course_id PRIMARY KEY, instructor, office)` stores `instructor → office` redundantly. Which problems does this design cause? Select all that apply."
    explanation: |-
      The three classic anomalies all follow from storing the fact `instructor → office` inside a relation whose key is `course_id`:

      - **Insertion anomaly:** a row needs a non-NULL `course_id` (primary key), so an instructor without a course has nowhere to live.
      - **Update anomaly:** the office is copied into every course row of that instructor; a partial update leaves contradictory copies.
      - **Deletion anomaly:** when the last course row goes, the only copy of the office goes with it.

      "Violates 1NF" is a common confusion: 1NF is about atomic values and no repeating groups within a row. Repeated values across rows are redundancy, which is what 2NF, 3NF and BCNF address.

      "The primary key is invalid" is wrong: `course_id` is the key; non-key columns may repeat freely.
    options:
      - text: "The table violates 1NF, because the same office value is repeated"
      - text: "The primary key is invalid, because `instructor` values repeat"
      - text: "A new instructor's office cannot be recorded until the instructor has a course"
        correct: true
      - text: "Changing an instructor's office requires updating every row of their courses"
        correct: true
      - text: "Deleting an instructor's last course also deletes the record of their office"
        correct: true
  - id: "delete-anomaly-trace"
    type: "single"
    prompt: "Starting from `('db1', 'kim', 'B12')`, `('db2', 'kim', 'B12')`, `('os1', 'lee', 'C3')` in `course_offering`, the course `os1` is cancelled: `DELETE FROM course_offering WHERE course_id = 'os1'`. What does `SELECT office FROM course_offering WHERE instructor = 'lee'` return now?"
    explanation: |-
      Row `os1` was the only place where `lee`'s office was stored. Deleting the course deletes that fact too:

      | Before | After |
      |---|---|
      | db1, kim, B12 | db1, kim, B12 |
      | db2, kim, B12 | db2, kim, B12 |
      | os1, lee, C3 | (deleted) |

      The `WHERE instructor = 'lee'` filter now matches nothing, so the result is empty.

      "One row: `C3`" assumes the office is stored independently of the course; in this design it is not.

      "One row: `NULL`" confuses an empty result with a row containing NULL; a `DELETE` removes rows, it does not blank out columns.

      "An error" would require a foreign key from some instructors table, which this design does not have. Adding `instructors(instructor, office)` is exactly the decomposition that removes the anomaly.
    options:
      - text: "No rows"
        correct: true
      - text: "One row: `C3`"
      - text: "One row: `NULL`"
      - text: "An error, because `lee` is still referenced"
  - id: "third-nf-not-bcnf"
    type: "single"
    prompt: "`addresses(street, city, zip)` has the FDs `street, city → zip` and `zip → city`. Its candidate keys are `{street, city}` and `{street, zip}`. What is the highest normal form it satisfies?"
    explanation: |-
      BCNF demands that the left side of every nontrivial FD be a superkey. 3NF relaxes this: a violating FD is allowed if its right side is prime (part of some candidate key).

      | FD | Left side a superkey? | Right side prime? | BCNF | 3NF |
      |---|---|---|---|---|
      | street, city → zip | yes (it is a key) | | ok | ok |
      | zip → city | no | yes (`city` is in key `{street, city}`) | violated | ok |

      So the relation is in 3NF but not BCNF.

      "BCNF" ignores `zip → city`.

      "2NF, but not 3NF" and "1NF, but not 2NF" would need a non-prime attribute, but every attribute here is prime (each appears in some key), so neither 2NF nor 3NF can be violated.

      This is the textbook case where BCNF decomposition (`zip_city(zip, city)`, `street_zip(street, zip)`) loses the ability to check `street, city → zip` within a single table, which the next section examines.
    options:
      - text: "BCNF"
      - text: "3NF, but not BCNF"
        correct: true
      - text: "2NF, but not 3NF"
      - text: "1NF, but not 2NF"
---
## Learning objectives

- State 1NF, 2NF, 3NF and BCNF in terms of keys and functional dependencies.
- Classify a relation's highest normal form from its FDs.
- Demonstrate insertion, update and deletion anomalies on a small table.
- Recognize the 3NF-but-not-BCNF pattern.

## The invariant

Normalization exists to make every fact be stored once. When a fact is stored many times, three **anomalies** appear (Codd 1971, "Further Normalization of the Data Base Relational Model"): an *insertion anomaly* (a fact cannot be recorded without an unrelated fact), an *update anomaly* (copies can disagree) and a *deletion anomaly* (removing one fact destroys another). The normal forms are increasingly strict rules on functional dependencies that remove the redundancy causing them.

Let a **prime** attribute be one that belongs to at least one candidate key.

| Form | Rule (for every nontrivial FD X → A) | Removes |
|---|---|---|
| 1NF | Every attribute value is atomic; no repeating groups | Lists hidden in a column |
| 2NF | 1NF, and no non-prime A depends on a proper subset of a candidate key | Partial dependencies |
| 3NF | X is a superkey, or A is prime | Transitive dependencies |
| BCNF | X is a superkey | Every FD-based redundancy |

Each form implies the previous ones. 3NF and BCNF differ only when a non-superkey determines a prime attribute, which requires overlapping composite candidate keys. Normal forms concern *schemas*, not data: a design is in BCNF because of its FDs, whatever rows it holds today. The SQL standard has no notion of normal forms; engines enforce only declared keys and constraints, so an FD that is not a key cannot be enforced by `UNIQUE` at all, which is precisely why redundancy is dangerous.

## Worked example

A single table for course offerings, with business rule `instructor → office`:

| course_id | instructor | office |
|---|---|---|
| db1 | kim | B12 |
| db2 | kim | B12 |
| os1 | lee | C3 |

The key is `course_id`. `instructor → office` has a left side that is not a superkey and a non-prime right side, so the table violates 3NF (and BCNF). All three anomalies follow:

```sql
-- Update anomaly: one copy changes, the other does not
UPDATE course_offering SET office = 'B14' WHERE course_id = 'db1';
SELECT DISTINCT office FROM course_offering WHERE instructor = 'kim';
-- B12, B14

-- Deletion anomaly: lee's office disappears with the course
DELETE FROM course_offering WHERE course_id = 'os1';

-- Insertion anomaly: no way to record a new instructor's office
-- without inventing a course_id (the primary key cannot be NULL)
```

The BCNF decomposition stores each fact once:

```sql
CREATE TABLE instructors (
  instructor TEXT PRIMARY KEY,
  office     TEXT NOT NULL
);
CREATE TABLE course_offering (
  course_id  TEXT PRIMARY KEY,
  instructor TEXT NOT NULL REFERENCES instructors (instructor)
);
```

Now an office change is one row, an instructor can exist without courses, and cancelling a course leaves the office intact.

The 3NF-but-not-BCNF case needs overlapping keys. In `addresses(street, city, zip)` with `street, city → zip` and `zip → city`, the keys are `{street, city}` and `{street, zip}`. `zip → city` violates BCNF (zip is not a superkey) but is allowed by 3NF because `city` is prime.

## Where this shows up today

Normalization is a property of the design, not of any engine release, so it has not changed. What has changed is where redundancy is introduced on purpose: service-per-database architectures, caches and read models deliberately copy facts, and each copy carries the update anomaly with it. Those designs replace the database's single-copy guarantee with propagation (events, change data capture) and must tolerate copies that disagree for a while.

## Pitfalls checklist

- Calling repeated values across rows a "1NF violation"; that is redundancy, handled by 2NF and above.
- Checking normal forms against sample rows instead of the domain's FDs.
- Fixing an update anomaly with a trigger that syncs copies, instead of removing the copies.
- Forgetting that a surrogate key does not normalize anything; the FDs among the other columns remain.
- Stopping at 3NF without checking whether overlapping composite keys hide a BCNF violation.

## Key takeaways

- Insertion, update and deletion anomalies are symptoms of a fact stored more than once.
- 2NF removes partial dependencies, 3NF transitive ones, BCNF every FD whose left side is not a superkey.
- 3NF differs from BCNF only when a non-superkey determines a prime attribute.
- The engine enforces keys, not FDs; normalization turns FDs into keys it can enforce.

## Sources

- E. F. Codd, "Further Normalization of the Data Base Relational Model", in *Data Base Systems*, Courant Computer Science Symposia 6, 1971.
- E. F. Codd, "Recent Investigations in Relational Data Base Systems", *IFIP Congress*, 1974 (Boyce–Codd normal form).
- C. Zaniolo, "A New Normal Form for the Design of Relational Database Schemata", *ACM TODS* 7(3), 1982 (the "prime attribute" formulation of 3NF).
