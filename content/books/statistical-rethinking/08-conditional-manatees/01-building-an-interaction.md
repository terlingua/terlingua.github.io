---
title: "8.1 Building an Interaction"
questions:
  - id: manatees-bombers
    prompt: "Why is the damage seen on surviving manatees and returning bombers misleading?"
    options:
      - text: "Because the damage was measured badly"
      - text: "Because propellers are rare"
      - text: "Because the evidence is conditional on survival: the fatal kinds of damage (keel strikes, hits to engines and cockpit) are under-represented among survivors"
        correct: true
      - text: "Because the sample is too small"
    explanation: "**p. 237:** “The evidence from surviving manatees and bombers is misleading, because it is conditional on survival.” Improving bombers “meant armoring the undamaged sections.” This is survivorship bias, the selection effect from Chapter 6. Engineering version: crash reports only come from clients that could still upload them; the crashes that killed the uploader are invisible, so the most common crash in your dashboard may not be the most harmful."
  - id: everything-conditional
    prompt: "Which statements reflect the chapter's claim that 'every inference is conditional on something'?"
    type: multiple
    options:
      - text: "Data are conditional on how they get into our sample"
        correct: true
      - text: "Posterior distributions are conditional on the data"
        correct: true
      - text: "All model-based inference is conditional on the model"
        correct: true
      - text: "Inference from large samples is unconditional"
    explanation: "**p. 237:** “Data, like the manatee scars and bomber damage, are conditional on how they get into our sample. Posterior distributions are conditional on the data. All model-based inference is conditional on the model. Every inference is conditional on something, whether we notice it or not.” Interactions extend conditioning so that the *effect* of one variable can itself be conditional on another."
  - id: interaction-def
    prompt: "What is an interaction (also called moderation)?"
    options:
      - text: "A causal path through a mediator"
      - text: "A correlation between two predictors"
      - text: "Two predictors with the same coefficient"
      - text: "A model where the importance of one predictor depends upon another predictor"
        correct: true
    explanation: "**p. 238:** “To model deeper conditionality—where the importance of one predictor depends upon another predictor—we need interaction (also known as moderation). Interaction is a kind of conditioning, a way of allowing parameters (really their posterior distributions) to be conditional on further aspects of the data.” Example: the effect of adding a cache depends on the hit-rate profile of the workload; the effect of a retry depends on whether the failure is transient."
  - id: ruggedness-reversal
    prompt: "Terrain ruggedness is associated with *higher* GDP inside Africa but *lower* GDP outside. What explanation does the book mention for the African pattern?"
    options:
      - text: "Rugged regions may have been protected from the Atlantic and Indian Ocean slave trades, whose economic damage persists"
        correct: true
      - text: "Rugged terrain has more natural resources"
      - text: "Rugged countries have more tourism"
      - text: "Measurement error in GDP"
    explanation: "**p. 240:** “If this relationship is at all causal, it may be because rugged regions of Africa were protected against the Atlantic and Indian Ocean slave trades. Slavers preferred to raid easily accessed settlements, with easy routes to the sea.” The book stresses this is hard to be sure of, because ruggedness correlates with other geography and the causal story involves historical exposures (box on p. 241)."
  - id: dont-split
    prompt: "Why not just fit separate regressions for African and non-African nations?"
    type: multiple
    options:
      - text: "Shared parameters like σ get two less accurate estimates, accidentally assuming they differ"
        correct: true
      - text: "You cannot get probability statements about the splitting variable unless it is in the model"
        correct: true
      - text: "Information criteria require models fit to the same data"
        correct: true
      - text: "Multilevel models later can borrow information across categories, which splitting prevents"
        correct: true
      - text: "Separate regressions cannot be fitted with quap"
    explanation: "**p. 241** gives four reasons. First, splitting means “you have accidentally assumed that variance differs ... you want to avoid accidental assumptions.” Second, to get probability statements about the split variable, “you need to include it in the model.” Third, to compare models with WAIC, “we need models that use all of the same data.” Fourth, multilevel models “borrow information in this way, in order to improve estimates in all categories. When we split the data, this borrowing is impossible.” Same reason not to analyse each region's telemetry in a separate script."
  - id: rescale-not-standardize
    prompt: "Why is ruggedness divided by its maximum rather than standardized to z-scores?"
    options:
      - text: "Because standardizing is not allowed in quap"
      - text: "Because zero ruggedness is meaningful (totally flat), and dividing by the maximum keeps zero at zero while scaling to [0, 1]"
        correct: true
      - text: "Because ruggedness is categorical"
      - text: "Because it makes the posterior Gaussian"
    explanation: "**p. 242:** “We don’t want to do that here, because zero ruggedness is meaningful. So instead terrain ruggedness is divided by the maximum value observed.” Log GDP is divided by its mean, so 1 means average. Choose scalings that preserve meaningful reference points: zero errors, 100% availability, average traffic. Then priors are easy to state."
  - id: practicing
    prompt: "The data swamp even awful priors in the ruggedness example. Why does the book still fuss over the priors?"
    options:
      - text: "Because flat priors are mathematically invalid"
      - text: "Because the journal requires it"
      - text: "To practice doing things right so you are ready when it matters, like wearing a seat belt even when you do not crash"
        correct: true
      - text: "Because priors always dominate"
    explanation: "**p. 245:** “we practice doing things right not because it always matters. Rather, we practice doing things right so that we are ready when it matters. No one would say that wearing a seat belt was a mistake, just because you didn’t get into an accident.” Equivalent to writing tests for simple code: the habit pays off on the complex code."
  - id: indicator-not-enough
    prompt: "Adding a continent index for the intercept (a[cid]) improves WAIC a lot. Does it recover the slope reversal?"
    options:
      - text: "Yes, fully"
      - text: "No, because WAIC got worse"
      - text: "Yes, but only with flat priors"
      - text: "No — it only shifts the line up or down by continent; the slope is still shared, so the reversal is invisible"
        correct: true
    explanation: "Section 8.1.2 (p. 245–247): with μᵢ = α[cid] + β(rᵢ − r̄), both continents share β. Figure 8.4 shows parallel lines. African nations have lower average GDP, which is why WAIC improves, but the relationship with ruggedness cannot differ. Adding a per-region intercept to a latency model captures that one region is slower, not that it responds differently to load."
  - id: index-slopes
    prompt: "How does the book build the interaction, and why does it prefer this form?"
    options:
      - text: "μᵢ = α[cid] + β[cid](rᵢ − r̄): each continent gets its own slope, and it is easy to give every slope the same prior, unlike an indicator-based γ term that makes one slope more uncertain a priori"
        correct: true
      - text: "μᵢ = α + βrᵢ + γAᵢrᵢ, because it is conventional"
      - text: "Fit two models and average them"
      - text: "μᵢ = α + β(rᵢ × cidᵢ), using cid as a number"
    explanation: "**p. 248:** the conventional (β + γAᵢ) form “is equivalent to our index approach, but it is much harder to state sensible priors. Any prior we put on γ makes the slope inside Africa more uncertain than the slope outside Africa ... in the indexing approach, we can easily assign the same prior to the slope, no matter which continent.” Figure 8.5 shows the slopes reversing within one model."
  - id: overfit-hint
    prompt: "The interaction model m8.3 has most of the PSIS weight, but m8.2 keeps a little, and some countries have high Pareto k. What does the book suggest?"
    options:
      - text: "Discard m8.2 entirely"
      - text: "The interaction slopes may be a little overfit; influential points suggest trying robust (Student-t) regression; and these comparisons say nothing about causation"
        correct: true
      - text: "Remove the influential countries"
      - text: "Use p-values instead"
    explanation: "**p. 249:** “the modicum of weight given to m8.2 suggests that the posterior means for the slopes in m8.3 are a little overfit ... If you plot PSIS Pareto k values for m8.3, you’ll notice some influential countries ... This is possibly a good context for robust regression.” And: “these comparisons are not reliable guides to causal inference. They just suggest how important features are for prediction.”"
