---
title: "Tour of the United States (pp. 1–6)"
questions:
  - id: tsp-definition
    prompt: >-
      A platform team must send one maintenance agent to patch 40 edge sites and return to
      the depot, minimizing total travel time. Every pair of sites has a known travel time.
      Which statement captures this as the traveling salesman problem (TSP)?
    explanation: >-
      Cook states the general form plainly: “we are given a collection of cities and the
      distance to travel between each pair of them. The problem is to find the shortest route
      to visit each city and to return to the starting point” (p. 1). Two ingredients define it:
      a complete table of pairwise costs and a *closed* tour that visits every city exactly once.
      Any cost works — miles, minutes, dollars, machine setup time — as long as it is given
      for every pair. The edge sites are the cities, the travel times are the costs, and the
      depot round trip is the tour. Visiting a subset, or not returning, are different (if
      related) problems.
    options:
      - text: "Given pairwise costs between all sites, find a closed route that visits every site exactly once with minimum total cost"
        correct: true
      - text: "Find the shortest path from the depot to the farthest site"
      - text: "Assign each site to its nearest depot so that no depot is overloaded"
      - text: "Choose the cheapest subset of sites that covers the most customers"
  - id: count-tours
    prompt: >-
      With symmetric costs, how many distinct tours must brute force consider for an n-city
      instance, and why?
    explanation: >-
      A tour is a cyclic ordering of the cities. Fix the starting city (rotations describe the
      same tour), leaving (n − 1)! orderings of the rest. With symmetric costs a tour and its
      reverse have the same length, so divide by two: (n − 1)!/2. Cook walks through exactly
      this for Car 54: “we may as well always start with city A. This leaves 32 choices for
      the second city, 31 choices for the third city … Altogether … 32!” and then “We can
      therefore cut down by half our count … leaving only 32!/2 orderings to check” (p. 3).
      For the 33-city contest that is about 1.3 × 10³⁵ tours. In code terms, the search space
      is permutations modulo rotation and reflection.
    options:
      - text: "n!, every permutation is a different tour"
      - text: "n², because each city connects to every other city"
      - text: "2ⁿ, one for each subset of cities"
      - text: "(n − 1)!/2 — fix the start city to remove rotations, then halve to remove reversals"
        correct: true
  - id: roadrunner-scale
    prompt: >-
      Cook estimates that the 2009 Roadrunner supercomputer (about 1.5 × 10¹⁵ operations per
      second), checking one tour per operation, would need roughly 28 trillion years for the
      33-city Car 54 problem. What is the lesson for an engineer?
    explanation: >-
      The point is factorial growth, not slow hardware. 32!/2 ≈ 1.3 × 10³⁵ tours divided by
      ≈10¹⁵ per second is ≈10²⁰ seconds — “roughly 28 trillion years … given that the universe
      is estimated to be only 14 billion years old” (p. 3). A thousandfold faster machine
      shaves three zeros off a 35-zero number. Each extra city multiplies the work by roughly n.
      This is why ‘just throw more cores at it’ fails for exhaustive enumeration and why the
      rest of the book is about smarter methods: heuristics for good tours and LP bounds for
      proofs.
    options:
      - text: "Brute force is hopeless because the count grows factorially; speedups in hardware cannot keep up with a factor of n per added city"
        correct: true
      - text: "Use 64-bit integers so tour lengths do not overflow"
      - text: "Buy a faster cluster; the problem is mainly hardware-bound"
      - text: "Parallelize across 1,000 nodes and it becomes tractable"
  - id: menger-claim
    prompt: >-
      Karl Menger wrote in 1930 that the problem is “solvable by finitely many trials” and
      that rules giving fewer trials than the number of permutations “are not known.” What did
      Menger *not* claim?
    explanation: >-
      Cook stresses this carefully: “we must keep in mind that Menger writes only that faster
      rules for solving the salesman problem are unknown, not that such rules are out of the
      question” (pp. 3–4). Unknown is not impossible. He then quotes Little et al. about the
      1962 contest: “A number of people, perhaps a little over-educated, wrote the company
      that the problem was impossible—an interesting misinterpretation of the state of the art”
      (p. 4). The same confusion shows up in engineering reviews when someone hears ‘NP-hard’
      and concludes ‘cannot be solved’ — in reality large instances are solved optimally
      every day.
    options:
      - text: "That the problem concerns the shortest route through given points"
      - text: "That the TSP can be solved by checking finitely many tours"
      - text: "That no method faster than enumeration was known"
      - text: "That faster methods are impossible"
        correct: true
  - id: rand-guarantee
    prompt: >-
      In 1954 Dantzig, Fulkerson and Johnson reported a 49-city tour of 12,345 miles. What
      made their result a *solution* rather than just a good route?
    explanation: >-
      They produced two matching objects: a tour of length 12,345 and a proof that “no tour
      through the 49 cities can have length less than 12,345 miles. Matching the proof with
      their tour of precisely this length shows that this particular instance of the TSP has
      been settled, once and for all” (p. 5). This pairing — an upper bound from a feasible
      solution plus a lower bound from a proof — is the central pattern of the whole book. In
      software terms it is like a passing test (the tour exists) plus a formal argument that
      nothing better can pass. The proof came from linear programming, the subject of chapters
      5–7.
    options:
      - text: "They produced a tour and a matching lower-bound proof that no tour is shorter"
        correct: true
      - text: "Their route was later confirmed by a computer search"
      - text: "Newsweek published it, so it was accepted"
      - text: "They checked every possible tour by hand"
  - id: upper-lower
    prompt: >-
      Your route optimizer returns a 980 km route and a provable lower bound of 970 km. Which
      statements are correct?
    type: multiple
    explanation: >-
      Any feasible tour is an *upper bound* on the optimum (the optimum can only be shorter or
      equal); any valid proof that all tours cost at least X is a *lower bound*. The optimum
      lies in [970, 980], so the reported route is at most (980 − 970)/970 ≈ 1.03% above
      optimal. Equality of the two numbers is what “solved” means, as with the 1954 result: a
      tour of 12,345 miles plus a proof that none is shorter (p. 5). Treat the gap as a quality
      SLA you can report to stakeholders without knowing the optimum itself.
    options:
      - text: "The optimal tour length is somewhere between 970 and 980 km"
        correct: true
      - text: "The returned route is at most about 1% longer than optimal"
        correct: true
      - text: "The optimal tour is exactly 975 km on average"
      - text: "If the bound rose to 980 km, the 980 km route would be proven optimal"
        correct: true
  - id: car54-winners
    prompt: >-
      In the 1962 Procter & Gamble Car 54 contest, several contestants submitted the optimal
      33-city tour, including Karg and Thompson. What did they lack?
    explanation: >-
      They found the shortest route with a “hit-or-miss heuristic strategy” (p. 6), but “no one
      in 1962 knew for certain that this was the shortest possible tour” (p. 5), and Little et
      al. “could not push their computer codes far enough to actually solve the 33-city
      challenge” (p. 4). Finding an optimal answer and *knowing* it is optimal are different
      achievements. A heuristic can hit the optimum by luck or skill; certification needs a
      bound. (The tie was broken by an essay on soap.)
    options:
      - text: "A legal tour that returned to Chicago"
      - text: "A proof that their tour was the shortest"
        correct: true
      - text: "Accurate distances between the cities"
      - text: "A computer to evaluate their tour length"
