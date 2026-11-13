---
title: "1.1 Statistical Golems"
questions:
  - id: what-is-a-golem
    prompt: "In the book's metaphor, what makes a statistical procedure a *golem*?"
    options:
      - text: "It is powerful, does exactly what it is told, and has no wisdom or foresight of its own"
        correct: true
      - text: "It is a model that learns causal structure from data automatically"
      - text: "It is any Bayesian model"
      - text: "It is a model that refuses to run on bad data"
    explanation: "The golem of Prague is a clay robot animated by truth that obeys its instructions literally and blindly. **p. 1:** “Scientists also make golems. Our golems rarely have physical form, but they too are often made of clay, living in silicon as computer code. These golems are scientific models.” A statistical procedure is the same: it will compute exactly what you specify, even when the specification is wrong for your question. Software engineers know this creature well — a deploy script, a cron job or an autoscaler does precisely what it was configured to do, including deleting production or scaling to zero during a spike. The responsibility for using it wisely stays with the engineer."
  - id: flowchart-problem
    prompt: "Introductory statistics often teaches a flowchart: answer a few questions about your data and it tells you which test to run. What is the book's main complaint about this?"
    options:
      - text: "Flowcharts are too slow to follow"
      - text: "The tools are not diverse enough for real research questions, and the flowchart encourages using procedures without understanding what they assume"
        correct: true
      - text: "Flowcharts only cover Bayesian tests"
      - text: "The tests in the flowchart are mathematically wrong"
    explanation: "**p. 3:** “The point isn’t that statistical tools are specialized. Of course they are. The point is that classical tools are not diverse enough to handle many common research questions.” A flowchart is a decision tree over a small menu of prebuilt golems (t-test, chi-square, ANOVA). When your problem does not match a leaf exactly — clustered data, measurement error, missing values, a causal question — the flowchart forces it into the nearest box. The analogy for engineers is copy-pasting a Stack Overflow answer for a problem that only resembles yours. The book's alternative is to learn how to *build* models from parts so that the model fits the question."
  - id: engineering-not-math
    prompt: "How does the book classify statistics?"
    options:
      - text: "As pure mathematics"
      - text: "As an empirical science like physics"
      - text: "As a branch of engineering: common design principles produce many specialized tools"
        correct: true
      - text: "As a branch of philosophy"
    explanation: "**p. 3:** “statistics is neither mathematics nor a science, but rather a branch of engineering. And like engineering, a common set of design principles and constraints produces a great diversity of specialized applications.” This framing matters for developers: you would not expect a single library to cover every networking need, but you would expect a person who understands TCP/IP to build or adapt one. The book aims to teach the design principles (likelihoods, priors, link functions, causal graphs) so you can engineer your own models rather than shop for tests."
  - id: no-causation
    prompt: "What does the book say statistical golems understand about cause and effect?"
    options:
      - text: "They infer causation when the sample size is large"
      - text: "They understand causation only if the data are randomized"
      - text: "Bayesian golems understand causation; frequentist ones do not"
      - text: "Nothing — no statistical tool on its own addresses the problem of inferring causes from evidence"
        correct: true
    explanation: "**p. 3:** “no statistical tool does anything on its own to address the basic problem of inferring causes from evidence. Statistical golems do not understand cause and effect.” A model can only measure associations in the data you give it. Whether those associations reflect causation depends on assumptions from outside the data — which the book later encodes in DAGs (Chapters 5 and 6). A dashboard that shows conversion rising after a release cannot, on its own, tell you the release caused it; a marketing campaign launched the same week is just as consistent with the numbers."
  - id: null-hypothesis-dogma
    prompt: "What does the book call the greatest obstacle it sees among students and colleagues?"
    options:
      - text: "The belief that the proper objective of statistical inference is to test null hypotheses"
        correct: true
      - text: "Lack of computing power"
      - text: "Lack of programming skill"
      - text: "Fear of mathematics"
    explanation: "**p. 4:** “The greatest obstacle that I encounter among students and colleagues is the tacit belief that the proper objective of statistical inference is to test null hypotheses.” The book does not say significance tests are always useless; it says making them *the* goal confuses rejecting a boring straw man (no effect at all) with learning which of several plausible mechanisms is true. In product terms: proving that a redesign has *some* nonzero effect on clicks is far less useful than estimating how big the effect is, for whom, and why."
  - id: golem-silicon
    prompt: "A team adopts an automated anomaly detector that flags any metric more than 3 standard deviations from its rolling mean. During a holiday traffic surge it pages everyone all night. Which lesson from the golem metaphor applies?"
    options:
      - text: "The detector is broken and should be rewritten in another language"
      - text: "The procedure did exactly what it was told; it had no knowledge that holidays change the baseline, so its assumptions, not its arithmetic, failed"
        correct: true
      - text: "Anomaly detection cannot work on time series"
      - text: "Three standard deviations is always the wrong threshold"
    explanation: "Golems are powerful and literal. The detector embodies an implicit model (stationary mean, symmetric noise, no seasonality). When the world violates that model, the procedure still runs and confidently produces nonsense. The book's remedy is to make the model explicit, so you can see and change its assumptions — for example by adding a seasonal term, or by making the baseline itself vary by day type, the way multilevel models (Chapter 13) let each cluster have its own intercept."
  - id: diverse-golems
    prompt: "Which statements reflect the chapter's view of the many classical procedures (t-tests, regression, etc.)?"
    type: multiple
    options:
      - text: "Each is useful in some particular context"
        correct: true
      - text: "Each is a special case of a small set of design principles"
        correct: true
      - text: "They should never be used"
      - text: "Ordinary linear regression is nearly always the best possible choice"
    explanation: "**p. 3:** “it remains a triumph of statistical science that there are now so many diverse golems, each useful in a particular context.” And on regression specifically: “it is nearly always possible to do better than ordinary linear regression, largely because of a phenomenon known as overfitting (Chapter 7).” The message is not anti-classical; many classical procedures reappear in the book as special cases of generalized linear models with flat priors. What changes is that you understand the parts."
---

The chapter opens with the legend of the golem of Prague: a clay robot that obeys commands literally and
eventually causes harm because it has no judgment. Statistical procedures are golems too.

## Learning objectives

- Explain the golem metaphor and what it says about responsibility
- Describe the limits of the statistical-test flowchart
- Explain why statistics is framed as engineering
- State why no statistical procedure infers causation on its own

## In a nutshell

- A **golem** is a powerful, literal machine without wisdom. Models are golems (p. 1–2).
- **Flowcharts of tests** offer a fixed menu; real problems often need custom models (p. 2–3).
- Statistics is **engineering**: learn the principles and build the tool you need (p. 3).
- **Causation is not in the data alone**; it comes from assumptions you add (p. 3).

## For engineers

| Golem in the book | Golem in your stack |
|---|---|
| A t-test run on clustered data | A retry loop with no backoff during an outage |
| Regression with every available variable | A feature store that joins in every column it can find |
| p < 0.05 as the release gate | A CI check that only verifies the build did not crash |

In each case the machine is not wrong about what it computes. It is wrong about what you meant.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 1.1, pp. 1–4.
- Scholem G. “The Idea of the Golem.” In *On the Kabbalah and Its Symbolism*, 1965 — background on the legend the book draws on.
