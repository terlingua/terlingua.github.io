---
title: "11.2 Poisson Regression"
questions:
  - id: poisson-from-binomial
    prompt: "A monastery has 1,000 monks, each finishing a manuscript on a given day with probability 0.001. Why is the daily count approximately Poisson?"
    options:
      - text: "Because monks are not independent"
      - text: "Because a binomial with huge N and tiny p has mean ≈ variance ≈ Np; that special shape is the Poisson, useful when N is unknown or effectively unbounded"
        correct: true
      - text: "Because manuscripts are continuous"
      - text: "Because the Poisson has two parameters"
    explanation: "**p. 346:** simulating gives mean ≈ variance ≈ 1. “This special shape is known as the Poisson distribution, and it is useful because it allows us to model binomial events for which the number of trials N is unknown or uncountably large.” Requests per second from millions of users, errors per hour, pages per day — all counts with no meaningful upper bound."
  - id: one-parameter
    prompt: "What does the Poisson's single parameter λ represent?"
    options:
      - text: "Only the variance"
      - text: "The maximum count"
      - text: "Both the expected value and the variance of the counts"
        correct: true
      - text: "The number of trials"
    explanation: "**p. 346:** “The parameter λ is the expected value of the outcome y. It is also the expected variance of the counts y.” This is a strong assumption: if real counts vary more than their mean (overdispersion, common in incident data), the Poisson is overconfident. Chapter 12 introduces the gamma-Poisson for that case."
  - id: log-link-poisson
    prompt: "What is the conventional link for a Poisson GLM?"
    options:
      - text: "logit"
      - text: "identity"
      - text: "probit"
      - text: "log: log(λᵢ) = α + βxᵢ, which keeps λ positive and makes effects multiplicative"
        correct: true
    explanation: "The log link ensures λ > 0 (p. 346). A unit change in x multiplies the expected count by exp(β). Remember the warning from Chapter 10 about extrapolating exponential relationships."
  - id: flat-poisson-prior
    prompt: "With a log link, what does a Normal(0, 10) prior on the intercept imply for the expected count?"
    options:
      - text: "Absurdly large expected counts: exp of a Normal(0, 10) is log-normal with mean exp(50), trillions of tools"
        correct: true
      - text: "A sensible range of 0 to 20 tools"
      - text: "Negative counts"
      - text: "Exactly 10 tools"
    explanation: "**p. 349:** “a log-normal distribution is exp(µ + σ²/2), which evaluates to exp(50), which is impossibly large.” Simulating gives a mean of about 9.6 × 10¹². The book uses Normal(3, 0.5), centred around e³ ≈ 20 tools. Slope priors need similar care (Figure 11.8). Exponentiating makes wide priors explode."
  - id: log-population
    prompt: "Why does the Oceanic tools model use log population as the predictor?"
    options:
      - text: "To avoid negative numbers"
      - text: "Because theory says the order of magnitude of population matters, and with a log link this implies diminishing returns of raw population"
        correct: true
      - text: "Because population is Poisson"
      - text: "Because the data are categorical"
    explanation: "**p. 348:** “Because that’s what the theory says, that it is the order of magnitude of the population that matters, not the absolute size of it.” And p. 351: “When a predictor variable is itself logged, this means we are assuming diminishing returns for the raw variable.” Like caching gains with more memory: the first gigabyte matters far more than the hundredth."
  - id: hawaii
    prompt: "Hawaii has a high Pareto k in the tools model. What does this suggest?"
    options:
      - text: "Hawaii should be deleted"
      - text: "Hawaii has missing data"
      - text: "Hawaii is highly influential — it has an extreme population — so the fit depends heavily on it; robust or more theory-driven models are worth considering"
        correct: true
      - text: "PSIS is wrong"
    explanation: "Figure 11.9 scales point size by Pareto k; Hawaii (k ≈ 1) dominates. The book notes the model even predicts that high-contact societies have fewer tools than low-contact ones at very large populations — impossible on theory grounds — because the intercept is free and the trend need not pass through the origin (p. 354–355)."
  - id: scientific-model
    prompt: "The book replaces the linear model with λ = α P^β / γ from a model of tool innovation and loss. What advantages does this have?"
    type: multiple
    options:
      - text: "Parameters have meanings outside a linear model (innovation rate, diminishing returns, loss rate)"
        correct: true
      - text: "Predictions pass through the origin: zero people, zero tools"
        correct: true
      - text: "No link function is needed; positivity comes from positive parameters"
        correct: true
      - text: "It guarantees a better WAIC"
    explanation: "From the box “Modeling tool innovation” (p. 356): setting ΔT = αP^β − γT to zero gives the equilibrium T̂ = αP^β/γ, used directly as λ. “Notice that there is no link function! All we have to do to ensure that λ remains positive is to make sure the parameters are positive.” Figure 11.10's caption notes that because the model forces trends through the origin, its behavior is more sensible, and its parameters mean something outside a linear model. Little's law and queueing formulas play the same role for latency models."
  - id: exposure-offset
    prompt: "One monastery reports daily counts, another weekly. How does a Poisson model handle the different exposures?"
    options:
      - text: "Divide weekly counts by 7 and round"
      - text: "Fit separate models"
      - text: "Use a binomial instead"
      - text: "Add log(exposure) to the linear model as an offset with no parameter: log λᵢ = log τᵢ + α + βxᵢ"
        correct: true
    explanation: "**p. 357:** “Implicitly, λ is equal to an expected number of events, µ, per unit time or distance, τ.” So log λᵢ = log(µᵢ/τᵢ) → log µᵢ = log τᵢ + α + βxᵢ. The offset is data, not a parameter. This is how you compare error counts from services observed for different durations, or incidents per thousand deploys: put log(duration) or log(deploys) in the model. Never divide counts to make rates and then use a Gaussian."
  - id: monastery-result
    prompt: "The comparison of the two monasteries finds the new one produces about 0.5 manuscripts per day versus about 1.3. How was this possible with only weekly records for the new one?"
    options:
      - text: "The offset puts both sources on the same per-day scale inside one model, so their rates are directly comparable"
        correct: true
      - text: "By interpolating daily counts"
      - text: "By assuming the new monastery is identical"
      - text: "It was not possible"
    explanation: "With log(days) as an offset and an indicator for monastery, the model estimates per-day rates for both (p. 358–359): “The new monastery produces about half as many manuscripts per day. So you aren’t going to pay that much for it.” Telemetry with mixed scrape intervals or reporting windows can be modelled the same way."
---

Counts without a known maximum: the Poisson GLM, priors through a log link, scientific models, and
exposures.

## Learning objectives

- Derive the Poisson as a limiting binomial and interpret λ
- Set priors through the log link with prior predictive simulation
- Interpret log-population predictors as diminishing returns
- Use exposure offsets for counts over different time spans

## In a nutshell

```text
Tᵢ ~ Poisson(λᵢ)
log λᵢ = α[cid] + β[cid] · log Pᵢ (std)
α ~ Normal(3, 0.5);  β ~ Normal(0, 0.2)

Scientific alternative:  λᵢ = α Pᵢ^β / γ     (no link)

Exposure:  log λᵢ = log τᵢ + α + β xᵢ
```

## For engineers

| Count | Exposure offset |
|---|---|
| Errors per service | log(requests) or log(hours observed) |
| Incidents per team | log(deploys) |
| Pages per on-call shift | log(shift length) |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 11.2, pp. 345–359; Figures 11.6–11.10.
- Kline MA, Boyd R. Population size predicts technological complexity in Oceania. *Proceedings of the Royal Society B* 2010. [doi:10.1098/rspb.2010.0452](https://doi.org/10.1098/rspb.2010.0452)
