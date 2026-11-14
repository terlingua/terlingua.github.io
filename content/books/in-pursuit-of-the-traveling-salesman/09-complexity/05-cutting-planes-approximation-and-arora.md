---
title: "State of the TSP: Cutting Planes, Approximation, Arora (pp. 182–184)"
questions:
  - id: xkcd-question
    prompt: >-
      Randall Munroe asked: “What's the complexity class of the best linear programming cutting-plane
      techniques?” What does the book answer?
    explanation: >-
      “It does not appear to be easy to get a handle on the worst-case performance of cutting planes. In 1987,
      Vašek Chvátal, Mark Hartmann, and I showed that a strong variant of branch-and-cut requires at least” an exponential number
      of operations, with n/72 in the exponent, “to solve a specially constructed nasty instance of the
      Hamiltonian-circuit problem … The analysis, however, leaves open the possibility that new classes of cutting planes could result in a
      much better running-time bound” (p. 182). The practical champion has exponential worst cases for the
      known cut families, which says nothing about cut families not yet discovered.
    options:
      - text: "Known strong branch-and-cut variants need exponential time on nasty instances; new cut classes might change this"
        correct: true
      - text: "Cutting planes are undecidable"
      - text: "It is O(n log n)"
      - text: "Cutting planes are polynomial"
  - id: why-metric
    prompt: >-
      Why must approximation results restrict to *metric* instances (symmetric + triangle inequality)?
    explanation: >-
      For general costs, encode Hamiltonian circuit: cost 0 for graph edges and 1 for non-edges. “A
      Hamiltonian circuit has cost 0, and any non-optimal tour has cost at least 1, so a method that returns a
      solution within any fixed percentage of optimality will provide an answer to the yes/no question”
      (p. 220). Any α-approximation for general TSP would decide an NP-complete problem, so none exists unless
      P = NP. With the triangle inequality this trick fails, which is why Christofides needs it (p. 183).
    options:
      - text: "Because non-metric instances are rare"
      - text: "Without the triangle inequality, any constant-factor approximation would solve Hamiltonian circuit (0/1 costs), implying P = NP"
        correct: true
      - text: "Because metric instances are easier to store"
      - text: "Because Christofides required it for speed"
  - id: christofides-open
    prompt: >-
      At publication, what was the status of approximating metric TSP?
    explanation: >-
      “It is a pressing open problem to find a polynomial-time α-approximation algorithm with α less than
      1.5” for all metric instances (p. 183). Christofides (1976) “seemed easy enough. Thirty
      years later, with no improvements in sight, it no longer seems so simple” (p. 183). (Chapter 13: Karlin,
      Klein and Oveis Gharan beat 1.5 by a tiny constant in 2020.)
    options:
      - text: "Exact polynomial algorithms existed"
      - text: "2 was the best known"
      - text: "1.01-approximations were known"
      - text: "Nothing better than 1.5 (Christofides) was known"
        correct: true
  - id: inapprox
    prompt: >-
      What hardness-of-approximation result does the book cite for metric TSP?
    explanation: >-
      “Christos Papadimitriou and Santosh Vempala proved that, unless P = NP, there can be no polynomial-time
      α-approximation algorithm for the metric TSP with α less than 1.0045” (p. 183), exactly 220/219
      (p. 220). “It is not clear where the real computational barrier lies, closer to 1.0045 or closer to 1.5.
      It is disturbing that we cannot narrow this range” (p. 183). (The lower bound was later raised to
      123/122, see chapter 13.)
    options:
      - text: "1.5 is proven optimal"
      - text: "No approximation below 2 unless P = NP"
      - text: "No approximation below about 1.0045 (220/219) unless P = NP; the true threshold lies somewhere in [1.0045, 1.5]"
        correct: true
      - text: "Approximation is impossible"
  - id: arora
    prompt: >-
      What did Arora's theorem (1998) show for the Euclidean TSP?
    explanation: >-
      “No matter what α we choose, as long as α is greater than 1.0, there exists a polynomial-time
      α-approximation algorithm for the Euclidean TSP” (p. 183), a polynomial-time approximation scheme
      (PTAS), found independently by Mitchell. That contrasts with the metric case, which has a hard
      threshold, which Cook calls an interesting aspect, “suggesting the Euclidean TSP may be more tractable than the
      general metric problem” (p. 183). Arora and Mitchell later shared the 2010 Gödel Prize.
    options:
      - text: "Euclidean TSP is in P"
      - text: "For every α > 1 there is a polynomial-time α-approximation (a PTAS) for Euclidean TSP"
        correct: true
      - text: "Euclidean TSP cannot be approximated"
      - text: "Only 1.5-approximation exists"
  - id: arora-practice
    prompt: >-
      Why isn't Arora's scheme used in practical TSP codes?
    explanation: >-
      “The running times of the algorithms increase dramatically as α gets close to 1.0, and experimental
      results have been discouraging. This is a common feature of approximation methods, where very fine
      divisions of space are used to obtain high-quality solutions, at the expense of long search times”
      (p. 184). The time is polynomial in n for fixed ε, but the exponent or constant depends badly on 1/ε,
      while LKH reaches 0.1% quickly in practice. A polynomial guarantee does not by itself make an algorithm
      practical.
    options:
      - text: "It only works on 10 cities"
      - text: "It requires quantum hardware"
      - text: "It is incorrect"
      - text: "Its running time explodes as α → 1, so practical heuristics beat it for real accuracy needs"
        correct: true
  - id: which-guarantee
    prompt: >-
      A logistics team has symmetric road costs that *mostly* satisfy the triangle inequality, with a few
      violations from toll avoidance. Which statement is right?
    explanation: >-
      Christofides’ 1.5 guarantee assumes metric costs (p. 183). Violations void the proof, though it may still
      perform well. One common fix is the *metric closure*: replace cost(i, j) with the shortest-path distance
      through the network, which satisfies the triangle inequality. Each tour edge can then be expanded back
      into its real path. Know your guarantee’s preconditions and engineer the input to meet them.
    options:
      - text: "The guarantee is void unless costs are made metric, e.g., by taking shortest-path (metric closure) costs"
        correct: true
      - text: "No algorithm can be used"
      - text: "Arora's scheme applies"
      - text: "The 1.5 guarantee still holds"
