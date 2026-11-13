---
title: "13.5 Multilevel Posterior Predictions"
questions:
  - id: sense-nonsense
    prompt: "Why does the book insist on posterior predictions for multilevel models?"
    options:
      - text: "Because tables of parameters are illegal"
      - text: "Because complex models are nearly impossible to understand from tables, predictions check that software worked, and counterfactual predictions are needed for causal questions"
        correct: true
      - text: "Because predictions remove uncertainty"
      - text: "Because WAIC requires them"
    explanation: "**p. 426:** “Every model is a merger of sense and nonsense. When we understand a model, we can find its sense and control its nonsense. But as models get more complex, it is very difficult to impossible to understand them just by inspecting tables of posterior means and intervals.” And: “Once you believe the posterior is correct, implied predictions are needed to consider the causal effects.”"
  - id: same-clusters
    prompt: "How do you predict for a cluster that was in the sample (e.g. chimpanzee 2)?"
    options:
      - text: "Use only ᾱ"
      - text: "Simulate a new cluster"
      - text: "Use that cluster's own varying intercept samples, combined with the other parameters, pushed through the model"
        correct: true
      - text: "Use the raw data mean"
    explanation: "For an observed cluster, its own α samples are the right input (p. 426–427). `link` does this, or by hand: inv_logit(a[,2] + b[,treatment]). “if it is a Bayesian model, then it is generative. This means that predictions are made by pushing samples up through the model” (p. 428)."
  - id: new-clusters
    prompt: "How should you predict for a *new* cluster — a chimpanzee not in the sample?"
    options:
      - text: "Use the intercept of the most similar observed chimpanzee"
      - text: "Use the no-pooling estimate"
      - text: "It is impossible"
      - text: "Use the population parameters ᾱ and σ: either an average cluster (intercept = ᾱ) or, more honestly, simulated clusters drawn from Normal(ᾱ, σ)"
        correct: true
    explanation: "**p. 428:** “We can’t use any of the a parameter estimates, because those apply to other individuals. But we can make good use of the a_bar and sigma_a parameters. These parameters describe a statistical population of actors, and we can simulate new actors from it.” For a newly onboarded service, predict its error rate from the fleet distribution, not from any one existing service."
  - id: average-vs-marginal
    prompt: "What is the difference between predictions for an 'average actor' and predictions 'marginal of actor'?"
    options:
      - text: "Average-actor predictions fix the intercept at ᾱ and ignore variation among actors (narrow); marginal predictions simulate intercepts from Normal(ᾱ, σα), showing how variable individuals are (wide)"
        correct: true
      - text: "They are identical"
      - text: "Marginal predictions ignore treatment"
      - text: "Average-actor predictions include more uncertainty"
    explanation: "**p. 428:** the average actor's uncertainty “is much smaller than it really should be, if we wish to honestly represent the problem of what to expect from a new individual.” Simulating `a_sim ~ Normal(a_bar, sigma_a)` gives predictions that “average over the uncertainty among actors” (p. 429). Both are useful: one shows the treatment effect, the other the spread among individuals."
  - id: same-simulated-actor
    prompt: "When simulating new actors across all four treatments, why must each simulated actor's intercept be sampled once, outside the per-treatment function?"
    options:
      - text: "For speed only"
      - text: "So the same simulated individual is used in every treatment; otherwise treatment differences would be mixed with differences between individuals"
        correct: true
      - text: "Because R requires it"
      - text: "It does not matter"
    explanation: "**p. 429:** “It’s important to do the chimpanzee sampling outside the link function, because we want to reference the same simulate chimpanzee, whichever treatment we consider.” This is common random numbers in simulation: hold the unit fixed while varying the intervention, as in a paired benchmark run on the same machine."
  - id: ceiling-interaction
    prompt: "Figure 13.7 (right) shows 100 simulated actors across treatments. Why do actors with very large intercepts show little treatment effect?"
    options:
      - text: "They were excluded from treatment"
      - text: "Because of a bug"
      - text: "Ceiling effects in the logit link: when the probability of pulling left is already near 1, treatment shifts on the log-odds scale barely change probability"
        correct: true
      - text: "Because treatment effects vary by actor in the model"
    explanation: "**p. 430:** “Because this is a binomial model, in principle all parameters interact, due to ceiling and floor effects. For actors with very large intercepts, near the top of the plot, treatment has very little effect.” The same treatment coefficient produces different probability changes for different units — Chapter 10's compression, made visible."
  - id: post-stratification
    prompt: "What is post-stratification, and why combine it with multilevel models (MRP)?"
    options:
      - text: "Dropping strata with small samples"
      - text: "Removing outliers by stratum"
      - text: "Stratifying data after collecting it to compute p-values"
      - text: "Estimating outcomes for each demographic slice, then re-weighting by known population composition; multilevel models stabilize estimates for small slices"
        correct: true
    explanation: "**p. 430:** “The idea is to fit a model in which each demographic slice of the population ... has its own voting intention. Then the estimates of these intentions are re-weighted using general census information about the full voting population. Because there are usually many demographic categories, and samples can be small in some of them, post-stratification is often combined with multilevel modeling”. Product analytics example: a survey of users over-represents power users; estimate satisfaction per segment and re-weight by the true segment mix."
  - id: no-unique-procedure
    prompt: "What does the book say about generalizing predictions beyond the sample?"
    options:
      - text: "There is no unique procedure; the right one depends on the causal model, the statistical model and your goals — use the posterior to parameterize a simulation of the target generalization"
        correct: true
      - text: "There is one correct procedure"
      - text: "Generalization is impossible"
      - text: "Always use the average cluster"
    explanation: "**p. 428:** “there is no unique procedure for generalizing predictions outside of a sample. The right thing to do depends upon the causal model, the statistical model, and your goals. But if you have a generative model, then you can often think your way through it. The key idea is to use the posterior to parameterize a simulation that embodies the target generalization.”"
---

Multilevel models predict differently for observed clusters, average new clusters, and the population of
new clusters.

## Learning objectives

- Predict for observed clusters using their own varying effects
- Predict for new clusters using the population distribution
- Distinguish average-cluster from marginal predictions
- Explain post-stratification and MRP

## In a nutshell

| Target | Intercept used | Uncertainty |
|---|---|---|
| Observed cluster j | α[j] samples | Parameters |
| Average new cluster | ᾱ samples | Parameters only (too narrow for a real new unit) |
| Typical new cluster | draws from Normal(ᾱ, σ) | Parameters + between-cluster variation |
| Population average | Re-weight cluster predictions by known composition | Post-stratification |

```csharp
var newActor = post.Select(s => s.ABar + s.SigmaA * StandardNormal(rng)).ToArray();
var pTreatment = Enumerable.Range(0, 4)
    .Select(k => post.Select((s, i) => Logistic(newActor[i] + s.B[k])).ToArray())
    .ToArray();
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 13.5, pp. 426–431; Figure 13.7.
- Gelman A, Little TC. Poststratification into many categories using hierarchical logistic regression. *Survey Methodology* 1997.
- Wang W, Rothschild D, Goel S, Gelman A. Forecasting elections with non-representative polls. *International Journal of Forecasting* 2015.
