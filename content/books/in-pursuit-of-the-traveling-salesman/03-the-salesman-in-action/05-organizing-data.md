---
title: "Organizing Data (pp. 56–58)"
questions:
  - id: max-similarity-path
    prompt: >-
      To cluster data with a TSP using *similarity* values (high = alike), what kind of path do you
      look for?
    explanation: >-
      “Using the similarity values as travel costs, a Hamiltonian path of maximum cost places
      similar points near to one another (since closely related points have high similarity
      measures), and thus segments in the path can be used as candidates for clusters” (p. 56).
      With similarities you maximize. Equivalently, convert to distances, for example
      d = max − s, and minimize. Splitting the path into clusters was usually done by hand at
      natural breakpoints.
    options:
      - text: "A random path"
      - text: "A path visiting only the most similar pair"
      - text: "A minimum-cost path on the similarity values"
      - text: "A maximum-cost Hamiltonian path on similarity values, or equivalently a minimum path on distances"
        correct: true
  - id: tsp-plus-k
    prompt: >-
      Climer and Zhang's “TSP+k” method adds k + 1 dummy cities at zero cost to everything. Why does
      this produce k clusters automatically?
    explanation: >-
      “The additional cities serve as breakpoints to identify k clusters, since a good tour will use
      the zero-cost connections to dummy cities to replace large travel costs between clusters of
      points” (p. 56). One dummy turns the tour into a path, and the other k cut the path at the
      k most expensive jumps. The optimizer decides where the cluster boundaries go. They applied it
      to 499 Arabidopsis genes under five conditions with Concorde, varying k (pp. 56–57).
    options:
      - text: "An optimal tour routes through free dummies in place of the costly jumps between clusters, which cuts the order into segments"
        correct: true
      - text: "Dummies make the problem symmetric"
      - text: "Dummies reduce the number of cities"
      - text: "The dummies are placed at cluster centers"
  - id: musicrainbow
    prompt: >-
      MusicRainbow arranges 558 artists on a circle using audio similarity. Why is a *tour* (not a
      path) natural here?
    explanation: >-
      “The TSP was then used to arrange the artists in a circular order, such that similar artists
      are near to one another … the music collection can be navigated by turning a knob” (p. 57).
      A rotating knob has no start or end, so the cyclic tour fits the interface. Path versus tour
      should follow the interaction design.
    options:
      - text: "Because artists must return home"
      - text: "Because the interface is a knob, a cycle with no endpoints, so a closed tour matches"
        correct: true
      - text: "Because paths are NP-hard but tours are not"
      - text: "Because music is periodic"
  - id: chord-costs
    prompt: >-
      Drew Krause used Concorde for composition. What were the cities and costs?
    explanation: >-
      “The cities are a collection of chords and the travel costs are defined as the sum of the
      half-step distances between the corresponding notes” (p. 58). Minimizing total cost gives
      smooth (conjunct) transitions. Any domain with a pairwise transition penalty can be ordered
      this way.
    options:
      - text: "Chords, with half-step distances between corresponding notes as costs"
        correct: true
      - text: "Songs and their durations"
      - text: "Instruments and their weights"
      - text: "Musicians and concert venues"
  - id: texture-layout
    prompt: >-
      Digital Extremes laid out game textures on DVD with Concorde. Which cost definition did they
      use, and what did it minimize?
    explanation: >-
      Sequential reads are fast and random seeks are slow. A *break* occurs when a set of textures
      needed for a scene is stored in more than one place; a set split into k intervals contributes
      k − 1 breaks. “The cities are the textures and the cost of travel between two textures is the
      number of sets that contain one of the textures but not the other” (p. 58). The answer is a
      path, again via an extra city. The same objective fits package or asset bundling, column
      ordering, and placing hot keys together in an SSTable.
    options:
      - text: "Color difference; minimizes visual artifacts"
      - text: "File size difference; minimizes disk usage"
      - text: "Number of scene sets containing exactly one of the two textures; minimizes breaks (extra seeks)"
        correct: true
      - text: "Alphabetical distance; minimizes lookup time"
  - id: trace-ordering
    prompt: >-
      A data team wants to order 5,000 microservices on a dashboard so that services that are often
      called together sit next to each other. Which approach follows this section most directly?
    explanation: >-
      Define cost(i, j) as the number of traces that contain exactly one of i and j (a break
      count), or 1 − similarity. Add a dummy city and solve or approximate a TSP path, which places
      co-occurring services together. To get groups, add k + 1 dummies as in TSP+k and let the
      optimizer choose the cluster boundaries (pp. 56–58).
    options:
      - text: "Shuffle randomly each day"
      - text: "Sort by error rate"
      - text: "Sort services by name"
      - text: "Build a co-occurrence cost matrix and find a short Hamiltonian path (optionally TSP+k for groups)"
        correct: true
---
## Learning objectives

- Use the TSP as a seriation and clustering tool.
- Build cost matrices from similarity or co-occurrence.
- Apply dummy-city tricks to get paths and clusters.

## Ideas

- **Seriation.** With similarity costs, a maximum-cost Hamiltonian path puts similar items next to each other. Cut it into clusters (p. 56).
- **TSP+k.** Add k + 1 zero-cost dummies and the optimal tour cuts itself into k clusters (Climer and Zhang, gene expression) (p. 56).
- **Circular orderings.** MusicRainbow (558 artists) and Traveller’s Sound Player (3,000+ tracks) for knob- or wheel-based browsing (p. 57).
- **Composition.** Chords as cities and half-step distance as cost (p. 58).
- **Storage layout.** Textures on DVD, with cost = number of sets containing exactly one of the pair, which minimizes breaks (p. 58).

```csharp
static int[,] BreakCost(IReadOnlyList<HashSet<int>> sets, int items)
{
    var cost = new int[items, items];
    foreach (var s in sets)
        for (var i = 0; i < items; i++)
            for (var j = 0; j < items; j++)
                if (s.Contains(i) != s.Contains(j))
                    cost[i, j]++;
    return cost;
}
```

## Sources

- Cook (2012), pp. 56–58.
- Climer, S., W. Zhang. “Rearrangement clustering: pitfalls, remedies, and applications.” *J. Machine Learning Research* 7 (2006), 919–943.
- Lenstra, J. K. “Clustering a data array and the traveling-salesman problem.” *Operations Research* 22 (1974), 413–414.
- Pampalk, E., M. Goto. “MusicRainbow: a new user interface to discover artists using audio-based similarity and web-based labeling.” *ISMIR* (2006).
