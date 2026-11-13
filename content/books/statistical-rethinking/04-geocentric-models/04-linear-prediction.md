---
title: "4.4 Linear Prediction"
questions:
  - id: linear-model-strategy
    prompt: "What is the *linear model strategy*?"
    options:
      - text: "Assume the outcome is a straight line with no noise"
      - text: "Make the mean μ of the outcome's distribution a linear function of predictor variables and new parameters, and let the golem rank all such lines by plausibility"
        correct: true
      - text: "Fit the line that minimizes squared error and report it"
      - text: "Assume every variable is normally distributed"
    explanation: "**p. 92:** “The strategy is to make the parameter for the mean of a Gaussian distribution, µ, into a linear function of the predictor variable and other, new parameters that we invent.” **p. 93:** “We ask the golem: “Consider all the lines that relate one variable to the other. Rank all of these lines by plausibility, given these data.” The golem answers with a posterior distribution.” Noise stays in the likelihood: heights scatter around μᵢ with spread σ."
  - id: alpha-beta-meaning
    prompt: "In `μᵢ = α + β(xᵢ − x̄)`, what questions do α and β answer?"
    type: multiple
    options:
      - text: "α: the expected height when weight equals the average weight"
        correct: true
      - text: "β: the change in expected height when weight increases by 1 unit"
        correct: true
      - text: "α: the height of a person with zero weight"
      - text: "β: the correlation between height and weight"
    explanation: "**p. 94:** “(1) What is the expected height when xi = x̄? The parameter α answers this question ... (2) What is the change in expected height, when xi changes by 1 unit? The parameter β answers this question.” Because x is centered, α is the mean height at the mean weight, not at weight zero (a nonsensical person). The book prefers to call β “a rate of change in expectation” rather than a slope. Units help: β is in cm/kg (p. 94)."
  - id: made-up-params
    prompt: "Where do α and β come from?"
    options:
      - text: "They are properties of the Gaussian distribution"
      - text: "They are estimated from the prior only"
      - text: "We invent them as devices for letting μ vary systematically across cases; μ and σ describe the Gaussian, α and β describe how μ changes"
        correct: true
      - text: "They are always the intercept and slope of the least-squares line"
    explanation: "**p. 93:** “We made them up. The parameters µ and σ are necessary and sufficient to describe a Gaussian distribution. But α and β are instead devices we invent for manipulating µ, allowing it to vary systematically across cases in the data.” Parameters are design decisions. Like configuration knobs you add to a system, each should have a clear meaning and a sensible range."
  - id: nothing-special-linear
    prompt: "According to the box “Nothing special or natural about linear models”, what else could μᵢ be?"
    options:
      - text: "Nothing; regression must be linear"
      - text: "Only polynomial functions"
      - text: "Only functions with a closed-form posterior"
      - text: "Any function of parameters and predictors, for example μᵢ = α exp(−β xᵢ); models built from substantive theory can dramatically outperform linear ones"
        correct: true
    explanation: "**p. 94:** “there’s nothing special about the linear model, really. You can choose a different relationship between α and β and µ.” And: “Models built out of substantive theory can dramatically outperform linear models of the same phenomena.” Chapter 16 develops this. For a queueing system, a theory-based form (latency rising sharply as utilization approaches 1) beats a straight line in utilization."
  - id: beta-prior-bad
    prompt: "Prior predictive lines with `β ~ Normal(0, 10)` (Figure 4.5, left) show heights from far below zero to above 272 cm across the weight range. What is wrong with this prior?"
    options:
      - text: "It implies that the weight–height relationship could be absurdly positive or negative, which no human population shows; before seeing data it is a bad model"
        correct: true
      - text: "It is too narrow"
      - text: "Nothing; vague priors are safe"
      - text: "It causes the posterior to be non-Gaussian"
    explanation: "**p. 95–96:** the pattern “doesn’t look like any human population at all. It essentially says that the relationship between weight and height could be absurdly positive or negative. Before we’ve even seen the data, this is a bad model.” Only prior predictive simulation reveals this — “There is no other reliable way to understand.” Same idea as checking the implied range of a config value: a 'retry multiplier' allowed to be negative or 1,000 is a bug waiting to happen."
  - id: lognormal-prior
    prompt: "Why does the book switch to `β ~ Log-Normal(0, 1)`?"
    options:
      - text: "Because it is the conjugate prior"
      - text: "Because it forces β to be positive, encoding the knowledge that average height increases with average weight"
        correct: true
      - text: "Because it is flat"
      - text: "Because weights are log-normal"
    explanation: "**p. 96:** “If the logarithm of β is normal, then β itself is strictly positive. The reason is that exp(x) is greater than zero for any real number x. This is the reason that Log-Normal priors are commonplace. They are an easy way to enforce positive relationships.” The book stresses that it fusses about the prior even though the data swamp it, because “there are many analyses in which no amount of data makes the prior irrelevant.”"
  - id: correct-prior
    prompt: "What is the book's answer to “What’s the correct prior?”"
    options:
      - text: "The prior that maximizes the posterior"
      - text: "The flat prior is always correct"
      - text: "There is no uniquely correct prior, just as there is no uniquely correct likelihood; priors are part of a machine for inference and can be evaluated by their implications"
        correct: true
      - text: "The prior that a journal requires"
    explanation: "**p. 96:** “This is a mistake. There is no more a uniquely correct prior than there is a uniquely correct likelihood. Statistical models are machines for inference. Many machines will work, but some work better than others.” And on 'objective' defaults (p. 97): “all “objective” means is that everyone does the same thing. It carries no guarantees of realism or accuracy.” Default timeouts are 'objective' in the same way — everyone uses 30 s — which does not make them right for your service."
  - id: p-hacking
    prompt: "How does the book's prior-setting procedure avoid a form of p-hacking?"
    options:
      - text: "By never using priors"
      - text: "By fitting the model many times"
      - text: "By using 89% intervals"
      - text: "By choosing priors from pre-data knowledge of constraints and ranges, not from the observed sample"
        correct: true
    explanation: "**p. 97:** “The procedure we’ve performed in this chapter is to choose priors conditional on pre-data knowledge of the variables—their constraints, ranges, and theoretical relationships. This is why the actual data are not shown in the earlier section. We are judging our priors against general facts, not the sample.” Choosing priors after peeking to get a desired result would be p-hacking by another name — like tuning an A/B test's metric after seeing which one moved."
  - id: everything-has-posterior
    prompt: "μᵢ is no longer a parameter. Does it have a posterior distribution?"
    options:
      - text: "Yes; anything computed from uncertain parameters is uncertain, and you get its posterior by computing it for each posterior sample"
        correct: true
      - text: "Only if we add a prior for μ"
      - text: "No; it is deterministic"
      - text: "No; only parameters have posteriors"
    explanation: "**p. 98:** “Since parameters are uncertain, everything that depends upon them is also uncertain ... By working with samples from the posterior, all you have to do to account for posterior uncertainty in any quantity is to compute that quantity for each sample from the posterior.” In code: `post.Select(s => s.A + s.B * (50 - xbar))` gives the posterior of mean height at 50 kg. Same for any derived metric — projected cost, SLA breach probability, anything."
  - id: plots-over-tables
    prompt: "Why does the book emphasize plotting posterior predictions over reading tables of estimates?"
    type: multiple
    options:
      - text: "Plots reveal whether the fitting procedure worked"
        correct: true
      - text: "Plots show the absolute magnitude of a relationship"
        correct: true
      - text: "Plots show uncertainty in the average relationship and in implied predictions"
        correct: true
      - text: "Tables are mathematically incorrect"
    explanation: "**p. 99** lists four things plots reveal: “(1) Whether or not the model fitting procedure worked correctly (2) The absolute magnitude, rather than merely relative magnitude, of a relationship between outcome and predictor (3) The uncertainty surrounding an average relationship (4) The uncertainty surrounding the implied predictions of the model, as these are distinct from mere parameter uncertainty”. Tables become unreadable once there are interactions or polynomials. Dashboards over raw numbers, for the same reason."
  - id: mu-vs-prediction-interval
    prompt: "Figure 4.10 shows a narrow band around the regression line and a much wider band. What are they?"
    options:
      - text: "The narrow band is the prior; the wide band the posterior"
      - text: "The narrow band is the uncertainty in the average height μ at each weight; the wide band is the prediction interval for individual heights, which also includes σ"
        correct: true
      - text: "Both are the same interval at different probabilities"
      - text: "The narrow band is the 50% interval; the wide one is 99%"
    explanation: "`link` computes μ for each posterior sample (uncertainty about the *mean*); `sim` additionally draws heights from Normal(μ, σ) (uncertainty about *individuals*). **p. 108:** the wide region “represents the area within which the model expects to find 89% of actual heights in the population, at each weight.” In capacity planning, the first is uncertainty in average load at a given hour; the second is the range of actual load you must provision for. Confusing them is a classic way to under-provision."
  - id: overconfident-band
    prompt: "The compatibility band around the regression line is very tight. What does the book caution about such tight bands?"
    options:
      - text: "They prove the relationship is linear"
      - text: "They are always wrong"
      - text: "They are conditional on the model: given a straight line, the most plausible lines are tightly clustered — but a different model could tell a different story"
        correct: true
      - text: "They mean σ is small"
    explanation: "The box “Overconfident intervals” (p. 107) warns that the inferences “are always conditional on the model.” A tight band says: *if* the relationship is a straight line, then this line is well determined. It says nothing about whether a line was the right choice. A precisely-fitted linear capacity trend can be confidently wrong if growth is actually exponential."
  - id: centering
    prompt: "Why does the book subtract x̄ from weight in the linear model?"
    options:
      - text: "It removes outliers"
      - text: "It is required for the Gaussian likelihood"
      - text: "It changes the slope estimate"
      - text: "Centering makes α the expected outcome at the average predictor value, makes priors easier to set, and reduces the posterior correlation between α and β"
        correct: true
    explanation: "Without centering, α is the expected height at weight 0 — a value outside the data with no natural prior — and α and β become strongly correlated in the posterior, because tilting the line around a distant pivot forces the intercept to move. Centering pivots the line at the data's center. The slope's posterior is unchanged. The habit is like choosing a sensible epoch for timestamps in a regression on time: center on 'now', not 1970."
