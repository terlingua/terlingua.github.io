---
title: "7.2 Entropy and Accuracy"
questions:
  - id: weatherperson
    prompt: "A forecaster predicts rain with 100% on 3 rainy days and 60% on 7 sunny days. A newcomer always predicts 0% rain. By hit rate the newcomer wins (0.70 vs 0.58). Which measure exposes the newcomer, and why?"
    options:
      - text: "The joint probability of the observed sequence (the likelihood): the newcomer assigns zero probability to the rainy days, so his joint probability is zero"
        correct: true
      - text: "Mean squared error"
      - text: "Hit rate, if computed over more days"
      - text: "The median prediction"
    explanation: "**p. 204:** the forecaster's probability of the exact sequence is about 0.005, but “For the newcomer, it’s 0³ × 1⁷ = 0.” Joint probability “is the same thing as the joint likelihood” and “It’s the unique measure that correctly counts up the relative number of ways each event (sequence of rain and shine) could happen.” A monitoring system that always predicts 'healthy' has a great hit rate and is useless; log-likelihood scoring punishes it severely for the outages it called impossible."
  - id: costs-benefits
    prompt: "Why might the forecaster also win on costs and benefits?"
    options:
      - text: "Because costs never matter"
      - text: "Because getting caught in rain without an umbrella costs more than carrying one unnecessarily, so a forecast that never predicts rain is costly even with a good hit rate"
        correct: true
      - text: "Because the forecaster is cheaper to employ"
      - text: "Because rain is rare"
    explanation: "Section 7.2.1.1 (p. 203–204) assigns a higher cost to being caught in rain than to carrying an umbrella unnecessarily; the newcomer then does worse. The two dimensions of evaluating predictions — costs and benefits, and accuracy in context — are separate. For an alert, missing a real outage usually costs far more than a false page, so 'accuracy' alone is the wrong objective."
  - id: entropy-def
    prompt: "What is information entropy H(p) = −Σ pᵢ log pᵢ?"
    options:
      - text: "The variance of a distribution"
      - text: "The probability of the most likely event"
      - text: "The uncertainty in a probability distribution — the average log-probability of an event, negated"
        correct: true
      - text: "The number of events"
    explanation: "**p. 206:** “The uncertainty contained in a probability distribution is the average log-probability of an event.” It is the unique measure that is continuous, increases with the number of possible events, and is additive across independent events. A fair coin has entropy log 2 ≈ 0.69 nats; a coin that lands heads 99% of the time has about 0.056. In engineering: a cache key distribution with high entropy (many equally likely keys) is hard to cache; low entropy means a few hot keys."
  - id: entropy-props
    prompt: "Which are among the three desiderata that uniquely determine information entropy?"
    type: multiple
    options:
      - text: "The measure should be continuous"
        correct: true
      - text: "It should increase as the number of possible events increases"
        correct: true
      - text: "It should be additive over independent events"
        correct: true
      - text: "It should always be between 0 and 1"
    explanation: "The book lists continuity, increase with the number of possible kinds of events, and additivity (p. 205–206), then notes: “There is only one function that satisfies these desiderata.” Entropy is unbounded above (it grows with the number of events) and measured in nats with natural logs or bits with log base 2; “As long as all of the entropies you compare use the same base, you’ll be fine.”"
  - id: zero-log-zero
    prompt: "How do you handle events with probability 0 when computing entropy?"
    options:
      - text: "Add 1 to every probability"
      - text: "Replace 0 with a tiny number"
      - text: "The entropy is undefined"
      - text: "Assume 0 log 0 = 0; events that never happen drop out"
        correct: true
    explanation: "**p. 207:** by L’Hôpital’s rule, pᵢ log(pᵢ) → 0 as pᵢ → 0. “So just assume that 0 log(0) = 0, when you compute H. In other words, events that never happen drop out.” In code: `p.Where(x => x > 0).Sum(x => -x * Math.Log(x))`."
  - id: kl-divergence
    prompt: "What does the KL divergence D_KL(p, q) measure?"
    options:
      - text: "The additional uncertainty induced by using distribution q to describe events that actually follow p"
        correct: true
      - text: "The distance between two means"
      - text: "The probability that p equals q"
      - text: "The correlation between p and q"
    explanation: "The book defines divergence as “the additional uncertainty induced by using probabilities from one distribution to describe another distribution” (p. 207). D_KL(p, q) = Σ pᵢ (log pᵢ − log qᵢ), which equals cross-entropy H(p, q) minus entropy H(p). It is zero only when q = p (Figure 7.5). It is the target the rest of the chapter tries to estimate: how far a model's predictions are from the truth."
  - id: kl-asymmetry
    prompt: "Using Earth's water/land mix to predict Mars gives a smaller divergence than using Mars to predict Earth. Why?"
    options:
      - text: "It is a numerical bug"
      - text: "Mars has very little water, so a Mars-based model is extremely surprised by Earth's water; Earth's model expects both water and land, so it is less surprised on Mars"
        correct: true
      - text: "Mars is closer to the Sun"
      - text: "Divergence is symmetric, so they are equal"
    explanation: "**p. 209:** “Divergence behaves this way as a feature, not a bug ... if we use a distribution with high entropy to approximate an unknown true distribution of events, we will reduce the distance to the truth and therefore the error.” A forecasting model that allows for a wide range of outcomes is rarely shocked; a narrow, overconfident one can be catastrophically surprised. This motivates maximum entropy distributions in Chapter 10."
  - id: relative-only
    prompt: "We never know the true distribution p. How can we still compare models' divergences?"
    options:
      - text: "We cannot compare them at all"
      - text: "By assuming p is uniform"
      - text: "By comparing their average log-probability of the observed data, because the unknown p terms cancel in the difference; only differences are meaningful"
        correct: true
      - text: "By using R² instead"
    explanation: "**p. 210:** comparing models q and r, “the absolute magnitude of these values will not be interpretable”; only the difference between the models' average log-probabilities “informs us about the divergence of each model from the target p.” The log-probability score S(q) = Σ log qᵢ is “the gold standard way to compare the predictive accuracy of different models.” Like comparing two services' latencies under the same load: the absolute number depends on the harness, but the difference is informative."
  - id: lppd
    prompt: "What is the lppd (log-pointwise-predictive-density), and why use the whole posterior?"
    options:
      - text: "The log of the posterior mean; using samples is optional"
      - text: "The prior predictive density"
      - text: "The average of log-probabilities over samples"
      - text: "For each observation, the log of the average probability across posterior samples, summed over observations; using only a point estimate throws away information"
        correct: true
    explanation: "**p. 210:** “To compute this score for a Bayesian model, we have to use the entire posterior distribution. Otherwise, vengeful angels will descend upon you.” lppd = Σᵢ log((1/S) Σₛ p(yᵢ | θₛ)). Average the *probabilities* across samples, then take the log — not the average of logs. In code, use log-sum-exp for stability: `LogSumExp(logp[i]) - Math.Log(S)`."
  - id: deviance
    prompt: "What is *deviance* in this chapter?"
    options:
      - text: "The lppd multiplied by −2, so smaller values are better"
        correct: true
      - text: "The variance of residuals"
      - text: "The number of parameters"
      - text: "The KL divergence itself"
    explanation: "**p. 210:** “It is also quite common to see something called the deviance, which is like a lppd score, but multiplied by −2 so that smaller values are better. The 2 is there for historical reasons.” WAIC, PSIS and cross-validation are all reported on this deviance scale."
  - id: train-test
    prompt: "In the train–test simulation (Figure 7.6), training deviance always improves with more parameters. What happens to test deviance?"
    options:
      - text: "It also always improves"
      - text: "It is smallest on average near the data-generating model (3 parameters) and worsens as extra parameters fit noise; individual train/test pairs can still reverse the pattern"
        correct: true
      - text: "It always gets worse"
      - text: "It is unrelated to the number of parameters"
    explanation: "**p. 213:** “While the training deviance always gets better with an additional parameter, the test deviance is smallest on average for 3 parameters, which is the data-generating model in this case.” But: “any individual pair of train and test samples may reverse the expectation ... Like all of statistical inference, there are no guarantees here.” A model that wins a single offline evaluation may lose in production by chance."
  - id: maxent-bayes
    prompt: "What surprising connection does the box “The benefits of maximizing uncertainty” state?"
    options:
      - text: "Maximum entropy is unrelated to Bayes"
      - text: "Maximum entropy always gives the uniform distribution"
      - text: "Bayesian updating is entropy maximization: maximizing entropy with prior knowledge as a constraint yields the posterior"
        correct: true
      - text: "Entropy should always be minimized"
    explanation: "**p. 207:** “Maximum entropy is a family of techniques for finding probability distributions that are most consistent with states of knowledge ... If you do this, you actually end up with the posterior distribution. So Bayesian updating is entropy maximization.” Chapter 10 uses this to motivate likelihood choices."
