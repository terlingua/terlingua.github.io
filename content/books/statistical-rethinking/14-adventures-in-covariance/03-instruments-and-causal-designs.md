---
title: "14.3 Instruments and Causal Designs"
questions:
  - id: iv-criteria
    prompt: "Which criteria must an instrumental variable Q satisfy for estimating E → W with an unobserved confound U?"
    type: multiple
    options:
      - text: "Q is independent of U"
        correct: true
      - text: "Q is not independent of E"
        correct: true
      - text: "Q cannot influence W except through E (the exclusion restriction)"
        correct: true
      - text: "Q must be randomized by the experimenter"
    explanation: "**p. 455:** “(1) Independent of U (Q ⊥⊥ U) (2) Not independent of E (Q ̸⊥⊥ E) (3) Q cannot influence W except through E. This last line is sometimes called the exclusion restriction. It cannot be strictly tested, and it is often implausible. Similarly, the first line above cannot be tested.” An instrument “acts like a natural experiment on the exposure.” It does not need to be randomized by you, only as-if random with respect to U."
  - id: quarter-birth
    prompt: "Why is quarter of birth proposed as an instrument for education?"
    options:
      - text: "Because birth quarter determines wages directly"
      - text: "Because it is a collider"
      - text: "Because it is easy to measure"
      - text: "Because people born earlier in the year tend to get less schooling (they start older and can drop out earlier), while birth quarter is plausibly unrelated to confounds like industriousness"
        correct: true
    explanation: "**p. 456:** “people born earlier in the year tend to get less schooling. This is both because they are biologically older when they start school and because they become eligible to drop out of school earlier.” If Q affects W only through E, and is independent of U, it is valid. The book simulates data with a true education effect of zero."
  - id: bias-amplifier
    prompt: "What happens if you just add the instrument Q as another predictor in a regression of W on E?"
    options:
      - text: "E is a collider of Q and U, so conditioning on E opens Q → E ← U → W; the estimate of E's effect gets *worse* — Q acts as a bias amplifier"
        correct: true
      - text: "The confound is removed"
      - text: "Nothing changes"
      - text: "The model fails to fit"
    explanation: "**p. 456:** “since we are conditioning on E in the same model, and E is a collider of Q and U, the non-causal path is open ... Used this way, an instrument like Q might be called a bias amplifier.” In the simulation the naive estimate is 0.40 and adding Q pushes it to 0.64, though the truth is zero (p. 457): “That is bias amplification in action.”"
  - id: iv-generative
    prompt: "How does the book use the instrument correctly?"
    options:
      - text: "By dropping E"
      - text: "By writing the generative model: W and E get their own linear models (E depends on Q), with a joint multivariate normal error whose correlation absorbs the shared confound U"
        correct: true
      - text: "By using a p-value"
      - text: "By conditioning on U"
    explanation: "**p. 458:** “Define W and E as coming from a common multivariate normal distribution.” μ_W = αW + βEW E and μ_E = αE + βQE Q, with error covariance S. The fitted model recovers βEW ≈ 0 and a positive residual correlation “that reflects the common influence of U” (p. 459). Treat the unobserved confound as correlated noise between the two equations."
  - id: tsls
    prompt: "What does the book say about two-stage least squares (2SLS)?"
    options:
      - text: "It is the definition of instrumental variables"
      - text: "It is always better than Bayesian estimation"
      - text: "It is one estimation procedure for IV models that relies on large-sample approximations and has known problems; it should not be mistaken for the model itself"
        correct: true
      - text: "It does not exist"
    explanation: "From the box “Two-stage worst squares” (p. 460): “Sometimes people mistake 2SLS for the model of instrumental variables. They are not the same thing. Any model can be estimated through a number of different procedures.” Separate the model from the estimator, as in Chapter 2's separation of posterior from engine."
  - id: believe-instrument
    prompt: "What is the hardest thing about instrumental variables, according to the book?"
    options:
      - text: "Computing them"
      - text: "Having enough data"
      - text: "Finding software"
      - text: "Believing in any particular instrument: whether the exclusion restriction and independence hold depends on your DAG and cannot be proven from data"
        correct: true
    explanation: "**p. 460:** “The hardest thing about instrumental variables is believing in any particular instrument. If you believe in your DAG, they are easy to believe. But should you believe in your DAG?” Wind direction for colonization, or distance to a hospital, can plausibly affect outcomes through other routes. “In general, it is not possible to statistically prove whether a variable is a good instrument.”"
  - id: devops-instrument
    prompt: "You want the causal effect of cache hit rate (E) on page latency (W), but traffic mix (U, unobserved) affects both. A random hashing bug sent some users to a cold cache shard for a week. How might this serve as an instrument?"
    options:
      - text: "Shard assignment (Q) changed hit rate, was effectively random with respect to traffic mix, and plausibly affected latency only through hit rate — so it satisfies the IV criteria if you believe the exclusion restriction"
        correct: true
      - text: "It cannot; bugs are not data"
      - text: "Only if you also condition on traffic mix"
      - text: "Only if the bug affected all users"
    explanation: "Accidental randomization is a classic source of instruments. Check each criterion: independence from U (assignment by hash — plausible), relevance (cold shard lowers hit rate — testable), exclusion (did the cold shard also differ in hardware or location? If so, it affects latency another way). Then fit the joint model of hit rate and latency."
  - id: front-door
    prompt: "What does the front-door criterion require?"
    options:
      - text: "An instrument"
      - text: "A mediator Z that fully transmits X's effect on Y and is not affected by the confound U, so X → Y can be estimated through X → Z → Y"
        correct: true
      - text: "A randomized experiment"
      - text: "Conditioning on U"
    explanation: "**p. 461:** “if we can find a perfect mediator Z, then we can possibly estimate the causal effect of X on Y. It isn’t crazy to think that causes are mediated by other causes. Everything has a mechanism.” Example: senators' college ties affecting votes through seating proximity, since junior seating is effectively random. “The front-door criterion isn’t used much. This may be because ... believable Z variables are rare.”"
  - id: rdd
    prompt: "What is the idea behind regression discontinuity design (RDD)?"
    options:
      - text: "Compare everyone above and below any threshold"
      - text: "Fit a regression with a discontinuous prior"
      - text: "Compare units just above and just below a sharp cutoff for a treatment; near the cutoff they should be similar in unobserved factors, as if treatment were randomized"
        correct: true
      - text: "Remove all data near the cutoff"
    explanation: "**p. 461–462:** for an award with a score cutoff, “if we compare individuals who were just below the cutoff for the award to those who were just above the cutoff, these individuals should be similar in the unobserved factors. It’s as if the award were applied at random.” Engineering cutoffs abound: customers just above a usage tier boundary, requests just over a timeout threshold, accounts created just before a feature launch."
