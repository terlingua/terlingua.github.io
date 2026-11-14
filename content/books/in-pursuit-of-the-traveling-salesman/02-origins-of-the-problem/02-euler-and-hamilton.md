---
title: "Euler and Hamilton (pp. 27–35)"
questions:
  - id: graph-abstraction
    prompt: >-
      Euler's key move on the Königsberg bridges was to abstract the town into a *graph*. What
      does a graph keep, and what does it throw away?
    explanation: >-
      “The interpretation of the drawing is not influenced by the shape or length of the points
      and lines, but only by which pairs of points are joined. An object such as this is called a
      graph. The points of the graph are called vertices, the lines are called edges, and each
      edge has as its ends two of the vertices” (p. 28). Land masses become vertices and
      bridges become edges. Size, shape and distance are dropped and only adjacency is kept. A
      service-dependency map, a network topology and a state machine are graphs in exactly this
      sense.
    options:
      - text: "It keeps the river's flow direction"
      - text: "It keeps coordinates of each bridge"
      - text: "It keeps distances and areas, and drops names"
      - text: "It keeps only which pairs of points are joined (vertices and edges), and drops shape, size and length"
        correct: true
  - id: parity-rule
    prompt: >-
      In any walk that uses edges, what is true of the number of edge-ends at each vertex, and why
      did that doom the Königsberg challenge?
    explanation: >-
      Each time a walk passes through a vertex it uses one edge to arrive and one to leave, which
      adds two. So “in any walk between two distinct points, the starting and ending vertices meet
      an odd number of edges and all other vertices meet an even number … if we have a closed
      walk … every vertex meets an even number of edges” (p. 29). The bridge graph has degrees
      5, 3, 3, 3, all odd: “All four vertices of their bridge graph meet an odd number of edges,
      thus there can be no walk using each edge exactly once” (p. 29). A walk that uses every
      edge once needs zero or two odd vertices.
    options:
      - text: "Interior vertices of a walk use edges in pairs, so a walk using every edge once needs zero or two odd-degree vertices; Königsberg had four"
        correct: true
      - text: "The number of edges must be even; Königsberg had seven"
      - text: "The graph must be planar; Königsberg was not"
      - text: "Every vertex must have degree at least 3; Königsberg had a vertex of degree 2"
  - id: euler-vs-hamilton
    prompt: >-
      Which statement correctly contrasts an *Eulerian* walk with a *Hamiltonian* circuit?
    explanation: >-
      “A Hamiltonian circuit in a graph is a closed walk that visits each vertex exactly once,
      while a Eulerian walk is a closed walk that travels along each edge exactly once … there
      is a world of difference between the two” (p. 33). For Euler there is a simple rule:
      the graph must be connected, apart from isolated vertices, and every vertex must have even
      degree (p. 34). That check runs in linear time. “Deciding whether or not a graph has a
      Hamiltonian circuit is an NP-complete problem, capturing much of the complexity of the
      general TSP” (p. 33). “So we understand Euler, but not Hamilton” (p. 34).
    options:
      - text: "Both are NP-complete"
      - text: "Eulerian: each edge once, decidable by a simple degree/connectivity rule. Hamiltonian: each vertex once, NP-complete"
        correct: true
      - text: "Eulerian: each vertex once; Hamiltonian: each edge once"
      - text: "Both are decidable by checking that degrees are even"
  - id: devops-coverage
    prompt: >-
      A QA team models an app's screens as vertices and navigation actions as edges. Which goal
      leads to the *easy* problem?
    explanation: >-
      Exercising every *transition* once in a single closed walk is an Eulerian question:
      check connectivity and even degrees (in directed graphs, in-degree equals out-degree). It is
      solvable fast, and when it fails you can add duplicate edges cheaply (the Chinese postman
      problem). Visiting every *screen* exactly once is the Hamiltonian-circuit problem, which is
      NP-complete (p. 33). Whether you cover edges or vertices decides whether the problem is
      easy or intractable, so check which one you actually need before building a tool.
    options:
      - text: "Neither can be decided"
      - text: "Visit every screen exactly once and return home"
      - text: "Traverse every navigation action exactly once in one closed walk"
        correct: true
      - text: "Both are equally hard"
  - id: knights-tour
    prompt: >-
      Euler's knight's tour is an early example of which problem?
    explanation: >-
      Make a vertex per square and an edge between squares a knight move apart. “A knight’s tour
      is a closed walk that visits each vertex exactly once. (Note the similarity with the
      Königsberg problem, where we sought a walk traversing each edge exactly once.)” (p. 30).
      That is a Hamiltonian circuit in the knight’s graph, a TSP in which every allowed move
      costs the same and disallowed moves are forbidden.
    options:
      - text: "A shortest path"
      - text: "An Eulerian walk"
      - text: "A Hamiltonian circuit in the knight-move graph"
        correct: true
      - text: "A minimum spanning tree"
  - id: icosian
    prompt: >-
      Hamilton marketed the Icosian Game (tours of the dodecahedron's 20 corners). Why did it
      flop?
    explanation: >-
      “If you try a few rounds of play you will see quickly the problem: finding tours in the
      Icosian graph is too easy” (p. 32). Hamilton had proved, with an algebra of symbols i, j, k
      satisfying i² = 1, j³ = 1, k⁵ = 1, k = ij, that any five-vertex starting path can be
      extended to a tour (pp. 31–32). A problem class can be NP-complete while particular
      instances, like a small symmetric graph, are trivial. Hardness belongs to the worst
      case, not to every instance.
    options:
      - text: "No tour exists on a dodecahedron"
      - text: "It required quaternions to play"
      - text: "The board was too expensive"
      - text: "Finding Hamiltonian circuits on that small, highly structured graph is too easy"
        correct: true
  - id: tait-tutte
    prompt: >-
      P. G. Tait conjectured that every 3-regular, 3-connected planar map has a Hamiltonian
      circuit, which would give a four-coloring. What happened?
    explanation: >-
      A Hamiltonian circuit through a map’s border graph splits the plane into inside and outside.
      Each side can be 2-colored by alternating across non-circuit edges, giving four colors in
      total (p. 34). Tait expected circuits in all 3-regular, 3-connected maps, but “William Tutte,
      the great graph theorist and Bletchley Park code breaker, eventually showed Tait’s conjecture
      to be false in 1946” (p. 35). Kempe’s own four-color “proof” had been refuted by Heawood in
      1890. Plausible conditions for Hamiltonicity keep failing, which is a hint of the
      problem’s difficulty.
    options:
      - text: "It remains open"
      - text: "Euler disproved it"
      - text: "Tait proved it, settling the four-color theorem"
      - text: "Tutte found a counterexample in 1946; simple sufficient conditions for Hamiltonian circuits keep failing"
        correct: true
  - id: cayley-trees
    prompt: >-
      Arthur Cayley, in Cook's academic family tree, matters to TSP methods because he introduced
      which concept?
    explanation: >-
      Cayley “is a well-known figure in graph theory, introducing the notion of ‘trees’ that we
      cover later in the book” (p. 35). A tree is a connected graph without cycles. Minimum
      spanning trees give both a 2-approximation for metric TSP and, through Held–Karp, strong
      lower bounds (chapters 4 and 8).
    options:
      - text: "Matchings"
      - text: "The simplex algorithm"
      - text: "Trees: connected graphs without cycles"
        correct: true
      - text: "Linear programming"
---
## Learning objectives

- Model a problem as a graph (vertices, edges, degree).
- Prove Euler’s parity rule and apply it.
- Contrast Eulerian walks (easy) with Hamiltonian circuits (NP-complete).

## Graphs from bridges

In 1735 Euler reduced Königsberg (four land regions, seven bridges) to a **graph**: vertices for regions and edges for bridges (pp. 27–29). Only adjacency matters.

**Parity rule.** A walk that passes through a vertex uses edges in pairs, one in and one out. In a walk between two different vertices, the two ends have odd degree within the walk and every other vertex has even degree. In a closed walk every vertex has even degree. All four Königsberg vertices are odd, so no walk can cross each bridge exactly once (p. 29).

```csharp
static bool HasEulerCircuit(int n, IReadOnlyList<(int U, int V)> edges)
{
    var degree = new int[n];
    var adjacency = Enumerable.Range(0, n).Select(_ => new List<int>()).ToArray();
    foreach (var (u, v) in edges)
    {
        degree[u]++; degree[v]++;
        adjacency[u].Add(v); adjacency[v].Add(u);
    }
    if (degree.Any(d => d % 2 != 0)) return false;
    var start = Array.FindIndex(degree, d => d > 0);
    if (start < 0) return true;
    var seen = new bool[n];
    var stack = new Stack<int>([start]);
    seen[start] = true;
    while (stack.TryPop(out var x))
        foreach (var y in adjacency[x].Where(y => !seen[y]))
        {
            seen[y] = true;
            stack.Push(y);
        }
    return Enumerable.Range(0, n).All(i => degree[i] == 0 || seen[i]);
}
```

## Hamilton’s question

The knight’s tour (Euler) and Hamilton’s Icosian game ask for a closed walk through every **vertex** once. That is a **Hamiltonian circuit** (pp. 29–33). The Icosian was commercially too easy, but the general question is NP-complete (p. 33).

| | Eulerian walk | Hamiltonian circuit |
|---|---|---|
| Visits | every edge once | every vertex once |
| Test | connected + all even degrees | no simple rule known |
| Complexity | linear time | NP-complete |

Simple sufficient conditions keep failing. Tait’s conjecture, which would have given a four-color proof, fell to Tutte in 1946 (pp. 34–35).

## In software terms

- **Edge coverage vs node coverage.** Testing every API transition is Eulerian or Chinese-postman territory and is easy. Visiting every state exactly once is Hamiltonian and hard.
- One easy instance (the Icosian) does not make the class easy, and one hard instance does not make yours hard.

## Sources

- Cook (2012), pp. 27–35.
- Euler, L. “Solutio problematis ad geometriam situs pertinentis” (1736/1741). English translation in Biggs, Lloyd, Wilson, *Graph Theory 1736–1936*, Oxford, 1976.
- Tutte, W. T. “On Hamiltonian circuits.” *J. London Math. Soc.* 21 (1946), 98–101.
