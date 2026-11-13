---
title: "16.1 Geometric People"
questions:
  - id: hunting-joke
    prompt: "What is the point of the joke about the physicist, engineer and statistician hunting a deer?"
    options:
      - text: "Statisticians are always right"
      - text: "Scientific models fail in precise, informative ways; averaging two wrong answers is 'right' on average but teaches nothing about cause"
        correct: true
      - text: "Physicists cannot hunt"
      - text: "Engineers should not use fudge factors"
    explanation: "**p. 525:** scientific models “make precise predictions, and can fail in equally precise ways. Being wrong in precise ways is useful, because the failures borrow meaning from the cause and effect relationships built into the models.” Applied statistics “focuses on average performance, regardless of the model.” A capacity formula derived from queueing theory that underpredicts tells you which assumption broke; an averaged ML ensemble that is right on average does not."
  - id: glm-problems
    prompt: "Which problems with the 'GLMs plus DAGs' approach does the chapter identify?"
    type: multiple
    options:
      - text: "Not everything can be modelled as a linear combination mapped onto an outcome"
        correct: true
      - text: "Theory may fix some parameters at special values, which a GLM-first workflow never notices"
        correct: true
      - text: "When a GLM fails it is hard to learn from the failure — debugging epicycles is a game no one can win"
        correct: true
      - text: "GLMs cannot be fitted with MCMC"
    explanation: "**p. 525:** “Not everything can be modeled as a GLM ... Other times the theory of interest can be expressed as a GLM, but the theory implies that some of the parameters are fixed at special values. We might never notice, if we start with GLMs instead of real models. And when a GLM fails, it’s not easy to learn from the failure. Debugging epicycles is a game no one can win.”"
  - id: bespoke
    prompt: "What does the box “Bespoken for” say about off-the-shelf models?"
    options:
      - text: "They are always best"
      - text: "They are illegal in science"
      - text: "Like mass-produced clothes, they rarely fit well; worse, they interrupt expertise, because standard GLMs give scientists no way to incorporate what they know"
        correct: true
      - text: "They are more accurate than bespoke models"
    explanation: "**p. 526:** “One problem with off-the-shelf models is that they interrupt expertise ... when those scientists turn to analyze their own data, they use tools that forbid the use of that knowledge.” And: “GLMs are unreasonably powerful. But we should remember that they are usually only geocentric devices.” Engineers who know Little's law, Amdahl's law or a cache's eviction policy should put that knowledge into their models."
  - id: cylinder
    prompt: "In the 'Vitruvian Can' model, how is a person's expected weight derived from height?"
    options:
      - text: "As a linear function of height"
      - text: "As a spline"
      - text: "As height squared"
      - text: "Treat the person as a cylinder with radius r = p·h; volume V = π(ph)²h = πp²h³; weight W = kV = kπp²h³"
        correct: true
    explanation: "**p. 527:** the cylinder model gives W = kV = kπp²h³ “and this is our formula for expected weight, given an individual’s height h. This is not obviously an ordinary generalized linear model. But that’s okay. It has a causal structure, it makes predictions, and we can fit it to data.” Weight is a consequence of height and shape, not the reverse."
  - id: k-p-identify
    prompt: "Only the product kp² appears in the model. How does the book still estimate k and p?"
    options:
      - text: "With informative priors from their scientific meaning: p is radius/height, between 0 and well below 0.5 (Beta(2, 18)); k is mass per volume, set from known scales"
        correct: true
      - text: "It cannot; the model is useless"
      - text: "By fixing k = 1 arbitrarily without reason"
      - text: "By using flat priors"
    explanation: "**p. 528:** “k and p, given this model and these data, are not identifiable. We could just replace the product kp² with a new parameter θ ... What we won’t get is an easy way to assign a prior to θ.” Because parameters have meanings, priors are easy: p “is almost certainly less than one-half” so p ~ Beta(2, 18). Figure 16.2 shows the posterior ridge in (k, p) that the priors tame. Meaningful parameters are a feature, even when not identified by data alone."
  - id: dimensionless
    prompt: "Why does the book divide height and weight by their means?"
    options:
      - text: "To standardize them to z-scores"
      - text: "To remove arbitrary measurement units, so priors can be set from known biological constraints without needing unit conversions"
        correct: true
      - text: "Because the data are skewed"
      - text: "For computational speed only"
    explanation: "**p. 529:** “get rid of the measurement scales altogether—measurement scales are arbitrary human inventions—and then use the known biological constraints to locate the prior.” After scaling, a person of average height has average weight, which pins kπp² near 1. Dimensionless ratios (utilization, relative latency) often make priors in systems models much easier."
  - id: misfit-informative
    prompt: "The cylinder model fits poorly at short heights (children). Why is this misfit useful?"
    options:
      - text: "It isn't; the model should be discarded"
      - text: "Because children are outliers to remove"
      - text: "Because the model has causal meaning, the failure points to which assumption broke — e.g. body proportions p and composition k change during growth"
        correct: true
      - text: "Because it improves WAIC"
    explanation: "**p. 530:** “k isn’t necessarily a constant, because relative muscle mass isn’t a constant. Similarly, the ratio of body width to height isn’t constant over development. So p may change as well.” A misfit of a mechanistic model is a diagnosis; a misfit of a polynomial is just a misfit."
  - id: log-linear
    prompt: "On the log scale, the cylinder model is log wᵢ = log k + log π + 2 log p + 3 log hᵢ. What does this show?"
    options:
      - text: "That the model is wrong"
      - text: "That k and p are identified"
      - text: "That logarithms are unnecessary"
      - text: "That many natural relationships are GLMs on some scale — here a linear regression with the coefficient on log height fixed at 3 by theory, not estimated"
        correct: true
    explanation: "**p. 531:** “On the log scale, this is a linear regression ... the term 3 log(hi) is a predictor variable with a fixed coefficient of 3. Theory gave us the value of that coefficient. We didn’t need to estimate it.” A GLM user would estimate that slope freely and learn less. Power laws in systems (e.g. Zipf-distributed keys) are similar: theory fixes or constrains exponents."
---

The chapter's turn: from geocentric GLMs to models built from scientific structure, starting with a person
shaped like a can.

## Learning objectives

- Explain why mechanistic models fail informatively
- Derive a weight-from-height model from geometry
- Use parameter meanings to set priors for non-identified products
- Remove units with reference values

## In a nutshell

```text
Wᵢ ~ LogNormal(μᵢ, σ)
exp(μᵢ) = k π p² hᵢ³
p ~ Beta(2, 18)
k ~ Exponential(0.5)        (after scaling by means)
σ ~ Exponential(1)
```

| GLM view | Scientific view |
|---|---|
| log w = α + β log h, estimate β | β = 3 from geometry |
| α is a nuisance | α = log(kπp²) has parts with meaning |
| Misfit means 'add terms' | Misfit means 'which assumption failed?' |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 16 opening and Section 16.1, pp. 525–531; Figures 16.1–16.2.
- West GB, Brown JH, Enquist BJ. A general model for the origin of allometric scaling laws in biology. *Science* 1997.
