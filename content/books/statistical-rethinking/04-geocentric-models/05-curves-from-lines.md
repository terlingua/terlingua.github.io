---
title: "4.5 Curves From Lines"
questions:
  - id: two-methods
    prompt: "Which two methods does the book use to build curves with linear regression, and what do they share?"
    options:
      - text: "Polynomial regression and B-splines; both transform one predictor into several synthetic predictor variables"
        correct: true
      - text: "Logistic and Poisson regression; both use link functions"
      - text: "Neural networks and decision trees; both are nonparametric"
      - text: "Kernel smoothing and LOESS; both are local averages"
    explanation: "**p. 110:** “The first is polynomial regression. The second is b-splines. Both approaches work by transforming a single predictor variable into several synthetic variables. But splines have some clear advantages.” And importantly: “Neither approach aims to do more than describe the function that relates one variable to another.” These are descriptive (geocentric) devices, not causal models."
  - id: standardize
    prompt: "Why does the book standardize weight before fitting a polynomial?"
    options:
      - text: "Standardizing changes the shape of the relationship"
      - text: "Squares and cubes of large values can be huge and cause numerical glitches; standardizing (subtract mean, divide by SD) largely resolves this and should be the default"
        correct: true
      - text: "Polynomials require normally distributed predictors"
      - text: "To make the intercept zero"
    explanation: "**p. 111:** “When predictor variables have very large values in them, there are sometimes numerical glitches ... These problems are very common for polynomial regression, because the square or cube of a large number can be truly massive. Standardizing largely resolves this issue. It should be your default behavior.” Engineers will recognize this as feature scaling: an optimizer handles inputs near 0 with unit spread far better than raw timestamps or byte counts."
  - id: still-linear
    prompt: "Is μᵢ = α + β₁xᵢ + β₂xᵢ² a *linear* model?"
    options:
      - text: "Only if β₂ = 0"
      - text: "No, because it contains x²"
      - text: "Yes — 'linear' here means μ is a linear function of each parameter, even though the curve in x is not a straight line"
        correct: true
      - text: "Only if x is standardized"
    explanation: "From the box “Linear, additive, funky” (p. 113): “What “linear” means in this context is that µi is a linear function of any single parameter. Such models have the advantage of being easier to fit to data.” But: “They have the disadvantage of being used thoughtlessly ... We should feel embarrassed to use them, just so we don’t become satisfied with the phenomenological explanations they provide.” The same word 'linear' means different things to a statistician and to an algorithms engineer; be explicit."
  - id: better-fit
    prompt: "The cubic model fits the !Kung data better than the parabola, which fits better than a line. Which problems does the book point out?"
    type: multiple
    options:
      - text: "A better fit to the sample might not be a better model"
        correct: true
      - text: "The models contain no biological information, so they teach no causal relationship"
        correct: true
      - text: "Cubic models cannot be fitted with quap"
      - text: "Polynomial parameters are hard to interpret without plotting"
        correct: true
    explanation: "**p. 113:** “First, a better fit to the sample might not actually be a better model. That’s the subject of Chapter 7. Second, the model contains no biological information. We aren’t learning any causal relationship between height and weight.” And p. 112: the β₁ and β₂ parameters are the linear and square components of the curve, “But that doesn’t make them transparent. You have to plot these model fits to understand what they are saying.” A 5th-degree polynomial fitted to request volume over the day will nail yesterday and explode at midnight."
  - id: polynomial-global
    prompt: "What is a key weakness of polynomials compared with splines?"
    options:
      - text: "Polynomials cannot be fitted with Bayesian methods"
      - text: "Polynomials are always underfit"
      - text: "Polynomials need more data than splines"
      - text: "Each polynomial parameter affects the shape of the curve everywhere, so fitting one region distorts others and predictions blow up at the edges"
        correct: true
    explanation: "A polynomial term like x³ acts across the whole range. To bend the curve in one place, the model must accept changes everywhere — the reason polynomials swing wildly near and beyond the edges of the data. Splines instead build the curve from local pieces, so each weight affects only its region. Think global mutable state versus scoped variables: local influence is easier to reason about."
  - id: bspline-idea
    prompt: "What is the core idea of a B-spline?"
    options:
      - text: "Divide the predictor's range into parts using knots, create synthetic basis functions that turn each parameter on and off within a range, and add up the weighted basis functions to get a smooth wiggly curve"
        correct: true
      - text: "Fit a separate regression to each data point"
      - text: "Use a single high-degree polynomial"
      - text: "Average neighboring observations"
    explanation: "**p. 115:** “B-splines ... divide the full range of some predictor variable, like year, into parts. Then they assign a parameter to each part. These parameters are gradually turned on and off in a way that makes their sum into a fancy, wiggly curve.” The model is still a linear regression: μᵢ = α + Σₖ wₖ Bᵢ,ₖ, with basis functions as synthetic predictors and weights as slopes. In the cherry blossom data it reveals a change in trend around 1800 and structure before it."
  - id: knots
    prompt: "Who is responsible for choosing knot locations and the number of knots?"
    options:
      - text: "The fitting algorithm chooses them automatically and optimally"
      - text: "You are — knot locations are part of the model; a common choice is evenly spaced quantiles of the predictor, which puts more knots where data are dense"
        correct: true
      - text: "They must be chosen by cross-validation"
      - text: "They are always evenly spaced"
    explanation: "**p. 117:** “You can, in principle, put the knots wherever you like. Their locations are part of the model, and you are responsible for them.” The book places 15 knots at quantiles of year, so periods with more records get more flexibility. More knots means more flexibility; the priors on the weights (w ~ Normal(0, 10)) control how wiggly the spline can get. Choosing histogram bucket boundaries for a latency metric is the same kind of design decision."
  - id: degree
    prompt: "What does the spline's polynomial *degree* control?"
    options:
      - text: "The order of the prior"
      - text: "The number of knots"
      - text: "How many basis functions combine at each point: degree 1 → two, degree 2 → three, degree 3 → four"
        correct: true
      - text: "The number of data points used"
    explanation: "**p. 117:** “For degree 1, as in Figure 4.12, two basis functions combine at each point. For degree 2, three functions combine at each point. For degree 3, four combine.” Higher degree gives smoother transitions between local pieces. Degree 1 gives piecewise linear segments, like linear interpolation between dashboard samples; degree 3 gives smooth curves."
  - id: exponential-prior
    prompt: "The spline model introduces `σ ~ Exponential(1)`. How should you read an exponential prior on a scale parameter?"
    options:
      - text: "As saying σ is exactly 1"
      - text: "As saying σ is uniformly distributed"
      - text: "As an improper prior"
      - text: "As a positive-only prior carrying little more information than an average deviation (the mean is 1/rate)"
        correct: true
    explanation: "**p. 118–119:** “Exponential distributions are useful priors for scale parameters, parameters that must be positive.” and “The way to read an exponential distribution is to think of it as containing no more information than an average deviation.” The mean is the inverse of the rate, so Exponential(1) has mean 1. **p. 119:** “We’ll use exponential priors for the rest of the book, in place of uniform priors. It is much more common to have a sense of the average deviation than of the maximum.” Uniform(0, 50) required knowing a hard maximum; most engineers know typical jitter, not the worst case."
  - id: matrix
    prompt: "In the spline code, `mu <- a + B %*% w` uses matrix multiplication. What does it compute for each row (year)?"
    options:
      - text: "The sum over basis functions of the basis value on that row times its weight"
        correct: true
      - text: "The determinant of B"
      - text: "The product of all weights"
      - text: "The inverse of B times w"
    explanation: "**p. 120:** the notation means “(1) multiplying each element of the vector w by each value in the corresponding row of B and then (2) summing up each result.” It is a dot product per row. In C#, `B.Select(row => a + row.Zip(w, (b, wk) => b * wk).Sum())`. Matrix notation is just a compact loop."
  - id: gam
    prompt: "What broader class of models do splines lead into, according to the end of the section?"
    options:
      - text: "Neural networks"
      - text: "Generalized additive models (GAMs), which predict outcomes with smooth functions of predictors"
        correct: true
      - text: "Support vector machines"
      - text: "Mixture models"
    explanation: "**p. 120:** “A entire class of models, generalized additive models (GAMs), focuses on predicting an outcome variable using smooth functions of some predictor variables. The topic is deep enough to deserve its own book.” GAMs are a practical workhorse for seasonal telemetry — e.g. traffic as a smooth function of hour-of-day plus a smooth function of day-of-year — and are what forecasting tools such as Prophet build on."
