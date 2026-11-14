---
title: "Cook's Theorem and Karp's List (pp. 174–178)"
questions:
  - id: decision-problems
    prompt: >-
      Complexity theory focuses on *decision* problems. How does that capture the optimization TSP?
    explanation: >-
      Decision problems have yes/no answers: “given a set of cities, is there a tour of length less than
      1,000 miles? Yes or no” (p. 174). “We can always find the length of a shortest tour by asking a sequence
      of yes/no questions” (note, p. 220), for example by binary search on K. With integer costs, a polynomial
      number of decision calls finds the optimum, so the decision and optimization versions stand or fall
      together for polynomial solvability.
    options:
      - text: "It doesn't; optimization is separate"
      - text: "Binary search over the threshold K turns a decision oracle into an optimizer with polynomially many calls"
        correct: true
      - text: "By asking one question"
      - text: "By enumerating tours"
  - id: p-definition
    prompt: >-
      What is the formal definition of P, and why is it robust?
    explanation: >-
      “P is the class of problems that can be solved in polynomial time on a single-tape Turing machine, that
      is … the machine is guaranteed to halt after a number of steps that is at most C times nᵏ” (p. 174).
      “We could replace the single-tape Turing machine by a multiple-tape machine, or even by a powerful
      modern digital computer, without altering the class” (p. 174), because simulations add only polynomial
      overhead.
    options:
      - text: "Problems solvable in linear time on a PC"
      - text: "Decision problems solvable in O(nᵏ) steps on a Turing machine — the same class for any reasonable model"
        correct: true
      - text: "Problems with polynomial-size answers"
      - text: "Problems solvable by humans"
  - id: np-certificate
    prompt: >-
      What is NP in terms of certificates, and where does the name come from?
    explanation: >-
      Problems where yes answers can be verified in polynomial time given a certificate, “to verify that a set
      of cities can be visited in less than 1,000 miles, we can provide the machine with such a tour” (p. 175).
      Equivalently, a nondeterministic machine, one that can “duplicating themselves during a computation”,
      could guess the certificate in one of its copies. “This view led Karp to propose the shorthand NP”
      (p. 175). Leonid Levin developed the idea independently (p. 220).
    options:
      - text: "Not-polynomial; problems with no fast algorithm"
      - text: "Yes-answers checkable in polynomial time given a certificate; named for nondeterministic polynomial time"
        correct: true
      - text: "Problems solvable in n to the power P time"
      - text: "Problems about networks"
  - id: factoring-np
    prompt: >-
      Factoring is in NP. Which statement matches the book?
    explanation: >-
      “Consider the factoring problem of writing a whole number as the product of two smaller whole numbers.
      This may be a difficult task (no polynomial-time algorithm is known), but it is a simple matter to check
      that an answer is correct” (p. 175). It is in NP but not known to be NP-complete. And “we do not know of
      a problem that is in NP but definitely not in P” (p. 175), so proving that anything is hard remains open.
    options:
      - text: "Factoring is not in NP"
      - text: "Factoring is NP-complete"
      - text: "Factoring answers are easy to check but no polynomial algorithm is known; no problem is known to be in NP but outside P"
        correct: true
      - text: "Factoring is in P"
  - id: reduction
    prompt: >-
      What is a polynomial-time reduction from problem A to problem B?
    explanation: >-
      “A polynomial-time Turing machine that takes any instance of problem A and creates an instance of
      problem B, such that the answers to A and B are the same, either both yes or both no” (p. 176). Example:
      Menger’s path problem reduces to the TSP by adding a dummy city, in time proportional to n (p. 176).
      “A problem reduction from A to B implies that if B is in P, then so is A” (p. 176). B is at least as
      hard as A.
    options:
      - text: "Removing constraints from B"
      - text: "Solving A and B in parallel"
      - text: "A way to shrink A's input"
      - text: "A polynomial transformation of A-instances into B-instances with the same yes/no answer; so B fast ⇒ A fast"
        correct: true
  - id: reduction-direction
    prompt: >-
      To show your new scheduling problem X is hard, which reduction must you give?
    explanation: >-
      “To show a problem is easy, you can try to reduce it to another easy problem. To show a problem is hard,
      you can try to reduce a known hard problem to your problem” (p. 176). Reduce a known NP-complete problem
      (Hamiltonian circuit, 3-SAT, …) **to** X. Reducing X to the TSP only shows X is no harder than the TSP.
      Getting the direction backwards is the classic mistake.
    options:
      - text: "Reduce X to the TSP"
      - text: "Reduce a known NP-complete problem to X"
        correct: true
      - text: "Reduce X to sorting"
      - text: "Show X has exponentially many solutions"
  - id: cook-theorem
    prompt: >-
      What did Stephen Cook prove in 1971?
    explanation: >-
      “His theorems show that every problem in NP can be formulated as a satisfiability problem” (p. 176).
      Satisfiability asks whether true/false values can make a given and/or formula true. The proof encodes
      each step of a polynomial-length verification as logical variables, and “the full proof in Cook’s
      original paper occupies less than a page” (p. 176). So if SAT ∈ P, then P = NP. Cook conjectured it is
      not (p. 175). This is “one problem to rule them all.”
    options:
      - text: "That the TSP is in P"
      - text: "That every NP problem reduces to satisfiability, so SAT ∈ P would mean P = NP"
        correct: true
      - text: "That SAT is undecidable"
      - text: "That P ≠ NP"
  - id: karp-21
    prompt: >-
      What did Karp's 1972 paper add?
    explanation: >-
      Chaining reductions: if SAT reduces to X, every NP problem reduces to X, so X is **NP-complete**. Karp
      gave “a now famous list of twenty-one NP-complete problems … The list includes two versions of the TSP:
      the Hamiltonian-circuit problem for undirected graphs and the Hamiltonian-circuit problem for directed
      graphs” (p. 177). Hundreds followed, catalogued in Garey and Johnson’s *Computers and Intractability*
      (1979), which “lives on the shelf of nearly everyone working in algorithms” (p. 177).
    options:
      - text: "The simplex method"
      - text: "A proof that P = NP"
      - text: "21 NP-complete problems by chaining reductions from SAT, including Hamiltonian circuits"
        correct: true
      - text: "A list of 21 polynomial algorithms"
  - id: np-complete-response
    prompt: >-
      Your team proves a feature-allocation problem NP-complete. According to the book, what do
      researchers typically do next?
    explanation: >-
      “Once a problem is shown to be NP-complete, researchers assume it will be nasty to solve and turn either
      to quick-and-dirty heuristic methods or to one of the heavy-duty approaches pioneered by attacks on the
      TSP” (p. 177): branch-and-cut, IP solvers, LP bounds. NP-completeness tells you what not to expect
      (a fast exact algorithm for all inputs). It does not tell you to give up.
    options:
      - text: "Use brute force"
      - text: "Give up on the feature"
      - text: "Use heuristics or heavy-duty exact methods (LP/IP, branch-and-cut) as appropriate"
        correct: true
      - text: "Search for a polynomial algorithm for a week"