---

Interactions let the effect of one variable depend on another — the conditionality that linear models so far
could not express.

## Learning objectives

- Explain survivorship bias as conditioning
- Define interaction and recognize when an effect is conditional
- Explain why a single model beats split data
- Build an interaction with index variables and interpret it

## In a nutshell

```text
No interaction:     μᵢ = α[cid] + β (rᵢ − r̄)            parallel lines
Interaction:        μᵢ = α[cid] + β[cid] (rᵢ − r̄)       each continent its own slope
Conventional form:  μᵢ = α[cid] + (β + γ Aᵢ)(rᵢ − r̄)    same model, awkward priors
```

## For engineers

| Situation | Interaction |
|---|---|
| Cache benefit depends on workload | latency ~ cache × hit-profile |
| Retry policy helps only for transient errors | success ~ retry × error-type |
| New instance type faster only for CPU-bound jobs | runtime ~ instance × job-type |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 8 opening and Section 8.1, pp. 237–250; Figures 8.1–8.5.
- Nunn N, Puga D. Ruggedness: the blessing of bad geography in Africa. *Review of Economics and Statistics* 2012. [doi:10.1162/REST_a_00161](https://doi.org/10.1162/REST_a_00161)
- Wald A. *A Method of Estimating Plane Vulnerability Based on Damage of Survivors*. Statistical Research Group, 1943 (reprinted CRC 432, 1980).
