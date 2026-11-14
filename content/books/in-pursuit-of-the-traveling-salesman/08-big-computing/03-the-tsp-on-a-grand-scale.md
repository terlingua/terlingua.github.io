---
title: "The TSP on a Grand Scale (pp. 163–167)"
questions:
  - id: size-not-difficulty
    prompt: >-
      The Mona Lisa TSP has 100,000 cities, only slightly more than the solved 85,900. Why might it be
      much harder?
    explanation: >-
      “The image displayed in figure 1.8 reveals many cities close together on straight lines. This geometry
      suggests that the 85,900-city instance may be somewhat easy relative to its size, whereas the
      distribution of points in the Mona Lisa TSP looks as complicated as one might imagine” (p. 164). Size
      is a poor proxy for difficulty, and structure matters. Benchmarks should vary structure as well as n.
    options:
      - text: "Because it uses asymmetric costs"
      - text: "Because it was created later"
      - text: "Because 100,000 is a round number"
      - text: "Its point distribution is far more irregular, while pla85900 has easy grid-like structure"
        correct: true
  - id: mona-gap
    prompt: >-
      In January 2010 the Mona Lisa bound reached 5,757,044 against Nagata's 5,757,191 tour. What
      remained, and what does Cook say is needed?
    explanation: >-
      A gap of 147 units (0.0026%), obtained with 1,065 branch-and-cut subproblems over 66 days and 4.37
      CPU-years. “Further runs with Concorde could probably push the gap down a few more units, but to
      conclude things we almost certainly need new ideas, particularly in cutting-plane separation” (p. 164).
      More compute gives small gains, and closing the gap needs better algorithms. Chapter 13 gives the
      updated numbers.
    options:
      - text: "The bound exceeded the tour"
      - text: "Nothing; it was solved"
      - text: "A 147-unit gap; new separation ideas, not just more computing, are likely needed"
        correct: true
      - text: "A 10% gap; better heuristics needed"
  - id: world-instance
    prompt: >-
      How are travel costs defined in the World TSP (1,904,711 cities)?
    explanation: >-
      “The points are specified by their latitude and longitude, and the cost of travel between pairs is given
      by an approximation of the great circle distance, treating the earth as a ball; this cost function is a
      variation of the TSPLIB GEO-norm, scaled to provide distances in meters” (p. 164). The data came from
      NIMA and GNIS in 2001 and covers every populated place. Geodesic distance on a sphere is not planar
      Euclidean distance, so pick the right metric when your coordinates are lat/long.
    options:
      - text: "Straight lines on a Mercator map"
      - text: "Flight times"
      - text: "Road distances from Google"
      - text: "Approximate great-circle distances (Earth as a ball), in meters"
        correct: true
  - id: world-gap-source
    prompt: >-
      The World TSP gap shrank from 0.47% (2001) to 0.0476% (2010). What surprised researchers about how?
    explanation: >-
      “Nearly 75% of this improvement was achieved through better tours. The TSP world in 2001 would not have
      thought this to be possible: the commonly held view was that state-of-the-art TSP heuristics produced
      near-optimal tours and thus the initial optimality gap must be due to a weak LP bound. Helsgaun’s LKH
      code changed this perception” (pp. 165–166). Cook will not guess where the optimum lies. When a gap
      exists, you do not know which side is weak, so work on both.
    options:
      - text: "The instance was changed"
      - text: "It grew instead"
      - text: "The bound did all the work"
      - text: "About 75% came from better tours, contrary to the belief that heuristics were already near-optimal"
        correct: true
  - id: world-engineering
    prompt: >-
      Why can't Concorde use its domino-parity module on the World TSP?
    explanation: >-
      “Currently it is not possible to employ the domino-parity-separation module due to the size of the data
      set. This is good news. More work to do!” (p. 166). Algorithms that work at 100k may not scale in memory
      or time to 2M. Engineering for scale, such as decomposition, streaming and better data structures, can
      unlock existing mathematics.
    options:
      - text: "It was removed from Concorde"
      - text: "It only works for asymmetric costs"
      - text: "Domino-parity is invalid for geographic data"
      - text: "The module does not scale to the instance's size yet; an engineering opportunity"
        correct: true
  - id: star-tsp
    prompt: >-
      The 526,280,881-object Star TSP models what, and how is it attacked?
    explanation: >-
      Distance estimates were too coarse for 3-D travel (all stars would sit “on a small number of concentric
      spheres”), so “the data specifies cities as locations in the sky, with travel costs given by a measure
      of the angle determined by each pair of cities”, which models a telescope (p. 166). “Even an n² running
      time is enormous when n is five hundred million. Thus the goal for now is to develop methods that split
      the data set apart, establish bounds and tours in the components, and then piece things back together”
      (p. 166). In 2007 the gap was 0.410%, “about ten years behind the World TSP” (p. 167). At this scale
      even quadratic algorithms are too slow, so decomposition is mandatory, as with any planet-scale data
      system.
    options:
      - text: "Telescope slewing by angular distance; attacked by decomposition because even n² is infeasible"
        correct: true
      - text: "Light-year distances; solved optimally"
      - text: "Galaxy clustering; LP only"
      - text: "Starship travel; solved by brute force"
  - id: bosch-instances
    prompt: >-
      What are Bob Bosch's art instances, and why does Cook like them as challenges?
    explanation: >-
      Six instances from 100,000 cities (Mona Lisa) up to 200,000 (Vermeer’s *Girl with a Pearl Earring*),
      built by placing cities by image tone (chapter 11). They sit “just beyond the TSPLIB test collection,”
      with public data “available to anyone wanting to take a crack at finding better tours or establishing
      bounds” (pp. 163–164). They are open, graded and visually appealing benchmarks just past the current
      frontier.
    options:
      - text: "Image-derived instances of 100k–200k cities, publicly available, just beyond the solved frontier"
        correct: true
      - text: "Small puzzles for children"
      - text: "Asymmetric road networks"
      - text: "Random uniform points; easy to solve"
---
## Learning objectives

- Understand the open giant instances and what limits progress.
- Recognize decomposition as necessary at extreme scale.

## Open challenges at publication

| Instance | Cities | Best tour | Bound | Gap |
|---|---|---|---|---|
| Mona Lisa (Bosch) | 100,000 | 5,757,191 (Nagata, 2009) | 5,757,044 (2010) | 0.0026% |
| World | 1,904,711 | 7,515,790,345 (Helsgaun, 2010) | 7,512,218,268 | 0.0476% |
| Star (USNO A2.0) | 526,280,881 | — | — | 0.410% (2007) |

*(pp. 163–167)*

Lessons:

- **Structure beats size** when predicting difficulty (p. 164).
- **Both sides of the gap matter:** about 75% of the World gap closure came from tours (pp. 165–166).
- **Scale breaks algorithms:** domino-parity cannot yet run on World (p. 166). Stars need split-and-merge (p. 166).
- **Metrics:** great-circle for World, angular for Stars.

## In software terms

At hundreds of millions of items, O(n²) is not viable. Partition, solve the pieces, stitch them together, and track a global quality bound, the same pattern as MapReduce-style processing with error bounds.

## Sources

- Cook (2012), pp. 163–167.
- Mona Lisa TSP challenge page: math.uwaterloo.ca/tsp/data/ml/monalisa.html.
- World TSP page: math.uwaterloo.ca/tsp/world/.
