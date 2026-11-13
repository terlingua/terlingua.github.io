---
title: "7.3 Golem Taming: Regularization"
questions:
  - id: worse-fit-better-prediction
    prompt: "How can making a model *worse* at fitting the sample produce better predictions?"
    options:
      - text: "It cannot; better fit always means better prediction"
      - text: "Worse fit makes the posterior Gaussian"
      - text: "Worse fit increases the number of parameters"
      - text: "A skeptical prior slows learning from the sample, so the model absorbs less of the sample's irregular features (noise) and generalizes better"
        correct: true
    explanation: "**p. 214:** “The root of overfitting is a model’s tendency to get overexcited by the training sample. When the priors are flat or nearly flat, the machine interprets this to mean that every parameter value is equally plausible.” A skeptical prior “slows the rate of learning from the sample.” This is regularization. In ML engineering: weight decay, dropout and early stopping all deliberately reduce training fit to improve test performance."
  - id: normal-0-1
    prompt: "With a standardized predictor, what does the prior β ~ Normal(0, 1) say?"
    options:
      - text: "That before seeing data, a 1 SD change in x is very unlikely to change the outcome by more than about 2 units; values beyond ±2 get only about 5% prior plausibility"
        correct: true
      - text: "That β must be positive"
      - text: "That β is exactly zero"
      - text: "That β is uniform from −1 to 1"
    explanation: "**p. 215:** the prior “says that, before seeing the data, the machine should be very skeptical of values above 2 and below −2, as a Gaussian prior with a standard deviation of 1 assigns only 5% plausibility to values above and below 2 standard deviations.” Narrower priors (σ = 0.5, 0.2) are more skeptical. Because more mass sits near zero, estimates are “shrunk towards zero—they are conservative.”"
  - id: prior-strength-sample
    prompt: "How does the benefit of regularizing priors depend on sample size (Figure 7.8)?"
    options:
      - text: "Priors matter equally at all sample sizes"
      - text: "Priors help most with small samples; with N = 100 the data overwhelm even fairly strong priors, and overfitting was less of a concern anyway"
        correct: true
      - text: "Priors matter more with larger samples"
      - text: "Priors only matter for sample sizes above 1,000"
    explanation: "**p. 216:** with N = 100, “The priors have much less of an effect here, because there is so much more evidence. The priors do help. But overfitting was less of a concern to begin with, and there is enough information in the data to overwhelm even the Normal(0, 0.2) prior.” For low-traffic services, new features or rare error types, regularization matters a lot; for high-volume metrics, less so."
  - id: too-skeptical
    prompt: "What is the danger of a regularizing prior that is too skeptical?"
    options:
      - text: "It causes overfitting"
      - text: "It makes MCMC fail"
      - text: "It prevents the model from learning from the data — underfitting"
        correct: true
      - text: "There is no danger; tighter is always better"
    explanation: "**p. 216:** “Regularizing priors are great, because they reduce overfitting. But if they are too skeptical, they prevent the model from learning from the data.” Steering between overfitting and underfitting applies to priors too. The fix the book points to: multilevel models, where “their central device is to learn the strength of the prior from the data itself.”"
  - id: adaptive-regularization
    prompt: "How does the book describe multilevel models in terms of regularization?"
    options:
      - text: "As models without priors"
      - text: "As models that always overfit"
      - text: "As models with flat priors"
      - text: "As adaptive regularization: the model learns from the data how skeptical it should be"
        correct: true
    explanation: "**p. 216:** “you can think of multilevel models as adaptive regularization, where the model itself tries to learn how skeptical it should be.” Instead of guessing σ = 0.5, the model estimates the spread of parameters across groups and shrinks each group's estimate accordingly (Chapter 13). Think of it as an autotuned penalty."
  - id: ridge
    prompt: "What is the relationship between Gaussian priors centered at zero and ridge regression?"
    options:
      - text: "Linear models with Gaussian priors centered at zero on the slopes correspond to ridge regression, whose penalty λ plays the role of the prior's precision"
        correct: true
      - text: "Ridge regression is a Bayesian method that requires MCMC"
      - text: "They are unrelated"
      - text: "Ridge regression uses uniform priors"
    explanation: "From the box “Ridge regression” (p. 216): models where slopes have zero-centered Gaussian priors “are sometimes known as ridge regression. Ridge regression typically takes as input a precision λ that essentially describes the narrowness of the prior.” The book notes machine learning is ahead of traditional statistics here: “regularization is one area where machine learning is more mature.” If you have used L2 regularization, you have used a Gaussian prior."
  - id: regularize-telemetry
    prompt: "You estimate a per-endpoint latency regression from 40 data points per endpoint with 15 candidate predictors. Which approach does this section support?"
    options:
      - text: "Flat priors so the data speak for themselves"
      - text: "Standardize predictors and use skeptical priors centered at zero (e.g. Normal(0, 0.5)), then check predictive accuracy with cross-validation or PSIS"
        correct: true
      - text: "Drop all predictors except the most significant"
      - text: "Use the polynomial of highest degree that converges"
    explanation: "With small N and many parameters, flat priors maximize overfitting. Skeptical priors shrink unhelpful coefficients toward zero; cross-validation or PSIS (Section 7.4) measures how much overfitting remains. **p. 225:** “Regularizing priors and CV/PSIS/WAIC are complementary. Regularization reduces overfitting, and predictive criteria measure it.”"
---

Skeptical priors are the Bayesian name for regularization.

## Learning objectives

- Explain why flat priors overfit and skeptical priors help
- Read the strength of a Gaussian prior on a standardized slope
- Relate regularization strength to sample size
- Connect zero-centered Gaussian priors to ridge regression and multilevel models

## In a nutshell

| Prior on β | Plausible range (≈95%) | Effect |
|---|---|---|
| Flat | Anything | Maximum overfitting |
| Normal(0, 1) | −2 to 2 | Mild regularization |
| Normal(0, 0.5) | −1 to 1 | Moderate |
| Normal(0, 0.2) | −0.4 to 0.4 | Strong; can underfit |

```text
L2 penalty λ Σ β²   ⇔   β ~ Normal(0, σ) with λ ∝ 1/σ²
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 7.3, pp. 214–217; Figures 7.7–7.8.
- Hoerl AE, Kennard RW. Ridge regression: biased estimation for nonorthogonal problems. *Technometrics* 1970.
- Hastie T, Tibshirani R, Friedman J. *The Elements of Statistical Learning*, 2nd ed. Springer, 2009.
