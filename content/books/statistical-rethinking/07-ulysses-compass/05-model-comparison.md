---
title: "7.5 Model Comparison"
questions:
  - id: no-selection
    prompt: "Why does the book say you should *never* do model selection — picking the model with the lowest WAIC and discarding the rest?"
    options:
      - text: "Because selection discards the information in the differences among criterion values, which tell you how confident you can be about the models"
        correct: true
      - text: "Because WAIC is always wrong"
      - text: "Because the lowest WAIC is always the most complex model"
      - text: "Because discarded models might be needed for MCMC"
    explanation: "**p. 225:** “A very common use of cross-validation and information criteria is to perform model selection, which means choosing the model with the lowest criterion value and then discarding the others. But you should never do this. This kind of selection procedure discards the information about relative model accuracy contained in the differences among the CV/PSIS/WAIC values.” Like picking the fastest of three benchmark runs without looking at variance: a 1% gap within noise is no basis for a decision."
  - id: not-causal
    prompt: "Another reason not to select models by WAIC/CV/PSIS alone is causal inference. What is it?"
    options:
      - text: "WAIC requires a DAG"
      - text: "Maximizing expected predictive accuracy is not the same as inferring causation; confounded models can predict well"
        correct: true
      - text: "Causal models always have higher WAIC"
      - text: "WAIC cannot be computed for causal models"
    explanation: "**p. 226:** “Maximizing expected predictive accuracy is not the same as inferring causation. Highly confounded models can still make good predictions, at least in the short term. They won’t tell us the consequences of an intervention, but they might help us forecast. So we need to be clear about our goals and not just toss variables into the causal salad and let WAIC select our meal.”"
  - id: fungus-waic
    prompt: "Comparing the plant models, the model with treatment *and fungus* (m6.7) has by far the best WAIC, though it gives a biased treatment effect. What does this teach?"
    options:
      - text: "WAIC detected the post-treatment bias"
      - text: "The treatment does not work"
      - text: "WAIC did its job — guessing predictive accuracy. Fungus predicts height better than treatment, but conditioning on it ruins causal inference about the treatment"
        correct: true
      - text: "The model with fungus is causally correct"
    explanation: "**p. 228:** “It means that WAIC cannot be used to infer causation ... WAIC did its job. Its job is not to infer causation. Its job is to guess predictive accuracy.” A model predicting user retention from 'number of support tickets resolved' may beat one using 'new onboarding flow' on WAIC; that tells you nothing about whether the onboarding flow works."
  - id: compare-columns
    prompt: "In a `compare` table, what do dWAIC and dSE mean?"
    options:
      - text: "Both are Akaike weights"
      - text: "dWAIC: change in WAIC per parameter; dSE: standard error of WAIC"
      - text: "dWAIC: posterior mean of WAIC; dSE: prior standard error"
      - text: "dWAIC: difference from the best model's WAIC; dSE: standard error of that difference"
        correct: true
    explanation: "From p. 227: the columns are WAIC, its standard error (SE), the difference from the best model (dWAIC), “standard error (dSE) of this difference, prediction penalty (pWAIC), and finally the Akaike weight.” To judge whether two models differ, compare dWAIC to dSE — not to each model's SE. The dSE is computed from the pointwise differences: √(N × var(waicᵢ(A) − waicᵢ(B)))."
  - id: treatment-small
    prompt: "The treatment-only model (m6.8) and the intercept-only model (m6.6) have similar WAIC, with dSE larger than the difference, even though the treatment truly works. Why?"
    options:
      - text: "The treatment effect is real but small relative to other sources of variation, so it does little to improve predictions"
        correct: true
      - text: "WAIC is broken"
      - text: "The treatment really does nothing"
      - text: "The sample was too large"
    explanation: "**p. 229:** “the posterior distribution of the treatment effect, bt in m6.8, is reliably positive. But it isn’t especially large. So it doesn’t do much alone to improve prediction of plant height. There are just too many other sources of variation.” And: “A variable can be causally related to an outcome, but have little relative impact on it.” A config change can reliably cut p50 by 2 ms while barely changing your ability to predict any single request's latency."
  - id: akaike-weights
    prompt: "What are Akaike weights, and what is a caution about them?"
    options:
      - text: "They are prior probabilities of each model being true"
      - text: "They rescale WAIC differences into relative support for each model as the best predictor on future data — but they are conditional on the set of models compared and on future data resembling the past"
        correct: true
      - text: "They are always equal"
      - text: "They are the number of parameters in each model"
    explanation: "The weights transform dWAIC into values that sum to 1 across models. The box “WAIC metaphors” (p. 230) compares models to race horses: “Akaike weights transform differences in finishing time into probabilities of being the best model/horse on future data/races. But if the track conditions or jockey changes, these probabilities may mislead.” They are not probabilities that a model is true."
  - id: race-horses
    prompt: "In the race-horse metaphor, what corresponds to a photo-finish?"
    options:
      - text: "A model with high Pareto k"
      - text: "A model with zero penalty"
      - text: "Two models with very similar WAIC, so it is hard to be confident which predicts better"
        correct: true
      - text: "Two models with identical priors"
    explanation: "**p. 230:** “if instead it’s a photo-finish, with a near tie between first and second place, then it is much harder to be confident about which is the best horse.” The divorce models m5.1 and m5.3 are such a case: dPSIS = 1.8 with dSE = 0.9 (p. 231), so they make very similar predictions."
  - id: idaho
    prompt: "PSIS warns that some Pareto k values are very high for the divorce model, and Idaho has k > 1 and a WAIC penalty over 2. What does this mean?"
    options:
      - text: "Idaho is a data entry error that must be deleted"
      - text: "The model has too few parameters"
      - text: "Idaho has missing data"
      - text: "Idaho is highly unexpected under the model and highly influential, adding overfitting risk"
        correct: true
    explanation: "**p. 232:** Idaho “has a very low divorce rate for its age at marriage. As a result, it is highly influential—it exerts more influence on the posterior distribution than other States do.” The model has 4 parameters but a total penalty near 6: “The outlier Idaho is causing this additional overfitting risk.”"
  - id: dropping-outliers
    prompt: "What does the book say about dropping outliers?"
    options:
      - text: "Never drop outliers based only on standard deviations from the mean before fitting; a point is unexpected only in light of a model. Prefer a model that expects extreme values"
        correct: true
      - text: "Always drop points more than 3 SD from the mean before fitting"
      - text: "Drop any point with Pareto k above 0.5"
      - text: "Outliers never affect Bayesian models"
    explanation: "**p. 232:** “People sometimes drop outliers even before a model is fit, based only on standard deviations from the mean outcome value. You should never do that—a point can only be unexpected and highly influential in light of a model.” Dropping a few with results reported both ways might be acceptable. But extreme observations often contain real information. In incident analysis, the outlier day *is* the incident; deleting it to make the trend line pretty is how you miss the next one."
  - id: student-t
    prompt: "How does robust regression with a Student-t likelihood (ν = 2) handle Idaho?"
    options:
      - text: "It removes Idaho"
      - text: "Its thick tails make extreme observations less surprising, reducing Idaho's influence; PSIS warnings disappear and the age-at-marriage coefficient moves farther from zero"
        correct: true
      - text: "It makes Idaho more influential"
      - text: "It changes the outcome variable"
    explanation: "**p. 233:** “The Student-t distribution arises from a mixture of Gaussian distributions with different variances. If the variances are diverse, then the tails can be quite thick.” With `dstudent(2, mu, sigma)` (p. 234), “you won’t get any warnings about Pareto k values. The relative influence of Idaho has been much reduced.” For latency or error data with occasional spikes, a Student-t likelihood is a principled alternative to deleting spikes."
  - id: what-good
    prompt: "So what good are WAIC and related criteria, according to the book?"
    type: multiple
    options:
      - text: "They measure expected predictive value of a variable, accounting for overfitting"
        correct: true
      - text: "They help test model implications, given a set of causal models"
        correct: true
      - text: "They measure a model's overfitting tendency and help tune multilevel models"
        correct: true
      - text: "They identify the true causal model automatically"
    explanation: "**p. 226:** “They measure expected predictive value of a variable on the right scale, accounting for overfitting. This helps in testing model implications, given a set of causal models. They also provide a way to measure the overfitting tendency of a model ... Finally, minimizing a criterion like WAIC can help in designing models, especially in tuning parameters in multilevel models.” Hence the shift from model *selection* to model *comparison*."