---

To steer between overfitting and underfitting you need a target. Information theory provides one.

## Learning objectives

- Explain why joint probability (likelihood) is the right accuracy measure
- Define entropy, cross-entropy and KL divergence, and explain divergence asymmetry
- Explain why only differences in log-scores are meaningful
- Compute lppd and deviance, and interpret train–test deviance

## In a nutshell

```text
Entropy        H(p)      = −Σ pᵢ log pᵢ
Cross-entropy  H(p, q)   = −Σ pᵢ log qᵢ
Divergence     D_KL(p,q) = H(p, q) − H(p) = Σ pᵢ log(pᵢ / qᵢ)
lppd           = Σᵢ log( (1/S) Σₛ p(yᵢ | θₛ) )
Deviance       = −2 × lppd
```

```csharp
static double LogSumExp(double[] x) { var m = x.Max(); return m + Math.Log(x.Sum(v => Math.Exp(v - m))); }
static double Lppd(double[][] logp) => logp.Sum(row => LogSumExp(row) - Math.Log(row.Length));
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 7.2, pp. 202–214; Figures 7.5–7.6.
- Shannon CE. A mathematical theory of communication. *Bell System Technical Journal* 1948.
- Kullback S, Leibler RA. On information and sufficiency. *Annals of Mathematical Statistics* 1951.
