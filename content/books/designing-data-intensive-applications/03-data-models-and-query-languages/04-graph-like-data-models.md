---
title: "Graph-Like Data Models"
questions:
  - id: graph-when
    prompt: "Your data has many-to-many relationships everywhere and queries follow chains of relationships of unknown length. Which model fits best?"
    options:
      - id: oc4ae0d
        text: "A graph model"
        correct: true
      - id: ofe182b
        text: "A DataFrame"
      - id: obf8614
        text: "A star schema"
      - id: o5c73a8
        text: "A document model"
    explanation: "“as the connections within your data become more complex, it becomes more natural to start modeling that data as a graph.”"
  - id: adjacency
    prompt: "Which representation does the chapter say suits graph traversals, and which suits machine learning?"
    options:
      - id: o967ea7
        text: "Adjacency lists for traversals; adjacency matrices for ML"
        correct: true
      - id: oa0ff74
        text: "Both suit only traversals"
      - id: o304e1c
        text: "Matrices for traversals; lists for ML"
      - id: oec6c1a
        text: "Neither is used in practice"
    explanation: "“An adjacency list is good for graph traversals, and matrices are good for machine learning”."
  - id: heterogeneous
    prompt: "What does Facebook's graph illustrate?"
    options:
      - id: oc42359
        text: "Graphs cannot store events"
      - id: o05219d
        text: "Graphs are only for road networks"
      - id: ob72e6c
        text: "Graphs can store completely different types of objects consistently in one database"
        correct: true
      - id: o49c8eb
        text: "Graphs must be homogeneous"
    explanation: "“An equally powerful use of graphs is to provide a consistent way of storing completely different types of objects in a single database.” Facebook's vertices include “people, locations, events, check-ins, and comments”."
  - id: property-graph-parts
    prompt: "Which belong to an edge in the property graph model?"
    type: multiple
    options:
      - id: o53b95e
        text: "A list of all vertices in the graph"
      - id: o1bd514
        text: "A unique identifier"
        correct: true
      - id: od60fc1
        text: "A collection of properties"
        correct: true
      - id: o465f66
        text: "Tail and head vertices"
        correct: true
      - id: o39ad2e
        text: "A label describing the relationship"
        correct: true
    explanation: "“Each edge consists of the following: A unique identifier, The vertex at which the edge starts (the tail vertex), The vertex at which the edge ends (the head vertex), A label ... A collection of properties”."
  - id: two-indexes
    prompt: "Why does the relational representation of a property graph index both `tail_vertex` and `head_vertex`?"
    options:
      - id: o9e6582
        text: "To enforce uniqueness of edges"
      - id: od4cf00
        text: "To sort edges by label"
      - id: o37071f
        text: "Because PostgreSQL requires two indexes per table"
      - id: o57d548
        text: "To find both incoming and outgoing edges efficiently and traverse forward and backward"
        correct: true
    explanation: "“Given any vertex, you can efficiently find both its incoming and its outgoing edges and thus traverse the graph ... both forward and backward. (That’s why Example 3-3 has indexes on both the tail_vertex and head_vertex columns.)”"
  - id: graph-limitation
    prompt: "What can a relational join table express that a single graph edge cannot?"
    options:
      - id: odd4894
        text: "Three-way or higher-degree relationships in one row"
        correct: true
      - id: oadabbe
        text: "Directional relationships"
      - id: oe663de
        text: "Properties on relationships"
      - id: o7d2790
        text: "Relationships between different types of objects"
    explanation: "“an edge can associate only two vertices with each other, whereas a relational join table can represent three-way or even higher-degree relationships”. Workarounds include an extra vertex per row or a hypergraph."
  - id: graph-evolvability
    prompt: "Why does the chapter call graphs good for evolvability?"
    options:
      - id: oe5f1d9
        text: "The graph can easily be extended as application data structures change"
        correct: true
      - id: o1939e9
        text: "They avoid migrations by storing everything as text"
      - id: o0267f9
        text: "They need no indexes"
      - id: obbd39e
        text: "They enforce strict schemas"
    explanation: "“Graphs are good for evolvability: as you add features to your application, a graph can easily be extended to accommodate changes in the application’s data structures.”"
  - id: within-star
    prompt: "What does `-[:WITHIN*0..]->` mean in Cypher?"
    options:
      - id: o40d3d0
        text: "Match any edge label"
      - id: ocfbc2e
        text: "Follow exactly one WITHIN edge"
      - id: o873b49
        text: "Follow a WITHIN edge zero or more times"
        correct: true
      - id: o06768b
        text: "Exclude WITHIN edges"
    explanation: "“:WITHIN*0.. expresses that fact very concisely: it means “follow a WITHIN edge, zero or more times.” It’s like the * operator in a regular expression.”"
  - id: execution-direction
    prompt: "For the emigrant query, why might the database start from the \"United States\" and \"Europe\" vertices instead of scanning all people?"
    options:
      - id: od3bc34
        text: "Because WITHIN edges only point outward"
      - id: o5b7af5
        text: "With an index on name, it can find those two vertices quickly and traverse incoming edges backward"
        correct: true
      - id: of2a3b8
        text: "Because scanning people is impossible"
      - id: o92ee3e
        text: "Because Cypher requires starting from Location vertices"
    explanation: "“If there is an index on the name property, you can efficiently find the two vertices representing the US and Europe. Then you can proceed to find all locations ... by following all incoming WITHIN edges.” This choice is possible because Cypher is declarative."
  - id: sql-graph-hard
    prompt: "Why are graph queries awkward in SQL?"
    options:
      - id: of509d2
        text: "SQL cannot join a table to itself"
      - id: o15a104
        text: "Relational databases cannot store edges"
      - id: o38a8ed
        text: "The number of joins needed for a variable-length traversal is not known in advance"
        correct: true
      - id: o6176d4
        text: "SQL has no indexes on foreign keys"
    explanation: "“in a graph query, you may need to traverse a variable number of edges before you find the vertex you’re looking for—that is, the number of joins is not fixed in advance.”"
  - id: lines
    prompt: "How much longer was the SQL version of the emigrant query compared with Cypher?"
    options:
      - id: o8dfdb8
        text: "100 lines versus 1"
      - id: ob9d521
        text: "31 lines versus 4"
        correct: true
      - id: oa91ea3
        text: "8 lines versus 4"
      - id: o094f30
        text: "About the same"
    explanation: "“The fact that a 4-line Cypher query requires 31 lines in SQL shows how much of a difference the right choice of data model and query language can make.”"
  - id: gql
    prompt: "What is GQL?"
    options:
      - id: o3d25f6
        text: "A SQL dialect for JSON"
      - id: of810fe
        text: "Google's internal name for GraphQL"
      - id: o55f733
        text: "An ISO standard graph query language based on Cypher, published in 2024"
        correct: true
      - id: oab04c9
        text: "A Datalog variant"
    explanation: "“The Graph Query Language (GQL) ISO standard, which is based on Cypher, was published in 2024.”"
  - id: triple
    prompt: "In the triple (lucy, marriedTo, alain), what is `marriedTo`?"
    options:
      - id: oefb118
        text: "The subject"
      - id: oc87baf
        text: "A property value"
      - id: oedda12
        text: "A vertex"
      - id: o97b178
        text: "The predicate, equivalent to an edge label between two vertices"
        correct: true
    explanation: "“In that case, the predicate is an edge in the graph, the subject is the tail vertex, and the object is the head vertex. For example, in (lucy, marriedTo, alain) ... the predicate marriedTo is the label of the edge”."
  - id: triple-property
    prompt: "In (lucy, birthYear, 1989), how does the triple map to a property graph?"
    options:
      - id: o4f5fed
        text: "A property `birthYear: 1989` on the vertex lucy"
        correct: true
      - id: o5cf426
        text: "An edge from lucy to a vertex 1989"
      - id: o2facd3
        text: "It cannot be mapped"
      - id: od62064
        text: "A new vertex called birthYear"
    explanation: "When the object is a primitive value, “the predicate and object of the triple are equivalent to the key and value of a property on the subject vertex.”"
  - id: rdf-uris
    prompt: "Why does RDF use URIs for predicates?"
    options:
      - id: o6fae8b
        text: "To make every predicate a clickable web page"
      - id: o28879c
        text: "So data from different sources can be combined without conflicts over what words like \"within\" mean"
        correct: true
      - id: o658019
        text: "Because SPARQL requires HTTP"
      - id: o357485
        text: "Because URIs are shorter than strings"
    explanation: "“The reasoning behind this design is that you should be able to combine your data with someone else’s data, and if they attach a different meaning to the word within or lives_in, you won’t get a conflict”."
  - id: semantic-web-legacy
    prompt: "Which are cited as living legacies of the Semantic Web?"
    type: multiple
    options:
      - id: od3a835
        text: "GraphQL"
      - id: oe4955a
        text: "Schema.org vocabularies"
        correct: true
      - id: o3ccd36
        text: "JSON-LD"
        correct: true
      - id: o8e505e
        text: "Wikidata knowledge graph"
        correct: true
    explanation: "“the project’s legacy lives on in, for example, linked data standards such as JSON-LD, the ontologies used in biomedical science, Facebook’s Open Graph protocol ... knowledge graphs such as Wikidata, and the standardized vocabularies for structured data maintained by Schema.org.”"
  - id: sparql-cypher
    prompt: "How are SPARQL and Cypher related historically?"
    options:
      - id: of044bd
        text: "Cypher predates SPARQL"
      - id: o7866a9
        text: "SPARQL is a subset of Cypher"
      - id: o656c40
        text: "They are unrelated"
      - id: o75a33e
        text: "SPARQL predates Cypher, and Cypher borrowed its pattern matching from SPARQL"
        correct: true
    explanation: "SPARQL “predates Cypher, and since Cypher’s pattern matching is borrowed from SPARQL, they look quite similar.”"
  - id: datalog-model
    prompt: "What data model is Datalog based on, and why is it in the graph section?"
    options:
      - id: ocbc8e8
        text: "The relational model; recursive queries on graphs are a particular strength"
        correct: true
      - id: o88f740
        text: "The document model; it handles nested JSON"
      - id: odda6e3
        text: "Key-value; it is fast for lookups"
      - id: o3f4f9b
        text: "The graph model only"
    explanation: "“It’s based on a relational data model, not a graph, but we discuss it here because recursive queries on graphs are a particular strength of Datalog.”"
  - id: datalog-rules
    prompt: "What do Datalog rules define?"
    options:
      - id: oa352bc
        text: "Indexes on facts"
      - id: o8d4520
        text: "Stored procedures that mutate facts"
      - id: o84f999
        text: "Virtual tables derived from facts, like SQL views, which can refer to other rules and to themselves"
        correct: true
      - id: o9eca61
        text: "Constraints that reject facts"
    explanation: "“We define rules that derive new virtual tables from the underlying facts. These derived tables are like (virtual) SQL views” and “Just as functions can be recursive, Datalog rules can also invoke themselves”."
  - id: datalog-apply
    prompt: "In Example 3-12, which facts let rule 2 derive `within_recursive(3, \"North America\")`?"
    options:
      - id: o085315
        text: "`born_in(100, 3)`"
      - id: o467ee3
        text: "`within(2, 1)` alone"
      - id: oc0b172
        text: "`location(3, \"Idaho\", \"state\")` alone"
      - id: o239484
        text: "`within(3, 2)` plus the earlier derived `within_recursive(2, \"North America\")`"
        correct: true
    explanation: "“within(3, 2) exists in the database and the previous step generated within_recursive(2, \"North America\"), so rule 2 applies. It generates within_recursive(3, \"North America\").”"
  - id: graphql-purpose
    prompt: "What is GraphQL designed for?"
    options:
      - id: o65bd25
        text: "Replacing SQL inside databases"
      - id: od2a371
        text: "Letting client software request a JSON document shaped for rendering its UI, for OLTP queries"
        correct: true
      - id: o416498
        text: "Recursive graph traversals"
      - id: o1db8fe
        text: "Running analytical queries over petabytes"
    explanation: "“It’s intended for OLTP queries; its purpose is to allow client software running on a user’s device ... to request a JSON document with a particular structure, containing the fields necessary for rendering its UI.”"
  - id: graphql-restrictions
    prompt: "Why does GraphQL forbid recursive queries and arbitrary search conditions?"
    options:
      - id: o7e7fe5
        text: "Recursion is impossible over HTTP"
      - id: off6bb6
        text: "Queries come from untrusted clients, and expensive queries could cause denial of service"
        correct: true
      - id: oec9a6d
        text: "JSON cannot represent recursive results"
      - id: o698218
        text: "GraphQL servers lack CPUs"
    explanation: "“The language is also intentionally limited, because GraphQL queries come from untrusted sources. It does not allow anything that could be expensive to execute, since otherwise users could ... cause a denial-of-service condition”."
  - id: graphql-dup
    prompt: "Why does a GraphQL response duplicate a sender's name in every message?"
    options:
      - id: o2d50dd
        text: "GraphQL cannot express references"
      - id: o16c195
        text: "GraphQL accepts larger responses to make UI rendering simpler"
        correct: true
      - id: o49bdab
        text: "The database stores it duplicated"
      - id: ocde268
        text: "It is a bug in the example"
    explanation: "“GraphQL makes the design choice to accept a larger response size in order to make it simpler to render the UI based on the requested data.”"
  - id: graphql-joins
    prompt: "Which joins can a GraphQL client request?"
    options:
      - id: o8c91b2
        text: "Any join the database supports"
      - id: ob1509e
        text: "Only recursive joins"
      - id: o48d7a3
        text: "Only those explicitly declared in the GraphQL schema"
        correct: true
      - id: o5877fd
        text: "None"
    explanation: "“However, only joins that have been explicitly declared in the GraphQL schema can be requested by the client.”"
  - id: graphql-backend
    prompt: "On what kind of database can GraphQL be implemented?"
    options:
      - id: o56b07b
        text: "Only document databases"
      - id: o3ecf9c
        text: "Only graph databases"
      - id: o3bd7a7
        text: "Any — relational, document or graph"
        correct: true
      - id: o127f46
        text: "Only databases with native JSON"
    explanation: "“GraphQL can be implemented on top of any type of database—relational, document, or graph.”"
  - id: graphql-cost
    prompt: "Which costs come with adopting GraphQL?"
    type: multiple
    options:
      - id: o202e42
        text: "Servers must store denormalized data"
      - id: oba6244
        text: "Authorization, rate limiting and performance challenges"
        correct: true
      - id: o6866f8
        text: "Clients can no longer choose fields"
      - id: o93b638
        text: "Tooling to translate queries into REST or gRPC calls to internal services"
        correct: true
    explanation: "“Organizations that adopt GraphQL often need tooling to convert the queries into requests to internal services ... Authorization, rate limiting, and performance challenges are additional concerns”."
