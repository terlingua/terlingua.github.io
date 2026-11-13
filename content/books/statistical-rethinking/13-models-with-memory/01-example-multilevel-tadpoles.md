---
title: "13.1 Example: Multilevel Tadpoles"
questions:
  - id: amnesia
    prompt: "What does the chapter mean by saying models with dummy (index) variables for clusters have *anterograde amnesia*?"
    options:
      - text: "They treat each cluster's parameter as learning only from that cluster's data, forgetting everything learned about other clusters"
        correct: true
      - text: "They forget the data after fitting"
      - text: "They cannot store more than one parameter"
      - text: "They cannot make predictions"
    explanation: "**p. 399:** “These models implicitly assume that nothing learned about any one category informs estimates for the other categories—the parameters are independent of one another and learn from completely separate portions of the data. This would be like forgetting you had ever been in a café, each time you go to a new café. Cafés do differ, but they are also alike.” A per-service error-rate dashboard that computes each service's rate in isolation is amnesiac: a service with 3 requests and 1 error shows 33%."
  - id: cafe-robot
    prompt: "In the café robot story, what should the robot's prior be when it reaches a new café?"
    options:
      - text: "The posterior from the first café, unchanged"
      - text: "A distribution for the population of cafés, learned from all cafés visited so far; its width (how much cafés differ) is itself estimated"
        correct: true
      - text: "The original vague prior, ignoring the first café"
      - text: "Exactly the waiting time at the last café"
    explanation: "**p. 400:** the robot tracks a parameter for each café and “at least two parameters to describe the population of cafés: an average and a standard deviation ... If the population seems highly variable, then the prior is flat and uninformative ... If instead the population seems to contain little variation, then the prior is narrow and highly informative.” This is a multilevel model: the prior for each cluster is learned from the clusters."
  - id: benefits
    prompt: "Which pragmatic benefits of multilevel models does the chapter repeat from Chapter 1?"
    type: multiple
    options:
      - text: "Improved estimates for repeat sampling"
        correct: true
      - text: "Improved estimates for imbalance in sampling, so over-sampled clusters do not dominate"
        correct: true
      - text: "Explicit estimates of variation among clusters"
        correct: true
      - text: "Avoiding pre-averaging, which manufactures false confidence"
        correct: true
    explanation: "**p. 400:** “(1) Improved estimates for repeat sampling ... (2) Improved estimates for imbalance in sampling ... This prevents over-sampled clusters from unfairly dominating inference. (3) Estimates of variation ... (4) Avoid averaging, retain variation.” And “When it comes to regression, multilevel regression deserves to be the default approach.” Telemetry has all four: many requests per host, a few huge tenants, real per-region variation, and dashboards built on averages of averages."
  - id: adaptive-prior
    prompt: "In the tadpole model αⱼ ~ Normal(ᾱ, σ), with priors on ᾱ and σ, where does the 'multi' in multilevel come from?"
    options:
      - text: "From having multiple outcome variables"
      - text: "From running multiple chains"
      - text: "From the prior for the tank intercepts having its own parameters (ᾱ, σ) with their own priors — two levels, each like a simpler model"
        correct: true
      - text: "From multiple predictors"
    explanation: "**p. 403:** “the prior for the tank intercepts is now a function of two parameters, ᾱ and σ ... These two parameters inside the prior is where the “multi” in multilevel arises.” Top level: survival counts given tank intercepts. Second level: “the “outcome” variable is the vector of intercept parameters, α.” The prior is *adaptive* because ᾱ and σ are learned."
  - id: effective-params
    prompt: "The multilevel tadpole model has 50 parameters but WAIC's pWAIC is about 21, fewer than the 48-intercept non-pooled model's ~26. Why?"
    options:
      - text: "WAIC is miscalculated"
      - text: "Some tanks were dropped"
      - text: "Multilevel models have fewer data"
      - text: "The adaptive prior shrinks intercepts toward ᾱ, so they are less free to fit noise; regularization reduces effective complexity"
        correct: true
    explanation: "**p. 404:** “the multilevel model has only 21 effective parameters. There are 28 fewer effective parameters than actual parameters, because the prior assigned to each intercept shrinks them all towards the mean ᾱ ... This is a regularizing prior ... but now the amount of regularization has been learned from the data itself.” More parameters, less overfitting."
  - id: shrinkage-figure
    prompt: "Figure 13.1 compares raw tank survival proportions with multilevel estimates. Which patterns appear?"
    type: multiple
    options:
      - text: "Every estimate is pulled toward the overall average"
        correct: true
      - text: "Small tanks are pulled more than large tanks"
        correct: true
      - text: "Tanks far from the average are pulled more than tanks near it"
        correct: true
      - text: "Estimates move away from the average"
    explanation: "Caption, Figure 13.1: “In every tank, the posterior mean from the multilevel model is closer to the dashed line than the empirical proportion is. This reflects the pooling of information across tanks.” Shrinkage is stronger for small samples (less information) and for extreme values (more likely partly noise). The 'top 10 worst services' on a raw error dashboard is dominated by low-traffic services; shrunk estimates fix that ranking."
  - id: population-distribution
    prompt: "What does the posterior of ᾱ and σ let you do that per-tank estimates alone cannot?"
    options:
      - text: "Describe the population of tanks and simulate survival probabilities for new, unobserved tanks, with uncertainty in both location and spread"
        correct: true
      - text: "Nothing more"
      - text: "Remove the need for data"
      - text: "Estimate each tank exactly"
    explanation: "Figure 13.2 (p. 407) shows 100 Gaussian population distributions from the posterior and survival for 8,000 simulated new tanks. “there is uncertainty about both the location, α, and scale, σ, of the population distribution of log-odds of survival. All of this uncertainty is propagated into the simulated probabilities.” Useful for capacity planning: 'what error rate should we expect from the next service we onboard?'"
  - id: varying-overdispersion
    prompt: "How are varying intercepts related to the over-dispersion models of Chapter 12?"
    options:
      - text: "They are unrelated"
      - text: "Varying intercepts also allow over-dispersion: each observation or cluster gets its own deviation from the mean, much like a beta-binomial or gamma-Poisson, but with more flexibility"
        correct: true
      - text: "Varying intercepts remove the need for priors"
      - text: "Varying intercepts only work for Gaussian outcomes"
    explanation: "From the box “Varying intercepts as over-dispersion” (p. 407): varying intercepts accomplish the same thing as continuous mixtures, “because when each observed count gets its own unique intercept, but these intercepts are pooled through a common distribution, the predictions expect over-dispersion just like a beta-binomial or gamma-Poisson model would.” Unlike continuous mixtures, they work at any level of aggregation and combine with other predictors."
  - id: half-normal
    prompt: "What alternative to Exponential(1) priors for variance components does the book mention, and why?"
    options:
      - text: "Uniform(0, 100), to be less informative"
      - text: "Normal(0, 1), allowing negative σ"
      - text: "Half-Normal(0, 1), which has a thinner tail and can help when sampling is inefficient because σ wanders to large values"
        correct: true
      - text: "No prior at all"
    explanation: "**p. 408:** “instead of using exponential priors for the variance components, you can use half-Normal priors or some other prior with a thin tail. A half-Normal is a Normal distribution with all mass above zero.” In Stan, give the parameter `lower=0` and a normal prior."
