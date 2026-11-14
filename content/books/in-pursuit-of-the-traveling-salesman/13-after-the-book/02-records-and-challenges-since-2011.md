---
title: "Records and Challenges since 2011"
questions:
  - id: road-records
    prompt: >-
      After 2011 Cook's team turned to *road-network* instances. Which statements are correct?
    type: multiple
    explanation: >-
      The UK pub crawl visits 49,687 pubs using walking distances. Its optimality proof finished March 5, 2018
      after 557,271 branch-and-bound subproblems and about 250 core-years. LKH found the optimal tour early, and
      Concorde (improved) with CPLEX proved it. The Netherlands tour of 57,912 stops was solved in February 2021.
      The South Korea tour of 81,998 bars, using OSRM walking times of about 178 days, was computed between
      December 2024 and March 2025 and is described as the largest road-map instance solved to proven
      optimality. pla85900 is larger but geometric. In each case a heuristic supplied the tour and LP machinery
      supplied the proof, the pattern from 1954 onward.
    options:
      - text: "49,687 UK pubs — proven optimal in 2018 after ≈557k subproblems"
        correct: true
      - text: "57,912 Netherlands stops — solved in 2021"
        correct: true
      - text: "81,998 Korean bars — largest road-map instance proven optimal (2025)"
        correct: true
      - text: "The World TSP was solved in 2020"
  - id: sparse-distances
    prompt: >-
      The UK instance has about 1.2 billion pub pairs, but only about 2.2 million walking distances were
      queried from a mapping service. Why does that work?
    explanation: >-
      Optimal tours use short edges, and LP reduced costs prove that most long edges cannot appear in any
      improving solution. Computing exact road distances only for promising pairs, with geometric estimates as
      safe bounds for the rest, keeps API cost manageable. The same lazy idea as cutting planes (chapter 6)
      is applied to *variables* instead of constraints. Engineers would call it column or edge generation, and
      it is a direct way to save money on paid distance APIs.
    options:
      - text: "The API was free"
      - text: "They guessed the rest randomly"
      - text: "Only edges that could plausibly appear in an optimal tour need exact costs; the rest can be bounded and pruned lazily"
        correct: true
      - text: "Walking distance equals straight-line distance"
  - id: mona-status
    prompt: >-
      What is the status of the Mona Lisa TSP per the Waterloo challenge page?
    explanation: >-
      Nagata’s March 2009 tour of 5,757,191 is still the best. The lower bound rose to 5,757,084 on July 27, 2012
      with Concorde, leaving a gap of 107 (about 0.0019%), down from 147 in the book (p. 164). The $1,000 prize
      for a shorter tour is unclaimed. Cook predicted that “new ideas, particularly in cutting-plane separation”
      would be needed (p. 164), and over a decade with no further bound movement supports that.
    options:
      - text: "Solved optimally in 2015"
      - text: "Still open: tour 5,757,191 (Nagata 2009), bound 5,757,084 (2012), gap 107"
        correct: true
      - text: "A shorter tour won the prize in 2020"
      - text: "The instance was withdrawn"
  - id: world-status
    prompt: >-
      What happened to the World TSP (1,904,711 cities) after the book?
    explanation: >-
      The tour side kept improving: Helsgaun reached 7,515,755,956 (Feb 2021), and Nagata found 7,515,755,912 on
      October 17, 2025, 44 m shorter. The bound is still Concorde’s 7,512,218,268 from 2007. The gap is about
      0.0471%, against 0.0476% in the book (p. 165). Heuristics keep improving, and the bound side is limited by
      engineering at this scale: domino-parity separation cannot yet run on it (p. 166).
    options:
      - text: "Solved optimally"
      - text: "Tours improved slightly (Nagata 2025); bound unchanged since 2007; gap ≈ 0.047%"
        correct: true
      - text: "The bound reached the tour"
      - text: "Abandoned in 2012"
  - id: gaia
    prompt: >-
      In 2020 Cook and Helsgaun reported a tour through 2,079,471 stars from Gaia DR1 in 3-D. What is
      remarkable?
    explanation: >-
      The 94,208,157.5-light-year tour is proven within a factor of 0.0000074 of optimal. Heuristic search
      (LKH-style) gave the tour and the LP cutting-plane method gave the guarantee. That is far tighter than the
      0.410% gap the book reports for the 526-million-object sky instance (p. 167). For a 2-million-point
      instance, a tour guaranteed within 0.00074% is useful in practice even without full optimality.
    options:
      - text: "It used straight-line 2-D distances"
      - text: "It proved P = NP"
      - text: "It was solved by quantum computer"
      - text: "A 2-million-star 3-D tour certified within 0.00074% of optimal via LP bounds"
        correct: true
  - id: lesson-pattern
    prompt: >-
      Across post-2011 records, which division of labor recurs?
    explanation: >-
      LKH, and Nagata’s GA for some instances, finds the best tours, often the optimal one, early. Concorde’s
      cutting planes, with CPLEX as the LP engine, and branch-and-bound prove the lower bound. Optimality is
      declared when bound meets tour. This is the 1954 Dantzig–Fulkerson–Johnson pattern (p. 5) at scale, and
      it applies to production optimization too: a fast heuristic for answers, LP relaxations for quality
      guarantees.
    options:
      - text: "Exact solvers find tours; heuristics prove bounds"
      - text: "Heuristics (LKH, GA) find tours; LP cutting planes + branching prove bounds"
        correct: true
      - text: "Neural networks do both"
      - text: "Brute force does both"
---
## Learning objectives

- Update the record book and the challenge gaps.

## Records and status (math.uwaterloo.ca/tsp, as of late 2025)

| Instance | Size | Status |
|---|---|---|
| UK pubs (walking) | 49,687 | Optimal, 63,739,687 m. Proof finished March 2018, ≈557k B&B nodes, ≈250 core-years |
| Netherlands | 57,912 | Optimal (Feb 2021) |
| South Korea bars (walking time) | 81,998 | Optimal (Dec 2024 to Mar 2025). Largest road-map instance proven optimal |
| Gaia DR1 stars (3-D) | 2,079,471 | Within 0.0000074 of optimal (2020) |
| Mona Lisa | 100,000 | Open. Tour 5,757,191 (2009), bound 5,757,084 (2012), gap 107 |
| World | 1,904,711 | Open. Tour 7,515,755,912 (Nagata, Oct 2025), bound 7,512,218,268 (2007), gap ≈ 0.047% |

## Pattern

The heuristic finds the tour, LP and branch-and-bound prove it optimal, and the solution goes on record. Road instances add **lazy edge pricing** so that only plausible pairs need paid distance queries.

## Sources

- University of Waterloo TSP pages: [UK pubs](https://math.uwaterloo.ca/tsp/uk/index.html), [Korea bars](https://www.math.uwaterloo.ca/tsp/korea/), [Mona Lisa](https://math.uwaterloo.ca/tsp/data/ml/monalisa.html), [World TSP](https://math.uwaterloo.ca/tsp/world/).
- University of Waterloo C&O news, “Finding a very-close-to-optimal tour through 2,079,471 stars” (Oct 25, 2020).
