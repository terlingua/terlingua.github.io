---
title: "4.3 Gaussian Model of Height"
questions:
  - id: distribution-of-distributions
    prompt: "In the model `hᵢ ~ Normal(μ, σ)`, what is the 'estimate' the book produces?"
    options:
      - text: "A single best μ and σ"
      - text: "A confidence interval for μ only"
      - text: "The sample mean and sample standard deviation"
      - text: "The entire joint posterior over (μ, σ) — a distribution of Gaussian distributions"
        correct: true
    explanation: "**p. 79:** “the “estimate” here will be the entire posterior distribution, not any point within it. And as a result, the posterior distribution will be a distribution of Gaussian distributions. Yes, a distribution of distributions.” Each (μ, σ) pair in the posterior describes one possible bell curve of heights. For a latency model, the posterior is a family of possible latency curves, each weighted by plausibility — the honest answer to 'what does our latency distribution look like?'"
  - id: filter-adults
    prompt: "Why does the book filter the Howell data to adults (age ≥ 18) before modeling height as a single Gaussian?"
    options:
      - text: "Because height is strongly correlated with age before adulthood, so mixing ages would violate the single-mean model"
        correct: true
      - text: "Because children's data are inaccurate"
      - text: "Because the Gaussian cannot model small values"
      - text: "To reduce computation time"
    explanation: "**p. 80:** “height is strongly correlated with age, before adulthood.” Including children would mix many different means, and the data would no longer look like one Gaussian. In telemetry terms: do not fit one latency distribution across cached and uncached endpoints; the mixture is not described by one mean. Either split the data or add a predictor — which the next section does."
  - id: iid
    prompt: "The model assumes heights are i.i.d. (independent and identically distributed). How does the book interpret this assumption?"
    options:
      - text: "As a claim about the physical world that must be verified before fitting"
      - text: "As a description of the golem's state of information: without further knowledge, each height is treated as independent with the same distribution, even if real people in families are correlated"
        correct: true
      - text: "As a frequentist assumption that Bayesians do not need"
      - text: "As something that holds only for large samples"
    explanation: "The book argues (p. 81) that i.i.d. is an epistemological assumption — it says what the model knows, not how the world is. Correlations among family members exist, but if the model has no information about families, the i.i.d. assumption is the most conservative description. Adding structure (like the family) is the job of multilevel models in Chapter 13. Programmers can think of it as the default behavior when no grouping key is provided."
  - id: prior-predictive
    prompt: "What is a *prior predictive simulation*?"
    options:
      - text: "Choosing priors from the data"
      - text: "Fitting the model and predicting new data"
      - text: "Simulating observable data from the priors alone, before seeing data, to check what the priors imply about outcomes"
        correct: true
      - text: "A frequentist power analysis"
    explanation: "**p. 82:** “The prior predictive simulation is an essential part of your modeling. Once you’ve chosen priors for h, µ, and σ, these imply a joint prior distribution of individual heights. By simulating from this distribution, you can see what your choices imply about observable height. This helps you diagnose bad choices.” Draw μ and σ from their priors, then draw heights. It is a dry run of your model's assumptions — like running a deployment plan in a staging environment before production."
  - id: bad-prior
    prompt: "With the flat-looking prior `μ ~ Normal(178, 100)`, the prior predictive simulation expects about 4% of people to have negative height and 18% to be taller than the tallest recorded human. What is the lesson?"
    options:
      - text: "Wide priors are always objective and safe"
      - text: "The data are wrong"
      - text: "Gaussian priors should never be used"
      - text: "Priors that look harmlessly vague can imply absurd observable data; prior predictive simulation exposes this, and scientific knowledge should be used to build sensible priors"
        correct: true
    explanation: "**p. 83–84:** “Priors with such large standard deviations are quite common in Bayesian models, but they are hardly ever sensible.” Here there is so much data that the silly prior is harmless, “But that won’t always be the case.” And crucially: “Using scientific knowledge to build priors is not cheating. The important thing is that your prior not be based on the values in the data, but only on what you know about the data before you see it.” For an SLO model, a prior allowing negative latency or 10-hour page loads is not 'neutral'; it is wrong."
  - id: epsilon
    prompt: "Why does the book bid “farewell to epsilon”, avoiding the form `hᵢ = μ + εᵢ, εᵢ ~ Normal(0, σ)`?"
    options:
      - text: "It is equivalent for Gaussian models but does not generalize to non-Gaussian models, so it is better to learn the `~` form that does"
        correct: true
      - text: "It is slower to compute"
      - text: "It is mathematically wrong"
      - text: "Epsilon is reserved for priors"
    explanation: "**p. 84:** “this ϵ form is poor form. The reason is that it does not usually generalize to other types of models. This means it won’t be possible to express non-Gaussian models using tricks like ϵ. Better to learn one system that does generalize.” A count of errors is not 'mean plus Gaussian noise'; it is Poisson or binomial. The `~` form expresses those models just as easily."
  - id: grid-2d
    prompt: "Grid approximation for the height model computes everything on the log scale. Why?"
    options:
      - text: "Because heights are log-normal"
      - text: "Because multiplying the likelihoods of 352 observations underflows to zero; summing log-likelihoods avoids that"
        correct: true
      - text: "Because the prior is on the log scale"
      - text: "Because R requires it"
    explanation: "From the box on p. 84: “Everything will be calculated on the log scale, so multiplication will become addition.” With hundreds of observations each contributing a density around 0.01–0.05, their product is far below `double.Epsilon`. The standard trick: sum log-likelihoods, add log-priors, subtract the maximum, then exponentiate. It is exactly how you would implement this robustly in C#."
  - id: sigma-skew
    prompt: "Why does the posterior for σ tend to have a long right-hand tail, especially with small samples?"
    options:
      - text: "Because σ has a uniform prior"
      - text: "Because the data are skewed"
      - text: "Because variances must be positive, so there is more uncertainty about how big σ could be than about how small"
        correct: true
      - text: "Because the quadratic approximation adds skew"
    explanation: "**p. 86:** “variances must be positive. As a result, there must be more uncertainty about how big the variance (or standard deviation) is than about how small it is.” With a sample of 20 heights, the skew is obvious, and a Gaussian (quadratic) approximation of σ's posterior is poor. Practical lesson: when you care about variability — say, jitter or latency spread — inspect σ's posterior rather than trusting a symmetric ± interval."
  - id: narrow-prior-moves-sigma
    prompt: "Changing only μ's prior to the very narrow `Normal(178, 0.1)` changes the posterior for σ substantially. Why?"
    options:
      - text: "Because σ's prior also changed"
      - text: "It is a bug in quap"
      - text: "Because narrow priors increase all variances"
      - text: "Because the golem is forced to believe μ ≈ 178, so it must explain the data's actual spread around 178 with a larger σ; parameters are estimated jointly"
        correct: true
    explanation: "**p. 89:** “Once the golem is certain that the mean is near 178—as the prior insists—then the golem has to estimate σ conditional on that fact. This results in a different posterior for σ, even though all we changed is prior information about the other parameter.” Parameters are coupled through the likelihood. In engineering: if you pin your capacity model's baseline to a wrong value, the 'noise' term absorbs the error, and your alert thresholds widen in compensation."
  - id: vcov
    prompt: "What does the variance–covariance matrix of a quadratic approximation describe?"
    type: multiple
    options:
      - text: "The variance of each parameter's posterior"
        correct: true
      - text: "How changes in one parameter correlate with changes in another in the posterior"
        correct: true
      - text: "The variance of the data"
      - text: "Together with the means, the entire multivariate Gaussian approximation"
        correct: true
    explanation: "**p. 90:** “a list of means and a matrix of variances and covariances are sufficient to describe a multi-dimensional Gaussian distribution.” It factors into variances and a correlation matrix. For the height model the correlation between μ and σ is near zero — “learning µ tells us nothing about σ” — but in regressions with uncentered predictors, intercept and slope are strongly correlated (next section). Sampling from the approximation draws vectors from this multivariate Gaussian."
  - id: interval-89
    prompt: "Why does the book's `precis` default to 89% intervals?"
    options:
      - text: "Because 89 is prime and, more importantly, avoiding 95% discourages readers from doing unconscious hypothesis tests"
        correct: true
      - text: "Because 89% is the optimal interval mass"
      - text: "Because Stan requires it"
      - text: "Because 89% intervals are always narrower"
    explanation: "**p. 56:** the book suggests nested 67%, 89% and 97% intervals: “Why these values? No reason. They are prime numbers, which makes them easy to remember. But all that matters is they be spaced enough to illustrate the shape of the posterior. And these values avoid 95%, since conventional 95% intervals encourage many readers to conduct unconscious hypothesis tests.” The number is a display choice, like choosing p50/p90/p99 for latency dashboards."
