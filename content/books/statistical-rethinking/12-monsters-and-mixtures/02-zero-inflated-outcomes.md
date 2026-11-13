---
title: "12.2 Zero-Inflated Outcomes"
questions:
  - id: drunk-monks
    prompt: "Monks sometimes spend a day drinking and produce no manuscripts; on working days, output is Poisson. Why can't you tell drinking days from unproductive working days?"
    options:
      - text: "Because the monks lie"
      - text: "Because Poisson cannot produce zeros"
      - text: "Because both processes can produce zero; a zero is ambiguous between 'did not work' and 'worked but finished nothing'"
        correct: true
      - text: "Because the data are aggregated"
    explanation: "Figure 12.3 (p. 377): “Drinking monks always produce an observation y = 0. Working monks may produce either y = 0 or y > 0 ... In real data, we typically cannot see which zeros come from which process.” Systems analogy: a service reporting zero errors in a minute could be healthy, or its error exporter could be down."
  - id: zi-likelihood
    prompt: "In the zero-inflated Poisson, what is the probability of observing y = 0?"
    options:
      - text: "exp(−λ)"
      - text: "p"
      - text: "(1 − p) exp(−λ)"
      - text: "p + (1 − p) exp(−λ): drink, OR work and finish nothing"
        correct: true
    explanation: "**p. 378:** “The probability of observing a zero is the probability that the monks didn’t drink OR (+) the probability that the monks worked AND (×) failed to finish anything.” For y > 0 the probability is (1 − p) × Poisson(y | λ). OR adds probabilities of exclusive paths; AND multiplies along a path — the garden of forking data again."
  - id: two-linear-models
    prompt: "How many linear models does a zero-inflated Poisson regression have?"
    options:
      - text: "Two: logit(pᵢ) for the zero process and log(λᵢ) for the count process, each with its own parameters and predictors"
        correct: true
      - text: "One"
      - text: "Three"
      - text: "None"
    explanation: "**p. 378:** “Notice that there are two linear models and two link functions, one for each process in the ZIPoisson. The parameters of the linear models differ, because any predictor such as x may be associated differently with each part of the mixture.” Deploy-day effects might change whether a pipeline runs at all, separately from how many failures occur when it runs."
  - id: zi-prior
    prompt: "Why does the book nudge the prior for the drinking probability below 0.5 (ap ~ Normal(−1.5, 1))?"
    options:
      - text: "To force the answer"
      - text: "Because domain knowledge says monks probably don't drink more often than not; this is a weakly informative prior on the logit scale"
        correct: true
      - text: "Because logit priors must be negative"
      - text: "Because the data contain no zeros"
    explanation: "**p. 379:** “I’m also going to nudge the prior for the probability of drinking so that there is more mass below 0.5 than above it—the monks probably do not drink more often than not.” Mixture components are weakly identified when they can explain the same data, so sensible priors help separate them."
  - id: log1m
    prompt: "Why does Stan code for mixtures use functions like `log1m(p)` and `log_mix` rather than `log(1 - p)` and direct sums?"
    options:
      - text: "They are shorter to type"
      - text: "They are required for MCMC to start"
      - text: "They avoid catastrophic rounding when p is near 0 or 1 and keep log-scale computations numerically stable"
        correct: true
      - text: "They give different answers on purpose"
    explanation: "**p. 380:** “The log1m function computes the log of one-minus a value. We need log(1 − p), but if p is very close to 1, then this can round catastrophically to zero.” And `log_mix` is “more stable under extreme values of p.” Standard numerical hygiene — like `Math.Log1P` or log-sum-exp in C#."
  - id: zero-inflation-devops
    prompt: "A dashboard counts customer support tickets per account per month. Many accounts show zero. Some never use the product; others use it and simply had no issues. Which model fits?"
    options:
      - text: "Plain Poisson, since tickets are counts"
      - text: "Gaussian on ticket counts"
      - text: "Binomial on 'any tickets'"
      - text: "Zero-inflated Poisson (or gamma-Poisson), with one linear model for the probability an account is inactive and another for the ticket rate among active accounts"
        correct: true
    explanation: "Two processes produce zeros: inactivity (structural zeros) and active accounts with no problems (sampling zeros). A plain Poisson underestimates zeros and distorts the rate. Separate linear models let product usage predict activity while feature adoption predicts ticket rate."
---

Two processes, one observed outcome: zeros that come from different sources.

## Learning objectives

- Explain why mixed zero-generating processes need a mixture likelihood
- Write the zero-inflated Poisson probabilities for y = 0 and y > 0
- Use two linear models and sensible priors
- Recognize numerically stable log-scale code

## In a nutshell

```text
Pr(0 | p, λ) = p + (1 − p) e^(−λ)
Pr(y | p, λ) = (1 − p) λ^y e^(−λ) / y!     for y > 0

yᵢ ~ ZIPoisson(pᵢ, λᵢ)
logit(pᵢ) = αp + βp xᵢ
log(λᵢ)   = αλ + βλ xᵢ
```

```csharp
static double LogZipZero(double p, double lambda) =>
    Math.Log(p + (1 - p) * Math.Exp(-lambda));
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 12.2, pp. 376–380; Figure 12.3.
- Lambert D. Zero-inflated Poisson regression, with an application to defects in manufacturing. *Technometrics* 1992.
