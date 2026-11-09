---
title: "Data Storage for Analytics"
questions:
  - id: htap-split
    prompt: "What trend does the chapter observe in HTAP databases?"
    options:
      - id: oac3fd2
        text: "They are increasingly becoming two separate storage and query engines behind a common SQL interface"
        correct: true
      - id: o0f64f7
        text: "They are being replaced by spreadsheets"
      - id: o5d3c80
        text: "They are dropping SQL"
      - id: o778d0b
        text: "They are merging into one storage format"
    explanation: "HTAP databases “are increasingly becoming two separate storage and query engines, which happen to be accessible through a common SQL interface”."
  - id: cloud-dw-elastic
    prompt: "Why are cloud data warehouses more elastic?"
    options:
      - id: o8a8374
        text: "They decouple query computation from storage, persisting data in object storage"
        correct: true
      - id: o933149
        text: "They have no SQL interface"
      - id: o2ea581
        text: "They limit query concurrency"
      - id: oc55f31
        text: "They store data on local SSDs"
    explanation: "“These warehouses are also more elastic because they decouple query computation from the storage layer. Data is persisted in object storage rather than on local disks”."
  - id: components
    prompt: "Match the component: \"defines which files constitute a table and its schema, enabling time travel and transactions\"."
    options:
      - id: o26947d
        text: "Table format (e.g., Iceberg, Delta)"
        correct: true
      - id: oa33195
        text: "Query engine (e.g., Trino)"
      - id: of9c8ef
        text: "Storage format (e.g., Parquet)"
      - id: o8a9457
        text: "Data catalog (e.g., Unity Catalog)"
    explanation: "“To support row inserts and deletions, a table format such as Apache Iceberg or Databricks’s Delta format can be used. Table formats specify a file format that defines which files constitute a table along with the table’s schema.”"
  - id: catalog
    prompt: "What does a data catalog define, and why does decoupling it help?"
    options:
      - id: o84feb4
        text: "How rows are encoded in files; it speeds queries"
      - id: o507d56
        text: "Which CPU runs a query; it reduces cost"
      - id: o900023
        text: "Which tables are in a database; decoupling lets discovery and governance systems access the metadata"
        correct: true
      - id: o958cd0
        text: "The schema of a single file; it enables compression"
    explanation: "“a data catalog defines which tables are contained in a database” and “decoupling them has enabled data discovery and data governance systems ... to access a catalog’s metadata as well.”"
  - id: columnar-why
    prompt: "A query touches 3 of 100 columns over billions of rows. Why does columnar storage help?"
    options:
      - id: o2163b2
        text: "Columnar storage keeps all rows in memory"
      - id: obf6417
        text: "It avoids the need for joins"
      - id: obb8964
        text: "It sorts each column independently"
      - id: oa02d97
        text: "Only the 3 needed columns are read and parsed"
        correct: true
    explanation: "“If each column is stored separately, a query needs to read and parse only those columns that are used in that query, which can save a lot of work.”"
  - id: reassemble
    prompt: "How does a column store reassemble the 23rd row?"
    options:
      - id: o86a736
        text: "Read the 23rd file"
      - id: o2916ae
        text: "Rows cannot be reassembled"
      - id: o09f7c4
        text: "Look up a row index in a B-tree"
      - id: ofd0ee9
        text: "Take the 23rd entry from each column, since all columns store rows in the same order"
        correct: true
    explanation: "“The column-oriented storage layout relies on each column storing the rows in the same order ... you can take the 23rd entry from each of the individual columns and put them together to form the 23rd row”."
  - id: blocks
    prompt: "Why do columnar engines split tables into blocks, often by timestamp range?"
    options:
      - id: oa396ea
        text: "To make writes random"
      - id: oa85419
        text: "So queries load only the needed columns in blocks overlapping the requested date range"
        correct: true
      - id: o0a652f
        text: "Because files cannot exceed 4 KiB"
      - id: o5121b8
        text: "To store rows together"
    explanation: "“it is common to make each block contain the rows for a particular timestamp range. A query then needs to load only the columns it needs in those blocks that overlap with the required date range.”"
  - id: bitmap
    prompt: "A column has 100,000 distinct products across billions of sales rows. How does bitmap encoding represent it?"
    options:
      - id: of30b4c
        text: "One integer per row"
      - id: o6a5bc2
        text: "One bitmap per row"
      - id: oeda4ad
        text: "A hash table of rows"
      - id: o08065c
        text: "One bitmap per distinct value, one bit per row, often run-length encoded because it is sparse"
        correct: true
    explanation: "“We can now take a column with n distinct values and turn it into n separate bitmaps: one bitmap for each distinct value, with one bit for each row.” Sparse bitmaps “can additionally be run-length encoded”."
  - id: bitmap-ops
    prompt: "How is `WHERE product_sk = 30 AND store_sk = 3` evaluated with bitmaps?"
    options:
      - id: o0bc29a
        text: "Sorting the store column"
      - id: of1221d
        text: "Bitwise OR of all product bitmaps"
      - id: o2b5c04
        text: "A nested-loop join"
      - id: oc1d573
        text: "Bitwise AND of the two bitmaps, which works because columns share row order"
        correct: true
    explanation: "“Load the bitmaps for product_sk = 30 and store_sk = 3, and calculate the bitwise AND. This works because the columns contain the rows in the same order”."
  - id: wide-column
    prompt: "Is HBase a column-oriented database in the analytics sense?"
    options:
      - id: od1025d
        text: "No — wide-column stores are row-oriented, storing a row's values together"
        correct: true
      - id: o0ec7ed
        text: "Yes — \"column family\" means columnar"
      - id: o8ed7c9
        text: "Only for analytics queries"
      - id: oa25623
        text: "Yes — it uses Parquet internally"
    explanation: "“Despite the similarity in name, wide-column databases are row-oriented, since they store all values from a row together. Google Bigtable, Apache Accumulo, and HBase are examples”."
  - id: sort-columns
    prompt: "Why can't each column be sorted independently?"
    options:
      - id: o0a6b94
        text: "Sorting is too expensive"
      - id: o09e53d
        text: "Columns cannot be sorted"
      - id: oca76ea
        text: "It would break compression"
      - id: o0d69e0
        text: "You would lose which values belong to the same row; rows must be sorted as a whole"
        correct: true
    explanation: "“sorting each column independently wouldn’t make sense because then we would no longer know which items in the columns belong to the same row ... the data needs to be sorted an entire row at a time”."
  - id: first-sort-key
    prompt: "Which column compresses best after sorting by (date_key, product_sk, …)?"
    options:
      - id: o129ee3
        text: "product_sk, because it has more values"
      - id: oa2bfad
        text: "All columns compress equally"
      - id: o8eab9b
        text: "The last column in sort priority"
      - id: o9828bf
        text: "date_key, the first sort key, because it forms long runs of repeated values"
        correct: true
    explanation: "“That compression effect is strongest on the first sort key. The second and third sort keys will be more jumbled up ... Columns further down the sorting priority appear in essentially random order”."
  - id: columnar-writes
    prompt: "How do columnar stores handle writes efficiently?"
    options:
      - id: o7e476e
        text: "Writes go to an in-memory row-oriented sorted store and are merged in bulk into new column files"
        correct: true
      - id: of392cb
        text: "They write each value to a B-tree"
      - id: oc5ad2a
        text: "They forbid inserts"
      - id: o36c4fe
        text: "Each insert rewrites all compressed columns"
    explanation: "“All writes first go to a row-oriented, sorted, in-memory store. When enough writes have accumulated, they are merged with the column-encoded files on disk and written to new files in bulk.”"
  - id: writes-visible
    prompt: "After an insert, will an analyst's next query see the row even though it isn't in column files yet?"
    options:
      - id: od9a111
        text: "Yes — queries combine on-disk column data with recent in-memory writes"
        correct: true
      - id: od3aed0
        text: "No — only after the next bulk merge"
      - id: o00eb75
        text: "Only if the query uses a hint"
      - id: od85c10
        text: "Only in HTAP databases"
    explanation: "“Queries need to examine both the column data on disk and the recent writes in memory, and combine the two ... data that has been modified with inserts, updates, or deletes is immediately reflected in subsequent queries.”"
  - id: compile-vs-vector
    prompt: "Which statement correctly contrasts query compilation and vectorized processing?"
    options:
      - id: o7a0eb0
        text: "Vectorization requires GPUs"
      - id: o1aab70
        text: "Both interpret one row at a time"
      - id: o54f15b
        text: "Compilation generates and compiles code that iterates row by row; vectorization interprets predefined operators that process batches of column values"
        correct: true
      - id: oe215ef
        text: "Compilation processes batches; vectorization compiles code"
    explanation: "Compilation: the engine “generates code for executing it ... then compiles the generated code to machine code”. Vectorized: “The query is interpreted, not compiled, but it is made fast by processing many values from a column in a batch”."
  - id: cpu-tricks
    prompt: "Which CPU-friendly techniques do both approaches exploit?"
    type: multiple
    options:
      - id: oc81773
        text: "Random memory access patterns"
      - id: o1465bb
        text: "Operating directly on compressed data"
        correct: true
      - id: o809129
        text: "SIMD and multithreading"
        correct: true
      - id: o2534b8
        text: "Tight inner loops without function calls"
        correct: true
      - id: o9960e8
        text: "Sequential memory access to reduce cache misses"
        correct: true
    explanation: "The chapter lists “Preferring sequential memory access”, “tight inner loops”, “parallelism such as multiple threads and single-instruction, multiple data (SIMD)” and “Operating directly on compressed data”."
  - id: virtual-vs-mat
    prompt: "How does a virtual view differ from a materialized view?"
    options:
      - id: oae0caa
        text: "A materialized view cannot be queried"
      - id: ofd29d3
        text: "A virtual view is expanded into its query at read time; a materialized view stores the results and must be updated on writes"
        correct: true
      - id: o6b72f6
        text: "They are identical"
      - id: ob67712
        text: "A virtual view is faster to read"
    explanation: "“A materialized view is an actual copy of the query results, written to disk, whereas a virtual view is just a shortcut for writing queries.”"
  - id: cube-tradeoff
    prompt: "A data cube aggregates sales by date and product. Which question can it not answer?"
    options:
      - id: od1d546
        text: "Total sales per product"
      - id: obea8b2
        text: "Total sales per date"
      - id: o447889
        text: "What fraction of sales came from items costing more than $100"
        correct: true
      - id: o598455
        text: "Total sales overall"
    explanation: "“there is no way of calculating which proportion of sales comes from items that cost more than $100, because price isn’t one of the dimensions.”"
  - id: raw-data
    prompt: "What do most data warehouses do about data cubes?"
    options:
      - id: o0c8541
        text: "Keep as much raw data as possible and use cubes only as a performance boost for certain queries"
        correct: true
      - id: oa4d0f0
        text: "Replace raw data with cubes"
      - id: oa312b0
        text: "Avoid cubes entirely"
      - id: oc10b73
        text: "Store only daily totals"
    explanation: "“Most data warehouses therefore try to keep as much raw data as possible and use aggregates such as data cubes only as a performance boost for certain queries.”"
---
Analytical engines read a few columns across billions of rows. Column storage, compression, sort order,
compiled or vectorized execution and pre-aggregation make that fast.

## Learning objectives

- Describe the disaggregated cloud warehouse stack: query engine, storage format, table format, catalog
- Explain why columnar storage and bitmap encoding speed up analytics
- Reason about sort order and its effect on compression
- Contrast query compilation and vectorized processing
- Weigh data cubes against raw data

## In a nutshell

- HTAP products are increasingly two engines behind one SQL interface.
- Open warehouses split into Trino/DataFusion, Parquet/ORC, Iceberg/Delta, and catalogs such as Unity or Polaris.
- Store columns together; reconstruct rows by position.
- Sort by the most-filtered column first; the first sort key compresses best.
- Writes go to an in-memory row store and are merged into new columnar files in bulk.
- Data cubes are fast but inflexible.

## Connections to today

- **Iceberg REST catalogs** are becoming the interoperability layer between engines such as Spark,
  Trino, Snowflake and DuckDB.
- **Apache Arrow** is the in-memory columnar format shared by Polars, DuckDB and many ML tools.
- **Lance** (named in the chapter) targets AI datasets, including embeddings and images.