---

The first real Gaussian model: adult heights from the !Kung San census data. Two parameters, μ and σ, and the
first multi-parameter posterior.

## Learning objectives

- Specify likelihood and priors for a Gaussian model
- Run and interpret a prior predictive simulation
- Explain why grid approximation works on the log scale and why it does not scale
- Explain the skew of σ's posterior and the coupling of parameters
- Sample from a multivariate quadratic approximation

## In a nutshell

```text
hᵢ ~ Normal(μ, σ)
μ  ~ Normal(178, 20)
σ  ~ Uniform(0, 50)
```

| Step | What it checks | Engineering analogue |
|---|---|---|
| Prior predictive simulation | Do the priors imply sensible data? | Dry-run in staging |
| Fit with `quap` | Mode and curvature of the posterior | Optimizer + Hessian |
| `vcov`, `cov2cor` | Parameter uncertainty and correlations | Covariance of config knobs |
| `extract.samples` | Vectors of parameter values | Monte Carlo inputs |

## Prior predictive simulation in C#

```csharp
var rng = new Random(1);
double Normal(double mu, double sd) =>
    mu + sd * Math.Sqrt(-2 * Math.Log(1 - rng.NextDouble())) * Math.Cos(2 * Math.PI * rng.NextDouble());
var heights = Enumerable.Range(0, 10_000)
    .Select(_ => Normal(Normal(178, 20), rng.NextDouble() * 50))
    .ToArray();
var negativeShare = heights.Count(h => h < 0) / (double)heights.Length;
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 4.3, pp. 78–91; Figure 4.3.
- Howell N. *Demography of the Dobe !Kung*. Academic Press, 1979; *Life Histories of the Dobe !Kung*, University of California Press, 2010.
- Gabry J et al. Visualization in Bayesian workflow. *JRSS A* 2019 — on prior predictive checks.
