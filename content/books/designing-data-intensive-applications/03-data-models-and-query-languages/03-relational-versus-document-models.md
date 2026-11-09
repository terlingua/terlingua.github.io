---
title: "Relational Versus Document Models"
questions:
  - id: impedance
    prompt: "What is the \"impedance mismatch\" criticism of the SQL model?"
    options:
      - id: o232d3d
        text: "An awkward translation layer is needed between application objects and tables, rows and columns"
        correct: true
      - id: oa1c301
        text: "SQL queries cannot be compiled"
      - id: o94f31d
        text: "SQL databases consume too much electricity"
      - id: o259d59
        text: "Relational tables cannot store numbers"
    explanation: "“if data is stored in relational tables, an awkward translation layer is required between the objects in the application code and the database model of tables, rows, and columns. The disconnect between the models is sometimes called an impedance mismatch.”"
  - id: n-plus-one
    prompt: "A page lists 50 comments and shows each author's name. With a naive ORM, how many queries might run, and what is the fix?"
    options:
      - id: o58c180
        text: "1 query; ORMs always join automatically"
      - id: oa0c9b3
        text: "51 queries; tell the ORM to fetch authors together with comments (a join)"
        correct: true
      - id: of46ca7
        text: "2 queries; nothing needs fixing"
      - id: o49d373
        text: "50 queries; the fix is caching"
    explanation: "“you might end up making a separate query on the users table for each of the N comments to look up its author, resulting in N+1 database queries in total, which is slower than performing the join in the database. To avoid this problem, you may need to tell the ORM to fetch the author information at the same time”."
  - id: orm-criticisms
    prompt: "Which are criticisms of ORMs listed in the chapter?"
    type: multiple
    options:
      - id: oce5352
        text: "They cannot perform simple inserts"
      - id: o6d25a0
        text: "Generated schemas can be awkward and inefficient"
        correct: true
      - id: o0cd6fe
        text: "They make inefficient queries easy to write"
        correct: true
      - id: oef08b3
        text: "They cannot fully hide the differences between models"
        correct: true
      - id: o4c924d
        text: "Analytics teams still need a good relational schema"
        correct: true
    explanation: "The bullets include “ORMs are complex and can’t completely hide the differences between the two models”, “the design of the relational schema still matters when using an ORM”, generated schemas “might be awkward ... and they might be inefficient”, and “ORMs make it easy to accidentally write inefficient queries.”"
  - id: orm-advantages
    prompt: "Which are advantages of ORMs according to the chapter?"
    type: multiple
    options:
      - id: oc33145
        text: "Help manage schema migrations"
        correct: true
      - id: oa40d21
        text: "Reduce boilerplate for translation that is inevitable anyway"
        correct: true
      - id: o8fb2cf
        text: "Help cache query results"
        correct: true
      - id: ode9b7d
        text: "Eliminate the need to understand SQL for complex queries"
    explanation: "“ORMs reduce the amount of boilerplate code required for this translation”, “Some ORMs help with caching the results of database queries”, and “ORMs can also help with managing schema migrations”. The chapter notes “Complicated queries may still need to be handled outside of the ORM”."
  - id: locality
    prompt: "Why can a JSON résumé be fetched faster than the multi-table relational version?"
    options:
      - id: o24ef71
        text: "Relational databases cannot index user_id"
      - id: o77663f
        text: "Document databases keep everything in RAM"
      - id: od08a9b
        text: "JSON is smaller than SQL rows"
      - id: od6dc78
        text: "It has better locality: everything is in one place instead of multiple queries or a multiway join"
        correct: true
    explanation: "“The JSON representation has better locality than the multi-table schema ... In the JSON representation, all the relevant information is in one place, making the query both faster and simpler.”"
  - id: one-to-few
    prompt: "When does embedding related items in a document become a poor choice?"
    options:
      - id: o324e67
        text: "When there are genuinely many related items, such as thousands of comments on a celebrity's post"
        correct: true
      - id: of3be4c
        text: "Whenever there is more than one related item"
      - id: o19be3a
        text: "When the items are strings"
      - id: o5cee30
        text: "When the document is read often"
    explanation: "“If you have a genuinely large number of related items—say, comments on a celebrity’s social media post, of which there could be many thousands—embedding them all in the same document may be too unwieldy, so the relational approach ... is preferable.”"
  - id: region-id
    prompt: "Why store `region_id` instead of the string \"Washington, DC\"?"
    type: multiple
    options:
      - id: o332ede
        text: "IDs make records human-readable"
      - id: o90b54a
        text: "Easy updates in one place"
        correct: true
      - id: o72be67
        text: "Localization and better search"
        correct: true
      - id: o1b8e8b
        text: "Consistent spelling and avoiding ambiguity"
        correct: true
    explanation: "The chapter lists consistent style, “Avoiding ambiguity if several places have the same name”, “Ease of updating”, “Localization support” and “Better search functionality”."
  - id: id-never-changes
    prompt: "Why does an ID never need to change?"
    options:
      - id: o36b95e
        text: "It has no meaning to humans, so it stays the same even when the information it identifies changes"
        correct: true
      - id: o0513b0
        text: "Databases forbid updating IDs"
      - id: od54a25
        text: "IDs are stored in a separate disk"
      - id: oa731bf
        text: "IDs are cryptographically random"
    explanation: "“The advantage of using an ID is that because it has no meaning to humans, it never needs to change: the ID can remain the same even if the information it identifies changes.”"
  - id: normalization-rule
    prompt: "Which statement captures the general principle of normalization trade-offs?"
    options:
      - id: ob68a64
        text: "They have identical performance"
      - id: ofda4d0
        text: "Denormalized: always better at scale"
      - id: o5f5c59
        text: "Normalized: faster to write, slower to query; denormalized: faster to read, more expensive to write"
        correct: true
      - id: oeed45e
        text: "Normalized: faster to read and write"
    explanation: "“normalized data is usually faster to write (since there is only one copy) but slower to query (since it requires joins); denormalized data is usually faster to read (fewer joins) but more expensive to write”."
  - id: denorm-derived
    prompt: "Why does the chapter suggest viewing denormalization as derived data?"
    options:
      - id: oe3265d
        text: "It is always computed by batch jobs"
      - id: o5eeb5a
        text: "Denormalized data can never be updated"
      - id: o3f8966
        text: "You need a process to keep the redundant copies updated, and to handle crashes midway"
        correct: true
      - id: o7ca0e0
        text: "Denormalized data is stored in a cache"
    explanation: "“You might find it helpful to view denormalization as a form of derived data ... since you need to set up a process for updating the redundant copies of the data. Besides the cost of performing all these updates, you need to consider the consistency of the database if a process crashes halfway”."
  - id: oltp-olap-norm
    prompt: "Why do analytical systems tolerate denormalization better than OLTP systems?"
    options:
      - id: o47d458
        text: "They have no joins"
      - id: o9ee17a
        text: "They are smaller"
      - id: oa72c59
        text: "They update in bulk and read-only query performance dominates, and the data is mostly an unchanging history"
        correct: true
      - id: oc468c3
        text: "Regulations require denormalization"
    explanation: "“analytical systems often fare better with denormalized data, since they perform updates in bulk and the performance of read-only queries is the dominant concern.”"
  - id: hydration
    prompt: "X's materialized timeline stores only post and sender IDs. Why not store text, like counts and profile photos?"
    options:
      - id: o80b34b
        text: "Text cannot be stored in a cache"
      - id: o6d44bc
        text: "The timeline is rebuilt every second anyway"
      - id: o2e65b7
        text: "Those change quickly and must show the latest values, and storing them would increase storage cost significantly"
        correct: true
      - id: o4fafb1
        text: "IDs are faster to encrypt"
    explanation: "“The reason for storing only IDs in the precomputed timeline is that the data they refer to is fast-changing ... Moreover, the storage cost would be increased significantly by such denormalization.”"
  - id: hydration-scale
    prompt: "What does hydrating IDs at read time show about joins?"
    options:
      - id: o8eabcd
        text: "Hydration requires a graph database"
      - id: ocdf86b
        text: "Joins can only be done in SQL"
      - id: o364cd1
        text: "Joins are always too slow at scale"
      - id: oe26c65
        text: "Joins at read time need not prevent high-performance, scalable services; hydration parallelizes well"
        correct: true
    explanation: "“having to perform joins when reading data is not, as sometimes claimed, an impediment to creating high-performance, scalable services. Hydrating post and user IDs is actually a fairly easy operation to scale, since it parallelizes well”."
  - id: mixed
    prompt: "What is the lesson of the timeline example for deciding whether to denormalize?"
    options:
      - id: o3f3726
        text: "The most scalable approach may denormalize some things and keep others normalized, depending on change rates and read/write costs"
        correct: true
      - id: o0219cb
        text: "Always denormalize for social networks"
      - id: o7acc8e
        text: "Only the number of users matters"
      - id: o3e2a1e
        text: "Always normalize for social networks"
    explanation: "“the most scalable approach may involve denormalizing some things and leaving others normalized ... Normalization and denormalization are not inherently good or bad”."
  - id: relationship-types
    prompt: "A person lives in one region, and many people live in each region. Which relationship is this?"
    options:
      - id: o7c2b7c
        text: "One-to-one"
      - id: o8d1e6f
        text: "One-to-many from person to region"
      - id: o0b6f11
        text: "Many-to-one"
        correct: true
      - id: oa96ef6
        text: "Many-to-many"
    explanation: "“the region_id field is an example of a many-to-one relationship (many people live in the same region, but we assume that each person lives in only one region at any one time).”"
  - id: both-directions
    prompt: "You store a person's org IDs in the résumé and the résumé IDs in each organization document. What is the risk, and the normalized alternative?"
    options:
      - id: o171491
        text: "No risk; the database keeps both in sync"
      - id: o9a3550
        text: "The relationship is stored twice and can become inconsistent; store it once and use secondary indexes for both directions"
        correct: true
      - id: oa02fa5
        text: "The risk is too many joins; use a graph database"
      - id: o0dd786
        text: "The risk is slow writes; use a cache"
    explanation: "“This representation is denormalized, since the relationship is stored in two places, which could become inconsistent with each other.” and “A normalized representation stores the relationship in only one place and relies on secondary indexes ... in both directions.”"
  - id: fact-table
    prompt: "In a star schema, what does a row in the fact table represent?"
    options:
      - id: o6520a8
        text: "A product"
      - id: od50c63
        text: "A day of the year"
      - id: o06c718
        text: "An event at a particular time, such as one product purchased"
        correct: true
      - id: oc2a3d4
        text: "A store"
    explanation: "“Each row of the fact table represents an event that occurred at a particular time (here, each row represents a customer’s purchase of a product).”"
  - id: date-dimension
    prompt: "Why might even dates be stored in a dimension table?"
    options:
      - id: oe72061
        text: "Because SQL lacks date types"
      - id: o7fc799
        text: "To save space"
      - id: o313872
        text: "Because dates cannot be stored in fact tables"
      - id: o7d7451
        text: "To attach extra information such as public holidays and query holiday versus non-holiday sales"
        correct: true
    explanation: "“Even date and time are often represented using dimension tables, because this allows additional information about dates (such as public holidays) to be encoded”."
  - id: snowflake
    prompt: "Why are star schemas often preferred over more normalized snowflake schemas?"
    options:
      - id: oa2c021
        text: "They are simpler for analysts to work with"
        correct: true
      - id: o87c6d1
        text: "They use less storage"
      - id: occa3b1
        text: "They avoid foreign keys"
      - id: o684ce7
        text: "They are the only schema BI tools support"
    explanation: "“Snowflake schemas are more normalized than star schemas, but star schemas are often preferred because they are simpler for analysts to work with”."
  - id: obt
    prompt: "What does \"one big table\" trade?"
    options:
      - id: o28a7dc
        text: "Fewer columns for more joins"
      - id: o45eea2
        text: "More storage for sometimes faster queries, by folding dimensions into the fact table"
        correct: true
      - id: o7c9d3c
        text: "Consistency for availability"
      - id: o6ddb8c
        text: "Less storage for slower queries"
    explanation: "OBT means “folding the information in the dimensions into denormalized columns in the fact table instead ... while it requires more storage space, it sometimes enables faster queries”."
  - id: multi-item
    prompt: "A customer buys five products in one checkout. How is this usually represented in a star schema?"
    options:
      - id: oc34aa6
        text: "A many-to-many join table"
      - id: oed04e0
        text: "One fact row with an array of products"
      - id: oc0d8c0
        text: "A separate transactions dimension"
      - id: of96b27
        text: "Five fact rows sharing customer ID, store ID and timestamp; the transaction is not represented explicitly"
        correct: true
    explanation: "“that multi-item transaction is not represented explicitly; instead, the fact table has a separate row for each product purchased, and those facts all just happen to have the same customer ID, store ID, and timestamp.”"
  - id: shredding
    prompt: "Your data is a tree of one-to-many relationships, usually loaded whole. Which model and why?"
    options:
      - id: o99e891
        text: "Graph; trees are graphs"
      - id: o9b3e8f
        text: "Document model; shredding into many tables leads to cumbersome schemas and complex code"
        correct: true
      - id: o73f99d
        text: "Key-value; trees need no structure"
      - id: o218b8a
        text: "Relational; documents cannot hold arrays"
    explanation: "“If the data in your application has a document-like structure ... then it’s probably a good idea to use a document model. The relational technique of shredding ... can lead to cumbersome schemas and unnecessarily complicated application code.”"
  - id: nested-ref
    prompt: "Why is it awkward to reference a nested item inside a document?"
    options:
      - id: odf9d2f
        text: "You cannot refer to it directly; you have to say something like \"the second item in user 251's positions\""
        correct: true
      - id: oaa5609
        text: "Nested items have no fields"
      - id: oad6086
        text: "Nested items cannot be stored"
      - id: o2a65fa
        text: "JSON forbids nesting deeper than two levels"
    explanation: "“you cannot refer directly to a nested item within a document; instead, you need to say something like, “the second item in the list of positions for user 251.” If you need to reference nested items, a relational approach works better”."
  - id: reorder
    prompt: "For a drag-and-drop reorderable to-do list, why does the chapter say the document model fits well?"
    options:
      - id: o37069f
        text: "Arrays are faster to index than tables"
      - id: o7d5c9a
        text: "Relational databases cannot store integers"
      - id: of9f99e
        text: "Documents sort themselves automatically"
      - id: oeff00f
        text: "The order can be stored directly in a JSON array, while relational databases need tricks like renumbering, linked lists or fractional indexing"
        correct: true
    explanation: "“The document model supports such applications well, because the items (or their IDs) can simply be stored in a JSON array to determine their order. In relational databases there isn’t a standard way of representing such reorderable lists”."
  - id: schemaless
    prompt: "Why is \"schemaless\" misleading?"
    options:
      - id: o36c0b3
        text: "Reading code still assumes a structure — the schema is implicit, not enforced (schema-on-read)"
        correct: true
      - id: od42a02
        text: "The term refers only to XML"
      - id: oa125d0
        text: "Schemaless databases cannot store fields"
      - id: oa9c505
        text: "Document databases always enforce JSON Schema"
    explanation: "“Document databases are sometimes called schemaless, but that’s misleading as the code that reads the data usually assumes some kind of structure—that is, there is an implicit schema, but it is not enforced by the database”."
  - id: typing-analogy
    prompt: "Schema-on-read is to schema-on-write as which programming concept pair?"
    options:
      - id: o99bf5c
        text: "Functional versus object-oriented"
      - id: o865dea
        text: "Interpreted versus compiled syntax"
      - id: o6e3caf
        text: "Dynamic (runtime) type checking versus static (compile-time) type checking"
        correct: true
      - id: o6bb808
        text: "Synchronous versus asynchronous"
    explanation: "“Schema-on-read is similar to dynamic (runtime) type checking in programming languages, whereas schema-on-write is similar to static (compile-time) type checking.”"
  - id: migration-cost
    prompt: "You add a nullable `first_name` column and populate it with `UPDATE` on a billion-row table. Which step is likely slow and why?"
    options:
      - id: off7379
        text: "Both equally"
      - id: o22093c
        text: "Neither; both are instant"
      - id: o67aa04
        text: "The ALTER TABLE, because it always copies the table"
      - id: od72ef5
        text: "The UPDATE, because every row must be rewritten; adding a nullable column with a default is usually fast"
        correct: true
    explanation: "“In most relational databases, adding a column with a default value is fast and unproblematic, even on large tables. However, running the UPDATE statement is likely to be slow on a large table since every row needs to be rewritten”."
  - id: schema-on-read-when
    prompt: "When is schema-on-read advantageous?"
    type: multiple
    options:
      - id: o1a59f8
        text: "All records are expected to have the same structure"
      - id: of4f011
        text: "The structure is dictated by external systems that may change at any time"
        correct: true
      - id: ofeb5db
        text: "You need the database to enforce data quality"
      - id: o145e92
        text: "Many object types make one table per type impractical"
        correct: true
    explanation: "Schema-on-read helps “if the items in the collection don’t all have the same structure”, e.g., many object types, or structure “determined by external systems over which you have no control”. “But when all records are expected to have the same structure, schemas are a useful mechanism”."
  - id: locality-downsides
    prompt: "Which are downsides of document locality?"
    type: multiple
    options:
      - id: oa42c1b
        text: "Updates usually rewrite the whole document"
        correct: true
      - id: oa0f3e3
        text: "The whole document is usually loaded even if you need a small part"
        correct: true
      - id: o5e61fd
        text: "Documents cannot be indexed"
      - id: o4e9c5b
        text: "Documents cannot contain arrays"
    explanation: "“The database typically needs to load the entire document, which can be wasteful if you need to access only a small part of a large document. Furthermore, on updates to a document, the entire document usually needs to be rewritten.”"
  - id: relational-locality
    prompt: "Which features give locality without the document model?"
    type: multiple
    options:
      - id: o3eff92
        text: "Oracle's multi-table index cluster tables"
        correct: true
      - id: o47e056
        text: "Column families in Bigtable-style wide-column stores"
        correct: true
      - id: o9c57fc
        text: "SQL views"
      - id: oe0a733
        text: "Spanner's interleaved tables"
        correct: true
    explanation: "“Google’s Spanner database offers the same locality properties in a relational data model, by allowing the schema to declare that a table’s rows should be interleaved”, Oracle offers “multi-table index cluster tables”, and wide-column stores have “column families”."
  - id: convergence
    prompt: "What does the chapter say about the convergence of document and relational databases?"
    options:
      - id: obccd74
        text: "It is good news, because the two models work best combined in one database"
        correct: true
      - id: o277d10
        text: "Only document databases have changed"
      - id: od1f593
        text: "It is bad news, because it blurs specialization"
      - id: o8d69f2
        text: "It has not happened"
    explanation: "“This convergence of the models is good news for application developers, because the relational model and the document model work best when you can combine both in the same database.”"
  - id: codd-nonsimple
    prompt: "What did Codd's \"nonsimple domains\" anticipate?"
    options:
      - id: o51aa65
        text: "Column-oriented storage"
      - id: o02cf27
        text: "Nested, JSON-like structures as values inside a relational row"
        correct: true
      - id: o077c97
        text: "Graph databases"
      - id: of787e2
        text: "Distributed transactions"
    explanation: "“Codd’s original description of the relational model allowed something similar to JSON within a relational schema. He called it nonsimple domains.”"
  - id: nosql
    prompt: "How does the chapter characterize NoSQL?"
    options:
      - id: o9cf069
        text: "A query language standard"
      - id: o6019e2
        text: "A specific database product"
      - id: od7aa8a
        text: "A loose set of ideas around new data models, schema flexibility, scalability and open source licensing, not a single technology"
        correct: true
      - id: ob29c28
        text: "A replacement for SQL that has won"
    explanation: "“NoSQL refers not to a single technology but a loose set of ideas around new data models, schema flexibility, scalability, and a move toward open source licensing models.”"
