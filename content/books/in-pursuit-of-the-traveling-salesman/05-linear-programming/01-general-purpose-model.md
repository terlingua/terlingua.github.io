---
title: "General-Purpose Model (pp. 94–99)"
questions:
  - id: lp-role-tsp
    prompt: >-
      Why is linear programming “the tool of choice” for proving TSP optimality?
    explanation: >-
      LP is “an amazingly effective method for combining a large number of simple rules, satisfied by
      all tours, to obtain a single rule of the form ‘no tour through this point set can be shorter than
      X.’ The number X gives an immediate quality measure: if we can also produce a tour of length X then
      we can be sure that it is optimal” (p. 94). Every successful exact TSP method, including Concorde,
      is built on it. LP is the source of the lower bound in the tour-plus-bound pair from chapter 1.
    options:
      - text: "Because tours are linear"
      - text: "Because LP finds tours directly"
      - text: "Because LP combines simple rules valid for all tours into a lower bound X; a tour of length X is then optimal"
        correct: true
      - text: "Because LP enumerates permutations efficiently"
  - id: three-elements
    prompt: >-
      Which three elements define Dantzig's LP model?
    type: multiple
    explanation: >-
      (1) Constraints may be **inequalities**, not just balance equations. The Mad Hatter’s “it’s very
      easy to take more than nothing” motivates nonnegativity, T ≥ 0 (p. 96). (2) Constraints are
      **linear**: sums of constants times variables. “It is not permitted to multiply variables together,
      such as XY ≥ 0, or to take square roots” (p. 96). (3) There is an explicit **linear objective** to
      maximize or minimize, which Dantzig saw as forcing managers “to be precise in expressing what they
      want to achieve” (p. 97). Integrality is *not* part of LP. That makes it integer programming (p. 122).
    options:
      - text: "Inequality constraints allowed (e.g., nonnegativity)"
        correct: true
      - text: "Only linear expressions — no products of variables"
        correct: true
      - text: "An explicit linear objective to optimize"
        correct: true
      - text: "Variables must be whole numbers"
  - id: linear-meaning
    prompt: >-
      Alice takes 2 lumps of sugar per cup of tea, so S = 2T. What assumption does “linear” encode?
    explanation: >-
      “Activities are assumed to consume resources in proportion to their levels. If Alice takes two
      lumps of sugar in one cup of tea, then we assume she would like four lumps of sugar in two cups …
      doubling the level requires double the resources” (p. 96). Proportionality and additivity, with no
      economies of scale, no fixed costs and no thresholds. When reality has fixed costs (a server costs
      a full instance even at 10% use), you need integer variables or approximations.
    options:
      - text: "Proportionality: doubling an activity doubles its resource use (and its contribution)"
        correct: true
      - text: "Fixed setup costs"
      - text: "Exponential growth"
      - text: "Diminishing returns"
  - id: widget-solve
    prompt: >-
      Widget LP: maximize 10A + 5B + 15C subject to 3A + 3B + C ≤ 100 (nickel), 4A + 2B + 8C ≤ 200
      (steel), A, B, C ≥ 0. Making only C gives $375. What is optimal?
    explanation: >-
      “An optimal solution to the LP model is to earn $450 by producing 30 units of widget A, none of
      widget B, and 10 units of widget C” (p. 98). Check: nickel 90 + 10 = 100 ✓, steel 120 + 80 = 200 ✓,
      profit 300 + 150 = 450. Both resources are fully used (binding). Putting everything into the most
      profitable product (C) is not optimal, because C is steel-hungry. Mixing in A uses nickel that
      would otherwise sit idle. Greedy ‘best margin first’ allocation fails when there are several
      constraints, which is relevant to cloud capacity planning across CPU and memory.
    options:
      - text: "30 A and 10 C for $450"
        correct: true
      - text: "33⅓ A for $333⅓"
      - text: "50 B for $250"
      - text: "25 C for $375"
  - id: hotelling-vonneumann
    prompt: >-
      When Hotelling objected “But we all know the world is nonlinear,” what was von Neumann's reply?
    explanation: >-
      “The speaker titled his talk ‘linear programming’ and carefully stated his axioms. If you have an
      application that satisfies the axioms, well use it. If it does not, then don’t” (p. 98). A model is
      a tool with stated assumptions, and you check whether your problem meets them. In practice LP is “a
      multibillion-dollar-a-year business” (p. 99), and many nonlinear realities can be approximated or
      expressed with linear models plus integer variables. Dantzig’s colleagues hung a cartoon: “Happiness
      is assuming the world is linear” (p. 99).
    options:
      - text: "He proposed quadratic programming instead"
      - text: "He agreed LP was useless"
      - text: "Use it when your application satisfies the stated assumptions; otherwise don't"
        correct: true
      - text: "He claimed the world is linear"
  - id: program-meaning
    prompt: >-
      In “linear programming,” what does *programming* originally mean?
    explanation: >-
      In Dantzig’s Air Force context, a program meant “proposed schedules of training, logistical supply
      and deployment of combat units,” not software (p. 95). His Pentagon colleagues asked him to
      mechanize this planning, and LP was the result. “Mathematical programming” means optimization, and
      the term is older than computer programming.
    options:
      - text: "Television scheduling"
      - text: "Assembly language"
      - text: "Writing computer code"
      - text: "Planning/scheduling (military “programs” of logistics and deployment)"
        correct: true
  - id: cloud-lp
    prompt: >-
      A team chooses how many of three VM types to run to maximize throughput subject to CPU, memory
      and budget limits, with throughput and usage proportional to counts. Treating counts as
      continuous, what is this?
    explanation: >-
      Variables are VM counts (≥ 0). Each resource gives a linear constraint (Σ usageᵢ·xᵢ ≤ capacity) and
      throughput is a linear objective: the widget model with nickel and steel replaced by CPU and memory
      (pp. 97–98). Fractional VMs are not real, so in practice you round or add integrality (an integer
      program, p. 122). The LP optimum is still an upper bound on any whole-number plan.
    options:
      - text: "A shortest-path problem"
      - text: "A linear program (an integer program if counts must be whole)"
        correct: true
      - text: "A sorting problem"
      - text: "A TSP"
