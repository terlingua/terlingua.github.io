---
title: "Cutting Planes for Integer Programming (pp. 144–145)"
questions:
  - id: gomory-insight
    prompt: >-
      Ralph Gomory's “tingling toes” insight: if every LP solution satisfies 3x + 2y ≤ 7.14, what holds
      for every *integer* solution?
    explanation: >-
      “If we know an inequality 3x + 2y ≤ 7.14 is satisfied by all solutions to an LP problem, then we know
      also that 3x + 2y ≤ 7 is satisfied by all integer solutions” (pp. 144–145). With integer coefficients
      and integer variables the left side is an integer, so you can round the right side down. “A halfspace
      that touches the border of the LP feasible region can be pushed in a small amount without cutting off
      integer solutions” (p. 145). Chvátal developed the theory of this rounding in the 1970s (p. 218).
    options:
      - text: "3x + 2y ≤ 8"
      - text: "3x + 2y ≤ 7 (round the right side down, since the left side is an integer)"
        correct: true
      - text: "3x + 2y = 7.14"
      - text: "Nothing more"
  - id: gomory-fate
    prompt: >-
      What happened to Gomory's pure cutting-plane algorithm for integer programs?
    explanation: >-
      It derives a cut whenever a simplex dictionary gives a fractional value. It “turned the field of
      integer programming upside down for a number of years, but once computers became speedy enough to
      tackle large instances it became apparent that the algorithm did not behave well in practice.
      Nonetheless … a variant of the mechanism for creating cutting planes, also developed by Gomory, is now
      the workhorse of commercial IP solvers” (p. 145). The pure method stalled, and Gomory cuts later came
      back inside branch-and-cut (chapter 7).
    options:
      - text: "It was proven wrong"
      - text: "It is used only for the TSP"
      - text: "It solved all IPs quickly"
      - text: "Poor as a standalone method, but its cuts became a core component of modern IP solvers"
        correct: true
  - id: general-vs-specific
    prompt: >-
      Subtours and combs are TSP-specific. What carries over to general integer programming?
    explanation: >-
      “Cutting planes are far and away the most important tool in modern IP solvers. Subtours, combs, clique
      trees, et al. are TSP-specific, but the overall method of improving an LP relaxation step-by-step via
      cutting planes carries over to the general IP setting” (p. 144). Generic families such as Gomory,
      mixed-integer rounding and cover cuts fill the role that combs play for tours.
    options:
      - text: "Nothing"
      - text: "The overall method — strengthen the LP relaxation iteratively with valid cuts"
        correct: true
      - text: "Only comb inequalities"
      - text: "Only the 42-city data"
  - id: dense-cuts
    prompt: >-
      Chapter 8 notes that Miliotis's Gomory-based approach failed at 90 cities because cuts filled the
      constraint matrix. Why are TSP-specific cuts better here?
    explanation: >-
      “Each additional cutting plane involves nearly all of the variables in an LP model, which eventually
      slows the simplex algorithm … In contrast, the beauty of the pure Dantzig et al. approach is that
      TSP-specific inequalities tend to assign most edges the value zero, resulting in LP models that are
      relatively easy to solve” (p. 156). Sparse constraints keep LPs fast, so the density of a cut matters
      as much as its strength.
    options:
      - text: "They are sparse (few nonzero coefficients), keeping the LP fast, while Gomory cuts are dense"
        correct: true
      - text: "They are fewer in number"
      - text: "They are nonlinear"
      - text: "They have integer coefficients"
---
## Learning objectives

- Derive a Gomory-type rounding cut.
- Explain why generic cuts matter in IP solvers and why sparsity matters.

## Summary

Gomory (Princeton, mid-1950s): solve the LP, and if an integer-valued expression is bounded by 7.14, it is bounded by 7. This pushes halfspaces inward without losing integer points (pp. 144–145). His pure algorithm behaved badly in practice, but **Gomory cuts** are now a workhorse of commercial solvers (p. 145). The cutting-plane *method* carries over from the TSP to all of IP (p. 144). Sparse, problem-specific cuts keep LPs tractable, and dense generic cuts can swamp them (p. 156).

## In software terms

When a MIP log shows “Gomory: 120 cuts, MIR: 45, cover: 30,” that is this chapter at work. When a model is slow, adding problem-specific valid inequalities (your own “combs”) is often the biggest improvement available.

## Sources

- Cook (2012), pp. 144–145, 156, 218.
- Gomory, R. E. “Outline of an algorithm for integer solutions to linear programs.” *Bull. AMS* 64 (1958), 275–278.
- Chvátal, V. “Edmonds polytopes and a hierarchy of combinatorial problems.” *Discrete Math.* 4 (1973), 305–337.
