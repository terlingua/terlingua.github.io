---
title: "3.1 Sampling from a Grid-Approximate Posterior (and the Vampire Test)"
questions:
  - id: vampire
    prompt: "A test detects vampirism 95% of the time, has a 1% false-positive rate, and 0.1% of people are vampires. Someone tests positive. Roughly what is the probability they are a vampire?"
    options:
      - text: "About 95%"
      - text: "About 9%"
        correct: true
      - text: "About 1%"
      - text: "About 50%"
    explanation: "**p. 49–50:** Pr(vampire | positive) = Pr(positive | vampire) Pr(vampire) / Pr(positive) ≈ 0.087. In the book's frequency format: in 100,000 people, 100 are vampires and 95 of them test positive; of the 99,900 mortals, 999 test positive. So 95 of 1,094 positives are vampires, about 8.7%. The same arithmetic governs a fraud model with 95% recall and 1% false positives on a 0.1% fraud base rate — most flagged transactions are legitimate. The rare base rate overwhelms a good test."
  - id: frequency-format
    prompt: "What does the book mean by presenting the vampire problem in a *frequency format*?"
    options:
      - text: "Using a frequentist test instead of Bayes"
      - text: "Repeating the test many times"
      - text: "Using counts of imagined people (100 vampires, 999 false positives among mortals) instead of probabilities"
        correct: true
      - text: "Measuring how often the test is run"
    explanation: "**p. 50:** “The second presentation of the problem, using counts rather than probabilities, is often called the frequency format or natural frequencies.” The chapter's strategy generalizes this: draw samples from the posterior and count them. Counting samples turns probability questions into data-summary questions, which programmers handle intuitively: filter, count, divide."
  - id: bad-science
    prompt: "In the box “Why statistics can’t save bad science”, the vampire logic is applied to hypotheses. Suppose 1% of tested hypotheses are true, a study has 95% power, and a 5% false-positive rate. What is the probability a 'significant' result is true?"
    options:
      - text: "95%"
      - text: "99%"
      - text: "5%"
      - text: "About 16%"
        correct: true
    explanation: "Same structure as the vampire test (p. 51): Pr(true | positive) = 0.95 × 0.01 / (0.95 × 0.01 + 0.05 × 0.99) ≈ 0.16. Even shrinking the false-positive rate to 1% only gets it to about 0.5. The box concludes: “The most important thing to do is to improve the base rate, Pr(true), and that requires thinking, not testing.” The same is true for A/B experimentation programs: if most shipped ideas do nothing, then most 'winning' experiments at p < 0.05 are false winners, and 'winner's curse' inflation of effect sizes follows."
  - id: why-sample
    prompt: "Why does the book teach you to work with *samples* from the posterior instead of the posterior's formula?"
    type: multiple
    options:
      - text: "Samples turn calculus (integrals) into data summary (counting), which is easier to do correctly"
        correct: true
      - text: "MCMC, used later, only produces samples, so the same skills carry over"
        correct: true
      - text: "Samples are more accurate than exact formulas"
      - text: "Many people find counting easier than integral calculus"
        correct: true
    explanation: "**p. 51:** “Working with samples transforms a problem in calculus into a problem in data summary, into a frequency format problem. An integral in a typical Bayesian context is just the total probability in some interval.” And because MCMC (Chapter 9) only returns samples, learning to summarize samples now means nothing changes later. Samples are an approximation, not more accurate than exact math; their advantage is generality and ease. Think of percentiles from a latency sample versus deriving a closed-form distribution."
  - id: how-to-sample
    prompt: "Given a grid of p values and their posterior probabilities, how do you draw posterior samples?"
    options:
      - text: "Draw grid values with replacement, with each value's chance of being drawn equal to its posterior probability"
        correct: true
      - text: "Take the p with the highest posterior and repeat it"
      - text: "Draw p values uniformly from 0 to 1"
      - text: "Draw from the prior and discard the likelihood"
    explanation: "In R: `samples <- sample(p_grid, prob=posterior, size=1e4, replace=TRUE)` (p. 52). It is weighted random selection — the same algorithm as weighted load balancing: each backend is chosen in proportion to its weight. The resulting collection of p values has a histogram that mimics the posterior. In C#: build the cumulative weights, draw a uniform number, and binary-search into the cumulative array."
  - id: sample-parameters
    prompt: "What are the 'events' being sampled when you sample from a posterior?"
    options:
      - text: "New observations of the data"
      - text: "Parameter values"
        correct: true
      - text: "Random seeds"
      - text: "Priors"
    explanation: "**p. 50:** “The posterior distribution is a probability distribution. And like all probability distributions, we can imagine drawing samples from it. The sampled events in this case are parameter values.” This is a subtle shift: you are sampling *possible states of the world* (values of p), weighted by plausibility. Later, in Section 3.3, you will feed those parameter samples into the likelihood to simulate data. Keep the two layers distinct: parameter samples versus data samples."
  - id: sample-count
    prompt: "You draw 10,000 posterior samples and your interval estimates wobble slightly between runs. What is happening?"
    options:
      - text: "The model is broken"
      - text: "The posterior itself changes between runs"
      - text: "Monte Carlo error: a finite sample only approximates the posterior, so summaries vary a little; more samples reduce the wobble"
        correct: true
      - text: "The prior is too wide"
    explanation: "Sampling introduces simulation variance, separate from the posterior's own uncertainty. With 10,000 samples the 2.5% and 97.5% quantiles might move in the third decimal place. That wobble is not information about the world, it is noise from your computation. The same applies to load-test percentiles: p99 from 1,000 requests is noisy, from 1,000,000 stable. Fix seeds for reproducibility, and draw more samples when tails matter."
---

The chapter opens with a medical-testing puzzle (vampirism) and then switches the whole approach to working
with samples.

## Learning objectives

- Apply Bayes' theorem to a rare-condition test and explain the base-rate effect
- Explain the frequency format and why the book prefers counts
- Apply the same logic to the reliability of significant findings
- Draw samples from a grid-approximate posterior

## In a nutshell

```text
Of 100,000 people:   100 vampires  -> 95 test positive
                   99,900 mortals  -> 999 test positive
P(vampire | positive) = 95 / (95 + 999) ≈ 0.087
```

- Posterior samples are parameter values drawn in proportion to their posterior probability.
- Summaries of samples approximate summaries of the posterior; more samples, less Monte Carlo error.

## Weighted sampling in C#

```csharp
double[] SamplePosterior(double[] grid, double[] posterior, int n, Random rng)
{
    var cumulative = new double[posterior.Length];
    var total = 0.0;
    for (var i = 0; i < posterior.Length; i++) cumulative[i] = total += posterior[i];
    return Enumerable.Range(0, n).Select(_ =>
    {
        var index = Array.BinarySearch(cumulative, rng.NextDouble() * total);
        return grid[index < 0 ? ~index : index];
    }).ToArray();
}
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 3 opening and Section 3.1, pp. 49–53.
- Gigerenzer G, Hoffrage U. How to improve Bayesian reasoning without instruction: frequency formats. *Psychological Review* 1995. [doi:10.1037/0033-295X.102.4.684](https://doi.org/10.1037/0033-295X.102.4.684)
- Ioannidis JPA. Why most published research findings are false. *PLoS Medicine* 2005. [doi:10.1371/journal.pmed.0020124](https://doi.org/10.1371/journal.pmed.0020124)
