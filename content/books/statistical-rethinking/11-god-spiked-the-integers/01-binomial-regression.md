---
title: "11.1 Binomial Regression"
questions:
  - id: tide-engine
    prompt: "Why does the chapter compare GLMs to Lord Kelvin's tide-prediction machines?"
    options:
      - text: "Because GLMs were invented for tides"
      - text: "Because, like the machine's gears, GLM parameters interact to produce non-obvious predictions and cannot be read directly; you must look at predictions on the outcome scale"
        correct: true
      - text: "Because GLMs are mechanical"
      - text: "Because GLMs predict periodic phenomena"
    explanation: "**p. 323:** “Generalized linear models (GLMs) are a lot like these early mechanical computers. The moving pieces within them, the parameters, interact to produce non-obvious predictions. But we can’t read the parameters directly to understand the predictions.” And: “When what we wish to predict is a count, the scale of the parameters is never the same as the scale of the outcome.” Like reading a compiled binary: inspect behavior, not the bytes."
  - id: two-flavors
    prompt: "What distinguishes logistic regression from aggregated binomial regression?"
    options:
      - text: "They use different link functions"
      - text: "Only aggregated binomial can include predictors"
      - text: "Logistic regression has single-trial 0/1 outcomes; aggregated binomial has counts of successes out of n trials with the same covariates — the same model, organized differently"
        correct: true
      - text: "Logistic regression is not a GLM"
    explanation: "**p. 325:** “(1) Logistic regression is the common name when the data are organized into single-trial cases ... (2) When individual trials with the same covariate values are instead aggregated together, it is common to speak of an aggregated binomial regression.” “Either form of binomial regression can be converted into the other by aggregating ... or exploding”. Per-request success flags versus per-minute success counts out of requests: same information if requests are exchangeable within a minute."
  - id: flat-logit-prior
    prompt: "What does a 'flat' Normal(0, 10) prior on a logistic intercept imply on the probability scale?"
    options:
      - text: "A flat prior on probability"
      - text: "Nothing; logit priors don't matter"
      - text: "A prior centered on 0.5 with small spread"
      - text: "That most prior probability piles up near 0 and 1 — the model believes beforehand that the event almost never or almost always happens"
        correct: true
    explanation: "Figure 11.3 (p. 328): pushing Normal(0, 10) through the logistic function makes nearly all values extreme. “The model thinks, before it sees the data, that chimpanzees either never or always pull the left lever.” Normal(0, 1.5) gives a roughly flat prior on probability. A prior that looks flat on one scale can be very informative on another. Always check priors on the outcome scale."
  - id: treatment-prior
    prompt: "Why does the book use Normal(0, 0.5) for treatment effects on the logit scale?"
    options:
      - text: "Because flatter priors imply that treatments are either identical or completely different; typical behavioral treatments have modest effects, which Normal(0, 0.5) expresses"
        correct: true
      - text: "To make the effects significant"
      - text: "Because it is conjugate"
      - text: "Because there are four treatments"
    explanation: "**p. 329:** with a flat prior, “the model believes, before it sees that data, that the treatments are either completely alike or completely different ... Typical behavioral treatments have modest effects on chimpanzees and humans alike.” Product experiments too: most UI changes shift conversion modestly. A prior that expects huge effects encourages overfitting and inflated winners."
  - id: actor-intercepts
    prompt: "In the chimpanzee model, why include a separate intercept for each actor?"
    options:
      - text: "To identify the chimpanzees by name"
      - text: "Because individuals have strong handedness preferences; modelling them subtracts that variation and estimates treatment effects more precisely"
        correct: true
      - text: "Because the actors are randomized"
      - text: "To increase overfitting"
    explanation: "The posterior shows big differences among actors — actor 2 always pulled left (p. 330–331). Repeat measures on each individual let the model separate individual preference from treatment effects. In telemetry, per-host or per-customer intercepts do the same: remove baseline differences so the effect of a change is measured within units. Chapter 13 makes these intercepts multilevel."
  - id: prosocial-result
    prompt: "What did the posterior predictions show about the prosocial hypothesis?"
    options:
      - text: "Chimpanzees strongly preferred the prosocial option when a partner was present"
      - text: "Chimpanzees avoided the prosocial option"
      - text: "Adding a partner produced almost no change; most variation in predictions came from actors' handedness"
        correct: true
      - text: "The model failed to converge"
    explanation: "**p. 333:** “The model expects almost no change when adding a partner. Most of the variation in predictions comes from the actor intercepts. Handedness seems to be the big story of this experiment.” The posterior prediction plot (Figure 11.4) compares predictions to observed proportions by actor and treatment. Inspect results on the outcome scale and by unit, not just as coefficients."
  - id: absolute-relative
    prompt: "Exponentiating a logistic coefficient gives proportional odds. A change multiplies odds by 5. Why might this still be unimportant?"
    options:
      - text: "Because proportional odds are always small"
      - text: "Because exp(β) is meaningless"
      - text: "Because odds cannot exceed 1"
      - text: "Because if the baseline probability is tiny, multiplying the odds by 5 barely changes the absolute probability — relative effects alone cannot say whether a variable matters"
        correct: true
    explanation: "**p. 336:** “If the other parameters in the model make the outcome very unlikely, then even a large proportional odds like 5.0 would not make the outcome frequent.” A disease at 1 in 10 million with 5× odds is about 5 in 10 million. A config change that makes a 0.001% failure 5× more likely may still be negligible; one that adds 2% absolute failure is not."
  - id: shark-deer
    prompt: "What is the point of the parable of relative shark and absolute deer?"
    options:
      - text: "Absolute risks matter for general advice, relative risks for decisions conditional on circumstances; neither is sufficient for all purposes"
        correct: true
      - text: "Sharks are more dangerous in every context"
      - text: "Deer are more dangerous than sharks in every context"
      - text: "Risk cannot be measured"
    explanation: "**p. 337:** deer kill far more people than sharks, but “Conditional on being in the ocean, sharks are much more dangerous than deer ... Neither absolute nor relative risk is sufficient for all purposes ... Sharks are absolutely safe, while deer are relatively safe. Both are important truths.” Report both: 'this flag raises error odds 3×' and 'that is +0.02 percentage points at our traffic mix'."
  - id: psis-aggregated
    prompt: "Why do PSIS/WAIC values differ hugely between the logistic and aggregated versions of the same chimpanzee model?"
    options:
      - text: "Because the posteriors differ"
      - text: "Because the aggregated likelihood includes a multiplicity (combinatorial) term, and leaving out one aggregated row means leaving out many trials at once — so compare only models fit to the same observation format"
        correct: true
      - text: "Because PSIS has a bug"
      - text: "Because aggregated models have more parameters"
    explanation: "**p. 339:** the aggregated model “contains an extra factor in its log-probabilities” — the count of orderings. And aggregation makes PSIS imagine “leave-eighteen-out” cross-validation (p. 340). “If you want to calculate WAIC or PSIS, you should use a logistic regression data format, not an aggregated format.” The posteriors are the same; only the scoring differs."
  - id: ucb-admissions
    prompt: "In the UC Berkeley admissions data, a model with only gender shows women admitted less often. Adding department changes the picture. Why?"
    options:
      - text: "Departments discriminate in favor of women"
      - text: "The first model had too few iterations"
      - text: "Women applied disproportionately to departments with low admission rates for everyone; within departments, the gender difference is small"
        correct: true
      - text: "Department is a random variable"
    explanation: "**p. 344:** “The departments with a larger proportion of women applicants are also those with lower overall admissions rates.” Within departments, “If male applicants have it worse, it is only by a very small amount, about 2% on average.” The posterior check of the gender-only model was terrible (Figure 11.5). The DAG is G → D → A with G → A: department mediates part of the effect, and conditioning on D estimates the direct effect."
  - id: collider-ucb
    prompt: "What caution does the book add about conditioning on department to estimate the direct effect of gender?"
    options:
      - text: "None; it is always correct"
      - text: "That department has too many categories"
      - text: "That gender must be standardized"
      - text: "If an unobserved variable like academic ability influences both department choice and admission, department is a collider, and conditioning on it opens a non-causal path"
        correct: true
    explanation: "**p. 345:** “Ability could influence choice of department and probability of admission. In that case, conditioning on department is conditioning on a collider, and it opens a non-causal path between gender and admissions, G → D ← U → A.” The same structure as the haunted DAG in Chapter 6."
  - id: simpsons-not-paradox
    prompt: "Why does the book say Simpson's paradox is not a paradox?"
    options:
      - text: "Because there is no violation of logic, only of the intuition that a population-level association must hold within subgroups; reversals can reveal causes or create confounds"
        correct: true
      - text: "Because it never happens in real data"
      - text: "Because aggregated data are always correct"
      - text: "Because stratified data are always correct"
    explanation: "From the box on p. 345: “Like most paradoxes, there is no violation of logic, just of intuition ... Any association between an outcome and a predictor can be nullified or reversed when another predictor is added to the model. And the reversal can reveal a true causal influence or rather just be a confound.” Classic ops example: a new region has lower overall error rate but higher error rate per endpoint, because it serves mostly easy endpoints. Which number matters depends on the causal question."
