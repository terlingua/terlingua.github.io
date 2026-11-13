---
title: "3.2 Sampling to Summarize"
questions:
  - id: three-questions
    prompt: "The book groups common posterior summaries into three kinds of questions. Which are they?"
    options:
      - text: "Mean, median and mode"
      - text: "Bias, variance and noise"
      - text: "Prior, likelihood and posterior"
      - text: "Intervals of defined boundaries, intervals of defined probability mass, and point estimates"
        correct: true
    explanation: "Section 3.2 is organized around these (p. 53): how much posterior probability lies below, above or between specified values (defined boundaries); which range of values contains a given amount of probability (defined mass); and which single value best represents the distribution (point estimates). Each maps onto a simple operation on samples: a filter-and-count, a quantile, or a statistic such as the mean."
  - id: defined-boundary
    prompt: "With 10,000 posterior samples of a conversion rate, how do you compute the posterior probability that the rate is below 0.5?"
    options:
      - text: "Count the samples below 0.5 and divide by 10,000"
        correct: true
      - text: "Use a t-test"
      - text: "It cannot be computed from samples"
      - text: "Compute the mean of the samples and compare it to 0.5"
    explanation: "An interval of defined boundaries is answered by counting: `sum(samples < 0.5) / 1e4` (p. 53–54). In LINQ: `samples.Count(p => p < 0.5) / (double)samples.Length`. This directly answers business questions like 'what is the probability the new flow's conversion rate is above the old one?' — compute the difference for each pair of samples and count how many are positive. No test statistic is needed."
  - id: compatibility-interval
    prompt: "Why does the book call posterior intervals *compatibility intervals* rather than confidence or credible intervals?"
    options:
      - text: "Because they are computed differently from credible intervals"
      - text: "To avoid the unwarranted implications of “confidence” and “credibility”: the interval shows values compatible with the model and data, which may themselves not deserve confidence"
        correct: true
      - text: "Because compatibility intervals are always wider"
      - text: "Because confidence intervals are Bayesian"
    explanation: "**p. 54:** “We’re going to call it a compatibility interval instead, in order to avoid the unwarranted implications of “confidence” and “credibility.” What the interval indicates is a range of parameter values compatible with the model and data. The model and data themselves may not inspire confidence, in which case the interval will not either.” A latency band computed from a sampling bug is compatible with the buggy data; that does not make it trustworthy."
  - id: pi-vs-hpdi
    prompt: "After observing 3 waters in 3 tosses, the posterior is piled up near p = 1. Why can a 50% percentile interval (PI) be misleading here, while the 50% HPDI is not?"
    options:
      - text: "The PI is always wider than the HPDI"
      - text: "The HPDI uses a different posterior"
      - text: "The PI assigns equal mass to each tail, so it excludes the most probable values near p = 1; the HPDI is the narrowest interval with that mass, so it includes them"
        correct: true
      - text: "The PI is not defined for skewed posteriors"
    explanation: "**p. 55:** percentile intervals “assign equal probability mass to each tail” and “do a good job of communicating the shape of a distribution, as long as the distribution isn’t too asymmetrical.” With a skewed posterior, the PI cuts off the peak. **p. 56:** “The HPDI is the narrowest interval containing the specified probability mass.” For symmetric posteriors the two are nearly the same. The book also notes the HPDI is more computationally intensive and suffers from more simulation variance — so prefer whichever communicates the shape honestly, and report both if they differ."
  - id: why-95
    prompt: "What does the book say about the convention of reporting 95% intervals?"
    options:
      - text: "95% has a deep mathematical justification"
      - text: "95% is required for Bayesian intervals"
      - text: "Intervals should always be 99%"
      - text: "It is hard to defend beyond convention; it is also a small-world number that holds only within the model's logic"
        correct: true
    explanation: "**p. 56:** “It is not easy to defend the choice of 95% (5%), outside of pleas to convention.” And **p. 58:** “The 95% is a small world number ... only true in the model’s logical world. So it will never apply exactly to the real or large world.” The book often uses 89% partly to avoid the reflex of treating 95% as a significance test. SLOs have the same arbitrariness: 99.9% is a choice, not a law of nature, and the right number depends on costs."
  - id: interval-not-test
    prompt: "A posterior interval for the difference between two variants includes zero. Which interpretation follows from this section?"
    options:
      - text: "Zero is among the values compatible with the model and data, along with the others in the interval; the interval describes the shape of uncertainty and is not a significance test"
        correct: true
      - text: "The experiment failed"
      - text: "Both variants are identical in every respect"
      - text: "There is definitely no difference"
    explanation: "The book stresses that intervals describe the posterior; they are not decision rules. Whether a difference of 0 or of +2% matters depends on the costs of shipping or not shipping. **p. 61:** “Usually it’s better to communicate as much as you can about the posterior distribution, as well as the data and the model itself, so that others can build upon your work.” Reporting 'the effect is between −0.5% and +3%, most likely about +1%' supports a far better decision than 'not significant'."
  - id: point-estimates
    prompt: "Which point estimate minimizes expected *absolute* loss |d − p|, and which minimizes expected *quadratic* loss (d − p)²?"
    options:
      - text: "Absolute loss: mean; quadratic loss: median"
      - text: "Absolute loss: median; quadratic loss: mean"
        correct: true
      - text: "Both: the mode (MAP)"
      - text: "Both: the mean"
    explanation: "**p. 60:** “The two most common examples are the absolute loss as above, which leads to the median as the point estimate, and the quadratic loss (d − p)², which leads to the posterior mean”. A point estimate is a decision, and the right one depends on the cost of being wrong. If you are paid for absolute error in a forecasting competition, report the median; if squared error, the mean. For symmetric posteriors they coincide."
  - id: map
    prompt: "What is the *maximum a posteriori* (MAP) estimate?"
    options:
      - text: "The posterior mean"
      - text: "The 95% interval midpoint"
      - text: "The parameter value with the highest posterior probability — the mode"
        correct: true
      - text: "The maximum likelihood estimate with a flat prior removed"
    explanation: "The MAP is the posterior mode (p. 58). With a flat prior it equals the maximum likelihood estimate. It is what `quap` finds before approximating curvature. For a skewed posterior (3 waters in 3 tosses) the MAP is 1.0, the median about 0.84 and the mean about 0.80 — three quite different 'best' values from one distribution. That spread is the argument for reporting the distribution rather than any single number."
  - id: asymmetric-loss
    prompt: "You must choose how many servers to provision for Black Friday, using a posterior for peak load. Running out of capacity costs far more than idle servers. What does the book's loss-function reasoning suggest?"
    options:
      - text: "Provision for the posterior mean"
      - text: "Provision for the posterior median"
      - text: "Provision for the MAP"
      - text: "Choose the value that minimizes expected cost under your asymmetric loss, which will lie above the mean or median"
        correct: true
    explanation: "The hurricane example (p. 60–61): when underestimating wind speed costs lives and overestimating costs only inconvenience, “the implied loss function is highly asymmetric” and “the optimal point estimate would tend to be larger than posterior mean or median.” Compute expected cost for each candidate capacity by averaging the cost over posterior samples, then pick the minimum. And the book adds: the real issue is the decision (how many servers), not the estimate itself."
  - id: scientists-loss
    prompt: "Why do research scientists usually *not* think about loss functions, according to the book?"
    options:
      - text: "Because point estimates in research are usually meant to describe the posterior's shape rather than support a particular decision"
        correct: true
      - text: "Because loss functions are only for machine learning"
      - text: "Because Bayesian inference does not allow loss functions"
      - text: "Because the mean is always correct"
    explanation: "**p. 61:** “any point estimate like the mean or MAP that they may report isn’t intended to support any particular decision, but rather to describe the shape of the posterior.” In engineering, by contrast, estimates usually feed decisions — rollbacks, capacity, alert thresholds — so the loss function is real and you should use it. The posterior is the same in both cases; what changes is the summary appropriate for the job."
