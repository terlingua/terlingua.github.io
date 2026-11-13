---
title: "2.1 The Garden of Forking Data"
questions:
  - id: small-vs-large
    prompt: "What is the difference between the *small world* and the *large world*?"
    options:
      - text: "The small world is a small dataset; the large world is big data"
      - text: "The small world is the self-contained logical world of the model, where all possibilities are nominated; the large world is the real context where the model is deployed and surprises can happen"
        correct: true
      - text: "The small world is the sample; the large world is the population"
      - text: "The small world is Bayesian; the large world is frequentist"
    explanation: "**p. 19:** “The small world is the self-contained logical world of the model. Within the small world, all possibilities are nominated. There are no pure surprises.” And: “The large world is the broader context in which one deploys a model. In the large world, there may be events that were not imagined in the small world.” A load test is a small world: traffic follows the script you wrote. Production is the large world: a viral post, a misbehaving client retrying in a tight loop, a leap second. Bayesian inference is optimal *within* the small world; whether it works in the large world must be checked separately."
  - id: columbus
    prompt: "The chapter opens with Columbus's voyage. What is the point of the story?"
    options:
      - text: "Columbus used Bayesian inference correctly"
      - text: "Small sample sizes always lead to errors"
      - text: "Columbus's small-world model of a small Earth made a confident prediction that failed only because the large world held an unmodeled continent"
        correct: true
      - text: "Maps are not models"
    explanation: "Columbus believed the Earth was small enough to reach Asia by sailing west, using a globe that was a model of the world. The model was internally consistent but wrong about the large world, and he was saved by a continent nobody had included (p. 19). Models fail not only through bad arithmetic but through the things they never nominated as possible. Capacity plans fail the same way: perfectly calculated for the traffic shapes you modeled, then broken by one you did not."
  - id: count-ways
    prompt: "A bag holds four marbles, each blue or white. You draw with replacement and see blue, white, blue. How many ways can the conjecture [1 blue, 3 white] produce this sequence?"
    options:
      - text: "1"
      - text: "9"
      - text: "0"
      - text: "3"
        correct: true
    explanation: "Count paths through the garden of forking data. On each draw there are 4 marbles you could get. For the conjecture with 1 blue and 3 white: first draw blue — 1 way; second draw white — 3 ways; third draw blue — 1 way. Total 1 × 3 × 1 = 3. Doing the same for every conjecture (p. 23) gives [0 blue]: 0, [1 blue]: 3, [2 blue]: 8, [3 blue]: 9, [4 blue]: 0. Each count says how many ways that hypothesis could have generated what you saw. Bayesian inference is nothing but this counting, made efficient with probability theory."
  - id: why-multiply
    prompt: "You draw a fourth marble and it is blue. To update, the book multiplies the old count by the number of ways the new observation can happen. Why multiply?"
    options:
      - text: "Multiplication is a shortcut for enumerating and counting every path through the garden that produces all the observations"
        correct: true
      - text: "Because probabilities must always be multiplied"
      - text: "Because the new observation is more important than the old ones"
      - text: "To keep the numbers small"
    explanation: "**p. 25:** “Multiplication is just a shortcut to enumerating and counting up all of the paths through the garden that could produce all the observations.” Each old path branches into as many new paths as the new observation allows. For [3 blue]: 9 old ways × 3 ways to draw blue = 27. This also shows why Bayesian updating can be done one observation at a time: yesterday's posterior is today's prior. It is how a streaming aggregator updates a running count without re-reading the whole event log."
  - id: impossible-zero
    prompt: "After seeing at least one blue and one white marble, what happens to the conjectures 'all white' and 'all blue'?"
    options:
      - text: "They get small but nonzero plausibility"
      - text: "They get zero ways, so they are ruled out entirely, and no later data can revive them"
        correct: true
      - text: "They become the most plausible"
      - text: "Nothing changes until more data arrive"
    explanation: "A conjecture with zero ways to produce the data has zero plausibility, and because updating multiplies, zero × anything = 0 forever. This is the strongest form of learning — logical elimination — and it is why the book later warns against priors that put exactly zero on possibilities you have not ruled out (Chapter 4 and onward). If your model says a value is impossible, no amount of evidence will convince it otherwise. Engineers see this with hard-coded assumptions: 'this field is never null' cannot be unlearned by the code, only by a human."
  - id: prior-from-other-data
    prompt: "The factory says blue marbles are rare: for every bag with 3 blue there are 2 bags with 2 blue and 3 with 1 blue. How should this information enter the analysis?"
    options:
      - text: "It should be ignored because it is not data from the bag"
      - text: "It replaces the counts from the draws"
      - text: "It is multiplied by the counts from the draws, as prior counts, just like another observation"
        correct: true
      - text: "It should be added to the counts"
    explanation: "The book treats prior information as having the same logical status as data (p. 25–26): multiply the ways each conjecture produces the observed draws by the factory's counts. Prior information is just information from another source. Whether you call it 'prior' or 'data' depends only on when it arrived. In practice: a fleet-wide historical failure rate is a legitimate prior for a new server's failure rate; it is evidence, not opinion."
  - id: names
    prompt: "Match the counting terms to probability theory. Which statements are correct?"
    type: multiple
    options:
      - text: "A conjectured proportion p is called a parameter value"
        correct: true
      - text: "The relative number of ways a value of p can produce the data is called a likelihood"
        correct: true
      - text: "The prior probability is the plausibility after seeing the data"
      - text: "The updated plausibility of p is called the posterior probability"
        correct: true
    explanation: "**p. 27:** “A conjectured proportion of blue marbles, p, is usually called a parameter value. It’s just a way of indexing possible explanations of the data.” “The relative number of ways that a value p can produce the data is usually called a likelihood.” “The prior plausibility of any specific p is usually called the prior probability.” “The new, updated plausibility of any specific p is usually called the posterior probability.” Normalizing the counts so they sum to 1 turns them into probabilities: 0, 3, 8, 9, 0 becomes 0, 0.15, 0.40, 0.45, 0."
  - id: randomization
    prompt: "According to the box on randomization, what does it mean to *randomize* a deck of cards?"
    options:
      - text: "The cards acquire a physical property called randomness"
      - text: "Every ordering becomes equally frequent across all shuffles in history"
      - text: "The deck becomes impossible to sort"
      - text: "We have processed it so that we know almost nothing about its arrangement"
        correct: true
    explanation: "**p. 28:** randomizing “just means that we have processed the thing so that we know almost nothing about its arrangement. Shuffling a deck of cards changes our state of knowledge”. Randomness is a property of information, not of the cards. That is why a seeded pseudo-random generator is fine for A/B assignment: users and analysts cannot predict the assignment, so it cannot correlate with anything that matters. It also explains why randomized assignment kills confounding — nothing about the user can be informative about which arm they got."
  - id: deploy-analogy
    prompt: "A canary analysis compares error counts under three conjectured error rates for a new build: 0.1%, 1% and 5%. In 1,000 requests you see 9 errors. In the language of this section, what are you doing when you compute how likely 9 errors are under each rate?"
    options:
      - text: "Computing the likelihood of the data under each conjecture, so you can multiply by prior plausibilities and normalize"
        correct: true
      - text: "Computing the prior"
      - text: "Computing the posterior directly"
      - text: "Running a null hypothesis test against 0%"
    explanation: "Each conjectured rate is a parameter value; the probability of 9 errors in 1,000 requests under each rate is the likelihood — the continuous-world version of counting the ways. Multiplying by how plausible each rate was before the canary (perhaps most builds sit near 0.1%) and normalizing gives the posterior. The 1% conjecture will dominate because 9/1,000 is close to 1%; 0.1% expects about 1 error and 5% about 50. This is the garden of forking data with a binomial doing the counting."
