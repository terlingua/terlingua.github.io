---
title: "9.2 Metropolis Algorithms"
questions:
  - id: metropolis-requirements
    prompt: "In the general Metropolis algorithm, which statements are correct?"
    type: multiple
    options:
      - text: "Islands become parameter values, which can be continuous"
        correct: true
      - text: "Population sizes become posterior probabilities at each parameter value"
        correct: true
      - text: "Weeks become samples from the joint posterior"
        correct: true
      - text: "The proposal distribution must be asymmetric"
    explanation: "Section 9.2 (p. 267) maps the metaphor: islands → parameter values (not necessarily discrete), population → posterior probability, weeks → samples. The plain Metropolis algorithm requires symmetric proposals (as likely to propose A from B as B from A); Metropolis–Hastings generalizes to asymmetric proposals with a correction."
  - id: gibbs
    prompt: "How does Gibbs sampling improve on plain Metropolis?"
    options:
      - text: "By rejecting more proposals"
      - text: "By using conjugate prior–likelihood pairs to make adaptive proposals for each parameter from its analytical conditional posterior"
        correct: true
      - text: "By running more chains"
      - text: "By using gradients"
    explanation: "**p. 267:** “Gibbs sampling is a variant of the Metropolis-Hastings algorithm that uses clever proposals and is therefore more efficient.” **p. 268:** “How Gibbs sampling computes these adaptive proposals depends upon using particular combinations of prior distributions and likelihoods known as conjugate pairs.” It powered BUGS and JAGS. The cost: you are steered toward conjugate priors, which may not be the ones you want."
  - id: correlated-posterior
    prompt: "Figure 9.3 shows Metropolis on a highly correlated 2D posterior. What happens with small and large step sizes?"
    options:
      - text: "Small steps explore efficiently; large steps reject nothing"
      - text: "Both work equally well"
      - text: "Small steps crawl slowly down the narrow valley; large steps move faster but reject most proposals because they land in implausible places"
        correct: true
      - text: "Step size does not matter"
    explanation: "Caption, Figure 9.3 (p. 269): with a small step size “the chain very slowly makes its way down the valley”; with a larger one “the chain moves faster, but it now rejects 70% of the proposals, because they tend to be even sillier. In higher dimensions, it is essentially impossible to tune Metropolis or Gibbs to be efficient.” Like tuning a retry backoff on a narrow ridge of good settings: too timid and you converge slowly, too bold and most attempts fail."
  - id: concentration-of-measure
    prompt: "What is *concentration of measure*, and why does it defeat Metropolis and Gibbs in high dimensions?"
    options:
      - text: "Probability mass concentrates at the mode, so samplers waste time elsewhere"
      - text: "Measures cannot be computed in high dimensions"
      - text: "High-dimensional distributions have no mode"
      - text: "In high dimensions, most probability mass is far from the mode, in a thin shell; samplers that move one parameter at a time or propose blindly rarely stay in that shell"
        correct: true
    explanation: "**p. 268:** concentration of measure is “an awkward name for the amazing fact that most of the probability mass of a high-dimension distribution is always very far from the mode of the distribution.” Figure 9.4 shows that for a 1,000-dimensional Gaussian, samples sit around radial distance ~31 from the mode, almost never near it. Random proposals in such a space almost always step out of the shell into low-probability territory and get rejected. Think of random config search in a 1,000-knob space: almost every random combination is bad."
  - id: less-random
    prompt: "What principle (credited to E. T. Jaynes) does the chapter apply to samplers?"
    options:
      - text: "Whenever there is a randomized way to do something, there is a non-randomized way that delivers better performance but requires more thought"
        correct: true
      - text: "Randomness always improves algorithms"
      - text: "Deterministic algorithms are always worse"
      - text: "Samplers should be as random as possible"
    explanation: "The book cites Jaynes's view that randomized methods can be beaten by smarter, less random ones (p. 270–271): “The Gibbs strategy has limitations, but it gets its improvement over plain Metropolis by being less random, not more. Hamiltonian Monte Carlo ... pushes Jaynes’ principle much further.” HMC uses the posterior's gradient to make informed proposals instead of blind ones."
  - id: write-your-own
    prompt: "Should you write your own MCMC sampler for real analyses?"
    options:
      - text: "Yes, always; packages hide bugs"
      - text: "For learning, yes; for real work, prefer a well-tested package like Stan, because it is easy to introduce subtle biases"
        correct: true
      - text: "No, MCMC should never be implemented"
      - text: "Only in R"
    explanation: "**p. 287:** “You might write your own MCMC code, for the sake of learning. But it is very easy to introduce subtle biases. A package like Stan, in contrast, is continuously tested against expected output.” The same advice as 'do not roll your own crypto': implement it once to understand it, then use the battle-tested library."
---

From the island story to the real algorithm, its smarter Gibbs variant, and why both fail in high dimensions.

## Learning objectives

- Map the island metaphor onto parameters, posterior and samples
- Explain how Gibbs sampling uses conjugacy
- Explain the step-size dilemma on correlated posteriors
- Explain concentration of measure

## In a nutshell

| Algorithm | Proposal | Strength | Weakness |
|---|---|---|---|
| Metropolis | Random symmetric jump | Simple, general | Slow, poorly tuned in high dimensions |
| Metropolis–Hastings | Random, possibly asymmetric | More flexible | Same scaling problems |
| Gibbs | Exact conditional per parameter (conjugate) | Efficient for conjugate models | Restricts priors; struggles with correlation |
| HMC | Physics simulation using gradients | Scales to thousands of parameters | Needs gradients, tuning (automated in Stan) |

## Concentration of measure in C#

```csharp
double RadialDistance(int dims, Random rng) =>
    Math.Sqrt(Enumerable.Range(0, dims).Sum(_ => Math.Pow(StandardNormal(rng), 2)));
```

For 1,000 dimensions this returns values near √1000 ≈ 31.6, never near 0.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 9.2, pp. 267–270; Figures 9.3–9.4.
- Hastings WK. Monte Carlo sampling methods using Markov chains and their applications. *Biometrika* 1970.
- Geman S, Geman D. Stochastic relaxation, Gibbs distributions, and the Bayesian restoration of images. *IEEE TPAMI* 1984.
- Betancourt M. A conceptual introduction to Hamiltonian Monte Carlo. arXiv:1701.02434, 2017.
