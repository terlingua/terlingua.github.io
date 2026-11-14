---
title: "Borrowing from Physics and Biology (pp. 84–91)"
questions:
  - id: metaheuristic
    prompt: >-
      What is a metaheuristic?
    explanation: >-
      “The idea is to produce metaheuristics, that is, heuristic methods for the design of heuristic
      methods” (p. 84). They treat the TSP as one instance of a general search problem, so the same
      framework (annealing, genetic algorithms, ant colonies) can be applied to scheduling, layout or
      hyperparameter tuning. The TSP is the standard testbed: “If you have a general search mechanism
      in mind, the TSP is a great place to develop, polish, test, and compare your strategy” (p. 91).
    options:
      - text: "A general strategy for designing heuristics, applicable across many search problems"
        correct: true
      - text: "Metadata about tours"
      - text: "A heuristic for metric instances only"
      - text: "A heuristic that is guaranteed optimal"
  - id: landscape-neighborhood
    prompt: >-
      In the “landscape” view of tour finding, what defines which tours are near each other?
    explanation: >-
      “This is typically handled by creating neighborhoods around each tour. For example, two tours
      can be defined to be neighbors if one can be reached from the other via a 2-opt exchange, or via
      an exchange found by Lin-Kernighan. Large neighborhoods are useful for navigating around a
      landscape, but they should be constructed so that algorithms can view and evaluate neighbors”
      (p. 85). The neighborhood *is* the design choice. A bigger one gives fewer local optima but
      costs more per step.
    options:
      - text: "Alphabetical order of city names"
      - text: "Tour length within 1%"
      - text: "Geographic proximity of starting cities"
      - text: "A neighborhood rule — e.g., tours one 2-opt or LK move apart"
        correct: true
  - id: hill-climb-start
    prompt: >-
      Why does the starting tour matter for hill-climbing, and what did Lin and Kernighan propose?
    explanation: >-
      Hill climbing always moves uphill and stops at a peak or plateau. “If the starting tour lands
      midway up a small hill, then the algorithm will be limited to reaching the modest-quality tour
      associated with the hill’s peak. For this reason Lin and Kernighan proposed to carry out repeated
      runs of their algorithm from random starting solutions … throw darts into the landscape” (p. 85).
      Random tours start deep in valleys and take long to climb, so a compromise is
      nearest-neighbor darts with random start cities (p. 86).
    options:
      - text: "Local search ends at the peak of whatever hill it starts on, so use multiple (random or NN) starts"
        correct: true
      - text: "Always start from the worst tour"
      - text: "Start from the optimal tour"
      - text: "It doesn't matter; hill climbing finds the global optimum"
  - id: annealing
    prompt: >-
      How does simulated annealing differ from hill climbing?
    explanation: >-
      “The hill-climbing strategy is relaxed to allow the algorithm to accept with a certain
      probability a neighbor that is worse than the current solution. At the start the probability of
      acceptance is high, but it is gradually decreased as the run progresses” (p. 86). Typically a
      worse move with cost increase Δ is accepted with probability exp(−Δ/T), where the temperature T
      cools over time. It is named after annealing in metallurgy (Kirkpatrick, Gelatt and Vecchi, 1983,
      tested on 400 cities). For the TSP its results “have to date been rather modest,” but as a general
      tool it is “a spectacular success,” with over 18,700 citations (p. 86).
    options:
      - text: "It heats the CPU"
      - text: "It is guaranteed optimal for the TSP"
      - text: "It only accepts improving moves"
      - text: "It sometimes accepts worse moves, with a probability that decreases over time, to escape local optima"
        correct: true
  - id: chained-lk
    prompt: >-
      What is Chained Lin–Kernighan (Martin, Otto and Felten), and why does it work so well?
    explanation: >-
      Rather than restarting from random darts, “kick the Lin-Kernighan solution to obtain a new
      starting tour … replacing the solution whenever we reach a better tour” (pp. 86–87). Two reasons
      it works: sampling nearby peaks is a better strategy, and “the reapplication of Lin-Kernighan to
      a kicked tour runs much more quickly than an application to a random tour … much of a kicked tour
      remains in good shape” (p. 87). This approach is now called iterated local search. Concorde’s
      version gets within 1% of optimal in 1–2 seconds on 100,000 cities (p. 87).
    options:
      - text: "Repeatedly perturbing (kicking) the current LK tour and re-optimizing, keeping improvements"
        correct: true
      - text: "Chaining cities in a fixed order"
      - text: "Combining LK with linear programming"
      - text: "Running LK on many random tours in parallel"
  - id: double-bridge
    prompt: >-
      Why is the double-bridge (a special random 4-opt) a good kick?
    explanation: >-
      “For the method to work, a kick must take the solution out of its neighborhood, so it should be a
      modification that Lin-Kernighan cannot easily undo. Martin et al. found that a random 4-opt
      exchange of the type indicated in figure 4.25 does the job nicely” (p. 87). The double-bridge cuts
      the tour into segments A B C D and reconnects them as A C B D. It is non-sequential, so a
      red–blue chain cannot directly reverse it, and the change is small, so most of the tour stays
      good. A kick should be big enough to escape and small enough to keep most of the work already
      done.
    options:
      - text: "It swaps two adjacent cities"
      - text: "It reverses the entire tour"
      - text: "It is a perturbation LK cannot easily undo, yet it keeps most of the tour intact"
        correct: true
      - text: "It always shortens the tour"
  - id: genetic-eax
    prompt: >-
      Early genetic algorithms for the TSP were weak. What made Nagata's genetic algorithm a world
      champion?
    explanation: >-
      “Merely adopting the language of Darwin does not imply we end up with a good tour” (p. 88). The
      real gains came from good mating operators and local search. Edge-assembly crossover (EAX)
      builds the union of two parents’ edges, picks circuits that alternate between blue and red
      edges, swaps them into one parent, and patches the resulting subtours with 2-opt moves (p. 89).
      Nagata’s “very fast implementation of EAX” allows many generations, and it found the best Mona
      Lisa tour (pp. 89–90).
    options:
      - text: "Simulated annealing inside each child"
      - text: "Using mutation only"
      - text: "A strong crossover (EAX) that recombines parents' edges, implemented fast enough for many generations"
        correct: true
      - text: "Larger populations of random tours"
  - id: aco
    prompt: >-
      What is the book's assessment of ant colony optimization (ACO) for the TSP?
    explanation: >-
      Agents build tours, choosing edges with probability tied to pheromone values. Edges in good
      tours get more pheromone afterward (p. 90). “The approach is both intuitive and appealing, but thus
      far ACO has not proved to be competitive with Lin-Kernighan-based methods,” though it works well
      in scheduling, graph coloring, classification and protein folding (pp. 90–91). A nice analogy
      does not make a method better than a well-engineered specialized one.
    options:
      - text: "It is provably optimal"
      - text: "It was invented by Lin and Kernighan"
      - text: "It is the best TSP method"
      - text: "Intuitive, but not competitive with LK-based methods on the TSP; useful elsewhere"
        correct: true
  - id: chained-scale
    prompt: >-
      Chained LK on a 25,000,000-city random instance reached about 0.3% above optimal. How long did
      it take, and how was “optimal” estimated?
    explanation: >-
      “In eight days, on a computer from the year 2000, a tour that is approximately 0.3% greater than
      optimal was found” (p. 87). Figure 4.26 compares against an *expected optimal value*. For random
      uniform instances that comes from the BHH asymptotics (chapter 2), since no exact solution exists
      at that size. A statistical estimate of the optimum is the only benchmark available at such scale.
    options:
      - text: "Eight days; against an expected optimum from random-instance theory"
        correct: true
      - text: "One year; by brute force"
      - text: "Ten seconds; by LP bound"
      - text: "One hour; by exact solution"
