---
title: "DataFrames, Matrices, and Arrays"
questions:
  - id: df-vs-sql
    prompt: "How is a DataFrame typically manipulated compared with a relational table?"
    options:
      - id: o8a709a
        text: "Through stored procedures in the database"
      - id: o0acfdf
        text: "Through a series of commands that incrementally modify structure and content, rather than a declarative query"
        correct: true
      - id: o1a703f
        text: "Only through a GUI"
      - id: oa0135a
        text: "Only through SQL"
    explanation: "“Instead of using a declarative query language such as SQL, a DataFrame is generally manipulated through a series of commands that modify its structure and content. This matches the typical workflow of data scientists, who incrementally “wrangle” the data”."
  - id: merge
    prompt: "What do DataFrames usually call a relational join?"
    options:
      - id: o699912
        text: "Pivot"
      - id: o02a575
        text: "Union"
      - id: o815720
        text: "Explode"
      - id: ob4b01f
        text: "Merge"
        correct: true
    explanation: "“joining the rows in one DataFrame with another DataFrame based on a key (what a relational database calls join is typically called merge on DataFrames).”"
  - id: sparse
    prompt: "Why would a user–movie ratings matrix not fit well in a relational database?"
    options:
      - id: o171252
        text: "Ratings are not numbers"
      - id: o603158
        text: "Relational databases cannot store users"
      - id: offe2af
        text: "Matrices are always small"
      - id: oe86d0e
        text: "It may have many thousands of columns and is sparse"
        correct: true
    explanation: "“This matrix may have many thousands of columns and would therefore not fit well in a relational database, but DataFrames and libraries that offer sparse arrays (such as NumPy for Python) can handle such data easily.”"
  - id: one-hot
    prompt: "How would one-hot encoding represent a movie's genre among comedy, drama and horror?"
    options:
      - id: o24133b
        text: "One column with values 1, 2 or 3"
      - id: o5306bd
        text: "Three columns, with 1 in the movie's genre column and 0 in the others"
        correct: true
      - id: o5e347a
        text: "A single bit"
      - id: oc17ab0
        text: "A string column"
    explanation: "“We create a column for each possible value (‘comedy,’ ‘drama,’ ‘horror,’ etc.), and for each row representing a movie, we put a 1 in the column corresponding to the genre of that movie and a 0 in all the other columns.”"
  - id: why-matrix
    prompt: "Why convert data into a matrix of numbers for ML?"
    options:
      - id: o6514b4
        text: "Matrices are amenable to linear algebra, which underlies many ML algorithms"
        correct: true
      - id: o0ff00f
        text: "Matrices compress better than tables"
      - id: odcb4af
        text: "Matrices avoid the need for training"
      - id: o8ef163
        text: "Matrices are required for SQL"
    explanation: "“Once the data is in the form of a matrix of numbers, it is amenable to linear algebra operations, which form the basis of many ML algorithms.”"
  - id: array-db
    prompt: "Which datasets are array databases most commonly used for?"
    type: multiple
    options:
      - id: o558a38
        text: "Geospatial raster grids"
        correct: true
      - id: o6b773d
        text: "Social network friendships"
      - id: o9d9d95
        text: "Astronomical telescope observations"
        correct: true
      - id: odb1286
        text: "Medical imaging"
        correct: true
    explanation: "Array databases “are most commonly used for scientific datasets such as geospatial measurements (raster data on a regularly spaced grid), medical imaging, or observations from astronomical telescopes”."
  - id: private-copy
    prompt: "Where do DataFrame manipulations usually take place?"
    options:
      - id: oa6bf4b
        text: "Inside the data warehouse only"
      - id: o9195c0
        text: "Directly on the production OLTP database"
      - id: o4a3e32
        text: "On the data scientist's private copy of the data, often on their local machine"
        correct: true
      - id: o5d9033
        text: "In the browser"
    explanation: "“These manipulations usually take place on the data scientist’s private copy of the dataset, often on their local machine, although the end result may be shared with other users.”"
  - id: summary-schema
    prompt: "What does the chapter summary say non-relational models typically have in common regarding schemas?"
    options:
      - id: o09ef30
        text: "They require XML Schema"
      - id: oc917cb
        text: "They have no structure at all"
      - id: o5d8983
        text: "They don't enforce a schema, yet applications still assume a structure — explicit on write or implicit on read"
        correct: true
      - id: obb8815
        text: "They enforce stricter schemas than SQL"
    explanation: "“One thing that nonrelational data models have in common is that they typically don’t enforce a schema for the data they store ... it’s just a question of whether the schema is explicit (enforced on write) or implicit (assumed on read).”"
  - id: summary-models
    prompt: "Match the model to its target: \"anything is potentially related to everything; queries traverse multiple hops\"."
    options:
      - id: o880702
        text: "Graph data models"
        correct: true
      - id: o5a414d
        text: "Document model"
      - id: o75cffb
        text: "DataFrames"
      - id: o6fe72e
        text: "Star schema"
    explanation: "“Graph data models go in the opposite direction, targeting use cases where anything is potentially related to everything and where queries potentially need to traverse multiple hops”."
  - id: unmentioned
    prompt: "Which specialized data models does the summary mention as outside the chapter's main coverage?"
    type: multiple
    options:
      - id: o65e56d
        text: "Genome sequence similarity search"
        correct: true
      - id: o88e080
        text: "Double-entry ledgers, as in TigerBeetle"
        correct: true
      - id: oe17921
        text: "Spreadsheets"
      - id: od76e96
        text: "Full-text search"
        correct: true
    explanation: "The summary lists genome databases such as GenBank, “ledgers with double-entry accounting” with “databases (such as TigerBeetle) that specialize in this data model”, and “Full-text search”."
---
DataFrames bridge relational data and the numeric matrices that ML algorithms consume.

## Learning objectives

- Compare DataFrames with relational tables
- Explain how data is turned into matrices (pivoting, scaling, one-hot encoding)
- Name where array databases are used

## In a nutshell

- DataFrames support relational-like operations (filter, group, merge) via a sequence of commands.
- They often transform data into sparse matrices with thousands of columns.
- One-hot encoding turns categories into 0/1 columns; matrices enable linear algebra.
- Array databases such as TileDB store scientific grids and images.

## Connections to today

- **Polars and DuckDB** offer fast, columnar DataFrame APIs on a laptop.
- **Embedding matrices** for recommender and LLM retrieval systems are dense numeric arrays derived from
  relational data.
- **Feature stores** formalize the DataFrame-to-matrix step for production ML.
