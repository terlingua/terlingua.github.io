---
title: "The Simplex Algorithm (pp. 99–105)"
questions:
  - id: dictionary
    prompt: >-
      In Chvátal's presentation, the widget problem's starting *dictionary* is Z = 10A + 5B + 15C,
      N = 100 − 3A − 3B − C, S = 200 − 4A − 2B − 8C. What is the “natural solution” of a dictionary?
    explanation: >-
      Set the right-hand-side variables to zero and read off the left-hand side. Initially A = B = C = 0
      gives Z = 0, N = 100 and S = 200 (p. 100). N and S are *slack* variables, the unused nickel and
      steel. Each pivot rewrites the dictionary so that a different set of variables sits on the right,
      giving a new natural solution. Geometrically, each natural solution is a corner of the feasible
      region.
    options:
      - text: "Set every variable to one"
      - text: "Set the left-hand variables to zero"
      - text: "Set right-hand-side variables to zero and read off the left-hand values"
        correct: true
      - text: "Average all variables"
  - id: ratio-test
    prompt: >-
      Increasing A from 0 with B = C = 0: nickel allows A ≤ 33⅓ and steel allows A ≤ 50. Which variable
      leaves, and why?
    explanation: >-
      “In this case nickel is the binding constraint” (p. 101). The tightest limit wins (the minimum
      ratio), so A rises to 33⅓ and N drops to 0. A moves to the left-hand side of the dictionary and N to the right:
      “A is called the incoming variable and N the leaving variable” (p. 101). Choosing the larger limit
      would drive N negative, which means using more nickel than exists. Vanderbei’s pivot tool warns
      you if you pick a leaving variable that does not correspond to a binding constraint (p. 103).
    options:
      - text: "S leaves, because 50 is larger"
      - text: "N leaves, because nickel runs out first (minimum ratio 33⅓)"
        correct: true
      - text: "A leaves"
      - text: "Both N and S leave"
  - id: reduced-cost
    prompt: >-
      After the first pivot, Z = 333⅓ − 3⅓N − 5B + 11⅔C. What does this tell you?
    explanation: >-
      “We see negative terms for N and B, implying that profit will decrease if we attempt to raise
      either of their levels. Fortunately, the term for C is positive, so we should attempt to increase
      its value with another pivot operation” (p. 102). The coefficients are **reduced costs**: the
      marginal change in objective per unit increase of a right-side variable. A positive coefficient
      when maximizing means there is more profit available.
    options:
      - text: "The solution is optimal"
      - text: "Increasing C would raise profit (positive coefficient); increasing N or B would lower it"
        correct: true
      - text: "B should be increased"
      - text: "The problem is infeasible"
  - id: optimality-proof
    prompt: >-
      The final dictionary has Z = 450 − N − (3/2)B − (7/4)S. Why does this *prove* optimality?
    explanation: >-
      “N, B, and S must all be nonnegative in any assignment, thus our profit is $450 minus a nonnegative
      number. And that is that” (p. 102). The objective row is an identity that every feasible solution
      satisfies, rewritten so that every remaining coefficient is ≤ 0. No feasible point can beat 450.
      This is the simplex stopping rule. It is checkable, but checking it requires trusting the pivot
      arithmetic, which is the motivation for duality in the next section.
    options:
      - text: "Because there were only two pivots"
      - text: "Because the algorithm ran out of time"
      - text: "Because Z equals 450 minus nonnegative quantities for every feasible solution"
        correct: true
      - text: "Because A = 30 is an integer"
  - id: pivot-rule-open
    prompt: >-
      What is known about the worst-case running time of the simplex method?
    explanation: >-
      “The simplex algorithm comes without a guarantee of polynomial running time … It is not known if
      there exists a plan for choosing the incoming and leaving variables at each step in such a way that
      we are sure to reach an optimal dictionary after only a polynomial number of pivots. In fact, it is
      known that several natural pivot rules are not polynomial” (p. 103). There are pathological
      examples with exponentially many pivots (Klee–Minty, 1972). Polynomial LP algorithms do exist,
      Khachiyan’s ellipsoid (1979) and Karmarkar’s interior point (1984), but no pivot rule is proven
      polynomial. Even Dantzig at first thought the method “might be efficient but not necessarily
      practical” (p. 103).
    options:
      - text: "Simplex is proven polynomial"
      - text: "No pivot rule is known to be polynomial; several natural rules have exponential worst cases"
        correct: true
      - text: "Simplex is proven exponential for every rule"
      - text: "Simplex is NP-complete"
  - id: theory-vs-practice
    prompt: >-
      Khachiyan (1979) and Karmarkar (1984) made front pages with polynomial-time LP algorithms. Did
      simplex die?
    explanation: >-
      “Dantzig’s method was not to be dethroned so easily. New computer implementations in the late
      1980s, including Bob Bixby’s CPLEX code … and John Forrest’s OSL code at IBM, showed the algorithm
      had plenty of punch left … in the year 2000 it was named one of ‘The Top Ten Algorithms of the
      Century’ and it remains the workhorse of the field” (p. 100). Worst-case theory and practical speed
      can disagree. Later work on smoothed analysis (Spielman–Teng, 2004) helped explain why: small
      random perturbations make simplex polynomial in expectation.
    options:
      - text: "Simplex was banned"
      - text: "Yes, interior-point methods replaced it entirely"
      - text: "No — engineered simplex codes stayed dominant; worst-case bounds don't decide practical performance"
        correct: true
      - text: "Yes, ellipsoid is used everywhere"
  - id: million-times
    prompt: >-
      Bixby measured a million-fold speedup in LP solving between 1987 and 2002. Where did it come
      from?
    explanation: >-
      “Three orders of magnitude in machine speed and three orders of magnitude in algorithmic speed add
      up to six orders of magnitude in solving power. A model that might have taken a year to solve 10
      years ago can now solve in less than 30 seconds” (pp. 104–105). Software and algorithms improved
      about as much as hardware did. The first large test, a 1947 diet problem with 9 constraints and 77
      variables, took 120 person-days on desk calculators (p. 104).
    options:
      - text: "Entirely from faster hardware"
      - text: "Roughly 1000× from hardware and 1000× from algorithms and implementation"
        correct: true
      - text: "Entirely from parallelism"
      - text: "From quantum computing"
  - id: simplex-name
    prompt: >-
      Where does the name “simplex” come from?
    explanation: >-
      It is not a play on “simple.” “In geometry a simplex is a classical object, namely an
      n-dimensional polytope having n + 1 corner points … in 2-dimensional space a simplex is just a
      triangle” (p. 105). Motzkin suggested it because the method, “viewed in the geometry of the columns,
      was best described as a movement from one simplex to a neighboring one” (p. 105). Cook would have
      preferred “Dantzig’s algorithm.”
    options:
      - text: "From a company called Simplex"
      - text: "From “simplex” radio communication"
      - text: "From “simple,” as a marketing term"
      - text: "From the geometric simplex (n + 1 corners in n dimensions), suggested by Motzkin"
        correct: true
