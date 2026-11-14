---
title: "Tour-finding Strategies (pp. 192–195)"
questions:
  - id: human-efficiency
    prompt: >-
      What do psychologists find notable about human tours on modest-size instances?
    explanation: >-
      “Human subjects consistently produce good-quality tours for modest-sized examples. And although many
      mathematical methods can easily match these results, it seems clear that humans use a smaller number of
      explicit calculations in arriving at their good tours” (pp. 192–193). People appear to rely on
      perception rather than arithmetic, which is why the TSP interests cognitive scientists.
    options:
      - text: "Humans always produce crossing tours"
      - text: "Humans are slower than brute force"
      - text: "Humans find optimal tours for 1,000 cities"
      - text: "Humans get good tours on small instances with surprisingly few explicit calculations"
        correct: true
  - id: gestalt
    prompt: >-
      In Vickers et al.'s study, one group optimized length and another made tours that looked
      “natural, attractive, or aesthetically pleasing.” Result?
    explanation: >-
      “The results showed a striking similarity in the quality of tours obtained by the two groups. The
      champion tour finder in the test was in fact a fashion designer belonging to the Gestalt group; she
      produced the shortest tours in five of the six examples” (p. 193). Good geometric tours “feel right”
      (p. 193). For Euclidean instances, short and smooth with no crossings tend to go together.
    options:
      - text: "The aesthetic group made crossing tours"
      - text: "Nobody finished"
      - text: "The optimization group was far better"
      - text: "Both groups were similar; a fashion designer in the aesthetic group was best"
        correct: true
  - id: children
    prompt: >-
      In van Rooij's study, how did 7-year-olds do on 15-city instances compared with adults?
    explanation: >-
      Average percent over optimal: 7-year-olds 9.4%, 12-year-olds 5.0%, adults 2.7% on 15 cities. For 5 cities
      the figures were 3.8%, 2.5% and 1.7% (table 10.1, p. 194). “Even young children obtained reasonably
      short tours” (p. 193), suggesting that perception does a lot of the work.
    options:
      - text: "About 9% above optimal, against about 3% for adults"
        correct: true
      - text: "Better than adults"
      - text: "Exactly optimal"
      - text: "About 50% above optimal"
  - id: convex-hull-rule
    prompt: >-
      What is the convex-hull rule for optimal Euclidean tours?
    explanation: >-
      The rubber band around all cities is the convex hull’s border. “In order to avoid crossings, an optimal
      tour must trace the border cities in the order in which they appear as we walk around the border”
      (p. 194). The 33-city Car 54 optimum visits its 12 hull cities in hull order, picking up interior points
      on short detours (p. 194). This follows from the no-crossing theorem and is a quick validity check for
      route visualizations.
    options:
      - text: "An optimal Euclidean tour visits the hull's boundary cities in their boundary order"
        correct: true
      - text: "The hull itself is always the optimal tour"
      - text: "Interior points must be visited first"
      - text: "Optimal tours avoid hull cities"
  - id: macgregor
    prompt: >-
      MacGregor and Ormerod hypothesize what about human TSP difficulty?
    explanation: >-
      “The complexity of finding an approximate solution to an instance of TSP is determined by the number of
      its interior points”, and humans use “the perception of global spatial properties … in particular, of
      the boundary of the convex hull” (pp. 194–195). This is debated: global-to-local (perceive shape, fit
      details) versus local-to-global (cluster, then link) (p. 195). It resembles convex-hull insertion
      heuristics.
    options:
      - text: "Difficulty depends on color"
      - text: "Difficulty depends only on n"
      - text: "Difficulty is driven by the number of interior (non-hull) points; humans start from the hull"
        correct: true
      - text: "Humans ignore geometry"
  - id: physical-tsp
    prompt: >-
      Wiener's Tübingen experiment differed from paper studies how?
    explanation: >-
      Participants physically walked a 6.0 × 8.4 m room to visit up to nine of 25 pillars and return. “The
      results again support the claim that humans are quite good at solving small instances of the TSP—in this
      case for a wage of eight euros per hour” (p. 195). Navigation, not just visual tracing, shows the same
      competence.
    options:
      - text: "It was done in virtual reality only"
      - text: "It used chimpanzees"
      - text: "Participants physically walked between targets in a room"
        correct: true
      - text: "It used 1,000 cities"
---
## Learning objectives

- Summarize findings on human TSP performance and strategies.
- State and justify the convex-hull rule.

## Findings

- People find good tours on small instances with few explicit calculations (pp. 192–193).
- **Gestalt:** aiming for beauty works as well as aiming for shortness. A fashion designer won (p. 193).
- **Children:** worse than adults but reasonable (table 10.1, p. 194).

| Cities | 7-year-olds | 12-year-olds | Adults |
|---|---|---|---|
| 5 | 3.8% | 2.5% | 1.7% |
| 10 | 5.8% | 3.4% | 1.7% |
| 15 | 9.4% | 5.0% | 2.7% |

- **Convex-hull rule:** optimal Euclidean tours visit hull cities in hull order (p. 194). MacGregor and Ormerod argue interior points drive difficulty (pp. 194–195).
- **Physical navigation** shows the same competence (p. 195).

## In software terms

Interactive route tools should let users drag and adjust plans visually. Humans are good at spotting crossings and odd detours. A human-in-the-loop override (Cleveland’s letter!) plus a solver often beats either alone.

## Sources

- Cook (2012), pp. 192–195.
- Vickers, D., et al. “Human performance on visually presented traveling salesman problems.” *Psychological Research* 65 (2001), 34–45.
- van Rooij, I., et al. “Children’s performance on the Euclidean traveling salesperson problem.” *J. Problem Solving* 1 (2006), 44–73.
- MacGregor, J. N., T. Ormerod. “Human performance on the traveling salesman problem.” *Perception & Psychophysics* 58 (1996), 527–539.
