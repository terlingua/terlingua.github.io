---
title: "12.3 Ordered Categorical Outcomes"
questions:
  - id: why-not-continuous
    prompt: "Why shouldn't a 1–7 rating be treated as a continuous measurement?"
    options:
      - text: "Because the distances between categories are not necessarily equal; moving from 1 to 2 may be much harder than from 5 to 6"
        correct: true
      - text: "Because ratings are always uniform"
      - text: "Because ratings are unordered"
      - text: "Because ratings cannot be averaged in R"
    explanation: "**p. 380:** “Unlike a count, the differences in value are not necessarily equal. It might be much harder to move someone’s preference for fish from 1 to 2 than it is to move it from 5 to 6. Just treating ordered categories as continuous measures is not a good idea.” NPS scores, CSAT ratings and incident severity levels are ordered categories. Averaging SEV1–SEV4 labels as numbers is the same error."
  - id: cumulative-logit
    prompt: "How does an ordered-logit model describe a distribution over K ordered responses?"
    options:
      - text: "With K independent probabilities"
      - text: "With K − 1 intercepts (cutpoints) on the log-cumulative-odds scale; probabilities of each response are differences between consecutive cumulative probabilities"
        correct: true
      - text: "With a single mean"
      - text: "With a Gaussian and rounding"
    explanation: "Figure 12.4 (p. 383) converts a histogram to cumulative proportions and then to log-cumulative-odds. Each cutpoint κₖ is the log-odds of a response ≤ k. The last cumulative probability is 1, so “for K = 7 possible response values, we only need K − 1 = 6 intercepts” (p. 384). pₖ = qₖ − qₖ₋₁, the likelihood for each observation (Figure 12.5)."
  - id: subtract-phi
    prompt: "Why does the ordered-logit model subtract the linear model φ from each cutpoint, logit(qₖ) = κₖ − φᵢ?"
    options:
      - text: "It is a typo in the book"
      - text: "To make cutpoints negative"
      - text: "So that a positive β shifts probability toward higher responses, increasing the average response"
        correct: true
      - text: "To make the model unidentifiable"
    explanation: "Lowering every cumulative log-odds moves probability mass to higher categories. **p. 387:** “that’s why we subtract ϕ, the linear model βxi, from each intercept, rather than add it. This way, a positive β value indicates that an increase in the predictor variable x results in an increase in the average response.” A sign convention chosen so coefficients read naturally."
  - id: categorical-under-hood
    prompt: "Underneath, what is an ordered-logit distribution?"
    options:
      - text: "A Gaussian"
      - text: "A binomial with n = 7"
      - text: "A Poisson"
      - text: "A categorical distribution whose probability vector is built from cumulative logits so that predictors move predictions through categories in order"
        correct: true
    explanation: "**p. 385:** the expanded form “exposes that an ordered-logit distribution is really just a categorical distribution that takes a vector p ... of probabilities of each response value”. The ordering constraint is what distinguishes it from the unordered softmax model of Chapter 11."
  - id: trolley-principles
    prompt: "In the trolley-problem data, which story features reduced the acceptability rating?"
    type: multiple
    options:
      - text: "Action: the actor does something rather than nothing"
        correct: true
      - text: "Intention: harm is a necessary means to the goal"
        correct: true
      - text: "Contact: the actor physically causes the harm"
        correct: true
      - text: "None of the features affected ratings"
    explanation: "In m12.5 all slopes (bA, bI, bC and interactions of intention with action and contact) are reliably negative: “Each of these story features reduces the rating—the acceptability of the story” (p. 388). The intention × contact interaction is largest in magnitude (about −1.2)."
  - id: plot-ordered
    prompt: "What does the book recommend for interpreting ordered-logit results?"
    options:
      - text: "Plot predicted distributions of responses across predictor combinations — cumulative probabilities or simulated response histograms"
        correct: true
      - text: "Report only the coefficients"
      - text: "Read the cutpoints directly"
      - text: "Convert to a continuous mean and use a t-test"
    explanation: "Figure 12.6 shows how predicted response distributions shift with intention for each action/contact combination, as cumulative probability curves and as simulated histograms. From the box “Staring into the abyss” (p. 391): “It’s better to see the guts of the machine than to live in awe or fear of it.”"
  - id: severity-model
    prompt: "You want to know whether a new runbook changes the severity (SEV1–SEV4) of incidents. Which approach does this section support?"
    options:
      - text: "Average the severity numbers and compare means"
      - text: "An ordered-logit model of severity with runbook as a predictor, checked by plotting predicted severity distributions"
        correct: true
      - text: "Logistic regression on 'SEV1 or not' only"
      - text: "A Poisson model of severity"
    explanation: "Severity is ordered but not interval-scaled. An ordered-logit model uses all categories and respects their order without assuming equal spacing. Collapsing to a binary throws away information; averaging assumes SEV2 − SEV1 = SEV4 − SEV3."
---

Ordered but not equally spaced outcomes — ratings, severities, Likert scales — modelled with cumulative logits.

## Learning objectives

- Explain why ordered categories should not be treated as continuous or unordered
- Build an ordered-logit model from cutpoints on the log-cumulative-odds scale
- Explain the sign convention κₖ − φ
- Interpret ordered models through predicted response distributions

## In a nutshell

```text
Rᵢ ~ OrderedLogit(φᵢ, κ)
logit(Pr(Rᵢ ≤ k)) = κₖ − φᵢ
φᵢ = βA Aᵢ + βC Cᵢ + BIᵢ Iᵢ
BIᵢ = βI + βIA Aᵢ + βIC Cᵢ
κₖ ~ Normal(0, 1.5)
```

```csharp
static double[] OrderedProbs(double[] cutpoints, double phi)
{
    var q = cutpoints.Select(k => Logistic(k - phi)).Append(1.0).ToArray();
    return q.Select((v, i) => i == 0 ? v : v - q[i - 1]).ToArray();
}
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 12.3, pp. 380–391; Figures 12.4–12.6.
- Cushman F, Young L, Hauser M. The role of conscious reasoning and intuition in moral judgment. *Psychological Science* 2006.
- Bürkner P-C, Vuorre M. Ordinal regression models in psychology: a tutorial. *Advances in Methods and Practices in Psychological Science* 2019.
