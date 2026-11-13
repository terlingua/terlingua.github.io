---
title: "7.1 The Problem With Parameters"
questions:
  - id: ulysses
    prompt: "What do Scylla and Charybdis represent in the chapter's metaphor of Ulysses' compass?"
    options:
      - text: "Overfitting (learning too much from the data) and underfitting (learning too little), both of which produce poor predictions"
        correct: true
      - text: "Bayesian and frequentist statistics"
      - text: "Bias and measurement error"
      - text: "Priors and likelihoods"
    explanation: "**p. 192:** “(1) The many-headed beast of overfitting, which leads to poor prediction by learning too much from the data (2) The whirlpool of underfitting, which leads to poor prediction by learning too little from the data”. There is a third monster, confounding: “confounded models can in fact produce better predictions than models that correctly measure a causal relationship ... we must decide whether we want to understand causes or rather just predict.” The book replaces Ockham's razor (prefer simpler) with a compass for steering between the two errors."
  - id: copernicus
    prompt: "Why does the chapter say Copernicus's argument for his heliocentric model looks poor today?"
    options:
      - text: "Because his model was wrong about the Sun"
      - text: "Because his model was neither particularly harmonious nor more accurate; it still used epicycles and made the same predictions as the geocentric model"
        correct: true
      - text: "Because he used too few parameters"
      - text: "Because he relied on p-values"
    explanation: "**p. 191:** “The model was neither particularly harmonious nor more accurate than the geocentric model.” Both were effectively Fourier series approximating periodic motion and made the same predictions. Choosing between models on 'simplicity' alone is vague; the chapter develops a principled criterion — expected predictive accuracy — instead."
  - id: stargazing
    prompt: "What is 'stargazing' in model selection, and why is it a mistake?"
    options:
      - text: "Choosing the model with the most parameters"
      - text: "Choosing models by their priors"
      - text: "Choosing the model in which every coefficient is statistically significant; p-values are not designed to navigate between underfitting and overfitting"
        correct: true
      - text: "Choosing models with the lowest WAIC"
    explanation: "**p. 193:** “The most common form of model selection among practicing scientists is to search for a model in which every coefficient is statistically significant ... using it to select among structurally different models is a mistake—p-values are not designed to help you navigate between underfitting and overfitting.” Feature selection by p-value in a churn model is the same mistake."
  - id: r-squared
    prompt: "Why is R² a poor criterion for choosing among models?"
    options:
      - text: "It is hard to compute"
      - text: "It decreases as variables are added"
      - text: "It only works for logistic regression"
      - text: "Like other measures of fit to sample, it increases as predictors are added, even when they are random numbers unrelated to the outcome"
        correct: true
    explanation: "**p. 194:** “Like other measures of fit to sample,” R² “increases as more predictor variables are added. This is true even when the variables you add to a model are just random numbers, with no relation to the outcome. So it’s no good to choose among models using only fit to the data.” Training accuracy in ML, or how well a capacity model fits last quarter, has the same flaw."
  - id: polynomial-hominins
    prompt: "Six polynomial models are fit to brain volume versus body mass for seven hominin species. The sixth-degree polynomial has R² = 1. What is wrong with it?"
    options:
      - text: "It passes through every point, so it has no residual variance, but it makes absurd predictions between and beyond points (including negative brain volumes)"
        correct: true
      - text: "Nothing; it is the best model"
      - text: "It has too few parameters"
      - text: "It cannot be fitted"
    explanation: "**p. 199:** “The sixth-degree polynomial actually does pass through every point, and it has no residual variance. It’s a perfect fit, R² = 1.” σ had to be fixed at 0.001 because it would otherwise shrink to zero. With seven data points and seven parameters, the model memorizes the data. Like a test that hard-codes expected outputs per input: perfect on known cases, meaningless on new ones."
  - id: compression
    prompt: "In the box “Model fitting as compression”, why does a model with one parameter per data point teach us nothing?"
    options:
      - text: "Because it compresses too much"
      - text: "Because it achieves no compression: it just encodes the raw data in another form, so it captures no regular structure"
        correct: true
      - text: "Because parameters cannot be stored"
      - text: "Because it has no prior"
    explanation: "**p. 201:** “When a model has a parameter to correspond to each datum, such as m7.6, then there is actually no compression. The model just encodes the raw data in a different form ... Learning about the data requires using a simpler model that achieves some compression, but not too much.” This is the Minimum Description Length idea. For developers: a lookup table is not a model; a good model is a small program that regenerates most of the data."
  - id: under-sensitive
    prompt: "Figure 7.4 refits models after dropping one point at a time. What distinguishes underfit from overfit models?"
    options:
      - text: "Underfit models change dramatically; overfit models barely change"
      - text: "Both change dramatically"
      - text: "An underfit model is insensitive to the sample, changing little when a point is dropped; an overfit model is very sensitive, changing dramatically"
        correct: true
      - text: "Neither changes"
    explanation: "**p. 201–202:** underfitting “produces models that are inaccurate both within and out of sample ... it is insensitive to the sample. We could remove any one point from the sample and get almost the same regression line. In contrast, the most complex model, m7.6, is very sensitive to the sample.” Sensitivity to individual data points is a practical overfitting diagnostic, and it foreshadows leave-one-out cross-validation."
  - id: regular-irregular
    prompt: "In the chapter's framing, what does a model learn when it overfits?"
    options:
      - text: "Only the regular features of the sample"
      - text: "Nothing"
      - text: "Only the priors"
      - text: "Both the regular features that generalize and the irregular features (noise) specific to that sample"
        correct: true
    explanation: "**p. 194:** “there are both regular and irregular features in every sample. The regular features are the targets of our learning, because they generalize well or answer a question of interest ... The irregular features are instead aspects of the data that do not generalize and so may mislead us.” An alerting threshold tuned to last week's exact traffic learns last week's quirks (a one-off batch job) along with the real daily pattern."
---

Adding parameters always improves fit to the sample, and often makes predictions worse.

## Learning objectives

- Describe overfitting, underfitting and confounding as distinct errors
- Explain why R² and stargazing are poor model-selection criteria
- Interpret the hominin polynomial example and the compression view
- Diagnose under- and over-sensitivity by dropping points

## In a nutshell

| Degree | R² | Behavior |
|---|---|---|
| 1 | 0.51 | Underfit: stable, misses curvature |
| 4 | 0.82 | Bends to fit points |
| 6 | 1.00 | Passes through every point; absurd elsewhere |

## For engineers

Every model of past telemetry is compressing it. If the model is as large as the data (a rule per incident,
a threshold per hour of the week tuned on one week), it has learned nothing that will carry to next week.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 7 opening and Section 7.1, pp. 191–202; Figures 7.1–7.4.
- Grünwald PD. *The Minimum Description Length Principle*. MIT Press, 2007.