---
## Learning objectives

- Define P, NP, reductions and NP-completeness precisely.
- Get the direction of reductions right.
- Know Cook’s theorem and Karp’s 21 problems.

## Definitions

- **Decision problem:** yes/no. Optimization reduces to it by binary search on the threshold (pp. 174, 220).
- **P:** solvable in C·nᵏ Turing-machine steps. Robust across models (p. 174).
- **NP:** yes-instances have polynomially checkable certificates (a tour for “length < K?”) (p. 175).
- **Reduction A ≤ B:** polynomial map of instances that preserves answers. B ∈ P ⇒ A ∈ P (p. 176).
- **NP-complete:** in NP, and every NP problem reduces to it (p. 177).

## Results

- **Cook (1971):** SAT is NP-complete (pp. 175–176). Levin independently.
- **Karp (1972):** 21 NP-complete problems, including undirected and directed Hamiltonian circuit (p. 177).
- **Garey and Johnson (1979):** the standard catalog (p. 177).
- Status: no problem is known to be in NP \ P. Woeginger’s list of claimed proofs splits about evenly between P = NP and P ≠ NP, and none has survived review (p. 178).

```text
To prove X is NP-hard:      KnownNPC  ≤p  X
To prove X is easy:         X  ≤p  KnownP
```

## In software terms

When a stakeholder asks for “the optimal schedule for any input, guaranteed fast,” the first step is to check whether the problem is NP-hard. If it is, decide between heuristics with quality measurement and exact solvers with time limits.

## Sources

- Cook (2012), pp. 174–178.
- Cook, S. A. (1971), *Proc. 3rd STOC*, 151–158.
- Karp, R. M. (1972). “Reducibility among combinatorial problems.”
- Garey, M. R., D. S. Johnson. *Computers and Intractability*. W. H. Freeman, 1979.
