---
title: "An Impossible Task? (pp. 6–10)"
questions:
  - id: solution-means-algorithm
    prompt: >-
      When researchers say the TSP is “unsolved,” what do they mean by a *solution*?
    explanation: >-
      Cook is explicit: “By a solution we mean an algorithm, that is, a step-by-step recipe for
      producing an optimal tour for any example we may throw at it. Just finding the best route
      through the United States or any other country does not do the job” (p. 6). Solving
      individual instances, even huge ones, is a different achievement. Think of the difference
      between passing one test case and shipping a function that is correct and fast on every
      input. The open question is about the general function and its running time.
    options:
      - text: "A heuristic that is usually within 1% of optimal"
      - text: "An algorithm that produces an optimal tour for every instance, within acceptable time"
        correct: true
      - text: "A closed-form formula for the length of the optimal tour"
      - text: "An optimal tour for the largest known instance"
  - id: good-algorithm
    prompt: >-
      Jack Edmonds defined a “good” algorithm. Which running-time bounds are *good* in his
      sense (n is the input size)?
    type: multiple
    explanation: >-
      “A good algorithm is one that comes with a guarantee to complete its work in time at most
      proportional to nᵏ for some power k. The power k can be any value, such as 2, 3, or more,
      but it must be a fixed number—it cannot increase as n gets larger. Thus, a growth rate of
      n³ is good, but growth rates of nⁿ and 2ⁿ are bad” (p. 8). n log n is bounded by n², so it
      is polynomial. n¹⁰⁰⁰ is technically good even though impractical — Cook notes Edmonds’s
      definition “might not always agree with our intuition” (p. 8). n² · 2ⁿ (Held–Karp) is
      exponential. Today the term is *polynomial-time*.
    options:
      - text: "n³"
        correct: true
      - text: "n log n"
        correct: true
      - text: "n² · 2ⁿ"
      - text: "n¹⁰⁰⁰"
        correct: true
      - text: "nⁿ"
  - id: table-growth
    prompt: >-
      Table 1.1 compares n³ and 2ⁿ on a machine doing 10⁹ operations per second. At n = 100,
      n³ takes 0.001 seconds. Roughly how long does 2ⁿ take?
    explanation: >-
      The table gives 2ⁿ times of 0.000001 s at n = 10, 0.03 s at n = 25, 13 days at n = 50, and
      “40 trillion years” at n = 100 (p. 8). At n = 10 the exponential algorithm is perfectly
      fine, so small benchmarks tell you nothing about it. Each extra unit of n *doubles* the
      time, so going from 50 to 100 multiplies 13 days by 2⁵⁰ ≈ 10¹⁵. This is why load tests
      on toy inputs can be badly misleading for exponential code paths (backtracking regexes,
      naive subset enumeration, unindexed joins).
    options:
      - text: "About one hour"
      - text: "About 13 days"
      - text: "About one second"
      - text: "About 40 trillion years"
        correct: true
  - id: held-karp-best
    prompt: >-
      What is the best known worst-case running time for an exact algorithm for the general TSP,
      as reported in the book?
    explanation: >-
      “The best result thus far is a solution method, discovered in 1962, that runs in time
      proportional to n²2ⁿ. Although not good, this growth rate is much smaller than the total
      number of tours through n points, which we know is (n − 1)!/2” (p. 9). This is the
      Held–Karp dynamic program covered in chapter 9. 2ⁿ grows far more slowly than n!, but it is
      still exponential. For general weighted TSP no algorithm with a smaller exponential base
      has been found since.
    options:
      - text: "n³, using linear programming"
      - text: "n² · 2ⁿ, from Held and Karp in 1962"
        correct: true
      - text: "(n − 1)!/2, brute force"
      - text: "n log n, using spanning trees"
  - id: class-np
    prompt: >-
      Why is the TSP (asked as “is there a tour of length less than 100 miles?”) in the class NP?
    explanation: >-
      NP consists of “all problems for which we can check the correctness of a solution in
      polynomial time” (p. 9). If someone hands you a tour, you add up n edge costs and compare
      with 100 — quick work. “A glimmer of hope is that at least we know a good tour when we see
      one” (p. 9). NP stands for *nondeterministic polynomial*, not ‘non-polynomial’. Every
      problem in P is also in NP. In engineering terms, NP problems are those where verifying a
      proposed answer is cheap, like checking a signed artifact against a hash, even when finding
      the answer may be expensive.
    options:
      - text: "Because no polynomial algorithm is known for it"
      - text: "Because NP means “non-polynomial”"
      - text: "Because a proposed tour can be checked against the target in polynomial time"
        correct: true
      - text: "Because it has exponentially many feasible solutions"
  - id: np-complete-meaning
    prompt: >-
      Cook's Theorem (Stephen Cook, 1971) and Karp's follow-up made the TSP NP-complete. What
      would a polynomial-time algorithm for the TSP imply?
    explanation: >-
      “Cook’s Theorem states that there exists a problem in NP such that if we have a good
      algorithm for this single problem, then there is a good algorithm for every problem in NP.
      In fact, Cook, Karp, and others have shown that there are many such NP-complete problems,
      the most prominent being the TSP itself. Finding a good algorithm for an NP-complete
      problem would show that P is equal to NP” (p. 9). That is worth the Clay Institute’s
      $1,000,000 (p. 10). NP-completeness is a statement about the worst case of the general
      problem. It does not say that your particular instances are hard.
    options:
      - text: "P = NP — every problem whose solutions can be checked quickly could also be solved quickly"
        correct: true
      - text: "Only that routing problems become easy"
      - text: "That NP-complete problems are undecidable"
      - text: "Nothing, because the TSP is not in NP"
  - id: crypto-consequence
    prompt: >-
      Why do security engineers care about whether P = NP?
    explanation: >-
      “Current encryption systems make use of the assumption that certain NP problems are
      difficult to solve. Internet commerce would grind to a halt if there were quick algorithms
      for these members of NP; this would be like handing code breakers and hackers a Swiss
      Army knife for snooping data” (p. 10). Checking a decryption key is easy, so breaking a
      cipher is an NP-type search. Cook balances this with Lance Fortnow’s view that P = NP
      would make optimization easy across logistics, science and engineering, so that “what we
      will gain from P = NP will make the whole Internet look like a footnote in history” (p. 10).
      Most experts bet P ≠ NP, but there is “no great theoretical reason” behind that bet.
    options:
      - text: "Because widely used cryptography relies on certain NP problems being hard to solve"
        correct: true
      - text: "Because P = NP would make hashing impossible"
      - text: "They do not; cryptography is unrelated to NP"
      - text: "Because TLS uses the TSP to choose routes"
  - id: three-quotes
    prompt: >-
      Flood (1956), Edmonds (1967) and Karp (1972) are quoted on TSP difficulty. Which best
      describes how the view developed?
    explanation: >-
      Flood suggested “there may well be no general method for treating the problem and
      impossibility results would also be valuable”. Edmonds sharpened it: “I conjecture that
      there is no good algorithm for the traveling salesman problem”. Karp then gave “theorems
      which strongly suggest, but do not imply” that such problems “will remain intractable
      perpetually” (p. 6). The progression runs from intuition, to a precise conjecture built on
      the concept of a good algorithm, to the NP-completeness theory that ties the TSP to
      thousands of other problems. None of them is a proof. The question is still open.
    options:
      - text: "All three proved that no good algorithm exists"
      - text: "From proof of impossibility to a found algorithm"
      - text: "From an intuition of hardness, to a precise conjecture, to a theory linking the TSP's fate to many other problems — still unproven"
        correct: true
      - text: "From a solved problem to an unsolved one"