---
When many-to-many relationships dominate, graphs become natural. The section covers property graphs,
Cypher, recursive SQL, triple stores and SPARQL, Datalog, and the deliberately limited GraphQL.

## Learning objectives

- Describe property graphs and triple stores and how they map to each other
- Explain why variable-length traversals are awkward in SQL
- Read a simple Datalog rule set and explain rule recursion
- Explain GraphQL's design choices and limits

## In a nutshell

- Graph vertices and edges can be homogeneous (a road network) or heterogeneous (Facebook's graph).
- Property graph ≈ two tables (vertices, edges) indexed on both edge ends.
- `-[:WITHIN*0..]->` in Cypher becomes a 31-line recursive CTE in SQL.
- GQL, an ISO standard based on Cypher, was published in 2024.
- GraphQL is for clients requesting UI-shaped JSON; no recursion, no arbitrary search.

## Connections to today

- **SQL/PGQ** (property graph queries in SQL:2023) and Postgres extensions such as Apache AGE bring graph
  patterns into relational databases.
- **Knowledge graphs for RAG.** "GraphRAG" approaches extract entities and relationships from documents
  into graphs to help LLMs answer multi-hop questions.
- **GraphQL federation** in large companies is the "tooling to convert queries into requests to internal
  services" the chapter mentions.
