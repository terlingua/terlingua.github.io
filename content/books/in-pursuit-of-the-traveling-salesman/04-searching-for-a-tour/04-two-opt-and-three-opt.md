---
title: "Alterations While You Wait: 2-opt and 3-opt (pp. 75–79)"
questions:
  - id: discover-bug
    prompt: >-
      Martin Gardner's 1985 Discover article showed a USA tour with obvious shortcuts. What caused
      the error?
    explanation: >-
      The tour came from Dantzig et al., “the problem did not lie with the tour, but rather with an
      overzealous editor who went ahead and shifted the locations of the cities over to the 48 state
      capitals” (p. 75). An optimal order for one set of points is not optimal for a different set.
      It is a data and model mismatch, like running yesterday’s cached plan against today’s data.
      Shen Lin then produced a 48-capitals tour of 10,628 miles, backed it with a $100 bet, and it was
      in fact optimal (pp. 76–77).
    options:
      - text: "Dantzig's tour was wrong"
      - text: "The tour was drawn on different cities (state capitals) than it was computed for"
        correct: true
      - text: "The printer reversed the map"
      - text: "Gardner used Manhattan distances"
  - id: two-opt-move
    prompt: >-
      What is a 2-opt move?
    explanation: >-
      “Two edges in a tour are deleted and the tour is reconnected with two shorter edges” (p. 78).
      Removing edges (a,b) and (c,d) leaves two paths, and there is exactly one other way to rejoin
      them into a tour: (a,c) and (b,d), which reverses the segment between them. Apply the move if
      d(a,c) + d(b,d) < d(a,b) + d(c,d). A 3-opt move deletes three edges and has several
      reconnection options. Cook uses one to remove the Tennessee “spike” in the Discover tour
      (p. 77).
    options:
      - text: "Run nearest neighbor twice"
      - text: "Swap the positions of two cities"
      - text: "Delete two tour edges and reconnect the two paths the other way (reversing a segment) if that is shorter"
        correct: true
      - text: "Add two cities to the tour"
  - id: no-crossing
    prompt: >-
      “Perhaps the oldest theorem concerning the TSP is the fact that for Euclidean instances … an
      optimal tour will never cross itself.” How is it proved?
    explanation: >-
      “The way to prove this is with a 2-opt move: replacing a crossing pair of edges will always
      shorten a tour” (p. 78). If segments AB and CD cross at X, the triangle inequality on triangles
      AXC and BXD gives |AC| + |BD| < |AX| + |XC| + |BX| + |XD| = |AB| + |CD|. Uncrossing always
      helps, so an optimal tour has no crossings. This is also a fast visual bug check for any
      Euclidean route renderer: a crossing means the route is not optimal.
    options:
      - text: "By linear programming duality"
      - text: "It is false; optimal tours can cross"
      - text: "By enumeration of all tours"
      - text: "Uncrossing two crossing edges with a 2-opt move strictly shortens the tour (triangle inequality)"
        correct: true
  - id: two-opt-result
    prompt: >-
      Starting from the 1,013-unit nearest-neighbor tour, repeated improving 2-opt moves stop at 758.
      What does this illustrate?
    explanation: >-
      After 27 improving moves, “there exist no further improving moves with just two edges, but
      this simple process has brought our faulty nearest-neighbor tour to within 8% of the optimal
      route” (p. 78). The result is a **local optimum**: no single 2-opt move helps, yet it is not
      globally optimal (699). Cheap local improvement fixes most of a poor construction, and
      escaping local optima needs larger moves (Lin–Kernighan) or restarts and kicks.
    options:
      - text: "Local search quickly removes most of the error but stops at a 2-opt local optimum (here 8% above optimal)"
        correct: true
      - text: "2-opt makes tours longer"
      - text: "2-opt only works on random tours"
      - text: "2-opt finds the optimum from any start"
  - id: two-opt-worst
    prompt: >-
      What worst-case guarantee is known for 2-opt local optima on metric instances?
    explanation: >-
      “Repeatedly making improving 2-opt moves is only guaranteed to produce a solution no worse than
      4√n times longer than an optimal tour on instances satisfying the triangle inequality” (p. 82),
      from Chandra, Karloff and Tovey. That bound is weak compared with Christofides’ 1.5, yet in
      practice 2-opt typically lands around 5% above optimal. This is “the dark side” of local search:
      excellent typical results with poor worst-case proofs.
    options:
      - text: "4√n×, weak in theory though good in practice"
        correct: true
      - text: "Exactly optimal"
      - text: "1.5×"
      - text: "2×"
  - id: segment-reversal-cost
    prompt: >-
      In an array-based tour, applying a 2-opt move reverses the segment between the two removed
      edges. What implementation concern arises for million-city tours?
    explanation: >-
      Reversal costs O(n) per move if done naively, which dominates at scale. Practical codes reverse
      the *shorter* side (a tour reads the same in either direction, p. 3), and use structures such
      as doubly linked lists with orientation bits or two-level lists to make reversals cheaper.
      They also restrict candidate edges to a few nearest neighbors per city so the search for
      moves stays near-linear. The pancake-flipping link in the next section is about counting
      these reversals. Algorithmic ideas only scale with the right data structures.
    options:
      - text: "Use recursion to reverse"
      - text: "No concern; reversal is O(1) in arrays"
      - text: "Reversal can cost O(n); reverse the shorter side, use better tour data structures, and limit candidates with neighbor lists"
        correct: true
      - text: "2-opt cannot be implemented on arrays"
