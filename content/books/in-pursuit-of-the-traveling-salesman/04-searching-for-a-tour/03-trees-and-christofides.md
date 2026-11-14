---
title: "Trees and Christofides' Algorithm (pp. 70–75)"
questions:
  - id: mst-greedy-works
    prompt: >-
      Greedy fails for tours but succeeds for the minimum-cost connection problem. What does that
      greedy algorithm (Kruskal's) do?
    explanation: >-
      “The greedy algorithm in this case, working in a shortest-first order, includes an edge in the
      solution only if it is not possible to travel from one of its ends to the other using
      previously selected edges … It is remarkable, and not too difficult to prove, that this simple
      method always produces a spanning tree of minimum cost” (p. 71). A cheapest connecting network
      never contains a cycle, since the last edge of a cycle is redundant, so it is a **tree**:
      connected and acyclic. The USA minimum spanning tree costs 591 against the 699 tour (p. 70).
      Greedy is optimal for some structures (matroids, such as spanning trees) and poor for others
      (tours).
    options:
      - text: "Adds edges shortest-first, skipping any edge whose ends are already connected; the result is always a minimum spanning tree"
        correct: true
      - text: "Grows one path from a root"
      - text: "Computes all trees and picks the cheapest"
      - text: "Adds the longest edges first until connected"
  - id: mst-lower-bound
    prompt: >-
      Why is the cost of a minimum spanning tree (MST) a lower bound on the optimal tour cost?
    explanation: >-
      Delete any one edge from an optimal tour and you get a Hamiltonian path, which is a spanning
      tree. So OPT ≥ cost(path) ≥ cost(MST). Cook: “the cost of an optimal tree cannot be more than
      the cost of an optimal tour” (p. 72). This is the simplest lower bound and the seed of Held and
      Karp’s much stronger 1-tree bounds (chapter 8). On the USA instance it is 591 against 699, a
      weak 15% gap that LP methods later close.
    options:
      - text: "Removing one edge from any tour leaves a spanning tree, so OPT ≥ MST"
        correct: true
      - text: "Because the MST is always a tour"
      - text: "It is not a lower bound"
      - text: "Because trees have more edges than tours"
  - id: double-tree
    prompt: >-
      The tree-doubling heuristic walks the MST depth-first and shortcuts repeated cities. Why is
      the result at most 2× optimal?
    explanation: >-
      A depth-first traversal uses each tree edge exactly twice, so its cost is 2·MST. Shortcutting
      over already-visited cities never increases cost *by the triangle inequality*. So
      tour ≤ 2·MST ≤ 2·OPT (pp. 71–72). On the USA instance it gives 823 (p. 72). Without the
      triangle inequality the shortcuts could be expensive and the guarantee disappears.
    options:
      - text: "Because every tree has n − 1 edges"
      - text: "Because the tour visits each city twice"
      - text: "Because DFS is optimal"
      - text: "Traversal costs 2·MST, shortcuts don't add cost under the triangle inequality, and MST ≤ OPT"
        correct: true
  - id: odd-vertices-even
    prompt: >-
      Christofides adds a minimum perfect matching on the *odd-degree* vertices of the MST. Why is
      the number of odd-degree vertices always even?
    explanation: >-
      Summing all degrees counts each edge twice, so the total is even. The odd degrees must
      therefore come in pairs (the handshake lemma), which guarantees a perfect matching on them
      exists. The USA tree has 26 odd vertices matched by 13 edges (p. 72). Adding the matching
      makes every degree even, so by Euler’s rule (chapter 2) an Eulerian walk exists and can be
      shortcut into a tour.
    options:
      - text: "Because trees are bipartite"
      - text: "The sum of degrees is twice the number of edges, so odd degrees come in pairs"
        correct: true
      - text: "Because Kruskal adds edges in pairs"
      - text: "It is not always even"
  - id: matching-half
    prompt: >-
      Why is the minimum perfect matching on the odd vertices at most half the optimal tour?
    explanation: >-
      Walk the optimal tour and shortcut past even vertices to get a circuit through only the odd
      vertices, which costs ≤ OPT by the triangle inequality. That circuit has an even number of
      edges and splits into two perfect matchings, alternating edges. “One of these two matchings
      must have cost no greater than half the cost of the tour, and Edmonds’s optimal matching can
      only be cheaper still” (p. 73). So MST + matching ≤ OPT + OPT/2 = 1.5·OPT.
    options:
      - text: "Because Edmonds proved matchings are free"
      - text: "Because matchings have n/2 edges"
      - text: "Shortcut the optimal tour to the odd vertices (≤ OPT), split it into two alternating matchings; one is ≤ OPT/2"
        correct: true
      - text: "Because the MST is half the tour"
  - id: christofides-steps
    prompt: >-
      Put Christofides' algorithm in order.
    explanation: >-
      (1) Compute a minimum spanning tree. (2) Find its odd-degree vertices. (3) Add a minimum-cost
      perfect matching on them, computable in polynomial time by Edmonds’s algorithm. (4) Every
      degree is now even, so take an Eulerian walk. (5) Shortcut repeated vertices to get a tour.
      The guarantee is ≤ 1.5·OPT on metric instances, and the USA result is 759 (pp. 72–75).
      “No polynomial-time algorithm is known to have a better worst-case guarantee than
      Christofides’ method” (p. 75). That held at publication; chapter 13 covers the 2020
      improvement.
    options:
      - text: "Nearest neighbor → 2-opt → matching"
      - text: "Euler walk → MST → matching → shortcut"
      - text: "Matching → MST → shortcut → Euler walk"
      - text: "MST → odd vertices → min perfect matching → Eulerian walk → shortcut"
        correct: true
  - id: approx-ratio-meaning
    prompt: >-
      A vendor claims their router is “a 1.5-approximation for metric instances.” What exactly is
      promised?
    explanation: >-
      An α-approximation algorithm runs in polynomial time and always returns a tour of cost
      ≤ α·OPT on every instance of the stated class, here costs that are symmetric and satisfy the
      triangle inequality (p. 183). It promises nothing for non-metric data such as asymmetric
      one-way costs or penalty-padded matrices. It does not promise typical quality either:
      Christofides “in practice … typically produces even better solutions” (p. 73), often around
      10% above optimal, while local search does better still.
    options:
      - text: "Tours within 1.5% of optimal on any input"
      - text: "On every metric instance, tour cost ≤ 1.5 × optimal, in polynomial time; nothing guaranteed for non-metric inputs"
        correct: true
      - text: "Average tour is 1.5× optimal"
      - text: "Runs in 1.5 seconds"
  - id: cayley-formula
    prompt: >-
      The Good Will Hunting blackboard shows Cayley's formula for the number of labeled trees on n
      vertices. What is it, and what does it say about enumerating trees?
    explanation: >-
      The formula is nⁿ⁻² (p. 70 mentions it without stating it). That is exponentially many trees,
      even more than the (n − 1)!/2 tours for large n, yet Kruskal finds the cheapest in near-linear
      time after sorting. A huge search space does not imply hardness. The preface makes the same
      point: there are easy problems with “far more candidate solutions than the number of ways to
      route a salesman” (p. xi).
    options:
      - text: "n!, the same as tours"
      - text: "n², so trees are easy to enumerate"
      - text: "nⁿ⁻², exponentially many — yet the minimum one is found fast, so a large search space alone does not mean hardness"
        correct: true
      - text: "2ⁿ, the same as subsets"
---
## Learning objectives

- Compute an MST with Kruskal and use it as a lower bound.
- Derive the 2× double-tree and 1.5× Christofides guarantees.
- Know exactly what an approximation ratio promises.

## Trees

A **tree** is connected with no cycles (Cayley). The cheapest network connecting all cities is a minimum spanning tree, and Kruskal’s shortest-first greedy finds it (pp. 70–71). On the USA instance the MST is 591 and the tour 699.

**MST ≤ OPT**, because a tour minus one edge is a spanning tree.

## Double tree, 2×

A depth-first walk around the tree costs 2·MST. Shortcutting is free under the triangle inequality. Result ≤ 2·OPT. USA: 823 (pp. 71–72).

## Christofides, 1.5×

1. MST.
2. Odd-degree vertices. There is always an even number of them.
3. Minimum-cost perfect matching on them (Edmonds, polynomial time).
4. All degrees are now even, so there is an Eulerian walk (Euler!).
5. Shortcut it to a tour.

**Proof sketch:** MST ≤ OPT, and matching ≤ OPT/2 by splitting the shortcut optimal tour into two matchings. Total ≤ 1.5·OPT. USA: 759 (pp. 72–75). In 1976, and still in 2011, nobody had a better polynomial-time guarantee (p. 75).

```csharp
static List<(int U, int V)> Kruskal(int n, IEnumerable<(int U, int V, double W)> edges)
{
    var parent = Enumerable.Range(0, n).ToArray();
    int Find(int x) => parent[x] == x ? x : parent[x] = Find(parent[x]);
    var tree = new List<(int, int)>();
    foreach (var (u, v, _) in edges.OrderBy(e => e.W))
    {
        int ru = Find(u), rv = Find(v);
        if (ru == rv) continue;
        parent[ru] = rv;
        tree.Add((u, v));
        if (tree.Count == n - 1) break;
    }
    return tree;
}
```

## In software terms

- Kruskal plus union-find is the same machinery used for network design, clustering (single-linkage) and dependency de-duplication.
- An approximation ratio is a contract about worst-case quality on a stated input class. Check whether your inputs are in that class: non-metric costs void it.

## Sources

- Cook (2012), pp. 70–75.
- Kruskal, J. B. “On the shortest spanning subtree of a graph and the traveling salesman problem.” *Proc. AMS* 7 (1956), 48–50.
- Christofides, N. “Worst-case analysis of a new heuristic for the travelling salesman problem.” Report 388, GSIA, Carnegie Mellon University, 1976.
- Edmonds, J. “Paths, trees, and flowers.” *Canadian J. Math.* 17 (1965), 449–467.
