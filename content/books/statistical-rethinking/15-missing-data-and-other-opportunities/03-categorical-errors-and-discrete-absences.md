---
title: "15.3 Categorical Errors and Discrete Absences"
questions:
  - id: discrete-hard
    prompt: "Why are missing *discrete* values harder than missing continuous values in Stan?"
    options:
      - text: "Because discrete values cannot be missing"
      - text: "Because Stan does not support integers"
      - text: "Because discrete values are always zero"
      - text: "Because HMC needs gradients, so you cannot declare a discrete parameter for each unknown value"
        correct: true
    explanation: "**p. 516:** “When a variable is continuous, you can just assign a parameter to each unknown value ... discrete variables are computationally trickier.” HMC simulates a particle on a continuous surface; there is no gradient with respect to a 0/1 variable. The solution is to marginalize — average over the possible values."
  - id: marginalize
    prompt: "How does the 'sneaking cat' model handle a house where you don't know whether a cat is present?"
    options:
      - text: "It averages the likelihood over both possibilities: Pr(N) = k × Pr(N | cat) + (1 − k) × Pr(N | no cat)"
        correct: true
      - text: "It drops the house"
      - text: "It assumes there is no cat"
      - text: "It flips a coin"
    explanation: "Section 15.3.1 (p. 518–519): when Cᵢ is unknown, the likelihood of the notes is the weighted average over cat present and absent, weighted by the probability k of a cat. On the log scale this is `log_sum_exp(log(k) + poisson_lpmf(notes | exp(a + b)), log(1 - k) + poisson_lpmf(notes | exp(a)))`. Same structure as zero inflation: OR becomes a sum of path probabilities."
  - id: no-info-lost
    prompt: "Does marginalizing out discrete parameters throw away information about them?"
    options:
      - text: "Yes, they cannot be recovered"
      - text: "No — after sampling the other parameters, you can compute the posterior probability of each discrete value, and removing them also makes chains more efficient"
        correct: true
      - text: "Only if k is known"
      - text: "Yes, unless you use Gibbs sampling"
    explanation: "**p. 517:** marginalizing removes discrete parameters, and “After sampling the other parameters, we can then use their samples to compute the posterior distribution of any discrete parameter that we removed. So no information is given up. And removing the discrete parameters actually makes the Markov chain more efficient ... This all sounds too good to be true. It is all true.”"
  - id: posterior-cat
    prompt: "How is Pr(cat present | notes) computed for a house with unknown cat status?"
    options:
      - text: "It equals k"
      - text: "It cannot be computed"
      - text: "By Bayes' rule in generated quantities: Pr(C = 1 | N) = k Pr(N | C = 1) / [k Pr(N | C = 1) + (1 − k) Pr(N | C = 0)], for each posterior sample"
        correct: true
      - text: "By the observed notes alone"
    explanation: "The `gq>` lines (p. 520) compute exactly this for each sample: the formula is “just the mathematical expression from before, Bayes rule.” A house where the bird sang few notes probably has a cat. Inferring a hidden discrete state (healthy/degraded) of a host from its observed error counts works the same way."
  - id: invalid-code
    prompt: "Why does the simulation code missing cat values as −9 rather than NA or 0?"
    options:
      - text: "Stan requires −9"
      - text: "For performance"
      - text: "Because −9 means 'cat present'"
      - text: "An impossible value makes coding mistakes fail loudly: if the model ever uses it as data, the Bernoulli probability is undefined and an error results"
        correct: true
    explanation: "**p. 518:** “There is nothing special about this value. The model will skip them. But it is usually good to use some invalid value, so that if you make a mistake in coding, an error will result.” Sentinel values that are impossible for the type — fail fast rather than silently computing with a plausible-looking default like 0."
  - id: combinations
    prompt: "With two discrete variables that can each be missing (cat and dog), how many cases must the likelihood handle?"
    options:
      - text: "Every combination of missingness: both observed, only cat missing, only dog missing, both missing — each averaging over the unobserved possibilities"
        correct: true
      - text: "One"
      - text: "Two"
      - text: "Infinitely many"
    explanation: "**p. 521:** “In principle, this is algorithmic and easy. In practice, it makes for complicated code. You have to account all combinations of missingness and assign each a different average likelihood.” State space models (Chapter 16) apply the same idea to hidden states over time — the forward algorithm used in hidden Markov models."
---

Discrete unknowns cannot be HMC parameters, so the model averages over their possible values.

## Learning objectives

- Explain why discrete missing values need marginalization
- Write the averaged likelihood for an unknown binary variable
- Recover posterior probabilities of discrete values
- Use sentinel values and handle combinations of missingness

## In a nutshell

```text
Nᵢ ~ Poisson(λᵢ),   log λᵢ = α + β Cᵢ
Cᵢ ~ Bernoulli(k)
If Cᵢ unknown:
  Pr(Nᵢ) = k · Poisson(Nᵢ | e^(α+β)) + (1 − k) · Poisson(Nᵢ | e^α)
Pr(Cᵢ = 1 | Nᵢ) = k · Poisson(Nᵢ | e^(α+β)) / Pr(Nᵢ)
```

```csharp
static double LogSumExp(double a, double b) { var m = Math.Max(a, b); return m + Math.Log(Math.Exp(a - m) + Math.Exp(b - m)); }
double LogLikUnknownCat(int notes, double a, double b, double k) =>
    LogSumExp(Math.Log(k) + LogPoisson(notes, Math.Exp(a + b)),
              Math.Log(1 - k) + LogPoisson(notes, Math.Exp(a)));
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 15.3, pp. 516–521.
- Stan Development Team. *Stan User's Guide*, chapter on latent discrete parameters.
