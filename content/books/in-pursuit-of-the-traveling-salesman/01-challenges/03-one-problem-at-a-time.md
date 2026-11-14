---
title: "One Problem at a Time (pp. 10–16)"
questions:
  - id: algorithm-engineering
    prompt: >-
      The TSP is called “the standard bearer of a pragmatic school of research known as
      algorithm engineering.” What is that school's attitude?
    explanation: >-
      “The motto here is to not take no for an answer. Theoretical considerations may suggest
      that once we reach a certain size there exist instances of the TSP that necessarily take
      an exorbitant amount of computation, but this does not imply that whenever we see a
      specific large example we must give up and resort to a rough guess” (p. 11). The book’s
      notes define algorithm engineering as the “design, analysis, implementation and
      experimental evaluation of practical algorithms” (p. 213). Worst-case hardness bounds the
      general problem. Your actual instances may be far easier, so measure on real data
      before deciding a problem is out of reach.
    options:
      - text: "Accept worst-case lower bounds and always fall back to heuristics"
      - text: "Refuse to give up on specific large instances; design, implement and measure methods that solve them despite worst-case theory"
        correct: true
      - text: "Only study problems known to be in P"
      - text: "Prefer elegant proofs over working code"
  - id: record-timeline
    prompt: >-
      Put these exact-solution records in chronological order: (A) Held–Karp 64 random points;
      (B) Grötschel 120 German cities; (C) Dantzig–Fulkerson–Johnson 49 cities; (D) Concorde
      85,900-city chip; (E) Applegate–Bixby–Chvátal–Cook 3,038-city drilling problem.
    explanation: >-
      1954: Dantzig et al., 49 cities, a record that “stood as an unapproachable record for
      seventeen years”. 1971: Held and Karp, 64 random points. 1977: Grötschel, 120 cities in
      Germany, in his doctoral thesis. 1992: the 3,038-city drilling problem on a network of
      computers. 2006: 85,900 cities for a laser-cut chip (pp. 11–13). Other milestones: Miliotis
      80 points (1975), Crowder–Padberg 318 (1980), the 1987 burst of 532, 666, 1,002 and 2,392
      cities, then 13,509 (1998) and 24,978 (2004). The gains came from better mathematics and
      engineering as well as faster hardware.
    options:
      - text: "C, B, A, E, D"
      - text: "C, A, B, E, D"
        correct: true
      - text: "C, A, E, B, D"
      - text: "A, C, B, E, D"
  - id: rand-record-duration
    prompt: >-
      The 1954 49-city record stood for 17 years despite the arrival of computers. What does the
      book suggest about which approach eventually dominated?
    explanation: >-
      Held and Karp broke the record in 1971 with a spanning-tree bound in branch-and-bound.
      Then “the Dantzig et al. approach struck back in 1975, when Panagiotis Miliotis eclipsed
      the Held-Karp record by employing a variant of the original RAND idea” (p. 11), and
      Grötschel and Padberg “laid foundations for a great expansion of the basic methodology”
      (p. 11). Every later record, through Concorde, used the LP-based cutting-plane approach
      the RAND team introduced. Good ideas can sit for decades until supporting theory and tools
      arrive. Dantzig et al. had a working method without the polyhedral theory or LP software
      to scale it.
    options:
      - text: "Dynamic programming in n²2ⁿ time"
      - text: "Pure enumeration on faster hardware"
      - text: "The LP-based cutting-plane method from RAND, extended with new theory"
        correct: true
      - text: "Genetic algorithms"
  - id: world-gap
    prompt: >-
      For the 1,904,711-city World TSP, the book reports a best tour of 7,515,790,345 m and a
      lower bound of 7,512,218,268 m. What does this tell you?
    explanation: >-
      “Thus Helsgaun’s tour is no more than 0.0476% longer than an optimal tour. That is close,
      but there is plenty of room for shortcuts” (p. 15). The gap is (tour − bound)/bound. The
      optimum lies somewhere in between, and nobody knows where. Closing it requires either a
      better tour, a better bound, or both. For production systems this is the right way to
      report quality: ‘within 0.05% of optimal, guaranteed’ is a stronger claim than “looks
      good.”
    options:
      - text: "The optimal tour is exactly 7,512,218,268 m"
      - text: "The best tour is provably within about 0.05% of optimal, but its optimality is unknown"
        correct: true
      - text: "The bound is wrong because it is less than the tour"
      - text: "The tour is optimal because the gap is less than 1%"
  - id: mona-lisa
    prompt: >-
      What is the Mona Lisa TSP, and why do researchers care about such challenge instances?
    explanation: >-
      Robert Bosch built a 100,000-city instance in 2009 whose optimal tour would be a
      continuous-line drawing of da Vinci’s portrait. Yuichi Nagata’s tour “is known to be at
      most 0.003% longer than an optimal solution”, and $1,000 awaits whoever improves it. Cook
      is clear about the purpose: “the real goal of problem-by-problem challenges is to gather
      ideas for use in general solution methods for the salesman, and beyond to application
      areas well outside the TSP” (p. 16). Hard benchmarks work like public stress tests: they
      push methods that then transfer to other problems.
    options:
      - text: "A copyright dispute about TSP art"
      - text: "A 100,000-city benchmark whose tours draw the portrait; its purpose is to force new general-purpose ideas"
        correct: true
      - text: "The first instance solved by quantum computers"
      - text: "A 33-city puzzle from an Italian museum"
  - id: chip-application
    prompt: >-
      The record 85,900-city instance came from industry. What did the cities and costs
      represent?
    explanation: >-
      “The 85,900 cities in the record problem represent locations of connections that must be
      cut by a laser to create a customized computer chip. The TSP in this case models the
      movement of the laser from location to location. Although movements are measured in
      fractions of an inch, the total travel time was a major contributor to the chip’s
      production cost” (p. 13). Cities can be any points a tool must visit, and travel cost
      can be any additive cost such as time, energy or wear.
    options:
      - text: "Laser cut points on a chip, with costs as laser travel time"
        correct: true
      - text: "Cities in Sweden"
      - text: "Stars to be photographed"
      - text: "Data centers to be connected by fiber"
  - id: concorde-availability
    prompt: >-
      Concorde, the code behind the 13,509-, 24,978- and 85,900-city optimal tours, is described
      how in the book?
    explanation: >-
      “The computer code used in these solutions is called Concorde and it is available over the
      internet” (p. 13). It is free for academic research. Many applications in chapter 3
      (genome mapping at NIH, scan chains, job sequencing) call Concorde directly. If you need
      certified optimal TSP tours in production, an open solver built on decades of research
      is a better starting point than a hand-rolled solver.
    options:
      - text: "A secret proprietary solver at IBM"
      - text: "A freely available code implementing the LP/branch-and-cut approach"
        correct: true
      - text: "A genetic algorithm library"
      - text: "Hardware built for the TSP"
