---
title: "6.4 Confronting Confounding"
questions:
  - id: confounding-definition
    prompt: "How does the book define *confounding*?"
    options:
      - text: "Any correlation between two predictors"
      - text: "Any omitted variable"
      - text: "Any context in which the association between an outcome Y and a predictor of interest X is not the same as it would be if we had experimentally determined the values of X"
        correct: true
      - text: "Measurement error in the outcome"
    explanation: "**p. 183:** “Let’s define confounding as any context in which the association between an outcome Y and a predictor of interest X is not the same as it would be, if we had experimentally determined the values of X.” This definition is about interventions: the gold standard is what you would see if you set X yourself, like flipping a feature flag for a random cohort. Anything that makes the observational association differ from that is confounding."
  - id: randomization-blocks
    prompt: "In E ← U → W with E → W (education, unobserved U, wages), why does randomly assigning education remove confounding?"
    options:
      - text: "It makes U observable"
      - text: "It increases sample size"
      - text: "It makes W independent of E"
      - text: "Manipulation removes the influence of U on E, deleting the arrow U → E, so the backdoor path E ← U → W disappears"
        correct: true
    explanation: "**p. 184:** “Manipulation removes the influence of U on E. The unobserved variables do not influence education when we ourselves determine education. With the influence of U removed from E, this then removes the path E ← U → W. It blocks the second path.” Randomized rollouts work this way: the hash of a user ID decides exposure, so nothing about the user (U) can influence it."
  - id: condition-on-u
    prompt: "Without an experiment, how can conditioning on U remove the confounding in E ← U → W?"
    options:
      - text: "Once U is known, learning E gives no additional information about W through that path, so the backdoor is blocked"
        correct: true
      - text: "Conditioning on U deletes the arrow E → W"
      - text: "It cannot; only experiments help"
      - text: "Conditioning on U makes E random"
    explanation: "**p. 184:** “it also blocks the flow of information between E and W through U.” If U is regional wealth driving both schools and pay, then once you know a person's region, education no longer tells you anything about wages via region. Stratifying an analysis by data center when the data center drives both a config choice and performance is the same move."
  - id: four-confounds
    prompt: "Which statements about the four elemental confounds (Figure 6.6) are correct?"
    type: multiple
    options:
      - text: "Fork X ← Z → Y: X and Y are independent conditional on Z"
        correct: true
      - text: "Pipe X → Z → Y: conditioning on Z blocks the path"
        correct: true
      - text: "Collider X → Z ← Y: X and Y are independent unless you condition on Z, which opens the path"
        correct: true
      - text: "Descendant: conditioning on a descendant of Z partly conditions on Z itself"
        correct: true
      - text: "Collider: conditioning on Z blocks the path"
    explanation: "**p. 185:** “So in both a fork and a pipe, conditioning of the middle variable blocks the path.” For colliders: “there is no association between X and Y unless you condition on Z. Conditioning on Z, the collider variable, opens the path.” For descendants: “Conditioning on a descendent partly conditions on its parent.” Every DAG is built from these four. A proxy metric (descendant) — e.g. a log line count that tracks a hidden queue depth — behaves partly like the hidden variable."
  - id: descendant-proxy
    prompt: "You cannot measure true user intent (Z), but you have 'search query length' (D), which is influenced by intent. What does the descendant rule say about conditioning on D?"
    options:
      - text: "It has no effect because D is not Z"
      - text: "It partly conditions on Z, with effects that depend on Z's role: partly blocking if Z is a fork or pipe, partly opening if Z is a collider"
        correct: true
      - text: "It always removes confounding"
      - text: "It always creates confounding"
    explanation: "**p. 185:** “the consequence of conditioning on a descendent depends upon the nature of its parent. Descendants are common, because often we cannot measure a variable directly and instead have only some proxy for it.” Proxies are partial measurements. They can partially fix confounding (if the parent is a confounder) or partially create it (if the parent is a collider). Treat every proxy metric by asking what it is a descendant of."
  - id: backdoor-recipe
    prompt: "What is the recipe for applying the backdoor criterion to estimate X → Y?"
    options:
      - text: "Condition on every variable measured before X"
      - text: "Condition on every variable correlated with Y"
      - text: "List every path between X and Y; classify each as open or closed by whether it contains a collider; for backdoor paths (those with an arrow entering X), condition on variables that close open ones without opening closed ones"
        correct: true
      - text: "Drop every variable correlated with X"
    explanation: "The book's procedure (p. 185–186): “(1) List all of the paths connecting X (the potential cause of interest) and Y (the outcome). (2) Classify each path by whether it is open or closed. A path is open unless it contains a collider. (3) Classify each path by whether it is a backdoor path. A backdoor path has an arrow entering X. (4) If there are any open backdoor paths, decide which variable(s) to condition on to close it (if possible).” It is a graph algorithm; tools like dagitty run it for you via `adjustmentSets`."
  - id: two-roads
    prompt: "In the 'two roads' DAG (A → U → X, A → C → Y, U → B ← C, X → Y, U unobserved), which variables could you condition on, and which must you avoid?"
    options:
      - text: "Condition on B only"
      - text: "Condition on all of A, B and C"
      - text: "No adjustment is possible because U is unobserved"
      - text: "Condition on A or C (C is better for precision); do not condition on B, which is a collider that would open the second path"
        correct: true
    explanation: "**p. 186:** path (1) X ← U ← A → C → Y is open, and conditioning on A or C closes it; path (2) X ← U → B ← C → Y is closed because B is a collider. “If a backdoor path is closed already, then we must not accidentally open it and create a confound.” dagitty returns `{ C }` and `{ A }`, and “Conditioning on C is the better idea, from the perspective of efficiency, since it could also help with the precision of the estimate of X → Y.”"
  - id: backdoor-waffles
    prompt: "In the Waffle House DAG, all backdoor paths from W to D pass through South (S). What are the minimal adjustment sets?"
    options:
      - text: "{ S } or { A, M }"
        correct: true
      - text: "{ W }"
      - text: "{ D }"
      - text: "No set works"
    explanation: "**p. 187:** “There are three open backdoor paths between W and D ... all of them pass first through S. So we can close them all by conditioning on S.” dagitty returns `{ A, M }` and `{ S }`. The book adds that this DAG “assumes there are no unobserved confounds, which is very unlikely for this sort of data.” An adjustment set is only as good as the DAG."
  - id: implied-independencies
    prompt: "The Waffle DAG implies `A ⊥ W | S`, `D ⊥ S | A, M, W` and `M ⊥ W | S`. Why are these useful?"
    options:
      - text: "They prove the DAG is correct"
      - text: "They are testable implications: if the data contradict them, the DAG is wrong. The data can never prove a DAG right, but they can show it is wrong"
        correct: true
      - text: "They tell you the causal effect of W"
      - text: "They are required by quap"
    explanation: "**p. 187:** “The data alone can never tell us when a DAG is right. But the data can tell us when a DAG is wrong.” Conditional independencies are the DAG's unit tests. Like tests in code, passing them does not prove correctness, but a failure tells you the model of the world is wrong somewhere."
  - id: dags-not-enough
    prompt: "What does the box “DAGs are not enough” say?"
    options:
      - text: "DAGs are useless"
      - text: "DAGs are the final goal of science"
      - text: "DAGs are great when you lack a mechanistic model, but they are not a destination; dynamical and domain-specific structural models can do more, even where a DAG cannot decide"
        correct: true
      - text: "DAGs must always include every variable"
    explanation: "**p. 188:** “If you don’t have a real, mechanistic model of your system, DAGs are fantastic tools. They make assumptions transparent and easier to critique ... But DAGs are not a destination. Once you have a dynamical model of your system, you don’t need a DAG.” For software systems with queueing theory or explicit control loops (autoscalers, rate limiters), a dynamical model can say far more than a DAG of metrics. Chapter 16 develops such models."
  - id: summary-golem
    prompt: "How does the chapter summary characterize multiple regression?"
    options:
      - text: "As an oracle that reveals causes"
      - text: "As valid only in experiments"
      - text: "As obsolete"
      - text: "As no oracle, only a golem: logical, but describing conditional associations rather than causal influences, so outside information is needed"
        correct: true
    explanation: "**p. 189:** “Multiple regression is no oracle, but only a golem. It is logical, but the relationships it describes are conditional associations, not causal influences. Therefore additional information, from outside the model, is needed to make sense of it.” The good news: “it is possible to reach valid causal inferences in the absence of experiments.” Many engineering decisions cannot be A/B tested (infrastructure migrations, org changes); careful causal reasoning on observational data is often all you have."
