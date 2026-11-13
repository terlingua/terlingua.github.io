---
title: "8.3 Continuous Interactions"
questions:
  - id: tulip-setup
    prompt: "Tulip blooms are measured under three water and three shade levels. Why is an interaction biologically plausible?"
    options:
      - text: "Water and shade are correlated in the experiment"
      - text: "There is no biological reason"
      - text: "Shade causes water to evaporate"
      - text: "Tulips need both water and light to produce blooms, so water helps little when light is scarce"
        correct: true
    explanation: "**p. 259:** “tulips need both water and light to produce blooms. At low light levels, water can’t have much of an effect, because the tulips don’t have enough light to produce blooms.” Resources that are jointly necessary produce interactions. In systems: adding CPU helps a service only if it is not blocked on I/O; adding I/O throughput helps only if CPU is not the bottleneck."
  - id: scaling-blooms
    prompt: "Why are blooms scaled by their maximum, while water and shade are centered?"
    options:
      - text: "Large raw values hinder optimization, scaling makes priors easier, and zero blooms is a meaningful boundary that standardization would destroy; centering water and shade makes the intercept the expected blooms at average conditions"
        correct: true
      - text: "Because quap requires positive outcomes"
      - text: "To make every variable range from −1 to 1"
      - text: "Because blooms are categorical"
    explanation: "**p. 254:** “I’ve scaled blooms by its maximum observed value, for three reasons. First, the large values on the raw scale will make optimization difficult. Second, it will be easier to assign a reasonable prior this way. Third, we don’t want to standardize blooms, because zero is a meaningful boundary we want to preserve.” And: “When rescaling variables, a good goal is to create focal points that you have prior information about, prior to seeing the actual data.”"
  - id: slope-priors
    prompt: "How does the book reason to the prior Normal(0, 0.25) for the water and shade slopes?"
    options:
      - text: "From the observed slopes in the data"
      - text: "Because the predictors span 2 units and blooms span 1, a slope of 0.5 would let one variable explain the whole range; SD 0.25 puts 95% of slopes within ±0.5, making that possible but unlikely"
        correct: true
      - text: "By default convention"
      - text: "Because 0.25 minimizes WAIC"
    explanation: "**p. 255:** “To take us from the theoretical minimum of zero blooms on one end to the observed maximum of 1 ... would require a slope of 0.5 from either variable ... So if we assign a standard deviation of 0.25 to each, then 95% of the prior slopes are from −0.5 to 0.5.” The goal: “impossibly large effects should be assigned low prior probability—and also to force ourselves to think about what the model means.” Bounding plausible effects from the range of the outcome is a generic, reusable technique."
  - id: continuous-form
    prompt: "How is a continuous interaction between water W and shade S written?"
    options:
      - text: "μᵢ = α + βWS (Wᵢ + Sᵢ)"
      - text: "μᵢ = α + βW Wᵢ × βS Sᵢ"
      - text: "μᵢ = α + βW Wᵢ + βS Sᵢ + βWS Wᵢ Sᵢ, which comes from letting the water slope be γW,ᵢ = βW + βWS Sᵢ"
        correct: true
      - text: "μᵢ = α[W] + β[S]"
    explanation: "**p. 256:** the slope for water is itself a linear model of shade — “the slope for shade on the slope of water. Remember, it’s turtles all the way down.” Substituting and expanding gives “the conventional form of a continuous interaction, with the extra term on the far right end holding the product of the two variables.” βW is the effect of water when shade is at its mean (zero after centering)."
  - id: one-parameter-sum
    prompt: "If you try to define separate interaction parameters βWS (water's slope depends on shade) and βSW (shade's slope depends on water), what happens?"
    options:
      - text: "Both are identified separately"
      - text: "The model cannot be written"
      - text: "The model becomes nonlinear"
      - text: "Only their sum is identified, because both multiply the same product WᵢSᵢ — you cannot tell which depends on which"
        correct: true
    explanation: "From the box “How is interaction formed?” (p. 257): substituting both γ definitions gives (βWS + βSW)WᵢSᵢ. “The only thing we can identify in such a model is the sum βWS + βSW ... We just cannot tell the difference between water depending upon shade and shade depending upon water.” This is the multicollinear-legs problem in another disguise."
  - id: triptych
    prompt: "What is a *triptych* plot and why use it for continuous interactions?"
    options:
      - text: "A set of panels showing the predicted relationship between outcome and one predictor at several fixed values (e.g. low, medium, high) of the other, so you can see how the slope changes"
        correct: true
      - text: "A plot of three posterior distributions"
      - text: "A plot of residuals"
      - text: "Three plots of the same data with different colors"
    explanation: "Section 8.3.3 (p. 257–258) plots blooms against water at shade = −1, 0 and 1. “the utility lies in making multiple figures that allow one to see how the interaction alters a slope, across changes in a chosen variable” (p. 253). Without the interaction, the slopes are parallel across panels; with it, water's slope flattens as shade increases (Figure 8.7). For more than three values, small multiples or heatmaps do the same job."
  - id: prior-triptych
    prompt: "Why does the book also draw triptych plots from the *prior* (Figure 8.8)?"
    options:
      - text: "Because posterior plots are unreliable"
      - text: "To show what interactions the priors allow before seeing data, checking that the priors are plausible and not overly constraining"
        correct: true
      - text: "To choose priors from the data"
      - text: "To compute WAIC"
    explanation: "Prior predictive simulation applies to interactions too: drawing lines from the prior at each shade level shows that the priors allow a wide range of slopes and interactions, mostly within the possible outcome range (p. 259–260). With interaction terms you cannot easily reason about priors from the formula alone."
  - id: interaction-telemetry
    prompt: "You suspect that the effect of connection-pool size on throughput depends on database CPU headroom. Following this section, what is a good analysis plan?"
    options:
      - text: "Fit throughput ~ pool size only and report the slope"
      - text: "Fit separate models for each CPU level and compare p-values"
      - text: "Center both predictors, scale throughput meaningfully, set weakly informative priors bounded by the outcome range, include a pool × headroom product term, and draw triptych plots at low, medium and high headroom"
        correct: true
      - text: "Use only the interaction term without main effects"
    explanation: "This is the tulip recipe applied to a system. Centering makes main effects interpretable at average conditions; scaling keeps meaningful zeros; bounded priors prevent absurd effects; the product term lets the pool-size slope vary with headroom; triptychs show how. If the interaction is real, the pool-size slope will flatten when the database has no headroom — exactly like water without light."
---

Interactions between two continuous variables: slopes that change continuously.

## Learning objectives

- Build a continuous interaction from a slope that is itself a linear model
- Set priors for interaction models from the outcome's range
- Explain why only one interaction parameter is identifiable
- Use triptych plots for priors and posteriors

## In a nutshell

```text
Bᵢ ~ Normal(μᵢ, σ)
μᵢ = α + βW Wᵢ + βS Sᵢ + βWS Wᵢ Sᵢ
α  ~ Normal(0.5, 0.25)
βW, βS, βWS ~ Normal(0, 0.25)
σ  ~ Exponential(1)
```

| Shade | Water slope without interaction | Water slope with interaction |
|---|---|---|
| Low (−1) | Same | Steep |
| Mean (0) | Same | Moderate |
| High (+1) | Same | Nearly flat |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 8.3, pp. 252–260; Figures 8.7–8.8.
- Brambor T, Clark WR, Golder M. Understanding interaction models: improving empirical analyses. *Political Analysis* 2006.
