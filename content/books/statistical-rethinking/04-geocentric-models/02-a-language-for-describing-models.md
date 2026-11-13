---
title: "4.2 A Language for Describing Models"
questions:
  - id: recipe
    prompt: "What is the book's three-step general recipe for describing a model?"
    options:
      - text: "Choose a test, compute a statistic, compare with a threshold"
      - text: "Recognize variables (data and parameters), define each one in terms of other variables or a probability distribution, and the combination becomes a joint generative model"
        correct: true
      - text: "Collect data, fit a line, report R²"
      - text: "Choose a prior, choose a posterior, compute the likelihood"
    explanation: "**p. 77:** “(1) First, we recognize a set of variables to work with. Some of these variables are observable. We call these data. Others are unobservable things like rates and averages. We call these parameters. (2) We define each variable either in terms of the other variables or in terms of a probability distribution. (3) The combination of variables and their probability distributions defines a joint generative model that can be used both to simulate hypothetical observations as well as analyze real ones.” This is like declaring a schema: every field is either computed from others or drawn from a distribution."
  - id: hard-part
    prompt: "Where does the book say the biggest difficulty in model building usually lies?"
    options:
      - text: "In the mathematics"
      - text: "In the software"
      - text: "In the subject matter — which variables matter and how theory says to connect them"
        correct: true
      - text: "In choosing priors"
    explanation: "**p. 77:** “The biggest difficulty usually lies in the subject matter—which variables matter and how does theory tell us to connect them?—not in the mathematics.” Just as the hard part of system design is understanding the domain, not writing the classes. A model of deploy risk needs someone who knows which services share databases; no notation substitutes for that."
  - id: read-model
    prompt: "How should you read the line `μᵢ = α + β xᵢ`?"
    options:
      - text: "μ is randomly drawn near α + βx"
      - text: "μ is a parameter with a prior"
      - text: "The data y equals α + βx exactly"
      - text: "The mean for row i is defined deterministically by α, β and that row's x — there is no uncertainty in this relationship once α and β are known"
        correct: true
    explanation: "`=` lines are deterministic: μᵢ is computed, not sampled. The subscript i means it is computed per row, like a derived column in a SQL view. Stochastic lines use `~`. Distinguishing the two tells you which quantities have their own uncertainty (parameters with priors, observations with likelihoods) and which inherit uncertainty from those (deterministic functions)."
  - id: stochastic-meaning
    prompt: "What makes a relationship *stochastic* in the book's sense?"
    options:
      - text: "It is a mapping of a variable onto a distribution, because no single instance of the variable is known with certainty"
        correct: true
      - text: "It changes over time"
      - text: "It involves random number generators in code"
      - text: "It is nonlinear"
    explanation: "**p. 78:** “A stochastic relationship is just a mapping of a variable or parameter onto a distribution. It is stochastic because no single instance of the variable on the left is known with certainty. Instead, the mapping is probabilistic: Some values are more plausible than others, but very many different values are plausible under any model.” Note it applies equally to data and parameters."
  - id: assumptions-visible
    prompt: "From `W ~ Binomial(N, p)` alone, which assumption do you automatically know the model makes?"
    options:
      - text: "That p is uniformly distributed"
      - text: "That each toss is independent of the others"
        correct: true
      - text: "That N is large"
      - text: "That water and land are equally likely"
    explanation: "**p. 78:** “Once we know the model in this way, we automatically know all of its assumptions. We know the binomial distribution assumes that each sample (globe toss) is independent of the others”. Explicit notation makes assumptions reviewable, the way an explicit interface makes a contract reviewable. A reviewer of `errors ~ Binomial(requests, p)` can immediately ask: are requests from the same user independent?"
  - id: model-to-bayes
    prompt: "How does the model definition become a posterior?"
    options:
      - text: "Posterior = prior + likelihood"
      - text: "Take the likelihood alone"
      - text: "Multiply the likelihood line by the prior lines for each candidate parameter value, then standardize so the result sums (integrates) to 1"
        correct: true
      - text: "Draw from the prior until the data match"
    explanation: "The box on p. 78 writes Pr(p | w, n) = Binomial(w | n, p) Uniform(p | 0, 1) / ∫ Binomial(w | n, p) Uniform(p | 0, 1) dp and notes “That monstrous denominator is just the average likelihood again. It standardizes the posterior to sum to 1. The action is in the numerator”. Each `~` line contributes one factor. Writing a model is therefore the same as writing down the function the engine must evaluate."
---

A short section that teaches the notation used for every model in the rest of the book.

## Learning objectives

- Apply the three-step recipe for defining a model
- Read stochastic (`~`) and deterministic (`=`) lines
- Read assumptions directly off a model definition
- Connect a model definition to Bayes' theorem

## In a nutshell

```text
yᵢ ~ Normal(μᵢ, σ)        likelihood (stochastic, per row)
μᵢ = α + β xᵢ             linear model (deterministic, per row)
α  ~ Normal(178, 20)      prior
β  ~ Normal(0, 10)        prior
σ  ~ Exponential(1)       prior
```

| Symbol | Meaning | Programming analogue |
|---|---|---|
| `~` | Is distributed as | Declared random generator |
| `=` | Is defined as | Computed property |
| subscript i | Per observation | Per row in a table |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 4.2, pp. 77–78.
