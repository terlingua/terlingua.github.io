---
title: "13.4 Divergent Transitions and Non-Centered Priors"
questions:
  - id: divergence-energy
    prompt: "In HMC terms, what is a divergent transition?"
    options:
      - text: "A transition with zero momentum"
      - text: "A transition that moves to a new chain"
      - text: "A transition that is always accepted"
      - text: "A transition where total energy at the end of the simulated trajectory differs substantially from the start, indicating the numerical simulation failed in a steep region of the posterior"
        correct: true
    explanation: "**p. 420:** “the total energy at the start should be equal to the total energy at the end ... But in a numerical system, it might not be ... In these cases, the energy is divergent ... It tends to happen when the posterior distribution is very steep in some region of parameter space.” Divergent transitions are rejected, but they reveal regions the sampler cannot explore correctly — a silent bias risk."
  - id: divergence-harm
    prompt: "Do divergent transitions directly corrupt the posterior samples?"
    options:
      - text: "No, they are rejected; but the region where they occur is hard to explore, so the approximation can be indirectly biased and inefficient"
        correct: true
      - text: "Yes, each divergent sample is wrong"
      - text: "No, they are harmless and can be ignored"
      - text: "Only if there are more than 100"
    explanation: "**p. 420:** “Divergent transitions are rejected. They don’t directly damage your approximation of the posterior distribution. But they do hurt it indirectly, because the region where divergent transitions happen is hard to explore correctly.” Like a crawler that times out on certain pages: what it returns is fine, but its index silently lacks those pages."
  - id: two-tricks
    prompt: "What are the two tricks for dealing with divergent transitions?"
    type: multiple
    options:
      - text: "Tune the sampler: more warmup and a higher target acceptance rate (adapt_delta), so steps are smaller"
        correct: true
      - text: "Reparameterize the model so the posterior geometry is easier, e.g. non-centered priors"
        correct: true
      - text: "Delete the divergent samples and continue"
      - text: "Increase the number of chains to 100"
    explanation: "**p. 420:** “The first is to tune the simulation so that it doesn’t overshoot the valley wall. This means doing more warmup with a higher target acceptance rate, Stan’s adapt_delta. But for many models, you can never tune the sampler enough to remove the divergent transitions. The second trick is to write the statistical model in a new way, to reparameterize it.” Reparameterization changes the geometry; tuning only slows down."
  - id: devils-funnel
    prompt: "In the Devil's Funnel, v ~ Normal(0, 3) and x ~ Normal(0, exp(v)). Why is it so hard to sample?"
    options:
      - text: "It has too many parameters"
      - text: "The scale of x depends on v: when v is small, x is confined to a very narrow, steep neck; when v is large, x spreads widely. No single step size works everywhere"
        correct: true
      - text: "Because x is discrete"
      - text: "Because the prior on v is flat"
    explanation: "**p. 421:** this distribution “represents a typical multilevel distribution, in which the scale of one variable (here x) depends upon another variable (here v).” With only two parameters, the centered model gives n_eff ≈ 39 and R̂ ≈ 1.06 with many divergences. In a multilevel model, v plays the role of log σ and x the role of cluster effects."
  - id: non-centered
    prompt: "What is the non-centered parameterization of the funnel?"
    options:
      - text: "x ~ Normal(v, 1)"
      - text: "Remove v from the model"
      - text: "Sample z ~ Normal(0, 1) and compute x = z × exp(v); the sampler explores independent v and z, and x is reconstructed deterministically"
        correct: true
      - text: "Use a uniform prior on x"
    explanation: "**p. 422–423:** v ~ Normal(0, 3), z ~ Normal(0, 1), x = z exp(v). It is like standardizing a variable: sample a standardized value, then scale it. The joint posterior of (v, z) has no funnel, so n_eff climbs to ~1,600 with no divergences. “That is the non-centered parameterization. It’s used often when working with multilevel models. However, there are times when the centered prior is better.”"
  - id: nc-chimps
    prompt: "How is the chimpanzee model made non-centered?"
    options:
      - text: "By removing the varying intercepts"
      - text: "By using flat priors"
      - text: "By centering the data"
      - text: "By writing α[actor] = ᾱ + z[actor] σα and γ[block] = x[block] σγ, with z, x ~ Normal(0, 1), reconstructing the intercepts inside the linear model"
        correct: true
    explanation: "**p. 424:** “There are three embedded (“centered”) parameters to smuggle out of these priors: ᾱ, σα, σγ ... we’ll define some new variables that are given standard Normal distributions, and then we’ll reconstruct the original variables by undoing the transformation.” Figure 13.6 shows n_eff improving for almost all parameters. The model is mathematically the same; only the sampler's coordinates change."
  - id: when-centered
    prompt: "When is the centered parameterization usually better than the non-centered?"
    options:
      - text: "When there is a lot of data per cluster, so the likelihood pins each cluster effect and the funnel geometry is weak"
        correct: true
      - text: "Never"
      - text: "When there are many clusters with little data"
      - text: "When σ is near zero"
    explanation: "The book warns that “there are times when the centered prior is better. So it pays to be comfortable with both” (p. 423). In general, non-centered helps when clusters have little data (the prior dominates and the funnel is strong); centered helps when clusters have lots of data. Choose per cluster type by trying both and comparing n_eff and divergences."
---

Multilevel models have funnel-shaped posteriors that HMC finds hard. Reparameterization fixes the geometry.

## Learning objectives

- Explain divergent transitions as energy errors in steep regions
- Recognize the Devil's Funnel in multilevel models
- Write non-centered parameterizations
- Choose between centered and non-centered forms

## In a nutshell

```text
Centered:       α_j ~ Normal(ᾱ, σ)
Non-centered:   z_j ~ Normal(0, 1);   α_j = ᾱ + σ · z_j
```

| Symptom | Fix |
|---|---|
| Few divergences, simple model | Raise adapt_delta, more warmup |
| Many divergences in multilevel model | Non-centered parameterization |
| Lots of data per cluster | Centered may sample better |

## For engineers

Reparameterization is like changing a database index rather than adding hardware: the question is the same,
but the access path stops hitting a pathological region.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 13.4, pp. 420–426; Figures 13.5–13.6.
- Betancourt M, Girolami M. Hamiltonian Monte Carlo for hierarchical models. arXiv:1312.0906, 2013.
- Neal RM. Slice sampling. *Annals of Statistics* 2003 — origin of the funnel example.
