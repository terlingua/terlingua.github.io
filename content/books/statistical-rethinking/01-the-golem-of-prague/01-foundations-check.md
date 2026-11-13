---
title: "Foundations Check — Probability for Programmers"
questions:
  - id: probability-as-counting
    prompt: "A feature flag is rolled out to a random 20% of 10,000 users. Before looking at any user, what does “the probability this user has the flag is 0.2” describe?"
    options:
      - text: "Your state of information: of all the ways the rollout could have assigned this user, 20% of them include the flag"
        correct: true
      - text: "A physical property of the user's device"
      - text: "A guarantee that exactly 2,000 users will see the flag every time you rerun the rollout"
      - text: "Nothing meaningful until the user is observed"
    explanation: "Statistical Rethinking treats probability as a calculus for counting possibilities consistent with what you know. **p. 10:** “Bayesian data analysis is no more than counting the numbers of ways the data could happen, according to our assumptions. Things that can happen more ways are more plausible.” The hashing function that assigned the flag is completely deterministic; the 0.2 is about *your* ignorance of which bucket this user's ID hashed into. Once you look up the user's assignment, the probability for you collapses to 0 or 1, even though nothing about the user changed. This is the core shift the book asks for: probability describes information, not a property of the world. The rollout still produces *frequencies* (about 2,000 users), but frequencies are facts about a particular population, whereas probabilities are statements about what you know."
  - id: conditional-probability
    prompt: "Your alerting logs show that P(alert | outage) = 0.95 and P(outage | alert) = 0.10. Which statement is correct?"
    options:
      - text: "These two numbers contradict each other; one of them must be a bug"
      - text: "The alert almost always fires during outages, but most alerts happen when there is no outage — typical when outages are rare and the alert is noisy"
        correct: true
      - text: "Conditional probabilities are symmetric, so both should be 0.95"
      - text: "The alert is useless and should be deleted"
    explanation: "P(A | B) and P(B | A) answer different questions. P(alert | outage) asks: among outages, how often does the alert fire? P(outage | alert) asks: among alerts, how often is there really an outage? Bayes' theorem links them: P(outage | alert) = P(alert | outage) × P(outage) / P(alert). If outages are rare (small prior) and the alert also fires on benign spikes, then P(alert) is dominated by false alarms, and P(outage | alert) is small even when the alert is sensitive. Chapter 3 opens with exactly this structure using a vampirism test (p. 49–50): a test that is 95% accurate for a rare condition still yields mostly false positives. Confusing the two directions is the base-rate fallacy, and it is why on-call engineers learn to distrust a page that 'always fires when things break'."
  - id: joint-and-marginal
    prompt: "In a table of requests by region and status code, what does summing the joint probabilities P(region, status) over all regions give you?"
    options:
      - text: "The likelihood of the region"
      - text: "The conditional probability P(region | status)"
      - text: "The marginal probability P(status)"
        correct: true
      - text: "Always 1 for every status"
    explanation: "Summing (or integrating) a joint distribution over one variable *marginalizes* it out. P(status) = Σ over regions of P(region, status). This operation is everywhere in Bayesian work: the denominator of Bayes' theorem, the “average probability of the data”, is a marginal obtained by summing the likelihood × prior over every possible parameter value (p. 37). Later, the book's predictive simulations (posterior predictive distributions, Chapter 3) are also marginals: they average predictions over the uncertainty in the parameters. Think of it as a `GROUP BY status` with `SUM(p)`."
  - id: distribution-vs-sample
    prompt: "You call `rand.NextDouble()` 10,000 times and draw a histogram. What is the relationship between the histogram and the uniform distribution?"
    options:
      - text: "They are identical by definition"
      - text: "The histogram replaces the distribution; the distribution no longer matters"
      - text: "The histogram is useless because the generator is pseudo-random"
      - text: "The histogram is an approximation of the distribution built from samples; with more samples it gets closer"
        correct: true
    explanation: "This is the central computational idea of Chapter 3, *Sampling the Imaginary*. Instead of doing calculus on a probability distribution, you draw many samples from it and summarize the samples: means, intervals, histograms. **p. 51:** “working with samples transforms a problem in calculus into a problem in data summary, into a frequency format problem.” Software developers already think this way: you profile latency by collecting samples and reading percentiles, not by deriving a closed-form distribution. The approximation error shrinks as the sample count grows, which is why later chapters care about *effective* sample size (Chapter 9)."
  - id: expected-value
    prompt: "A cache hit costs 1 ms and a miss costs 40 ms. The hit rate is 0.9. What concept gives you 0.9 × 1 + 0.1 × 40 = 4.9 ms?"
    options:
      - text: "The expected (mean) latency, a probability-weighted average"
        correct: true
      - text: "The median latency"
      - text: "The 90th percentile latency"
      - text: "The maximum likelihood estimate"
    explanation: "An expectation is a weighted average of outcomes, with probabilities as weights. Note how badly it describes the actual experience: no single request takes 4.9 ms. That gap between a summary and the distribution it summarizes is a theme of Chapter 3, where the book warns that a point estimate throws away the shape of the posterior and argues that you should usually report the whole distribution, or at least an interval, rather than one number (p. 58–61). Expected values reappear in Chapter 7 as the foundation of entropy and deviance, which are averages of log-probabilities."
  - id: independence
    prompt: "Two microservices fail independently, each with probability 0.01 per hour. Which statements are correct?"
    type: multiple
    options:
      - text: "The probability both fail in the same hour is 0.01 × 0.01 = 0.0001"
        correct: true
      - text: "Independence means they share no code"
      - text: "If they share a database, assuming independence will probably understate the probability that both fail together"
        correct: true
      - text: "Independence means P(A and B) = P(A) + P(B)"
    explanation: "Independence means P(A, B) = P(A) × P(B): knowing one tells you nothing about the other. It is a statement about information, not about code. Shared dependencies (a database, a region, a deploy) create correlation, and multiplying marginal probabilities then underestimates joint failure — the classic mistake in naive availability math. The book's likelihoods rely on the same assumption: the binomial in Chapter 2 assumes each globe toss is independent of the others (p. 33), and multilevel models in Chapter 13 exist precisely because observations from the same cluster (same user, same host) are *not* independent."
  - id: log-probabilities
    prompt: "Why does statistical software usually work with log-probabilities instead of probabilities?"
    type: multiple
    options:
      - text: "Multiplying many small probabilities underflows floating-point numbers; adding logs does not"
        correct: true
      - text: "Logarithms turn products of independent likelihoods into sums, which are easier to differentiate and accumulate"
        correct: true
      - text: "Log-probabilities are always positive"
      - text: "Logs make the posterior distribution normal"
    explanation: "The likelihood of a dataset is a product of the probabilities of each observation. With thousands of rows that product falls below the smallest representable double. Taking logs turns the product into a sum (log of a product = sum of logs). Log-probabilities are negative or zero, not positive. The book relies on this repeatedly: grid approximation code in Chapter 4 works on the log scale, the `lppd` and deviance in Chapter 7 are sums of logs (p. 210), and Hamiltonian Monte Carlo in Chapter 9 uses the gradient of the log-posterior as a potential energy surface (p. 272–276)."
  - id: random-variable
    prompt: "In Statistical Rethinking's notation `y ~ Normal(μ, σ)`, what does the tilde `~` mean?"
    options:
      - text: "y is approximately equal to μ"
      - text: "y is distributed as — the relationship is stochastic, described by a probability distribution"
        correct: true
      - text: "y is assigned the value μ plus noise drawn once at startup"
      - text: "y is a function of σ only"
    explanation: "The book distinguishes **stochastic** relationships, written with `~`, from **deterministic** ones, written with `=`. **p. 77:** the recipe is to recognize variables, then “define each variable either in terms of the other variables or in terms of a probability distribution.” If you write code, think of `~` as declaring the type of a random generator ('this column is generated by a Normal with these arguments') and `=` as an ordinary assignment, for example `μ = α + β x`. A model written this way is also a recipe for *simulating* data, which the book uses constantly to check models before fitting them."
  - id: model-as-generator
    prompt: "Why does the book insist that every model be written out in full, rather than using a one-line formula like `y ~ x + (1|g)`?"
    options:
      - text: "Because formula syntax is slower to execute"
      - text: "Because full model definitions are required by Stan"
      - text: "Because writing every likelihood and prior makes all assumptions explicit and shows how the mathematical model maps onto the code"
        correct: true
      - text: "Because one-line formulas cannot fit multilevel models"
    explanation: "From the preface to the second edition (**p. x**): formulas like `y ~ (1|x) + z` “don’t show the parameters, nor even all of the terms”, so “it is not easy to see how the mathematical model relates to the code. It is ultimately kinder to be a bit cruel and require more work.” For engineers the analogy is explicit dependency injection versus magic auto-wiring: the magic is convenient once you understand it, but you learn the system by wiring it by hand. Tools like brms and rstanarm are recommended *after* the book."
  - id: why-not-mcmc-first
    prompt: "Why does the book postpone Markov chain Monte Carlo until Chapter 9?"
    options:
      - text: "Because MCMC is not Bayesian"
      - text: "Because the R package did not support MCMC until the second edition"
      - text: "Because MCMC only works for linear models"
      - text: "Because Bayes is not about MCMC: the goal is the posterior, and MCMC is one of several ways to approximate it"
        correct: true
    explanation: "**p. x:** “Bayes is not about MCMC. We seek the posterior distribution, but there are many legitimate approximations of it. MCMC is just one set of strategies.” The first half uses quadratic approximation (`quap`), which is fast and ties neatly to non-Bayesian maximum likelihood. Separating *what* you compute (a posterior) from *how* you compute it (grid, quadratic approximation, MCMC) is like separating an interface from its implementations."