---

Binary and count-of-successes outcomes: the logit link, priors that make sense on the probability scale,
and relative versus absolute effects.

## Learning objectives

- Build logistic and aggregated binomial models and convert between them
- Set logit-scale priors by checking the probability scale
- Interpret proportional odds versus absolute changes in probability
- Work through the UC Berkeley example, mediation and Simpson's paradox

## In a nutshell

```text
Lᵢ ~ Binomial(1, pᵢ)                       logistic
logit(pᵢ) = α[actor] + β[treatment]
α ~ Normal(0, 1.5);  β ~ Normal(0, 0.5)

Aᵢ ~ Binomial(Nᵢ, pᵢ)                      aggregated
logit(pᵢ) = α[gender] + δ[dept]
```

| Effect type | Formula | Answers |
|---|---|---|
| Relative (proportional odds) | exp(β) | 'How much more likely, relatively?' |
| Absolute | p₁ − p₀ on probability scale | 'How many more events?' |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 11 opening and Section 11.1, pp. 323–345; Figures 11.1–11.5.
- Silk JB et al. Chimpanzees are indifferent to the welfare of unrelated group members. *Nature* 2005.
- Bickel PJ, Hammel EA, O'Connell JW. Sex bias in graduate admissions: data from Berkeley. *Science* 1975.