---

Bayesian inference, stripped down, is counting. This section builds it from a bag of marbles before any
formula appears.

## Learning objectives

- Distinguish the small world of a model from the large world it is used in
- Compute plausibilities by counting the ways each conjecture produces the data
- Explain why updating multiplies counts, and why prior information is used the same way
- Name parameter, likelihood, prior and posterior

## In a nutshell

| Conjecture (blue of 4) | Ways to get blue, white, blue | Plausibility |
|---|---|---|
| 0 | 0 × 4 × 0 = 0 | 0 |
| 1 | 1 × 3 × 1 = 3 | 0.15 |
| 2 | 2 × 2 × 2 = 8 | 0.40 |
| 3 | 3 × 1 × 3 = 9 | 0.45 |
| 4 | 4 × 0 × 4 = 0 | 0 |

```text
plausibility of p after D  ∝  ways p can produce D  ×  prior plausibility of p
```

## Why this matters for engineers

- The posterior is an **incremental fold**: `posterior = observations.Aggregate(prior, Update)`. Order does not
  matter, as long as the observations are independent.
- A **zero** anywhere is permanent. Never give zero prior weight to something merely unlikely.
- **Randomness is ignorance.** Hash-based bucketing is random enough because nobody can predict it.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 2 opening and Section 2.1, pp. 19–28.
- Borges JL. “The Garden of Forking Paths”, 1941 — the story the section's title refers to.
