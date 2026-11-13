---
title: "11.3 Multinomial and Categorical Models"
questions:
  - id: k-minus-one
    prompt: "In a multinomial (categorical) GLM with K unordered outcome types, how many linear models do you need?"
    options:
      - text: "One"
      - text: "K − 1, with one outcome chosen as a pivot whose score is fixed (e.g. at 0)"
        correct: true
      - text: "K"
      - text: "K²"
    explanation: "**p. 360:** “in a multinomial (or categorical) GLM, you need K − 1 linear models for K types of events. One of the outcome values is chosen as a “pivot” and the others are modeled relative to it.” In the binomial case there is only one linear model, which is why it is easier. Picking among three CDN providers, four instance types or five error categories are categorical outcomes."
  - id: softmax
    prompt: "What link function turns K scores into K probabilities?"
    options:
      - text: "logit"
      - text: "log"
      - text: "softmax: pₖ = exp(sₖ) / Σⱼ exp(sⱼ)"
        correct: true
      - text: "identity"
    explanation: "**p. 361:** “we’ll use the multinomial logit link, which is called softmax.” It generalizes the logistic: exponentiate each score and normalize. Machine-learning engineers know it as the final layer of a classifier. Adding a constant to all scores changes nothing, which is why one score is pinned (the pivot)."
  - id: two-cases
    prompt: "What are the two basic cases for predictors in categorical models?"
    type: multiple
    options:
      - text: "Predictors whose values differ by outcome type (e.g. each career has its own expected income)"
        correct: true
      - text: "Predictors that are the same for each outcome but whose parameters differ by outcome (e.g. a person's family income with a separate coefficient per career)"
        correct: true
      - text: "Predictors that must be identical across all models"
      - text: "Predictors that are only allowed in the pivot"
    explanation: "**p. 360:** “There are two basic cases: (1) predictors have different values for different values of the outcome, and (2) parameters are distinct for each value of the outcome.” Choosing a cloud region: latency to each region is outcome-specific (case 1); the user's country is individual-level with region-specific coefficients (case 2)."
  - id: hard-to-interpret
    prompt: "In the family-income career model, b[2] came out negative. Does that mean family income reduces the chance of career 2?"
    options:
      - text: "Yes, directly"
      - text: "Yes, because softmax preserves signs"
      - text: "No; the sign is random"
      - text: "Not necessarily — coefficients are relative to the pivot, so their signs are hard to read; compute predictions on the probability scale"
        correct: true
    explanation: "**p. 363:** “computing implied predictions is the safest way to interpret these models ... the parameters are on a scale that is very hard to interpret. In this case, b[2] ended up negative, because it is relative to the pivot, for which family income has a positive effect.” The tide engine again: inspect the predicted probabilities, not individual gears."
  - id: counterfactual-competing
    prompt: "Why is the effect of doubling one career's income on choice probabilities subtle?"
    options:
      - text: "Because the probability change always depends on the other choices available — options compete for the same probability mass"
        correct: true
      - text: "Because incomes cannot double"
      - text: "Because the multinomial ignores income"
      - text: "Because the pivot cannot change"
    explanation: "**p. 362:** “the probability change always depends upon the other choices.” Making one option more attractive takes probability from the others in proportion to how they are distributed. Like adding capacity to one region in a latency-based load balancer: the shift in traffic depends on what the other regions offer."
  - id: poisson-disguise
    prompt: "How can a multinomial model be fit as a set of Poisson regressions?"
    options:
      - text: "It cannot"
      - text: "By modelling each outcome type's count with its own Poisson rate λₖ; the multinomial probabilities are recovered as pₖ = λₖ / Σⱼ λⱼ"
        correct: true
      - text: "By averaging the categories"
      - text: "By using one Poisson for the total only"
    explanation: "**p. 363:** “Another way to fit a multinomial/categorical model is to refactor it into a series of Poisson likelihoods ... It’s principled, because the mathematics justifies it. And it’s commonplace, because it is usually computationally easier.” For UCB admissions, two Poissons (admits and rejects) give p_admit = exp(a₁)/(exp(a₁) + exp(a₂)) ≈ 0.387, the same as the binomial (p. 364–365). Counting events of each type per period and modelling each count is natural for telemetry."
---

More than two unordered outcomes: K − 1 linear models, softmax, and the Poisson trick.

## Learning objectives

- Set up a categorical GLM with a pivot and softmax
- Distinguish outcome-specific predictors from outcome-specific parameters
- Interpret categorical models through predictions
- Fit multinomial outcomes with Poisson likelihoods

## In a nutshell

```text
careerᵢ ~ Categorical(p)
sₖ = αₖ + βₖ xᵢ   for k = 1..K−1;   s_K = 0   (pivot)
p  = softmax(s)

Poisson form:  yₖ ~ Poisson(λₖ),  pₖ = λₖ / Σ λⱼ
```

```csharp
static double[] Softmax(double[] s)
{
    var max = s.Max();
    var e = s.Select(v => Math.Exp(v - max)).ToArray();
    var sum = e.Sum();
    return e.Select(v => v / sum).ToArray();
}
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 11.3, pp. 359–365.
- Baker SG. The multinomial-Poisson transformation. *The Statistician* 1994.
