---
title: "5.2 Masked Relationship"
questions:
  - id: masking-pattern
    prompt: "Milk energy (K) is only weakly associated with neocortex percent (N) and with log body mass (M) separately, but strongly with both in one model. Which pattern of correlations produces this masking?"
    options:
      - text: "N and M are uncorrelated with each other"
      - text: "K is uncorrelated with both"
      - text: "N and M are both negatively associated with K"
      - text: "N and M are positively correlated with each other, while N is positively and M negatively associated with K, so their effects cancel in bivariate views"
        correct: true
    explanation: "**p. 151:** “there are two variables correlated with the outcome, but one is positively correlated with it and the other is negatively correlated with it. In addition, both of the explanatory variables are positively correlated with one another ... The result of this pattern is that the variables tend to cancel one another out.” The multiple regression asks: among species with similar body mass, do those with more neocortex have richer milk? It separates the two opposing effects."
  - id: masking-devops
    prompt: "A release adds a slower code path (raising latency) but also enables a cache that serves heavier users faster (lowering latency). Heavier usage correlates with having the new code path. Aggregate latency barely moves. What does this section suggest?"
    options:
      - text: "Two real effects may be masking each other; a model including both the code path and the cache/usage variable can reveal both"
        correct: true
      - text: "The release has no effect on latency"
      - text: "Latency should be log-transformed"
      - text: "The cache must be broken"
    explanation: "This is the masking structure: two correlated causes pushing the outcome in opposite directions. Bivariate summaries (latency versus release, latency versus usage) show little, because each effect is hidden by the other. Conditioning on both in one model uncovers them. Just as with the primates, you need a causal story to decide which conditional comparison is meaningful."
  - id: complete-cases
    prompt: "The first milk model fails with “initial value in 'vmmin' is not finite”. What was the cause and the fix?"
    options:
      - text: "The priors were too narrow; widen them"
      - text: "The neocortex variable contains missing values (NA); the quick fix is to drop incomplete rows, keeping in mind that this is itself an assumption"
        correct: true
      - text: "The data were not standardized"
      - text: "quap cannot fit two-parameter models"
    explanation: "The neocortex column has NA values, so the log-likelihood cannot be computed (p. 145–146). The book uses complete cases for now, and Chapter 15 shows how to impute missing values instead of throwing rows away. Dropping incomplete rows silently changes the population you analyse — like computing p99 only over requests that completed, ignoring timeouts."
  - id: tighten-priors
    prompt: "Prior predictive lines from `a ~ Normal(0, 1)` and `bN ~ Normal(0, 1)` swing wildly across the standardized outcome space. What tighter priors does the book choose, and why do they make sense for standardized variables?"
    options:
      - text: "a ~ Normal(0, 10) and bN ~ Normal(0, 10), because more data are available"
      - text: "Flat priors, because they are objective"
      - text: "a ~ Normal(0, 0.2) and bN ~ Normal(0, 0.5): with standardized variables the outcome should be near zero when the predictor is zero, and slopes should rarely imply impossibly strong relationships"
        correct: true
      - text: "Priors centered on the observed slope"
    explanation: "**p. 147:** “With two standardized variables, when predictor is zero, the expected value of the outcome should also be zero. And the slope βN needs to be a bit tighter as well, so that it doesn’t regularly produce impossibly strong relationships.” A slope of 2 between two standardized variables would mean a 1 SD change in N shifts K by 2 SD — very unusual. These priors are still vague but stay “within the high probability region of the observable data.”"
  - id: log-mass
    prompt: "Why does the book use the logarithm of body mass as a predictor?"
    options:
      - text: "To remove outliers"
      - text: "Because mass has missing values"
      - text: "Because quap requires it"
      - text: "Because the log converts a measure into magnitudes, and scaling measurements like body mass are often related to other variables by magnitudes"
        correct: true
    explanation: "**p. 148:** “It is often true that scaling measurements like body mass are related by magnitudes to other variables. Taking the log of a measure translates the measure into magnitudes.” Species range from tiny primates to gorillas; a doubling matters more than an absolute kilogram. Same for request sizes or customer revenue: a log scale is often the natural one for size-like quantities."
  - id: markov-equivalence
    prompt: "Three DAGs (M → N, N → M, and M ← U → N, each with both pointing to K) are all consistent with the milk data. Why can't the data choose among them?"
    options:
      - text: "Because they imply the same conditional independencies — here, none — so they form a Markov equivalence set"
        correct: true
      - text: "Because the sample is too small"
      - text: "Because U is unobserved"
      - text: "Because the priors are too wide"
    explanation: "**p. 151:** “We can’t tell from the data alone, because these graphs imply the same set of conditional independencies. In this case, there are no conditional independencies—each DAG above implies that all pairs of variables are associated, regardless of what we condition on.” Such a set is a **Markov equivalence** set. Domain knowledge must break the tie — the box on p. 152–153 suggests a lifespan-and-learning story favoring M → N. Telemetry alone often cannot tell 'CPU drives latency' from 'a hidden load factor drives both'."
  - id: unobserved
    prompt: "In the book's DAG drawings, what does a circled variable like U mean?"
    options:
      - text: "A variable that is randomized"
      - text: "A variable that is unobserved but may influence observed variables"
        correct: true
      - text: "The outcome variable"
      - text: "A variable that has been conditioned on"
    explanation: "**p. 151:** “there could be an unobserved variable U that influences both M and N, producing a correlation between them. In this book, I’ll circle variables that are unobserved. One of the threats to causal inference is that there are potentially many unobserved variables that influence an outcome or the predictors.” In systems, unobserved variables are things you did not instrument: noisy neighbors, upstream retries, a client's own caching."
  - id: simulate-masking
    prompt: "What does simulating `M <- rnorm(n); N <- rnorm(n, M); K <- rnorm(n, N - M)` demonstrate?"
    options:
      - text: "That masking only occurs in real data"
      - text: "That N causes M"
      - text: "That you can create masking with known structure and confirm that bivariate models understate both effects while the joint model recovers them"
        correct: true
      - text: "That K is independent of N"
    explanation: "From the box on p. 152: running m5.5, m5.6 and m5.7 on simulated data reproduces “the same masking pattern where the slopes become more extreme in m5.7.” The simulation is a test fixture. If your analysis pipeline cannot find an effect you planted, it will not find one in production data either."
---

The other side of multiple regression: revealing associations that are hidden because two predictors cancel
each other.

## Learning objectives

- Recognize the correlation pattern that produces masking
- Build sensible priors for standardized variables using prior predictive simulation
- Explain Markov equivalence and the limits of data for choosing among DAGs
- Simulate masking to test an analysis

## In a nutshell

| Model | Slope on N | Slope on M |
|---|---|---|
| K ~ N | weak, uncertain, positive | — |
| K ~ M | — | weak, uncertain, negative |
| K ~ N + M | clearly positive | clearly negative |

```text
Three Markov-equivalent DAGs:
  M → N,  M → K ← N
  N → M,  M → K ← N
  M ← U → N, M → K ← N   (U unobserved)
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 5.2, pp. 144–153; Figures 5.8–5.9.
- Hinde K, Milligan LA. Primate milk: proximate mechanisms and ultimate perspectives. *Evolutionary Anthropology* 2011.
