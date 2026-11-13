---
title: "9.4 Easy HMC: ulam"
questions:
  - id: ulam-prep
    prompt: "What two preparation steps does the book recommend before fitting with `ulam`?"
    type: multiple
    options:
      - text: "Preprocess all variable transformations (logs, squares) before fitting"
        correct: true
      - text: "Build a trimmed data list containing only the variables the model uses"
        correct: true
      - text: "Remove all priors"
      - text: "Convert all variables to integers"
    explanation: "**p. 280:** “(1) Preprocess all variable transformations ... It’s a waste of computing power to do these transformations repeatedly in every step of the Markov chain.” And (2) build a clean data list with just the needed variables, because Stan is strict about types and missing values. Same as hoisting invariant work out of a hot loop and passing a minimal, typed DTO to a service."
  - id: ulam-translate
    prompt: "What does `ulam` actually do?"
    options:
      - text: "It translates the formula list into Stan code; Stan compiles the model and runs the sampler"
        correct: true
      - text: "It implements its own HMC sampler in R"
      - text: "It approximates the posterior with a Gaussian"
      - text: "It fits models by least squares"
    explanation: "**p. 281:** “All that ulam does is translate the formula above into a Stan model, and then Stan defines the sampler and does the hard part.” `stancode(m9.1)` shows the generated program. The book calls tools like ulam “bridges” (p. 287): learning raw Stan frees you from R or any specific scripting language. Stan has interfaces for Python (CmdStanPy), Julia and the command line."
  - id: n-eff
    prompt: "What is `n_eff` in the precis output?"
    options:
      - text: "The number of raw samples drawn"
      - text: "A crude estimate of the number of independent samples the chain is worth, accounting for autocorrelation"
        correct: true
      - text: "The number of parameters"
      - text: "The number of chains"
    explanation: "**p. 281:** “n_eff is a crude estimate of the number of independent samples you managed to get.” Autocorrelated samples carry less information than independent ones. n_eff can even exceed the raw number of samples when HMC produces anti-correlated draws (p. 282). It is the statistical analogue of 'effective throughput' versus raw throughput."
  - id: rhat
    prompt: "What does R̂ (Rhat) indicate, and what value do you want?"
    options:
      - text: "The posterior mean; it should be near zero"
      - text: "The correlation between parameters; it should be large"
      - text: "The convergence of the chains to the target distribution; it should approach 1.00 from above when all is well"
        correct: true
      - text: "The number of divergent transitions; it should be 0"
    explanation: "**p. 281:** “Rhat (R̂) is an indicator of the convergence of the Markov chains to the target distribution. It should approach 1.00 from above, when all is well.” It compares variance between chains with variance within chains; if chains disagree, R̂ > 1. The book's “4” suffix means a newer (rank-normalized) version. Values above about 1.01 deserve a look."
  - id: two-hundred
    prompt: "Roughly how many effective samples does the book say are usually enough for a good approximation of the posterior mean?"
    options:
      - text: "About 10"
      - text: "Exactly 1,000"
      - text: "At least 100,000"
      - text: "About 200"
        correct: true
    explanation: "**p. 281:** “as few as 200 effective samples is usually plenty for a good approximation of the posterior.” For tail quantities (95% intervals) you need more, and Stan may warn about “tail ESS” (p. 288). Decide based on what you need to estimate, as you would choose a sample size for a p99 latency estimate versus a mean."
  - id: chains-parallel
    prompt: "Why run multiple chains, and can they run in parallel?"
    options:
      - text: "Multiple chains with different starting points check that they all converge to the same region; they can run in parallel on separate cores"
        correct: true
      - text: "Multiple chains are required to compute the mean"
      - text: "Chains must run sequentially because each depends on the last"
      - text: "Multiple chains always give more accurate answers than one long chain"
    explanation: "Chains are independent processes, so `cores = 4` runs four chains at once (p. 281–282). **p. 289:** “When you run multiple Markov chains, each with different starting positions, and see that all of them end up in the same region of parameter space, it provides a check that the machine is working correctly.” After you have verified sampling works, one long chain is fine. Embarrassingly parallel, like independent load-test agents."
  - id: trace-plot
    prompt: "In a trace plot, which three properties indicate a healthy chain?"
    type: multiple
    options:
      - text: "Stationarity: the chain stays in the same high-probability region throughout"
        correct: true
      - text: "Good mixing: it rapidly zig-zags across the region rather than wandering slowly"
        correct: true
      - text: "Convergence: multiple independent chains stick to the same region"
        correct: true
      - text: "A steady upward trend"
    explanation: "**p. 284:** “Typically we look for three things in these trace plots: (1) stationarity, (2) good mixing, and (3) convergence.” A trend means non-stationarity. Trace plots are the time-series dashboard of your sampler: a healthy one looks like stable noise around a constant level."
  - id: trank
    prompt: "What is a *trank* (trace rank) plot?"
    options:
      - text: "A trace plot sorted by value"
      - text: "Histograms, one per chain, of the ranks of each chain's samples among all samples; healthy chains give overlapping, flat histograms"
        correct: true
      - text: "A plot of n_eff by parameter"
      - text: "A ranking of models by WAIC"
    explanation: "**p. 284–285:** rank all samples of a parameter across chains, then draw a histogram of ranks per chain. If chains explore the same distribution, each chain's ranks are uniform and the histograms overlap. A chain that is persistently above or below the others shows up immediately. Easier to read than overlapping trace lines."
  - id: stan-blocks
    prompt: "In raw Stan code generated by ulam, what are the data, parameters and model blocks for?"
    options:
      - text: "They are all optional comments"
      - text: "data: priors; parameters: likelihood; model: plotting"
      - text: "data: declare observed variables and types; parameters: declare unknowns (with constraints like lower=0); model: priors, linear model and likelihood"
        correct: true
      - text: "data: generated quantities; parameters: data; model: output"
    explanation: "From the box “Raw Stan model code” (p. 285–287): the data block declares names, types and sizes (`int cid[170]`); the parameters block declares parameters such as `real<lower=0> sigma`; the model block computes μ and states priors and likelihood. Stan is statically typed and compiled — a familiar world for C# developers."
