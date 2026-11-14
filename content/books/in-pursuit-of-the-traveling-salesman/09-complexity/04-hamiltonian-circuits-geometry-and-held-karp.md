---
title: "State of the TSP: Special Cases, Geometry, Held–Karp (pp. 178–182)"
questions:
  - id: tsp-claims
    prompt: >-
      Of the 25 “Equal” (P = NP) claims on Woeginger's list at publication, how many attack the TSP, and
      how have they fared?
    explanation: >-
      “Nine of the twenty-five ‘Equal’ results in Woeginger’s list are established by delivering good
      algorithms for variants of the TSP … Each of the works appears to have serious flaws, but attacking the
      salesman is indeed an attractive route to proving P = NP” (p. 178). Methods range from enumeration to
      attempted polynomial-size LP formulations. (A later theorem, covered in chapter 13, rules out
      polynomial-size LP formulations of the TSP polytope.)
    options:
      - text: "Twenty-five; all accepted"
      - text: "One; it won the Clay prize"
      - text: "None; nobody attacks the TSP"
      - text: "Nine; all appear to have serious flaws"
        correct: true
  - id: special-cases
    prompt: >-
      Which restricted problems remain NP-complete and are therefore fair targets for a P = NP attempt?
    type: multiple
    explanation: >-
      Hamiltonian circuit in general graphs is NP-complete. It stays NP-complete in **bipartite** graphs and
      even in **grid graphs**, finite subsets of the square grid (Itai, Papadimitriou, Szwarcfiter), the
      “ultimate special case of the TSP” (pp. 178–179). The Euclidean TSP is NP-hard as well (p. 179).
      Eulerian walks are polynomial (chapter 2).
    options:
      - text: "Hamiltonian circuit in bipartite graphs"
        correct: true
      - text: "Hamiltonian circuit in grid graphs"
        correct: true
      - text: "Euclidean TSP (decision version is NP-hard)"
        correct: true
      - text: "Eulerian walk existence"
  - id: euclid-np-open
    prompt: >-
      Why is it unknown whether the Euclidean TSP decision problem is even in NP?
    explanation: >-
      The natural certificate is a tour, but checking “length ≤ K” requires summing square roots of integers.
      “The sum of the square roots may add up to a value very, very close to K. It is not known if we can in
      polynomial time obtain a sufficiently accurate approximation” (pp. 179–180). Graham’s example: two sums
      of nine √(10⁶ + aᵢ) values first differ in the 37th decimal place (p. 180). Floating-point comparisons of
      sums of irrational numbers are not a reliable way to decide exact inequalities.
    options:
      - text: "Because Euclidean distances violate the triangle inequality"
      - text: "Because the coordinates are real numbers"
      - text: "Because tours are too long to write down"
      - text: "Because verifying length ≤ K needs sums of square roots compared to K with unknown required precision"
        correct: true
  - id: floating-point
    prompt: >-
      A Euclidean route optimizer compares two candidate tours using `double` sums of distances. What
      does the sum-of-square-roots issue suggest?
    explanation: >-
      Near-ties can be indistinguishable at 53-bit precision, and Graham’s example needs at least 37 decimal
      digits (p. 180). Practical TSP codes, including TSPLIB conventions and Concorde, round each distance to
      an integer (for example nint of the Euclidean distance). That fixes an exact, slightly different problem
      that is verifiable in integer arithmetic. Decide on a cost representation explicitly and stop treating
      floats as exact.
    options:
      - text: "Doubles are always exact enough"
      - text: "Near-ties may be undecidable in double precision; define integer (rounded) costs or use exact arithmetic deliberately"
        correct: true
      - text: "Use single precision for speed"
      - text: "Sort distances first"
  - id: held-karp-dp
    prompt: >-
      In the Held–Karp dynamic program, trip(S, j) is the cheapest path starting at city 1, visiting all of S,
      ending at j ∈ S. What is the recurrence?
    explanation: >-
      trip(S, j) = min over i ∈ S \ {j} of [trip(S \ {j}, i) + cost(i, j)]. For example, trip({2,3,4,5,6}, 6)
      is the minimum over i ∈ {2,3,4,5} of trip({2,3,4,5}, i) + cost(i, 6) (p. 181). Base case:
      trip({j}, j) = cost(1, j). Answer: min over j of trip({2,…,n}, j) + cost(j, 1). Subsets are built up in
      increasing size (p. 181). An optimal path is made of optimal subpaths, and only the set of visited
      cities and the endpoint matter, not their order.
    options:
      - text: "trip(S, j) = max over all tours"
      - text: "trip(S, j) = trip(S, j − 1) + 1"
      - text: "trip(S, j) = minᵢ [trip(S \\ {j}, i) + cost(i, j)] over i ∈ S \\ {j}"
        correct: true
      - text: "trip(S, j) = Σ cost over S"
  - id: held-karp-time
    prompt: >-
      Why does Held–Karp run in O(n²·2ⁿ), and what would count as real progress?
    explanation: >-
      There are 2ⁿ⁻¹ subsets without city 1, at most n endpoints each, and fewer than 2n work per state:
      2ⁿ⁻¹ · n · 2n = n²2ⁿ (pp. 181–182). Memory is also exponential, O(n·2ⁿ), which is the practical limit
      around n ≈ 25–30. “Replacing n²2ⁿ by n2ⁿ would not be considered an important step. But a bound of
      n²(1.99)ⁿ”, or one with 2 raised to √n, “would be newsworthy indeed” (p. 182). It remains the record for the general TSP
      after decades.
    options:
      - text: "n! permutations pruned to n²"
      - text: "It is polynomial"
      - text: "n² per subset; improving n² to n would be big news"
      - text: "2ⁿ⁻¹ subsets × n endpoints × O(n) work; only lowering the base 2 would be real progress"
        correct: true
  - id: dp-vs-brute
    prompt: >-
      At what size does Held–Karp's n²2ⁿ become better than checking (n − 1)!/2 tours?
    explanation: >-
      “The running-time bound is better than checking all tours, once you get beyond ten cities” (p. 182). At
      n = 10: 100 · 1024 ≈ 10⁵ against 9!/2 ≈ 1.8 × 10⁵. At n = 20: about 4 × 10⁸ against about 6 × 10¹⁶. For
      very small n, brute force is fine. DP is exponentially better asymptotically but still exponential.
    options:
      - text: "Never"
      - text: "Beyond about ten cities"
        correct: true
      - text: "Beyond 1,000 cities"
      - text: "Always, from n = 2"
