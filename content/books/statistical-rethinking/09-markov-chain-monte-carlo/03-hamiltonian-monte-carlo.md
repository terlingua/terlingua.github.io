---
title: "9.3 Hamiltonian Monte Carlo"
questions:
  - id: hmc-cost
    prompt: "How does HMC's computational cost compare with Metropolis and Gibbs?"
    options:
      - text: "Each sample is cheaper and you need more of them"
      - text: "HMC is always slower in total"
      - text: "Each sample is more expensive, but proposals are much more efficient, so you need far fewer samples and less total time — especially for complex models"
        correct: true
      - text: "They cost the same"
    explanation: "**p. 271:** “HMC is more computationally costly than Metropolis or Gibbs sampling. But its proposals are also much more efficient. As a result, HMC doesn’t need as many samples to describe the posterior distribution ... as models become more complex—thousands or tens of thousands of parameters—HMC can really outshine other algorithms, because the other algorithms just won’t work.” A classic engineering trade: more work per step, far fewer steps."
  - id: king-monty
    prompt: "In the King Monty metaphor, the king's vehicle is given random momentum and glides along a valley where population density is high at the bottom. What happens?"
    options:
      - text: "The vehicle stays at the valley floor"
      - text: "The vehicle moves randomly without physics"
      - text: "The vehicle visits only the highest-density point"
      - text: "The vehicle slows going uphill (low density) and speeds downhill (high density); stopping after fixed time and recording positions visits locations in proportion to density, with very low autocorrelation"
        correct: true
    explanation: "Figure 9.5 (p. 272): “In the long run, positions are visited in proportion to their population density. The autocorrelation between locations visited is very low under this strategy.” Contrast with King Markov's highly autocorrelated walk. The momentum carries the sampler across the distribution in one trajectory, so consecutive samples are nearly independent."
  - id: physics
    prompt: "What is HMC actually simulating?"
    options:
      - text: "A frictionless particle whose position is the parameter vector, gliding over a surface given by the negative log-posterior"
        correct: true
      - text: "A random walk with Gaussian steps"
      - text: "A queue of samples"
      - text: "A genetic algorithm"
    explanation: "**p. 273:** “This is not another metaphor. HMC really does run a physics simulation, pretending the vector of parameters gives the position of a little frictionless particle. The log-posterior provides a surface for this particle to glide across.” Flat log-posterior regions let the particle glide far; steep ones turn it around quickly. The potential energy is −log posterior; kinetic energy comes from the random momentum."
  - id: hmc-ingredients
    prompt: "Which of these does the raw HMC algorithm in the box need?"
    type: multiple
    options:
      - text: "A function U returning the negative log-probability at the current parameters"
        correct: true
      - text: "A function returning the gradient of U"
        correct: true
      - text: "A step size and a number of leapfrog steps"
        correct: true
      - text: "A starting position"
        correct: true
      - text: "The normalizing constant of the posterior"
    explanation: "**p. 276:** “The HMC algorithm needs five things to go: (1) a function U that returns the negative log-probability of the data at the current position (parameter values), (2) a function grad_U that returns the gradient of the negative log-probability at the current position, (3) a step size epsilon, (4) a count of leapfrog steps L, and (5) a starting position current_q.” Like Metropolis, no normalizing constant is required. Stan computes gradients by automatic differentiation — the same technique deep-learning frameworks use."
  - id: leapfrog
    prompt: "What can go wrong with the step size in the leapfrog simulation?"
    options:
      - text: "Small steps always cause divergences"
      - text: "If the step size is too large, the discrete simulation can overshoot sharp bends in the log-posterior and the approximation breaks down"
        correct: true
      - text: "Step size has no effect"
      - text: "Large steps make samples correlated"
    explanation: "**p. 278:** the path “is only an approximation, because it is a series of linear jumps, not an actual smooth curve. This can have important consequences, if the log-posterior bends sharply and the simulation jumps over a bend.” When energy is not conserved because of such an overshoot, the proposal is rejected; severe cases are flagged as **divergent transitions**. A numerical integrator with too large a timestep — the same failure as an unstable game-physics simulation."
  - id: u-turn
    prompt: "What problem does the No-U-Turn Sampler (NUTS) solve?"
    options:
      - text: "It prevents chains from starting in the same place"
      - text: "It replaces gradients with random walks"
      - text: "It adaptively chooses the number of leapfrog steps by detecting when a trajectory starts turning back toward where it began, avoiding wasted computation and returning to the start"
        correct: true
      - text: "It removes the need for warmup"
    explanation: "With a fixed number of steps, trajectories can loop back near their starting point (Figure 9.6, top-right), producing correlated samples. **p. 274:** “A no-U-turn sampler uses the shape of the posterior to infer when the path is turning around. Then it stops the simulation.” Stan also adapts the step size during warmup. Together these make HMC usable without manual tuning."
  - id: warmup
    prompt: "What happens during Stan's warmup phase?"
    options:
      - text: "It produces the final samples"
      - text: "It checks the data for errors"
      - text: "It compiles the model"
      - text: "It tunes the sampler (e.g. step size) for the posterior at hand; warmup samples are not used for inference"
        correct: true
    explanation: "**p. 274:** “warmup is not like burn-in. Technically, burn-in samples are just samples. They are part of the posterior. But Stan’s warmup phase, for example, does not produce useful samples. It is just tuning the simulation.” Think JIT warmup in a benchmark: you discard early iterations because the system is still adapting."
  - id: hmc-limits
    prompt: "Which are limitations of HMC?"
    type: multiple
    options:
      - text: "It requires continuous parameters, because it needs gradients"
        correct: true
      - text: "It can struggle with posteriors that have sharp curvature changes, signalled by divergent transitions"
        correct: true
      - text: "It cannot handle more than ten parameters"
      - text: "Each sample is more expensive than a Metropolis sample"
        correct: true
    explanation: "HMC needs the gradient of the log-posterior, so discrete parameters must be marginalized out (Chapter 15 does this for discrete missing data). Sharp curvature, such as the 'funnel' in multilevel models, causes divergences; Chapter 13 fixes this by reparameterization. It scales to thousands of parameters, at a higher per-sample cost."
---

HMC replaces blind proposals with physics: momentum and gradients carry the sampler efficiently through
high-dimensional posteriors.

## Learning objectives

- Explain HMC as a physics simulation on the log-posterior surface
- List what HMC needs: U, its gradient, step size, number of steps, start
- Explain leapfrog error, divergences and the no-U-turn idea
- Explain warmup

## In a nutshell

```text
U(q)      = −log posterior(q)                  potential energy
K(p)      = Σ p² / 2                            kinetic energy, p ~ Normal(0, 1)
leapfrog:  p ← p − ε/2 ∇U(q);  q ← q + ε p;  p ← p − ε/2 ∇U(q)   (repeat L times)
accept with probability min(1, exp(H_start − H_end)),  H = U + K
```

| Setting | Too small | Too large |
|---|---|---|
| Step size ε | Slow, many gradient evaluations | Overshoots bends → divergences, rejections |
| Leapfrog steps L | Short trajectories, correlated samples | U-turns, wasted work (NUTS stops early) |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 9.3, pp. 270–279; Figures 9.5–9.6.
- Neal RM. MCMC using Hamiltonian dynamics. In *Handbook of Markov Chain Monte Carlo*, 2011.
- Hoffman MD, Gelman A. The No-U-Turn Sampler. *JMLR* 2014.
- Betancourt M. A conceptual introduction to Hamiltonian Monte Carlo. arXiv:1701.02434, 2017.
