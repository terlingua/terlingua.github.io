---
title: "Breaking Up (pp. 146–148)"
questions:
  - id: why-branch
    prompt: >-
      When should a cutting-plane computation switch to branching?
    explanation: >-
      “At some point it may happen that each new cutting plane removes only a tiny amount of hay. Rather than
      continue to cut, cut, cut, we can instead consider splitting the remaining haystack into two smaller
      stacks” (p. 146). When bound improvement per round stalls (tailing off), branching usually pays off
      better. Monitor the marginal bound gain per unit of work and switch strategies when it flattens.
    options:
      - text: "Only when the LP is infeasible"
      - text: "Immediately, before any cuts"
      - text: "When new cuts raise the bound only marginally (tailing off)"
        correct: true
      - text: "Never; cuts always suffice"
  - id: edge-branch
    prompt: >-
      What is the standard TSP branching step?
    explanation: >-
      “We select one of the red [fractional] edges and force the problem to decide to use the edge or not
      … create the first subproblem by adding the constraint xᵢⱼ = 0 and the second subproblem by adding the
      constraint xᵢⱼ = 1” (p. 147). These are the 0-side and 1-side. Every tour lies in exactly one side, so
      the overall lower bound is the *minimum* of the two child bounds. The current fractional solution is
      infeasible in both children, so it disappears.
    options:
      - text: "Pick a fractional edge and create children with xᵢⱼ = 0 and xᵢⱼ = 1"
        correct: true
      - text: "Split cities by longitude"
      - text: "Duplicate the LP"
      - text: "Remove a city"
  - id: boise-carson
    prompt: >-
      Branching USA42 at bound 682.5 on Boise–Carson City gives 687.5 (0-side) and 686 (1-side). What is
      the new global lower bound, and what happens after cuts on the 1-side raise it to 703.5?
    explanation: >-
      “Since each tour through the forty-two cities is a feasible solution to either one or the other of the
      two LP models, we now know that no tour can have value less than 686 units” (p. 147), the minimum of
      the children. Then adding subtour cuts on the 1-side gives 703.5 > 699, the known tour, so no better
      tour can exist there: “This allows us to prune the 1-side subproblem” (p. 148). One branch plus a few
      cuts eliminated half the search.
    options:
      - text: "686 (the minimum); the 1-side is pruned because 703.5 exceeds the 699 tour"
        correct: true
      - text: "703.5 globally"
      - text: "682.5; the 0-side is pruned"
      - text: "687.5; nothing more"
  - id: global-bound-min
    prompt: >-
      In a branch-and-bound tree, how is the global lower bound computed at any moment?
    explanation: >-
      Every tour lies in some active (unpruned, unexplored) leaf, so the global bound is the minimum LP bound
      over active leaves. Once all leaves are pruned (bound ≥ incumbent), the incumbent is optimal. On USA42
      after the first branch, min(687.5, 686) = 686 (p. 147). Solver logs report exactly this “best bound,”
      and the gap to the incumbent tour shows how much work remains.
    options:
      - text: "The average of leaf bounds"
      - text: "The maximum over all nodes"
      - text: "The minimum bound over active (unpruned) leaves"
        correct: true
      - text: "The root bound only"
---
## Learning objectives

- Branch on a fractional edge and compute the resulting bounds.
- Prune with an incumbent tour.

## Summary

When cuts tail off, **branch** (p. 146). Pick a fractional edge (i, j) and create:

- **0-side:** xᵢⱼ = 0 (tour avoids the edge)
- **1-side:** xᵢⱼ = 1 (tour uses the edge)

Each tour lies in exactly one child, so the global bound is the minimum of the children’s bounds (p. 147).

**USA42 example** (handicapped to island cuts only): root 682.5 → branch on Boise–Carson City: 0-side 687.5, 1-side 686, so the bound is 686. More cuts on the 1-side give 703.5 ≥ 699 (incumbent), so the 1-side is **pruned** (pp. 147–148).

The idea goes back to Dantzig et al. Eastman (1958) worked it into a full algorithm (p. 146).

## In software terms

Divide-and-conquer with bounds is a general pattern: search tools that skip subtrees whose best possible score is below the current threshold, or query planners that prune join orders. Pruning is safe only when the bound is valid.

## Sources

- Cook (2012), pp. 146–148.
- Eastman, W. L. *Linear Programming with Pattern Constraints*. Ph.D. thesis, Harvard, 1958.