---

When backdoors cannot be closed, natural experiments and clever designs can still identify causes.

## Learning objectives

- State the three criteria for an instrumental variable
- Explain bias amplification when an instrument is used as an ordinary predictor
- Build the generative (multivariate) IV model
- Recognize front-door and regression discontinuity designs

## In a nutshell

```text
DAG:  Q → E → W,  E ← U → W   (U unobserved)

[Wᵢ, Eᵢ] ~ MVNormal([μW,ᵢ, μE,ᵢ], S)
μW,ᵢ = αW + βEW Eᵢ
μE,ᵢ = αE + βQE Qᵢ
S = diag(σ) · R · diag(σ);   R ~ LKJcorr(2)
```

| Design | Key assumption |
|---|---|
| Instrumental variable | Q affects W only through E; Q ⊥ U |
| Front door | Mediator Z captures all of X → Y and is unconfounded |
| Regression discontinuity | Units near the cutoff are comparable |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 14.3, pp. 455–462; Figure 14.7.
- Angrist JD, Krueger AB. Does compulsory school attendance affect schooling and earnings? *Quarterly Journal of Economics* 1991.
- Angrist JD, Pischke J-S. *Mostly Harmless Econometrics*. Princeton University Press, 2009.
- Pearl J. Causal diagrams for empirical research. *Biometrika* 1995 (front-door criterion).
