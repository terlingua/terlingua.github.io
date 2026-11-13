---
title: "7.4 Predicting Predictive Accuracy"
questions:
  - id: two-families
    prompt: "What are the two families of strategies for estimating out-of-sample accuracy without a new sample?"
    options:
      - text: "Bootstrapping and permutation"
      - text: "R² and adjusted R²"
      - text: "Cross-validation and information criteria"
        correct: true
      - text: "p-values and confidence intervals"
    explanation: "**p. 217:** “There are two families of strategies: cross-validation and information criteria. These strategies try to guess how well models will perform, on average, in predicting new data ... Despite subtle differences in their mathematics, they produce extremely similar approximations.”"
  - id: cross-validation
    prompt: "How does cross-validation estimate out-of-sample accuracy?"
    options:
      - text: "By fitting the model once and scoring it on the training data"
      - text: "By comparing priors"
      - text: "By adding noise to the data"
      - text: "By dividing the sample into folds, fitting on all but one fold, scoring on the held-out fold, and averaging across folds"
        correct: true
    explanation: "**p. 217:** “divide the sample in a number of chunks, called “folds.” The model is asked to predict each fold, after training on all the others. We then average over the score for each fold.” Leave-one-out cross-validation (LOOCV) uses each observation as its own fold. The problem: with N observations you must fit N models — expensive. For time series like telemetry, folds should respect time order (train on the past, test on the future)."
  - id: psis
    prompt: "What does Pareto-smoothed importance sampling (PSIS) do?"
    options:
      - text: "It approximates leave-one-out cross-validation from a single fit by reweighting posterior samples by each observation's importance, smoothing the largest weights with a Pareto distribution"
        correct: true
      - text: "It adds a penalty equal to the number of parameters"
      - text: "It samples from the prior"
      - text: "It replaces MCMC"
    explanation: "Importance sampling uses how 'surprising' each observation is (its likelihood) to estimate what the posterior would look like without it. Raw importance weights can be unreliable, so PSIS fits a generalized Pareto distribution to the largest weights (p. 218). “The best feature of PSIS is that it provides feedback about its own reliability. It does this by noting particular observations with very high weights that could make the PSIS score inaccurate.”"
  - id: pareto-k
    prompt: "What does a high Pareto k value (above about 0.7) indicate?"
    options:
      - text: "The model fits perfectly"
      - text: "That observation's importance weights are unreliable — it is highly influential — so the PSIS estimate may be inaccurate"
        correct: true
      - text: "The sample size is large"
      - text: "The prior is too narrow"
    explanation: "**p. 219:** “both theory and simulation suggest PSIS’s weights perform well as long as k < 0.7. When we start using PSIS, you’ll see warnings about large k values. These are very useful for identifying influential observations.” A k above 1 implies the weight distribution has no finite mean. These are the data points that, if removed, would change your model the most — like the one tenant whose traffic dominates your capacity model."
  - id: pointwise-se
    prompt: "Why is the pointwise nature of CV, PSIS and WAIC useful?"
    options:
      - text: "It makes them faster"
      - text: "It removes the need for priors"
      - text: "Each observation gets its own score, which lets you estimate a standard error for the total and find which observations drive overfitting"
        correct: true
      - text: "It makes them exact"
    explanation: "**p. 218:** “This pointwise nature provides an approximate—sometimes very approximate—estimate of the standard error of our estimate of out-of-sample deviance.” s = √(N × var(psisᵢ)). Pointwise penalties also show which observations are hardest to predict — a natural list for investigation."
  - id: twice-params
    prompt: "In the train–test simulations with flat priors, the gap between test and training deviance is about twice the number of parameters. What does this observation underlie?"
    options:
      - text: "Regularizing priors"
      - text: "Cross-validation"
      - text: "R²"
      - text: "Information criteria such as AIC, which estimate out-of-sample deviance as training deviance plus a penalty of about 2 × number of parameters"
        correct: true
    explanation: "**p. 219:** “For ordinary linear regressions with flat priors, the expected overfitting penalty is about twice the number of parameters. This is the phenomenon behind information criteria.” AIC = D_train + 2p. But: “AIC is an approximation that is reliable only when: (1) The priors are flat or overwhelmed by the likelihood. (2) The posterior distribution is approximately multivariate Gaussian. (3) The sample size N is much greater than the number of parameters k.”"
  - id: waic
    prompt: "How is WAIC computed?"
    options:
      - text: "−2 × (lppd − Σᵢ var_θ log p(yᵢ | θ)): the lppd minus a penalty equal to the summed variance of each observation's log-probability across posterior samples"
        correct: true
      - text: "−2 × lppd + 2 × number of parameters"
      - text: "The posterior mean of the deviance"
      - text: "The prior predictive density"
    explanation: "**p. 220:** “WAIC is just the log-posterior-predictive-density ... plus a penalty proportional to the variance in the posterior predictions.” Watanabe's criterion “makes no assumption about the shape of the posterior.” Observations whose predictions vary a lot across posterior samples contribute large penalties. The total penalty is often called the effective number of parameters, pWAIC."
  - id: not-consistent
    prompt: "Information criteria are 'not consistent for model identification'. Does that mean they are broken?"
    options:
      - text: "Yes; they should not be used"
      - text: "No — they aim to choose the model with the best expected predictions, not to identify the 'true' model; they are not broken if you care about prediction"
        correct: true
      - text: "Yes, they always select the simplest model"
      - text: "No, because they are consistent for causal inference"
    explanation: "**p. 221:** “information criteria are not consistent for model identification. These criteria aim to nominate the model that will produce the best predictions ... So are information criteria broken? They are not broken, if you care about prediction.” At very large N they may favour a more complex model, but its extra parameters will be estimated near zero, so its predictions match the true model's."
  - id: uniformitarian
    prompt: "A height-from-weight model is fit in a poor town and used to predict heights in a wealthy town. WAIC looked good. What went wrong?"
    options:
      - text: "WAIC was computed incorrectly"
      - text: "The priors were too skeptical"
      - text: "The train–test logic assumes future data come from the same process and range as past data; WAIC cannot detect that the new population lies outside the training range"
        correct: true
      - text: "Weight is unrelated to height"
    explanation: "From the box “Diverse prediction frameworks” (p. 224–225): the train–test thought experiment “is a kind of uniformitarian assumption, in which future data are expected to come from the same process as past data ... WAIC will not automatically recognize nor solve this problem. Nor will any other isolated procedure.” This is dataset shift. A latency model trained on normal traffic tells you nothing reliable about a 10× launch-day spike; no cross-validation score will warn you."
  - id: cv-vs-waic-sim
    prompt: "In the simulation comparing CV, PSIS and WAIC (Figure 7.9), how well do they estimate test deviance?"
    options:
      - text: "They are wildly different"
      - text: "Only WAIC works"
      - text: "Only CV works"
      - text: "All three track the average out-of-sample deviance closely, though none is perfect, and they agree closely with each other"
        correct: true
    explanation: "Figure 7.9 shows the CV, PSIS and WAIC estimates following the average test deviance across models with flat and regularizing priors (p. 223–224). Their accuracy improves with sample size. They are estimates with error, not oracles — use their standard errors."