---
## Learning objectives

- Implement 2-opt and understand 3-opt.
- Prove that optimal Euclidean tours do not cross.
- Recognize local optima and the weak worst-case behavior of local search.

## Tour improvement

**Tour-improvement methods** take a tour, look for flaws and fix them (p. 77). Shen Lin was the master of this, and his Discover tour through the 48 capitals (10,628 miles) was optimal (pp. 76–77).

- **2-opt:** remove two edges and reconnect the other way, which reverses a segment (p. 78).
- **3-opt:** remove three edges and choose the best reconnection (p. 77).
- **Uncrossing:** in Euclidean instances, crossing edges can always be uncrossed to shorten the tour, so optimal tours never cross (p. 78).

On the USA instance: NN 1,013 → 27 two-opt moves → **758** (8% above 699), a 2-opt local optimum (p. 78). Worst case for 2-opt local optima: 4√n × OPT (p. 82).

```csharp
static void TwoOpt(int[] tour, double[,] d)
{
    var n = tour.Length;
    var improved = true;
    while (improved)
    {
        improved = false;
        for (var i = 0; i < n - 1; i++)
            for (var j = i + 2; j < n; j++)
            {
                int a = tour[i], b = tour[i + 1], c = tour[j], e = tour[(j + 1) % n];
                if (a == e) continue;
                var delta = d[a, c] + d[b, e] - d[a, b] - d[c, e];
                if (delta < -1e-9)
                {
                    Array.Reverse(tour, i + 1, j - i);
                    improved = true;
                }
            }
    }
}
```

## In software terms

- **Local search is everywhere:** autoscaler tuning, query plan rewrites, bin packing in schedulers. Expect fast gains and then a plateau at a local optimum.
- **Scale tricks:** neighbor lists (consider only the 5–10 nearest cities), “don’t look” bits, and orientation-aware tour structures.
- **Visual test:** for Euclidean routes, any crossing means the route is not optimal.

## Sources

- Cook (2012), pp. 75–79, 82.
- Croes, G. A. “A method for solving traveling-salesman problems.” *Operations Research* 6 (1958), 791–812.
- Lin, S. “Computer solutions of the traveling salesman problem.” *Bell System Technical Journal* 44 (1965), 2245–2269.
- Chandra, B., H. Karloff, C. Tovey. “New results on the old k-opt algorithm for the TSP.” *Proc. 5th SODA* (1994), 150–159.
