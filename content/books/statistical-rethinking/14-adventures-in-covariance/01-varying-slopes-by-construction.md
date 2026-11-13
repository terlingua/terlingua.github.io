---
title: "14.1 Varying Slopes by Construction"
questions:
  - id: pool-slopes
    prompt: "Why should the coffee robot pool information about *slopes* (afternoon minus morning wait) across cafés, not just intercepts?"
    options:
      - text: "Slopes cannot vary"
      - text: "Any batch of parameters with exchangeable index values can and probably should be pooled; slopes vary by café just as intercepts do"
        correct: true
      - text: "Only slopes need pooling"
      - text: "Because the data are balanced"
    explanation: "**p. 435:** “This is the essence of the general varying effects strategy: Any batch of parameters with exchangeable index values can and probably should be pooled. Exchangeable just means the index values have no true ordering, because they are arbitrary labels. There’s nothing special about intercepts.” The effect of a new deploy on latency can differ by service; pooling those per-service effects improves each estimate."
  - id: covary
    prompt: "Why do cafés' intercepts and slopes covary, and why does that help?"
    options:
      - text: "They don't covary"
      - text: "Because the robot visits in the morning first"
      - text: "Busy cafés have long morning waits and big afternoon improvements; quiet cafés have short waits and little change. Knowing this correlation, a long morning wait at a new café predicts a bigger afternoon improvement"
        correct: true
      - text: "Because covariance is required by Stan"
    explanation: "**p. 435–436:** “In the population of cafés, a long wait in the morning is associated with a shorter wait in the afternoon ... If we can figure out a way to pool information across parameter types—intercepts and slopes—what the robot learns in the morning can improve learning about afternoons, and vice versa.” Services with high baseline latency often benefit most from a caching change: intercepts and slopes correlate."
  - id: interaction-machines
    prompt: "In what sense are varying slopes models 'massive interaction machines'?"
    options:
      - text: "They include every pairwise interaction term automatically"
      - text: "They require interaction terms in the data"
      - text: "They interact with the user"
      - text: "They allow every unit to have its own response to a treatment or exposure, while pooling to improve estimates — an interaction between unit and predictor"
        correct: true
    explanation: "**p. 436:** “varying slopes models are massive interaction machines. They allow every unit in the data to have its own response to any treatment or exposure or event, while also improving estimates via pooling. When the variation in slopes is large, the average slope is of less interest. Sometimes, the pattern of variation in slopes provides hints about omitted variables.”"
  - id: mvnormal
    prompt: "How are correlated varying intercepts and slopes given a joint prior?"
    options:
      - text: "A two-dimensional multivariate Gaussian with means (ᾱ, β̄) and a covariance matrix built from σα, σβ and a correlation ρ"
        correct: true
      - text: "Two independent Normal priors"
      - text: "A uniform prior over all pairs"
      - text: "A Poisson"
    explanation: "**p. 437:** “the device that makes this possible is a joint multivariate Gaussian distribution for all of the varying effects, both intercepts and slopes.” The covariance matrix has σα² and σβ² on the diagonal and σασβρ off the diagonal (p. 438). The book builds it as diag(σ) R diag(σ), separating standard deviations from the correlation matrix R."
  - id: lkj
    prompt: "What does an LKJcorr(η) prior do?"
    options:
      - text: "It fixes the correlation at η"
      - text: "It is a prior over correlation matrices; η = 1 is flat over valid matrices, and η > 1 is increasingly skeptical of extreme correlations near −1 and 1"
        correct: true
      - text: "It is a prior on standard deviations"
      - text: "It only works for 2 × 2 matrices"
    explanation: "**p. 442:** “LKJcorr(2) does is define a weakly informative prior on ρ that is skeptical of extreme correlations near −1 or 1. You can think of it as a regularizing prior for correlation matrices ... When we use LKJcorr(1), the prior is flat over all valid correlation matrices.” Figure 14.3 shows densities for η = 1, 2, 4."
  - id: learned-rho
    prompt: "The model never sees the true café intercepts and slopes. How does it recover the negative correlation?"
    options:
      - text: "From the prior"
      - text: "It cannot"
      - text: "From the observed morning and afternoon waits: across cafés, those with long morning waits show larger drops, and the joint model learns ρ from that pattern"
        correct: true
      - text: "From the true parameters supplied in the data"
    explanation: "**p. 443:** “The posterior is concentrated on negative values, because the model has learned the negative correlation ... Keep in mind that the model did not get to see the true intercepts and slopes. All it had to work from was the observed wait times in morning and afternoon.” The book suggests re-running with LKJcorr(1) and LKJcorr(4) to see the prior's influence."
  - id: shrinkage-2d
    prompt: "What does shrinkage look like in two dimensions (Figure 14.5)?"
    options:
      - text: "Each dimension shrinks independently toward its mean"
      - text: "Only slopes shrink"
      - text: "Nothing shrinks"
      - text: "Estimates are pulled toward the center of the inferred bivariate population along its correlation structure; cafés far from the center move most, and a surprising intercept also moves the slope"
        correct: true
    explanation: "Raw unpooled estimates (blue) move toward the inferred population ellipse. Because intercepts and slopes are correlated, an unusual intercept also pulls the slope estimate. On the outcome scale (morning versus afternoon waits), the same shrinkage appears. Information flows across parameter types."
  - id: misspecification
    prompt: "What does the box “Simulation and misspecification” say about Bayesian models and true data-generating processes?"
    options:
      - text: "Real data are always analyzed with misspecified models; simulation can explore how models perform when they do not match the process, and Bayesian inference does not depend on the likelihood being literally true"
        correct: true
      - text: "Bayesian inference requires the likelihood to be true"
      - text: "Misspecification cannot be studied"
      - text: "Simulated data are always misspecified"
    explanation: "**p. 441:** “in the real world, we’re never so lucky. Instead we are always forced to analyze data with a model that is misspecified ... Simulation can be used however to explore misspecification ... In a Bayesian model, a likelihood is a prior for the data, and inference about parameters can be surprisingly insensitive to its details.” Chaos testing for statistical models: simulate the wrong world and see if your analysis breaks."
---

Pooling slopes as well as intercepts — and pooling across them through their correlation.

## Learning objectives

- Explain why slopes, like intercepts, should be pooled
- Build a multivariate Gaussian prior with standard deviations and a correlation matrix
- Interpret the LKJ prior
- Explain two-dimensional shrinkage

## In a nutshell

```text
Wᵢ ~ Normal(μᵢ, σ)
μᵢ = α[café] + β[café] Aᵢ
[α_j, β_j] ~ MVNormal([ᾱ, β̄], S)
S = diag(σα, σβ) · R · diag(σα, σβ)
ᾱ ~ Normal(5, 2);  β̄ ~ Normal(−1, 0.5)
σ, σα, σβ ~ Exponential(1);  R ~ LKJcorr(2)
```

| Parameter | Meaning for engineers |
|---|---|
| σα | How much baseline latency varies across services |
| σβ | How much the effect of a change varies across services |
| ρ | Whether slow services benefit more (negative ρ) or less |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 14 opening and Section 14.1, pp. 435–447; Figures 14.1–14.5.
- Lewandowski D, Kurowicka D, Joe H. Generating random correlation matrices based on vines and extended onion method. *Journal of Multivariate Analysis* 2009.