---

The rethinking package's `ulam` compiles your formula list into Stan, then runs HMC.

## Learning objectives

- Prepare data and transformations for `ulam`
- Read n_eff and R̂
- Run and interpret multiple chains
- Read trace and trank plots and the generated Stan code

## In a nutshell

| Diagnostic | Healthy | Warning sign |
|---|---|---|
| n_eff | Hundreds or more | Tiny relative to samples |
| R̂ | 1.00 | > 1.01 |
| Trace plot | Stable, fast zig-zag, chains overlap | Trends, long excursions, chains apart |
| Trank plot | Flat, overlapping histograms | One chain persistently high or low |

```stan
data { vector[170] log_gdp_std; vector[170] rugged_std; int cid[170]; }
parameters { vector[2] a; vector[2] b; real<lower=0> sigma; }
model {
  vector[170] mu;
  sigma ~ exponential(1); b ~ normal(0, 0.3); a ~ normal(1, 0.1);
  for (i in 1:170) mu[i] = a[cid[i]] + b[cid[i]] * (rugged_std[i] - 0.215);
  log_gdp_std ~ normal(mu, sigma);
}
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 9.4, pp. 279–287; Figures 9.7–9.8.
- Vehtari A et al. Rank-normalization, folding, and localization: an improved R̂ for assessing convergence of MCMC. *Bayesian Analysis* 2021. [doi:10.1214/20-BA1221](https://doi.org/10.1214/20-BA1221)
- Stan Development Team. *Stan User's Guide*. [mc-stan.org](https://mc-stan.org/)
