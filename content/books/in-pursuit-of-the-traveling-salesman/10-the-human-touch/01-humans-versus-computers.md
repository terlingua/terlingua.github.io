---
title: "Humans versus Computers (pp. 191–192)"
questions:
  - id: sheffield
    prompt: >-
      What made Charles Sheffield wonder whether humans could compete on the TSP?
    explanation: >-
      Covering the 1997 Kasparov–Deep Blue match for IBM, “he was struck by the fact that a human could go
      toe-to-toe with a massive computing device on what is essentially a computational problem. He speculated
      whether humans could also compete successfully on other nasty computational challenges, such as the
      TSP” (p. 191). He even imagined training talented young people on an NP problem (epigraph, p. 191).
    options:
      - text: "A Turing test"
      - text: "Reading Lin–Kernighan"
      - text: "A TSP contest he lost"
      - text: "Watching Kasparov compete against Deep Blue"
        correct: true
  - id: boyd-contest
    prompt: >-
      At a 2007 workshop, Applegate and Cook won Sylvia Boyd's by-hand 50-city challenge. What was the
      sobering part?
    explanation: >-
      “Sadly, our tour was not optimal. After twenty years of research on the problem, we did not have the
      skill to match what the computer could knock out in under a second” (pp. 191–192). Expertise in TSP
      theory did not translate into unaided tour-finding skill. Cook’s bet goes to the computer without
      special training.
    options:
      - text: "They lost to a child"
      - text: "Their winning tour wasn't optimal — a computer would find the optimum in under a second"
        correct: true
      - text: "The contest was cancelled"
      - text: "They used a computer illegally"
  - id: product-implication
    prompt: >-
      A dispatch product lets supervisors hand-edit routes. Given chapter 10's opening, what is the sensible
      default?
    explanation: >-
      Even TSP experts could not match a computer on a 50-city instance by hand (pp. 191–192), so the solver
      should produce the base plan. People add value where the model is blind: local constraints such as
      Cleveland’s unbridged river (p. 21) or Schreck’s lighted runways (p. 49). Show the cost change of each
      manual edit so overrides are informed decisions, and feed recurring overrides back into the model as
      constraints.
    options:
      - text: "Forbid all edits"
      - text: "Randomize routes daily"
      - text: "Let supervisors plan from scratch; humans beat solvers"
      - text: "Generate routes with a solver, allow informed manual edits for unmodeled constraints, and show the cost impact"
        correct: true
---
## Learning objectives

- Calibrate expectations about human vs machine performance on the TSP.

## Summary

Inspired by Kasparov vs Deep Blue, Sheffield asked if trained humans could rival computers on NP-hard problems (p. 191). Cook bets on the computer. Even TSP experts doing a 50-city instance by hand fell short of the optimum, which a computer finds in under a second (pp. 191–192).

## Sources

- Cook (2012), pp. 191–192.
