---
title: "Guiding Industrial Machines (pp. 53–56)"
questions:
  - id: pcb-drilling
    prompt: >-
      What is the classic TSP formulation for printed-circuit-board drilling, and what gain did it
      bring?
    explanation: >-
      Automated drills move between hole locations, “and a classic application of the TSP is to
      minimize the travel time of the drill head during the production process” (p. 54). “The use
      of TSP algorithms has led to improvements of approximately 10% in the overall throughput of
      circuit-board production lines” (p. 54), on instances of several hundred to several
      thousand holes. TSPLIB, the standard benchmark library, contains many such instances,
      including the 2,392- and 3,038-hole records.
    options:
      - text: "Chips are cities, power is cost; no measurable gain"
      - text: "Drills are cities, wear is cost; 1% gain"
      - text: "Holes are cities, drill-head travel time is cost; about 10% more line throughput"
        correct: true
      - text: "Boards are cities, shipping cost is cost; about 50% savings"
  - id: engraving-flex
    prompt: >-
      In CNC brass engraving, the machine can descend anywhere above a letter, not at a single
      point. How does that change the model?
    explanation: >-
      “Additional flexibility in this case comes from the fact that elements to be cut are not
      single points, so the machine can be guided to any location above the element” (p. 55).
      Each city becomes a region, so the problem is to choose the entry and exit points as well as
      the order. That is a generalized TSP with continuous choices, like the county visits in
      Road Trips. The engraver reported working time cut in half for text-heavy dies (p. 55).
    options:
      - text: "It becomes an ordinary TSP with fixed points"
      - text: "Each element is a region, so entry/exit points are also chosen, which is a generalized TSP"
        correct: true
      - text: "Costs become asymmetric only"
      - text: "The problem becomes a shortest path"
  - id: bell-labs-chips
    prompt: >-
      Bentley and Johnson's fast heuristics for Bell Labs laser-cut chips had what effect, and what
      record later came from this application?
    explanation: >-
      “Jon Bentley and David Johnson provided fast TSP heuristic methods that lowered the slewing
      time by over 50% on typical examples … This application also holds a place of honor as the
      source of the record 85,900-city TSP instance” (p. 55). The industrial instance served as a
      production win and later as a research benchmark (pla85900).
    options:
      - text: "Doubled cost; source of the World TSP"
      - text: "No effect; it was a theoretical study"
      - text: "Over 50% less laser slewing time; source of the 85,900-city record instance"
        correct: true
      - text: "5% improvement; source of the Mona Lisa instance"
  - id: solder-and-wafers
    prompt: >-
      Which of these are machine-guidance TSPs described in the section?
    type: multiple
    explanation: >-
      Wladimir Nickel’s CNC solder-paste dispenser visits 256 board locations ordered by Concorde
      (p. 54). Applied Materials guides machinery between defects when cleaning silicon wafers,
      also with Concorde (p. 56). Brass dies are engraved by CNC mills (p. 55). Scheduling smart-card
      jobs (BÖWE CARDTEC) is a TSP too, but a sequencing problem rather than a moving tool head,
      and it appears in “Scheduling Jobs” (p. 60).
    options:
      - text: "Dispensing solder paste at board locations"
        correct: true
      - text: "Cleaning defects on silicon wafers"
        correct: true
      - text: "Engraving brass dies"
        correct: true
      - text: "Choosing a supplier for copper"
  - id: tool-path-devops
    prompt: >-
      A 3-D printer firmware team asks how to reduce non-printing “travel moves” between islands
      on each layer. What does this section suggest?
    explanation: >-
      This has the same structure as drilling, soldering and engraving. Each island is a city (or
      a region, if entry points are flexible), and travel moves are the costs. A fast heuristic such
      as greedy plus 2-opt or Lin–Kernighan (chapter 4) usually gives most of the benefit at
      firmware-friendly speeds. The 10%–50% gains reported in this section (pp. 54–55) are typical
      when the starting order is naive, such as file order.
    options:
      - text: "Nothing; printers cannot benefit from the TSP"
      - text: "Treat islands as cities (or regions) and order them with a fast TSP heuristic"
        correct: true
      - text: "Print islands alphabetically"
      - text: "Use brute force on each layer"
---
## Learning objectives

- Identify tool-path TSPs in manufacturing and their typical gains.
- Handle regions instead of points.

## Examples

- **PCB drilling.** About 10% more throughput, on hundreds to thousands of holes. A TSPLIB staple (p. 54).
- **Solder paste dispensing.** A 256-point CNC path ordered by Concorde (p. 54).
- **Brass engraving.** Elements are regions. Working time halved for text-heavy dies (p. 55).
- **Bell Labs custom chips.** Laser cuts. Over 50% less slewing. Source of pla85900 (p. 55).
- **Wafer cleaning.** Concorde guides machinery between defects (p. 56).

## In software terms

Tool paths in CNC, 3-D printing, plotters, pick-and-place, and even the order a crawler visits URLs on a rate-limited host share one structure: an ordered visit sequence with a cost between consecutive visits. File order or a naive sweep often leaves 10–50% on the table.

## Sources

- Cook (2012), pp. 53–56.
- Grötschel, M., M. Jünger, G. Reinelt. “Optimal control of plotting and drilling machines: a case study.” *Zeitschrift für Operations Research* 35 (1991), 61–84.
- Reinelt, G. “TSPLIB—A traveling salesman problem library.” *ORSA J. Computing* 3 (1991), 376–384.