---

Use criteria to compare, not to select — and remember prediction is not causation.

## Learning objectives

- Explain why model selection discards information
- Read a `compare` table: WAIC, SE, dWAIC, dSE, pWAIC, weight
- Explain why WAIC favours the confounded fungus model
- Handle influential observations with PSIS diagnostics and robust likelihoods

## In a nutshell

| Column | Meaning |
|---|---|
| WAIC / PSIS | Estimated out-of-sample deviance (smaller is better) |
| SE | Standard error of that estimate |
| dWAIC | Difference from the best model |
| dSE | Standard error of the difference — compare dWAIC to this |
| pWAIC | Penalty (effective number of parameters) |
| weight | Akaike weight: relative predictive support within this set |

```text
yᵢ ~ Student-t(ν = 2, μᵢ, σ)     robust alternative to dropping outliers
```

## For engineers

Compare models like benchmark results: look at the difference and its noise, not the leaderboard order.
And remember the best-predicting model of an incident metric may be the worst guide for what to change.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 7.5, pp. 225–235; Figures 7.10–7.11.
- Burnham KP, Anderson DR. *Model Selection and Multimodel Inference*, 2nd ed. Springer, 2002.
- Vehtari A et al. Pareto smoothed importance sampling. arXiv:1507.02646 (revised 2024).
