---
title: "3.3 Sampling to Simulate Prediction"
questions:
  - id: reasons-to-simulate
    prompt: "Which of these are reasons the book gives for simulating observations from a model?"
    type: multiple
    options:
      - text: "Model design: sample from the prior to see what the model expects before data"
        correct: true
      - text: "Model checking: compare simulated data from the fitted model with the real data"
        correct: true
      - text: "Software validation: simulate data with known parameters and check the fitting code recovers them"
        correct: true
      - text: "Research design and forecasting"
        correct: true
      - text: "Replacing the need for real data"
    explanation: "**p. 61** lists: “(1) Model design ... (2) Model checking ... (3) Software validation. In order to be sure that our model fitting software is working, it helps to simulate observations under a known model and then attempt to recover the values of the parameters the data were simulated under. (4) Research design ... (5) Forecasting.” Point 3 is unit testing for statistics: generate data with known truth, run the estimator, assert it recovers the truth within tolerance. Every model in production analytics should have this test."
  - id: generative
    prompt: "What does the book mean by saying likelihood functions *work in both directions*?"
    options:
      - text: "They can be computed forwards or backwards in time"
      - text: "Given an observation they say how plausible it is, and given parameters they define a distribution you can sample to simulate observations"
        correct: true
      - text: "They can be used as priors or posteriors"
      - text: "They are symmetric in p and 1 − p"
    explanation: "**p. 62:** “Given a realized observation, the likelihood function says how plausible the observation is. And given only the parameters, the likelihood defines a distribution of possible observations that we can sample from, to simulate observation. In this way, Bayesian models are always generative, capable of simulating predictions.” For developers this is the most practical property in the book: every model is also a synthetic-data generator. You can build fake telemetry with realistic noise from the very model you use to analyse real telemetry."
  - id: dummy-data
    prompt: "With p = 0.7 and N = 2 tosses, what are the probabilities of 0, 1 and 2 waters?"
    options:
      - text: "0.33, 0.33, 0.33"
      - text: "0.30, 0.40, 0.30"
      - text: "0.09, 0.42, 0.49"
        correct: true
      - text: "0.49, 0.42, 0.09"
    explanation: "Binomial: P(0) = 0.3² = 0.09, P(1) = 2 × 0.7 × 0.3 = 0.42, P(2) = 0.7² = 0.49 (p. 62). Simulating 100,000 draws with `rbinom` reproduces these frequencies to about two decimals. The book calls such simulated data **dummy data**, “to indicate that it is a stand-in for actual data.” Checking that simulated frequencies match theoretical probabilities is exactly the kind of sanity test you would write for a random generator."
  - id: two-uncertainties
    prompt: "When predicting the next 9 tosses, which two sources of uncertainty must the prediction include?"
    options:
      - text: "Prior uncertainty and likelihood uncertainty"
      - text: "Measurement error and rounding error"
      - text: "Bias and variance"
      - text: "Observation uncertainty (sampling variation even if p were known) and uncertainty about p itself"
        correct: true
    explanation: "Even with p known exactly, the number of waters varies from sample to sample — that is observation uncertainty. On top of that, p is uncertain, as described by the posterior. The **posterior predictive distribution** combines both: for each posterior sample of p, simulate data, and pool the results. The caption of Figure 3.6 says it “propagates uncertainty about parameter to uncertainty about prediction” (p. 65). A capacity forecast that plugs in the single best-fit growth rate ignores the second source and is overconfident."
  - id: ppd-code
    prompt: "How do you simulate the posterior predictive distribution for 9 tosses from posterior samples of p?"
    options:
      - text: "Simulate one binomial count of 9 tosses for each posterior sample of p, using that sample's value"
        correct: true
      - text: "Simulate all counts using the posterior mean of p"
      - text: "Simulate all counts using the MAP"
      - text: "Use the prior to simulate counts"
    explanation: "In R: `w <- rbinom(1e4, size=9, prob=samples)` — R vectorizes over `samples`, so each simulated count uses a different p (p. 66). In C#: `samples.Select(p => Binomial(9, p, rng))`. Using only the mean of p would collapse parameter uncertainty and make predictions too narrow. This pattern — loop over posterior draws, simulate data per draw — is how every posterior prediction in the book is made."
  - id: overconfidence
    prompt: "Compared with simulating from the single value p = 0.6, what does the posterior predictive distribution look like?"
    options:
      - text: "Identical"
      - text: "Wider, because it also includes uncertainty about p"
        correct: true
      - text: "Narrower"
      - text: "Shifted toward zero"
    explanation: "Averaging sampling distributions over many plausible values of p spreads the predictions out. The book warns that using a point estimate to make predictions will be overconfident (p. 66). An SRE equivalent: forecasting next month's incident count using last month's exact rate, rather than a distribution over plausible rates, will underestimate the chance of a bad month."
  - id: other-views
    prompt: "The observed tosses W L W W W L W L W have 6 switches between W and L. Simulated data from the model usually have fewer switches. What does this suggest?"
    options:
      - text: "The model is perfect"
      - text: "The globe has more water than estimated"
      - text: "The tosses may not be independent — they appear negatively correlated — which the binomial model cannot represent"
        correct: true
      - text: "The prior was too narrow"
    explanation: "The model was built to explain the *count* of W, so it naturally fits that. Checking other aspects of the data — run lengths and switches (Figure 3.7, p. 67) — tests assumptions the count hides. **p. 67:** the data are “consistent with lack of independence between tosses of the globe, in which each toss is negatively correlated with the last.” This is like testing a load model not just on total requests but on burstiness: a Poisson model can match the daily total and badly miss the autocorrelation."
  - id: misspecified-ok
    prompt: "Does the switch-count mismatch mean the globe model should be abandoned?"
    options:
      - text: "Yes; any detected misfit invalidates a model"
      - text: "Yes, because the posterior mean is wrong"
      - text: "No; misfits never matter"
      - text: "It depends on your purpose: all models are misspecified, and here the main consequence is that each toss carries less information than the model assumes"
        correct: true
    explanation: "**p. 67:** “The model will always be wrong in some sense, be mis-specified. But whether or not the mis-specification should lead us to try other models will depend upon our specific interests.” Negatively correlated tosses mean each toss is less informative, so the posterior is too narrow, though in the long run it still converges near the true proportion. Same judgment as a flaky test: you fix it if the flakiness affects decisions, not because it is imperfect."
  - id: posterior-predictive-check
    prompt: "A model of daily login failures fits the total well, but its posterior predictive simulations never produce the long streaks of failure-free days you actually see. What is the most appropriate next step?"
    options:
      - text: "Treat it as evidence of a structure the model lacks (for example clustering or zero-inflation) and consider a model that can produce it"
        correct: true
      - text: "Tighten the prior"
      - text: "Report a p-value for the mismatch and stop"
      - text: "Ignore it; the total fits"
    explanation: "Posterior predictive checks look for aspects of the data the model cannot reproduce. Long failure-free streaks with occasional bursts suggest correlated or zero-inflated processes (Chapter 12) or regime changes. The book's approach is not to 'test' the model into rejection but to learn where it fails and improve it, guided by purpose. **p. 32:** “the objective is to check the model’s adequacy for some purpose.”"
