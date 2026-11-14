---
title: "A Statistical View (pp. 39–43)"
questions:
  - id: jute-application
    prompt: >-
      Why did statistician P. C. Mahalanobis care about TSP tour lengths in the 1930s?
    explanation: >-
      To forecast the Bengal jute crop, grown on about six million small farms, he proposed
      random sample surveys. “A major component in the cost of making the survey would be the
      time spent in moving men and equipment from one sample area to the next. This is the TSP
      aspect of the application” (p. 40). He needed the *expected cost* of the survey for
      budgeting, not specific routes. His estimates fed the decision to run a test in 1937 and a
      large survey in 1938 (p. 41).
    options:
      - text: "To schedule harvest machinery"
      - text: "To design irrigation canals"
      - text: "To route jute shipments to ports"
      - text: "To estimate the travel cost of random sample surveys for budgeting"
        correct: true
  - id: sqrt-law
    prompt: >-
      For n points dropped uniformly at random in a unit square, how does the expected optimal
      tour length grow?
    explanation: >-
      Mahalanobis argued the cost would be “roughly proportional to √n.” Eli Marks (1948) proved
      a lower bound of about (1/√2)(√n − 1/√n), and M. N. Ghosh (1949) an upper bound of 1.27√n,
      so “the expected length of the tour is indeed proportional to √n” (p. 41). Intuitively,
      each point’s nearest neighbors are about 1/√n away and the tour makes n such hops:
      n · (1/√n) = √n. In a region of area A this becomes about β√(nA).
    options:
      - text: "Proportional to log n"
      - text: "Proportional to n²"
      - text: "Proportional to n"
      - text: "Proportional to √n"
        correct: true
  - id: capacity-planning
    prompt: >-
      A field-service team in one city averages 40 random stops per day with a 120 km optimal
      route. Demand quadruples to 160 stops in the same area. Using the √n law, what is a rough
      estimate of the new route length?
    explanation: >-
      Length ≈ β√(nA). With A fixed, quadrupling n doubles √n, so about 240 km and not 480 km.
      Per-stop travel *falls*, from 3 km to 1.5 km per stop, because density shortens each hop.
      This is the economics behind last-mile delivery density and why couriers want more stops
      per zone. Mahalanobis used the same reasoning to price surveys (pp. 40–41). The estimate
      assumes roughly uniform random locations and a large n.
    options:
      - text: "About 120 km"
      - text: "About 960 km"
      - text: "About 480 km"
      - text: "About 240 km"
        correct: true
  - id: bhh-theorem
    prompt: >-
      What does the Beardwood–Halton–Hammersley theorem (1959) add to the √n law?
    explanation: >-
      The earlier results bounded the *average*. Beardwood, Halton and Hammersley showed that as n
      grows, “the distribution of tour lengths will spike around a particular number called b,
      the TSP constant” (p. 43). With probability 1, length/√n → b. Figure 2.20 already shows a
      tight bell curve with mean 0.7313 for 1,000-city instances. For large random instances the
      optimal length is close to predictable, so you can sanity-check solver output against
      β√(nA).
    options:
      - text: "It gives an exact formula for every instance"
      - text: "It proves the TSP is in P for random points"
      - text: "It proves tours are always longer than √n"
      - text: "It shows the normalized tour length concentrates around a constant b as n grows"
        correct: true
  - id: tsp-constant
    prompt: >-
      What is known about the value of the TSP constant b?
    explanation: >-
      “Proven estimates do not come close to pinning it down. So we have a natural constant whose
      actual value is unknown” (p. 43). Applegate, Johnson, Sloane and Cook solved over 600
      million random instances with Concorde, and the trend points “toward an ultimate value of
      approximately 0.712 for b” (p. 43). That is strong computational evidence, but “the
      mountain of computation alone cannot prove any definitive results” (p. 43). Experiments
      estimate, proofs establish, and engineers need to keep the two apart.
    options:
      - text: "It is exactly 1/√2"
      - text: "It is proven to be 0.7124"
      - text: "It is unknown; massive computation suggests about 0.712, but nothing is proven to that precision"
        correct: true
      - text: "It is 1.27, from Ghosh"
  - id: ghosh-operational
    prompt: >-
      Ghosh noted that after locating the random points “it is very difficult to find out
      actually the shortest path connecting the points.” Why is that notable?
    explanation: >-
      It shows that “he observed the heart of the TSP challenge of finding optimal tours,
      apparently without connection to Menger, Whitney, and Flood” (pp. 41–42). Statisticians
      could estimate averages easily while finding specific optimal tours stayed hard.
      Estimation is easy, optimization is hard: you can forecast fleet mileage without solving
      a single route.
    options:
      - text: "It was the first proof of NP-completeness"
      - text: "It independently identified the hardness of computing specific optimal tours, separate from the averages"
        correct: true
      - text: "It solved the jute problem exactly"
      - text: "It introduced cutting planes"
  - id: small-n-correction
    prompt: >-
      Figure 2.21 shows the average of (tour length)/√n *decreasing* as n increases toward 1,000.
      What causes this, and what is the practical consequence?
    explanation: >-
      Points near the square’s boundary have fewer neighbors, which inflates tour length for
      small n. The effect fades as n grows, so the normalized average slides down toward b ≈ 0.712
      (p. 43). The 1,000-city mean was 0.7313 (p. 43). For capacity models with tens of stops, use
      a larger coefficient than the asymptotic constant, or fit β to your own data.
    options:
      - text: "Boundary effects inflate small instances; for small n use a larger coefficient or fit one empirically"
        correct: true
      - text: "Random numbers are biased"
      - text: "The constant b changes with hardware"
      - text: "Solvers get worse on large instances"
