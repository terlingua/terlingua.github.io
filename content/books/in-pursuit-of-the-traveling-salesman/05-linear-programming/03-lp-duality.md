---
title: "Two for the Price of One: LP Duality (pp. 105–108)"
questions:
  - id: convince-manager
    prompt: >-
      Why does Cook introduce duality with the problem of convincing a manager?
    explanation: >-
      The optimal dictionary is a proof, but checking it means redoing every pivot or auditing “the
      100,000 or so lines of computer code in Bob Bixby’s LP solver … What we need is a concise route to
      the dictionary’s proof. Providing such a route is the role of duality” (p. 105). A dual solution is
      a short **certificate** that anyone can verify with a few multiplications, without trusting the
      solver. The same idea appears in the 85,900-city certificate in chapter 8 and in proof-carrying
      code generally.
    options:
      - text: "Because it avoids nonnegativity"
      - text: "Because managers prefer geometry"
      - text: "Because a dual solution is a short, independently checkable certificate of optimality"
        correct: true
      - text: "Because duality makes the LP faster"
  - id: multipliers
    prompt: >-
      Multiply nickel (3A + 3B + C ≤ 100) by 1 and steel (4A + 2B + 8C ≤ 200) by 7/4 and add. What do
      you get and what does it prove?
    explanation: >-
      1·(3, 3, 1) + 1.75·(4, 2, 8) = (10, 6.5, 15) and 100 + 350 = 450, so 10A + 6.5B + 15C ≤ 450 for
      every feasible plan (p. 106). Since A, B, C ≥ 0, profit 10A + 5B + 15C ≤ 10A + 6.5B + 15C ≤ 450.
      No plan earns more than $450, and the plan with A = 30, C = 10 earns exactly 450, so it is optimal.
      Bonus: “we cannot earn more than $450, even if the profit margin on widget B were increased to
      $6.50 per unit” (p. 106).
    options:
      - text: "10A + 6.5B + 15C ≤ 450, so profit ≤ 450 for every feasible plan"
        correct: true
      - text: "The LP is infeasible"
      - text: "Profit ≥ 450 for every plan"
      - text: "10A + 5B + 15C = 450 exactly, for all plans"
  - id: dual-constraints
    prompt: >-
      Which conditions must the multipliers y_N, y_S satisfy for the combined inequality to bound
      profit?
    type: multiple
    explanation: >-
      “The values we extract … are both nonnegative, allowing us to use them as multipliers for the
      nickel and steel constraints” (multiplying a ≤ inequality by a negative number flips it). “When we
      add the two resulting constraints the values in front of A, B, and C are each at least as large as
      the profits” (p. 106). These rules form the **dual LP**: minimize 100y_N + 200y_S subject to
      3y_N + 4y_S ≥ 10, 3y_N + 2y_S ≥ 5, y_N + 8y_S ≥ 15, y ≥ 0 (p. 107). Each dual constraint
      corresponds to a primal variable and each dual variable to a primal constraint.
    options:
      - text: "y_N ≥ 0 and y_S ≥ 0"
        correct: true
      - text: "For each widget, the combined coefficient is at least its profit"
        correct: true
      - text: "y_N + y_S = 1"
      - text: "y_N and y_S must be integers"
  - id: weak-strong
    prompt: >-
      Distinguish weak duality from strong duality.
    explanation: >-
      **Weak:** any feasible dual solution gives a bound on the primal objective (here
      10A + 5B + 15C ≤ 100y_N + 200y_S). **Strong** (von Neumann): if both problems are feasible, their
      optimal values are *equal* (p. 108, note p. 216). “In our TSP discussion we really only need the
      much easier statement that any dual LP solution provides a bound … this is called the weak duality
      theorem” (p. 108). Any feasible dual is a lower bound on tour cost, whether or not it is optimal.
    options:
      - text: "They are the same theorem"
      - text: "Weak duality applies only to integers"
      - text: "Weak: optimal values are equal; strong: any dual gives a bound"
      - text: "Weak: any feasible dual bounds the primal; strong: the optimal values coincide"
        correct: true
  - id: shadow-price
    prompt: >-
      The optimal dual value for steel is y_S = 7/4. What is its economic meaning?
    explanation: >-
      Dual values are **shadow prices**: the marginal objective value of one more unit of a resource,
      within a range. One more pound of steel is worth about $1.75 of extra profit, and one more pound of
      nickel about $1 (y_N = 1). These are the same coefficients that appear, negated, in front of S and
      N in the final dictionary Z = 450 − N − (3/2)B − (7/4)S (pp. 102, 107). In ops terms they tell you
      which constraint to relax first: buying capacity where the shadow price is highest gives the most
      return.
    options:
      - text: "Steel costs $7/4 per pound to buy"
      - text: "One extra unit of steel would raise optimal profit by about $1.75 (its marginal value)"
        correct: true
      - text: "We should make 7/4 widgets"
      - text: "Steel is not binding"
  - id: certificate-check
    prompt: >-
      A solver returns primal x and dual y for a maximization LP. What must you check to accept the
      optimality claim *without trusting the solver*?
    explanation: >-
      Check primal feasibility (constraints and nonnegativity), dual feasibility (y ≥ 0 and the dual
      constraints) and equal objectives. Then weak duality proves optimality, the “two LP solutions for
      the price of one” argument (p. 107). The check is a few loops of arithmetic, independent of the
      solver’s 100,000 lines of code. Use exact or rational arithmetic, or careful tolerances, since
      floating point can fake equality. Concorde’s 85,900-city verification code is 6,646 lines of C
      (chapter 8).
    options:
      - text: "That the solver printed “optimal”"
      - text: "Re-run the same solver"
      - text: "Primal feasible, dual feasible, and equal objective values"
        correct: true
      - text: "Only that x is feasible"