---
## Learning objectives

- State what is known about worst cases for cutting planes.
- Explain why approximation needs metric costs.
- Place Christofides, Papadimitriou–Vempala and Arora on one map.

## Summary

- **Cutting planes:** an exponential lower bound (exponent of order n/72) for a strong branch-and-cut on nasty Hamiltonian instances. New cut classes might do better (p. 182).
- **Approximation needs metric costs.** Otherwise a 0/1 encoding of Hamiltonian circuit makes any constant factor NP-hard (pp. 183, 220).

| Setting | Upper (best algorithm) | Lower (unless P = NP) |
|---|---|---|
| General costs | none | no constant factor |
| Metric | 1.5 (Christofides) at publication | 1.0045 (220/219) at publication |
| Euclidean | 1 + ε for any ε (Arora, Mitchell) | — (PTAS exists) |

*(pp. 182–184. Chapter 13 updates the metric row.)*

- A PTAS is not necessarily practical. Running times explode as ε → 0 (p. 184).

## In software terms

Guarantees come with preconditions. Precompute the metric closure, or validate the triangle inequality on samples, before relying on approximation claims. In practice, combine a guaranteed method as a fallback with a strong heuristic, and report the LP bound.

## Sources

- Cook (2012), pp. 182–184, 220.
- Chvátal, V., W. Cook, M. Hartmann. “On cutting-plane proofs in combinatorial optimization.” *Linear Algebra Appl.* 114/115 (1989), 455–499.
- Papadimitriou, C. H., S. Vempala. “On the approximability of the traveling salesman problem.” *Combinatorica* 26 (2006), 101–120.
- Arora, S. “Polynomial time approximation schemes for Euclidean traveling salesman and other geometric problems.” *J. ACM* 45 (1998), 753–782.
- Mitchell, J. S. B. “Guillotine subdivisions approximate polygonal subdivisions.” *SIAM J. Computing* 28 (1999), 1298–1309.
