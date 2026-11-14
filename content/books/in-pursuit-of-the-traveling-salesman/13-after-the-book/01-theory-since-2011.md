---
title: "Theory since 2011: Christofides Beaten, Sharper Limits"
questions:
  - id: kko
    prompt: >-
      What did Karlin, Klein and Oveis Gharan announce in 2020?
    explanation: >-
      *A (Slightly) Improved Approximation Algorithm for Metric TSP* (STOC 2021, *Operations Research* 2024): a
      randomized polynomial-time algorithm with approximation ratio 1.5 − ε for ε about 10⁻³⁶. It was the first
      improvement on Christofides for general metric TSP in over 40 years. The book called beating 1.5 “a
      pressing open problem” (p. 183). The method samples a random spanning tree from a max-entropy
      distribution guided by the subtour LP solution, then fixes parity with a matching, as in Christofides.
      The gain is tiny in practice and large in theory: the 1.5 barrier is gone.
    options:
      - text: "A polynomial-time exact TSP algorithm"
      - text: "A (1.5 − 10⁻³⁶)-approximation for metric TSP — the first improvement over Christofides"
        correct: true
      - text: "A proof that 1.5 is optimal"
      - text: "A 1.01-approximation"
  - id: kko-ingredients
    prompt: >-
      Which ingredients connect the Karlin–Klein–Oveis Gharan algorithm to concepts in this companion?
    type: multiple
    explanation: >-
      It starts from an optimal **subtour LP** solution (chapter 5), samples a **spanning tree** at random
      from a max-entropy distribution whose edge marginals match the LP (chapter 4’s trees), and adds a
      minimum-cost **matching** on odd-degree vertices before shortcutting an **Eulerian** walk, the
      Christofides skeleton (chapter 4). Its analysis bounds the expected matching cost using the LP solution.
      It does not use genetic algorithms.
    options:
      - text: "The subtour LP relaxation"
        correct: true
      - text: "Random spanning trees whose marginals follow the LP"
        correct: true
      - text: "Parity correction by matching, then an Eulerian walk"
        correct: true
      - text: "Genetic crossover"
  - id: kls
    prompt: >-
      The book cites the 220/219 ≈ 1.0045 inapproximability bound (Papadimitriou–Vempala). What is the
      improved bound?
    explanation: >-
      Karpinski, Lampis and Schmied (*New inapproximability bounds for TSP*, ISAAC 2013; *J. Computer and System
      Sciences* 2015) showed no polynomial-time algorithm can approximate metric TSP better than 123/122
      ≈ 1.0082 unless P = NP. The known range moved from [1.0045, 1.5] to [1.0082, 1.5 − 10⁻³⁶]. The gap Cook
      called “disturbing” (p. 183) has narrowed only slightly from each end.
    options:
      - text: "1.0001"
      - text: "3/2"
      - text: "123/122 ≈ 1.0082"
        correct: true
      - text: "2"
  - id: extension-complexity
    prompt: >-
      Woeginger's list included “P = NP” claims via polynomial-size LP formulations of the TSP (p. 178). What
      did Fiorini, Massar, Pokutta, Tiwary and de Wolf prove in 2012?
    explanation: >-
      The TSP polytope has **no polynomial-size extended formulation**. Any LP whose projection gives the TSP
      polytope, even with extra variables, needs exponentially many inequalities (STOC 2012, *J. ACM* 2015).
      This holds regardless of P vs NP, so a whole class of attempted proofs, compact LPs for the TSP, cannot
      work. It does not affect cutting-plane practice, which never writes the full description and generates
      cuts lazily (chapter 6).
    options:
      - text: "That the TSP polytope has few facets"
      - text: "That every LP describing the TSP polytope (even with extra variables) has exponential size, ruling out “compact LP” P = NP proofs"
        correct: true
      - text: "That P = NP"
      - text: "That cutting planes are useless"
  - id: hamiltonicity-faster
    prompt: >-
      Cook asked for a bound like n²(1.99)ⁿ to beat Held–Karp (p. 182). What is the status?
    explanation: >-
      For the *unweighted* undirected Hamiltonian cycle problem, Björklund (FOCS 2010; *SIAM J. Computing*
      2014) gave a randomized O*(1.657ⁿ) algorithm using algebraic ‘determinant sums’, the first to beat 2ⁿ.
      For the general *weighted* TSP with arbitrary costs, no algorithm with base below 2 is known, and
      Held–Karp’s O(n²2ⁿ) still stands. Special cases moved and the general weighted case did not.
    options:
      - text: "Held–Karp was beaten for general weighted TSP"
      - text: "Beaten for unweighted undirected Hamiltonicity (≈1.657ⁿ, randomized); general weighted TSP still at n²2ⁿ"
        correct: true
      - text: "A polynomial algorithm was found"
      - text: "Nothing changed for any variant"
  - id: still-open
    prompt: >-
      Which of the book's open problems were still open as of the mid-2020s?
    type: multiple
    explanation: >-
      P vs NP is open. The 4/3rds Conjecture for the subtour LP integrality gap is open, though special cases
      such as half-integral and graphic instances saw progress. Beating n²2ⁿ for general weighted TSP is open.
      The Mona Lisa instance is unsolved. Christofides’ 1.5 is *no longer* the best guarantee (KKO 2020).
    options:
      - text: "P vs NP"
        correct: true
      - text: "The 4/3rds Conjecture"
        correct: true
      - text: "Beating n²2ⁿ for general weighted TSP"
        correct: true
      - text: "Beating Christofides' 1.5 for metric TSP"
---
## Learning objectives

- Update the theory picture from the book (2011) to today.

## What changed

| Question in the book | Status at publication | Status now |
|---|---|---|
| Metric approximation below 1.5 (p. 183) | Open | **Solved, barely**: 1.5 − 10⁻³⁶ (Karlin–Klein–Oveis Gharan, 2020) |
| Inapproximability (p. 183) | 220/219 | **123/122** (Karpinski–Lampis–Schmied, 2013) |
| Compact LP for the TSP (p. 178) | Claimed in flawed P = NP papers | **Impossible**: exponential extension complexity (Fiorini et al., 2012) |
| Beat 2ⁿ (p. 182) | Open | Unweighted Hamiltonicity ≈ 1.657ⁿ (Björklund, 2010). **Weighted TSP still n²2ⁿ** |
| 4/3rds Conjecture (p. 117) | Open | Open |
| P vs NP | Open | Open |

## Takeaways

- Christofides’ structure (tree, then parity fix, then Euler) was not the limit. Randomizing the tree using LP information broke it.
- Lower bounds on LP *size* say nothing about lazy cutting-plane methods, which never write the whole LP.

## Sources

- Karlin, A. R., N. Klein, S. Oveis Gharan. “A (slightly) improved approximation algorithm for metric TSP.” *Proc. 53rd STOC* (2021), 32–45; *Operations Research* 72(6) (2024), 2543–2594. arXiv:2007.01409.
- Karpinski, M., M. Lampis, R. Schmied. “New inapproximability bounds for TSP.” *J. Computer and System Sciences* 81 (2015), 1665–1677. arXiv:1303.6437.
- Fiorini, S., S. Massar, S. Pokutta, H. R. Tiwary, R. de Wolf. “Exponential lower bounds for polytopes in combinatorial optimization.” *J. ACM* 62(2) (2015), article 17.
- Björklund, A. “Determinant sums for undirected Hamiltonicity.” *SIAM J. Computing* 43 (2014), 280–299.
