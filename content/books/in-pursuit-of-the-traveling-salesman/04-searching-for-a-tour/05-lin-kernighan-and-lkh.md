---
title: "Lin–Kernighan and LKH (pp. 79–84)"
questions:
  - id: why-not-kopt-direct
    prompt: >-
      Why can't we just check all k-opt moves for large k?
    explanation: >-
      “The computational burden of searching directly for improving k-opt moves makes the process
      impractical for k much larger than 2 or 3” (p. 79). There are about nᵏ ways to choose k edges
      to remove, plus many ways to reconnect. Lin and Kernighan’s idea was to find good k-opt moves
      *without* enumerating them, by growing the move one exchange at a time and pruning with a
      gain rule.
    options:
      - text: "Because 3-opt is always optimal"
      - text: "Because k-opt moves are illegal"
      - text: "The number of candidate k-edge exchanges grows roughly like nᵏ, so direct search is impractical beyond k ≈ 3"
        correct: true
      - text: "Because tours have only 3 edges"
  - id: lk-red-blue
    prompt: >-
      In the Lin–Kernighan search, “reds” are tour edges to remove and “blues” are new edges to add.
      Which rule keeps the search going?
    explanation: >-
      The search chains red–blue pairs. “The search continues to further red-blue pairs, as long as
      the sum of blue costs is less than the sum of red costs” (p. 80). This is the *positive gain
      criterion*. At each step there is a unique “come home” edge that closes a valid tour, and the
      best closed move seen so far is recorded. When no extension is possible it backtracks to try
      other blues at earlier levels. Finally it applies the best recorded move and repeats (pp. 79–81).
      The cumulative-gain rule lets individual steps look bad as long as the running total stays
      positive.
    options:
      - text: "Continue while each individual blue is cheaper than its red"
      - text: "Continue while the cumulative blue cost stays below the cumulative red cost; record the best closed tour seen"
        correct: true
      - text: "Continue until k = 3"
      - text: "Continue while the tour is getting longer"
  - id: lk-come-home
    prompt: >-
      Why is the step-by-step red–blue (sequential) structure so convenient to implement?
    explanation: >-
      “A great feature of working with a red-blue sequence is that at each step there is only one way
      to come home. In other words, if we remove two edges from a tour, then there is a unique way to
      hook up the resulting subpaths to obtain a new tour” (p. 83). Every partial move can be closed
      into a valid tour in exactly one way, so evaluating ‘what if I stop here’ is cheap. LKH’s 5-opt
      steps lose this: there are 148 ways to join five subpaths (p. 83).
    options:
      - text: "It needs no distance matrix"
      - text: "At each step there is exactly one way to close the tour, so evaluating partial moves is easy"
        correct: true
      - text: "It always finds the optimum"
      - text: "It never backtracks"
  - id: lk-legacy
    prompt: >-
      Lin–Kernighan (1973) was designed for a few hundred cities. What is its legacy?
    explanation: >-
      “Their basic method, designed for instances with only several hundred cities or fewer, has
      served as the cornerstone for the majority of the most successful TSP heuristic methods
      developed in the past several decades” (p. 82). Current implementations handle “ten million
      cities and more” (p. 81). It solves the 42-city USA instance optimally in five iterations from
      the 2-opt tour (p. 81). Cook calls it “one of the great achievements of TSP research” (p. 79).
      Their “crystal-clear exposition” (p. 81) made it reproducible, a reminder that clear technical
      writing is part of an algorithm’s success.
    options:
      - text: "It replaced linear programming in exact solvers"
      - text: "It was abandoned after the 1970s"
      - text: "It underlies most of the strongest TSP heuristics and scales, when engineered, to millions of cities"
        correct: true
      - text: "It is only used for 42-city instances"
  - id: lkh-five-opt
    prompt: >-
      What was Keld Helsgaun's key change in LKH (1998)?
    explanation: >-
      “Whereas standard Lin-Kernighan can be viewed as a search for a sequence of 2-opt exchanges …
      the new method searches for a sequence of 5-opt exchanges” (p. 82). Checking all 5-red/5-blue
      sets would be far too slow, so LKH restricts itself to *sequential* 5-opt moves, those a
      red–blue search could produce if the per-step gain condition were ignored. This reaches
      improvements that standard LK cannot find. Result: “for a typical 100-city problem the optimal
      solution is found in less than a second, and for a typical 1000-city problem [the] optimum is
      found in less than a minute” (p. 82).
    options:
      - text: "Using genetic crossover"
      - text: "Using random restarts only"
      - text: "Replacing local search with LP"
      - text: "Making the basic step a sequential 5-opt exchange instead of a 2-opt step"
        correct: true
  - id: lkh-148
    prompt: >-
      How did Helsgaun implement the 148 reconnection cases of sequential 5-opt?
    explanation: >-
      Applegate and Cook looked for a clever trick and found that “in fact there was no stealthy
      method: the code contained a full listing of the 148 cases, independently covering each
      possibility. Helsgaun had put in a Herculean effort to write a correct and efficient code”
      (p. 83). Sometimes careful, exhaustive engineering is the breakthrough. Releasing the code let
      others verify and build on it.
    options:
      - text: "By random sampling of 10 cases"
      - text: "With a neural network"
      - text: "With a secret mathematical formula"
      - text: "By explicitly coding all 148 cases by hand"
        correct: true
  - id: code-generation
    prompt: >-
      Sequential 6-opt has 1,358 reconnection cases and 9-opt has 2,998,656. How did Applegate and
      Cook handle general k?
    explanation: >-
      First they wrote “a computer program that could produce the actual computer code to handle a
      k-opt move, for any value of k. A computer code building a computer code”. That produced
      120,228 lines for 6-opt, 1,259,863 for 7-opt and 17,919,296 for 8-opt, all in C (p. 83). 9-opt
      was out of reach, so they made the generator compute the needed steps *on the fly* during the
      search (p. 84). Code generation hits a size wall, and moving from ahead-of-time generation to
      runtime interpretation is the usual way past it, a trade-off familiar from JIT and AOT
      compilers.
    options:
      - text: "They generated C code for each k, then switched to computing reconnection steps on the fly when code size exploded"
        correct: true
      - text: "They used a lookup table for all k"
      - text: "They used quantum computers"
      - text: "They gave up beyond 5-opt"
  - id: pancakes
    prompt: >-
      What do pancake flipping (Gates and Papadimitriou, 1979) and 2-opt have in common?
    explanation: >-
      “A flip of a top portion of a stack of pancakes corresponds to a reversal of a subpath in a
      tour, which is what happens in a 2-opt move. An implementation of a k-opt code generator calls
      for an algorithm to find a minimum number of flips to rearrange a tour in the order produced by
      a k-opt move” (p. 84). Any k-opt move can be done as a sequence of reversals, and fewer
      reversals means faster code. Helsgaun used similar ideas to let users choose the move size, and
      ran 10-opt on the 24,978-city Sweden instance in 2003, producing the tour proved optimal in 2004
      (p. 84).
    options:
      - text: "A pancake flip is a subpath reversal; executing a k-opt move with few reversals is a pancake-sorting variant"
        correct: true
      - text: "Both use dynamic programming"
      - text: "Nothing; it is an anecdote"
      - text: "Both are NP-complete"
