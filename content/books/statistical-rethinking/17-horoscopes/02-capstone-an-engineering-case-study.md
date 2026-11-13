---
title: "Capstone: Rethinking an Engineering Decision"
questions:
  - id: cs-question-type
    prompt: "Your VP asks: 'Did the new connection-pooling library reduce checkout latency, and should we roll it out to every service?' Which kind of question is this, in the book's terms?"
    options:
      - text: "A pure forecasting question; cross-validation alone will answer it"
      - text: "A question that only a p-value can answer"
      - text: "A descriptive question with no causal content"
      - text: "A causal (interventional) question: you need to know what happens if you change the library, so you need a causal model, not just a well-predicting one"
        correct: true
    explanation: "Chapter 1 distinguishes forecasting from predicting the consequences of intervention: “Successful prediction does not require correct causal identification” (p. 16). Rolling out a library is an intervention, so the analysis needs a DAG and an adjustment strategy (Chapters 5–6). Model comparison by WAIC or PSIS can help with prediction, but Chapter 7 warns against letting it “select our meal” for causal questions (p. 226)."
  - id: cs-dag
    prompt: "The library was adopted first by teams with newer services, which also have lower baseline latency. Traffic volume affects both latency and whether a team prioritized the migration. What should you condition on to estimate the library's effect?"
    options:
      - text: "The variables that close backdoor paths — service age and traffic — while avoiding post-treatment variables such as connection count after the change"
        correct: true
      - text: "Every metric available, to be safe"
      - text: "Nothing; compare adopters to non-adopters"
      - text: "Only post-change CPU usage"
    explanation: "Service age and traffic are common causes of adoption and latency (forks), so they open backdoors that must be closed. Connection count after the change is a consequence of the library (post-treatment); conditioning on it would block part of the effect (Chapter 6.2). Throwing in everything risks colliders (Chapter 6.3) — causal salad."
  - id: cs-clusters
    prompt: "You have latency samples from 180 services, some with millions of requests and some with a few hundred. How should per-service effects be estimated?"
    options:
      - text: "Separately per service, with no sharing"
      - text: "With a multilevel model: varying intercepts and varying slopes for the library effect by service, partially pooled through a fleet distribution"
        correct: true
      - text: "One effect for all services"
      - text: "By averaging per-service averages"
    explanation: "Chapters 13–14: partial pooling gives better estimates on average than no pooling (overfit for small services) or complete pooling (ignores real differences). Varying slopes estimate the *distribution* of effects — “The average treatment effect is not really as interesting as the distribution of treatment effects” when only some units benefit (p. 453). Averaging averages discards variation and manufactures false confidence (p. 15)."
  - id: cs-likelihood
    prompt: "Latency is positive, right-skewed, and has occasional huge spikes. Which likelihood choice fits the book's advice?"
    options:
      - text: "A Gaussian on raw milliseconds, because of the central limit theorem"
      - text: "Choose by gazing at the histogram of all latencies combined"
      - text: "Model log-latency (multiplicative effects), and consider a thick-tailed likelihood such as Student-t so spikes inform without dominating; check with posterior predictive simulation"
        correct: true
      - text: "Delete the spikes before modeling"
    explanation: "Chapter 4: multiplicative processes are Gaussian on the log scale. Chapter 7: “Many natural (and unnatural) processes have much heavier tails” and Student-t robust regression reduces the influence of extremes without deleting them. Chapter 10 warns against Histomancy: the likelihood describes residuals conditional on predictors, not the raw aggregate histogram (p. 314)."
  - id: cs-priors
    prompt: "How should you set the prior for the library's effect on log-latency?"
    options:
      - text: "Flat, to be objective"
      - text: "From the observed effect in the data"
      - text: "Choose whichever prior yields the desired result"
      - text: "From pre-data knowledge — e.g. Normal(0, 0.2) on the log scale, meaning effects beyond about ±40% are unlikely — and checked with prior predictive simulation"
        correct: true
    explanation: "Chapters 4 and 11: priors should come from what you know before the data and be checked on the outcome scale. Flat priors overfit and can imply absurd effects. Choosing priors from the sample, or to get a desired result, is p-hacking by another name (p. 97). exp(±0.4) ≈ 0.67–1.49 gives an interpretable plausible range."
  - id: cs-missing
    prompt: "Latency samples are missing for some services during the rollout window because their metrics agent crashed under high load. What should you do?"
    options:
      - text: "Recognize missingness that depends on load (and possibly on latency itself), add it to the DAG, model or impute accordingly, and report how much was missing"
        correct: true
      - text: "Fill them with the service's mean latency"
      - text: "Drop those windows and proceed"
      - text: "Assume they were zero"
    explanation: "Chapter 15: missing data are data; static replacement is “never warranted” (p. 499). If missingness depends on load, which you observe, modelling load can rescue inference; if it depends on latency itself (the agent crashes when latency is high), the observed data are biased and only a missingness model helps. Always report counts of missing values and their handling (p. 516)."
  - id: cs-mcmc
    prompt: "Your multilevel model reports 87 divergent transitions and R̂ up to 1.04. What is the most appropriate first response?"
    options:
      - text: "Report the results; a few divergences don't matter"
      - text: "Treat it as a model problem: try a non-centered parameterization for the varying effects, check priors on the σ parameters, and rerun with multiple chains until diagnostics are clean"
        correct: true
      - text: "Increase iterations tenfold and hope"
      - text: "Switch to a frequentist test"
    explanation: "Chapter 9: Gelman's folk theorem says computational problems usually signal a problem with the model (p. 293). Chapter 13: divergences in multilevel models usually come from funnel geometry, fixed by non-centered priors. Do not report from unconverged chains."
  - id: cs-report
    prompt: "The posterior for the fleet-average effect is −8% latency with an 89% interval of −14% to −2%; σ for the per-service effect is large, and some services show near-zero benefit. What should the recommendation emphasize?"
    options:
      - text: "'Significant improvement; roll out everywhere'"
      - text: "'Not proven; do nothing'"
      - text: "The expected benefit and its uncertainty, the spread of effects across services (who benefits most and plausibly why), predictions for a new service from the fleet distribution, and the costs and benefits of rollout"
        correct: true
      - text: "Only the point estimate −8%"
    explanation: "Chapter 3: report the posterior and use loss functions for decisions; point estimates and thresholds discard information. Chapter 13.5: predictions for new clusters should use the population distribution, not an 'average service'. Chapter 14: the pattern of variation in slopes can hint at omitted variables. Chapter 17: publish the plan, data and scripts; the deck is just the advertisement."
  - id: cs-simulation
    prompt: "Before running any of this on production data, which step does the whole book recommend?"
    options:
      - text: "Skip straight to fitting"
      - text: "Ask a statistician to approve the p-value threshold"
      - text: "Run a t-test as a sanity check"
      - text: "Simulate data from your assumed generative model (including confounds and missingness), fit your analysis to it, and confirm it recovers the effects you planted"
        correct: true
    explanation: "From Chapter 3's software validation (p. 61) to Chapter 6's “If a procedure cannot figure out the truth in a simulated example, we shouldn’t trust it in a real one” (p. 177) and Chapter 15's workflow (p. 499): simulate first. It is the statistical equivalent of a test suite — and the most transferable habit from this book to engineering work."
