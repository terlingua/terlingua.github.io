---
title: "Capstone II: Bounds, Proofs, and Complexity"
questions:
  - id: bound-ladder
    prompt: >-
      Order these lower bounds from weakest to strongest on a typical geometric instance: subtour LP, MST,
      degree LP, subtour LP + combs.
    explanation: >-
      MST ≤ OPT (tour minus an edge, p. 72). The degree LP is usually at least as strong as the MST in practice
      (641 on USA42 against the MST’s 591). Its dual is control zones, and Held–Karp 1-trees with penalties
      reach the subtour LP. Subtour LP: 697 on USA42 (p. 117). Adding combs closes the rest: 699 (p. 131). Each
      family of valid constraints tightens the relaxation. (The MST and degree LP are not comparable on every
      instance, but this order is typical.)
    options:
      - text: "Degree LP, combs, MST, subtour LP"
      - text: "Subtour LP, MST, degree LP, combs"
      - text: "MST, degree LP, subtour LP, subtour + combs"
        correct: true
      - text: "Combs, subtour LP, degree LP, MST"
  - id: three-certificates
    prompt: >-
      Match each certificate to what it proves: (1) a tour, (2) a feasible dual LP solution, (3) a flow of value
      2 between s and t in the LP support graph.
    explanation: >-
      (1) A tour proves an **upper bound**: OPT ≤ its length (p. 5). (2) A dual solution proves a **lower
      bound**: every tour ≥ the dual objective, by weak duality (p. 108). (3) An s–t flow of 2 proves that **no
      subtour inequality separating s and t is violated**, by max-flow/min-cut (pp. 138–139). Each is cheap to
      check and independent of the software that produced it.
    options:
      - text: "(1) feasibility of the LP, (2) a tour, (3) a comb"
      - text: "(1) lower bound, (2) upper bound, (3) optimality"
      - text: "(1) upper bound on OPT, (2) lower bound on OPT, (3) no violated subtour cut separating s and t"
        correct: true
      - text: "All three prove optimality"
  - id: gap-diagnosis
    prompt: >-
      Your branch-and-cut stalls with a 2% gap after hours. The best tour has not changed since minute one, and
      the bound creeps up slowly. Following the World TSP lesson, what should you suspect?
    explanation: >-
      Do not assume which side is weak. For the World TSP, about 75% of the gap closure came from *better tours*
      (pp. 165–166). Run a stronger heuristic (LKH, longer chained LK) to test the upper side. On the bound side,
      add cut families (combs, domino-parity, local cuts), use strong branching, and check for tailing-off
      (pp. 146, 151). Measure both sides.
    options:
      - text: "The instance is infeasible"
      - text: "Add more CPU only"
      - text: "The bound must be the problem"
      - text: "Either side may be weak — try stronger heuristics for the tour and stronger cuts/branching for the bound"
        correct: true
  - id: np-hard-claim
    prompt: >-
      A colleague says: “Our 200-stop routing is NP-hard, so we can't get optimal answers.” What is the best
      response?
    explanation: >-
      NP-hardness is a worst-case statement about the general problem (chapter 1, p. 6). Instances of 200 cities
      are routinely solved optimally by Concorde in seconds to minutes, and much larger records exist (85,900;
      81,998 road stops). The right question is whether your instances and time budget allow exact solving
      or whether a heuristic with a reported gap is better. That is the algorithm-engineering mindset (p. 11).
    options:
      - text: "Agree; switch to random routes"
      - text: "NP-hard is a worst-case class statement; 200-stop instances are typically solved optimally, so test exact methods and report gaps"
        correct: true
      - text: "Use brute force"
      - text: "Wait for quantum computers"
  - id: quantum-pitch
    prompt: >-
      A vendor pitches “quantum TSP solving: all routes evaluated at once.” Which questions does the book equip
      you to ask?
    type: multiple
    explanation: >-
      Measurement gives one outcome, so how do you amplify the best tour without exploitable structure
      (Aaronson, pp. 189–190)? Where is the exponential cost hidden, in qubits, repetitions or precision, as
      with the optical machine (p. 188)? How does it compare with LKH on standard instances under DIMACS-style
      reporting (p. 92)? Its office location has nothing to do with any of this.
    options:
      - text: "How is the best tour amplified given measurement collapse?"
        correct: true
      - text: "Where does the exponential cost go (qubits, repetitions, precision)?"
        correct: true
      - text: "How does it compare to LKH on TSPLIB with gaps reported?"
        correct: true
      - text: "Is their office near a data center?"
  - id: separation-equals-optimization
    prompt: >-
      Why would a polynomial-time separation algorithm for the full TSP polytope be worth $1,000,000?
    explanation: >-
      By Grötschel–Lovász–Schrijver, polynomial separation over a polytope gives polynomial optimization over
      it via the ellipsoid method (p. 143). Optimizing over the TSP polytope solves the TSP, which is
      NP-complete, so this would prove P = NP and win the Clay prize (pp. 143–144). Extension-complexity results
      (chapter 13) do not rule this out, because separation does not require writing down the polytope.
    options:
      - text: "Because separation is used in cryptography"
      - text: "Separation ⇔ optimization (GLS); optimizing over the TSP polytope solves an NP-complete problem, implying P = NP"
        correct: true
      - text: "Because Clay funds LP research"
      - text: "It wouldn't be"
  - id: dp-when
    prompt: >-
      For which situation is Held–Karp dynamic programming the right tool?
    explanation: >-
      Exact optimality on tiny instances (n ≲ 20–25) with arbitrary costs, including asymmetric or non-metric
      ones, and a predictable runtime of O(n²2ⁿ) time and O(n2ⁿ) memory (pp. 180–182). Examples are a
      12-stop daily route or ordering 15 test suites. For hundreds of cities, LP-based branch-and-cut or
      heuristics win. Memory, 2²⁵ × 25 entries, runs out first.
    options:
      - text: "Approximate answers for millions of nodes"
      - text: "1,000-city routing every minute"
      - text: "Exact answers for ≈12–20 nodes with arbitrary (even asymmetric) costs"
        correct: true
      - text: "The World TSP"
  - id: final-synthesis
    prompt: >-
      Which sentence best summarizes the book's technical core?
    explanation: >-
      Heuristics (chapter 4) supply tours, which are upper bounds. Linear programming (chapter 5) turns simple
      rules satisfied by all tours into lower bounds, with duality as the certificate. Cutting planes (chapter 6)
      strengthen the relaxation lazily through separation. Branching (chapter 7) splits the search when cuts
      stall. Optimality is proved when the bound meets the tour (chapter 8). Complexity theory (chapter 9)
      explains why no polynomial guarantee is expected, while algorithm engineering keeps solving larger
      instances anyway.
    options:
      - text: "The TSP is polynomial in practice and theory"
      - text: "Faster computers will eventually brute-force the TSP"
      - text: "Good tours from heuristics + LP lower bounds strengthened by cuts and branching = proven optimal tours, despite NP-hardness"
        correct: true
      - text: "Genetic algorithms prove optimality"
---
## Purpose

A final self-check across bounds, certificates and complexity. If you can answer these without looking back, you have the core of the book.

## The whole pipeline in one picture

```text
            ┌──────── heuristics (NN, greedy, LK, LKH, EAX) ────────┐
instance ──►│                                                        │──► tour  (upper bound)
            └────────────────────────────────────────────────────────┘
            ┌── LP relaxation ─► separation ─► cuts ─► branch ─► … ──┐
            │   (degree, subtours, combs, domino, local cuts)        │──► bound (lower bound)
            └────────────────────────────────────────────────────────┘
                     tour == bound  ⇒  optimal (+ dual certificate)
```

## Sources

- Cook (2012), as cited in each explanation.