---
## Learning objectives

- Explain the √n law for random Euclidean tours and use it for estimates.
- State what Beardwood–Halton–Hammersley proved and what remains unknown about b.
- Separate *estimating* typical tour length from *finding* an optimal tour.

## Jute, sampling and tours

Mahalanobis designed random sample surveys of Bengal’s jute fields. The cost of moving crews between sample points is a TSP, and he estimated its expected length as proportional to √n, which became part of the 1937–38 survey budget (pp. 39–41).

## The √n law

For n uniform random points in a unit square:

- Marks (1948): E[length] ≥ (1/√2)(√n − 1/√n).
- Ghosh (1949): E[length] ≤ 1.27 √n.
- Hence E[length] = Θ(√n) (p. 41). In a region of area A, length ≈ b·√(n·A).

**Beardwood–Halton–Hammersley (1959):** with probability 1, length/√(nA) → b as n → ∞. Optimal tour lengths concentrate (p. 43).

**The constant b** is unknown. Over 600,000,000 instances solved with Concorde suggest about 0.712 (p. 43). Finite-n averages are higher because of boundary effects (0.7313 at n = 1,000).

```csharp
static double EstimateTourLength(int stops, double areaKm2, double beta = 0.7124) =>
    beta * Math.Sqrt(stops * areaKm2);
```

## In software terms

- **Capacity planning without solving.** Doubling stops in a fixed area raises route length by about √2 ≈ 1.41, and per-stop travel falls.
- **Sanity checks.** If your solver’s output on random-like data is far above b√(nA), suspect a bug or a bad cost matrix.
- **Calibrate.** Real cities are not uniform squares. Fit β to historical optimal or near-optimal routes.

## Sources

- Cook (2012), pp. 39–43.
- Mahalanobis, P. C. “A sample survey of the acreage under jute in Bengal.” *Sankhyā* 4 (1940), 511–530.
- Beardwood, J., J. H. Halton, J. M. Hammersley. “The shortest path through many points.” *Proc. Cambridge Phil. Soc.* 55 (1959), 299–327.
- Applegate, D. L., W. J. Cook, D. S. Johnson, N. J. A. Sloane. “Using large-scale computation to estimate the Beardwood–Halton–Hammersley TSP constant.” Presentation, 2010 (estimate ≈ 0.7124).
