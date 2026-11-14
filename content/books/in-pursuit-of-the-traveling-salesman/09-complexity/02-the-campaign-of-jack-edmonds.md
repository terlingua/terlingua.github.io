---
title: "The Campaign of Jack Edmonds (pp. 171–174)"
questions:
  - id: finite-not-enough
    prompt: >-
      Beckmann and Koopmans (1953) and Flood (1954) both argued that “finite” isn't enough. What did Flood
      emphasize?
    explanation: >-
      Flood stressed time-to-answer: “The important thing gained by this would not be economy in carrying
      out the calculations but rather a shortening of the length of time it would take to recompute the
      operation … use of a high speed computer may indeed cost more dollars yet make the calculation possible
      in time” (p. 172). Speed matters because decisions have deadlines, so a correct answer that arrives
      too late is useless. Yet the field lacked a precise target, and Flood himself said practical methods
      were “not normally an interesting problem” to pure mathematicians (p. 172).
    options:
      - text: "That TSP is easy"
      - text: "That brute force is acceptable"
      - text: "That speed matters because results must arrive in time to be useful"
        correct: true
      - text: "That computers are too expensive"
  - id: simplex-complacency
    prompt: >-
      Why might the success of the simplex method have *delayed* a theory of efficient algorithms?
    explanation: >-
      “The simplex method appeared to solve every linear-programming problem in sight, although it could not be
      proven to always run efficiently. This led to an over-comfortable acceptance of algorithms whose
      performance could not be guaranteed” (pp. 172–173). When things work well in practice, there is little
      pressure to demand guarantees. Production systems that ‘never had an outage’ without load tests are
      in the same position.
    options:
      - text: "It was too slow"
      - text: "It worked so well in practice that people accepted methods without performance guarantees"
        correct: true
      - text: "It was proven exponential"
      - text: "It was secret"
  - id: edmonds-matching
    prompt: >-
      What result was the centerpiece of Edmonds's campaign at the 1961 RAND workshop?
    explanation: >-
      “During the course of the workshop he succeeded in producing a good algorithm for the problem [optimal
      matchings], that is, an algorithm whose number of steps grows at a rate at most proportional to n⁴ …
      This deep result served as the focal point of Edmonds’s campaign” (p. 173). Skeptics replied “if it were
      n to the 28th, you know, that doesn’t …” (p. 173). A concrete, elegant algorithm helped make the case
      for the abstract definition.
    options:
      - text: "A proof that P ≠ NP"
      - text: "A TSP algorithm"
      - text: "A polynomial (n⁴) algorithm for optimal matching"
        correct: true
      - text: "The simplex method"
  - id: polynomial-term
    prompt: >-
      Why did “polynomial-time” replace Edmonds's word “good”?
    explanation: >-
      “Researchers were not entirely comfortable with the emotional charge that comes with the word ‘good’”
      (p. 9), and “it is a bit harsh to say an algorithm as successful as the simplex method is bad” (p. 174).
      Polynomial-time is neutral and exact. Simplex is not known to be polynomial, yet it is excellent in
      practice.
    options:
      - text: "Because good algorithms are rare"
      - text: "To remove the value judgment — e.g., simplex is not known to be polynomial yet is excellent"
        correct: true
      - text: "Because polynomial is shorter"
      - text: "Because Edmonds disliked it"
---
## Learning objectives

- Understand why efficiency became a mathematical subject.
- Credit Edmonds for polynomial time as the target.

## Summary

In the 1950s, finite was not good enough (Beckmann–Koopmans, Flood), but there was no target (pp. 171–172). The simplex method’s practical success encouraged acceptance of methods with no guarantees (pp. 172–173). **Jack Edmonds** pushed for polynomial-time as the definition of a good algorithm, anchored by his n⁴ matching algorithm at RAND in 1961, against considerable skepticism (p. 173). Today “Edmonds is the hero of algorithms and computational complexity” (p. 173). The term became *polynomial-time* (p. 174).

## In software terms

SLOs in terms of growth (“latency must scale at most linearly with tenant size”) are Edmonds’s idea applied to operations: state a bound on growth, not just today’s timing.

## Sources

- Cook (2012), pp. 171–174.
- Edmonds, J. “Paths, trees, and flowers.” *Canadian J. Math.* 17 (1965), 449–467.
- Edmonds, J. “A glimpse of heaven.” In J. K. Lenstra et al. (eds.), *History of Mathematical Programming*, North-Holland, 1991, 32–54.
