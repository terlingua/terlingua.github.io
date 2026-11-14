---
title: "Before the Mathematicians (pp. 19–26)"
questions:
  - id: cleveland-letter
    prompt: >-
      Salesman H. M. Cleveland complained in 1925 that his Maine route was “balled up” because
      “the river from Bangor down has no bridge.” What modeling lesson does this teach?
    explanation: >-
      The office planned the route as if he “could cross it anywhere” (p. 21), effectively using
      straight-line distance. Real travel costs come from the road network, so
      two towns that are close geographically can be far apart in travel. Cook notes that the
      apparent backtracking in Cleveland’s good tour is “due to the available road network,
      where one town can only be reached by traveling to and from another town” (p. 20). For a
      routing service, this means computing a cost matrix from road (or network) data rather
      than Euclidean distance, and treating stale cost data as a production bug.
    options:
      - text: "The cost matrix must reflect the real network; geometry alone can mislead"
        correct: true
      - text: "Salesmen should avoid rivers"
      - text: "Tours should never revisit a town"
      - text: "Straight-line distances are always good enough"
  - id: commis-voyageur
    prompt: >-
      The 1832 German handbook by an “alten Commis-Voyageur” is important to TSP history
      because it …
    explanation: >-
      The handbook says “The main thing to remember is always to visit as many localities as
      possible without having to touch them twice,” and Cook remarks: “This is an explicit
      description of the TSP, made by a traveling salesman himself!” (p. 23). One of its five
      routes is a true TSP tour (the 33-city “Commis tour” of p. 14), “very good, perhaps even optimal, given
      road conditions at the time” (p. 23). Practitioners were optimizing tours long before
      mathematicians formalized the problem.
    options:
      - text: "describes Euler’s bridge problem"
      - text: "contains the first proof that the TSP is hard"
      - text: "states the TSP objective explicitly and gives a near-optimal tour, a century before the mathematics"
        correct: true
      - text: "introduces the term “traveling salesman problem”"
  - id: circuit-origin
    prompt: >-
      Why are U.S. regional courts called *circuit* courts?
    explanation: >-
      Judges and lawyers once “served their districts by riding a circuit of the towns and
      villages, where court was held during specified times of the year,” a practice adopted in
      the United States where “regional courts are still referred to as circuit courts, even
      though judges no longer take to the road” (p. 24). Abraham Lincoln rode Illinois’s Eighth
      Judicial Circuit, 14 county seats, more than 400 miles over eleven weeks. His 1850 route
      was “not quite the shortest possible” but clearly “constructed with an eye toward
      minimizing the travel” (p. 25).
    options:
      - text: "Because of electrical circuits in early courthouses"
      - text: "Because Lincoln designed them"
      - text: "Because courtrooms were round"
      - text: "Because judges travelled a circuit of towns holding court, a tour-like route"
        correct: true
  - id: preachers
    prompt: >-
      Methodist circuit preachers covered circuits of 20–30 places. Why does Cook infer that
      route planning mattered to them even without detailed itineraries?
    explanation: >-
      “A goal of their work was to reach as many church members as possible, so minimizing time
      on the trail would have been an important consideration” (p. 26). Billy Hibbard in 1825
      preached 63 sermons in four weeks over a 500-mile circuit. When the resource you are
      maximizing (sermons, customer visits, deploys) competes with travel time, routing
      becomes part of the economics.
    options:
      - text: "Because time travelling was time not spent serving members, so shorter circuits meant more service"
        correct: true
      - text: "Because they had GPS"
      - text: "Because Wesley required optimal tours"
      - text: "Because they wrote down every route"
  - id: pins-and-strings
    prompt: >-
      The 1922 Rand McNally pin map in figure 2.3 shows central offices planning salesmen's
      routes with pins and strings. What is the modern equivalent?
    explanation: >-
      Tours were “often … planned in a central office, such as was done in the Page Seed Company.
      The images in figure 2.3 indicate one way such tours were optimized, using pins and strings
      to plot potential routes on a map” (p. 22). The string measures a route physically. Today a
      dispatch service computes a cost matrix and calls a solver, and field staff still override
      plans with local knowledge, as Cleveland did. Chapter 4 shows Dantzig et al. used the same
      peg-and-string device in 1954.
    options:
      - text: "Random assignment of towns"
      - text: "A blockchain of routes"
      - text: "A centralized routing service that computes tours from a cost matrix for field agents"
        correct: true
      - text: "Each salesman planning alone"
  - id: why-salesman
    prompt: >-
      Why is the salesman a fitting namesake for the problem?
    explanation: >-
      There were an estimated 200,000 American traveling salesmen in 1883 and 350,000 by 1900
      (p. 22). Their route planning appears in guidebooks, poems, and even an 1890 board game,
      *Commercial Traveller*, that asked players to build tours (p. 24). Cook concludes “the
      choice of the salesman as the representative for the TSP is definitely well founded” (p. 24).
      The name was a happy accident (chapter 2 later shows nobody knows who coined it), but it
      fits the history.
    options:
      - text: "Because the problem is about selling"
      - text: "Because RAND sold the solution"
      - text: "Because Menger was a salesman"
      - text: "Because salesmen were the most visible large group who planned multi-stop round trips, documented in guides, letters and games"
        correct: true
---
## Learning objectives

- Recognize that the TSP was practiced long before it was formalized.
- Draw modeling lessons from historical tours: real networks, constraints, human overrides.

## Practitioners first

- **Salesmen.** H. M. Cleveland of the Page Seed Company made 350 stops in Maine in 1925 and over 1,000 across several states. His route was efficient, and his letter complaining about an office-planned route that ignored a river with no bridge reads like a modern bug report (pp. 19–21). Offices planned routes with pins and string on map cabinets (p. 22).
- **The Commis-Voyageur (1832).** A German salesman’s handbook states the TSP objective (“visit as many localities as possible without having to touch them twice”) and gives a 33-city tour (p. 14) that may be optimal for its time (pp. 22–23).
- **Lawyers and judges.** “Circuit” courts get their name from judges who travelled. Lincoln’s Eighth Judicial Circuit, 14 county seats and more than 400 miles, was planned to keep travel low (pp. 24–25).
- **Preachers.** Methodist circuits of 20–30 places took a month or six weeks to cover. Less time on the trail meant more people reached (pp. 25–26).

## Lessons for modeling

1. **Use the real cost.** Road networks, rivers and one-way streets break simple geometry.
2. **Humans add constraints the model lacks**, such as bridges, market days and customer preferences. Build in a way for them to override the plan.
3. **Good enough is old.** Practitioners found near-optimal tours by intuition. What mathematics added was certainty and scale.

## Sources

- Cook (2012), pp. 19–26.
- Spears, T. *100 Years on the Road: The Traveling Salesman in American Culture*. Yale University Press, 1994.
- Fraker, G. C. “The real Lincoln highway.” *J. Abraham Lincoln Assoc.* 25 (2004), 76–97.
