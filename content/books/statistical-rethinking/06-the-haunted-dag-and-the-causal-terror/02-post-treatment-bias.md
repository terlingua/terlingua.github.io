---
title: "6.2 Post-Treatment Bias"
questions:
  - id: included-variable-bias
    prompt: "What is the chapter's point about *included* variable bias?"
    options:
      - text: "Including variables is always safe; only omitting them causes bias"
      - text: "Included variables only matter in observational studies"
      - text: "Mistaken inferences can come from including variables, and carefully randomized experiments can be ruined by it just as easily as observational studies"
        correct: true
      - text: "Included variable bias only affects standard errors"
    explanation: "**p. 170:** “It is much less routine to worry about mistaken inferences arising from including variables. But included variable bias is real. Carefully randomized experiments can be ruined just as easily as uncontrolled observational studies. Blindly tossing variables into the causal salad is never a good idea.” An A/B test analysis that 'controls for' metrics measured after assignment can destroy the clean causal estimate the randomization bought you."
  - id: plant-setup
    prompt: "In the greenhouse experiment, plants get an anti-fungal soil treatment (T); fungus presence (F) is recorded later; final height H1 is the outcome. Why is fungus a dangerous variable to include when estimating the treatment effect?"
    options:
      - text: "It is measured imprecisely"
      - text: "It has too many categories"
      - text: "It is a pre-treatment confounder"
      - text: "It is a post-treatment variable: mostly a consequence of the treatment, so conditioning on it blocks the path through which the treatment works"
        correct: true
    explanation: "**p. 173:** “The problem is that fungus is mostly a consequence of treatment. This is to say that fungus is a post-treatment variable. So when we control for fungus, the model is implicitly answering the question: Once we already know whether or not a plant developed fungus, does soil treatment matter? The answer is “no,” because soil treatment has its effects on growth through reducing fungus.” With F included, βT ≈ 0; without it, βT is clearly positive."
  - id: devops-post-treatment
    prompt: "You test whether a new retry policy (T) reduces user-facing errors (Y). The policy works by reducing the number of upstream timeouts (Z). An analyst regresses Y on T and Z and finds no effect of T. What happened?"
    options:
      - text: "Conditioning on Z, a post-treatment mediator, blocked the pipe T → Z → Y, hiding the treatment's effect"
        correct: true
      - text: "Z is a confounder that should be included"
      - text: "The retry policy truly has no effect"
      - text: "The sample size was too small"
    explanation: "Same structure as the fungus example: a pipe T → Z → Y. Conditioning on the middle of a pipe blocks the path. To estimate the total causal effect of the policy, leave Z out. Including Z is still informative — the book notes that if conditioning on the mediator zeros the treatment coefficient, “the treatment works for exactly the anticipated reasons. It tells us about mechanism” (p. 174). It just answers a different question."
  - id: pretreatment-ok
    prompt: "Which statement about pre-treatment variables in the plant model is consistent with the book?"
    options:
      - text: "Pre-treatment variables should never be included"
      - text: "It makes sense to account for pre-treatment differences such as initial height h0, which might otherwise mask the treatment's effect"
        correct: true
      - text: "Pre-treatment variables are always safe in every DAG"
      - text: "Initial height is a collider"
    explanation: "**p. 174:** “It makes sense to control for pre-treatment differences, like the initial height h0, that might mask the causal influence of treatment. But including post-treatment variables can actually mask the treatment itself.” Note the caveat in the box on p. 171: the heuristic 'pre-treatment is safe' “is not principled. Pre-treatment variables can also create bias, as you’ll see later in this chapter.” The backdoor criterion (6.4) replaces heuristics with a rule."
  - id: proportional-growth
    prompt: "Why does the book model h1 = h0 × p with p ~ Log-Normal(0, 0.25) rather than an additive model?"
    options:
      - text: "To make the model run faster"
      - text: "Because heights are categorical"
      - text: "Because growth is naturally a proportion of starting height, p must be positive, and a log-normal prior expresses expectations from about 40% shrinkage to 50% growth"
        correct: true
      - text: "Because the treatment is binary"
    explanation: "**p. 172:** p is the proportion of h0 that h1 is: p = 1 means no change and p = 2 doubling. A Log-Normal keeps p positive, and Log-Normal(0, 0.25) “expects anything from 40% shrinkage up to 50% growth.” Then p becomes a linear model of the treatment and fungus. Choosing the right scale for an effect (multiplicative speedup versus additive milliseconds) is a modelling decision engineers make constantly — 'the new allocator makes requests 10% faster' is proportional, not additive."
  - id: d-separation
    prompt: "In the plant DAG H0 → H1 ← F ← T, what does it mean that conditioning on F *d-separates* T from H1?"
    options:
      - text: "F is unobserved"
      - text: "T and H1 are always independent"
      - text: "T causes H1 directly"
      - text: "Once F is known, there is no open path between T and H1, so T carries no further information about H1"
        correct: true
    explanation: "**p. 174:** “conditioning on F induces d-separation. The “d” stands for directional. D-separation means that some variables on a directed graph are independent of others. There is no path connecting them.” A testable implication of this DAG: H0 should be independent of T and F if nothing is conditioned on. In randomized experiments, you can check that pre-treatment variables are balanced across arms — the analogue of checking an A/A test."
  - id: post-treatment-creates-effect
    prompt: "Suppose fungus has *no* effect on growth, but an unobserved moisture variable M affects both fungus and growth. What happens if you include fungus in the model?"
    options:
      - text: "Fungus becomes a collider of T and M; conditioning on it opens a path T → F ← M → H1, creating a spurious association between treatment and growth"
        correct: true
      - text: "The treatment effect is estimated more precisely"
      - text: "Nothing; the treatment estimate stays at zero"
      - text: "The model fails to fit"
    explanation: "**p. 175:** “conditioning on a post-treatment variable can not only fool you into thinking the treatment doesn’t work. It can also fool you into thinking it does work.” A regression of H1 on T alone shows no effect (correctly); including F creates one. In production, 'controlling for' a downstream metric affected by both your change and some hidden load factor can manufacture a fake win."
  - id: heuristics
    prompt: "What is the book's view of causal inference heuristics such as 'control for pre-treatment variables, never post-treatment ones'?"
    options:
      - text: "Heuristics are always wrong"
      - text: "Heuristics can be safe in the contexts they were built for, but we still need principles to know when to deploy them"
        correct: true
      - text: "Heuristics are sufficient for all designs"
      - text: "Heuristics are only for frequentists"
    explanation: "**p. 171:** “There is nothing wrong, in principle, with heuristics. They are safe in the context for which they were developed. But we still need principles to know when to deploy them.” Like coding rules of thumb ('never use exceptions for control flow'): useful defaults, but you need the underlying principle to know when the rule does not apply."
