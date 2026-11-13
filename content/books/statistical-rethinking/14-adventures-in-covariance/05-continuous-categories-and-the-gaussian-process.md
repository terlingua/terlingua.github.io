---
title: "14.5 Continuous Categories and the Gaussian Process"
questions:
  - id: continuous-categories
    prompt: "Why are ordinary varying effects inadequate for categories like age, income or location?"
    options:
      - text: "Ordinary varying effects treat every category as equally different from every other; for continuous dimensions, nearby values should have more similar effects than distant ones"
        correct: true
      - text: "They are too slow"
      - text: "Because age is categorical"
      - text: "Because they need more data"
    explanation: "**p. 468:** “individuals of similar ages also experienced some of these same exposures, but to a lesser extent than individuals of the same age. The covariation falls off as any two individuals become increasingly dissimilar.” Unique ages per individual make discrete pooling useless. Gaussian process regression lets covariance depend on distance. Servers in the same rack, same AZ, same region: similarity decays with distance."
  - id: gp-kernel
    prompt: "In the Oceanic tools Gaussian process, the covariance between societies i and j is Kᵢⱼ = η² exp(−ρ² Dᵢⱼ²) + δᵢⱼσ². What do η² and ρ² control?"
    options:
      - text: "η² is the mean; ρ² is the variance"
      - text: "η² is the maximum covariance; ρ² is the rate at which covariance declines with distance"
        correct: true
      - text: "Both are distances"
      - text: "η² is the number of societies; ρ² is the sample size"
    explanation: "**p. 470:** “The part of the formula for K that gives the covariance model its shape is exp(−ρ² D²ᵢⱼ) ... the covariance between any two societies i and j declines exponentially with the squared distance between them. The parameter ρ determines the rate of decline.” η² sets the maximum covariance; the δᵢⱼσ² term adds extra variance for each society with itself (nugget). This is the squared-exponential (L2) kernel."
  - id: why-square
    prompt: "Why use squared distance in the kernel, according to the book?"
    options:
      - text: "It is mathematically required"
      - text: "Because distances are negative"
      - text: "You don't have to — it is a modeling choice; the squared form makes covariance decline slowly at first and then quickly, which often fits the idea that nearby units are nearly identical"
        correct: true
      - text: "To make the model linear"
    explanation: "**p. 470:** “Why square the distance? You don’t have to. This is just a model.” The squared-exponential kernel is smooth near zero; the L1 (Ornstein–Uhlenbeck) kernel, exp(−ρ D), declines immediately. The phylogenetic example uses the L1 kernel. Kernels are design choices — the covariance analogue of choosing a likelihood."
  - id: spatial-result
    prompt: "What does the Gaussian process reveal about Malekula, Tikopia and Santa Cruz?"
    options:
      - text: "They are outliers to delete"
      - text: "They are unrelated"
      - text: "They have the most tools"
      - text: "They are geographically close and all have fewer tools than expected for their populations; the posterior covariance describes this shared spatial deviation"
        correct: true
    explanation: "**p. 476:** “the correlations among Malekula, Tikopia, and Santa Cruz describe the fact that they are below the expected number of tools for their populations. All three societies lying below the expectation, and being so close, is consistent with spatial covariance.” Similarly Manus and Trobriand. Neighbouring hosts with correlated anomalies in a fleet model may share a rack-level cause."
  - id: phylogeny
    prompt: "In the primate example, regressing brain size on group size and body mass gives a positive group-size effect. Including the Brownian-motion phylogenetic covariance makes it nearly vanish. Why?"
    options:
      - text: "Because both brain size and group size cluster in the tree; closely related species share both through common ancestry, producing a spurious association that the phylogenetic covariance absorbs"
        correct: true
      - text: "Because phylogeny causes group size"
      - text: "Because the sample was reduced"
      - text: "Because Brownian motion is always correct"
    explanation: "**p. 482:** “This model annihilates group size ... The big change from the previous model suggests that there is a lot of clustering of brain size in the tree and that this produces a spurious relationship with group size, which also clusters in the tree.” But the book is critical: the result depends on the assumed covariance, and the Gaussian process version with an L1 kernel finds the data favor very small phylogenetic covariance (Figure 14.14)."
  - id: phylogeny-critical
    prompt: "What is the book's critical stance on phylogenetic regression?"
    options:
      - text: "It always gives causal estimates"
      - text: "Phylogeny is a proxy for unobserved past confounds; the result depends heavily on the assumed covariance model, and if you had the past variables you would not need phylogeny at all"
        correct: true
      - text: "It should never be used"
      - text: "It is identical to ordinary regression"
    explanation: "**p. 478:** “if we did have measurements of G₁ and U₁, we could use these and not worry at all about phylogeny. Since we haven’t observed the past, we need some way to estimate its influence ... Phylogeny is associated with the patterns of covariation across species.” The chosen kernel (Brownian motion versus OU/L1) changes conclusions, so treat it as an assumption to scrutinize. Code ancestry in a monorepo is similar: shared history explains similar bug rates, but it is a proxy for the real mechanisms."
  - id: cholesky-gp
    prompt: "For large Gaussian process covariance matrices, what parameterization is often necessary?"
    options:
      - text: "Centered with flat priors"
      - text: "Removing the covariance"
      - text: "The non-centered (Cholesky) parameterization: decompose K, multiply its Cholesky factor by standard-normal z-scores to get the correlated effects"
        correct: true
      - text: "Using only the diagonal"
    explanation: "**p. 477:** “cholesky_decompose ... takes covariance (or correlation) matrix and returns its Cholesky factor. That Cholesky factor can then be mixed with z-scores as before ... With a very large SIGMA matrix, often there is no choice but to use the Cholesky (non-centered) parameterization.” GP computation is O(n³) in the number of units, which limits naive GPs to a few thousand points."
  - id: gp-devops
    prompt: "You model daily error rates for 500 edge servers placed in cities worldwide. Which use of a Gaussian process fits this section?"
    options:
      - text: "A GP over server IDs as unordered categories"
      - text: "Drop distance and use complete pooling"
      - text: "A GP over the error counts directly with no predictors"
      - text: "Varying intercepts per server whose covariance declines with geographic distance (or network latency) between servers, letting nearby servers share information"
        correct: true
    explanation: "Continuous categories — location, time, configuration distance — call for a covariance that depends on distance. A GP prior on server intercepts lets regional incidents (power, ISP, weather) show up as correlated deviations while still pooling across the fleet. For time, a GP over hours gives smooth seasonal effects, a relative of the splines in Chapter 4."
