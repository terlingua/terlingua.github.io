---
title: "Growing Tours: Nearest Neighbor, Greedy, Insertion (pp. 65–70)"
questions:
  - id: nn-rule
    prompt: >-
      What is the nearest-neighbor heuristic, and what is its characteristic failure?
    explanation: >-
      “Always drive to the closest city among those not yet visited” (p. 65). On the 42-city USA
      instance it starts well, but in the Pacific Northwest “we have no place to go other than to
      travel all the way back to the East Coast to pick up cities carelessly skipped over … This is
      typical of the algorithm, where we paint ourselves into a corner by not looking ahead”
      (p. 65). Result: 1,013 units against the optimal 699. Greedy local choices create expensive
      forced moves at the end, much like a scheduler that grabs the easiest work first and leaves
      the awkward jobs to pile up.
    options:
      - text: "It always goes to the closest unvisited city; it paints itself into corners and pays long edges at the end"
        correct: true
      - text: "It inserts cities into a loop; it crosses itself"
      - text: "It sorts cities by x-coordinate; it ignores y"
      - text: "It picks the farthest city; it is too slow"
  - id: nn-adversarial
    prompt: >-
      Cook raises the Montpelier–Phoenix cost by 1,000,000 and nearest neighbor still picks the same
      tour, now costing 1,001,013 against an optimum of 699. What condition rules out such
      adversarial instances?
    explanation: >-
      Nearest neighbor is “forced to take the last leg of the journey, back to the starting city,
      regardless of its travel cost” (p. 65). Real distances obey the **triangle inequality**: “for
      any three cities A, B, and C, the cost to travel from A to B plus the cost to travel from B to
      C must not be less than the cost to travel from A directly to C” (pp. 65–66). With it, plus
      symmetry, nearest neighbor is never worse than 1 + log₂(n)/2 times optimal (p. 66). Without
      it, no heuristic has any guarantee (chapter 9).
    options:
      - text: "An even number of cities"
      - text: "Integer costs"
      - text: "The triangle inequality: cost(A,B) + cost(B,C) ≥ cost(A,C)"
        correct: true
      - text: "All costs below 1,000"
  - id: nn-bound
    prompt: >-
      Under the triangle inequality, nearest neighbor is at most 1 + log₂(n)/2 times optimal. What
      does this give for 50 cities and for a million cities?
    explanation: >-
      log₂ 50 ≈ 5.6, so 1 + 2.8 ≈ 3.8 (“no longer than four times”). log₂ 10⁶ ≈ 19.9, so about 11
      (“no worse than eleven times optimal”) (p. 66). The guarantee *grows* with n, so it is not a
      constant-factor approximation. In practice nearest neighbor on random points lands around 25%
      above optimal (p. 68). Worst-case guarantees and typical performance can differ a lot, and
      both matter.
    options:
      - text: "About 4× for 50 cities and about 11× for a million"
        correct: true
      - text: "At most 1.5× in both cases"
      - text: "About 50× and 1,000,000×"
      - text: "At most 2× in both cases"
  - id: greedy-edge
    prompt: >-
      How does the TSP “greedy” algorithm differ from nearest neighbor?
    explanation: >-
      Nearest neighbor grows a single path. Greedy “grows many subpaths simultaneously, adding
      shortest available road segments wherever they may be found” (p. 67). It scans edges from
      shortest to longest and accepts an edge only “if it joins two subpaths into a longer subpath”
      (p. 68), meaning no vertex gets degree 3 and no premature cycle forms. It looks great early
      and then accepts a few very long edges at the end: 995 units on the USA instance (p. 68). On
      random geometric instances greedy is typically within 1.15× of optimal against about 1.25×
      for nearest neighbor (p. 68).
    options:
      - text: "Greedy builds a spanning tree and stops"
      - text: "Greedy picks the longest edges first"
      - text: "Greedy adds globally shortest edges that keep a set of growing subpaths, rather than extending one path"
        correct: true
      - text: "Greedy is identical to nearest neighbor"
  - id: empirical-vs-guarantee
    prompt: >-
      Greedy usually beats nearest neighbor in experiments, yet its proven worst-case factor
      (1/2 + log₂(n)/2) is barely better. What should an engineer conclude?
    explanation: >-
      “Unfortunately, this is only an empirical observation. As far as worst-case guarantees go,
      greedy is known only to do no worse than 1/2 + log(n)/2 times optimal” (p. 68). Experiments
      tell you about typical data, and proofs bound the worst case. If your instances resemble the
      test distribution, the empirical result is more useful. If adversarial or odd inputs are
      possible, the guarantee is what protects you. Benchmark on data that looks like production.
    options:
      - text: "Nearest neighbor is better in practice"
      - text: "Guarantees are useless"
      - text: "Experiments describe typical behavior, guarantees describe the worst case; use both and benchmark on representative data"
        correct: true
      - text: "Greedy must be bugged"
  - id: insertion-family
    prompt: >-
      Insertion heuristics grow a subtour “like a rubber band.” Which statements about them are
      correct?
    type: multiple
    explanation: >-
      “Start with a subtour through a small number of cities and stretch it out … to enclose one
      additional city after another” (p. 68). Each new city goes “into the spot that causes the
      smallest increase in the subtour’s length” (p. 69). The rule for choosing the next city varies:
      cheapest, nearest, farthest, or random. Cheapest and nearest insertion are proven to be within
      2× optimal under the triangle inequality (p. 70). Farthest insertion, Cook’s favorite, has only
      a log(n) guarantee but “is generally the best-performing variant in practice” (p. 70): 778 units
      on the USA instance (p. 69). Boldyreff and Robacker’s 1955 version gave 851 against 699, 20% off
      (p. 68).
    options:
      - text: "Each new city is placed where it increases subtour length least"
        correct: true
      - text: "Cheapest and nearest insertion are within 2× optimal under the triangle inequality"
        correct: true
      - text: "Farthest insertion has the best proven guarantee"
      - text: "Farthest insertion often performs best in practice despite a weaker guarantee"
        correct: true
  - id: farthest-intuition
    prompt: >-
      Why does farthest insertion tend to work well?
    explanation: >-
      “It obtains a good overall shape for a tour early on, and then completes the details as the
      last cities are added” (p. 69). Picking the farthest remaining city first sketches the outline,
      reaching New Orleans, Minneapolis and the two Portlands by stage five, so later insertions are
      small local fixes. Settle the architecture first and the details later.
    options:
      - text: "It never adds long edges"
      - text: "It is optimal for convex point sets"
      - text: "It minimizes every individual step"
      - text: "It fixes the global outline early, so later insertions are small local adjustments"
        correct: true
