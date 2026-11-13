---
title: "9.1 Good King Markov and His Island Kingdom"
questions:
  - id: fortuna-minerva
    prompt: "What does the chapter mean by saying Fortuna (chance) and Minerva (wisdom) became collaborators in the twentieth century?"
    options:
      - text: "Computers removed randomness from science"
      - text: "Scientists stopped using randomness"
      - text: "Chance and wisdom became indistinguishable"
      - text: "Randomness became a tool for understanding: random numbers drive simulation, randomized experiments and sampling algorithms"
        correct: true
    explanation: "**p. 263:** “by the twentieth century, Fortuna and Minerva had become collaborators. Now few of us are bewildered by the notion that an understanding of chance could help us acquire wisdom.” Books of random digits (Figure 9.1) look like madness to earlier centuries but power randomized trials, Monte Carlo simulation and MCMC. Engineers rely on the same idea in chaos engineering, randomized load balancing and fuzzing."
  - id: stan-name
    prompt: "Who is the Stan language named after?"
    options:
      - text: "Stanisław Ulam, credited as one of the inventors of Markov chain Monte Carlo"
        correct: true
      - text: "A Stanford acronym"
      - text: "Standard Analysis Notation"
      - text: "A statistician named Stan Lee"
    explanation: "**p. 264:** “The Stan programming language is not an abbreviation or acronym. Rather, it is named after Stanisław Ulam (1909–1984). Ulam is credited as one of the inventors of Markov chain Monte Carlo.” Monte Carlo methods were born in the Manhattan Project era, among the first serious uses of electronic computers."
  - id: metropolis-steps
    prompt: "King Markov visits islands with this rule: flip a coin to propose a neighbor; if the proposal island has more people, move; if fewer, move with probability (proposal population / current population). What is the long-run result?"
    options:
      - text: "He visits every island equally often"
      - text: "He spends time on each island in proportion to its population"
        correct: true
      - text: "He gets stuck on the biggest island"
      - text: "He visits islands in a fixed order"
    explanation: "Figure 9.2 shows that “the time spent on each island turns out to be proportional to its population size.” This is the Metropolis algorithm. Replace islands with parameter values and population with posterior probability, and you have a sampler: the frequency of visits approximates the posterior. In any single week the king's location looks random (left panel), but the long-run distribution is right (right panel)."
  - id: local-info
    prompt: "What information does King Markov need at each step?"
    options:
      - text: "The populations of all islands"
      - text: "The total population of the kingdom"
      - text: "Only the populations of the current island and the proposal island"
        correct: true
      - text: "A map of the whole archipelago"
    explanation: "**p. 267:** “All he needs to know at any point in time is the population of the current island and the population of the proposal island. Then, without any forward planning or backwards record keeping, King Markov can satisfy his royal obligation to visit his people proportionally.” This is why MCMC works for posteriors: you only need to evaluate the *unnormalized* posterior (prior × likelihood) at two points, and the ratio cancels the intractable normalizing constant — the 'average probability of the data' from Chapter 2."
  - id: why-ratio
    prompt: "Why does the Metropolis algorithm only need the *ratio* of populations (posterior densities)?"
    options:
      - text: "Because ratios are faster to compute than sums"
      - text: "Because the prior is uniform"
      - text: "Because populations are always integers"
      - text: "Because the normalizing constant, which is usually impossible to compute, cancels in the ratio"
        correct: true
    explanation: "Posterior(θ′)/Posterior(θ) = [Likelihood(θ′) × Prior(θ′) / Z] / [Likelihood(θ) × Prior(θ) / Z]; the Z cancels. That integral over all parameter values is what made exact Bayesian inference impractical. MCMC sidesteps it entirely — the same trick as comparing two relative scores without knowing the total."
  - id: markov-property
    prompt: "Why is the king's route a *Markov chain*?"
    options:
      - text: "Because his next location depends only on his current location, not on his history"
        correct: true
      - text: "Because it was invented by Andrey Markov in the twentieth century"
      - text: "Because each week is independent of all others"
      - text: "Because it forms a ring"
    explanation: "A Markov chain is a sequence where the next state depends only on the current state. The king's position this week depends on last week's, so consecutive samples are correlated (autocorrelation), unlike independent draws. The *Monte Carlo* part is the use of randomness. Together, MCMC: a random process with one-step memory designed so its long-run visits match the target distribution. A state machine with probabilistic transitions."
---

Before any math: a king who must visit his islands in proportion to their populations, using only local
information.

## Learning objectives

- Explain the Metropolis algorithm with the island metaphor
- Explain why MCMC needs only relative (unnormalized) posterior values
- Define the Markov property and why samples are autocorrelated

## In a nutshell

```text
loop:
    propose a neighbor θ′ of current θ
    r = posterior(θ′) / posterior(θ)        normalizing constant cancels
    move to θ′ with probability min(1, r)
    record current position
```

## Metropolis in C#

```csharp
var position = 10;
var visits = new int[11];
for (var week = 0; week < 100_000; week++)
{
    visits[position]++;
    var proposal = position + (rng.Next(2) == 0 ? -1 : 1);
    proposal = proposal < 1 ? 10 : proposal > 10 ? 1 : proposal;
    if (rng.NextDouble() < proposal / (double)position) position = proposal;
}
```

Island k has population proportional to k, so `visits[k]` ends up proportional to k.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 9 opening and Section 9.1, pp. 263–267; Figures 9.1–9.2.
- Metropolis N, Rosenbluth AW, Rosenbluth MN, Teller AH, Teller E. Equation of state calculations by fast computing machines. *Journal of Chemical Physics* 1953.
- RAND Corporation. *A Million Random Digits with 100,000 Normal Deviates*. 1955.