---
## Learning objectives

- Explain how Lin–Kernighan builds deep k-opt moves without enumerating them.
- Explain LKH’s 5-opt basic step and the engineering behind it.

## Lin–Kernighan (1973)

1. Pick a home city, a tour edge (red) at it, and a cheaper non-tour edge (blue) from its far end.
2. Extend with red–blue pairs while **total blue cost < total red cost**.
3. At each depth, close the tour with the unique “come home” edge and **record the best tour**.
4. At a dead end, backtrack to try other blues. Stop for time or when choices run out.
5. Apply the best recorded move and repeat. If nothing improved, try a new home city (pp. 79–81).

On the USA instance it reaches the optimum in 5 iterations from the 2-opt tour (p. 81). Engineered versions handle 10 million cities or more.

## LKH (Helsgaun, 1998)

- The basic step is a **sequential 5-opt** move: 10 edges at a time, with 148 reconnection cases, all coded by hand (pp. 82–83).
- It is optimal on typical 100-city instances in under a second and 1,000-city instances in under a minute (p. 82).
- Generalized to arbitrary k by generating code, then by computing reconnections on the fly. Linked to pancake flipping. Helsgaun ran 10-opt on Sweden in 2003 (pp. 83–84).

## In software terms

- **Variable-depth search** (keep going while cumulative gain is positive, remember the best point) works for many combinatorial problems, including graph partitioning: Kernighan–Lin (1970), the same Kernighan.
- **Code generation vs runtime interpretation:** 17.9 million generated lines for 8-opt shows where ahead-of-time generation stops paying.
- **Open-sourcing** LKH let the community verify it and build on it.

## Sources

- Cook (2012), pp. 79–84.
- Lin, S., B. W. Kernighan. “An effective heuristic algorithm for the traveling-salesman problem.” *Operations Research* 21 (1973), 498–516.
- Helsgaun, K. “An effective implementation of the Lin–Kernighan traveling salesman heuristic.” *European J. Oper. Res.* 126 (2000), 106–130.
- Gates, W. H., C. H. Papadimitriou. “Bounds for sorting by prefix reversal.” *Discrete Mathematics* 27 (1979), 47–57.