---
## Learning objectives

- Formulate a linear program: variables, linear constraints, linear objective.
- Solve the widget example and see why greedy allocation fails.
- Know LP’s assumptions and when to drop them.

## Dantzig’s model

LP grew out of Dantzig’s Pentagon work on mechanizing military planning (“programs”), influenced by Leontief’s input–output economics (pp. 94–95). Three ingredients (pp. 96–97):

1. **Inequalities** allowed, notably nonnegativity (x ≥ 0).
2. **Linearity**: no products or square roots of variables. Doubling an activity doubles what it uses.
3. **A linear objective**, maximized or minimized.

## Widgets

```text
maximize   10A + 5B + 15C
subject to  3A + 3B +  C ≤ 100   (nickel)
            4A + 2B + 8C ≤ 200   (steel)
            A, B, C ≥ 0
```

Optimal: **A = 30, B = 0, C = 10, profit $450**. Producing only C, the highest-margin product, gives just $375 (pp. 97–98).

## The lesson

Hotelling’s “the world is nonlinear” met von Neumann’s “if your application satisfies the axioms, use it” (p. 98). Today LP is a multibillion-dollar industry covering refineries, airlines, farms, power and more (p. 99).

## In software terms

- Capacity planning, cost-optimized instance mixes, traffic engineering (multi-commodity flow), and ad budget allocation are LPs.
- When you need whole numbers or yes/no decisions, move to integer programming (later in this chapter).

## Sources

- Cook (2012), pp. 94–99.
- Dantzig, G. B. *Linear Programming and Extensions*. Princeton, 1963.
- Williams, H. P. *Model Building in Mathematical Programming*. Wiley (5th ed. 2013).
