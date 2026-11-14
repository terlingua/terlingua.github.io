---
title: "The Separation Problem (pp. 137–142)"
questions:
  - id: separation-definition
    prompt: >-
      What is the separation problem?
    explanation: >-
      “The task is to find from among the known TSP inequalities one or more that are violated by a
      specified LP solution. This is called the separation problem, since we think of the corresponding
      halfspaces as ‘separating’ the solution from the convex hull of tours. Separation is the heart of the
      cutting-plane method: computer codes such as Concorde are basically wrappers for calling separation
      routines” (p. 137). Given an LP solution x̄, either return a violated cut or report that none exists in the family.
      It is the oracle in the cutting-plane loop.
    options:
      - text: "Separating the primal and dual LPs"
      - text: "Separating tours from heuristics"
      - text: "Splitting cities into two groups"
      - text: "Given an LP solution, find a valid inequality it violates (or report none)"
        correct: true
  - id: islands-easy
    prompt: >-
      What is the easiest subtour separation check?
    explanation: >-
      Build the support graph (edges with xᵢⱼ > 0) and find connected components. Any component S that is
      not the whole graph has zero crossing value, which violates x(δ(S)) ≥ 2, so it is an “island” (pp.
      127–128). This takes linear time, but it misses violated sets in a *connected* support graph, such as a
      cluster joined to the rest by two half-edges, total 1 (p. 129). Catching those needs minimum cuts.
    options:
      - text: "Sort edges by value"
      - text: "Check whether the objective is an integer"
      - text: "Find connected components of the support graph; any component short of everything is a violated subtour"
        correct: true
      - text: "Check edge costs"
  - id: maxflow-mincut
    prompt: >-
      How do maximum flows solve subtour separation exactly?
    explanation: >-
      Treat xᵢⱼ as pipe capacities. The capacity of a cut separating s from t is exactly the left-hand side
      of the subtour inequality for one side of the cut (p. 138). Max-flow = min-cut (strong duality), and
      max-flow algorithms return the minimum cut. Fix a source and compute max flow to each of the n − 1
      other vertices. If every value is ≥ 2, no subtour inequality is violated. Otherwise the minimum cut
      gives a violated set (p. 139). This runs in polynomial time and can be made very fast (p. 139).
    options:
      - text: "Edge values are capacities; any s–t min cut < 2 is a violated subtour set; n − 1 max-flow runs cover all sets"
        correct: true
      - text: "By maximizing tour length"
      - text: "It cannot be solved exactly"
      - text: "By routing the salesman as a flow"
  - id: cut-equals-subtour
    prompt: >-
      On USA42 after seven cuts, four paths each carry 1/2 from Phoenix to Montpelier. What does this flow
      of 2 prove?
    explanation: >-
      “The flow of value two between Phoenix and Montpelier implies there are no violated subtour
      inequalities corresponding to sets containing one of these cities but not the other. Repeating this
      construction for each of the remaining forty choices for the destination vertex demonstrates that
      there are no violated subtour inequalities at all” (pp. 138–139). A flow is a *certificate* that every
      cut separating s from t has capacity ≥ 2, by weak duality for flows.
    options:
      - text: "That the LP solution is a tour"
      - text: "That no set separating Phoenix from Montpelier has a violated subtour inequality"
        correct: true
      - text: "That Phoenix has degree 4"
      - text: "Nothing"
  - id: comb-separation-status
    prompt: >-
      What is the complexity status of separating comb inequalities?
    explanation: >-
      “At the moment there is no known polynomial-time algorithm that is guaranteed to spot a violated comb
      inequality if one exists. The comb-separation problem is also not known to be in the NP-complete
      complexity class, so its status is wide open” (p. 139). In practice codes use “hit-or-miss strategies
      for growing handles, shrinking teeth, splicing sets” (p. 139). Exact methods exist for special cases.
    options:
      - text: "Proven NP-complete"
      - text: "Undecidable"
      - text: "Polynomial, via max-flow"
      - text: "Open: neither a polynomial algorithm nor NP-completeness is known; heuristics are used"
        correct: true
  - id: red-islands
    prompt: >-
      Describe the quick comb heuristic based on “red islands.”
    explanation: >-
      “As a first step we can delete all black edges [xᵢⱼ = 1] and examine the remaining red islands. If the
      borders of any of these are met by an odd number of deleted black edges, then we have a violated comb
      inequality: the island is the handle and the black edges are the teeth” (p. 139). Two-vertex teeth from
      black edges have border value exactly 2, which is ideal for teeth. This is how the three-tooth comb in
      the USA42 run was found, along with a shrinking heuristic for the bigger one (pp. 139–140).
    options:
      - text: "Delete red edges and count black islands"
      - text: "Delete edges with x = 1; a remaining component met by an odd number of deleted edges gives a comb (handle = component, teeth = those edges)"
        correct: true
      - text: "Pick three random sets"
      - text: "Use max-flow on the full graph"
  - id: shrinking
    prompt: >-
      How do practical codes find combs with large teeth?
    explanation: >-
      “The idea is to replace clusters of cities by single vertices and then search for single-edge combs in
      the shrunken graph. Any combs that are found can be expanded back to general combs in the original.
      The art here is to select clusters in a creative way, typically using methods that search for sets
      having borders of value close to two” (pp. 139–140). Contract, solve a simpler problem, then expand
      the result. It is the same coarsening idea used in multilevel graph partitioning.
    options:
      - text: "By solving the dual"
      - text: "By enumerating all subsets"
      - text: "By shrinking well-chosen clusters to single vertices, finding simple combs, then expanding"
        correct: true
      - text: "By randomly adding edges"
  - id: fleischer-tardos-letchford
    prompt: >-
      What did Fleischer–Tardos and Letchford contribute, and what happened in practice?
    explanation: >-
      For LP solutions whose support graph is planar (“non-crossing”), common in geometric instances,
      Fleischer and Tardos gave a polynomial algorithm that finds a comb of *maximum* violation (value 3k
      against the required 3k + 1), though it may miss less-violated combs (pp. 140–142). Letchford designed
      **domino-parity** inequalities, which include combs, with polynomial separation in the planar case
      (p. 142). Boyd, Cockburn and Vella showed it works well, and Cook, Espinoza and Goycoolea automated it.
      It became a Concorde module that “played a big role in the sprint up to the record 85,900-city
      solution” (p. 142).
    options:
      - text: "They proved comb separation NP-complete"
      - text: "Polynomial separation for planar LP supports (max-violated combs; domino-parity), which became a Concorde module key to the 85,900 record"
        correct: true
      - text: "They replaced LP with heuristics"
      - text: "They found the Mona Lisa tour"
