---
title: "4.1 Why Normal Distributions Are Normal"
questions:
  - id: geocentric
    prompt: "Why does the book call linear regression a *geocentric* model?"
    options:
      - text: "Because it was invented by astronomers"
      - text: "Because, like Ptolemy's epicycles, it can describe and predict many phenomena well while being mechanistically wrong, so it is useful but must not be mistaken for the true process"
        correct: true
      - text: "Because it puts the data at the center of the analysis"
      - text: "Because it only works for data about the Earth"
    explanation: "**p. 71:** “Linear regression is the geocentric model of applied statistics.” The Ptolemaic system predicted planetary positions very well using circles on circles, despite a wrong picture of the solar system. Regression likewise approximates relationships flexibly without knowing the mechanism. Treat it like a well-tuned heuristic in a scheduler: excellent within its domain, dangerous if you read its internals as the truth about the system."
  - id: football-field
    prompt: "In the football-field experiment, 1,000 people each flip a coin 16 times and step left or right by a random amount. Why does the distribution of their final positions look Gaussian?"
    options:
      - text: "Because coins are fair"
      - text: "Because the steps are normally distributed"
      - text: "Because adding many independent random fluctuations tends to cancel large deviations, leaving a bell-shaped distribution of sums regardless of the step distribution"
        correct: true
      - text: "Because the field is symmetric"
    explanation: "**p. 73:** “Any process that adds together random values from the same distribution converges to a normal.” Each step is a fluctuation from the mean; big positive steps tend to be offset by negative ones, and there are vastly more ways to end near zero than far away. This is the central limit theorem in action. It is why total request latency through many independent middleware stages, or a daily sum of many small independent costs, often looks roughly normal even when each stage does not."
  - id: multiplication
    prompt: "Twelve genes each increase growth by a random 0–10%. Their effects multiply. Why is the resulting growth still approximately normal?"
    options:
      - text: "Multiplication always produces normal distributions"
      - text: "It is not; it is log-normal"
      - text: "Because genes are independent"
      - text: "Because when each effect is small, multiplying is approximately the same as adding: 1.1 × 1.1 = 1.21 ≈ 1 + 0.1 + 0.1"
        correct: true
    explanation: "**p. 74:** “Multiplying small numbers is approximately the same as addition.” The cross terms (0.01 here) are tiny when effects are small, so the product behaves like a sum. With large effects (0–50%) the approximation fails and the product becomes skewed. Engineering example: several caching layers each shaving a few percent off latency combine roughly additively; but if each layer can double or halve latency, the combination is strongly skewed."
  - id: log-multiplication
    prompt: "Large multiplicative effects produce skewed distributions. On which scale do they become Gaussian again?"
    options:
      - text: "The log scale, because adding logs is equivalent to multiplying the original numbers"
        correct: true
      - text: "The square-root scale"
      - text: "The reciprocal scale"
      - text: "They never become Gaussian"
    explanation: "**p. 74–75:** “Large deviates that are multiplied together do not produce Gaussian distributions, but they do tend to produce Gaussian distributions on the log scale.” Log turns products into sums, and sums tend toward normal. This is why many engineering quantities — file sizes, latencies, request payloads, revenue per customer — look roughly log-normal, and why analysing them on the log scale is natural. The book adds that “Since measurement scales are arbitrary, there’s nothing suspicious about this transformation.”"
  - id: ontological-epistemological
    prompt: "What are the two justifications the book gives for using the Gaussian distribution?"
    type: multiple
    options:
      - text: "Ontological: the world is full of approximately Gaussian distributions, because many processes add up fluctuations"
        correct: true
      - text: "Epistemological: if all you are willing to say is a mean and finite variance, the Gaussian is the least informative, most consistent assumption"
        correct: true
      - text: "Computational: the Gaussian is the only distribution computers can sample"
      - text: "Historical: Gauss used it for astronomy, so it is traditional"
    explanation: "**p. 75:** “The justifications for using the Gaussian distribution fall into two broad categories: (1) ontological and (2) epistemological.” Ontologically, adding fluctuations “results in a distribution of sums that have shed all information about the underlying process, aside from mean and spread.” Epistemologically, the Gaussian “is the shape that can be realized in the largest number of ways and does not introduce any new assumptions. It is the least surprising and least informative assumption to make.” That second idea — maximum entropy — returns in Chapters 7 and 10."
  - id: cant-identify-process
    prompt: "What does it imply that adding fluctuations erases everything about the underlying process except mean and spread?"
    options:
      - text: "Gaussian models can reliably identify micro-processes"
      - text: "Gaussian models cannot reliably identify micro-process, but they can still do useful work without knowing it"
        correct: true
      - text: "Gaussian models are useless"
      - text: "The Gaussian is universal in nature"
    explanation: "**p. 75:** “statistical models based on Gaussian distributions cannot reliably identify micro-process ... But it also means that these models can do useful work, even when they cannot identify process. If we had to know the development biology of height before we could build a statistical model of height, human biology would be sunk.” A normally distributed build-time metric does not tell you which steps cause slowness, but it still supports forecasting and alerting."
  - id: heavy-tails
    prompt: "What risk does the box on heavy tails warn about?"
    options:
      - text: "Gaussian tails are too heavy, so it overpredicts extremes"
      - text: "Heavy tails only matter in finance"
      - text: "The Gaussian has very thin tails; processes with heavy tails produce extreme events far more often than a Gaussian model expects"
        correct: true
      - text: "Heavy tails make the mean undefined in every case"
    explanation: "**p. 76:** “the Gaussian distribution has some very thin tails—there is very little probability in them.” And: “Many natural (and unnatural) processes have much heavier tails. These processes have much higher probabilities of producing extreme events.” Latency, incident severity and traffic spikes are classic heavy-tailed quantities in software. A Gaussian-based alert or capacity plan will be surprised by p99.9 events far more often than it predicts. Chapter 7 introduces Student-t likelihoods as a robust alternative."
  - id: density-above-one
    prompt: "R reports `dnorm(0, 0, 0.1)` ≈ 4. Is this a bug?"
    options:
      - text: "Yes; probabilities cannot exceed 1"
      - text: "No; R scales densities by 4"
      - text: "Yes; σ must be at least 1"
      - text: "No; a probability density is the rate of change of cumulative probability, so it can exceed 1 while the area under the curve never exceeds 1"
        correct: true
    explanation: "From the box on the Gaussian distribution (p. 76): “probability densities can be greater than 1 ... Probability density is the rate of change in cumulative probability. So where cumulative probability is increasing rapidly, density can easily exceed 1. But if we calculate the area under the density function, it will never exceed 1.” Discrete distributions have probability mass (Pr); continuous ones have density (p). Do not assert `density <= 1` in your code."
  - id: precision
    prompt: "Some texts write the Gaussian with τ instead of σ. What is τ?"
    options:
      - text: "The precision, τ = 1/σ², the inverse of the variance"
        correct: true
      - text: "The skewness"
      - text: "The mean"
      - text: "The sample size"
    explanation: "The book notes (p. 76–77) that the Gaussian is sometimes parameterized by precision τ = 1/σ², a convention common in older Bayesian software such as BUGS and JAGS. It is the same distribution; only the parameterization differs. Be careful when porting priors between tools: a prior with 'precision 0.001' in JAGS corresponds to a standard deviation of about 31.6, not 0.001."
