---
title: "Foundations Check — Before Chapter 3"
questions:
  - id: inner-join
    prompt: "`users(id, name)` has 3 rows; `posts(id, user_id)` has 5 rows, all with valid `user_id`s, and one user has no posts. How many rows does `SELECT * FROM users JOIN posts ON posts.user_id = users.id` return?"
    options:
      - id: oc9c3ba
        text: "3"
      - id: o89edb1
        text: "5"
        correct: true
      - id: o634c91
        text: "15"
      - id: oe01c06
        text: "8"
    explanation: "An inner join produces one row per matching pair. Every post matches exactly one user; the user without posts produces no row."
  - id: left-join
    prompt: "Which join returns users even if they have no posts?"
    options:
      - id: o9eee67
        text: "`posts JOIN users ON posts.user_id = users.id`"
      - id: oa6ef1c
        text: "`users INNER JOIN posts ON posts.user_id = users.id`"
      - id: o0c4ee3
        text: "`users CROSS JOIN posts`"
      - id: obb3dba
        text: "`users LEFT JOIN posts ON posts.user_id = users.id`"
        correct: true
    explanation: "A left outer join keeps every row from the left table and fills the right side with NULLs when there is no match."
  - id: json-types
    prompt: "Which is not a native JSON value type?"
    options:
      - id: o912e48
        text: "Boolean"
      - id: o9fad7d
        text: "Array"
      - id: ocd1497
        text: "Object"
      - id: oa992d9
        text: "Date"
        correct: true
    explanation: "JSON has strings, numbers, booleans, null, arrays and objects. Dates must be encoded, typically as strings — one of the encoding pitfalls Chapter 5 revisits."
  - id: graph-directed
    prompt: "In a directed graph, an edge from A to B means what?"
    options:
      - id: o538b48
        text: "A and B have the same properties"
      - id: od932b5
        text: "You can traverse from A to B along that edge, but the edge itself does not lead from B to A"
        correct: true
      - id: ob8faf9
        text: "A and B are the same vertex"
      - id: o8e7996
        text: "There is necessarily also an edge from B to A"
    explanation: "Directed edges have a tail and a head. The book's property-graph model indexes both ends so you can traverse in either direction efficiently."
  - id: recursive-cte
    prompt: "What does `WITH RECURSIVE` in SQL allow?"
    options:
      - id: o7dfb10
        text: "Calling a stored procedure in a loop"
      - id: o8c7605
        text: "Creating a temporary index"
      - id: oc0a112
        text: "Running the same query on several servers"
      - id: o7f6526
        text: "A query that references its own result to iterate until no new rows appear, e.g., walking a hierarchy of unknown depth"
        correct: true
    explanation: "A recursive CTE has a base case and a recursive step, unioned until a fixed point. The chapter uses it to traverse a location hierarchy."
  - id: third-normal
    prompt: "A table stores `order_id, customer_id, customer_city`. Why does this violate typical normalization?"
    options:
      - id: o20222d
        text: "It has too few columns"
      - id: oda6da8
        text: "`customer_city` depends on `customer_id`, not on the order, so it is duplicated across a customer's orders"
        correct: true
      - id: o06cefd
        text: "Orders cannot reference customers"
      - id: o39acbd
        text: "Cities must be stored as numbers"
    explanation: "A non-key attribute depending on another non-key attribute (a transitive dependency) creates redundant copies that can disagree."
  - id: bfs-dfs
    prompt: "Which traversal finds the shortest path (fewest edges) in an unweighted graph?"
    options:
      - id: o898d14
        text: "Breadth-first search"
        correct: true
      - id: obe20a0
        text: "Depth-first search"
      - id: o311995
        text: "Binary search"
      - id: ob72457
        text: "Sorting the edges"
    explanation: "BFS explores layer by layer, so the first time it reaches a vertex is via a shortest path. The chapter notes graph queries must choose between breadth-first and depth-first traversal."
  - id: declarative-vs-imperative
    prompt: "Which is declarative?"
    options:
      - id: of0a079
        text: "A Python `for` loop that appends matching users to a list"
      - id: o127e9d
        text: "A shell script that greps a file line by line"
      - id: oabc320
        text: "`SELECT name FROM users WHERE age > 30`"
        correct: true
      - id: oc093ac
        text: "A C function that walks a linked list"
    explanation: "A declarative statement describes the result wanted; the engine chooses how to compute it."
---
Chapter 3 assumes comfort with SQL joins, JSON, basic graph vocabulary and recursion. These questions
are not drawn from the book's text.

## Learning objectives

- Read join and recursive SQL
- Recognize JSON structure and graph terminology
- Recall normal-form basics
