---
title: "10.1 Maximum Entropy"
questions:
  - id: tangled-cords
    prompt: "Why do cords in a shaken box end up tangled, in the chapter's opening argument?"
    options:
      - text: "Because of static electricity"
      - text: "Because shaking adds energy"
      - text: "Because cords are designed to tangle"
      - text: "Because there are vastly more ways for cords to be knotted than untangled; betting on the outcome that can happen the most ways is betting on entropy"
        correct: true
    explanation: "**p. 299:** “There are vastly more ways for cords to end up in a knot than for them to remain untied ... We don’t need to know anything about the physics of cords or knots. We just have to bet on entropy. Events that can happen vastly more ways are more likely.” The same reasoning explains why a cluster's config drifts away from the golden state: there are many more drifted states than tidy ones."
  - id: maxent-principle
    prompt: "What is the principle of maximum entropy?"
    options:
      - text: "Choose the distribution that can happen the most ways — the one with the biggest information entropy — among those consistent with your constraints; it is the most conservative choice"
        correct: true
      - text: "Choose the distribution with the most parameters"
      - text: "Choose the distribution with the smallest variance"
      - text: "Choose the distribution that best fits the histogram"
    explanation: "**p. 301:** “The distribution that can happen the most ways is also the distribution with the biggest information entropy. The distribution with the biggest entropy is the most conservative distribution that obeys its constraints.” It spreads probability as evenly as the known constraints allow, adding no unjustified assumptions."
  - id: three-justifications
    prompt: "Which justifications does the chapter give for betting on the maximum entropy distribution?"
    type: multiple
    options:
      - text: "It is the widest, least informative distribution consistent with what we know"
        correct: true
      - text: "Nature tends to produce maximum entropy distributions, because many natural processes shed information"
        correct: true
      - text: "Regardless of why, it tends to work in practice"
        correct: true
      - text: "It guarantees correct predictions"
    explanation: "**p. 299–300:** “First, the distribution with the biggest entropy is the widest and least informative distribution.” Second, natural processes like addition produce maximum entropy distributions. “Third, regardless of why it works, it tends to work ... This is no guarantee that it will work on your problem. But no approach can guarantee that.”"
  - id: pebbles
    prompt: "Ten numbered pebbles are tossed into five buckets. Why is the even distribution (2, 2, 2, 2, 2) the most likely *kind* of arrangement?"
    options:
      - text: "Because each bucket is larger"
      - text: "Because it can be realized by the greatest number of individual pebble arrangements (113,400 ways versus 1 way for all in one bucket)"
        correct: true
      - text: "Because pebbles repel each other"
      - text: "Because the experimenter aims at the middle"
    explanation: "Figure 10.1 (p. 302) counts ways: all ten in one bucket can happen 1 way; the even split 113,400 ways. Each particular sequence of tosses is equally likely, but far more sequences produce the flat distribution. **p. 303:** “Information entropy is a way of counting how many unique arrangements correspond to a distribution.” Log-ways per pebble closely tracks entropy. Hashing keys into shards behaves the same way: even load is the overwhelmingly typical outcome."
  - id: center-of-gravity
    prompt: "Why is betting on maximum entropy often effective, even if the true distribution is not exactly maxent?"
    options:
      - text: "Because the true distribution is always maxent"
      - text: "Because it has the fewest parameters"
      - text: "Because the maximum entropy distribution and those very similar to it account for the vast majority of possible arrangements — it is the center of gravity of plausible distributions"
        correct: true
      - text: "Because it is easiest to compute"
    explanation: "**p. 303:** “the vast majority of unique arrangements of pebbles produce either the maximum entropy distribution or rather a distribution very similar to it. And that is why it’s often effective to bet on maximum entropy: It’s the center of gravity for the highly plausible distributions. Its high plausibility is conditional on our assumptions, of course.”"
  - id: gaussian-maxent
    prompt: "Under what constraint is the Gaussian the maximum entropy distribution?"
    options:
      - text: "A known minimum and maximum"
      - text: "Only two possible outcomes"
      - text: "Positive values only"
      - text: "A continuous outcome on the real line with a given (finite) variance"
        correct: true
    explanation: "Section 10.1.1 shows that among continuous distributions with the same variance, the Gaussian has the highest entropy (Figure 10.2; proof on p. 306–307: “The Gaussian has the highest entropy possible for any continuous distribution with variance σ²”). Flatter shapes would need different variance; any other shape with the same variance is more 'committed'. This is the epistemological justification from Chapter 4 made precise."
  - id: binomial-maxent
    prompt: "Under what constraints is the binomial the maximum entropy distribution?"
    options:
      - text: "Only two unordered outcomes are possible and the expected value is constant across trials"
        correct: true
      - text: "The outcome is continuous with fixed variance"
      - text: "The outcome is a positive real number"
      - text: "Outcomes are ordered categories"
    explanation: "**p. 311:** “If only two un-ordered outcomes are possible and you think the process generating them is invariant in time—so that the expected value remains constant at each combination of predictor values—then the distribution that is most conservative is the binomial.” The simulation with expected value 1.4 blue marbles in two draws finds the highest-entropy distribution is {0.09, 0.21, 0.21, 0.49} — exactly binomial with p = 0.7 (p. 309–310). For pass/fail health checks with a stable failure rate, the binomial is the principled default."
  - id: bayes-is-maxent
    prompt: "How is Bayesian updating related to maximum entropy?"
    options:
      - text: "They are opposites"
      - text: "The posterior has the greatest entropy relative to the prior (smallest cross-entropy) among distributions consistent with the constraints and observed data"
        correct: true
      - text: "Bayesian updating minimizes entropy"
      - text: "They are unrelated"
    explanation: "From the box “Bayesian updating is entropy maximization” (p. 300): “The posterior distribution has the greatest entropy relative to the prior (the smallest cross entropy) among all distributions consistent with the assumed constraints and the observed data.” Updating changes your beliefs as little as possible while respecting new information — a conservative revision."
---

How to choose distributions without guessing: bet on the one that can happen the most ways.

## Learning objectives

- State the maximum entropy principle and its justifications
- Explain entropy as counting arrangements (the pebbles)
- Identify the constraints that make the Gaussian and binomial maxent
- Connect Bayesian updating to entropy maximization

## In a nutshell

| Constraint on the outcome | Maximum entropy distribution |
|---|---|
| Real-valued, finite variance | Gaussian |
| Two outcomes, constant expected value | Binomial |
| Non-negative count, constant expected value, no known maximum | Poisson |
| Non-negative real, constant mean | Exponential |
| Non-negative real, given mean and mean log | Gamma |

```csharp
static double Entropy(double[] p) => -p.Where(x => x > 0).Sum(x => x * Math.Log(x));
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 10 opening and Section 10.1, pp. 299–312; Figures 10.1–10.4.
- Jaynes ET. *Probability Theory: The Logic of Science*. Cambridge University Press, 2003.
- Jaynes ET. Information theory and statistical mechanics. *Physical Review* 1957.