---

Once you have samples, every question about the posterior becomes a summary of a list of numbers.

## Learning objectives

- Compute probabilities of defined boundaries by counting samples
- Compute and compare percentile intervals and HPDIs
- Explain why the book says compatibility interval and avoids 95% by reflex
- Choose point estimates with loss functions

## In a nutshell

| Question | Sample operation | C# |
|---|---|---|
| P(p < 0.5) | Count below, divide | `s.Count(x => x < 0.5) / (double)s.Length` |
| 80% percentile interval | 10% and 90% quantiles | Sort, index at 0.1n and 0.9n |
| 50% HPDI | Narrowest window containing 50% of sorted samples | Slide a window of 0.5n over sorted samples |
| Point estimate under quadratic loss | Mean | `s.Average()` |
| Point estimate under absolute loss | Median | Sorted middle |

```csharp
(double Low, double High) Hpdi(double[] samples, double mass)
{
    var s = samples.Order().ToArray();
    var width = (int)Math.Ceiling(mass * s.Length);
    var best = Enumerable.Range(0, s.Length - width + 1).MinBy(i => s[i + width - 1] - s[i]);
    return (s[best], s[best + width - 1]);
}
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 3.2, pp. 53–61; Figures 3.2–3.4.
- Amrhein V, Greenland S, McShane B. Scientists rise up against statistical significance. *Nature* 2019. [doi:10.1038/d41586-019-00857-9](https://doi.org/10.1038/d41586-019-00857-9)