---
## Learning objectives

- Define what a *solution* to the TSP means: an algorithm, not an answer.
- Classify running times as polynomial (“good”) or exponential (“bad”).
- Explain P, NP, NP-completeness and why the TSP sits at the center of P vs NP.
- Connect the question to cryptography and to everyday engineering.

## A solution is an algorithm

Solving the 49-city USA tour did not “solve the TSP.” A solution means an algorithm that returns an optimal tour **for every instance** (p. 6). Merrill Flood (1956), Jack Edmonds (1967) and Richard Karp (1972) all bet against an efficient one (p. 6).

## Good and bad algorithms

Edmonds made “good” precise. An algorithm is good if its running time is at most proportional to **nᵏ** for some fixed k (p. 8). n³ is good. 2ⁿ and nⁿ are bad. The boundary does not always match practice, since n¹⁰⁰⁰ counts as good. Its value is that it gives researchers a clear target, and once a problem is known to be in P, researchers drive the exponent down (p. 8).

| n | n³ | 2ⁿ |
|---|---|---|
| 10 | 0.000001 s | 0.000001 s |
| 25 | 0.00002 s | 0.03 s |
| 50 | 0.0001 s | 13 days |
| 100 | 0.001 s | 40 trillion years |

*(Table 1.1, at 10⁹ operations per second, p. 8.)*

The best exact TSP algorithm runs in time proportional to **n²·2ⁿ** (Held–Karp, 1962). That is far better than (n − 1)!/2 but still exponential (p. 9).

## P, NP and the big question

- **P**: decision problems with polynomial-time algorithms.
- **NP**: problems whose *yes* answers can be checked in polynomial time. “Is there a tour shorter than 100 miles?” is in NP because a proposed tour is easy to check (p. 9).
- **NP-complete**: problems in NP to which every NP problem reduces. Stephen Cook showed in 1971 that such problems exist. Karp and others showed that many natural problems, the TSP among them, are NP-complete (p. 9).

A polynomial algorithm for the TSP would prove **P = NP** and win the Clay Institute’s $1,000,000 prize (p. 10). Most researchers bet against it. A fast algorithm would undermine cryptography, but it would also make optimization easy almost everywhere (Fortnow, p. 10).

## In software terms

- **Verification vs search.** Checking a build artifact’s signature is cheap. Finding a collision should be infeasible. That gap is the P vs NP question in practice.
- **NP-hard is a worst-case statement.** It does not stop you from solving your instances. Chapters 4–8 show 85,900-city instances solved to optimality.
- **Watch for hidden exponentials.** Nested subset loops, backtracking parsers and unconstrained schedulers behave like the 2ⁿ row above: fine at n = 10, disastrous at n = 100.

## Sources

- Cook (2012), pp. 6–10.
- Edmonds, J. “Optimum branchings.” *J. Res. Nat. Bur. Standards* 71B (1967), 233–240. The “good algorithm” idea also appears in Edmonds, “Paths, trees, and flowers,” *Canadian J. Math.* 17 (1965).
- Cook, S. A. “The complexity of theorem-proving procedures.” *Proc. 3rd ACM STOC* (1971), 151–158.
- Karp, R. M. “Reducibility among combinatorial problems.” In *Complexity of Computer Computations* (1972), 85–103.
- Fortnow, L. “The status of the P versus NP problem.” *Communications of the ACM* 52(9) (2009), 78–86.
