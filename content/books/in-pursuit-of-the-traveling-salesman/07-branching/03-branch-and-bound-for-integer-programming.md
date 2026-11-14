---
title: "Branch-and-Bound for Integer Programming (pp. 151–152)"
questions:
  - id: land-doig
    prompt: >-
      Who brought branch-and-bound to general integer programming?
    explanation: >-
      “The pioneers in this case were Ailsa Land and Alison Doig from the London School of Economics”
      (p. 151), in 1960. They thought of it “in the ‘geometrical’ interpretation of exploring the convex
      feasible region defined by the LP constraints” and did not call it branch-and-bound. French researchers
      called it “Lawndwa,” a French pronunciation of “Land-Doig” (p. 152).
    options:
      - text: "Gomory"
      - text: "Held and Karp"
      - text: "Dantzig and Fulkerson"
      - text: "Ailsa Land and Alison Doig (LSE, 1960)"
        correct: true
  - id: ip-branch-rule
    prompt: >-
      In a general IP, an LP relaxation gives x = 3.7 for an integer variable. What is the analog of TSP edge
      branching?
    explanation: >-
      Create two children, x ≤ 3 and x ≥ 4. Every integer solution lies in exactly one, and 3.7 is
      infeasible in both. For a binary variable this is the xᵢⱼ = 0 / xᵢⱼ = 1 split from the TSP (p. 147).
      Land–Doig branch-and-bound “dominated the practical world of IP computation in a way that Gomory’s
      algorithm failed to do” (p. 152).
    options:
      - text: "Remove x from the model"
      - text: "x = 3.7 in both children"
      - text: "Children x ≤ 3 and x ≥ 4"
        correct: true
      - text: "Round to 4 and stop"
  - id: rivals-united
    prompt: >-
      When did commercial IP software adopt branch-and-cut, combining the two rival approaches?
    explanation: >-
      “It was well into the 1990s before branch-and-cut took hold in commercial IP software, finally bringing
      together the IP rivals” (p. 152), branching (Land–Doig) and cutting (Gomory). Today every major MIP
      solver uses branch-and-cut with heuristics, presolve and cut pools, all traceable to TSP work.
    options:
      - text: "Well into the 1990s"
        correct: true
      - text: "Never"
      - text: "In 2020"
      - text: "In the 1950s"
---
## Learning objectives

- Translate TSP branching to general integer variables.
- Place Land–Doig in history.

## Summary

Land and Doig (1960) applied branch-and-bound to general IPs, branching on a fractional integer variable (x ≤ ⌊v⌋ / x ≥ ⌈v⌉). It dominated practice over Gomory’s pure cutting method. Commercial solvers combined the two as branch-and-cut in the 1990s (pp. 151–152).

## In software terms

When a MIP solver log shows nodes explored, best bound and incumbent, you are watching Land–Doig plus Padberg–Rinaldi plus Gomory. Branching priorities and good starting solutions are tuning settings worth knowing.

## Sources

- Cook (2012), pp. 151–152.
- Land, A. H., A. G. Doig. “An automatic method of solving discrete programming problems.” *Econometrica* 28 (1960), 497–520.
