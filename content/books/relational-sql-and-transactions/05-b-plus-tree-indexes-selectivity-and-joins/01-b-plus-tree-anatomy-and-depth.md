---
title: "B+tree Anatomy and Depth Arithmetic"
questions:
  - id: "depth-from-fanout"
    type: "single"
    prompt: "A B+tree index has a fanout of 500 at every level (each internal page has up to 500 children, each leaf holds up to 500 entries), and pages are full. How many levels (root to leaf, inclusive) are needed for 100 million entries?"
    explanation: |-
      With fanout f and full pages, a tree of h levels indexes up to f^h entries, so h = ⌈log_f N⌉.

      | Level (from bottom) | Pages needed |
      |---|---|
      | leaves | 100,000,000 / 500 = 200,000 |
      | level 2 | 200,000 / 500 = 400 |
      | root | 400 / 500 → 1 page |

      Three levels. Check: 500³ = 125,000,000 ≥ 100,000,000 > 500² = 250,000. Equivalently log₅₀₀(10⁸) = 8 / log₁₀(500) ≈ 8 / 2.699 ≈ 2.96, rounded up to 3.

      "27" is ⌈log₂ N⌉, the depth of a balanced *binary* tree; high fanout is the whole point of B-trees.

      "2" would cover only 500² = 250,000 entries.

      "4" would cover 62.5 billion entries; it is an overestimate that ignores how fast powers of 500 grow.
    options:
      - text: "27"
      - text: "2"
      - text: "4"
      - text: "3"
        correct: true
  - id: "fanout-from-page-size"
    type: "single"
    prompt: "An internal B+tree page is 16,384 bytes with a 128-byte page header. Each entry is an 8-byte key, a 6-byte child pointer and a 2-byte slot. What is the page's maximum fanout?"
    explanation: |-
      Fanout = usable bytes ÷ bytes per entry.

      | Quantity | Value |
      |---|---|
      | usable bytes | 16,384 − 128 = 16,256 |
      | bytes per entry | 8 + 6 + 2 = 16 |
      | fanout | 16,256 ÷ 16 = 1,016 |

      "1024" divides the whole page by 16 and forgets the header.

      "2048" divides by the key size alone (16,384 ÷ 8), forgetting pointers and slots.

      "1170" divides the whole page by 14 bytes, forgetting the slot array and the header.

      Real engines also reserve free space and use variable-length keys, so practical fanout is lower; but the lesson is that fanout is in the hundreds or thousands, and it shrinks as keys grow (a 100-byte string key cuts it by roughly a factor of six here).
    options:
      - text: "1016"
        correct: true
      - text: "1024"
      - text: "2048"
      - text: "1170"
  - id: "range-scan-leaf-chain"
    type: "single"
    prompt: "A 3-level B+tree has 500 entries per full leaf, and leaves are linked left to right. A range predicate matches exactly 50,000 consecutive entries that start at the first entry of a leaf. What is the minimum number of index pages read to return them all?"
    explanation: |-
      A range scan descends once to the first matching leaf, then follows the sibling links:

      | Step | Pages |
      |---|---|
      | root and one internal page (descent) | 2 |
      | leaves: 50,000 ÷ 500 | 100 |
      | total | 102 |

      If the range started mid-leaf, one more leaf would be touched (101 leaves).

      "300" re-descends from the root for every leaf (100 × 3); the linked leaf level exists precisely to avoid this.

      "3" counts only one root-to-leaf path, as for a point lookup.

      "50,000" counts entries, not pages. (If each entry then requires a table-page fetch, those fetches come on top; that cost is the subject of the selectivity section.)
    options:
      - text: "300"
      - text: "102"
        correct: true
      - text: "3"
      - text: "50,000"
  - id: "why-keys-in-leaves"
    type: "single"
    prompt: "In a B+tree, all keys with their row pointers live in the leaves, and internal pages hold only separator keys and child pointers. What is the main consequence compared with a B-tree that stores row pointers in internal pages too?"
    explanation: |-
      Comer's 1979 survey describes the B+tree variant: data entries only at the leaf level, internal levels act purely as an index, and leaves are chained for sequential access. Removing row pointers from internal pages packs more separators per page (higher fanout), and an in-order or range scan becomes a walk along one level instead of an in-order tree traversal.

      "Stop early at an internal page" describes the classic B-tree; in a B+tree every lookup goes to a leaf, which also makes the cost predictable (always h page reads).

      "Leaves need not be ordered" is backwards: sorted, linked leaves are what make range scans cheap.

      "Inserts never split" is false: leaves split when full, and splits propagate separators upward, which is how the tree stays balanced.
    options:
      - text: "Point lookups can stop early at an internal page whenever the key matches a separator"
      - text: "Leaves no longer need to be kept in key order, because internal pages hold the order"
      - text: "Internal pages fit more separators, so fanout is higher and the tree shallower, and range scans need only walk the linked leaves"
        correct: true
      - text: "Inserts never split pages, because data never enters internal pages"
  - id: "point-lookup-page-reads"
    type: "single"
    prompt: "A secondary B+tree index on `email` has 3 levels; the table is a heap (rows stored separately, located by row pointer). Ignoring caching, how many page reads does `SELECT * FROM users WHERE email = 'a@x'` need when exactly one row matches?"
    explanation: |-
      The lookup descends the index from root to leaf, finds the entry with the row pointer, then reads the table page that holds the row:

      | Page read | Purpose |
      |---|---|
      | 1 | index root |
      | 2 | index internal page |
      | 3 | index leaf (finds the row pointer) |
      | 4 | heap page with the row (`SELECT *` needs all columns) |

      "3" forgets the table fetch; that would be enough only if the index *covered* the query.

      "1" assumes hashing straight to the row.

      "27" is a binary-tree depth for about 100 million keys.

      In practice the root and upper levels are almost always cached, so a lookup typically costs one or two physical reads; the logical count is still 4.
    options:
      - text: "3"
      - text: "1"
      - text: "27"
      - text: "4"
        correct: true
  - id: "height-growth"
    type: "single"
    prompt: "The index from the first question (fanout 500, 3 levels for 100 million entries) grows to 10 billion entries. How many levels does it need now?"
    explanation: |-
      Height grows with the logarithm of N, base fanout: ⌈log₅₀₀(10¹⁰)⌉ = ⌈10 / 2.699⌉ = ⌈3.705⌉ = 4. Check: 500³ = 1.25 × 10⁸ < 10¹⁰ ≤ 500⁴ = 6.25 × 10¹⁰.

      A hundredfold growth adds one level, because each level multiplies capacity by 500.

      "300" scales height linearly with data (3 × 100).

      "6" doubles the height, as if height grew with the square of data.

      "5" adds a level per factor of 10, the intuition from base-10 digits rather than base-500.
    options:
      - text: "4"
        correct: true
      - text: "300"
      - text: "6"
      - text: "5"
