---
title: "Eliminating Subtours (pp. 113–118)"
questions:
  - id: moat
    prompt: >-
      When control zones bump into each other and leave a gap around a cluster, Jünger and Pulleyblank
      add a *moat*. Why does a moat of width w add 2w to the bound?
    explanation: >-
      “Any tour must at some time visit the cluster, so a salesman must cross over the strip at least
      twice, once on the way in and once on the way out. We can thus add twice the smallest width of the
      strip to our bound” (p. 113). Moats and zones together often give very strong bounds. A 100-city
      packing proves a tour is within 0.65% of optimal (p. 114). In dual terms a moat is the multiplier on
      a subtour inequality.
    options:
      - text: "It adds only w"
      - text: "Because water slows the salesman"
      - text: "Every tour must cross into and out of the enclosed cluster, so it crosses the strip at least twice"
        correct: true
      - text: "Because moats double each radius"
  - id: two-triangles
    prompt: >-
      On a six-city instance with two distant clusters, the degree LP returns two triangles. Which
      constraint forbids that?
    explanation: >-
      “Any tour must include at least two of the edges crossing the gap between the clusters”, so for
      S = {1, 2, 6}: x₁₃ + x₁₄ + x₁₅ + x₂₃ + x₂₄ + x₂₅ + x₆₃ + x₆₄ + x₆₅ ≥ 2 (p. 115). This is a **subtour
      inequality**: for any proper subset S, the edges with exactly one end in S must sum to at least 2
      (p. 116). Two disjoint triangles have zero crossing edges, which violates it.
    options:
      - text: "An upper bound xᵢⱼ ≤ 1"
      - text: "An integrality constraint"
      - text: "A degree constraint"
      - text: "A subtour inequality: edges leaving S = {1, 2, 6} must sum to ≥ 2"
        correct: true
  - id: subtour-count
    prompt: >-
      How many subtour inequalities are there for an n-city TSP, and why is that a problem?
    explanation: >-
      There is one for every proper subset S (S and its complement give the same inequality), roughly
      2ⁿ⁻¹, “approximately 2ⁿ/²” in the book’s IP discussion (p. 124). The 42-city USA instance has
      2,199,023,254,648 (p. 127). That is far too many to hand to an LP solver at once, which motivates
      the cutting-plane method: generate only the ones that are violated (chapter 6).
    options:
      - text: "n, one per city"
      - text: "Exponentially many (one per subset), so they cannot all be listed — they must be generated on demand"
        correct: true
      - text: "n², one per edge"
      - text: "Exactly 42"
  - id: subtour-bound-quality
    prompt: >-
      How good is the subtour LP bound in practice?
    explanation: >-
      “The subtour-relaxation bound is almost always within 1% of the length of an optimal tour for
      randomly generated geometric instances. In the specific case of the 42-city USA data set, the bound
      is 697 units, compared with the optimal tour length of 699 units” (pp. 116–117), a gap of 0.3%.
      That strength “is a key ingredient in the success of the entire LP approach” (p. 116). Weak
      relaxations give huge branch-and-bound trees, and strong ones prune quickly.
    options:
      - text: "Usually within 50%"
      - text: "Usually within about 1% on random geometric instances; 697 vs 699 for USA42"
        correct: true
      - text: "Always exactly optimal"
      - text: "Worse than the MST bound"
  - id: four-thirds
    prompt: >-
      What is the “4/3rds Conjecture”?
    explanation: >-
      For metric instances, the optimal tour is known to be at most 3/2 times the subtour bound. A known
      family approaches a ratio of 4/3. The conjecture is that 4/3 is the worst possible: OPT ≤ (4/3) ×
      subtour bound (p. 117). Sylvia Boyd and colleagues verified it for all instances with at most ten
      cities. Proving it would “almost certainly require a deep understanding of the structure” of subtour
      LP solutions and could yield new cutting planes (p. 117). It remains open (chapter 13).
    options:
      - text: "Christofides tours are at most 4/3 of optimal"
      - text: "On metric instances, optimal tour ≤ (4/3) × subtour LP bound (the known worst case approaches 4/3; proven bound 3/2)"
        correct: true
      - text: "The TSP constant equals 4/3"
      - text: "Subtour inequalities number 4/3·n"
  - id: integrality-gap
    prompt: >-
      The 4/3rds Conjecture concerns the *integrality gap* of the subtour relaxation. Which statements
      are correct?
    type: multiple
    explanation: >-
      The integrality gap is the worst-case ratio of the integer optimum (best tour) to the relaxation
      optimum, over all instances in a class. For metric TSP and the subtour LP, it is known to lie
      between 4/3 (a lower-bound family) and 3/2 (a proven upper bound) (p. 117). It is a worst-case
      statement, since typical random instances are within about 1% (p. 116). A smaller gap is what lets
      bounds certify near-optimality and prune search.
    options:
      - text: "It is the worst-case ratio of optimal tour to LP bound over instances"
        correct: true
      - text: "For metric TSP and the subtour LP it is known to be between 4/3 and 3/2"
        correct: true
      - text: "It is about 1% on typical random geometric instances, as a ratio"
        correct: true
      - text: "It is proven to equal 4/3"
---
## Learning objectives

- State subtour inequalities and see them as moats.
- Know the quality of the subtour bound and the 4/3rds Conjecture.

## Moats and subtours

Zones often bump together and leave gaps. A **moat** around a cluster must be crossed twice by every tour, which adds 2 × width to the bound (pp. 113–114).

Algebraically, a **subtour inequality** for a proper subset S of cities:

> Σ { xᵢⱼ : i ∈ S, j ∉ S } ≥ 2

It forbids islands (subtours) in LP solutions (pp. 115–116). The **subtour LP** is the degree constraints plus all subtour inequalities. There are about 2ⁿ of them, 2.2 trillion for 42 cities (p. 127), so they must be generated on demand (chapter 6).

## How strong is it?

- Random geometric instances: usually within 1%. USA42: 697 vs 699 (pp. 116–117).
- Metric worst case: OPT ≤ 1.5 × subtour bound (proven). Instances near 4/3 exist. **4/3rds Conjecture**: 4/3 is tight. Verified for n ≤ 10 (Boyd) (p. 117).

## In software terms

A relaxation is only as useful as its integrality gap. Before running a big branch-and-bound, measure the gap between the best solution and the LP bound on sample instances. A large gap means you should add constraints (cuts) before scaling out compute.

## Sources

- Cook (2012), pp. 113–118.
- Wolsey, L. A. “Heuristic analysis, linear programming and branch and bound.” *Math. Programming Study* 13 (1980), 121–134 (the 3/2 bound).
- Benoit, G., S. Boyd. “Finding the exact integrality gap for small traveling salesman problems.” *Math. Oper. Res.* 33 (2008), 921–931.
- Geodual software (University of Cologne), referenced on p. 114.
