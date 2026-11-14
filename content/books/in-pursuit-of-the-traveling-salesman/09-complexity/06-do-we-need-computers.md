---
title: "Do We Need Computers? (pp. 184–190)"
questions:
  - id: adleman-dna
    prompt: >-
      How did Adleman's 1994 DNA experiment encode a Hamiltonian path problem?
    explanation: >-
      Each of 7 vertices got a random 20-letter DNA tag. A directed edge a→b was encoded as the second half of
      a’s tag joined to the first half of b’s tag. Complementary “splint” strands of each vertex tag glued
      consecutive edges together. Many copies were mixed so that paths self-assembled in parallel, and “after
      careful work over a seven-day period, a double strand yielding a Hamiltonian path was identified”
      (pp. 184–186). Molecules provide massive parallelism.
    options:
      - text: "Vertices as proteins, edges as enzymes"
      - text: "Vertex tags as DNA strands, edges as half-tag joins, complementary splints to assemble paths in parallel"
        correct: true
      - text: "A DNA sequencer ran Held–Karp"
      - text: "Edges as RNA, vertices as ribosomes"
  - id: exponential-resources
    prompt: >-
      DNA, bacterial, amoeba and optical TSP solvers all share what limitation?
    explanation: >-
      “DNA, bacteria, amoeba, and optical TSP solvers all have an all-tours-at-once aspect, but they also
      require resources that grow exponentially with the number of cities” (p. 189). Parallelism moves the
      exponential cost from time to molecules, cells, photons or cable. The optical machine needs delays of at
      least 2ⁿ units (Oltean). A 33-vertex graph would take a second but 8 × 10¹¹ m of cable, and a few hundred
      vertices would need more photons than the sun emits in a year (pp. 188–189).
    options:
      - text: "Their physical resources (molecules, cells, photons, cable) grow exponentially with n"
        correct: true
      - text: "They are slower than brute force"
      - text: "They give wrong answers"
      - text: "They only work on Tuesdays"
  - id: optical-hype
    prompt: >-
      The 2007 light-based Hamiltonian-path machine seemed to run in time proportional to graph size. What
      did Oltean's analysis show?
    explanation: >-
      “In choosing the delays to give unique signatures, we necessarily have D at least as large as 2ⁿ time
      units” (p. 188). Oscilloscopes resolve about 10⁻¹² s, so solve time is at least 2ⁿ × 10⁻¹² s, which is
      exponential. The headline claim hid an exponential cost in the signal encoding. When a system claims to
      beat a known complexity barrier, look for where the cost went: memory, precision, hardware, or encoding
      size.
    options:
      - text: "Unique delay signatures force total delay ≥ 2ⁿ units, so time is exponential after all"
        correct: true
      - text: "It needed no light"
      - text: "It solved the World TSP"
      - text: "It was polynomial"
  - id: quantum-misconception
    prompt: >-
      100 qubits can represent 2¹⁰⁰ possibilities at once. Why doesn't that make the TSP easy?
    explanation: >-
      “Although all tours are represented simultaneously, when we actually examine the states of the qubits,
      all but one of the tours will vanish. Given this disappearing act, how can we guide the machine to
      select the best tour? It is not at all clear that this is possible” (p. 189). Aaronson: polynomial
      quantum algorithms for NP-complete problems would have to exploit structure “unlike anything we have
      seen … Quantum magic by itself is not going to do the job” (pp. 189–190). Shor’s algorithm factors
      quickly, but factoring is not known to be NP-complete. For unstructured search, Grover gives only a
      square-root speedup, which is still exponential for the TSP.
    options:
      - text: "Quantum computers can't store numbers"
      - text: "Because the TSP is not in NP"
      - text: "Qubits are too slow"
      - text: "Measurement yields one outcome; without exploitable structure, superposition alone doesn't find the best tour"
        correct: true
  - id: amoeba
    prompt: >-
      In the amoeba TSP solver, how were constraints like “city A appears once” enforced?
    explanation: >-
      A star-shaped chamber with 16 arms, labelled A1…D4 for city and position. The amoeba avoids light, so
      when it grew into A4 the program lit A1, A2, A3 (A only once) and B4, C4, D4 (one city in position 4).
      Distance was encouraged by lighting far cities in adjacent positions. It settled on D-C-B-A (pp. 187–188).
      Constraints become penalties applied as feedback, similar to Lagrangian penalties. The authors stress
      the novelty of a biological computer, not the 4-city scale.
    options:
      - text: "By shining light on forbidden arms as feedback penalties"
        correct: true
      - text: "By cutting the amoeba"
      - text: "By temperature changes"
      - text: "By chemical poisons"
  - id: pegs-practical
    prompt: >-
      What does Cook call the most practical physical TSP aid devised to date?
    explanation: >-
      “A map with pins or pegs at the destination cities, and a string to lay out potential tours. This is a
      device to speed up by-hand computations, measuring the length of a tour by keeping a grip on the far end
      of the string. A bit easier to build than a time-traveling quantum computer, and the most practical
      physical TSP aid devised to date” (p. 190). Simple tools that support human judgment often beat exotic
      machines.
    options:
      - text: "Pegs and string on a map"
        correct: true
      - text: "A DNA computer"
      - text: "Closed timelike curves"
      - text: "A quantum computer"
---
## Learning objectives

- Evaluate unconventional computing claims against exponential-resource arguments.
- Understand why quantum computing is not an automatic TSP solver.

## Survey (pp. 184–190)

| Device | Idea | Catch |
|---|---|---|
| DNA (Adleman 1994) | Strands assemble all paths in parallel | Molecules grow exponentially. 7 vertices took a week |
| Bacteria | Fluorescent colonies indicate paths | 3 cities |
| Amoeba (Aono et al.) | Shape-shifting with light penalties | 4 cities |
| Optics (2007) | Light through delay-line graph | Delays ≥ 2ⁿ, cable and photon counts explode |
| Quantum | Superposition of tours | Measurement returns one outcome, needs structure (Aaronson) |
| Time travel (CTCs) | Receive the answer from the future | Speculative physics |
| **Pegs and string** | Physical measurement | The most practical so far |

## In software terms

When a vendor claims to beat NP-hardness with special hardware, ask where the exponential cost went: hardware count, precision, preprocessing, or encoding. Quantum annealers and “Ising machines” are heuristics, and they need benchmarking against LKH like everything else.

## Sources

- Cook (2012), pp. 184–190.
- Adleman, L. M. “Molecular computation of solutions to combinatorial problems.” *Science* 266 (1994), 1021–1024.
- Oltean, M. “Solving the Hamiltonian path problem with a light-based computer.” *Natural Computing* 7 (2008), 57–70.
- Aaronson, S. “The limits of quantum computers.” *Scientific American* 298(3) (2008), 62–69.
- Aono, M., et al. “Amoeba-based chaotic neurocomputing.” *New Generation Computing* 27 (2009), 129–157.
