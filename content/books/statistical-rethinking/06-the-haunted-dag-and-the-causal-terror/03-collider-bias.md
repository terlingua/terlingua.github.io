---
title: "6.3 Collider Bias"
questions:
  - id: collider-definition
    prompt: "In the DAG T → S ← N, what makes S a collider, and what happens when you condition on it?"
    options:
      - text: "S is unobserved; conditioning on it is impossible"
      - text: "S has two arrows leaving it; conditioning on it removes association"
      - text: "S has two arrows entering it; conditioning on it creates a statistical (not causal) association between T and N"
        correct: true
      - text: "S is the outcome; conditioning is required"
    explanation: "**p. 176:** “The fact that two arrows enter S means it is a collider. The core concept is easy to understand: When you condition on a collider, it creates statistical—but not necessarily causal—associations among its causes.” If a funded proposal has low trustworthiness, it must have high newsworthiness, “Otherwise it wouldn’t have been funded.” Bug-report triage behaves this way: among tickets that reached the on-call queue (high severity OR high visibility), visibility and severity look negatively related."
  - id: selection-as-collider
    prompt: "How is analyzing only a selected sample related to collider bias?"
    options:
      - text: "They are unrelated"
      - text: "Selection is only a problem for small samples"
      - text: "Selection reduces variance but never biases associations"
      - text: "Selecting cases by a common consequence of two variables is conditioning on a collider, so it can create associations that do not exist in the full population"
        correct: true
    explanation: "Practice problem 6E4 asks exactly this, and the chapter's opening proposals example is the answer: the funded set is the sample conditioned on S = 1. **p. 176:** “it means we have to pay attention to processes that select our sample of observations and may distort associations among variables.” Survivorship bias in engineering is the same phenomenon: analysing only services that still exist, only crash reports that were successfully uploaded, only users who did not churn."
  - id: happiness-sim
    prompt: "In the happiness simulation, happiness never changes with age, and happier people are more likely to marry. A regression of happiness on age *and* marriage status finds happiness declines with age. Why?"
    options:
      - text: "Marriage status is a collider of age and happiness (A → M ← H); conditioning on it induces a spurious negative association between age and happiness"
        correct: true
      - text: "Aging reduces happiness in the simulation"
      - text: "Marriage makes people unhappy"
      - text: "The priors favored a negative slope"
    explanation: "**p. 179:** “The collider is marriage status. It is a common consequence of age and happiness. As a result, when we condition on it, we induce a spurious association between the two causes.” Among married people, older ones include more of the less happy who married later; among unmarried people, the happy ones have left over time. Both groups show declining happiness with age, though no individual's happiness changes. A model without marriage status correctly finds no association."
  - id: simulation-trust
    prompt: "Why does the book rely on simulations for these examples?"
    options:
      - text: "Because real data are unavailable"
      - text: "Because simulations are the only times we know the true causal model; if a procedure cannot find the truth in a simulation, we should not trust it on real data"
        correct: true
      - text: "Because simulations are faster than fitting"
      - text: "Because journals require them"
    explanation: "**p. 177:** “Simulations are useful in these examples, because these are the only times when we know the true causal model. If a procedure cannot figure out the truth in a simulated example, we shouldn’t trust it in a real one.” This is the testing mindset: before using an analysis on production telemetry, run it on synthetic data where you planted the effect, including adversarial structures like colliders."
  - id: haunted-dag
    prompt: "In the grandparents (G), parents (P), children (C) example, an unobserved neighborhood U affects P and C. Why does conditioning on P make grandparents appear to *harm* grandchildren's education?"
    options:
      - text: "Grandparents really do harm education in the simulation"
      - text: "P is a confounder that should not be omitted"
      - text: "P is a collider of G and U; conditioning on P makes G informative about U, which affects C, producing a spurious negative association"
        correct: true
      - text: "The sample was too small"
    explanation: "**p. 180:** “Now P is a common consequence of G and U, so if we condition on P, it will bias inference about G → C, even if we never get to measure U.” **p. 182:** “once we know P, learning G invisibly tells us about the neighborhood U, and U is associated with the outcome C.” Among parents with the same education, those with highly educated grandparents must have lived in worse neighborhoods to end up at that level, and their children inherit the neighborhood. The DAG is *haunted* by an unobserved variable."
  - id: haunted-devops
    prompt: "You want the direct effect of a library version (G) on request latency (C), controlling for CPU usage (P). G affects P, and an unmeasured noisy-neighbor effect (U) affects both P and C. What does this section predict?"
    options:
      - text: "Conditioning on CPU gives the clean direct effect"
      - text: "Bias only occurs if U is binary"
      - text: "U cancels out because it is unmeasured"
      - text: "Conditioning on CPU opens G → P ← U → C, so the estimated direct effect of G on latency can be badly biased, even reversed"
        correct: true
    explanation: "Same structure as the haunted grandparents DAG. CPU usage is a collider of the library version and the noisy-neighbor load. Among hosts with the same CPU usage, those running the efficient library must be suffering more noisy-neighbor load to reach that CPU level, and that load also slows requests. The library looks harmful when it may help. Measuring U (or a good proxy) is the fix, as model m6.12 shows by recovering the true slopes when U is included (p. 183)."
  - id: simpsons
    prompt: "How does the book relate the grandparents example to Simpson's paradox?"
    options:
      - text: "Including another predictor reversed the direction of an association; whether the reversal reflects causation requires something beyond the statistical model"
        correct: true
      - text: "Simpson's paradox proves causation"
      - text: "It is not related"
      - text: "Simpson's paradox only occurs with categorical data"
    explanation: "**p. 183:** “Simpson’s paradox: Including another predictor (P in this case) can reverse the direction of association between some other predictor (G) and the outcome (C). Usually, Simpson’s paradox is presented in cases where adding the new predictor helps us. But in this case, it misleads us. Simpson’s paradox is a statistical phenomenon.” Whether the aggregated or the stratified answer is correct depends on the causal structure — a DAG question, not a data question."
  - id: need-science
    prompt: "What does the book conclude about using multiple regression to discover which of several causal models is right?"
    options:
      - text: "Regression alone can identify the right causal model"
      - text: "Without a causal model you cannot make inferences from a multiple regression, and the regression itself does not justify a causal model — you need some science"
        correct: true
      - text: "Only experiments can identify causes"
      - text: "Adding all variables is safest"
    explanation: "**p. 179–180:** “If you don’t have a causal model, you can’t make inferences from a multiple regression. And the regression itself does not provide the evidence you need to justify a causal model. Instead, you need some science.” In SRE terms: correlations in your observability stack become root causes only after you combine them with knowledge of how the system is wired."
