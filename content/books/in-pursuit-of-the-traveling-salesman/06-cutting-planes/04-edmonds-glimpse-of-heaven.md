---
title: "Edmonds's Glimpse of Heaven (pp. 142–144)"
questions:
  - id: perfect-matching
    prompt: >-
      What is the minimum-cost perfect matching problem?
    explanation: >-
      “A perfect matching in a graph is a set of edges that pair up the vertices, that is, each vertex is
      the end of exactly one edge … the problem is to find a perfect matching such that the total cost of
      the edges in the matching is as small as possible” (pp. 142–143). Examples include pairing engineers
      for reviews or pairing servers for replication with minimum cross-rack cost. It is also step 3 of
      Christofides.
    options:
      - text: "Pair up all vertices with disjoint edges of minimum total cost"
        correct: true
      - text: "Match each vertex to its nearest neighbor"
      - text: "Find a spanning tree"
      - text: "Find a tour of minimum cost"
  - id: blossom
    prompt: >-
      What are Edmonds's blossom inequalities?
    explanation: >-
      “His blossom inequalities capture the fact that a graph with an odd number of vertices cannot have a
      perfect matching. Thus, for any odd cluster of vertices, a perfect matching must contain at least one
      edge connecting the cluster to the remainder … The corresponding linear inequality has the form of a
      subtour-elimination constraint, but this time we ask only that the sum of variables be at least one”
      (p. 143). Formally, x(δ(S)) ≥ 1 for every odd set S, alongside degree constraints x(δ(v)) = 1.
    options:
      - text: "x(δ(S)) = 0 for even S"
      - text: "x(δ(S)) ≥ 2 for every set S"
      - text: "x(δ(S)) ≥ 1 for every odd-size set S"
        correct: true
      - text: "Each edge at most 1/2"
  - id: matching-polytope
    prompt: >-
      Why was Edmonds's result a “glimpse of heaven”?
    explanation: >-
      He proved that degree constraints plus *all* blossom inequalities describe exactly the convex hull of
      perfect matchings, and used LP duality to get a polynomial-time algorithm without step-by-step
      cutting planes (p. 143). In his words: “Here is a solved integer program. It was my first glimpse of
      heaven” (p. 143). For matchings there is a complete, understandable polyhedral description plus an
      efficient algorithm, which is exactly what we lack for tours.
    options:
      - text: "He gave a complete linear description of the matching polytope and a polynomial algorithm from it"
        correct: true
      - text: "He found a fast heuristic"
      - text: "He proved P ≠ NP"
      - text: "He solved the TSP"
  - id: separation-optimization
    prompt: >-
      What is the Grötschel–Lovász–Schrijver equivalence of separation and optimization?
    explanation: >-
      Khachiyan’s ellipsoid method “can be executed without an explicit list of the constraints … as long as
      a separation routine is available. Thus, subject to a few technical conditions … a good separation
      algorithm yields a good optimization algorithm, and, vice versa, a good algorithm for optimization
      yields a good algorithm for separation” (p. 143), proven fully by Grötschel, Lovász and Schrijver in the
      1980s. Exponentially many constraints are not a barrier if you can find a violated one quickly.
    options:
      - text: "Optimization never needs separation"
      - text: "They are unrelated"
      - text: "Separation is harder than optimization"
      - text: "Over a polytope, polynomial separation ⇔ polynomial optimization (via the ellipsoid method)"
        correct: true
  - id: tsp-implication
    prompt: >-
      What does this equivalence imply for anyone trying to solve the TSP in polynomial time?
    explanation: >-
      “It follows that to solve the TSP we must have a characterization of the convex hull of tours, equipped
      with a polynomial-time separation algorithm” (pp. 143–144). Edmonds anticipated this in 1964: finding
      a good TSP algorithm “is undoubtedly equivalent to finding such a characterization” (p. 143). The
      practical goal of faster separation and the $1,000,000 question are the same strategy pushed to its
      limit.
    options:
      - text: "That heuristics are enough"
      - text: "A polynomial TSP algorithm amounts to polynomial separation over the TSP polytope, so practical separation work and the P vs NP question are linked"
        correct: true
      - text: "That the TSP polytope has few facets"
      - text: "Nothing"
---
## Learning objectives

- Understand the matching polytope and blossom inequalities.
- State the separation ⇔ optimization equivalence and what it means for the TSP.

## Summary

- **Perfect matching**: pair up all vertices at minimum cost (pp. 142–143).
- **Blossoms**: for odd S, x(δ(S)) ≥ 1. Degree constraints plus all blossoms give exactly the matching polytope (Edmonds, 1965), and duality gives a polynomial algorithm: “a solved integer program … my first glimpse of heaven” (p. 143).
- **GLS theorem**: with the ellipsoid method, polynomial separation ⇔ polynomial optimization (p. 143).
- **Implication**: a good TSP algorithm amounts to a good separation routine for the TSP polytope (pp. 143–144). Edmonds conjectured that none exists (1967).

| | Matching | TSP |
|---|---|---|
| Degree | x(δ(v)) = 1 | x(δ(v)) = 2 |
| Set constraints | odd S: x(δ(S)) ≥ 1 | all S: x(δ(S)) ≥ 2, + combs, + … |
| Complete description known? | Yes | No |
| Polynomial algorithm? | Yes | Unknown (NP-hard) |

## In software terms

Matching algorithms (blossom, Hungarian for bipartite graphs) are in standard libraries: assignment, pairing, load balancing. When a problem has a known polytope like this, use the polynomial algorithm. When it does not, as with the TSP, you need cuts and branching.

## Sources

- Cook (2012), pp. 142–144.
- Edmonds, J. “Maximum matching and a polyhedron with 0,1-vertices.” *J. Res. Nat. Bur. Standards* 69B (1965), 125–130.
- Grötschel, M., L. Lovász, A. Schrijver. *Geometric Algorithms and Combinatorial Optimization*. Springer, 1988 (2nd ed. 1993).
