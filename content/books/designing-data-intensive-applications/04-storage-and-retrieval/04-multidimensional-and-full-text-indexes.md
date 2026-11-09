---
title: "Multidimensional and Full-Text Indexes"
questions:
  - id: concatenated
    prompt: "A phone-book index on (lastname, firstname). Which query can it not serve efficiently?"
    options:
      - id: of12e48
        text: "All people with last name \"Lovelace\""
      - id: o2c3f50
        text: "Last names between L and M"
      - id: o8cceb9
        text: "All people with first name \"Ada\""
        correct: true
      - id: o70ca27
        text: "People named \"Lovelace, Ada\""
    explanation: "“the index can be used to find all the people with a particular last name, or all the people with a particular lastname–firstname combination. However, the index is useless if you want to find all the people with a particular first name.”"
  - id: map-query
    prompt: "Why can't a concatenated (latitude, longitude) index answer a map-rectangle query efficiently?"
    options:
      - id: obef9c2
        text: "Longitude can be negative"
      - id: o47298b
        text: "B-trees cannot store decimals"
      - id: o1d42b1
        text: "The query uses AND"
      - id: o23fd8e
        text: "It narrows only one dimension at a time — all latitudes in range at any longitude, or vice versa"
        correct: true
    explanation: "“The index can give you either all the restaurants in a range of latitudes (but at any longitude) or all the restaurants in a range of longitudes ... but not both simultaneously.”"
  - id: spatial-options
    prompt: "Which techniques support multidimensional queries?"
    type: multiple
    options:
      - id: o78319c
        text: "R-trees or Bkd-trees"
        correct: true
      - id: of99d4b
        text: "Space-filling curves with a regular B-tree"
        correct: true
      - id: o25f7e4
        text: "Regular grids of triangles, squares or hexagons"
        correct: true
      - id: o795d35
        text: "A hash index on latitude"
    explanation: "The chapter mentions translating location “into a single number via a space-filling curve”, “specialized spatial indexes such as R-trees or Bkd-trees”, and “regularly spaced grids of triangles, squares, or hexagons”."
  - id: non-geo
    prompt: "Which is a non-geographic use of multidimensional indexes from the chapter?"
    options:
      - id: o6eb190
        text: "Hashing passwords"
      - id: o286d18
        text: "Looking up a user by email"
      - id: o57a812
        text: "Sorting posts by time"
      - id: o3eee75
        text: "Searching products by (red, green, blue) color ranges"
        correct: true
    explanation: "“on an ecommerce website you could use a three-dimensional index on the dimensions (red, green, blue) to search for products in a certain range of colors”."
  - id: fts-dimensions
    prompt: "How can full-text search be viewed as a multidimensional query?"
    options:
      - id: oe1780b
        text: "Each term is a dimension; a document has 1 in a term's dimension if it contains the term"
        correct: true
      - id: o01d8ec
        text: "Each character is a dimension"
      - id: ob3ad60
        text: "Each document is a dimension"
      - id: o9325b5
        text: "Each language is a dimension"
    explanation: "“each word that might appear in a text (a term) is a dimension. A document that contains term x has a value of 1 in dimension x”."
  - id: inverted
    prompt: "What is an inverted index?"
    options:
      - id: oc34825
        text: "A reversed hash table"
      - id: od5ccda
        text: "A B-tree sorted in descending order"
      - id: oe22cf6
        text: "A map from document to its words"
      - id: o3bbd49
        text: "A map from each term to the list of IDs of documents containing it (the postings list)"
        correct: true
    explanation: "“This is a key-value structure where the key is a term and the value is the list of IDs of all the documents that contain the term (the postings list).”"
  - id: lucene-lsm
    prompt: "How does Lucene store its term-to-postings mapping?"
    options:
      - id: o19729b
        text: "In a relational table"
      - id: o03088a
        text: "In SSTable-like sorted files merged in the background, log-structured style"
        correct: true
      - id: o2f79e7
        text: "In memory only"
      - id: o55d3e6
        text: "In a single B-tree overwritten in place"
    explanation: "Lucene “stores the mapping from term to postings list in SSTable-like sorted files, which are merged in the background using the same log-structured approach”."
  - id: trigram
    prompt: "What is the trade-off of a trigram index?"
    options:
      - id: o625d98
        text: "It is small but supports only exact words"
      - id: o20226e
        text: "It cannot handle typos at all"
      - id: o9761f3
        text: "It supports arbitrary substring and regex search but is quite large"
        correct: true
      - id: o4b8bd5
        text: "It works only for English"
    explanation: "Trigram indexes let you “search the documents for arbitrary substrings that are at least three characters long ... the downside is that they are quite large”."
  - id: edit-distance
    prompt: "How does Lucene find words within an edit distance of a query?"
    options:
      - id: o40896f
        text: "It uses Bloom filters"
      - id: o38fdb7
        text: "It stores terms as a finite state automaton and transforms it into a Levenshtein automaton"
        correct: true
      - id: od15a20
        text: "It uses vector embeddings"
      - id: o3a5b11
        text: "It computes edit distance against every term linearly"
    explanation: "“It does this by storing the set of terms as a finite state automaton ... and transforming it into a Levenshtein automaton, which supports efficient search for words within a given edit distance.”"
  - id: semantic
    prompt: "A user searches \"how to close my account\" and should find \"canceling your subscription\". What technique is needed?"
    options:
      - id: of70663
        text: "Semantic search with vector embeddings"
        correct: true
      - id: o625370
        text: "A bitmap index"
      - id: o6001f3
        text: "A trigram index"
      - id: o46d655
        text: "Edit-distance search"
    explanation: "The chapter's example: such a page should be found with “how to close my account” because the phrases “are close in terms of meaning even though they use completely different words.”"
  - id: vector-meanings
    prompt: "What is the difference between \"vector\" in vectorized processing and in embeddings?"
    options:
      - id: o212a9a
        text: "In vectorized processing, a batch of values processed by optimized code; in embeddings, an array of floats locating a point in multidimensional space"
        correct: true
      - id: o1e1a2d
        text: "Embeddings are batches of bits"
      - id: oe0db12
        text: "They mean the same thing"
      - id: oad335e
        text: "Vectorized processing uses floating-point locations"
    explanation: "“In vectorized processing, the vector refers to a batch of bits that can be processed with specially optimized code. In embedding models, a vector is an array of floating-point numbers that represents a location in multidimensional space.”"
  - id: distance
    prompt: "Which distance functions does the chapter mention for comparing embeddings?"
    type: multiple
    options:
      - id: of65964
        text: "Euclidean distance"
        correct: true
      - id: o745e66
        text: "Cosine similarity"
        correct: true
      - id: obb1110
        text: "Levenshtein distance"
      - id: oe057d0
        text: "Hamming distance"
    explanation: "“Search engines use distance functions such as cosine similarity or Euclidean distance to measure the distance between vectors”."
  - id: why-not-rtree
    prompt: "Why aren't R-trees used for vector search?"
    options:
      - id: o64198e
        text: "They require GPUs"
      - id: o2a8503
        text: "They are too new"
      - id: of4bc0c
        text: "They don't work well for vectors with many dimensions"
        correct: true
      - id: oa8a667
        text: "They only work for text"
    explanation: "“Since the R-trees we saw previously don’t work well for vectors with many dimensions, specialized vector indexes are used”."
  - id: flat-ivf-hnsw
    prompt: "Which vector index is exact but slow?"
    options:
      - id: o993ff3
        text: "IVF index"
      - id: o33520c
        text: "Bloom filter"
      - id: oe58428
        text: "Flat index"
        correct: true
      - id: o903684
        text: "HNSW index"
    explanation: "“Flat indexes are accurate, but measuring the distance between the query and each vector is slow.” IVF and HNSW are approximate."
  - id: ivf-probes
    prompt: "On an IVF index, what happens if you increase the number of probes?"
    options:
      - id: ofed01b
        text: "More partitions are checked: more accurate but slower"
        correct: true
      - id: o308782
        text: "Results become exact always"
      - id: o8b7215
        text: "Fewer partitions are checked: faster but less accurate"
      - id: oac267a
        text: "The index is rebuilt"
    explanation: "“Queries that use more probes will be more accurate but slower, as more vectors must be compared.”"
  - id: ivf-miss
    prompt: "Why can an IVF index miss a close match?"
    options:
      - id: o55676f
        text: "The query and a nearby document can fall into different partitions"
        correct: true
      - id: od610da
        text: "IVF uses edit distance"
      - id: oaba07f
        text: "IVF ignores vector length"
      - id: oc643a6
        text: "IVF only stores half the vectors"
    explanation: "“the query and a document may fall into different partitions, even though they are close to each other.”"
  - id: hnsw
    prompt: "How does an HNSW query proceed?"
    options:
      - id: of82c7a
        text: "Hash the query into a bucket"
      - id: o0c9335
        text: "Start at the sparse top layer, find the nearest node, then descend to denser layers refining the search"
        correct: true
      - id: oa6579a
        text: "Binary-search sorted vectors"
      - id: o74b27e
        text: "Scan every vector in order"
    explanation: "“A query starts by locating the nearest vector in the topmost layer, which has a small number of nodes. The query then moves to the same node in the layer below and follows the edges in that layer, which is more densely connected”."
  - id: summary-oltp-olap
    prompt: "According to the summary, what do analytical systems use to minimize CPU time?"
    options:
      - id: o7327bf
        text: "In-memory hash maps"
      - id: of2f0f3
        text: "Bloom filters"
      - id: o9cdc09
        text: "B-tree indexes on every column"
      - id: of0c940
        text: "JIT compilation of queries or vectorization"
        correct: true
    explanation: "Analytical systems use columnar layout with compression “and JIT compilation of queries or vectorization to minimize the amount of CPU time spent processing the data.”"
---
Beyond one-key lookups: concatenated indexes, spatial indexes, inverted indexes and vector indexes for
semantic search.

## Learning objectives

- Explain what a concatenated index can and cannot answer
- Explain why 2D range queries need spatial indexes
- Describe inverted indexes, n-grams and edit-distance search
- Explain embeddings, distance functions, and flat, IVF and HNSW indexes

## In a nutshell

- (lastname, firstname) helps last-name queries, not first-name queries.
- R-trees, Bkd-trees, space-filling curves and grids handle 2D/3D ranges.
- Full-text search: term → postings list; AND = bitmap intersection.
- Vector search: embeddings near each other mean similar meaning; HNSW and IVF are approximate.

## Connections to today

- **pgvector and RAG.** Retrieval-augmented generation commonly stores document embeddings in pgvector,
  OpenSearch or a dedicated vector database and searches with HNSW.
- **Hybrid search** combines BM25-style inverted-index scores with vector similarity — both indexes from
  this section, used together.
- **H3 hexagonal grids** (Uber) are the "regularly spaced grids of hexagons" the chapter mentions.