---
The section revisits the relational/document debate: impedance mismatch and ORMs, one-to-many trees,
normalization and joins, many-to-many relationships, star schemas, and when to use which model.

## Learning objectives

- Explain the impedance mismatch and the pros and cons of ORMs, including N+1 queries
- Decide when a document's locality helps and when it hurts
- Weigh normalized versus denormalized data for reads, writes and consistency
- Describe star, snowflake and one-big-table schemas
- Compare schema-on-read with schema-on-write

## In a nutshell

- Documents fit trees of one-to-many relationships loaded together; relational fits many-to-one and
  many-to-many.
- Normalized: faster writes, slower reads (joins). Denormalized: faster reads, costlier writes, and a
  consistency burden.
- X/Twitter's timeline stores only IDs and hydrates them at read time — joins can scale.
- Schemaless is really schema-on-read.
- The models are converging: JSON in SQL databases, joins in document databases.

## Connections to today

- **PostgreSQL `jsonb`** with GIN indexes is the relational–document hybrid the book praises.
- **dbt and the medallion "gold" layer** often produce one-big-table outputs for BI tools.
- **EF Core's `Include`** and similar eager-loading APIs exist to avoid N+1 queries.
- **Fractional indexing** powers ordering in collaborative tools such as Figma and Linear-style task lists.
