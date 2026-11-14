---
title: "World Records, 1954–1987 (pp. 153–159)"
questions:
  - id: dfj-greatest
    prompt: >-
      How does Cook rank the 1954 Dantzig–Fulkerson–Johnson paper?
    explanation: >-
      “For breadth of new ideas, wide-ranging impact, and quality of exposition, their 1954 paper stands
      alone as the greatest achievement in the history of the salesman problem” (p. 153). Applegate et al.:
      “all that came afterward is just icing on the cake.” Nemhauser and Grötschel called it “absolutely
      astonishing” that they proved optimality “by manual computation” (p. 153).
    options:
      - text: "A flawed first attempt"
      - text: "Less important than Held–Karp"
      - text: "A historical curiosity"
      - text: "The greatest achievement in TSP history"
        correct: true
  - id: held-karp-bound
    prompt: >-
      Held and Karp (1971) broke the record with 64 random points using a bound “derived from spanning
      trees.” How does that bound relate to LP?
    explanation: >-
      “The Held-Karp bound is an approximation to the optimal objective value of the subtour LP relaxation,
      and their entire approach can be seen as a means to avoid the use of general LP software” (p. 154). The
      technique uses *1-trees*: a spanning tree on cities 2…n plus two edges at city 1. Every tour is a
      1-tree, so the cheapest 1-tree is a lower bound. Node penalties πᵢ are added to edge costs and adjusted
      iteratively (Lagrangian relaxation, subgradient steps) to push the 1-tree toward degree 2 everywhere.
      The best such bound equals the subtour LP value. Karp recalled the trees “went more or less directly to
      the solution” (p. 154).
    options:
      - text: "It is the degree LP bound"
      - text: "It is an upper bound from heuristics"
      - text: "It is unrelated to LP"
      - text: "It approximates the subtour LP bound via penalized 1-trees, avoiding general LP software"
        correct: true
  - id: miliotis-dense
    prompt: >-
      Miliotis solved 80 random cities in 1975 with subtour cuts plus Gomory's algorithm but failed at 90.
      Why?
    explanation: >-
      “It is unfortunate that the cuts occupy lots of space in the A matrix … a 90 cities random Euclidean
      problem failed … because the non-zero elements in the A matrix exceeded 30,000, most of these occurring
      in the cutting plane constraints” (p. 156). Gomory cuts are dense and slow the simplex method, while
      TSP-specific cuts are sparse (p. 156).
    options:
      - text: "Dense Gomory cuts filled the constraint matrix and choked the LP solver"
        correct: true
      - text: "Random points are unsolvable beyond 80"
      - text: "He ran out of subtour inequalities"
      - text: "The computer broke"
  - id: grotschel-hand
    prompt: >-
      Grötschel's 120-city record (1977) combined what human and machine roles?
    explanation: >-
      “He used LP software to solve his models, but found his inequalities with by-hand calculations” (p. 156):
      13 LP rounds, 36 subtour and 60 comb inequalities, 30 minutes to 3 hours of hand work per round and 30
      seconds to 2 minutes of computer time, on IBM MPSX (pp. 156–157). He added “something like 5 to 20 cuts
      per run making educated choices” (p. 157). The human acted as the separation oracle, which shows
      how powerful combs are.
    options:
      - text: "The computer found cuts; Grötschel solved LPs by hand"
      - text: "Grötschel found subtour and comb cuts by hand on drawings; the computer solved the LPs"
        correct: true
      - text: "Everything was automated"
      - text: "Everything was by hand"
  - id: crowder-padberg
    prompt: >-
      What was notable about the Crowder–Padberg solution of Lin and Kernighan's 318-city drilling
      problem?
    explanation: >-
      It used a fully automated cutting-plane code. They went to dinner, expecting hours of computation, and
      on return “the program proclaimed optimality of the solution it had found in under 6 minutes” (p. 157).
      Automating separation turned a by-hand art into software. Padberg had earlier worked with Hong,
      solving up to 75 cities and bounding others (p. 157).
    options:
      - text: "It used genetic algorithms"
      - text: "It was solved by hand"
      - text: "It took 6 months"
      - text: "An automated cutting-plane code proved optimality in under 6 minutes"
        correct: true
  - id: year-1987
    prompt: >-
      Why was 1987 a banner year?
    type: multiple
    explanation: >-
      Grötschel and Holland solved the 666-city world instance (the “beastly” challenge) with a hybrid of cuts
      and IBM’s MIP code, built on many new comb separation methods (p. 158). Padberg and Rinaldi used
      branch-and-cut to solve 532 (USA), the 666-city instance, and drilling problems of 1,002 and 2,392
      cities, “easily the most complex attack on an optimization problem up to that time” (p. 159). Their new
      separation heuristics for combs and clique trees mattered more than their supercomputers (p. 159). No
      instance had been solved by simulated annealing.
    options:
      - text: "Grötschel–Holland solved 666 world cities"
        correct: true
      - text: "Padberg–Rinaldi solved 2,392 cities with branch-and-cut"
        correct: true
      - text: "Algorithmic advances in separation drove the gains"
        correct: true
      - text: "Simulated annealing solved the 2,392-city instance optimally"
---
## Learning objectives

- Trace the record line and the method behind each step.
- Understand the Held–Karp 1-tree bound and why dense cuts hurt.

## Records

| Year | Cities | Team | Key idea |
|---|---|---|---|
| 1954 | 49 | Dantzig, Fulkerson, Johnson | LP + cuts by hand |
| 1971 | 64 (random) | Held, Karp | 1-tree Lagrangian bound + B&B |
| 1975 | 67 | Camerini et al. | Improved Held–Karp |
| 1975 | 80 | Miliotis | Subtours + Gomory IP (dense cuts failed at 90) |
| 1977 | 120 | Grötschel | Combs by hand, MPSX LPs |
| 1980 | 318 | Crowder, Padberg | Automated cutting planes, 6 minutes |
| 1987 | 666 | Grötschel, Holland | Comb separation + MIP |
| 1987 | 2,392 | Padberg, Rinaldi | Branch-and-cut |

*(pp. 153–159)*

**Held–Karp bound.** 1-tree = spanning tree on cities 2…n plus the two cheapest edges at city 1. Add penalties πᵢ to edges at i, subtract 2Σπᵢ, and adjust π by subgradient steps toward degree 2. The maximum over π equals the subtour LP bound (p. 154).

## In software terms

- **Lagrangian relaxation** (move hard constraints into the objective with penalties) is a general way to get bounds without an LP solver.
- **Sparse beats dense** for iterative LP work.
- **Automate the expert.** Grötschel hand-picking cuts became Crowder–Padberg’s code.

## Sources

- Cook (2012), pp. 153–159.
- Held, M., R. M. Karp. “The traveling-salesman problem and minimum spanning trees.” *Operations Research* 18 (1970), 1138–1162; Part II, *Math. Programming* 1 (1971), 6–25.
- Grötschel, M. “On the symmetric travelling salesman problem: solution of a 120-city problem.” *Math. Programming Study* 12 (1980), 61–77.
- Crowder, H., M. W. Padberg. “Solving large-scale symmetric travelling salesman problems to optimality.” *Management Science* 26 (1980), 495–509.
- Padberg, M., G. Rinaldi (1991), *SIAM Review* 33, 60–100.
