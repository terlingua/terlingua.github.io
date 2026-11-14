---
title: "The Degree LP Relaxation of the TSP (pp. 108–113)"
questions:
  - id: edge-variables
    prompt: >-
      In the LP model of a symmetric TSP, what are the variables, and how many are there for 24 cities?
    explanation: >-
      One variable xᵢⱼ per pair of cities, “the edges assigned value one are those that are in the tour”
      (p. 108). There are n(n − 1)/2 of them: 15 for six cities and 276 for 24 (p. 108). The objective
      Σ cᵢⱼxᵢⱼ is the tour cost, “since cᵢⱼ multiplied by xᵢⱼ has the value cᵢⱼ if the edge is in the tour
      and the value zero otherwise” (p. 109). Tours are encoded as 0/1 vectors, one coordinate per edge.
    options:
      - text: "One per permutation; 24!"
      - text: "One per city pair and direction; 552"
      - text: "One per city; 24"
      - text: "One per pair of cities (n(n − 1)/2); 276"
        correct: true
  - id: degree-constraint
    prompt: >-
      What is the degree constraint, and who introduced it?
    explanation: >-
      Julia Robinson observed that “every vertex of the graph meets exactly two edges in any tour … if we
      add up the xᵢⱼ values for all edges meeting a given vertex, then the sum must be exactly two” (p. 110).
      For vertex 1 of a six-city instance: x₁₂ + x₁₃ + x₁₄ + x₁₅ + x₁₆ = 2. Together with xᵢⱼ ≥ 0 this
      is the **degree LP relaxation**. Robinson’s 1949 work was “the first LP-based method for the TSP”
      (p. 108).
    options:
      - text: "Edge values at each vertex sum to exactly 2; Julia Robinson"
        correct: true
      - text: "Total edges equal n; Menger"
      - text: "No edge exceeds cost 2; Euler"
      - text: "Each city has exactly one outgoing edge; Dantzig"
  - id: relaxation-bound
    prompt: >-
      Why is the optimal value of the degree LP never greater than the optimal tour cost?
    explanation: >-
      “Every tour is an allowable solution to the LP problem, so the optimal LP objective can never be
      greater than the cost of an optimal tour. This is the most important point to understand. In the LP
      problem we optimize over a larger set of allowable solutions and we thus obtain a bound” (p. 110).
      Minimizing over a superset can only give a smaller or equal value. That is what *relaxation* means:
      drop or loosen constraints, solve the easier problem, and get a bound. It is the foundation of every
      exact method in chapters 5–8.
    options:
      - text: "Because LP rounds costs down"
      - text: "It is sometimes greater"
      - text: "Because LP solutions are always tours"
      - text: "Because the LP minimizes over a superset of the tours, so its minimum is ≤ the best tour"
        correct: true
  - id: relaxation-general
    prompt: >-
      A scheduler needs integer assignments of jobs to nodes. An engineer solves the LP version with
      fractional assignments allowed and gets cost 812. The best integer schedule found costs 840. What
      can be concluded?
    explanation: >-
      Relaxing integrality enlarges the feasible set, so the LP optimum 812 is a lower bound on any
      integer schedule. The optimum lies in [812, 840] and the found schedule is within 3.4%. This is the
      relaxation argument from the TSP (p. 110) applied to a different problem. A large gap means either
      the heuristic is poor or the relaxation is weak, which motivates cutting planes (chapter 6).
    options:
      - text: "The integer optimum is exactly 812"
      - text: "Every integer schedule costs at least 812; the found one is within about 3.4% of optimal"
        correct: true
      - text: "The LP is wrong because 812 < 840"
      - text: "Nothing can be concluded"
  - id: control-zones
    prompt: >-
      Jünger and Pulleyblank's control zones are non-overlapping disks around cities. Why does twice
      the sum of radii bound every tour?
    explanation: >-
      “The salesman must at some point in his or her tour visit this city, and to do so he or she will
      need to travel at least distance r to arrive at the city and at least distance r to leave” (p. 111).
      With disjoint disks these crossings never overlap, so tour ≥ 2Σrᵢ. The disks must satisfy
      rᵢ + rⱼ ≤ cᵢⱼ for every pair, and maximizing 2Σrᵢ under that condition is itself an LP (p. 112).
    options:
      - text: "The tour must enter and leave each disk, travelling ≥ 2r per city, and disjoint disks don't share travel"
        correct: true
      - text: "Because radii equal edge lengths"
      - text: "Because the tour is a circle"
      - text: "Because disks have area πr²"
  - id: zones-are-dual
    prompt: >-
      How do control zones relate to the degree LP?
    explanation: >-
      “The zone-packing problem is precisely the dual of the degree LP relaxation!” (p. 112). Each degree
      constraint gets a multiplier yᵢ. Because xᵢⱼ appears in the constraints of i and j, dual feasibility
      requires yᵢ + yⱼ ≤ cᵢⱼ, which is exactly the no-overlap rule. The dual objective is Σ2yᵢ (right-hand
      sides are 2). Caveat: degree constraints are equations, so dual multipliers may be negative
      (“negative radii”), though under the triangle inequality an optimal nonnegative solution exists
      (pp. 112–113). Geometric pictures make duality intuitive.
    options:
      - text: "Zones correspond to subtour constraints"
      - text: "They are unrelated"
      - text: "Zone packing is the dual LP; radii are dual multipliers with yᵢ + yⱼ ≤ cᵢⱼ"
        correct: true
      - text: "Zones are the primal; tours are the dual"
  - id: degree-integrality
    prompt: >-
      What special structure do degree-LP solutions have, and what is the standard fix for one of its
      flaws?
    explanation: >-
      “There always exist optimal solutions such that every variable has value 0, 1, or 2” (p. 117). A
      value of 2 means going from i to j and straight back, a two-city subtour. The fix is the bound
      xᵢⱼ ≤ 1. With these upper bounds, optimal solutions exist with every variable in {0, ½, 1}, which
      is “not quite integer valued, but pretty close” (pp. 117–118). This bounded degree LP is the usual
      starting model in practice.
    options:
      - text: "Values are negative"
      - text: "All values are integers 0/1; no fix needed"
      - text: "Values 0, 1, or 2; adding xᵢⱼ ≤ 1 gives optimal solutions with values 0, ½, 1"
        correct: true
      - text: "Values are random reals"