---
## Learning objectives

- Describe tour finding as search over a landscape with neighborhoods.
- Compare multistart, simulated annealing, chained LK, genetic algorithms and ACO.
- Design a good perturbation (kick).

## The landscape

Tours are points on a landscape where better tours sit higher. A **neighborhood** (2-opt, LK) says which tours are adjacent. **Hill climbing** climbs to a local peak, and the starting point decides which peak (pp. 84–86).

| Method | Idea | TSP verdict |
|---|---|---|
| Multistart (darts) | Restart local search from random or NN tours | Simple and decent (pp. 85–86) |
| Simulated annealing | Accept worse moves with decreasing probability | Modest for the TSP, huge general impact (p. 86) |
| **Chained LK** | Kick (double-bridge), re-run LK, keep if better | Dominant on huge instances: ≈1% in seconds on 100k cities, ≈0.3% on 25M (p. 87) |
| Genetic (EAX) | Population and edge-assembly crossover | Nagata: best Mona Lisa tour (pp. 87–90) |
| Ant colony | Pheromone-guided construction | Not competitive with LK on the TSP (p. 90) |
| Others | Tabu, neural nets, honeybees | (p. 91) |

```csharp
static bool Accept(double delta, double temperature, Random rng) =>
    delta <= 0 || rng.NextDouble() < Math.Exp(-delta / temperature);

static int[] DoubleBridge(int[] t, Random rng)
{
    var n = t.Length;
    var cuts = Enumerable.Range(1, n - 1).OrderBy(_ => rng.Next()).Take(3).Order().ToArray();
    int p = cuts[0], q = cuts[1], r = cuts[2];
    return [.. t[..p], .. t[q..r], .. t[p..q], .. t[r..]];
}
```

## In software terms

- **Iterated local search** (kick and re-optimize) is a strong default for scheduling, placement and configuration tuning.
- **Exploration vs exploitation** shows up in annealing temperatures, kick strength and population diversity.
- **Be skeptical of metaphors.** Physics and biology analogies are inspiration. The benchmark decides.

## Sources

- Cook (2012), pp. 84–91.
- Kirkpatrick, S., C. D. Gelatt, M. P. Vecchi. “Optimization by simulated annealing.” *Science* 220 (1983), 671–680.
- Martin, O., S. W. Otto, E. W. Felten. “Large-step Markov chains for the traveling salesman problem.” *Complex Systems* 5 (1991), 299–326.
- Nagata, Y., S. Kobayashi. “A powerful genetic algorithm using edge assembly crossover for the traveling salesman problem.” *INFORMS J. Computing* 25 (2013), 346–363.
- Dorigo, M., L. M. Gambardella. “Ant colony system.” *IEEE Trans. Evolutionary Computation* 1 (1997), 53–66.