---
## Learning objectives

- Implement and compare nearest neighbor, greedy and insertion heuristics.
- Use the triangle inequality to reason about guarantees.
- Separate empirical quality from worst-case bounds.

## Results on the 42-city USA instance (optimum 699)

| Method | How it grows | USA result | Typical (random) | Worst-case (metric) |
|---|---|---|---|---|
| Nearest neighbor | One path, nearest unvisited next | 1,013 | ≈1.25× | 1 + log₂(n)/2 |
| Greedy | Shortest edges that keep subpaths | 995 | ≈1.15× | 1/2 + log₂(n)/2 |
| Farthest insertion | Rubber band, farthest city next | 778 | good | O(log n) |
| Cheapest/nearest insertion | Rubber band | — | fair | 2× |
| Boldyreff/Robacker (1955) | Insertion | 851 | — | — |

*(pp. 65–70)*

**Triangle inequality.** cost(A,B) + cost(B,C) ≥ cost(A,C). Without it, a single huge forced edge can make any constructive method arbitrarily bad (pp. 65–66). Instances that satisfy it are called *metric*.

```csharp
static int[] NearestNeighbor(double[,] d, int start = 0)
{
    var n = d.GetLength(0);
    var tour = new int[n];
    var used = new bool[n];
    tour[0] = start;
    used[start] = true;
    for (var k = 1; k < n; k++)
    {
        var last = tour[k - 1];
        var best = -1;
        for (var j = 0; j < n; j++)
            if (!used[j] && (best < 0 || d[last, j] < d[last, best]))
                best = j;
        tour[k] = best;
        used[best] = true;
    }
    return tour;
}
```

## In software terms

- Nearest neighbor is O(n²), or close to O(n log n) with spatial indexes. It is a fine *starting* tour for local search (next sections).
- Greedy needs a union-find structure to detect cycles, the same one Kruskal’s algorithm uses.
- Report worst-case guarantees *and* benchmark numbers. They answer different questions.

## Sources

- Cook (2012), pp. 65–70.
- Rosenkrantz, D. J., R. E. Stearns, P. M. Lewis. “An analysis of several heuristics for the traveling salesman problem.” *SIAM J. Computing* 6 (1977), 563–581.
- Johnson, D. S., L. A. McGeoch. “Experimental analysis of heuristics for the STSP.” In Gutin and Punnen (eds.), *The Traveling Salesman Problem and Its Variations*, Kluwer, 2002, 369–443.