---
## Learning objectives

- Write the TSP as an LP over edge variables.
- Explain relaxations and why they give lower bounds.
- Read the dual of the degree LP as packing control zones.

## Encoding

Variables: xᵢⱼ ≥ 0 for each pair (n(n − 1)/2). Objective: minimize Σ cᵢⱼxᵢⱼ (pp. 108–109).

**Degree constraints** (Robinson, 1949): for each city i, Σⱼ xᵢⱼ = 2 (p. 110).

```text
minimize   c12 x12 + c13 x13 + … + c56 x56
subject to x12 + x13 + x14 + x15 + x16 = 2     (city 1)
           …                                    (cities 2–6)
           xij ≥ 0
```

**Relaxation principle.** Tours ⊆ LP-feasible set ⇒ LP optimum ≤ optimal tour (p. 110).

## Control zones = the dual

Disks of radius rᵢ around cities with rᵢ + rⱼ ≤ cᵢⱼ. Every tour has length ≥ 2Σrᵢ. Maximizing that is exactly the dual of the degree LP (pp. 111–113).

## Structure

The degree LP has optimal solutions with values in {0, 1, 2}. Add xᵢⱼ ≤ 1 to rule out back-and-forth pairs, and optimal solutions have values in {0, ½, 1} (pp. 117–118).

## In software terms

Relaxations are the general way to get bounds: drop integrality, drop hard constraints, or aggregate. Any optimization service can report “best found” together with “relaxation bound” to show how much improvement might remain.

## Sources

- Cook (2012), pp. 108–113, 117–118.
- Jünger, M., W. R. Pulleyblank. “Geometric duality and combinatorial optimization.” In *Jahrbuch Überblicke Mathematik* (1993), 1–24.
- Robinson, J. “On the Hamiltonian game (a traveling salesman problem).” RAND RM-303 (1949).