---
## Learning objectives

- Define separation and why it drives cutting-plane codes.
- Separate subtours exactly with connectivity and max-flow/min-cut.
- Describe heuristic and exact approaches to comb separation.

## Separation

Given an LP solution x̄, find a valid inequality with x̄ on the wrong side, or report that none exists in the family. Concorde is “basically” a wrapper around separation routines (p. 137).

## Subtours: solved

1. **Components:** if the support graph is disconnected, each component is a violated subtour.
2. **Min cut:** treat xᵢⱼ as capacities. Max-flow = min-cut. Run n − 1 flows from a fixed source. Any value < 2 means its min cut is a violated set (pp. 137–139). (Gomory–Hu trees reduce the work further.)

```csharp
static List<List<int>> Islands(int n, IEnumerable<(int U, int V, double X)> support)
{
    var adj = Enumerable.Range(0, n).Select(_ => new List<int>()).ToArray();
    foreach (var (u, v, x) in support.Where(e => e.X > 1e-9))
    {
        adj[u].Add(v);
        adj[v].Add(u);
    }
    var comp = new int[n];
    Array.Fill(comp, -1);
    var result = new List<List<int>>();
    for (var s = 0; s < n; s++)
    {
        if (comp[s] >= 0) continue;
        var members = new List<int>();
        var stack = new Stack<int>([s]);
        comp[s] = result.Count;
        while (stack.TryPop(out var x))
        {
            members.Add(x);
            foreach (var y in adj[x].Where(y => comp[y] < 0))
            {
                comp[y] = result.Count;
                stack.Push(y);
            }
        }
        result.Add(members);
    }
    return result;
}
```

## Combs: open

No polynomial algorithm is known, and no NP-completeness proof either (p. 139). Practice uses:

- **Red islands:** delete x = 1 edges. A component met by an odd number of them is a handle with single-edge teeth (p. 139).
- **Shrinking:** contract clusters, find simple combs, expand (pp. 139–140).
- **Planar special case:** Fleischer–Tardos finds maximally violated combs. Letchford’s domino-parity separation is a Concorde module behind the 85,900-city record (pp. 140–142).

## In software terms

Most of an exact solver’s speed comes from the oracle: given a candidate, find what is wrong with it fast. Flow algorithms, a library staple, provide exact detection for one important family. The rest is heuristics and careful engineering.

## Sources

- Cook (2012), pp. 137–142.
- Padberg, M., G. Rinaldi. “An efficient algorithm for the minimum capacity cut problem.” *Math. Programming* 47 (1990), 19–36.
- Fleischer, L., É. Tardos. “Separating maximally violated comb inequalities in planar graphs.” *Math. Oper. Res.* 24 (1999), 130–148.
- Letchford, A. N. “Separating a superclass of comb inequalities in planar graphs.” *Math. Oper. Res.* 25 (2000), 443–454.
- Cook, W., D. G. Espinoza, M. Goycoolea. “Computing with domino-parity inequalities for the TSP.” *INFORMS J. Computing* 19 (2007), 356–365.