---
## Learning objectives

- State the traveling salesman problem precisely and recognize instances of it.
- Count tours and explain why factorial growth defeats brute force.
- Distinguish *finding* a good tour from *proving* a tour optimal.
- Read an optimality gap as an upper bound / lower bound pair.

## The problem

The **traveling salesman problem (TSP)** gives you a set of cities and a cost for travelling between every pair. You must find the cheapest *tour*: a closed route that visits each city exactly once and returns to the start (p. 1). The costs can be anything additive — miles, minutes, fuel, machine reconfiguration time.

The book opens with a 1962 Procter & Gamble contest: help TV police officers Toody and Muldoon drive through 33 locations, from Chicago back to Chicago, in the fewest miles, for a $10,000 prize.

## Why brute force fails

There are finitely many tours, so in principle you can try them all. Fixing the start city removes rotations, and symmetric costs let you ignore direction, leaving **(n − 1)!/2** tours. For 33 cities that is about 1.3 × 10³⁵. Even at one tour per operation on 2009's fastest supercomputer, the search takes about 28 trillion years (p. 3). Hardware improvements add constant factors. Each extra city multiplies the work by about n.

Menger noticed this in 1930. He said only that faster rules were *not known*, not that they could not exist (pp. 3–4).

## Tours plus proofs

In 1954 Dantzig, Fulkerson and Johnson at RAND solved a 49-city USA instance (one city per state plus Washington, D.C.) by hand. They gave a 12,345-mile tour **and** a proof that no tour is shorter (p. 5). That pair is the template for everything that follows:

| Object | Role | Where it comes from |
|---|---|---|
| A tour | Upper bound on the optimum | Heuristics (chapter 4) |
| A proof “every tour ≥ X” | Lower bound | Linear programming (chapters 5–7) |
| Tour length = X | Optimality certificate | Matching the two |

The Car 54 contestants who found the optimal route had no such proof. They had an answer without a certificate (pp. 5–6).

## In software terms

- An optimizer that returns only a route is a heuristic. One that also returns a bound tells you how far from optimal the route can be. Report both.
- The search space is *permutations modulo rotation and reflection*. If your test harness enumerates tours for tiny cases, enumerate (n − 1)!/2 and not n!.
- Exhaustive search is fine for about 10 cities and out of the question for 30. Plan for that before a product manager asks for “the optimal route” over 200 stops.

## Sources

- Cook, W. J. *In Pursuit of the Traveling Salesman*. Princeton University Press, 2012, pp. 1–6.
- Dantzig, G., R. Fulkerson, S. Johnson. “Solution of a large-scale traveling-salesman problem.” *Operations Research* 2 (1954), 393–410.
- Little, J. D. C., et al. “An algorithm for the traveling salesman problem.” *Operations Research* 11 (1963), 972–989.
