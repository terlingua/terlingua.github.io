---
title: "15.1 Measurement Error"
questions:
  - id: pancakes
    prompt: "Three pancakes: burnt on both sides (BB), one side (BU), neither (UU). One is served at random and the up side is burnt. What is the probability the down side is burnt?"
    options:
      - text: "2/3"
        correct: true
      - text: "1/3"
      - text: "1/2"
      - text: "1"
    explanation: "**p. 489:** Pr(burnt down | burnt up) = Pr(BB) / Pr(burnt up) = (1/3) / (1/2) = 2/3, where Pr(burnt up) = Pr(BB)(1) + Pr(BU)(0.5) + Pr(UU)(0) = 0.5. The trick is to count *sides*, not pancakes (p. 490). The chapter's opening point: “A big advantage of Bayesian inference is that it obviates the need to be clever.” Ruthlessly apply conditional probability instead of trusting intuition."
  - id: data-as-distribution
    prompt: "What is the key insight for handling measurement error?"
    options:
      - text: "Drop observations with large errors"
      - text: "Ordinary data are a special case of a distribution: if a measurement has known error, put a parameter for the true value and let the observation be distributed around it"
        correct: true
      - text: "Average all measurements"
      - text: "Measurement error never matters"
    explanation: "**p. 493:** “ordinary data are a special case of a distribution. And here is the key insight: If we don’t know the true value ... then we can just put a parameter there and let Bayes do the rest.” Dobs,ᵢ ~ Normal(Dtrue,ᵢ, Dse,ᵢ). Simulating error goes from truth to data; estimating runs it in reverse. Synthetic-monitoring results with known probe jitter are a natural fit."
  - id: double-role
    prompt: "In the divorce measurement error model, what two roles does each Dtrue parameter play?"
    options:
      - text: "Prior and posterior"
      - text: "Intercept and slope"
      - text: "The mean of the distribution of the observed measurement, and the outcome of the regression on age and marriage rate"
        correct: true
      - text: "Data and noise"
    explanation: "**p. 493:** “Each Dtrue parameter also gets a second role as the mean of another distribution, one that predicts the observed measurement.” Dtrue,ᵢ ~ Normal(μᵢ, σ), with μᵢ = α + βA Aᵢ + βM Mᵢ, and Dobs,ᵢ ~ Normal(Dtrue,ᵢ, Dse,ᵢ). Information flows both ways: the regression informs the true values, and the observations inform the regression."
  - id: me-shrinkage
    prompt: "Which states' divorce rates are adjusted most by the measurement error model (Figure 15.2)?"
    options:
      - text: "States with the smallest standard errors"
      - text: "None; estimates stay at observed values"
      - text: "All states equally"
      - text: "States with the largest standard errors (typically small states), whose estimates are pulled toward the regression line"
        correct: true
    explanation: "Caption: “The less error in the original measurement, the less shrinkage in the posterior.” This is the multilevel shrinkage idea again: less certain estimates borrow strength from more certain ones. Smaller states produce more uncertain estimates (Figure 15.1). Metrics from low-traffic regions are noisy and should be pulled toward what the model expects."
  - id: me-effect
    prompt: "What happens to the regression trend when measurement error in divorce rate is modelled?"
    options:
      - text: "It moves: uncertain states are pulled toward the trend, while precisely measured states pull the trend toward themselves"
        correct: true
      - text: "It reverses"
      - text: "It becomes infinite"
      - text: "Nothing at all changes"
    explanation: "Figure 15.2 (right) compares the error-ignoring regression with the one that models error: the trend moves. **p. 495:** “For a State with an uncertain divorce rate, the trend has strongly influenced the new estimate of divorce rate. For a State with a fairly certain divorce rate—a small standard error—the State has instead strongly influenced the trend.” Noisy states with extreme observed rates no longer get full weight. Ignoring measurement error can exaggerate or mask relationships depending on where the error is."
  - id: predictor-error
    prompt: "How is measurement error in a *predictor* (marriage rate) handled?"
    options:
      - text: "It cannot be handled"
      - text: "The same way: Mobs ~ Normal(Mtrue, Mse), with Mtrue given its own prior, and Mtrue used in the linear model instead of Mobs"
        correct: true
      - text: "By dropping the predictor"
      - text: "By averaging it with the outcome"
    explanation: "**p. 496:** the model adds Mobs,ᵢ ~ Normal(Mtrue,ᵢ, Mse,ᵢ) and Mtrue,ᵢ ~ Normal(0, 1), using Mtrue in μ. Figure 15.3 shows both variables shrinking toward the inferred relationship. Errors-in-variables matter in engineering when a predictor is itself an estimate — e.g. a sampled CPU utilization or an estimated user count."
  - id: correlated-errors
    prompt: "What extra danger arises if errors on D and M are both influenced by a common variable P (e.g. population size)?"
    options:
      - text: "None"
      - text: "The errors cancel out"
      - text: "A non-causal path opens through the errors, so naive regression of Dobs on Mobs is confounded; you must model the covariance between errors"
        correct: true
      - text: "The model becomes linear"
    explanation: "**p. 498:** “if we naively regress Dobs on Mobs, then there is an open non-causal path through P. If we have information about the measurement process ... there is still hope. But we’ll need to consider the covariance between the errors.” In observability, two metrics sampled by the same flaky collector have correlated errors that create spurious correlation between them."
  - id: me-workflow
    prompt: "What general workflow does the book recommend for measurement concerns?"
    options:
      - text: "Ignore them unless reviewers ask"
      - text: "Use only nonparametric methods"
      - text: "Always collect more data"
      - text: "Use background knowledge to write generative models including the measurement process, simulate to understand the risks, and design a statistical approach that works at least in theory"
        correct: true
    explanation: "**p. 499:** “Use your background knowledge to write down a generative model or models, simulate data from these models in order to understand the inferential risks, and design a statistical approach that can work at least in theory.” The same three steps that run through the whole book."
---

Measured values are not true values. Bayesian models can treat the truth as a parameter and the measurement
as data about it.

## Learning objectives

- Solve the pancake problem by conditional probability
- Model outcome and predictor measurement error with true-value parameters
- Explain the resulting shrinkage
- Recognize correlated measurement errors as a confounding path

## In a nutshell

```text
Dobs,ᵢ  ~ Normal(Dtrue,ᵢ, Dse,ᵢ)          observation model (known error)
Dtrue,ᵢ ~ Normal(μᵢ, σ)                   process model
μᵢ = α + βA Aᵢ + βM Mtrue,ᵢ
Mobs,ᵢ  ~ Normal(Mtrue,ᵢ, Mse,ᵢ)
Mtrue,ᵢ ~ Normal(0, 1)
```

## For engineers

Sampled metrics, estimated cardinalities (HyperLogLog), synthetic probe timings and survey responses all come
with error you can quantify. Treat them as noisy observations of a true quantity rather than as truth.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 15 opening and Section 15.1, pp. 489–499; Figures 15.1–15.3.
- Carroll RJ et al. *Measurement Error in Nonlinear Models*, 2nd ed. Chapman & Hall/CRC, 2006.
