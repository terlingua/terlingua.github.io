---
title: "13.2 Varying Effects and the Underfitting/Overfitting Trade-Off"
questions:
  - id: three-pooling
    prompt: "Match the three pooling strategies to their errors."
    type: multiple
    options:
      - text: "Complete pooling: one intercept for all ponds — precise but underfit for any particular pond"
        correct: true
      - text: "No pooling: separate intercepts with no sharing — overfit, especially for small ponds"
        correct: true
      - text: "Partial pooling: adaptive regularization — less underfit than the grand mean and less overfit than no pooling"
        correct: true
      - text: "No pooling is always best when clusters differ"
    explanation: "**p. 408–409:** “(1) Complete pooling. This means we assume that the population of ponds is invariant ... (2) No pooling. This means we assume that each pond tells us nothing about any other pond. This is the model with amnesia. (3) Partial pooling. This means using an adaptive regularizing prior.” Partial pooling produces “estimates for each cluster that are less underfit than the grand mean and less overfit than the no-pooling estimates.”"
  - id: no-pool-infinite
    prompt: "In terms of the population variance σ, what assumption does the no-pooling model make?"
    options:
      - text: "σ = 0: all ponds are identical"
      - text: "σ is estimated"
      - text: "σ = 1"
      - text: "σ is infinite: nothing learned from one pond helps predict another"
        correct: true
    explanation: "**p. 409:** no-pooling is “like assuming that the variation among ponds is infinite, so nothing you learn from one pond helps you predict another.” Complete pooling assumes σ = 0. Partial pooling estimates σ — so the two classical extremes are special cases of a parameter the multilevel model learns."
  - id: simulation-design
    prompt: "Why does the book simulate tadpole ponds with known true survival before comparing estimates?"
    options:
      - text: "Because with simulated data you know the true per-pond values, so you can measure each method's error directly"
        correct: true
      - text: "Because real tadpoles are unavailable"
      - text: "Because simulation is faster than fitting"
      - text: "To generate p-values"
    explanation: "The simulation sets ᾱ, σ, 60 ponds with 5, 10, 25 or 35 tadpoles, draws true log-odds, then binomial survivors (p. 409–411). With the truth known, the absolute error of no-pooling and partial-pooling estimates can be compared pond by pond (Figure 13.3). Planted-truth testing, again."
  - id: results-error
    prompt: "What does Figure 13.3 show about estimation error?"
    options:
      - text: "No pooling is more accurate everywhere"
      - text: "Partial pooling has lower average error, with the biggest advantage in the smallest ponds; in large ponds the two converge"
        correct: true
      - text: "Both are equally bad"
      - text: "Partial pooling is worse in small ponds"
    explanation: "Caption: “Smaller ponds produce more error, but the partial pooling estimates are better on average, especially in smaller ponds.” **p. 414:** “small ponds are prone to overfitting, and so they receive a bigger dose of the underfit grand mean. Likewise, the larger ponds shrink much less, because they contain more information.” This is the James–Stein phenomenon in Bayesian form."
  - id: not-every-cluster
    prompt: "Does partial pooling give a better estimate for *every* individual cluster?"
    options:
      - text: "Yes, always"
      - text: "No, it is worse on average"
      - text: "No — it is better on average; a particular cluster that truly is extreme can be shrunk too far, but the expected error across clusters is lower"
        correct: true
      - text: "Only for clusters with zero data"
    explanation: "The benefit is in expectation: shrinkage trades a little bias for a large variance reduction. In Figure 13.3 some individual ponds have higher partial-pooling error than no-pooling error; the averages favor partial pooling. A genuinely broken low-traffic service may look healthier than it is after shrinkage — so pair shrunk estimates with alerting on clear absolute signals."
  - id: simulate-again
    prompt: "If you rerun the simulation many times, what pattern does the book say you should expect?"
    options:
      - text: "No pooling wins about half the time"
      - text: "Partial pooling only works with 60 ponds"
      - text: "The results reverse every time"
      - text: "Partial pooling's average advantage is consistent, especially for small clusters, although individual runs vary"
        correct: true
    explanation: "The book encourages re-simulating (p. 414) and notes the advantage is systematic, not a fluke of one seed. Like any benchmark, run it many times before concluding."
  - id: devops-pooling
    prompt: "You rank 300 microservices by p95 latency regression after a platform upgrade. Many services had only a handful of requests during the test window. What does this section recommend?"
    options:
      - text: "Fit a multilevel model with a per-service effect drawn from a fleet distribution, then rank by the shrunk estimates and their uncertainty"
        correct: true
      - text: "Use one fleet-wide estimate for all services"
      - text: "Rank by raw per-service p95 change"
      - text: "Drop services with fewer than 1,000 requests and rank the rest"
    explanation: "Raw per-service changes (no pooling) will put noisy low-traffic services at both ends of the ranking. A single fleet estimate (complete pooling) hides real differences. Partial pooling gives each service an estimate that relies on its own data in proportion to how much it has. Dropping small services throws away data and introduces selection effects."
---

Why partial pooling beats both 'one size fits all' and 'every cluster alone', demonstrated by simulation.

## Learning objectives

- Distinguish complete, no and partial pooling and their implied σ
- Explain why shrinkage helps most for small clusters
- Interpret average versus individual-cluster improvements
- Apply partial pooling to rankings of many small groups

## In a nutshell

| Strategy | Assumed σ | Small cluster estimate | Error |
|---|---|---|---|
| Complete pooling | 0 | Grand mean | Underfit |
| No pooling | ∞ | Raw proportion | Overfit |
| Partial pooling | Estimated | Weighted toward grand mean | Lowest on average |

```text
shrunk estimate ≈ w · raw cluster mean + (1 − w) · population mean,
w grows with cluster sample size and with σ
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 13.2, pp. 408–415; Figure 13.3.
- James W, Stein C. Estimation with quadratic loss. *Proceedings of the Fourth Berkeley Symposium* 1961.
- Gelman A, Hill J. *Data Analysis Using Regression and Multilevel/Hierarchical Models*. Cambridge University Press, 2007, Chapter 12.
