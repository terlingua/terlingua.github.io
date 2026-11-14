---
title: "Julian Lethbridge; Jordan Curves (pp. 199–203)"
questions:
  - id: math-beauty
    prompt: >-
      When mathematicians call the TSP beautiful, what do they mean, according to Cook?
    explanation: >-
      “A tour through a set of points may have a pleasing shape, but it is the combined beauty of the geometry
      and complexity of the problem, not the tour itself, that attracts mathematicians” (p. 199). Some artworks
      nonetheless “successfully capturing the mathematical essence” (p. 199).
    options:
      - text: "The combined beauty of the problem's geometry and complexity, not the drawn tour"
        correct: true
      - text: "The salesmen's lives"
      - text: "The colors used in maps"
      - text: "The tour drawings themselves"
  - id: jordan-curve
    prompt: >-
      A non-crossing tour is a Jordan curve. What does that mean?
    explanation: >-
      “The non-crossing tour forms a simple closed curve, known as a Jordan curve, partitioning space into two
      regions, one bounded and the other unbounded; the bounded region is the one we think of as the interior”
      (p. 201). Lethbridge’s *Traveling Salesman* (1995) textures interior and exterior differently, separated
      by a white tour (p. 201). Since optimal Euclidean tours never cross (chapter 4), every optimal Euclidean
      tour is a Jordan curve.
    options:
      - text: "A spiral"
      - text: "A straight line"
      - text: "A curve that crosses itself once"
      - text: "A simple closed curve dividing the plane into inside and outside"
        correct: true
  - id: lethbridge-inspiration
    prompt: >-
      What struck Julian Lethbridge about the TSP?
    explanation: >-
      “He came upon a description of the TSP in a journal and was struck immediately by the economy of thought
      and space provided by good solutions to the problem” (p. 200). His second painting suggests the
      alternative paths with radiating strokes from unseen cities. It is in the National Gallery’s Meyerhoff
      Collection (p. 201).
    options:
      - text: "Its history at RAND"
      - text: "Its financial applications"
      - text: "The economy of thought and space in good solutions"
        correct: true
      - text: "Its computational complexity proofs"
  - id: side-constraint
    prompt: >-
      Bosch “bends the curve to his will” by requiring two chosen points to lie on opposite sides of the tour.
      How is this modeled?
    explanation: >-
      “This geometric requirement is equivalent to stating that the line segment joining the two points must
      cross the TSP tour an odd number of times, and this is a condition that can be modeled as an additional
      constraint in an integer-programming formulation of the TSP” (p. 203). He then solves with an IP code and
      iterates on the design (pp. 202–203). Requirements like this become side constraints in an IP model,
      the same way business rules are added to a scheduling model.
    options:
      - text: "The segment between the points must cross the tour an odd number of times — a parity side constraint in an IP"
        correct: true
      - text: "By deleting cities"
      - text: "By using asymmetric costs"
      - text: "By recoloring after solving"
  - id: embrace
    prompt: >-
      Bosch's *Embrace* sculpture (2010 Mathematical Art Exhibition First Prize) was made how?
    explanation: >-
      A 726-city TSP tour, adjusted with side constraints, was cut by water-jet along the tour in quarter-inch
      stainless steel and brass. Pairing the inside of one metal with the outside of the other produced two
      versions, and the cutting kerf leaves the visible path (p. 203). A 2,840-city symmetric-ring design used
      symmetry and interlacing constraints, solved heuristically because the IPs were too hard (p. 203).
    options:
      - text: "3-D printed from a random walk"
      - text: "Carved from a single block"
      - text: "Painted by hand"
      - text: "Water-jet cutting along a constrained 726-city tour in steel and brass, swapping inside and outside"
        correct: true
---
## Learning objectives

- Connect non-crossing tours with Jordan curves.
- See how side constraints shape tours for design goals.

## Summary

- Mathematical beauty lies in the problem (p. 199). Lethbridge’s *Traveling Salesman* works use the inside/outside split of a Jordan-curve tour (pp. 199–201).
- **Jordan curve:** a simple closed curve with a bounded interior and an unbounded exterior (p. 201).
- **Bosch’s Embrace:** a 726-city tour with an IP side constraint (“opposite sides” ⇔ odd number of crossings), water-jet cut in steel and brass (pp. 201–203). The 2,840-city ring uses symmetry constraints (p. 203).
- Bosch’s workflow: idea → optimization model → solve → render → revise (p. 202), much like iterating on code in a REPL.

## Sources

- Cook (2012), pp. 199–203.
- Bosch, R. “Embrace.” *2010 Joint Mathematics Meetings Art Exhibition Catalog*; Bosch, R., *Proceedings of Bridges 2009* (“bending the curve”).
- Bosch, R. *Opt Art: From Mathematical Optimization to Visual Design*. Princeton, 2019.
