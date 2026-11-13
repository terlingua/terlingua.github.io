---
title: "13.3 More Than One Type of Cluster"
questions:
  - id: cross-classified
    prompt: "In the chimpanzee data, each pull belongs to an actor and to an experimental block (day). How does the book model this?"
    options:
      - text: "With one varying intercept that combines actor and block"
      - text: "With two sets of varying intercepts, α[actor] ~ Normal(ᾱ, σα) and γ[block] ~ Normal(0, σγ), both added to the linear model"
        correct: true
      - text: "By fitting separate models per block"
      - text: "By ignoring block"
    explanation: "**p. 415:** “each observed pull belongs to both an actor (1 to 7) and a block (1 to 6). There may be unique intercepts for each actor as well as for each block.” The structure is cross-classified (actors are not nested in blocks). In ops data a request belongs to both a host and a deployment version, or a tenant and a region: two crossed cluster types."
  - id: one-mean
    prompt: "Why is there only one global mean ᾱ, with the block intercepts centered on 0?"
    options:
      - text: "Because blocks have no effect"
      - text: "Because of a Stan limitation"
      - text: "Because both intercept types add to the same linear prediction, so separate means for each cluster type are not separately identifiable"
        correct: true
      - text: "Because actors are more important"
    explanation: "**p. 416:** “We can’t identify a separate mean for each varying intercept type, because both intercepts are added to the same linear prediction. If you do include a mean for each cluster type, it won’t be the end of the world, however. It’ll be like the right leg and left leg example from Chapter 6.” Only their sum would be identified."
  - id: sigma-comparison
    prompt: "The posterior shows σα ≈ 2.0 and σγ ≈ 0.2. What does this mean?"
    options:
      - text: "Blocks vary more than actors"
      - text: "The model failed"
      - text: "Both vary equally"
      - text: "Actors vary a lot in their baseline tendency, while blocks (days) barely differ; block intercepts are strongly shrunk toward zero"
        correct: true
    explanation: "Figure 13.4 contrasts the posterior distributions of σ by actor and by block. Because σγ is near zero, “each of the 6 g parameters is strongly shrunk towards zero” (p. 418). Estimating each σ tells you where variation lives — e.g. latency varies a lot by host but little by day, pointing investigation at hosts."
  - id: pwaic-blocks
    prompt: "The model with block intercepts has 7 more parameters than the one without, but only about 2 more effective parameters. Why?"
    options:
      - text: "Because σγ is near zero, the block intercepts are heavily regularized and contribute little flexibility"
        correct: true
      - text: "WAIC ignores block parameters"
      - text: "Because blocks have no data"
      - text: "Because pWAIC counts only fixed effects"
    explanation: "**p. 418:** “While m13.4 has 7 more parameters than m13.5 does, it has only 2 more effective parameters. Why? Because the posterior distribution for sigma_g ended up close to zero.” The two models predict almost identically. The book keeps both: “By retaining and reporting both models, we and our readers learn more about the experiment” (p. 419)."
  - id: fixed-vs-random
    prompt: "Some people say varying (random) effects should only be used for variables not controlled by the experimenter. What does the book say?"
    options:
      - text: "That is correct"
      - text: "This is all wrong: varying effects are used because they give better inferences; if units are exchangeable — indices could be reassigned without changing the model's meaning — partial pooling can help, regardless of how the clusters arose"
        correct: true
      - text: "Treatments can never be pooled"
      - text: "Varying effects are only for time"
    explanation: "**p. 419:** “This is all wrong. The reason to use varying effects is because they provide better inferences. It doesn’t matter how the clusters arise. If the individual units are exchangable—the index values could be reassigned without changing the meaning of the model—then partial pooling could help.” With only four treatments and lots of data each, pooling the treatments changes little. 'Fixed' versus 'random' effects is confusing terminology the book avoids."
  - id: n-eff-varies
    prompt: "In m13.4, n_eff varies widely across parameters. What is one common reason in multilevel models?"
    options:
      - text: "Random seeds"
      - text: "Parameters with more data always have lower n_eff"
      - text: "Some parameters, especially variance parameters and intercepts near a boundary, live in regions of difficult posterior geometry that HMC explores less efficiently"
        correct: true
      - text: "Stan assigns n_eff arbitrarily"
    explanation: "The book notes (p. 417) that varying n_eff “is common in complex models.” The sampler explores some directions less efficiently, often because σ parameters interact with the intercepts they govern. Stan's divergence warnings point the same way. Section 13.4 addresses this with reparameterization."
---

Real data are often clustered several ways at once. Each cluster type gets its own varying effects and its
own estimate of variation.

## Learning objectives

- Build cross-classified varying intercepts
- Explain why only one global mean is identified
- Compare σ across cluster types to locate variation
- Reject the 'fixed versus random' dogma in favor of exchangeability

## In a nutshell

```text
Lᵢ ~ Binomial(1, pᵢ)
logit(pᵢ) = α[actor] + γ[block] + β[treatment]
α_j ~ Normal(ᾱ, σα)
γ_j ~ Normal(0, σγ)
β_k ~ Normal(0, 0.5)
ᾱ ~ Normal(0, 1.5);  σα, σγ ~ Exponential(1)
```

## For engineers

| Data | Crossed clusters |
|---|---|
| Request latency | host × deploy version |
| Build duration | repository × runner pool |
| Ticket resolution time | team × product area |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 13.3, pp. 415–420; Figure 13.4.
- Gelman A. Analysis of variance—why it is more important than ever. *Annals of Statistics* 2005.