---

Bayesian models are generative. Simulating from them lets you design priors, check fits, test code and
forecast.

## Learning objectives

- List the uses of simulation from a model
- Generate dummy data from a likelihood
- Build a posterior predictive distribution and explain why it is wider than plug-in predictions
- Use alternative summaries of data to check model assumptions

## In a nutshell

```text
for each posterior sample p_i:
    simulate y_i ~ Binomial(9, p_i)
the collection {y_i} is the posterior predictive distribution
```

| Prediction method | Includes sampling noise | Includes parameter uncertainty |
|---|---|---|
| Plug in MAP or mean | Yes | No (overconfident) |
| Posterior predictive | Yes | Yes |

## A simulation-based test

```csharp
var rng = new Random(42);
var trueP = 0.7;
var data = Enumerable.Range(0, 1000).Count(_ => rng.NextDouble() < trueP);
var estimate = FitPosteriorMean(data, 1000);
Assert.InRange(estimate, 0.65, 0.75);
```

Recovering known parameters from simulated data is the book's software validation step.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 3.3, pp. 61–68; Figures 3.5–3.7.
- Gabry J et al. Visualization in Bayesian workflow. *JRSS A* 2019. [doi:10.1111/rssa.12378](https://doi.org/10.1111/rssa.12378)
- Talts S et al. Validating Bayesian inference algorithms with simulation-based calibration. arXiv:1804.06788, 2018.
