---
title: "9.5 Care and Feeding of Your Markov Chain"
questions:
  - id: how-many-samples
    prompt: "What really matters when deciding how many samples to draw?"
    options:
      - text: "The raw number of iterations"
      - text: "The number of chains"
      - text: "Always exactly 10,000"
      - text: "The effective number of samples for the quantity you care about — e.g. a few hundred for a posterior mean, more for tails and extreme intervals"
        correct: true
    explanation: "**p. 287:** “what really matters is the effective number of samples, not the raw number. The effective number of samples is an estimate of the number of independent samples from the posterior distribution, in terms of estimating some function like the posterior mean.” **p. 288:** for highly skewed posteriors, “you’ll have to think more about which region of the distribution interests you.”"
  - id: warmup-length
    prompt: "How much warmup is reasonable for Stan models?"
    options:
      - text: "Typically up to half of iter; simple models may need far less, but more warmup can make sampling more efficient"
        correct: true
      - text: "Exactly 10 iterations"
      - text: "None"
      - text: "Ten times the number of samples"
    explanation: "**p. 288:** “With Stan models, typically you can devote as much as half of your total samples, the iter value, to warmup and come out very well. But for simple models like those you’ve fit so far, much less warmup is really needed.” The default ulam setting is warmup = iter/2. It is a tuning budget, like warming caches before measuring."
  - id: one-vs-many
    prompt: "After verifying that sampling works, is it better to run three chains or one long chain for the same number of post-warmup samples?"
    options:
      - text: "Three chains are always better for inference"
      - text: "It doesn't matter much for inference; one long chain can be more efficient because each chain repeats warmup"
        correct: true
      - text: "One long chain is always invalid"
      - text: "Neither; MCMC needs at least ten chains"
    explanation: "**p. 289:** “It doesn’t really matter, in terms of inference. But it might matter in efficiency, because the 3 chains duplicate warmup effect that just gets thrown away.” Same page: “Using 3 or 4 chains is often enough to reassure us that the sampling is working properly. But once you’ve verified that the sampling is working well, and you have a good idea of how many warmup samples you need, it’s perfectly safe to just run one long chain.”"
  - id: wild-chain
    prompt: "A model with two observations (−1, 1) and very flat priors (`alpha ~ dnorm(0, 1000)`, `sigma ~ dexp(0.0001)`) produces absurd estimates, tiny n_eff and divergent transitions. Why?"
    options:
      - text: "Stan has a bug"
      - text: "Two observations are not allowed"
      - text: "With little data and flat priors, the posterior has enormous, nearly flat regions; chains drift to extreme values like −30 million and the sampler cannot explore reliably"
        correct: true
      - text: "The likelihood is wrong"
    explanation: "**p. 290–291:** “The reason the model above drifts wildly in both dimensions is that there is very little data, just two observations, and flat priors. The flat priors say that every possible value of the parameter is equally plausible.” Trace plots show drifts and spikes; trank plots show chains stuck above or below each other (Figure 9.9)."
  - id: weakly-informative-fix
    prompt: "How is the wild chain cured?"
    options:
      - text: "By increasing iterations to a million"
      - text: "By removing sigma"
      - text: "By using quap instead"
      - text: "By adding weakly informative priors (e.g. alpha ~ Normal(1, 10), sigma ~ Exponential(1)); the data still dominate the posterior, but the sampler can now explore it"
        correct: true
    explanation: "**p. 292–293:** with weakly informative priors “Even with only two observations, the likelihood easily overcomes these priors. Yet the posterior cannot be successfully approximated without them.” And: “even though the priors end up getting washed out right away ... they still have a big effect on inference, by allowing us to get an answer.” Like bounding a retry delay: it rarely binds in practice, but removes pathological states."
  - id: folk-theorem
    prompt: "What is Andrew Gelman's *folk theorem of statistical computing*?"
    options:
      - text: "When you have computational problems, often there's a problem with your model"
        correct: true
      - text: "More iterations always fix convergence problems"
      - text: "Every model can be fit with MCMC"
      - text: "Flat priors are computationally optimal"
    explanation: "**p. 293:** “When you have computational problems, often theres a problem with your model. Before we begin to tune the software and pour more computer power into a problem, it can be useful to go over the model specification again, and the data itself, to make sure the problem isn’t in the pre-sampling stage.” In ops terms: when the system is slow, check the query plan before buying bigger machines."
  - id: non-identifiable
    prompt: "A model with `mu <- a1 + a2` and flat priors on a1 and a2 gives n_eff ≈ 2 and R̂ ≈ 2.8, with a1 and a2 at huge opposite values. What is the problem?"
    options:
      - text: "The data are too large"
      - text: "Only the sum a1 + a2 is identified; with flat priors the chains wander along an infinite ridge of equally good combinations. Weakly regularizing priors rescue the sampler"
        correct: true
      - text: "The sampler needs more chains"
      - text: "sigma is misspecified"
    explanation: "Section 9.5.4 (p. 293–295) is the multicollinear-legs problem again. The posterior means are about the same distance from zero in opposite directions. With `a1, a2 ~ Normal(0, 10)` the chains behave (n_eff ≈ 389, R̂ = 1), and a1 + a2 is estimated well. Treedepth warnings often accompany this: “These treedepth warnings usually indicate inefficient chains, but not necessarily broken chains.”"
  - id: divergences-meaning
    prompt: "What does a *divergent transition* warning from Stan mean?"
    options:
      - text: "A chain finished faster than expected"
      - text: "The model has too many parameters"
      - text: "The HMC simulation lost energy conservation, usually because the posterior has regions of sharp curvature the step size cannot handle; the samples may be biased, so investigate"
        correct: true
      - text: "The priors are too narrow"
    explanation: "A divergence means the leapfrog integrator went badly wrong somewhere, typically in a region of strong curvature. HMC detects this, which the book counts as a feature: “HMC ... tells us when things are going wrong. Other algorithms, like Metropolis-Hastings, can remain silent about major problems” (p. 284). Treat divergences like failing health checks: a few may be noise, a pattern means something is wrong. Chapter 13 shows the standard fix (non-centered parameterization)."
  - id: debugging-checklist
    prompt: "Your Bayesian model of service error rates shows R̂ = 1.08 and wandering trace plots. Which steps follow from this section?"
    type: multiple
    options:
      - text: "Check the model and data for problems (folk theorem) before adding compute"
        correct: true
      - text: "Replace flat priors with weakly informative ones"
        correct: true
      - text: "Look for non-identified combinations of parameters, such as two intercepts that are only identified by their sum"
        correct: true
      - text: "Report the results anyway since the mean looks plausible"
    explanation: "The chapter's sick-chain examples point to the model, not the machine: flat priors over almost-empty data, or non-identified parameters. Fix those, rerun with multiple chains, and confirm R̂ ≈ 1 and healthy trace and trank plots. Never report from chains that have not converged; their means can look plausible while being meaningless."
---

How to tell a healthy Markov chain from a sick one, and what usually cures it.

## Learning objectives

- Choose samples and warmup based on effective sample size and purpose
- Decide between multiple chains and one long chain
- Diagnose wild chains and non-identified models
- Apply the folk theorem of statistical computing

## In a nutshell

| Symptom | Likely cause | Cure |
|---|---|---|
| Huge drifts, divergences, tiny n_eff | Flat priors + little data | Weakly informative priors |
| Opposite-signed parameters, R̂ ≫ 1 | Non-identified sum | Regularizing priors or reparameterize |
| Treedepth warnings | Inefficient geometry | Often a model problem; check identification |
| Divergences in multilevel models | Funnel geometry | Non-centered parameterization (Chapter 13) |

Use several chains while developing a model to check convergence; once sampling is verified, one long chain is fine.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 9.5, pp. 287–296; Figures 9.9–9.11.
- Gelman A. The folk theorem of statistical computing. *Statistical Modeling, Causal Inference, and Social Science* blog, 2008.
- Gelman A et al. Bayesian workflow. arXiv:2011.01808, 2020.
