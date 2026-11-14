---
title: "The Search Party: Branch-and-Bound, Branch-and-Cut, Strong Branching (pp. 148–151)"
questions:
  - id: bnb-loop
    prompt: >-
      Which description of branch-and-bound (named by Little et al.) is correct?
    explanation: >-
      Start with the root relaxation. “If at any point the LP bound associated with a subproblem is greater
      than or equal to the length of a known tour, then we prune the subproblem … At each step we choose a
      remaining subproblem and branch, creating two new child subproblems. The process stops when each
      unbranched subproblem has been pruned” (pp. 148–149). At that point the best known tour is optimal.
      The method is exact.
    options:
      - text: "Branch on every edge in order"
      - text: "Bound only the root"
      - text: "Branch until a time limit and return the best guess"
      - text: "Repeatedly branch subproblems, prune any whose bound ≥ the best known tour, stop when all are pruned"
        correct: true
  - id: bound-quality
    prompt: >-
      Eastman's 1958 branch-and-bound handled only 10 cities. Why did the same framework solve USA42 in a
      small tree?
    explanation: >-
      “The method can only succeed if we have a high-quality bounding mechanism. Weak bounds lead to very
      large search trees, while strong bounds allow for quick pruning … Eastman himself did not consider the
      idea of improving the degree-constraint bounds with cutting planes” (p. 149). With subtour cuts at
      every node, the USA42 tree (figure 7.3) is small and each branch raises bounds substantially. The
      search tree is only as small as the bound is tight.
    options:
      - text: "Better branching order alone"
      - text: "Smaller cities"
      - text: "Faster computers"
      - text: "Strong bounds from cutting planes at each node, which prune early"
        correct: true
  - id: incumbent-matters
    prompt: >-
      Why does a good starting tour (from LKH, say) speed up exact branch-and-bound?
    explanation: >-
      Pruning compares node bounds against the best known tour. A shorter incumbent prunes more nodes earlier,
      which is why exact solvers first run strong heuristics (chapter 4). In the USA42 example the 699 tour
      let the 703.5 side be discarded immediately (p. 148). Heuristics and bounds work together: one gives the
      upper bound and the other the lower bound.
    options:
      - text: "Because heuristics compute bounds"
      - text: "It doesn't; only bounds matter"
      - text: "A tighter incumbent lets more subproblems be pruned (bound ≥ incumbent)"
        correct: true
      - text: "Because the tree starts from the tour"
  - id: branch-and-cut
    prompt: >-
      What is branch-and-cut, and who coined the term?
    explanation: >-
      Branching and cutting “are natural partners … Manfred Padberg and Giovanni Rinaldi leading the way.
      These researchers coined the term branch-and-cut” (p. 150): cutting planes are generated at every node
      of the branch-and-bound tree, not just the root. They “unleashed whatever cuts they could obtain” and
      solved 2,392 cities in 1987 (p. 150). It is the architecture of Concorde and of every modern MIP solver.
    options:
      - text: "Cutting the map into branches; Dantzig"
      - text: "Branch-and-bound with cutting planes generated at each node; Padberg and Rinaldi"
        correct: true
      - text: "Branching only at the root; Eastman"
      - text: "A heuristic; Lin"
  - id: cut-pool
    prompt: >-
      Why did Padberg and Rinaldi store cuts in a shared *pool*?
    explanation: >-
      “First, searching the pool can be much faster than carrying out elaborate separation algorithms.
      Second, most separation algorithms employed in TSP computations are hit-or-miss heuristic methods. And
      sometimes they miss at one subproblem but hit at another. By collecting the inequalities into a pool, we
      gather the hits and see what they can do at other parts of the search tree” (p. 150). Every cut is valid
      for all tours, so it can be reused anywhere in the tree. A cut pool is a cache of verified work.
    options:
      - text: "To share cuts between different instances"
      - text: "To save memory"
      - text: "Pool lookups are cheaper than separation, and cuts found by hit-or-miss heuristics in one node often help others"
        correct: true
      - text: "Because cuts expire"
  - id: strong-branching
    prompt: >-
      What is strong branching (Concorde)?
    explanation: >-
      “Take some of the guesswork out of the selection by calling on the LP solver to give a strong hint …
      carrying out a limited number of simplex pivots, say fifty, for each pair of children determined by the
      branching candidates. With these values in hand, we select the branch that appears to give the greatest
      increase in the LP bounds” (p. 151). It is expensive, “but it is well worth the effort to avoid a bad
      split that can potentially double the size of a search tree” (p. 151). The “all-in” version fully solves
      the top candidates’ child LPs with some cuts, which pays only on the hardest instances.
    options:
      - text: "Branch on the longest edge always"
      - text: "Evaluate candidate branches with a few simplex pivots each and pick the one that raises bounds most"
        correct: true
      - text: "Branch on many edges at once"
      - text: "Use random branching"
  - id: dating-metaphor
    prompt: >-
      Chvátal compares branching to marriage. What does the metaphor emphasize?
    explanation: >-
      “Once we decide to branch we are committed to the new view of the problem; there is no undoing what
      branching has done” (p. 150). Early methods married “the first potential partner they met” (the first
      fractional edge). Padberg and Rinaldi used a “dating service”: most-fractional edges, preferring the
      longest. Concorde chose to “first go out on a few dates” (strong branching) and later to “live together with
      several top choices” (full LP trials) (pp. 150–151). When a decision is irreversible and expensive, spending effort to choose
      well is worth it. Rollout strategies in deployment follow the same logic.
    options:
      - text: "Branching is reversible"
      - text: "Branching is an irreversible commitment, so it is worth investing in choosing well"
        correct: true
      - text: "Always branch on the first fractional edge"
      - text: "Branching should be random"
  - id: padberg-rinaldi-choice
    prompt: >-
      Padberg and Rinaldi chose branching edges from the most-fractional set, preferring the longest. Why
      the longest?
    explanation: >-
      “From among these, they select the edge having greatest travel cost. This makes a lot of sense.
      Branching on a long edge will tend to have a greater impact on the LP bounds than branching on a short
      edge” (p. 151). Forcing a costly edge in, or out, changes the objective more. Most-fractional means
      values far from 0 and 1, where the LP is most undecided.
    options:
      - text: "Fixing a costly, undecided edge tends to move the LP bound more"
        correct: true
      - text: "Short edges are infeasible"
      - text: "It is arbitrary"
      - text: "Long edges are always in the optimal tour"