---

Straight lines are not special. Two ways to bend them while keeping the machinery of linear regression.

## Learning objectives

- Fit and interpret polynomial regressions with standardized predictors
- Explain what 'linear' means in 'linear model'
- Explain how B-splines build curves from local basis functions and knots
- Read an exponential prior for scale parameters

## In a nutshell

| Method | Synthetic predictors | Influence of each parameter | Risk |
|---|---|---|---|
| Polynomial | x, x², x³ … | Global | Wild behavior at edges; uninterpretable |
| B-spline | Basis functions B₁…B_K over knots | Local | Knot choice; overfitting if weights unconstrained |

```text
Dᵢ ~ Normal(μᵢ, σ)
μᵢ = α + Σₖ wₖ Bᵢ,ₖ
α  ~ Normal(100, 10)
wₖ ~ Normal(0, 10)
σ  ~ Exponential(1)
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 4.5, pp. 110–120; Figures 4.11–4.13.
- Wood SN. *Generalized Additive Models: An Introduction with R*, 2nd ed. CRC Press, 2017.
- Aono Y, Kazui K. Phenological data series of cherry tree flowering in Kyoto, Japan. *International Journal of Climatology* 2008.
- Taylor SJ, Letham B. Forecasting at scale. *The American Statistician* 2018. [doi:10.1080/00031305.2017.1380080](https://doi.org/10.1080/00031305.2017.1380080)
