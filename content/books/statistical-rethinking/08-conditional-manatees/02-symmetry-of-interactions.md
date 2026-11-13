---
title: "8.2 Symmetry of Interactions"
questions:
  - id: two-phrasings
    prompt: "The ruggedness interaction can be phrased two ways. How does the model treat them?"
    options:
      - text: "Only the first phrasing is valid"
      - text: "As two different models that must be compared"
      - text: "As identical: 'how much does the ruggedness effect depend on Africa?' and 'how much does the Africa effect depend on ruggedness?' are the same model"
        correct: true
      - text: "Only the second phrasing is valid"
    explanation: "**p. 251:** “(1) How much does the association between ruggedness and log GDP depend upon whether the nation is in Africa? (2) How much does the association of Africa with log GDP depend upon ruggedness? While these two possibilities sound different to most humans, your golem thinks they are identical.” Mathematically the same μᵢ can be rewritten either way."
  - id: buridan
    prompt: "What is the point of the reference to Buridan's ass?"
    options:
      - text: "Interactions cause indecision in MCMC"
      - text: "Interactions are always asymmetric"
      - text: "Models must choose randomly"
      - text: "Simple interactions are symmetric: within the model there is no basis to prefer one interpretation, though our causal minds prefer one"
        correct: true
    explanation: "**p. 252:** “Simple interactions are symmetric, just like the choice facing Buridan’s ass. Within the model, there’s no basis to prefer one interpretation over the other, because in fact they are the same interpretation. But when we reason causally about models, our minds tend to prefer one interpretation over the other.” The preference comes from the causal story (we can imagine changing ruggedness, not moving a country to another continent), not from the statistics."
  - id: other-side
    prompt: "Figure 8.6 plots the expected difference in log GDP between being in Africa and not, across ruggedness. What does it reveal?"
    options:
      - text: "At low ruggedness, being in Africa predicts lower GDP; only at very high ruggedness do African and non-African nations have similar expected GDP"
        correct: true
      - text: "Continent has no effect"
      - text: "The difference is constant across ruggedness"
      - text: "African nations are always better off"
    explanation: "**p. 252:** “At low ruggedness, we expect “moving” a nation to Africa to hurt its economy. But at high ruggedness, the opposite is true. The association between continent and economy depends upon ruggedness, just as much as the association between ruggedness and economy depends upon continent.” And this view makes it clear that African nations are on average nearly always worse off. Plotting both sides of an interaction often reveals what one side hides."
  - id: devops-symmetry
    prompt: "A model finds an interaction between instance type and request payload size on latency. A teammate says 'payload size matters more on small instances'. Another says 'instance size matters more for large payloads'. Who is right?"
    options:
      - text: "Only the first teammate"
      - text: "Both: a simple interaction is symmetric, so both statements describe the same model; which one is more useful depends on which variable you can change"
        correct: true
      - text: "Only the second teammate"
      - text: "Neither; interactions cannot be interpreted"
    explanation: "Algebraically, μ = α + βP·P + βI·I + βPI·P·I can be grouped as (βP + βPI·I)·P + βI·I or as (βI + βPI·P)·I + βP·P. Both say the same thing. If you control instance choice but not payloads, the second framing (the benefit of a bigger instance grows with payload size) is the actionable one."
  - id: rewrite
    prompt: "The book rewrites μᵢ = α[cid] + β[cid](rᵢ − r̄) as (2 − cidᵢ)(α₁ + β₁(rᵢ − r̄)) + (cidᵢ − 1)(α₂ + β₂(rᵢ − r̄)). What does this show?"
    options:
      - text: "That the model has four separate equations"
      - text: "That cid must be continuous"
      - text: "That the intercept can equally be read as conditional on ruggedness: switching a nation's continent changes its prediction by an amount that depends on its ruggedness"
        correct: true
      - text: "That the slopes are identical"
    explanation: "**p. 251:** “The interpretation previously has been that the slope is conditional on continent. But it’s also fine to say that the intercept is conditional on ruggedness.” The rewrite makes the switch explicit: for cid = 1 only the Africa terms remain, for cid = 2 only the others. The difference between the two terms varies with r."
---

Interactions do not have a direction. Any simple interaction can be read either way.

## Learning objectives

- Explain why a simple interaction is symmetric
- Plot both sides of an interaction
- Choose the actionable framing using causal knowledge

## In a nutshell

```text
μ = α + βA·A + βB·B + βAB·A·B
  = α + (βA + βAB·B)·A + βB·B     effect of A depends on B
  = α + (βB + βAB·A)·B + βA·A     effect of B depends on A
```

Same parameters, same predictions, two stories. The statistics cannot pick one; your causal model can.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 8.2, pp. 250–252; Figure 8.6.