---

Adding a variable that is a consequence of the treatment can hide a real effect — or create a fake one.

## Learning objectives

- Recognize post-treatment variables and why they bias causal estimates
- Model growth proportionally with a log-normal prior
- Explain d-separation using the plant DAG
- Show how a post-treatment variable can also create a spurious effect

## In a nutshell

```text
H0 → H1 ← F ← T            (fungus mediates the treatment)
Condition on F → T looks useless (pipe blocked)

H0 → H1 ← M → F ← T        (M unobserved, F has no effect)
Condition on F → T looks useful (collider opened)
```

| Model | βT | Question answered |
|---|---|---|
| H1 ~ T + F | ≈ 0 | Does T matter once fungus is known? |
| H1 ~ T | > 0 | What is the total effect of T? |

## For engineers

In an experiment, every metric measured after assignment is post-treatment: session length, pages viewed,
errors encountered. Do not 'control' for them when estimating the effect of the change. Use them to
understand mechanism, as separate questions.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 6.2, pp. 170–176.
- Rosenbaum PR. The consequences of adjustment for a concomitant variable that has been affected by the treatment. *JRSS A* 1984.
- Montgomery JM, Nyhan B, Torres M. How conditioning on posttreatment variables can ruin your experiment and what to do about it. *American Journal of Political Science* 2018. [doi:10.1111/ajps.12357](https://doi.org/10.1111/ajps.12357)