---

Conditioning on a common consequence creates associations among its causes — even when one cause is
unobserved.

## Learning objectives

- Define a collider and explain why conditioning on it creates association
- Recognize selection effects as collider bias
- Work through the happiness and haunted-grandparents examples
- Relate collider bias to Simpson's paradox

## In a nutshell

```text
T → S ← N               condition on S → T and N become associated
A → M ← H               condition on marriage → age and happiness look related
G → P ← U → C, G → P → C   condition on P (U unobserved) → G appears to harm C
```

| Variable type | Default action for estimating X → Y |
|---|---|
| Collider on a path between X and Y | Do not condition on it |
| Descendant of a collider | Do not condition on it (partly opens the path) |
| Unobserved common cause of a collider's parent and Y | Makes conditioning on the collider dangerous |

## For engineers

Survivorship bias, 'only incidents that paged someone', 'only users who completed onboarding', 'only
requests that returned a response' — every filter on an outcome-related variable is a candidate collider.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 6.3, pp. 176–183; Figures 6.4–6.5.
- Elwert F, Winship C. Endogenous selection bias: the problem of conditioning on a collider variable. *Annual Review of Sociology* 2014. [doi:10.1146/annurev-soc-071913-043455](https://doi.org/10.1146/annurev-soc-071913-043455)
