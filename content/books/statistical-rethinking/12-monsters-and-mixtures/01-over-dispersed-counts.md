---
title: "12.1 Over-Dispersed Counts"
questions:
  - id: monsters
    prompt: "What does the chapter mean by statistical 'monsters'?"
    options:
      - text: "Models that always fail"
      - text: "Models with too many parameters"
      - text: "Hybrid likelihoods and links built by piecing together simpler components, like mythological creatures assembled from parts of different animals"
        correct: true
      - text: "Outliers"
    explanation: "**p. 369:** “By piecing together parts of different creatures, it’s easy to make a monster. Many monsters are hybrids. Many statistical models are too. This chapter is about constructing likelihood and link functions by piecing together the simpler components of previous chapters.” Beta-binomial, gamma-Poisson, zero-inflated and ordered-logit models are compositions of distributions you already know — like composing middleware."
  - id: overdispersion-def
    prompt: "What is over-dispersion?"
    options:
      - text: "Counts with fewer zeros than expected"
      - text: "A model with too many parameters"
      - text: "Counts with negative values"
      - text: "Observed variance in counts greater than the count distribution implies, after conditioning on predictors — usually a sign of an omitted source of variation"
        correct: true
    explanation: "**p. 370:** for a binomial the variance is Np(1 − p). “When the observed variance exceeds this amount—after conditioning on all the predictor variables—this implies that some omitted variable is producing additional dispersion in the observed counts.” Incident counts per week are almost always over-dispersed: some weeks have a bad deploy, a cloud outage, or a holiday freeze."
  - id: overdispersion-harm
    prompt: "What can go wrong if you ignore over-dispersion?"
    options:
      - text: "The same problems as ignoring any predictor: heterogeneity can confound, hide effects or produce spurious ones, and the model will be overconfident"
        correct: true
      - text: "Nothing; it only affects variance"
      - text: "The model will not run"
      - text: "Priors become improper"
    explanation: "**p. 370:** “ignoring over-dispersion can also lead to all of the same problems as ignoring any predictor variable. Heterogeneity in counts can be a confound, hiding effects of interest or producing spurious inferences.” The best fix is to find the missing variable. Otherwise, continuous mixtures or multilevel models absorb the extra variation."
  - id: beta-binomial
    prompt: "How does the beta-binomial model work?"
    options:
      - text: "It replaces the binomial with a Gaussian"
      - text: "Each binomial count gets its own probability of success drawn from a beta distribution with mean p̄ and dispersion θ; the model estimates the beta distribution rather than a single p"
        correct: true
      - text: "It adds a random intercept per observation"
      - text: "It fits two binomials and averages them"
    explanation: "The beta-binomial is a continuous mixture: “every binomial count is assumed to have its own independent beta-distributed probability of success” (p. 375). The beta has mean p̄ and shape θ: “When θ = 2, every probability from zero to 1 is equally likely ... When θ < 2, the distribution is so dispersed that extreme probabilities near zero and 1 are more likely than the mean” (p. 371). Per-tenant error rates are a natural fit: each tenant has its own rate, drawn from a population distribution."
  - id: beta-ucb
    prompt: "Fitting the beta-binomial to UC Berkeley admissions without department, what happens to the gender difference?"
    options:
      - text: "It becomes huge and certain"
      - text: "It disappears with certainty"
      - text: "It becomes highly uncertain, because the wide beta distribution allows departments to have very different admission rates, absorbing the variation the gender-only binomial wrongly attributed to gender"
        correct: true
      - text: "The model fails"
    explanation: "**p. 372:** the difference da “is highly uncertain.” The posterior validation check (Figure 12.1) now contains the raw data within its intervals. The beta-binomial does not know about departments, but it knows rows differ, so it no longer attributes all variation to gender."
  - id: gamma-poisson
    prompt: "What is the variance of the gamma-Poisson (negative-binomial) distribution, and what does φ control?"
    options:
      - text: "λ; φ is the mean"
      - text: "λ/φ; smaller φ makes it closer to Poisson"
      - text: "φ²; φ is the rate"
      - text: "λ + λ²/φ; larger φ makes it closer to a pure Poisson"
        correct: true
    explanation: "**p. 374:** “The variance of the gamma-Poisson is λ + λ²/ϕ. So larger ϕ values mean the distribution is more similar to a pure Poisson process.” Each observation's rate is drawn from a gamma distribution with mean λ. Hawaii becomes much less influential in the tools model, “Because gamma-Poisson expects more variation around the mean rate” (Figure 12.2)."
  - id: no-waic-mixtures
    prompt: "Why does the book warn against using WAIC and PSIS with beta-binomial and gamma-Poisson models unless you know what you are doing?"
    options:
      - text: "Because these models cannot be aggregated or disaggregated across rows without changing causal assumptions — each row shares one latent probability or rate — so 'leave one out' is ambiguous"
        correct: true
      - text: "Because they cannot be computed"
      - text: "Because they always overfit"
      - text: "Because WAIC requires Gaussian likelihoods"
    explanation: "**p. 375:** unlike plain binomial or Poisson models, “the same is not true of beta-binomial and gamma-Poisson models.” Disaggregating applications loses the fact that the model implies “the same latent probability for all of the applicants from the same row.” Multilevel models (next chapter) make this explicit, which is why “it is often easier to use multilevel models ... in place of continuous mixtures” (p. 370)."
  - id: incident-overdispersion
    prompt: "Weekly incident counts across 50 teams have mean 2 and variance 9. Which modeling choice fits this section's advice best?"
    options:
      - text: "Poisson regression, since counts are counts"
      - text: "Look for the omitted source of variation (team size, deploy frequency); if unavailable, use a gamma-Poisson or a multilevel Poisson with team-level effects"
        correct: true
      - text: "Gaussian regression on raw counts"
      - text: "Drop the teams with the most incidents"
    explanation: "Variance far exceeding the mean is over-dispersion. A pure Poisson would be overconfident and let a few extreme teams dominate. Including the missing driver (exposure such as deploys, as an offset) is best; otherwise gamma-Poisson or varying effects per team absorb the heterogeneity."
---

When counts vary more than binomial or Poisson models allow, mix in a distribution of probabilities or rates.

## Learning objectives

- Define over-dispersion and explain why it matters
- Build beta-binomial and gamma-Poisson models
- Interpret their dispersion parameters
- Explain why information criteria are tricky for these models

## In a nutshell

| Model | Mixture | Variance |
|---|---|---|
| Binomial | none | Np(1 − p) |
| Beta-binomial | p ~ Beta(p̄, θ) per row | larger; θ controls spread |
| Poisson | none | λ |
| Gamma-Poisson | λ ~ Gamma per row | λ + λ²/φ |

```text
Aᵢ ~ BetaBinomial(Nᵢ, p̄ᵢ, θ)       logit(p̄ᵢ) = α[gid]
Tᵢ ~ GammaPoisson(λᵢ, φ)           λᵢ = α[cid] Pᵢ^β[cid] / γ
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 12 opening and Section 12.1, pp. 369–376; Figures 12.1–12.2.
- Hilbe JM. *Negative Binomial Regression*, 2nd ed. Cambridge University Press, 2011.
