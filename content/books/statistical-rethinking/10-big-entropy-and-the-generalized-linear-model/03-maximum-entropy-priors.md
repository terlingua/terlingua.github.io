---
title: "10.3 Maximum Entropy Priors"
questions:
  - id: two-uses
    prompt: "What are the two ways the principle of maximum entropy helps with modelling choices, according to this section?"
    type: multiple
    options:
      - text: "Choosing the likelihood: the least informative distribution consistent with constraints on the outcome"
        correct: true
      - text: "Choosing priors: when you have background information about a parameter, encode it while assuming as little else as possible"
        correct: true
      - text: "Choosing the number of MCMC chains"
      - text: "Choosing which data to drop"
    explanation: "**p. 321:** “When pressed to choose an outcome distribution—a likelihood—maximum entropy nominates the least informative distribution consistent with the constraints on the outcome variable.” And: “sometimes, rarely, some of the parameters in a GLM refer to things we might actually have background information about. When that’s true, maximum entropy provides a way to generate a prior that embodies the background information, while assuming as little else as possible.”"
  - id: weakly-informative-default
    prompt: "What does the book say about conventional weakly informative priors in GLMs?"
    options:
      - text: "They should never be used"
      - text: "They make inference subjective"
      - text: "They are always maximum entropy priors"
      - text: "They are nice because they let the data dominate inference while taming pathologies of unconstrained estimation — their “soft power”"
        correct: true
    explanation: "**p. 321:** “GLMs are easy to use with conventional weakly informative priors ... Such priors are nice, because they allow the data to dominate inference while also taming some of the pathologies of unconstrained estimation. There were some examples of their “soft power” in Chapter 9.” Maximum entropy priors are an option for the rarer cases where you have real background knowledge."
  - id: maxent-prior-example
    prompt: "You know only that a service's mean time between failures is about 30 days and that it must be positive. Which prior does maximum entropy suggest?"
    options:
      - text: "Exponential with mean 30 days (rate 1/30), the maximum entropy distribution for a positive quantity with a known mean"
        correct: true
      - text: "Normal(30, 30)"
      - text: "Uniform from 0 to infinity"
      - text: "Log-normal with any parameters"
    explanation: "Among distributions on positive reals with a fixed mean, the exponential has maximum entropy. It encodes exactly 'positive, average about 30' and nothing more. This is also why the book reads an exponential prior on σ as “containing no more information than an average deviation” (Chapter 4, p. 118–119). A Normal(30, 30) would allow negative times; a uniform has no stated mean."
---

A short section: maximum entropy can also generate priors that encode exactly what you know and no more.

## Learning objectives

- Distinguish the two uses of maximum entropy in modelling
- Recognize when a maxent prior is worth the effort
- Match simple knowledge statements to maxent priors

## In a nutshell

| What you know about a parameter | Maximum entropy prior |
|---|---|
| Lies in [a, b], nothing else | Uniform(a, b) |
| Positive, with known mean | Exponential |
| Real, with known mean and variance | Normal |
| Probability in (0, 1), with known average log p and log(1 − p) | Beta |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 10.3, p. 321.
- Jaynes ET. Prior probabilities. *IEEE Transactions on Systems Science and Cybernetics* 1968.