---

We cannot score models on data we do not have. Cross-validation and information criteria estimate it.

## Learning objectives

- Explain k-fold and leave-one-out cross-validation
- Explain what PSIS approximates and how Pareto k flags influential points
- Explain AIC's assumptions and WAIC's computation
- Recognize when out-of-sample estimates mislead (dataset shift)

## In a nutshell

| Method | What it estimates | Cost | Diagnostic |
|---|---|---|---|
| k-fold CV | Out-of-sample lppd | k fits | Fold variability |
| LOOCV | Out-of-sample lppd | N fits | — |
| PSIS | LOOCV | 1 fit | Pareto k per observation |
| AIC | Out-of-sample deviance | 1 fit | Assumes flat priors, Gaussian posterior, N ≫ k |
| WAIC | Out-of-sample KL divergence | 1 fit | Pointwise penalty |

```text
WAIC = −2 ( lppd − Σᵢ var_s[ log p(yᵢ | θₛ) ] )
```

## For engineers

- For time-ordered telemetry, use forward-chaining cross-validation, not random folds.
- High Pareto k values are a free list of observations that dominate your model.
- No criterion protects against forecasting outside the range of your training data.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 7.4, pp. 217–225; Figure 7.9.
- Vehtari A, Gelman A, Gabry J. Practical Bayesian model evaluation using leave-one-out cross-validation and WAIC. *Statistics and Computing* 2017. [doi:10.1007/s11222-016-9696-4](https://doi.org/10.1007/s11222-016-9696-4)
- Watanabe S. Asymptotic equivalence of Bayes cross validation and widely applicable information criterion in singular learning theory. *JMLR* 2010.
- Akaike H. A new look at the statistical model identification. *IEEE Transactions on Automatic Control* 1974.
