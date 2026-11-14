---
title: "Vienna to Harvard to Princeton (pp. 35–38)"
questions:
  - id: messenger-problem
    prompt: >-
      Menger's 1930 “messenger problem” asks for the shortest *path* through all points, with no
      return trip. How does Cook turn it into a TSP?
    explanation: >-
      “This is easily converted to the TSP by adding an extra ‘dummy’ city that serves to link
      the ends of the path. The cost of travel between the dummy and each of the real cities can
      be set to zero, so that visiting the extra city will not influence the choice of the path’s
      starting point or ending point” (p. 36). An optimal tour enters and leaves the dummy for
      free. Delete the dummy and you have an optimal open path. Chapter 3 reuses the trick for
      genome maps, texture layouts and clustering.
    options:
      - text: "Duplicate every city"
      - text: "Add a dummy city joined to every real city at cost zero, solve the TSP, then delete the dummy"
        correct: true
      - text: "Set all costs to zero"
      - text: "Remove the most expensive edge from any tour"
  - id: dummy-fixed-endpoints
    prompt: >-
      A deployment pipeline must run 12 jobs starting with `build` and ending with `release`,
      minimizing total switchover cost. How would you adapt the dummy-city trick?
    explanation: >-
      With a free dummy, the tour can attach to any cities, so the path ends are free. To force
      specific ends, connect the dummy at cost 0 only to `build` and `release`, and give every
      other dummy edge a prohibitive cost. The optimal tour then enters and leaves the dummy
      through those two jobs, and deleting the dummy gives a path from `build` to `release`. This
      is the same kind of reduction Cook uses for Menger’s problem (p. 36): change the input so
      that an existing solver answers your question.
    options:
      - text: "It cannot be done with a TSP solver"
      - text: "Give the dummy zero-cost edges only to `build` and `release` (huge cost elsewhere), solve, and delete the dummy"
        correct: true
      - text: "Give the dummy huge costs to all jobs"
      - text: "Remove `build` and `release` from the instance"
  - id: whitney-flood
    prompt: >-
      According to Dantzig, Fulkerson and Johnson, how did the TSP reach U.S. mathematicians?
    explanation: >-
      “Both Flood and A. W. Tucker (Princeton University) recall that they heard the problem first
      in a seminar talk by Hassler Whitney at Princeton in 1934 (although Whitney, recently
      queried, does not seem to recall the problem)” (p. 36). Merrill Flood, who had tried
      school-bus routing as early as 1937, called it the “48-states problem of Hassler Whitney.”
      Menger and Whitney met at Harvard in 1930–31, so Whitney may have been “a messenger from
      Menger” (pp. 36–38).
    options:
      - text: "Through Euler's collected works"
      - text: "Through a 1934 Princeton seminar by Hassler Whitney, popularized by Merrill Flood"
        correct: true
      - text: "Through a 1954 Newsweek article"
      - text: "Through the Commis-Voyageur handbook"
  - id: whitney-notes
    prompt: >-
      Whitney's archived notes pose: “Given a set of countries, is it possible to travel through
      them in such a way that at the end of the trip we have visited each country exactly once?”
      What graph problem is this?
    explanation: >-
      Place a vertex in each country and join two vertices if the countries share a border. “A
      trip through the countries is a Hamiltonian circuit in the graph” (p. 37). Whitney called
      it “much more difficult” than the Euler problem, and it is “clearly not a far step from the
      TSP” (p. 37). It matches Flood’s memory of a 48-states problem.
    options:
      - text: "Minimum spanning tree"
      - text: "Eulerian walk in the border graph"
      - text: "Hamiltonian circuit in the country-adjacency graph"
        correct: true
      - text: "Four-coloring"
  - id: menger-context
    prompt: >-
      What research interest probably led Menger to the messenger problem?
    explanation: >-
      “One of Menger’s favorite topics in the 1920s was the study of techniques to measure lengths
      of curves in space. This esoteric research likely provided inspiration” (pp. 35–36). He
      also observed that the problem is faced “by every postman, and, by the way, also by many
      travelers” (p. 36). Abstract research on curve length led to a very concrete optimization
      problem.
    options:
      - text: "Cryptography"
      - text: "Railway timetables"
      - text: "Quantum mechanics"
      - text: "Measuring lengths of curves"
        correct: true
---
## Learning objectives

- Reduce a shortest Hamiltonian *path* problem to a TSP with a dummy city.
- Trace how the problem moved from Vienna to Princeton.

## Menger’s messenger

On February 5, 1930, Karl Menger posed the **messenger problem** in Vienna: given points and pairwise distances, find the shortest *path* connecting them (pp. 35–36). He was interested in measuring the lengths of curves.

**Reduction to the TSP.** Add a dummy city with cost 0 to every real city. An optimal tour passes through the dummy, and removing it leaves an optimal path (p. 36). To fix the path’s endpoints, give the dummy zero-cost edges only to the required start and end cities and very large costs to all others.

```csharp
static double[,] AddDummy(double[,] cost, int? start = null, int? end = null, double big = 1e12)
{
    var n = cost.GetLength(0);
    var c = new double[n + 1, n + 1];
    for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            c[i, j] = cost[i, j];
    for (var i = 0; i < n; i++)
    {
        var free = start is null || i == start || i == end;
        c[i, n] = c[n, i] = free ? 0 : big;
    }
    return c;
}
```

## To Princeton

Flood and Tucker recalled hearing the problem in **Hassler Whitney**’s 1934 Princeton seminar. Whitney’s Harvard notes pose a Hamiltonian-circuit question through countries, which matches Flood’s “48-states problem.” Menger visited Harvard in 1930–31, so Whitney may have carried the problem from Vienna (pp. 36–38).

## Sources

- Cook (2012), pp. 35–38.
- Menger, K. “Das Botenproblem.” *Ergebnisse eines Mathematischen Kolloquiums* 2 (1932), 11–12.
- Schrijver, A. “On the history of combinatorial optimization (till 1960).” In *Handbook of Discrete Optimization* (2005).
