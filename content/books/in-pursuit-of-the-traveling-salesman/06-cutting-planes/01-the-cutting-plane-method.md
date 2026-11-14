---
title: "The Cutting-Plane Method (pp. 127–131)"
questions:
  - id: pay-as-you-go
    prompt: >-
      What is the core idea of the Dantzig–Fulkerson–Johnson cutting-plane method?
    explanation: >-
      “Their cutting-plane method does not attempt to solve the full LP problem in a stroke, but rather
      computes LP bounds on a pay-as-you-go basis, generating specific TSP inequalities only as they are
      needed” (p. 127). Solve a small LP, inspect the solution, add an inequality valid for every tour that
      the current solution violates, re-solve, and repeat. If the LP returns a tour, it is optimal
      (p. 128). In software terms this is lazy constraint generation: never materialize what you do not
      need.
    options:
      - text: "Cut the map into regions and solve each"
      - text: "Remove edges until a tour remains"
      - text: "Solve the full LP with all constraints at once"
      - text: "Start small and add violated tour-valid inequalities only as needed, re-solving each time"
        correct: true
  - id: cut-definition
    prompt: >-
      Why are these added inequalities called *cutting planes*?
    explanation: >-
      “At each step the corresponding halfspace cuts off the current LP solution from the feasible region of
      the model” (p. 131). A cut must be satisfied by every tour (valid) and violated by the current LP
      point. Adding it shrinks the feasible region without losing any tour, so the LP bound can only rise or
      stay the same.
    options:
      - text: "Because each one slices off the current LP solution while keeping every tour feasible"
        correct: true
      - text: "Because they are planar graphs"
      - text: "Because they cut the number of cities"
      - text: "Because they cut tours into pieces"
  - id: finger-in-dike
    prompt: >-
      Adding subtour cuts on USA42 keeps producing new “islands,” like plugging leaks in a dam (Paxson's
      “finger in the dike method”). Why is progress still being made?
    explanation: >-
      “Looks are deceiving. Although the solution may appear to be no closer to a tour, we are nonetheless
      making great progress. The initial LP model delivered a solution with objective value 641, but adding
      the single northeastern subtour inequality raised this to 676 and the second inequality further raised
      the bound to 681” and then 682.5, 686, 686, 688 and 697 (p. 128). Track the bound, not how
      the LP solution looks. The bound is the quantity that converges.
    options:
      - text: "It isn't; the method loops forever"
      - text: "The lower bound rises with each cut (641 → 676 → 681 → … → 697), even if solutions still look broken"
        correct: true
      - text: "Because islands get bigger"
      - text: "Because the tour gets shorter"
  - id: seven-of-trillions
    prompt: >-
      After seven subtour cuts the USA42 LP solution satisfies *all* 2.2 trillion subtour inequalities.
      What does this show?
    explanation: >-
      “We have computed an optimal solution to the full set of over two trillion subtour-elimination
      constraints by adding only seven inequalities!” (p. 129). Only a handful of constraints are active at
      the optimum, and the rest are slack. This is the basis of lazy generation: an exponential family can
      be optimized over by touching a tiny part of it, provided you can find violated members, which is the
      separation problem.
    options:
      - text: "That most subtour inequalities are wrong"
      - text: "That optimizing over an exponential family can need only a handful of explicitly added members"
        correct: true
      - text: "That seven is always enough"
      - text: "That the LP solver skipped constraints"
  - id: comb-four-sets
    prompt: >-
      At 697 the LP solution satisfies all subtour constraints yet is not a tour. What finished USA42?
    explanation: >-
      Dantzig et al. used two ad hoc inequalities, equivalent to four-set configurations: a handle crossed by
      three teeth. “Every tour must cross the four boundaries of the sets at least ten times” (p. 130). Each
      tooth is crossed at least twice, and the handle at least three times, which becomes four by parity
      (note, pp. 217–218). The LP crossed them for a total of only 9, so the cut was violated. Adding it
      raised the bound to 698, and a second configuration produced a tour of 699 (pp. 130–131). In total
      “nine inequalities together settle the challenge … Nine inequalities from trillions and trillions”
      (p. 131).
    options:
      - text: "Two four-set (comb-type) inequalities requiring ≥ 10 crossings; nine cuts in total"
        correct: true
      - text: "Branch-and-bound with 1,000 nodes"
      - text: "Rounding the LP solution"
      - text: "More subtour inequalities"
  - id: dual-certificate-usa
    prompt: >-
      When the LP finally returns a tour of 699, what convinces “one and all” that 699 is optimal?
    explanation: >-
      “At this point we bring in the dual LP solution to convince one and all that 699 is indeed the optimal
      value” (p. 131). The tour is feasible at 699, and the dual multipliers on degree, subtour and comb
      constraints combine into a proof that every tour costs ≥ 699 (weak duality). The certificate is the
      set of cuts plus the multipliers, and anyone can check it.
    options:
      - text: "A dual LP solution proving every tour costs at least 699"
        correct: true
      - text: "Comparison with Newsweek"
      - text: "Running nearest neighbor"
      - text: "The fact that the computer stopped"
  - id: lazy-constraints-devops
    prompt: >-
      A capacity planner has a constraint for every possible failure scenario (exponentially many). Which
      approach follows the cutting-plane idea?
    explanation: >-
      Solve with a few scenarios, then run a checker (an oracle) that finds a scenario the current plan
      violates, add that constraint, and repeat until the oracle finds nothing. This is how DFJ handled 2.2
      trillion subtour constraints with seven cuts (p. 129). Modern MIP solvers expose it as *lazy
      constraint callbacks*, and robust optimization uses it as scenario or constraint generation. The
      requirement is a fast separation oracle (next sections).
    options:
      - text: "Ignore scenarios"
      - text: "Pick 10 random scenarios and stop"
      - text: "Enumerate all scenarios up front"
      - text: "Iteratively solve, find a violated scenario with an oracle, add it, and re-solve"
        correct: true
---
## Learning objectives

- Run the cutting-plane loop and explain why the result is optimal when it returns a tour.
- Read bound progress as the signal of convergence.

## The method (Dantzig, Fulkerson, Johnson, 1954)

```text
LP := degree constraints (+ x ≤ 1)
loop
    x* := solve LP
    if x* is a tour: stop — optimal (every tour is LP-feasible)
    find inequality valid for all tours but violated by x*     ← separation
    add it to LP
```

**USA42 run** (pp. 127–131):

| Step | Cut | Bound |
|---|---|---|
| 0 | degree LP | 641 |
| 1–7 | subtours (islands, then non-obvious sets) | 676, 681, 682.5, 686, 686, 688, 697 |
| 8 | four-set comb | 698 |
| 9 | second comb | tour of 699, optimal |

Seven cuts optimize over all 2.2 trillion subtour inequalities. Nine cuts in total settle the instance. The dual solution certifies 699.

## Why it works

Every cut is valid for every tour, so the LP stays a relaxation and the bound only rises. When the LP optimum is a tour, nothing cheaper exists (p. 128).

## In software terms

- **Lazy constraint generation**, in MIP-solver callbacks, robust optimization and Benders decomposition, is this same loop.
- **Monitor the bound**, not the look of intermediate solutions.
- The hard part is the **oracle** that finds violated constraints. That is separation (p. 137).

## Sources

- Cook (2012), pp. 127–131, 217–218.
- Dantzig, G., R. Fulkerson, S. Johnson. “Solution of a large-scale traveling-salesman problem.” *Operations Research* 2 (1954), 393–410.