---

The unifying framework: four elemental relations, the backdoor criterion, and testable implications.

## Learning objectives

- Define confounding in terms of interventions
- Classify forks, pipes, colliders and descendants and their conditional (in)dependencies
- Apply the backdoor criterion to find adjustment sets
- Use implied conditional independencies to test a DAG

## In a nutshell

| Relation | Shape | X ⊥ Y? | X ⊥ Y \| Z? |
|---|---|---|---|
| Fork | X ← Z → Y | No | Yes |
| Pipe | X → Z → Y | No | Yes |
| Collider | X → Z ← Y | Yes | No |
| Descendant | Z → D | — | Conditioning on D partly conditions on Z |

```text
Backdoor criterion
1. List all paths between X and Y
2. A path is open unless it contains a collider (that is not conditioned on)
3. Backdoor paths have an arrow entering X
4. Condition on variables that close every open backdoor path, without opening closed ones
```

## For engineers

Write the DAG of your system's metrics before analysing an incident or a migration. Run
`adjustmentSets` (dagitty, also available at dagitty.net in the browser) to know what to stratify by,
and check the implied independencies against your telemetry.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 6.4, pp. 183–189; Figure 6.6.
- Pearl J. *Causality*, 2nd ed. Cambridge University Press, 2009.
- Hernán MA, Robins JM. *Causal Inference: What If*. Chapman & Hall/CRC, 2020. [Free online](https://www.hsph.harvard.edu/miguel-hernan/causal-inference-book/)
- dagitty: [dagitty.net](http://www.dagitty.net/)
