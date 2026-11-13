---
title: "16.3 Ordinary Differential Nut Cracking"
questions:
  - id: nut-model
    prompt: "How does the book derive the rate of nut opening as a function of age?"
    options:
      - text: "Body mass grows toward an adult maximum by M(t) = Mmax(1 − exp(−kt)), strength scales with mass, and nut-opening rate increases with strength raised to a power θ > 1"
        correct: true
      - text: "A linear regression on age"
      - text: "A spline over age"
      - text: "A Poisson with constant rate"
    explanation: "Section 16.3 (p. 537–538) builds the model from a growth differential equation dM/dt = k(Mmax − Mt), whose solution is Mt = Mmax(1 − exp(−kt)). Strength is proportional to mass, and rate λ = α(βMmax(1 − exp(−kt)))^θ, with “θ is some exponent greater than 1” for increasing returns to strength. Like deriving throughput from a warm-up curve (JIT, caches) times a nonlinear efficiency."
  - id: threshold
    prompt: "What realistic implication follows from increasing returns to strength (θ > 1)?"
    options:
      - text: "Babies open nuts fastest"
      - text: "There is a threshold below which an individual can barely open any nuts in reasonable time"
        correct: true
      - text: "Nut opening is linear in age"
      - text: "Adults cannot open nuts"
    explanation: "**p. 538:** “A realistic implication of assuming increasing returns to strength is that there will be a threshold below which an individual cannot open a single nut in reasonable time.” Prior predictive curves (Figure 16.4) all start near zero for infants. Systems analog: a service below a minimum memory size thrashes and does nearly no useful work."
  - id: simplify
    prompt: "How does the book reduce the 'soup of parameters' in the nut model?"
    options:
      - text: "By dropping parameters arbitrarily"
      - text: "By using flat priors"
      - text: "By rescaling body mass so Mmax = 1 (units are arbitrary) and combining parameters that are only identified together into one φ"
        correct: true
      - text: "By fitting a GLM instead"
    explanation: "**p. 538:** “we can just rescale body mass Mmax so that it equals 1. This might seem like cheating. But measurement scales are arbitrary.” Products of non-identified parameters collapse into one, leaving λ = φ(1 − exp(−kt))^θ. Meaningful parameters still guide priors: φ, k, θ all Log-Normal because they must be positive (p. 539)."
  - id: exposure-nuts
    prompt: "Bouts differ in duration. How does the model account for this?"
    options:
      - text: "It ignores duration"
      - text: "It drops short bouts"
      - text: "It divides counts by duration and uses a Gaussian"
      - text: "Nuts opened ~ Poisson(λ × duration): the rate per second times exposure time"
        correct: true
    explanation: "The outcome is a count over a bout of known length, so the expected count is rate × seconds — the exposure idea from Chapter 11. Figure 16.5 scales points by duration. Errors per request-second and jobs completed per runner-minute work the same way."
  - id: varying-by-individual
    prompt: "Which parameters might sensibly vary by individual chimpanzee?"
    options:
      - text: "Growth or strength parameters like φ and k, which plausibly differ between individuals; θ is a feature of the physics and should not"
        correct: true
      - text: "All parameters, including θ"
      - text: "None"
      - text: "Only duration"
    explanation: "**p. 541:** “some of the model parameters make sense as varying by individual while others do not. The scaling parameter θ for example is a feature of the physics, not of an individual.” Deciding which parameters get varying effects is a scientific decision — e.g. per-host CPU speed varies, but the algorithm's complexity exponent does not."
---

A developmental model built from growth and physics: parameters with meaning, curves with the right shape.

## Learning objectives

- Derive a nut-opening rate from a growth ODE and returns to strength
- Simplify non-identified parameter combinations by rescaling
- Use exposure for counts over different durations
- Decide which parameters can vary by individual

## In a nutshell

```text
dM/dt = k (Mmax − M)  →  M(t) = Mmax (1 − e^(−k t))
nᵢ ~ Poisson(λᵢ · durationᵢ)
λᵢ = φ (1 − e^(−k ageᵢ))^θ
φ ~ LogNormal(log 1, 0.1);  k ~ LogNormal(log 2, 0.25);  θ ~ LogNormal(log 5, 0.25)
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 16.3, pp. 536–541; Figures 16.3–16.5.
- Boesch C et al. Learning curves and teaching when acquiring nut-cracking in humans and chimpanzees. *Scientific Reports* 2019.
