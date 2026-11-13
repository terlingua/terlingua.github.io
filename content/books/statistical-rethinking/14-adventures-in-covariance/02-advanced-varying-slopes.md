---
title: "14.2 Advanced Varying Slopes"
questions:
  - id: many-effects
    prompt: "In the advanced chimpanzee model, what varies by actor and by block?"
    options:
      - text: "Only intercepts"
      - text: "All four treatment effects vary by actor and by block, each cluster type with its own 4 × 4 correlation matrix and standard deviations"
        correct: true
      - text: "Only slopes by block"
      - text: "Nothing varies"
    explanation: "Section 14.2 (p. 447) places varying effects on each of four treatment parameters, for both actors and blocks: each actor and each block gets a vector of four offsets, drawn from a multivariate Gaussian with its own σ vector and correlation matrix. The model estimates how much the treatment effect varies by individual and by day, and how those variations correlate."
  - id: how-you-fit
    prompt: "What does the book mean by 'how you fit the model is part of the model'?"
    options:
      - text: "Different algorithms produce different posteriors by definition"
      - text: "You must always use quap"
      - text: "Mathematically equivalent parameterizations behave differently inside MCMC; choosing a better parameterization can dramatically improve sampling"
        correct: true
      - text: "Model code must be in R"
    explanation: "**p. 447:** “For any given multilevel model, there are several different ways to write it down. These ways are called “parameterizations.” Mathematically, these alternative parameterizations are equivalent, but inside the MCMC engine they are not. Remember, how you fit the model is part of the model.” Same as two SQL queries returning the same result with very different execution plans."
  - id: cholesky
    prompt: "How is a covariance matrix 'smuggled out' of the prior in a non-centered multivariate model?"
    options:
      - text: "By deleting it"
      - text: "By fixing the correlations at zero"
      - text: "By using a uniform prior"
      - text: "By the Cholesky decomposition: sample a matrix of independent z-scores, then multiply by diag(σ) and the Cholesky factor L of the correlation matrix to get correlated varying effects"
        correct: true
    explanation: "From the box on p. 453: “Model m14.3 uses a trick known as the Cholesky decomposition to smuggle the covariance matrix out of the prior.” If R = L Lᵀ, then (diag(σ) L z) has covariance diag(σ) R diag(σ) when z ~ Normal(0, 1). It is the multivariate version of x = z exp(v) from the Devil's Funnel. ulam's `compose_noncentered` does this; Stan uses `lkj_corr_cholesky`. The same trick generates correlated random variables in Monte Carlo risk simulations."
  - id: long-chains-not-enough
    prompt: "The centered model m14.2 eventually gives reliable samples if run long enough. Why still reparameterize?"
    options:
      - text: "Because running long is inefficient and unreliable: chains may still be subtly biased in ways hard to detect"
        correct: true
      - text: "Because longer chains are illegal"
      - text: "Because centered models are wrong"
      - text: "Because WAIC requires it"
    explanation: "**p. 453:** “When there are inefficient chains, often running the chains long enough will produce reliable samples from the posterior ... But this is both inefficient and unreliable. The chains could still be biased in subtle ways that are hard to detect. Better to re-parameterize.” Throwing hardware at a slow algorithm hides the design flaw."
  - id: distribution-of-effects
    prompt: "Why might the *distribution* of treatment effects matter more than the average effect, as in the aspirin example?"
    options:
      - text: "Because averages are always zero"
      - text: "Because when only some units benefit, the average effect can be small while some units benefit greatly; the variation itself is the scientifically important quantity"
        correct: true
      - text: "Because distributions are easier to compute"
      - text: "It doesn't; only averages matter"
    explanation: "**p. 453:** “For many medications, only some people benefit. The average treatment effect is not really as interesting as the distribution of treatment effects, in such cases.” A performance optimization might help 10% of customers enormously and do nothing for the rest; the average hides the targeting opportunity. Varying slopes estimate that distribution."
  - id: sigma-interpret
    prompt: "In the advanced chimpanzee model, the posterior σ for actors is much larger than for blocks. What does that say?"
    options:
      - text: "Blocks matter more"
      - text: "The model is non-identified"
      - text: "Individual chimpanzees differ much more in their treatment responses than different days do"
        correct: true
      - text: "Treatments have no effect"
    explanation: "Comparing σ vectors across cluster types tells you where variation lives. Individual differences (handedness and responses) dominate; day-to-day variation is small — consistent with Section 13.3. In operations, comparing per-host and per-day variance components tells you whether to look at machines or at time."
---

Many varying effects across several cluster types, and the Cholesky trick that makes them sample well.

## Learning objectives

- Specify multiple varying slopes for multiple cluster types
- Explain why parameterization matters to MCMC
- Use the Cholesky factor to build non-centered correlated effects
- Interpret variance components and distributions of effects

## In a nutshell

```text
logit(pᵢ) = γ[tid] + α[actor, tid] + β[block, tid]
α[actor, ·] = (diag(σ_actor) · L_actor · z_actor)ᵀ,   z ~ Normal(0, 1)
L_actor ~ LKJ_corr_cholesky(2);   σ_actor ~ Exponential(1)
```

```csharp
double[] Correlated(double[] sigma, double[,] l, double[] z) =>
    Enumerable.Range(0, sigma.Length)
        .Select(i => sigma[i] * Enumerable.Range(0, i + 1).Sum(k => l[i, k] * z[k]))
        .ToArray();
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 14.2, pp. 447–455.
- Stan Development Team. *Stan User's Guide*, section on multivariate priors for hierarchical models.