---

Adding a predictor: height as a linear function of weight.

## Learning objectives

- Build a linear model for the mean and interpret α and β
- Use prior predictive simulation to choose priors for slopes
- Explain why every derived quantity has a posterior
- Distinguish uncertainty in μ from prediction intervals for observations
- Explain why centering helps

## In a nutshell

```text
hᵢ ~ Normal(μᵢ, σ)
μᵢ = α + β (xᵢ − x̄)
α  ~ Normal(178, 20)
β  ~ Log-Normal(0, 1)
σ  ~ Uniform(0, 50)
```

| Tool | Computes | Uncertainty included |
|---|---|---|
| `link` | μ for each posterior sample at each x | Parameters |
| `sim` | Simulated observations at each x | Parameters + σ |
| `extract.samples` | Joint posterior samples | Parameters |

## Posterior of μ at a new weight in C#

```csharp
record Sample(double A, double B, double Sigma);
IEnumerable<double> MuAt(IEnumerable<Sample> post, double weight, double xbar) =>
    post.Select(s => s.A + s.B * (weight - xbar));
IEnumerable<double> HeightAt(IEnumerable<Sample> post, double weight, double xbar, Random rng) =>
    post.Select(s => s.A + s.B * (weight - xbar) + s.Sigma * StandardNormal(rng));
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 4.4, pp. 91–110; Figures 4.5–4.10.
- Galton F. Regression towards mediocrity in hereditary stature. *Journal of the Anthropological Institute* 1886.