---

Varying effects for continuous dimensions: covariance that decays with distance in space, time or ancestry.

## Learning objectives

- Explain why continuous categories need distance-based covariance
- Read a Gaussian process kernel and its parameters
- Interpret spatial and phylogenetic examples critically
- Use Cholesky parameterization for large covariance matrices

## In a nutshell

```text
Tᵢ ~ Poisson(λᵢ)
λᵢ = exp(k[society]) · α Pᵢ^β / γ
k ~ MVNormal(0, K)
Kᵢⱼ = η² exp(−ρ² Dᵢⱼ²) + δᵢⱼ σ²         squared-exponential (L2)
Kᵢⱼ = η² exp(−ρ Dᵢⱼ)                    Ornstein–Uhlenbeck (L1)
```

```csharp
double[,] Kernel(double[,] d, double etaSq, double rhoSq, double delta)
{
    var n = d.GetLength(0);
    var k = new double[n, n];
    for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            k[i, j] = etaSq * Math.Exp(-rhoSq * d[i, j] * d[i, j]) + (i == j ? delta : 0);
    return k;
}
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 14.5, pp. 467–485; Figures 14.10–14.14.
- Rasmussen CE, Williams CKI. *Gaussian Processes for Machine Learning*. MIT Press, 2006. [Free online](http://gaussianprocess.org/gpml/)
- Street SE et al. Coevolution of cultural intelligence, extended life history, sociality, and brain size in primates. *PNAS* 2017.
