---
title: "A Perfect Relaxation (pp. 118–122)"
questions:
  - id: halfspace
    prompt: >-
      Geometrically, what does a single linear inequality like x + y ≤ 13 describe?
    explanation: >-
      The line x + y = 13 “separates points (x, y) into two sets, those on the forbidden side of the line
      and those on the allowable side, where points on the line itself are also allowable. The allowable
      side is called a halfspace” (p. 118). The feasible region of an LP is the intersection of the
      halfspaces of all its constraints. In higher dimensions the boundary line becomes a hyperplane.
    options:
      - text: "A single point"
      - text: "A parabola"
      - text: "A circle"
      - text: "A halfspace — all points on one side of a line, including the line"
        correct: true
  - id: corner-optimal
    prompt: >-
      Maximize x + 2y subject to x + y ≤ 13, x ≤ 8, y ≤ 8, x, y ≥ 0. Where is the optimum, and what
      general fact does this illustrate?
    explanation: >-
      The corners are (0,0), (8,0), (8,5), (5,8) and (0,8). x + 2y is largest at (5,8), giving 21 (p. 119).
      “No matter what objective is set for the LP model, we can be sure that one of these five corner points
      will be an optimal solution … if we push any line as far as we can it will always meet one of the
      corners” (p. 119). An LP with infinitely many feasible points can be solved by considering finitely
      many corners, and simplex moves from corner to corner along edges.
    options:
      - text: "(8, 8); optima are always at maximum coordinates"
      - text: "(5, 8); some corner of the feasible region is always optimal"
        correct: true
      - text: "(6.5, 6.5); optima are at the center"
      - text: "(8, 5); optima prefer x"
  - id: why-not-list-corners
    prompt: >-
      If an optimum is always at a corner, why not just enumerate all corners?
    explanation: >-
      “For a big problem there could be many combinations (corner points)—perhaps as many as the stars in
      the heavens” (Dantzig, quoted p. 119). In d dimensions a corner is an intersection of d bounding
      hyperplanes, and the number of possible choices explodes combinatorially. Simplex visits only a path
      of improving corners. This is the same contrast as brute force versus smart search for tours.
    options:
      - text: "Because corners cannot be computed"
      - text: "Because LP has no corners"
      - text: "Because corners are not optimal"
      - text: "There can be astronomically many corners; simplex walks only an improving path"
        correct: true
  - id: convex-hull
    prompt: >-
      What does Minkowski's theorem guarantee about any finite set of points?
    explanation: >-
      “It is in fact possible to enclose any set of points in an LP feasible region such that each corner
      of the region is a point in the set. In two dimensions you can think of stretching out a rubber band
      and letting it snap back … In three dimensions imagine shrink-wrapping the points” (p. 121). That
      region is the **convex hull**, a convex polytope. Optimizing a linear function over the hull gives an
      optimal point of the original set.
    options:
      - text: "That every polytope is a cube"
      - text: "That the TSP is in P"
      - text: "That points can be sorted in linear time"
      - text: "Every finite point set's convex hull is an LP region (a polytope) whose corners are points of the set"
        correct: true
  - id: tsp-polytope
    prompt: >-
      “Any TSP can be modeled precisely as a linear-programming problem!” Why doesn't this solve the TSP?
    explanation: >-
      Tours are 0/1 points in n(n − 1)/2 dimensions, and their convex hull, the **TSP polytope**, is an LP
      region whose corners are exactly the tours, so the LP would return an optimal tour (p. 121). The catch
      is that “the number of halfspaces one needs to describe a TSP polytope is enormous. By the time we get
      up to ten cities it is known that at least 51,043,900,866 inequalities are required” (p. 121), and
      nobody knows a full description for general n. The model exists. We cannot write it down or separate
      over it efficiently.
    options:
      - text: "Because tours are not points"
      - text: "Because LP cannot handle 0/1 points"
      - text: "Because the polytope needs an astronomical, unknown set of inequalities (≥ 51 billion for 10 cities)"
        correct: true
      - text: "Because Minkowski's theorem is false in high dimensions"
  - id: kuhn-insight
    prompt: >-
      Harold Kuhn recalled that he and Dantzig were “keenly aware” of a way around the enormous
      constraint set. What was it?
    explanation: >-
      “If you could find an optimal solution to a relaxed problem with a subset of the faces that is a
      tour, then you had solved the underlying Traveling Salesman Problem” (p. 122). You do not need every
      facet, only enough constraints that the relaxed LP optimum happens to be a tour. Since every tour is
      feasible for the relaxation, that tour must be optimal. This is the logic of cutting planes in
      chapter 6.
    options:
      - text: "Use only the first 1,000 constraints"
      - text: "If an LP with only some of the constraints returns a tour, that tour is optimal"
        correct: true
      - text: "Randomly sample constraints"
      - text: "Solve the dual only"
---
## Learning objectives

- Visualize LPs: halfspaces, feasible regions, corners.
- Understand convex hulls and the TSP polytope.
- See why “enough constraints to get a tour” suffices.

## Geometry of LP

Each inequality is a **halfspace**. The feasible region is their intersection. A linear objective is optimized at a **corner**: maximizing x + 2y over the five-constraint example gives (5, 8) (pp. 118–119). Simplex walks along edges from corner to corner (p. 119).

## Convex hulls

**Minkowski:** any finite set of points is the set of corners of its convex hull, which is a polytope described by finitely many inequalities (pp. 120–121). So in principle **the TSP is an LP** over the TSP polytope, whose corners are the tours (p. 121).

In practice even 10 cities need at least **51,043,900,866** facets (p. 121), and no complete description is known for general n.

## The way out

You need only enough inequalities that the LP optimum is a tour. Then it is optimal (Kuhn, p. 122). Chapter 6 shows how to find those inequalities on demand.

## In software terms

Convex hulls are a standard tool in geometry libraries (collision detection, GIS). The same idea in very high dimensions lets an LP solver stand in for a combinatorial search, provided you can produce the right constraints when they are needed.

## Sources

- Cook (2012), pp. 118–122.
- Ziegler, G. M. *Lectures on Polytopes*. Springer, 1995.
- Christof, T., G. Reinelt. “Decomposition and parallelization techniques for enumerating the facets of combinatorial polytopes.” *Int. J. Comput. Geom. Appl.* 11 (2001), 423–437.
