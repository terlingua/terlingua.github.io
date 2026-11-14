---
title: "Aiming Telescopes, X-rays, and Lasers (pp. 51–53)"
questions:
  - id: slewing
    prompt: >-
      In telescope scheduling, what are the “cities” and the “travel costs”?
    explanation: >-
      “The cities in the TSP are the objects to be imaged and the travel costs are estimates of
      the slewing times to move from one object to the next” (p. 51). Nothing physically travels
      between sites. The instrument changes orientation, and the cost is repositioning time. The
      same applies to a disk head, a robotic arm, a camera gimbal, or a job scheduler that pays
      a context-switch cost.
    options:
      - text: "Nights, and weather forecasts"
      - text: "Galaxies, and their distances from Earth"
      - text: "Observatories, and distances between them"
      - text: "Objects to image, and slewing (repositioning) times between them"
        correct: true
  - id: fragile-telescope
    prompt: >-
      Shawn Carlson used a TSP heuristic for an old telescope imaging about 200 galaxies per night.
      What was his main concern?
    explanation: >-
      “Because large excursions from horizon to horizon sent the telescope’s 40-year-old drive
      system into shock, it was vital that the feeble old veteran be moved as little as possible”
      (p. 51). The objective was equipment wear, not just time. The cost function can encode
      wear, energy or risk, so pick the objective that matches what you are protecting.
    options:
      - text: "Minimizing large motions that stressed a 40-year-old drive system"
        correct: true
      - text: "Maximizing the number of galaxies"
      - text: "Avoiding cloud cover"
      - text: "Saving electricity"
  - id: occulter
    prompt: >-
      For NASA planet-finding with a starshade (occulter), why is the observation sequence planned
      years in advance instead of nightly?
    explanation: >-
      “This preplanning is due to the great amount of fuel consumed in slewing operations and to
      the length of time needed to study each star” (p. 51). Martin Lu estimated about fifty stars
      in a three-year mission. Kolemen and Kasdin optimized fuel for an occulter 50,000–100,000 km
      from the telescope, choosing 80 of the top 100 candidate stars with two occulters (p. 52).
      When moves are expensive and irreversible, as with fuel in space, offline global
      optimization pays for itself.
    options:
      - text: "Because the telescope cannot rotate"
      - text: "Because stars move unpredictably"
      - text: "Because slewing burns scarce fuel and each observation is long, so the global sequence matters"
        correct: true
      - text: "Because NASA forbids nightly planning"
  - id: diffractometer
    prompt: >-
      Bland and Shallcross applied the TSP to X-ray crystallography at Cornell. What improvement
      did they report?
    explanation: >-
      Experiments can need up to 30,000 observations per crystal, and the costs are motor
      repositioning times. They “reported improvements of up to 46% in total slewing time with the
      help of TSP methods” (p. 53). Sequencing many small repositioning moves well can nearly halve
      instrument time.
    options:
      - text: "Exactly 1% improvement"
      - text: "No improvement"
      - text: "10× more crystals"
      - text: "Up to 46% less slewing time"
        correct: true
  - id: largest-industrial
    prompt: >-
      Laser crystal art (e.g., Precision Laser Art) is singled out for what reason?
    explanation: >-
      A laser fractures points inside clear crystal, and the TSP orders the points. “This
      application holds a place of honor as having generated the largest industrial instances of
      the TSP we have encountered to date, with some examples exceeding one million cities” (p. 53).
      Dickens used Concorde’s heuristic methods, because exact solution at a million points is out
      of reach. In production at that scale, a good heuristic tour is the deliverable.
    options:
      - text: "It was solved by humans"
      - text: "It was the first TSP application"
      - text: "It produced the largest industrial TSP instances, over a million cities, handled with heuristics"
        correct: true
      - text: "It required asymmetric costs"
---
## Learning objectives

- Recognize “aiming” TSPs, where an instrument changes orientation instead of travelling.
- Choose cost functions that encode time, fuel or wear.

## Examples

| Application | Cities | Cost | Notes |
|---|---|---|---|
| Ground telescopes | Objects to image | Slewing time | Carlson: protect a 40-year-old drive (p. 51) |
| Space planet-finding | Target stars | Fuel for occulter moves | Planned years ahead (pp. 51–52) |
| X-ray crystallography | Crystal orientations | Motor reposition time | Up to 46% savings (pp. 52–53) |
| Laser crystal art | 3-D points | Laser travel | Over a million cities, heuristic tours (p. 53) |

## In software terms

Anything that pays a cost to *reposition* between tasks fits this model: disk-head scheduling, robot pick paths, camera tours in visual inspection, GPU kernel ordering to reuse state. The TSP applies whenever the cost depends on the ordered pair of consecutive tasks.

## Sources

- Cook (2012), pp. 51–53.
- Carlson, S. “Algorithm of the gods.” *Scientific American* 276 (March 1997), 121–124.
- Bland, R. G., D. F. Shallcross. “Large travelling salesman problems arising from experiments in X-ray crystallography.” *Oper. Res. Letters* 8 (1989), 125–128.
- Kolemen, E., N. J. Kasdin. “Optimal trajectory control of an occulter-based planet-finding telescope.” *Adv. Astronaut. Sci.* 128 (2007), 215–233.