---
## Learning objectives

- Explain algorithm engineering and why it coexists with NP-hardness.
- Trace the major exact-solution records and the methods behind them.
- Interpret optimality gaps on open challenge instances.

## Not taking no for an answer

Until P vs NP is settled, the practical goal is to solve larger and harder *instances*. Cook calls the TSP “the standard bearer” of **algorithm engineering**: theory may predict nasty worst cases, but that does not mean you give up on the specific instance in front of you (p. 11). Each new record is like a first ascent of a Himalayan peak. What matters is the push itself, because the push produces ideas.

## From 49 to 85,900

| Year | Cities | Who | Method notes |
|---|---|---|---|
| 1954 | 49 | Dantzig, Fulkerson, Johnson | LP + cutting planes by hand |
| 1971 | 64 | Held, Karp | Spanning-tree bounds + branch-and-bound |
| 1975 | 80 | Miliotis | RAND-style cuts + integer programming |
| 1977 | 120 | Grötschel | Combs found by hand |
| 1980 | 318 | Crowder, Padberg | Automated cutting planes |
| 1987 | 532 → 2,392 | Grötschel–Holland; Padberg–Rinaldi | Branch-and-cut |
| 1992 | 3,038 | Applegate, Bixby, Chvátal, Cook | Parallel network |
| 1998 | 13,509 | same team (Concorde) | USA towns ≥ 500 people |
| 2004 | 24,978 | same team | Sweden |
| 2006 | 85,900 | same team | Laser-cut chip |

*(pp. 11–13; details in chapter 8.)*

## Open challenges

- **World TSP**, 1,904,711 cities. Tour 7,515,790,345 m, bound 7,512,218,268 m, gap 0.0476% at publication (p. 15).
- **Mona Lisa TSP**, 100,000 cities. Nagata’s tour is within 0.003%, and a $1,000 prize is offered for any improvement (pp. 15–16).

Chapter 13 of this companion updates both numbers.

## In software terms

- Benchmarks with public leaderboards, like TSPLIB, DIMACS and these challenge instances, keep progress honest and reproducible. Use the same habit for your own performance work.
- “NP-hard” plus “real data” often means solvable in practice. Prototype, measure, and keep the bound.

## Sources

- Cook (2012), pp. 10–16.
- Applegate, D. L., R. E. Bixby, V. Chvátal, W. J. Cook. *The Traveling Salesman Problem: A Computational Study*. Princeton, 2006.
- Concorde TSP solver: math.uwaterloo.ca/tsp/concorde.