---

The chapter's subject is linear regression, the "geocentric" model of statistics. First, it explains why
the Gaussian distribution is everywhere and why it is a reasonable default.

## Learning objectives

- Explain how addition, small multiplications and log-multiplications produce Gaussian distributions
- State the ontological and epistemological justifications for the Gaussian
- Recognize the danger of thin tails
- Distinguish densities from probabilities

## In a nutshell

| Process | Result | Example in software |
|---|---|---|
| Sum of many small independent effects | Gaussian | Total CPU time across many small jobs |
| Product of many small effects | Approximately Gaussian | Small percentage improvements stacked |
| Product of large effects | Log-normal (Gaussian on log scale) | File sizes, response sizes, latency |
| Rare large shocks | Heavy-tailed, not Gaussian | Incidents, traffic spikes, p99.9 latency |

## Gaussian density

```text
p(y | μ, σ) = 1 / sqrt(2πσ²) × exp(−(y − μ)² / (2σ²))
```

The important part is `(y − μ)²`: a quadratic, exponentiated into a bell. Its log is a parabola, which is
why Chapter 2's quadratic approximation is a Gaussian approximation.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 4.1, pp. 71–77.
- Taleb NN. *The Black Swan*. Random House, 2007 — on heavy tails in finance and beyond.
- Frank SA. The common patterns of nature. *Journal of Evolutionary Biology* 2009. [doi:10.1111/j.1420-9101.2009.01775.x](https://doi.org/10.1111/j.1420-9101.2009.01775.x)