---
## Learning objectives

- Describe the structure of a B+tree: root, internal pages, linked leaves.
- Compute fanout from page and entry sizes, and height from fanout and row count.
- Count page reads for point lookups and range scans.
- Explain why B+trees stay shallow as data grows.

## The invariant

Bayer and McCreight introduced the B-tree in 1972; Comer's 1979 survey "The Ubiquitous B-Tree" describes the **B+tree** variant that nearly every relational engine uses for indexes:

- All entries (key plus row pointer, or key plus the row itself in a clustered index) live in **leaf pages**, sorted by key.
- **Internal pages** hold separator keys and child pointers only.
- Leaves are **linked** to their neighbors, so a range is read by walking sideways.
- Every leaf is at the same depth (the tree is balanced). Inserts split full pages and push a separator up; the root splits last, which is the only way the tree grows taller.

Two numbers determine cost:

| Quantity | Formula |
|---|---|
| fanout f | usable page bytes ÷ bytes per entry |
| height h | ⌈log_f N⌉ for N entries in full pages |

Because f is in the hundreds or thousands, h is tiny: with f = 500, three levels hold 125 million entries and four hold 62.5 billion. A point lookup costs h index page reads (plus a table read if the index does not contain the needed columns); a range scan costs h − 1 descent reads plus one read per leaf covered. Pages are rarely full in practice (under random inserts, split pages end up roughly two-thirds full on average), which lowers the effective fanout but changes the height by at most a level.

These properties are algorithmic invariants of the structure; the SQL standard says nothing about indexes at all. Indexes are an implementation feature, created with non-standard (but near-universal) `CREATE INDEX`.

## Worked example

An `orders` index on `order_id` (8-byte key) in 8 KiB pages:

```python
import math

page, header, entry = 8192, 96, 8 + 8   # bytes; key + pointer
fanout = (page - header) // entry        # 506
for n in (10**6, 10**8, 10**10):
    print(n, math.ceil(math.log(n) / math.log(fanout)))
# 1000000      3
# 100000000    3
# 10000000000  4
```

So a ten-thousandfold growth in rows adds one level. A query for 10,000 consecutive order ids reads 2 internal pages and about 10,000 ÷ 506 ≈ 20 leaves; fetching the rows themselves from the table is usually the larger cost, which the next sections quantify.

## Where this shows up today

As of November 2026, the default page sizes are still 8 KiB in PostgreSQL (the `block_size` setting, fixed at build time; 8192 in this book's check environment) and SQL Server, 16 KiB for InnoDB (`innodb_page_size`, as documented in the MySQL 8.4 manual) and 4 KiB for new SQLite databases (checked on SQLite 3.53.1). The arithmetic above applies to all of them; only f changes.

## Pitfalls checklist

- Estimating index depth with log₂ and fearing "deep" trees.
- Using wide keys (long strings, many columns) without realizing they cut fanout.
- Counting index pages but forgetting the table-page fetch per matching row.
- Assuming a range scan re-descends the tree per row; it walks the leaf chain.
- Believing tree height is a common cause of slow lookups; it rarely exceeds four or five levels.

## Key takeaways

- Fanout = usable page bytes ÷ entry size; height = ⌈log_f N⌉.
- A hundredfold or thousandfold data growth usually adds a single level.
- Point lookups cost h index reads; ranges cost a descent plus one read per leaf.
- Keys live only in leaves, which are linked for range scans.

## Sources

- R. Bayer and E. McCreight, "Organization and Maintenance of Large Ordered Indexes", *Acta Informatica* 1(3), 1972.
- D. Comer, "The Ubiquitous B-Tree", *ACM Computing Surveys* 11(2), 1979.
- PostgreSQL 19 documentation, "Preset Options" (`block_size`); MySQL 8.4 Reference Manual, "InnoDB Startup Options and System Variables" (`innodb_page_size`); SQLite documentation, "PRAGMA page_size"; Microsoft Learn, "Pages and extents architecture guide".