---

The book's central model: clusters that share information through a learned population distribution.

## Learning objectives

- Explain anterograde amnesia in single-level models and the café robot
- Build a varying-intercepts model with an adaptive prior
- Explain shrinkage and effective number of parameters
- Use the population distribution to predict new clusters

## In a nutshell

```text
Sᵢ ~ Binomial(Nᵢ, pᵢ)
logit(pᵢ) = α[tank]
α_j ~ Normal(ᾱ, σ)        adaptive prior
ᾱ   ~ Normal(0, 1.5)
σ   ~ Exponential(1)
```

| Model | Information sharing | Effective parameters |
|---|---|---|
| One α for all tanks | Complete (ignores differences) | ~2 |
| α per tank, fixed prior | None (amnesia) | ~26 |
| α per tank, adaptive prior | Partial, learned | ~21 |

## For engineers

Per-tenant, per-host, per-endpoint and per-region metrics are clustered data. A multilevel model gives every
cluster an estimate that borrows strength from the fleet, in proportion to how little data the cluster has.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 13 opening and Section 13.1, pp. 399–408; Figures 13.1–13.2.
- Vonesh JR, Bolker BM. Compensatory larval responses shift trade-offs associated with predator-induced hatching plasticity. *Ecology* 2005.
- Efron B, Morris C. Stein's paradox in statistics. *Scientific American* 1977.
