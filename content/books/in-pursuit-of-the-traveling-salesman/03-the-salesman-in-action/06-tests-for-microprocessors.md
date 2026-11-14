---
title: "Tests for Microprocessors (pp. 59–60)"
questions:
  - id: scan-chain
    prompt: >-
      What is a scan chain, and why is its ordering a TSP?
    explanation: >-
      “Scan chains were introduced in the 1980s to link components, or scan points, of a computer
      chip in a path having input and output connections on the chip’s boundary” (p. 59). Test data
      shifts in at one end and results shift out at the other. “The TSP is used to determine the
      ordering of scan points to make the chain as short as possible” (p. 59), which saves wiring
      space and speeds testing. The endpoints are fixed at the In and Out pins, so it is a
      Hamiltonian path with fixed ends, handled with a dummy city connected only to them.
    options:
      - text: "A list of test cases; ordering is alphabetical"
      - text: "A wired path through test points from an input to an output pin; shortest ordering is a fixed-endpoint TSP path"
        correct: true
      - text: "A clock tree; ordering is irrelevant"
      - text: "A memory cache line; ordering is FIFO"
  - id: manhattan-metric
    prompt: >-
      Distances in scan-chain TSPs are measured how, and why?
    explanation: >-
      “In most cases chip manufacturing technology allows only horizontal and vertical connections,
      thus the distance between two points in a scan-chain TSP is measured using paths that travel
      only horizontally or vertically, such as walking the streets of Manhattan” (p. 59). That is
      the L1 or Manhattan metric |x₁ − x₂| + |y₁ − y₂|. It still satisfies the triangle inequality,
      so metric-TSP guarantees such as Christofides still apply. Use the metric your hardware or
      network actually follows.
    options:
      - text: "Straight-line Euclidean, because wires are straight"
      - text: "Manhattan (L1), because wires run only horizontally and vertically"
        correct: true
      - text: "Great-circle distance"
      - text: "Number of transistors between points"
  - id: multiple-chains
    prompt: >-
      The 764-city Sun Microsystems example was “one of twenty-five chains on the given chip.” Why
      split into many chains?
    explanation: >-
      “To reduce the time required for testing, a modern computer chip will typically have multiple
      scan chains” (p. 60). Shorter parallel chains shift data in parallel, which reduces test time,
      much like sharding a long sequential job. Each chain is a TSP path, and assigning points to
      chains is a separate partitioning decision.
    options:
      - text: "Because each chain tests a different chip"
      - text: "Because Concorde cannot solve more than 764 cities"
      - text: "Parallel shorter chains cut test time; each chain is its own TSP path"
        correct: true
      - text: "To increase wiring"
  - id: nvidia
    prompt: >-
      Which company does the book say adopted Concorde for on-chip test circuitry of graphics
      processors?
    explanation: >-
      “The computing technology firm NVIDIA recently adopted Concorde in optimizing the on-chip
      circuitry used to test their graphics processors” (p. 59). An exact academic solver ended up in
      a production EDA flow.
    options:
      - text: "Qualcomm"
      - text: "Intel"
      - text: "NVIDIA"
        correct: true
      - text: "Apple"
---
## Learning objectives

- Model scan-chain ordering as a fixed-endpoint TSP path in the Manhattan metric.

## Summary

Scan chains connect a chip’s test points into a path from an input pin to an output pin. Shorter chains save wiring and make testing faster. Wires run horizontally and vertically, so distances are **Manhattan**. Sun’s 764-point chain was one of 25 on its chip, solved with Concorde. NVIDIA used Concorde for GPU test circuitry (pp. 59–60).

```csharp
static int Manhattan((int X, int Y) a, (int X, int Y) b) =>
    Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
```

## In software terms

Splitting one long chain into many parallel short ones is the same trade as sharding a sequential pipeline: partitioning is one decision and ordering inside each shard is another.

## Sources

- Cook (2012), pp. 59–60.