---

The book assumes no background in probability theory, but it moves fast. This section checks the handful of
ideas every later chapter leans on, framed in situations a developer meets every week.

## Learning objectives

- Read probability as a statement about information rather than a property of objects
- Distinguish joint, marginal and conditional probability, and apply Bayes' theorem in words
- Explain why samples can stand in for distributions
- Read the book's model notation (`~` versus `=`)
- Explain why the book makes you write models out in full and delays MCMC

## About this companion

This is an unofficial companion to Richard McElreath, *Statistical Rethinking: A Bayesian Course with
Examples in R and Stan*, second edition (CRC Press, 2020). Chapters and sections follow the book. Page
numbers (p. N) refer to the printed second edition. Short quotations appear inside “ ” so they can be
checked against the source.

Every question's explanation is written to teach the concept in full, so you can study from the
companion alone and open the book when you want depth, code or figures. The book is not written for
programmers, so most questions put the idea into a software, DevOps or product setting: A/B tests,
incident counts, latency, feature flags, telemetry, CI pipelines.

## In a nutshell

| Idea | Plain meaning | Where it returns |
|---|---|---|
| Probability as counting | Plausibility is proportional to the number of ways the data could arise | Chapter 2 |
| Conditional probability | P(A \| B) is not P(B \| A) | Chapters 2–3 |
| Marginalizing | Sum a joint distribution over the variable you do not care about | Chapters 2, 3, 15 |
| Samples as distributions | Summarize draws instead of doing calculus | Chapters 3, 9 |
| Log scale | Products become sums; avoids underflow | Chapters 7, 9 |
| Model notation | `~` stochastic, `=` deterministic | Chapter 4 onward |