---
## Learning objectives

- Run branch-and-bound and understand why bound strength controls tree size.
- Explain branch-and-cut, cut pools and strong branching.

## Branch-and-bound

```text
active := {root}; incumbent := best heuristic tour
while active not empty
    P := pick(active)
    bound := solve LP(P) (+ cuts)
    if bound ≥ incumbent: prune P
    else if LP(P) solution is a tour: incumbent := it; prune P
    else branch P on a fractional edge → add both children to active
return incumbent           // optimal
```

Strong bounds mean a small tree. Eastman, without cuts, stopped at 10 cities. With cuts, the USA42 tree is tiny (pp. 148–150).

## Branch-and-cut (Padberg–Rinaldi, 1987)

Cuts at every node, a **cut pool** shared across nodes, 2,392 cities solved (p. 150).

## Choosing the branch

| Strategy | Metaphor | Description |
|---|---|---|
| First fractional edge | Marry first person met | Quick, often bad |
| Most fractional + longest | Dating service | Padberg–Rinaldi |
| **Strong branching** | A few dates | ≈50 simplex pivots per candidate child, pick biggest bound gain |
| Full trial LPs with cuts | Live together | Most accurate, very expensive |

*(pp. 150–151)*

## In software terms

- An **incumbent from a good heuristic** speeds up the exact search.
- **Cache validated work** (cut pools).
- **Spend effort on irreversible decisions** (strong branching). A bad split can double the work.

## Sources

- Cook (2012), pp. 148–151.
- Little, J. D. C., K. G. Murty, D. W. Sweeney, C. Karel. “An algorithm for the traveling salesman problem.” *Operations Research* 11 (1963), 972–989.
- Padberg, M., G. Rinaldi. “A branch-and-cut algorithm for the resolution of large-scale symmetric traveling salesman problems.” *SIAM Review* 33 (1991), 60–100.
- Applegate, D., R. Bixby, V. Chvátal, W. Cook. “On the solution of traveling salesman problems.” *Documenta Mathematica*, Extra Volume ICM III (1998), 645–656.