---

A companion-only section that strings the whole book together in one realistic engineering decision.

## Learning objectives

- Classify a question as forecasting or causal
- Build a DAG, choose an adjustment set and avoid post-treatment conditioning
- Choose likelihood, priors and multilevel structure for fleet telemetry
- Handle missing data and sampling diagnostics
- Report results as distributions that support a decision

## The workflow on one page

| Step | Book chapter | What you do |
|---|---|---|
| Question | 1 | Forecast or intervention? |
| Causal model | 5, 6 | DAG, backdoor paths, adjustment set |
| Generative simulation | 3, 6, 15 | Fake data with planted effects |
| Likelihood | 4, 7, 10–12 | Scale, tails, counts, mixtures |
| Priors | 4, 7, 11 | Pre-data knowledge, prior predictive checks |
| Structure | 13, 14 | Varying intercepts and slopes, partial pooling |
| Missingness and error | 15 | Model it, impute, report |
| Computation | 9, 13 | HMC diagnostics, non-centered forms |
| Comparison | 7 | PSIS/WAIC for prediction, not causation |
| Reporting | 3, 13, 17 | Distributions, new-cluster predictions, open code |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapters 1–17 as cited in each explanation.
- Gelman A et al. Bayesian workflow. arXiv:2011.01808, 2020.
- Kohavi R, Tang D, Xu Y. *Trustworthy Online Controlled Experiments*. Cambridge University Press, 2020.
