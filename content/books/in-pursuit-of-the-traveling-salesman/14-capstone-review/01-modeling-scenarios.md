---
title: "Capstone I: Modeling Scenarios"
questions:
  - id: cache-warmup
    prompt: >-
      A CDN pre-warms 300 edge caches each night. Moving the warm-up job between POPs costs time depending on
      the ordered pair, and the job must end at the primary POP but can start anywhere. What is the cleanest
      model?
    explanation: >-
      Costs are direction-dependent, so this is an **asymmetric** TSP (chapter 3, p. 60). It is an open path
      with one fixed end, so add a dummy city with zero-cost edges from the dummy to every POP (free start) and
      a zero-cost edge from the primary into the dummy only (forced end), with prohibitive costs elsewhere
      (chapter 2, p. 36). Solve, then delete the dummy. A symmetric solver can handle it after the 2n-city
      transformation (p. 215).
    options:
      - text: "Shortest path from primary to farthest POP"
      - text: "Symmetric TSP tour, ignore the end constraint"
      - text: "Asymmetric TSP with a dummy city wired to force the end at the primary POP"
        correct: true
      - text: "Minimum spanning tree"
  - id: choose-method
    prompt: >-
      You need routes for 5,000 delivery stops every 10 minutes, with quality reported to operations. Which
      architecture fits the book's lessons?
    explanation: >-
      Run fast construction (greedy or spacefilling curve) plus strong local search (chained LK or LKH) under a
      time budget (chapter 4: chained LK reaches ≈1% in seconds at 100k cities, p. 87). For reporting, compute
      a lower bound: the subtour LP via cutting planes, or a Held–Karp 1-tree bound, and publish the gap
      (chapters 5–8). Exact branch-and-cut every 10 minutes at 5,000 stops is possible for easy instances but
      risky as an SLA. Brute force and Held–Karp DP are impossible at this size.
    options:
      - text: "Brute force on a GPU cluster"
      - text: "Held–Karp dynamic programming"
      - text: "Heuristic (greedy + chained LK/LKH) under a time budget, plus an LP/1-tree bound to report the gap"
        correct: true
      - text: "Nearest neighbor alone with no reporting"
  - id: seriation-logs
    prompt: >-
      An observability team wants to order 2,000 log templates so templates that co-occur in incidents sit
      together, and to split them into 12 groups. Which method from the book applies?
    explanation: >-
      Define cost(i, j) as the number of incidents that contain exactly one of the two templates (the break
      metric from texture and genome layouts, p. 58), or a distance derived from similarity. Then use
      **TSP+k**: add k + 1 = 13 zero-cost dummy cities so the optimal tour cuts itself into 12 clusters at the
      most expensive jumps (Climer–Zhang, p. 56).
    options:
      - text: "Sort alphabetically and split evenly"
      - text: "Break-count costs plus TSP+k with 13 dummy cities"
        correct: true
      - text: "Minimum spanning tree with 12 edges"
      - text: "Christofides with 12 matchings"
  - id: which-coverage
    prompt: >-
      A chaos-engineering tool must exercise every directed dependency edge between services at least once in
      a single closed run, minimizing repeats. Which problem is this?
    explanation: >-
      Covering *edges* is Euler’s side, not Hamilton’s (chapter 2, pp. 33–34). If every node has in-degree equal
      to out-degree and the graph is connected, an Eulerian circuit covers each edge exactly once and is found
      in linear time. Otherwise, add the cheapest duplicate edges to balance degrees (directed Chinese postman,
      solvable with min-cost flow). It is polynomial, unlike the TSP.
    options:
      - text: "Eulerian circuit / Chinese postman over dependency edges — polynomial"
        correct: true
      - text: "Hamiltonian circuit — NP-complete"
      - text: "Minimum spanning tree"
      - text: "TSP over services"
  - id: metric-check
    prompt: >-
      Before relying on Christofides' 1.5 guarantee for a fleet's cost matrix, what should you verify or
      enforce?
    type: multiple
    explanation: >-
      The guarantee needs **metric** instances: symmetric costs that satisfy the triangle inequality (p. 183).
      Check symmetry, or apply a transformation. Enforce the triangle inequality by replacing each cost with its
      shortest-path distance (metric closure). Then expand each tour edge back into its real path. Integer
      rounding is a separate concern about exactness, not the guarantee (chapter 9). Costs do not need to be
      Euclidean.
    options:
      - text: "Costs are symmetric"
        correct: true
      - text: "Triangle inequality holds — e.g., by taking shortest-path closure"
        correct: true
      - text: "Costs are Euclidean distances"
      - text: "Tour edges in the closure are expanded back into real paths"
        correct: true
  - id: kick-design
    prompt: >-
      You use iterated local search for a container-placement problem. The local optimizer is a pairwise-swap
      search. Which perturbation follows the double-bridge principle?
    explanation: >-
      A good kick “must take the solution out of its neighborhood, so it should be a modification that [the
      local search] cannot easily undo,” while keeping most of the solution intact so re-optimization is fast
      (p. 87). For pairwise swaps, a small random *cyclic* move of 3–4 containers across nodes (not reachable by
      a single swap) fits. A full random restart throws away good structure, and a single swap is undone
      immediately.
    options:
      - text: "A single random swap"
      - text: "A small multi-element cyclic move that one swap cannot reverse, keeping most of the placement"
        correct: true
      - text: "A full random restart every time"
      - text: "No perturbation"
  - id: real-constraints
    prompt: >-
      A planner's optimal tour ignores maintenance windows at some sites. Using lessons from Ron Schreck and
      H. M. Cleveland, what should the system do?
    explanation: >-
      Schreck modified Concorde’s tour to hit lighted airports before sunrise (p. 49), and Cleveland fixed a
      route that ignored an unbridged river (p. 21). Pure TSP optima ignore side constraints. Add time windows
      to the model (a TSP with time windows, often via IP or constraint programming with LKH-3-style
      heuristics), or at minimum validate and let people override with visible cost deltas.
    options:
      - text: "Ship the pure TSP optimum anyway"
      - text: "Model the windows as constraints (TSPTW) or validate and allow informed overrides"
        correct: true
      - text: "Remove the sites"
      - text: "Randomize visit order"
---
## Purpose

These questions combine several chapters into the kind of decisions engineers actually make. Each explanation points back to the relevant pages.

## Quick reference

| If you need… | Use… | Chapter |
|---|---|---|
| Path instead of tour | Dummy city (zero-cost, or wired for fixed ends) | 2, 3 |
| Direction-dependent costs | Asymmetric TSP, or the 2n-city symmetric transform | 3, 4 |
| Clusters | TSP+k dummy cities | 3 |
| Edge coverage | Euler / Chinese postman (polynomial) | 2 |
| Fast good tours | Greedy + chained LK / LKH | 4 |
| Quality guarantee | Subtour LP / Held–Karp bound → gap | 5, 8 |
| Proof of optimality | Branch-and-cut (Concorde) + certificate | 6–8 |
| Worst-case guarantee | Christofides (metric only), KKO | 4, 9, 13 |

## Sources

- Cook (2012), as cited in each explanation.