---
## Learning objectives

- Know which special cases are still NP-complete.
- Explain the sum-of-square-roots issue for Euclidean instances.
- Implement and analyze Held–Karp.

## Restricted versions (pp. 178–180)

- Hamiltonian circuit: NP-complete, even in bipartite graphs and grid graphs (“ultimate special case”).
- Euclidean TSP: solving it gives P = NP. Membership in NP is **open** because of exact sums of square roots (Graham’s 37-digit example).

## Held–Karp (1962)

```csharp
static double HeldKarp(double[,] cost)
{
    var n = cost.GetLength(0);
    var full = 1 << (n - 1);
    var trip = new double[full, n];
    for (var s = 0; s < full; s++)
        for (var j = 0; j < n; j++)
            trip[s, j] = double.PositiveInfinity;
    for (var j = 1; j < n; j++)
        trip[1 << (j - 1), j] = cost[0, j];
    for (var s = 1; s < full; s++)
        for (var j = 1; j < n; j++)
        {
            var bj = 1 << (j - 1);
            if ((s & bj) == 0 || s == bj) continue;
            var prev = s ^ bj;
            for (var i = 1; i < n; i++)
                if ((prev & (1 << (i - 1))) != 0)
                    trip[s, j] = Math.Min(trip[s, j], trip[prev, i] + cost[i, j]);
        }
    var best = double.PositiveInfinity;
    for (var j = 1; j < n; j++)
        best = Math.Min(best, trip[full - 1, j] + cost[j, 0]);
    return best;
}
```

Time O(n²2ⁿ), memory O(n2ⁿ). Still the best general bound. Lowering the base 2 would be news (pp. 180–182).

## In software terms

Bitmask DP over subsets is a standard tool for small n (≤ 20–25): feature-flag combinations, small route planners, test-ordering. Beyond that, memory runs out before time does.

## Sources

- Cook (2012), pp. 178–182.
- Held, M., R. M. Karp. “A dynamic programming approach to sequencing problems.” *J. SIAM* 10 (1962), 196–210.
- Bellman, R. “Dynamic programming treatment of the travelling salesman problem.” *J. ACM* 9 (1962), 61–63.
- Itai, A., C. H. Papadimitriou, J. L. Szwarcfiter. “Hamilton paths in grid graphs.” *SIAM J. Computing* 11 (1982), 676–686.
- Woeginger, G. J. “Exact algorithms for NP-hard problems: a survey.” *LNCS* 2570 (2003), 185–207.
