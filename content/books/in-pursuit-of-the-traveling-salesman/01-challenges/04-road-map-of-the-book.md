---
title: "Road Map of the Book (pp. 16–18)"
questions:
  - id: heart-of-book
    prompt: >-
      Which chapters does Cook call “the heart of our technical treatment”?
    explanation: >-
      “The heart of our technical treatment of the problem is the material presented in
      chapters 4 through 7, followed by a discussion of how TSP computer codes stack up to the
      task of solving large examples in chapter 8” (p. 17). Chapter 4 covers finding tours
      (heuristics), chapter 5 linear programming and bounds, chapter 6 cutting planes, and
      chapter 7 branching. Together they make up the Concorde pipeline: get a good tour, prove a
      bound, tighten it with cuts, and branch when cuts stall.
    options:
      - text: "Chapters 1–3"
      - text: "Chapters 4–7"
        correct: true
      - text: "Chapters 9–10"
      - text: "Chapter 12"
  - id: dont-skip-to-9
    prompt: >-
      Why does Cook advise readers chasing the $1,000,000 prize *not* to skip straight to the
      complexity chapter?
    explanation: >-
      “The seeds of a successful theoretical attack may well be in methods that have proved
      themselves in the computational field of play. And if you are going for an impossibility
      result, you will need to handle the successful practical techniques in your proof” (p. 17).
      A proof that P ≠ NP must explain why cutting planes, branch-and-cut and the rest
      cannot work in general, so you have to understand them. A proof that P = NP would likely
      grow out of them. Chapter 6 shows that separation and optimization are formally linked
      (Grötschel–Lovász–Schrijver), so the practical and theoretical sides are connected.
    options:
      - text: "Because chapter 9 contains errors"
      - text: "Because practical methods may hold the seeds of a theoretical breakthrough, and an impossibility proof must account for them"
        correct: true
      - text: "Because the prize has already been awarded"
      - text: "Because the complexity chapter requires calculus"
  - id: bash-on
    prompt: >-
      “Bash on regardless,” from J. P. Donleavy’s Rashers Ronald, is the rallying cry of the
      Applegate–Bixby–Chvátal–Cook team. What attitude does it describe?
    explanation: >-
      Cook recommends “the reader, too, adopt this attitude when approaching the problem. We will
      cover work of numerous experts who have made huge advances, but the TSP remains essentially
      open. A new point of view could be just what is needed to dramatically alter our ability
      to tackle the salesman” (p. 18). It is persistence in the face of a problem that may be
      unsolvable in general, which is the algorithm-engineering mindset. The book closes on the
      same phrase (p. 212).
    options:
      - text: "Abandon exact methods for heuristics"
      - text: "Ignore theory entirely"
      - text: "Keep working on a hard, open problem despite setbacks; new viewpoints may break through"
        correct: true
      - text: "Use brute force regardless of size"
  - id: book-scope
    prompt: >-
      Besides mathematics and computation, which topics does the book cover in its later
      chapters?
    type: multiple
    explanation: >-
      “In chapter 10 we cover studies on how humans, unaided by computers, go about solving the
      TSP; this area brings the problem into the realm of psychologists and neuroscientists. In
      chapter 11 we turn to the adoption of TSP tours in works of art” (p. 17). Chapter 3 covers
      applications such as genome sequencing, planet finding and music, and chapter 12 is a call
      to action. Quantum algorithms are discussed only briefly, in chapter 9’s section on exotic
      computers.
    options:
      - text: "How humans and animals solve TSPs"
        correct: true
      - text: "TSP tours in art"
        correct: true
      - text: "Applications such as genome mapping and telescopes"
        correct: true
      - text: "A full course in quantum algorithms"
---
## Learning objectives

- Know where each part of the story lives, so you can map the companion back to the book.

## Map

| Chapter | Topic | Companion focus |
|---|---|---|
| 2 | Origins | Graphs, Euler vs Hamilton, the √n law |
| 3 | Applications | Modeling tricks: dummy cities, asymmetric costs, similarity as cost |
| 4 | Heuristics | Constructive methods, local search, LK/LKH, metaheuristics |
| 5 | Linear programming | Simplex, duality, relaxations, polytopes |
| 6 | Cutting planes | Combs, facets, separation, max-flow |
| 7 | Branching | Branch-and-bound/cut, strong branching |
| 8 | Big computing | Records, Concorde, certificates |
| 9 | Complexity | P, NP, reductions, Held–Karp, approximation |
| 10–11 | Humans, art | Cognition, Jordan curves |
| 12 | Open problems | What is left |

Two additions follow the book’s order: chapter 13 (developments from 2011 to 2026) and chapter 14 (cross-cutting review).

## Advice from the author

Don’t jump to the million-dollar chapter. Practical methods may be where a theoretical breakthrough comes from, and an impossibility proof has to explain why they fail (p. 17). And **bash on regardless** (p. 18).

## Sources

- Cook (2012), pp. 16–18.