---
## Learning objectives

- Run simplex by hand with dictionaries: entering and leaving variables, ratio test, reduced costs.
- Read an optimal dictionary as a proof.
- Contrast worst-case theory with practical performance.

## Pivoting the widget LP

Start: Z = 10A + 5B + 15C, N = 100 − 3A − 3B − C, S = 200 − 4A − 2B − 8C. Natural solution: everything on the right is 0.

1. **A enters** (positive coefficient). Ratio test: nickel limits A to 33⅓ and steel to 50, so **N leaves**. Z = 333⅓ (pp. 100–101).
2. **C enters** (coefficient 11⅔ > 0). Limits: 100 from the A row, 10 from the S row, so **S leaves** (p. 102).
3. Final: **Z = 450 − N − (3/2)B − (7/4)S**, A = 30, C = 10. All coefficients ≤ 0, so this is optimal (p. 102).

Geometrically, simplex walks from corner to corner of the feasible region (p. 119).

## Complexity vs practice

- No pivot rule is known to be polynomial, and natural ones have exponential examples (p. 103).
- Polynomial LP algorithms exist: Khachiyan’s ellipsoid (1979) and Karmarkar’s interior point (1984) (pp. 99–100).
- Simplex remains the workhorse. LP solving got about 10⁶× faster from 1987 to 2002, half from hardware and half from algorithms (pp. 100, 104–105).

## In software terms

- Simplex supports **warm starts**: after adding a constraint, re-optimize from the previous basis. This is why cutting-plane methods (chapter 6) use it.
- Shadow prices and reduced costs are actionable sensitivity data (“what is one more unit of steel worth?”).
- Algorithmic work can match hardware gains. Profile and improve the algorithm before buying hardware.

## Sources

- Cook (2012), pp. 99–105.
- Chvátal, V. *Linear Programming*. W. H. Freeman, 1983.
- Klee, V., G. J. Minty. “How good is the simplex algorithm?” In *Inequalities III* (1972), 159–175.
- Bixby, R. E. “Solving real-world linear programs: a decade and more of progress.” *Operations Research* 50 (2002), 3–15.
- Spielman, D. A., S.-H. Teng. “Smoothed analysis of algorithms.” *J. ACM* 51 (2004), 385–463.