---
## Learning objectives

- Build the dual of a small LP and interpret dual variables.
- State weak and strong duality and use them to certify optimality.

## Duality by example

Multiply the widget constraints by y_N = 1 and y_S = 7/4 and add: 10A + 6.5B + 15C ≤ 450, which dominates the profit function, so profit ≤ 450 (p. 106).

**Dual LP** (p. 107):

```text
minimize    100 yN + 200 yS
subject to    3 yN +   4 yS ≥ 10    (A)
              3 yN +   2 yS ≥  5    (B)
                yN +   8 yS ≥ 15    (C)
              yN, yS ≥ 0
```

- **Weak duality:** every feasible y bounds every feasible x: 10A + 5B + 15C ≤ 100y_N + 200y_S.
- **Strong duality** (von Neumann): the optimal values are equal, 450 = 450 (pp. 107–108).
- **Shadow prices:** y_S = 7/4 is the marginal value of steel.

```csharp
static bool CertifyMax(double[,] a, double[] b, double[] c, double[] x, double[] y, double eps = 1e-9)
{
    int m = b.Length, n = c.Length;
    for (var i = 0; i < m; i++)
    {
        var lhs = 0.0;
        for (var j = 0; j < n; j++) lhs += a[i, j] * x[j];
        if (lhs > b[i] + eps || y[i] < -eps) return false;
    }
    for (var j = 0; j < n; j++)
    {
        var lhs = 0.0;
        for (var i = 0; i < m; i++) lhs += a[i, j] * y[i];
        if (lhs < c[j] - eps || x[j] < -eps) return false;
    }
    var primal = c.Zip(x, (p, q) => p * q).Sum();
    var dual = b.Zip(y, (p, q) => p * q).Sum();
    return Math.Abs(primal - dual) <= eps * Math.Max(1, Math.Abs(primal));
}
```

## In software terms

- **Trust but verify.** Accept solver output only with a checkable certificate. This is the spirit of proof-carrying code and reproducible builds.
- **Shadow prices** for capacity planning: which constraint (CPU, memory, budget) is worth relaxing first?
- For the TSP, any feasible dual is a lower bound on every tour. You never need the optimal dual to have a valid bound.

## Sources

- Cook (2012), pp. 105–108.
- Chvátal, V. *Linear Programming* (1983), chapter 5.
- von Neumann, J. Discussion of duality (1947), reported in Dantzig (1963).
