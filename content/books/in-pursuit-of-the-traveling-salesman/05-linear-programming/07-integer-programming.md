---
title: "Integer Programming (pp. 122–124)"
questions:
  - id: why-ip
    prompt: >-
      Why do LP users want integer variables?
    explanation: >-
      Partly to avoid “a production schedule calling for 33⅓ widgets,” “but the main point is to capture
      decisions that come down to individual choices. Should we build an additional factory? Yes or no …
      Such decisions can be brought into an LP model if we are permitted to include variables that take on
      values zero or one only” (p. 122). This is **integer programming (IP)**. It is very expressive, but it
      “comes at great computational cost” (p. 122). Feature flags, server on/off decisions and
      region-placement choices are all binary decisions.
    options:
      - text: "To model yes/no and whole-number decisions (build or not, assign or not)"
        correct: true
      - text: "To remove constraints"
      - text: "Because simplex requires integers"
      - text: "To make LPs faster"
  - id: coloring-ip
    prompt: >-
      For 3-coloring a graph as an IP, each vertex i has x_{i,red}, x_{i,green}, x_{i,blue}. Which
      constraints are used?
    type: multiple
    explanation: >-
      Each vertex gets exactly one color: x_{i,red} + x_{i,green} + x_{i,blue} = 1. For each edge (i, j) and
      color c: x_{i,c} + x_{j,c} ≤ 1, so endpoints cannot share a color. Variables are 0/1 (pp. 123–124). In
      the LP relaxation every variable can be 1/3, which satisfies everything and tells you nothing: “The
      integer variables rule the day” (p. 124).
    options:
      - text: "x_{i,red} + x_{i,green} + x_{i,blue} = 1 for each vertex"
        correct: true
      - text: "x_{i,c} + x_{j,c} ≤ 1 for each edge and color"
        correct: true
      - text: "Variables restricted to 0 or 1"
        correct: true
      - text: "x_{i,red} · x_{j,red} = 0 (a product constraint)"
  - id: lp-useless-coloring
    prompt: >-
      What does the LP relaxation of the 3-coloring IP return, and what lesson does that hold?
    explanation: >-
      “An LP solution to the model without the integer restrictions would be to set all variables equal to
      1/3, providing no information that could be used to assign colors” (p. 124). Some relaxations are
      useless. The quality of a formulation matters, and the TSP is fortunate that subtour LPs are strong.
      For hard IPs, much of the modeling work goes into tightening the relaxation.
    options:
      - text: "All variables 1/3 — feasible but uninformative; relaxation strength depends on the formulation"
        correct: true
      - text: "An infeasibility proof"
      - text: "All variables 0"
      - text: "A valid coloring"
  - id: tsp-ip
    prompt: >-
      Degree constraints plus all subtour inequalities with 0/1 variables exactly describe tours. Why
      not hand that IP to a commercial solver?
    explanation: >-
      “The collection of subtour inequalities numbers approximately 2ⁿ/² for an n-city TSP” (p. 124), far
      too many to list. Compact IPs exist (Dantzig’s n³-variable model, and later Miller–Tucker–Zemlin with
      O(n²) constraints), but “all alternative models are dominated, in terms of practical performance, by
      the subtour formulation and the methods we describe in the next chapter” (p. 124). Their LP
      relaxations are much weaker. A compact formulation with a weak relaxation loses to an exponential one
      that is generated lazily and has a strong relaxation.
    options:
      - text: "The subtour family is exponential, and compact alternatives have weak relaxations that perform worse"
        correct: true
      - text: "Because the IP is incorrect"
      - text: "Because the TSP is not an IP"
      - text: "Because IP solvers cannot handle binaries"
  - id: ip-hard
    prompt: >-
      Why does the existence of compact TSP formulations show that general IP is hard?
    explanation: >-
      “The existence of compact models should, however, convince you that solving IP problems is in general
      difficult: a polynomial-time IP solver would give directly a polynomial-time TSP solver” (p. 124). A
      polynomial-size IP formulation is a polynomial reduction from TSP to IP, so IP is at least as hard,
      that is, NP-hard (chapter 9). Your MIP solver can stall on a small model, and when it does that is
      expected behavior, not a bug.
    options:
      - text: "A polynomial-size IP for the TSP is a reduction: a fast IP solver would be a fast TSP solver"
        correct: true
      - text: "Because integers are infinite"
      - text: "It shows IP is easy"
      - text: "Because IP has more variables"
  - id: tsp-pays-bills
    prompt: >-
      Cook says TSP work “often pays the bills” for researchers. Why?
    explanation: >-
      “Nearly all known general IP methods were discovered first in pursuit of solutions for the salesman”
      (p. 124). Cutting planes, branch-and-bound and branch-and-cut all started on the TSP and now power
      commercial MIP solvers used in business, the ones that “duel in the commercial marketplace” (p. 124).
      Work on one specific problem produced general tools.
    options:
      - text: "Because of the Clay prize"
      - text: "Because the TSP is easy"
      - text: "Because companies pay per TSP solved"
      - text: "Because general IP methods (cuts, branch-and-bound, branch-and-cut) were first developed for the TSP"
        correct: true
---
## Learning objectives

- Model yes/no decisions with 0/1 variables.
- Recognize weak vs strong relaxations.
- Understand why the TSP shows that IP is hard, and why TSP methods matter to IP.

## Summary

- **IP** = LP + integrality. It captures discrete decisions and is expensive to solve (p. 122).
- Dantzig showed many problems can be written as IPs, including the TSP and map coloring (pp. 122–123).
- **3-coloring IP:** one color per vertex, and no shared color across an edge. The LP relaxation (all 1/3) is useless (pp. 123–124).
- **TSP as an IP:** degree + subtour constraints with binaries. Exponentially many constraints, but this formulation beats compact ones in practice (p. 124).
- A fast general IP solver would solve the TSP fast, so IP is hard in general (p. 124).
- Most general IP techniques came from TSP research (p. 124).

## In software terms

MIP solvers (Gurobi, CPLEX, HiGHS, SCIP, OR-Tools) are standard tools for scheduling, placement and planning. Formulation quality, meaning the tightness of the LP relaxation, often matters more than hardware. Prefer strong formulations, even large ones, when the solver can add constraints lazily.

## Sources

- Cook (2012), pp. 122–124.
- Dantzig, G. B. “On the significance of solving linear programming problems with some integer variables.” *Econometrica* 28 (1960), 30–44.
- Miller, C. E., A. W. Tucker, R. A. Zemlin. “Integer programming formulation of traveling salesman problems.” *J. ACM* 7 (1960), 326–329.
