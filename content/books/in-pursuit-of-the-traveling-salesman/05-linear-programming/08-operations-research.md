---
title: "Operations Research (pp. 125–126)"
questions:
  - id: or-definition
    prompt: >-
      How does INFORMS define operations research?
    explanation: >-
      “In a nutshell, operations research (O.R.) is the discipline of applying advanced analytical methods
      to help make better decisions” (p. 125), marketed as “The Science of Better.” It combines optimization
      (LP, IP) with probability, game theory and more, across health care, transportation, finance and
      forestry: “Any place where decisions are made, operations research can be applied” (p. 125).
    options:
      - text: "Computer operations and system administration"
      - text: "Military history"
      - text: "The study of surgical operations"
      - text: "Applying advanced analytical methods to help make better decisions"
        correct: true
  - id: or-origin
    prompt: >-
      Where does the “operations” in operations research come from?
    explanation: >-
      “Operations research traces its roots to work on military planning in the mid-1900s, hence the
      ‘operations’ part of the name” (p. 125), the same military context in which Dantzig built LP. Today
      it is taught at Berkeley, Carnegie Mellon, Columbia, Cornell, Georgia Tech, MIT, Princeton, Stanford
      and others (p. 125).
    options:
      - text: "Stock market operations"
      - text: "Hospital operations"
      - text: "Military operations planning in the mid-twentieth century"
        correct: true
      - text: "Arithmetic operations"
  - id: or-devops
    prompt: >-
      Which of these DevOps or platform problems are naturally operations research problems?
    type: multiple
    explanation: >-
      O.R. applies wherever decisions are made under constraints (p. 125). Bin-packing containers onto nodes
      (IP), on-call rotation scheduling with fairness rules (IP), and choosing reserved versus on-demand
      capacity (LP or stochastic programming) all fit. Choosing a code formatter is a matter of taste and has
      no quantitative objective.
    options:
      - text: "Packing containers onto nodes to minimize cost"
        correct: true
      - text: "Building an on-call rotation that satisfies fairness rules"
        correct: true
      - text: "Choosing a mix of reserved and on-demand capacity"
        correct: true
      - text: "Choosing a code formatter"
---
## Learning objectives

- Place LP, IP and the TSP within operations research.

## Summary

Operations research is “the discipline of applying advanced analytical methods to help make better decisions” (INFORMS). It began in mid-century military planning and now runs through every industry (pp. 125–126). Cook recommends Michael Trick’s O.R. blog for applications, including sports scheduling (p. 126), and Laura McLay’s *Punk Rock Operations Research* (p. 217).

## In software terms

Platform engineering is full of O.R. problems: scheduling, bin packing, capacity planning, routing, queueing. You do not need to write a solver. You need to recognize the problem class and pick the right modeling tool.

## Sources

- Cook (2012), pp. 125–126, 217.
- INFORMS, “What is O.R.?” informs.org.
