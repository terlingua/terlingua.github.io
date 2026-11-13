---
title: "10.2 Generalized Linear Models"
questions:
  - id: glm-definition
    prompt: "What is a generalized linear model (GLM)?"
    options:
      - text: "A linear regression with more predictors"
      - text: "A neural network with one layer"
      - text: "A model that replaces a parameter of any likelihood (not just the Gaussian mean) with a linear model, usually through a link function"
        correct: true
      - text: "A model without priors"
    explanation: "**p. 300:** a GLM “is a model that replaces a parameter of a likelihood function with a linear model. But GLMs need not use Gaussian likelihoods. Any likelihood function can be used, and linear models can be attached to any or all of the parameters that describe its shape.” Maximum entropy chooses the likelihood from the outcome's constraints; the linear model describes how its parameter varies."
  - id: why-link
    prompt: "Why are link functions needed (Figure 10.5)?"
    options:
      - text: "To make models run faster"
      - text: "To make outcomes Gaussian"
      - text: "To remove the need for priors"
      - text: "Because a linear model is unbounded, but parameters like probabilities (0 to 1) or rates (positive) are constrained; the link maps the linear model onto the parameter's valid range"
        correct: true
    explanation: "**p. 313–314:** a linear model for a probability “will happily continue upwards” past 1, and a linear model of a count “would happily predict negative values.” “The link function f provides a solution to this common problem.” Just as you clamp or transform a computed value into a valid range before using it — except here the transform is part of the model."
  - id: histomancy
    prompt: "What is *Histomancy*, and why is it a false god?"
    options:
      - text: "Choosing a likelihood by gazing at the histogram of the raw outcome; the likelihood's assumptions are about residuals after conditioning on predictors, not about the aggregate histogram"
        correct: true
      - text: "Plotting too many histograms"
      - text: "Using histograms instead of density plots"
      - text: "Using priors from historical data"
    explanation: "**p. 314:** “Histomancy, the ancient art of divining likelihood functions from empirical histograms.” But “at most what a Gaussian likelihood assumes is not that the aggregated data look Gaussian, but rather that the residuals, after fitting the model, look Gaussian.” Combined male and female weights are not Gaussian, but conditional on sex may be. Similarly, latency across all endpoints may look wildly non-normal, while per-endpoint latency is well behaved."
  - id: exponential-family
    prompt: "Which statements about the exponential family match the chapter?"
    type: multiple
    options:
      - text: "Every member is a maximum entropy distribution for some set of constraints"
        correct: true
      - text: "The exponential distribution is the core of event-history analysis, for non-negative durations with a constant rate"
        correct: true
      - text: "The gamma distribution arises for waiting times when an event requires two or more exponentially distributed events to happen"
        correct: true
      - text: "You must only use exponential-family distributions"
    explanation: "**p. 314:** “Every member of this family is a maximum entropy distribution, for some set of constraints.” **p. 315:** the gamma: “If an event can only happen after two or more exponentially distributed events happen, the resulting waiting times will be gamma distributed.” **p. 316:** “It’s also not important that an outcome distribution be a member of the exponential family—if you think you have good reasons to use some other distribution, then use it.” Time-to-failure (exponential), time to complete a multi-step job (gamma), events per minute (Poisson)."
  - id: likelihood-is-prior
    prompt: "In what sense does the book say “A likelihood is a prior”?"
    options:
      - text: "Likelihoods and priors are always the same distribution"
      - text: "Likelihoods are priors for the data, conditional on parameters; like priors there is no single correct likelihood, only better and worse ones"
        correct: true
      - text: "Likelihoods should be chosen after seeing the data"
      - text: "Priors are irrelevant"
    explanation: "From the box on p. 316: “In Bayesian statistics, likelihoods are deeply related to prior probability distributions: They are priors for the data, conditional on the parameters. And just like with other priors, there is no correct likelihood. But there are better and worse likelihoods”. The 'objective likelihood, subjective prior' split is not real: both are assumptions to be checked."
  - id: logit
    prompt: "What does the logit link do?"
    options:
      - text: "Rounds probabilities to 0 or 1"
      - text: "Maps a positive value to its logarithm"
      - text: "Maps a probability p to log-odds log(p / (1 − p)); its inverse, the logistic function, maps any linear model value back to a probability between 0 and 1"
        correct: true
      - text: "Squares the linear model"
    explanation: "logit(pᵢ) = α + βxᵢ, so pᵢ = exp(α + βxᵢ) / (1 + exp(α + βxᵢ)) — the logistic or inverse-logit (p. 317). Figure 10.7 shows how equal steps in log-odds become smaller and smaller steps in probability near 0 and 1. The log-odds of 0 is p = 0.5; ±4 is roughly 0.02 and 0.98."
  - id: compression
    prompt: "With a logit link, why does a fixed change in a predictor not produce a fixed change in probability?"
    options:
      - text: "Because of a coding error"
      - text: "Because probabilities are always linear in x"
      - text: "Because β changes with x"
      - text: "Because the logistic function compresses changes near 0 and 1: when an event is already very likely, even a strong predictor cannot raise its probability much"
        correct: true
    explanation: "**p. 318:** “The key lesson for now is just that no regression coefficient, such as β, from a GLM ever produces a constant change on the outcome scale ... every predictor essentially interacts with itself ... every predictor variable effectively interacts with every other predictor variable.” Improving a 99%-reliable deploy pipeline by the same log-odds amount as a 50% one yields a tiny change in probability. Report effects on the outcome scale at realistic baseline values."
  - id: log-link
    prompt: "What does the log link assume, and what is its danger?"
    options:
      - text: "It keeps the parameter positive by assuming exponential scaling with predictors; extrapolated far beyond the data, exponential growth becomes absurd"
        correct: true
      - text: "It assumes a linear relationship; no danger"
      - text: "It assumes probabilities; danger of exceeding 1"
      - text: "It assumes the outcome is categorical"
    explanation: "log(λᵢ) = α + βxᵢ means λᵢ = exp(α + βxᵢ): a unit change on the log scale is a multiplicative change on the outcome scale (p. 318–319). **p. 319:** “Just like a linear model cannot be linear forever, an exponential model cannot be exponential forever.” Exponential growth of incidents with traffic may hold in the observed range; extrapolating to 100× traffic predicts absurdities."
  - id: glm-omitted
    prompt: "Why can omitted variables bias GLMs even when they are not confounders?"
    options:
      - text: "They cannot"
      - text: "Because of ceiling and floor effects: if an unmeasured cause Z is sufficient to make Y = 1, cases with small X but Y = 1 make X look weaker than it is"
        correct: true
      - text: "Because GLMs have no intercept"
      - text: "Because the link function removes variables"
    explanation: "**p. 320:** “even a variable that isn’t technically a confounder can bias inference, once we have a link function. The reason is that the ceiling and floor effects described above can distort estimates by suppressing the causal influence of a variable.” If either a bad config (X) or a network partition (Z) is sufficient for an outage, ignoring partitions makes bad configs look less dangerous than they are."
  - id: sensitivity-analysis
    prompt: "How does sensitivity analysis differ from p-hacking, according to the box “When in doubt, play with assumptions”?"
    options:
      - text: "They are the same"
      - text: "Sensitivity analysis is only for frequentists"
      - text: "Sensitivity analysis explores how alternative justifiable assumptions change inference and reports all of them; p-hacking tries many analyses and reports the one that attains significance"
        correct: true
      - text: "Sensitivity analysis picks the best link function by WAIC"
    explanation: "**p. 319:** “A sensitivity analysis explores how changes in assumptions influence inference. If none of the alternative assumptions you consider have much impact on inference, that’s worth reporting. Likewise, if the alternatives you consider do have an important impact on inference, that’s also worth reporting ... The goal of sensitivity analysis is really the opposite of p-hacking.” Report robustness, don't cherry-pick."
---

GLMs extend the linear model to any likelihood, using link functions to keep parameters in range.

## Learning objectives

- Define a GLM and the role of the link function
- Avoid choosing likelihoods by Histomancy
- Read the logit and log links and their compression effects
- Recognize GLM-specific omitted variable bias and use sensitivity analysis

## In a nutshell

| Outcome | Likelihood | Parameter | Link | Inverse link |
|---|---|---|---|---|
| Real number | Normal | μ | identity | identity |
| Successes out of n | Binomial | p ∈ (0, 1) | logit | logistic |
| Count, no upper bound | Poisson | λ > 0 | log | exp |
| Positive duration | Exponential/Gamma | rate > 0 | log | exp |

```csharp
static double Logistic(double x) => 1.0 / (1.0 + Math.Exp(-x));
static double Logit(double p) => Math.Log(p / (1 - p));
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 10.2, pp. 312–321; Figures 10.5–10.8.
- Nelder JA, Wedderburn RWM. Generalized linear models. *JRSS A* 1972.
- McCullagh P, Nelder JA. *Generalized Linear Models*, 2nd ed. Chapman & Hall, 1989.