## Bayes' theorem in one paragraph

For a parameter p and data D:

```text
P(p | D) = P(D | p) × P(p) / P(D)
posterior = likelihood × prior / average probability of the data
```

The denominator just makes the posterior sum to 1. What matters is the numerator: the posterior is
proportional to how many ways each candidate value of p could have produced D, weighted by how plausible
that value was beforehand.

## Helpers used in the C# snippets

Code in later sections is illustrative and assumes these small helpers and a shared `Random rng`.

```csharp
static double StandardNormal(Random rng) =>
    Math.Sqrt(-2 * Math.Log(1 - rng.NextDouble())) * Math.Cos(2 * Math.PI * rng.NextDouble());

static double Logistic(double x) => 1.0 / (1.0 + Math.Exp(-x));

static double LogFactorial(int n) => Enumerable.Range(1, n).Sum(i => Math.Log(i));

static double LogPoisson(int k, double lambda) => k * Math.Log(lambda) - lambda - LogFactorial(k);
```

## Sources

- McElreath R. *Statistical Rethinking: A Bayesian Course with Examples in R and Stan*, 2nd ed. CRC Press, 2020. Preface to the second edition, pp. ix–x; Chapter 2–3.
- McElreath R. *Statistical Rethinking* lecture series and materials: [github.com/rmcelreath/stat_rethinking_2024](https://github.com/rmcelreath/stat_rethinking_2024)
