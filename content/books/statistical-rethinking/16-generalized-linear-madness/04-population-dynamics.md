---
title: "16.4 Population Dynamics"
questions:
  - id: ar-problems
    prompt: "Which problems with autoregressive models does the book identify?"
    type: multiple
    options:
      - text: "Lags beyond one period make little causal sense; something from two periods ago acts only through the state one period ago"
        correct: true
      - text: "If states are measured with error, using observed past values as predictors propagates measurement error"
        correct: true
      - text: "Parameters like the intercept imply nonsense, such as hares appearing when there were none"
        correct: true
      - text: "They cannot be fitted to data"
    explanation: "**p. 543:** “nothing that happened two time periods ago causes the present, except through its influence on the state of the system one time period ago ... It’s pure predictive epicycle.” And: “It isn’t the observed Ht−1 that influences Ht, but rather the real unobserved Ht−1.” The intercept “implies that even when there are no hares ... there can be α hares in the next period.” Forecasting telemetry with lag-7 and lag-24 terms is often epicycles too."
  - id: lotka-volterra
    prompt: "What does the Lotka–Volterra model assume about hares and lynx?"
    options:
      - text: "Populations grow linearly"
      - text: "Hares grow at a per-capita birth rate and die at a rate increasing with the number of lynx; lynx are born at a rate increasing with hares and die at a constant rate"
        correct: true
      - text: "Lynx and hares are independent"
      - text: "Both populations are constant"
    explanation: "dH/dt = H(bH − mH L) and dL/dt = L(bL H − mL) (p. 543–544, 547). “Everything is multiplied by Ht, because if there are no hares, then there are no births or deaths. Reproduction and death are per capita processes.” The model produces cycles (Figure 16.7): “lynx eat hares. Once the hares are eaten, the lynx begin to die off. Then the cycle repeats.” Autoscalers and load interact similarly and can oscillate."
  - id: numerical-solution
    prompt: "The Lotka–Volterra equations have no explicit solution. How are they used in the statistical model?"
    options:
      - text: "They are approximated by a linear model"
      - text: "They are ignored"
      - text: "They are solved numerically inside the model (e.g. Stan's integrate_ode_rk45), turning parameters and initial populations into population trajectories"
        correct: true
      - text: "By guessing the populations"
    explanation: "**p. 544:** “These equations have no explicit solution ... So how do we use them? We solve them numerically, through simulation.” In Stan, the ODE solver runs in the transformed parameters block; the population trajectories are deterministic functions of birth/death rates and initial sizes (p. 548). The likelihood connects trajectories to observed pelts."
  - id: observation-model
    prompt: "Why does the model have a separate observation model linking true populations to pelt counts?"
    options:
      - text: "Because pelts are populations"
      - text: "For computational speed"
      - text: "Because Stan requires two likelihoods"
      - text: "Because pelts are an imperfect, variable fraction of the true population (trapping probability varies by year), so observed counts are noisy emissions of an unseen state"
        correct: true
    explanation: "Section 16.4.2 (p. 545–546): a varying trapping probability pₜ makes pelts a skewed, noisy function of true population (Figure 16.8), approximated with a Log-Normal: hₜ ~ LogNormal(log(p Hₜ), σH). Because p cannot be estimated well, the book fixes it with a strong prior and notes (p. 546) that “the model has forced us to realize that we cannot do any better than relative population estimates, unless we have a good way to know p.” This separates *process* (dynamics) from *measurement* (trapping), as in Chapter 15. Monitoring data are emissions of the system's state, not the state itself."
  - id: smooth-vs-jagged
    prompt: "Why are posterior pelt predictions jagged but posterior population trajectories smooth (Figure 16.9)?"
    options:
      - text: "Because measurement error is independent from year to year, while the underlying ODE dynamics are deterministic and smooth"
        correct: true
      - text: "Because of a plotting bug"
      - text: "Because hares are smoother than lynx"
      - text: "Because the populations were smoothed after fitting"
    explanation: "**p. 549:** “The jagged path is caused by uncorrelated measurement error.” And: “This is an example of why it is almost always a mistake to model a time series as if observed data cause observed data in the next time step ... if there is measurement error, and there always is, the data are emissions of some unseen state.” Separate the latent state from noisy observations, as a Kalman filter does."
  - id: glm-as-decision
    prompt: "What is the chapter's final message about GLMs?"
    options:
      - text: "Never use GLMs"
      - text: "Use of a GLM should be a decision, not an obligation; when science provides structural models, they give better fit and better inference"
        correct: true
      - text: "GLMs are always better"
      - text: "Only ODEs are valid models"
    explanation: "**p. 550:** “The goal was not to persuade you to never use a generalized linear model. But hopefully it inspires you to see the use of a GLM as a decision in itself, not an obligation.” For engineers: before fitting a generic regression to system metrics, ask whether queueing theory, a control-loop model or a capacity formula already tells you the shape."
  - id: lynx-doubts
    prompt: "Does the book claim the Lotka–Volterra model fully explains lynx and hare dynamics?"
    options:
      - text: "Yes, completely"
      - text: "Yes, because the fit is perfect"
      - text: "No — there are good reasons to doubt it (real ecologies are complicated); it is a stupidly simple model that still captures cycles and teaches how to fit ODEs to data"
        correct: true
      - text: "No, because ODEs cannot be fitted"
    explanation: "**p. 550:** “There are good reasons to doubt that this model is a good explanation of the population dynamics of hares and lynx.” And p. 544: the model suggests “that, while nature is more complicated, all that is necessary to see cyclical population dynamics is captured in a stupidly simple model.” Simple mechanistic models are valuable as baselines whose failures are interpretable."
---

Time series as hidden dynamic states observed with error, using differential equations instead of
autoregression.

## Learning objectives

- Critique autoregressive models causally
- Write a predator–prey ODE system and solve it numerically
- Separate process and observation models
- Treat GLMs as one choice among many

## In a nutshell

```text
dH/dt = H (bH − mH L)          hares
dL/dt = L (bL H − mL)          lynx
hₜ ~ LogNormal(log(pH Hₜ), σH)  observed hare pelts
lₜ ~ LogNormal(log(pL Lₜ), σL)  observed lynx pelts
H₁, L₁ ~ LogNormal(log 10, 1)
```

```csharp
(double H, double L) Step(double h, double l, double bH, double mH, double bL, double mL, double dt) =>
    (h + dt * h * (bH - mH * l), l + dt * l * (bL * h - mL));
```

(Euler steps for intuition; real fits use adaptive solvers like RK45.)

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 16.4, pp. 541–550; Figures 16.6–16.9.
- Lotka AJ. *Elements of Physical Biology*. 1925; Volterra V. Fluctuations in the abundance of a species considered mathematically. *Nature* 1926.
- Carpenter B et al. Stan: a probabilistic programming language. *Journal of Statistical Software* 2017.
