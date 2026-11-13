---
title: "2.3 Components of the Model"
questions:
  - id: three-components
    prompt: "Which three things does the book say a model needs, in counting terms, for the globe example?"
    options:
      - text: "A mean, a variance and a p-value"
      - text: "A number of ways each conjecture could produce an observation, an accumulated number of ways for the whole data, and an initial plausibility for each conjecture"
        correct: true
      - text: "A training set, a test set and a loss function"
      - text: "Data, code and documentation"
    explanation: "These map onto the formal pieces: ways per observation and accumulated ways → the **likelihood**; initial plausibility → the **prior**; and their normalized product → the **posterior** (p. 32). Every Bayesian model in the book, however complex, has these parts. When you read a later model with dozens of lines, ask of each line: is this describing how data arise (likelihood), what I believed before (prior), or a deterministic relationship?"
  - id: variables
    prompt: "In the globe model, which of these are *variables* in the book's sense?"
    type: multiple
    options:
      - text: "The count of water observations W"
        correct: true
      - text: "The count of land observations L"
        correct: true
      - text: "The proportion of water p"
        correct: true
      - text: "The name of the person tossing the globe"
    explanation: "The book calls anything you want to infer or that you observe a variable. W and L are observed variables (data); p is an unobserved variable (a parameter). The total N = W + L is also known. Treating parameters and data as the same kind of thing — just observed or not — is a deeply Bayesian move that pays off in Chapter 15, where missing data become parameters too. A missing metric value and an unknown error rate are the same kind of object: something not observed that the model must reason about."
  - id: binomial
    prompt: "Why is the binomial distribution the right likelihood for the globe data?"
    options:
      - text: "Because it is the default in R"
      - text: "Because the data are continuous"
      - text: "Because it gives the probability of W successes in N independent trials each with the same probability p, which is exactly the data story"
        correct: true
      - text: "Because it is symmetric"
    explanation: "The binomial counts the ways to get W waters among N tosses, each independent with probability p: Pr(W, L | p) = (W + L)! / (W! L!) × p^W × (1 − p)^L. The likelihood follows from the story; it is not chosen for convenience. In software: the number of failed health checks out of N probes, each failing independently with probability p, is binomial. If probes are correlated (they hit the same overloaded node), the binomial story is wrong and Chapter 12's beta-binomial becomes relevant."
  - id: likelihood-most-influential
    prompt: "Which assumption does the book call the most influential in both Bayesian and many non-Bayesian models?"
    options:
      - text: "The prior"
      - text: "The choice of software"
      - text: "The random seed"
      - text: "The likelihood — the distributions assigned to data"
        correct: true
    explanation: "**p. 34:** “the most influential assumptions in both Bayesian and many non-Bayesian models are the distributions assigned to data, the likelihood functions. The likelihoods influence inference for every piece of data, and as sample size increases, the likelihood matters more and more.” This explains why Bayesian and classical results are often close: they usually share the likelihood. Debates about priors are often a distraction from the bigger choice, like arguing about logging verbosity while the metric being logged is wrong."
  - id: prior-as-assumption
    prompt: "How does the book say a prior should be treated?"
    options:
      - text: "As an assumption that should be interrogated like any other, by changing it and checking how sensitive inference is"
        correct: true
      - text: "As something to be avoided whenever possible"
      - text: "As a sacred belief that must never change"
      - text: "As the scientist's personal feeling about the result"
    explanation: "**p. 35:** “Because the prior is an assumption, it should be interrogated like other assumptions: by altering it and checking how sensitive inference is to the assumption. No one is required to swear an oath to the assumptions of a model, and no set of assumptions deserves our obedience.” This is sensitivity analysis, the same thing you do with a capacity model: double the assumed peak factor and see whether the conclusion survives. If it does not, the prior matters and deserves scrutiny and justification."
  - id: priors-engineering
    prompt: "Which statements about priors are consistent with this section?"
    type: multiple
    options:
      - text: "A prior can be chosen to constrain parameters to reasonable ranges, like a regularizer"
        correct: true
      - text: "Flat priors are always the most objective choice"
      - text: "Within Bayesian data analysis, priors are part of the model, like the likelihood"
        correct: true
      - text: "Non-Bayesian procedures make choices with similar effects, such as penalties or choice of estimator"
        correct: true
    explanation: "The book frames priors as engineering assumptions that help the machine learn. Flat priors are rarely best; they imply that absurd values (a conversion rate of 0.999) are as plausible as reasonable ones, and in later chapters they cause overfitting. Non-Bayesian methods have their own tuning choices — ridge penalties, which estimator to use — that play the same role. Chapter 7 shows that a regularizing prior is the Bayesian name for the regularization machine-learning engineers already use."
  - id: model-notation
    prompt: "How is the globe model written in the book's compact notation?"
    options:
      - text: "`W = Binomial(N) + p`"
      - text: "`W ~ Binomial(N, p)` and `p ~ Uniform(0, 1)`"
        correct: true
      - text: "`p ~ Binomial(W, N)`"
      - text: "`W ~ Normal(p, N)`"
    explanation: "The model is born when the likelihood and the prior are written together (p. 35–36): `W ~ Binomial(N, p)` — the observed count is binomially distributed given N and p; `p ~ Uniform(0, 1)` — every proportion is equally plausible beforehand. Read it like a type signature for a data generator: to simulate data, draw p from its prior, then draw W from the binomial. Everything in the book follows this pattern, with more lines."
  - id: uniform-density
    prompt: "A uniform prior on p over [0, 1] has density 1 everywhere. What is the density of a uniform prior over [0, 10]?"
    options:
      - text: "10"
      - text: "1"
      - text: "0.1"
        correct: true
      - text: "0"
    explanation: "From the box on p. 35: “for a uniform prior from a to b, the probability of any point in the interval is 1/(b − a).” So 1/10 = 0.1. A density is not a probability; it must integrate to 1 over its range, so wider ranges have lower density. This matters for continuous parameters later: densities can exceed 1 when ranges are narrow (a uniform on [0, 0.1] has density 10), which surprises programmers who expect every probability-like number to be at most 1."
---

The globe model has three formal components: variables, definitions of how they relate (likelihood and
priors), and the resulting model.

## Learning objectives

- Distinguish observed variables (data) from unobserved variables (parameters)
- Derive the binomial likelihood from the data story
- Explain why the likelihood is usually the most influential assumption
- Treat priors as testable engineering assumptions
- Write the globe model in the book's notation

## In a nutshell

```text
W ~ Binomial(N, p)     likelihood: how the data arise
p ~ Uniform(0, 1)      prior: plausibility of p before the data
```

```text
Pr(W, L | p) = (W + L)! / (W! L!) × p^W × (1 − p)^L
```

| Component | Globe example | A/B test analogue |
|---|---|---|
| Observed variable | W, L | Conversions, non-conversions |
| Parameter | p | Conversion rate |
| Likelihood | Binomial | Binomial |
| Prior | Uniform(0, 1) | Beta concentrated near historical rates |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 2.3, pp. 32–36.
- Gelman A, Simpson D, Betancourt M. The prior can often only be understood in the context of the likelihood. *Entropy* 2017. [doi:10.3390/e19100555](https://doi.org/10.3390/e19100555)
