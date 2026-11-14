---
title: "The DIMACS Challenge and Tour Champions (pp. 91–93)"
questions:
  - id: misleading-papers
    prompt: >-
      What problem in 1980s TSP heuristic research led to TSPLIB and the DIMACS Challenge?
    explanation: >-
      “Research papers appeared in premier scientific journals, such as Nature, describing
      computations on TSP instances having 30 or 50 cities. The reported results were typically weak
      approximations, at a time when Lin-Kernighan could reliably deliver optimal solutions in a blink
      of an eye” (p. 91). Without shared benchmarks and baselines, weak methods looked new. The same
      failure shows up today when ML or systems papers skip strong baselines.
    options:
      - text: "Lack of computers"
      - text: "Copyright disputes"
      - text: "Too many exact solutions"
      - text: "Papers reported weak results on tiny instances without comparing to strong existing methods"
        correct: true
  - id: tsplib
    prompt: >-
      What is TSPLIB?
    explanation: >-
      Created after the TSP 90 conference at Rice by Gerhard Reinelt, published in 1991 with “over 100
      challenge instances of the TSP gathered from academic and industrial sources. The TSPLIB
      collection provides a common test bed for researchers around the world and across academic
      disciplines” (p. 91). Instances such as pr2392, usa13509, sw24978 (Sweden) and pla85900 are the
      records discussed in chapter 8. TSPLIB is to TSP research roughly what TPC benchmarks or ImageNet
      have been to their fields.
    options:
      - text: "A public collection of 100+ benchmark TSP instances from academia and industry (Reinelt, 1991)"
        correct: true
      - text: "A theorem library"
      - text: "A conference"
      - text: "A C library of TSP solvers"
  - id: dimacs-goal
    prompt: >-
      What was the goal of the DIMACS TSP Challenge led by David S. Johnson?
    explanation: >-
      “To create a reproducible picture of the state of the art in the area of TSP heuristics (their
      effectiveness, their robustness, their scalability, etc.), so that future algorithm designers can
      quickly tell on their own how their approaches compare with already existing TSP heuristics”
      (p. 92). It drew 130 algorithms and implementations, a comparison website, and the
      Johnson–McGeoch survey. Johnson received the 2010 Knuth Prize (p. 92).
    options:
      - text: "To prove P ≠ NP"
      - text: "To standardize distance functions"
      - text: "To award a prize for the optimal World tour"
      - text: "A reproducible, comparable picture of heuristic performance (quality, robustness, scalability)"
        correct: true
  - id: benchmark-practice
    prompt: >-
      You are evaluating a new route-optimization library for your company. Which practices follow
      the DIMACS spirit?
    type: multiple
    explanation: >-
      DIMACS stressed reproducibility and comparison against existing methods across effectiveness,
      robustness and scalability (p. 92). In practice: run against strong baselines (LKH, Concorde’s
      chained LK), report quality against a lower bound or best-known value, measure time across sizes,
      and publish instances and scripts. Running one 30-city example in a notebook is exactly the
      mistake of the 1980s papers (p. 91).
    options:
      - text: "Compare against strong baselines such as LKH on the same instances"
        correct: true
      - text: "Report percent over a lower bound or best known tour, plus runtime, across sizes"
        correct: true
      - text: "Publish instances and scripts so others can reproduce results"
        correct: true
      - text: "Demo it on a single 30-city example"
  - id: champions
    prompt: >-
      Who are the book's “world champions” of tour finding, and what are they known for?
    explanation: >-
      “Keld Helsgaun of Denmark and Yuichi Nagata of Japan.” Helsgaun’s LKH “has been the gold standard
      in tour finding since its introduction in 1998,” with the best World TSP tour and the optimal
      85,900-city tour. Nagata’s genetic algorithm produced the best Mona Lisa tour and records on the
      largest National TSP instances. “If you want a good solution to a large problem, these are the
      people to call” (p. 93).
    options:
      - text: "Dorigo and Holland — ants and genes"
      - text: "Dantzig and Fulkerson — by hand"
      - text: "Helsgaun (LKH) and Nagata (EAX genetic algorithm)"
        correct: true
      - text: "Kirkpatrick and Vecchi — annealing"
---
## Learning objectives

- Understand why shared benchmarks and reproducible comparison matter.
- Know the leading heuristic codes.

## Summary

In the 1980s, weak results on tiny instances got published in top journals. Two fixes followed (p. 91):

1. **TSPLIB** (Reinelt, 1991): 100+ shared instances.
2. **DIMACS TSP Challenge** (Johnson and McGeoch): 130 implementations compared reproducibly on quality, robustness and scalability (p. 92).

Heuristics trade time for quality. At the top end (“Formula One”), **Helsgaun’s LKH** and **Nagata’s EAX GA** hold most records (pp. 92–93).

## In software terms

Benchmark the way DIMACS did: strong baselines, representative sizes, quality against a bound, runtime curves, published artifacts. That applies to query engines and schedulers as much as to route optimizers.

## Sources

- Cook (2012), pp. 91–93.
- Reinelt, G. “TSPLIB—A traveling salesman problem library.” *ORSA J. Computing* 3 (1991), 376–384.
- Johnson, D. S., L. A. McGeoch. “Experimental analysis of heuristics for the STSP.” In Gutin and Punnen (2002), 369–443.
- LKH software: webhotel4.ruc.dk/~keld/research/LKH/.
