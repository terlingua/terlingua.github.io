---
title: "The 48-States Problem (pp. 62–65)"
questions:
  - id: heuristic-mindset
    prompt: >-
      Chapter 4 opens by giving up “the notion that only the absolute best solution will do.” What
      does this branch of TSP research aim for instead?
    explanation: >-
      “Let’s give up for now the notion that only the absolute best solution will do, and focus on
      delivering, as quickly as possible, a near-optimal route” (p. 62). Techniques developed here,
      “simulated annealing, genetic algorithms, and local search,” are “now workhorses in
      computational science” (p. 62). Tour finding is “a sandbox for testing methods that aim to
      select a good solution from a large population” (p. 62). Most production optimization works
      this way: a good answer quickly, measured against a bound when one is available.
    options:
      - text: "Proving optimality for every instance"
      - text: "Delivering near-optimal tours quickly"
        correct: true
      - text: "Finding the longest tour"
      - text: "Enumerating all tours in parallel"
  - id: dropping-cities
    prompt: >-
      Dantzig et al. dropped seven northeastern cities from their 49 because the atlas's shortest
      Washington–Boston drive passed through them. What assumption made this safe?
    explanation: >-
      If the optimal 42-city tour uses the direct Washington–Boston link, the seven cities can be
      picked up on that road at no extra cost, “rolling down the window and waving at Baltimore,
      Wilmington, Philadelphia, Newark, New York, Hartford, and Providence” (p. 63). If it did not
      use that link, “it would have been back to the drawing board” (p. 63). It did. Shrinking a
      problem with a structural argument and then checking the assumption is a standard
      preprocessing trick. Cook notes that with today’s Interstate 84 the assumption no longer holds
      (p. 63).
    options:
      - text: "If the optimal reduced tour used the Washington–Boston link, the dropped cities lie on it at no extra cost"
        correct: true
      - text: "They had no road distances"
      - text: "The atlas omitted them"
      - text: "Those cities were unimportant"
  - id: distance-transform
    prompt: >-
      Dantzig et al. transformed distances as round((d − 10)/17). Why, and does an affine transform
      change which tour is optimal?
    explanation: >-
      “This particular transformation was chosen to make the dᵢⱼ of the original table less than 256,
      which would permit compact storage … however, no use was made of this” (p. 64). Subtracting
      10 from every edge subtracts 10n from every tour, and dividing by 17 scales all tours equally,
      so the optimal tour is unchanged. Rounding can break ties or change the order of nearly equal
      tours. Their 699-unit tour corresponds to 12,345 miles (p. 65). Keep integer rounding in mind
      when you quantize costs, for example from seconds to minutes.
    options:
      - text: "To hide the data; it reverses tour order"
      - text: "To make the instance asymmetric"
      - text: "To make distances metric; it changes the optimum"
      - text: "To fit values in a byte; subtracting a constant and scaling preserve the optimal tour, though rounding can perturb near-ties"
        correct: true
  - id: peg-string
    prompt: >-
      How did the RAND team find their initial 49-city tour?
    explanation: >-
      “The team constructed a wooden model of the problem, placing pegs at each of the 49 locations,
      and used a string, tied to a starting city, to wrap around the pegs and trace out a tour”
      (p. 64). The taut string measures route length and suggests good continuations. It “does not
      provide a solution algorithm in any sense,” but it found the tour later proved optimal
      (pp. 64–65). It was a physical heuristic. The proof was separate.
    options:
      - text: "With a computer search"
      - text: "With a peg-and-string wooden model as a physical heuristic"
        correct: true
      - text: "By asking salesmen"
      - text: "With dynamic programming"
  - id: symmetric-assumption
    prompt: >-
      Chapter 4 assumes symmetric costs. According to the book's notes, why is this not a real loss
      of generality?
    explanation: >-
      “It is an exercise to show that any TSP instance with n cities can be converted to a 2n-city
      TSP with symmetric costs” (p. 215). The standard construction splits each city into an “in”
      copy and an “out” copy, joined by a strongly rewarded edge that any good tour must use, and
      carries the directed costs on the in–out edges. Symmetric solvers like Concorde can therefore
      handle asymmetric instances at twice the size.
    options:
      - text: "Any n-city asymmetric instance converts to a 2n-city symmetric one"
        correct: true
      - text: "Asymmetric instances are always easy"
      - text: "Asymmetric instances cannot be solved"
      - text: "Asymmetric instances never occur"
---
## Learning objectives

- Adopt the heuristic mindset: fast, near-optimal tours.
- Understand the classic 42/49-city USA instance used throughout the chapter.

## The instance

Dantzig, Fulkerson and Johnson picked a city in each of the 48 states plus Washington, D.C. (only 20 are capitals), because road distances were easy to read from a Rand McNally atlas (pp. 62–63). They:

- dropped 7 northeastern cities that lie on the Washington–Boston road, a gamble that paid off (p. 63);
- transformed distances to round((d − 10)/17), so the optimal tour is **699 units = 12,345 miles** (pp. 64–65);
- found the tour with **pegs and string** (p. 64).

The **42-city version** is the running example for every heuristic in this chapter. Remember 699 as the optimum to compare against.

## In software terms

- **Preprocess with proofs.** Shrinking an instance with a structural argument (cities on a forced path) and then checking it is a standard technique.
- **Cost transforms.** Adding a constant to every edge or scaling every edge preserves the optimum. Rounding can reorder near-ties. Be careful when quantizing.
- **Symmetric vs asymmetric.** n asymmetric cities become 2n symmetric cities (p. 215).

## Sources

- Cook (2012), pp. 62–65, 215.
- Dantzig, Fulkerson, Johnson (1954).
- Jonker, R., T. Volgenant. “Transforming asymmetric into symmetric traveling salesman problems.” *Oper. Res. Letters* 2 (1983), 161–163.
