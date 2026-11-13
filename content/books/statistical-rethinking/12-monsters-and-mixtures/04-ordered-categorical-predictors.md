---
title: "12.4 Ordered Categorical Predictors"
questions:
  - id: factor-order
    prompt: "R's factor for education listed levels alphabetically (Bachelor's, Elementary, Graduate…). What does the book do first?"
    options:
      - text: "Use the factor codes as they are"
      - text: "Drop the variable"
      - text: "Recode levels into their true ascending order (1 = Elementary … 8 = Graduate Degree)"
        correct: true
      - text: "Treat each level as continuous years"
    explanation: "**p. 391:** “There are 8 different levels of completed education in the sample. Unfortunately, they aren’t actually in order, from lowest to highest. This is typical with R, when it constructs a factor variable from character data.” A reminder of the Chapter 5 warning: never trust implicit mappings from labels to integers. Sorting 'v10' before 'v9' is the same class of bug."
  - id: incremental-effects
    prompt: "How does the book model an ordered predictor like education?"
    options:
      - text: "As a single linear slope on the level number"
      - text: "As unordered index categories"
      - text: "As a polynomial"
      - text: "As a sum of incremental effects: each completed level adds its own δⱼ, scaled by a maximum effect βE"
        correct: true
    explanation: "**p. 392:** “each step up in value comes with its own incremental, or marginal, effect on the outcome.” An individual at level 3 gets δ₁ + δ₂; at level 8 gets the sum of all seven. Written as φᵢ = βE Σⱼ₌₀^{Eᵢ−1} δⱼ, where the δ's are proportions summing to 1, so βE is the effect of going from lowest to highest level."
  - id: dirichlet
    prompt: "What prior does the book use for the vector of incremental proportions δ, and why?"
    options:
      - text: "A Dirichlet distribution — the multivariate extension of the beta — because δ is a simplex of proportions that must be positive and sum to 1"
        correct: true
      - text: "Independent Normals"
      - text: "A Poisson"
      - text: "A uniform from 0 to 1 for each"
    explanation: "**p. 393:** “The Dirichlet distribution is the multivariate extension of the beta distribution ... Like the beta, the Dirichlet is a distribution for probabilities, values between zero and one that all sum to one.” With α = (2, …, 2), the prior “doesn’t expect all the probabilities to be equal. Instead it expects that any of the probabilities could be bigger or smaller than the others” (Figure 12.7). In Stan the type is `simplex[7]`."
  - id: education-result
    prompt: "What did the education model find, and what causal caution does the book add?"
    options:
      - text: "Education strongly increases approval; it is clearly causal"
      - text: "More education is associated with slightly more disapproval, with uneven increments (Some College adds little); but age may confound the association, since age drives completed education"
        correct: true
      - text: "Education has no association"
      - text: "Every level has exactly the same effect"
    explanation: "**p. 395:** “The overall association of education bE is negative—more educated individuals disapproved more of everything. The association is smaller than the treatment effects.” Figure 12.8 shows that Some College adds very little. And p. 396: “Education is highly correlated with age, because age causes ... the completion of levels of education. So there is plausibly a backdoor”."
  - id: linear-vs-ordered
    prompt: "Treating education as a plain linear predictor gave a much weaker association. Why might that be?"
    options:
      - text: "Linear models always underestimate"
      - text: "Because of a coding error"
      - text: "Because the true effect is not linear in the level number — different levels have different increments — and forcing equal steps misrepresents it"
        correct: true
      - text: "Because of the Dirichlet prior"
    explanation: "**p. 396:** “This model seems to think that education is much more weakly associated with rating. This is possibly because the effect isn’t actually linear. Different levels have different incremental associations.” Using a semantic-version major number or support tier as a linear predictor makes the same equal-steps assumption."
  - id: tier-predictor
    prompt: "You want the effect of a customer's support tier (Free < Basic < Pro < Enterprise) on churn. Which encoding does this section recommend?"
    options:
      - text: "Integer 1–4 as a continuous slope"
      - text: "One-hot unordered categories ignoring order"
      - text: "Drop the variable"
      - text: "An ordered predictor: a total effect β times a cumulative sum of Dirichlet-distributed increments, so each step up can contribute a different amount"
        correct: true
    explanation: "The monotonic-effect construction respects order (effects accumulate in one direction) without forcing equal spacing. It uses fewer assumptions than a linear slope and more structure than unordered categories."
---

When a predictor is ordered but not interval-scaled, model each step's increment.

## Learning objectives

- Recode ordered factors into the right order
- Build monotonic effects as sums of incremental proportions
- Use the Dirichlet prior for a simplex
- Compare linear and ordered treatments of a predictor and consider confounding

## In a nutshell

```text
φᵢ = βE Σⱼ₌₀^{Eᵢ−1} δⱼ + βA Aᵢ + βI Iᵢ + βC Cᵢ
δ₀ = 0,   (δ₁ … δ₇) ~ Dirichlet(2, 2, …, 2)
```

| Encoding | Assumes |
|---|---|
| Linear in level | Equal steps |
| Unordered index | No order at all |
| Ordered (Dirichlet increments) | Order, unequal steps |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 12.4, pp. 391–397; Figures 12.7–12.8.
- Bürkner P-C, Charpentier E. Modelling monotonic effects of ordinal predictors in Bayesian regression models. *British Journal of Mathematical and Statistical Psychology* 2020.
