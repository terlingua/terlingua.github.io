---
title: "14.4 Social Relations as Correlated Varying Effects"
questions:
  - id: raw-correlation
    prompt: "Gifts between 300 household dyads have a raw correlation of 0.24 between directions. Why is this a poor measure of reciprocity?"
    options:
      - text: "Because correlations cannot be computed for counts"
      - text: "Because gifts are not counts"
      - text: "Because 0.24 is too small"
      - text: "Because households appear in many dyads with different general tendencies to give and receive; the raw correlation mixes those household effects with dyad-specific balance"
        correct: true
    explanation: "Each household appears in many dyads (Figure 14.8). A generous household gives to everyone; a needy one receives from many. Those household-level tendencies, not dyad balance, drive much of the raw correlation. The social relations model separates them, like separating a service's overall call volume from the specific relationship between two services."
  - id: srm-structure
    prompt: "In the social relations model, what explains the log rate of gifts from A to B?"
    options:
      - text: "An intercept, A's general giving gA, B's general receiving rB, and a dyad-specific effect dAB"
        correct: true
      - text: "Only an intercept"
      - text: "Only household B's wealth"
      - text: "A random walk"
    explanation: "**p. 463:** “log λAB = α + gA + rB + dAB”, and symmetrically for B → A. Household effects (g, r) get a 2D multivariate normal prior with correlation ρgr; dyad effects (dAB, dBA) get another with correlation ρd. Two custom covariance structures, each with scientific meaning. In service-call telemetry: caller fan-out, callee popularity, and the specific edge."
  - id: give-receive
    prompt: "The posterior correlation between general giving and general receiving is negative (about −0.4). What does that suggest?"
    options:
      - text: "Households that give a lot also receive a lot"
      - text: "Households that give more across all dyads tend to receive less — consistent with need-based giving"
        correct: true
      - text: "Giving and receiving are unrelated"
      - text: "The model is broken"
    explanation: "**p. 465:** “This implies that individuals who give more across all dyads tend to receive less.” Figure 14.9 (left): “The households with the lowest rates of giving have some of the highest rates of receiving. This likely reflects need-based gifts.” Also, σ for giving is larger than for receiving: giving rates vary more."
  - id: dyad-balance
    prompt: "The dyad correlation ρd is strongly positive (about 0.88). What does that mean?"
    options:
      - text: "Dyads are unbalanced"
      - text: "Gifts are random"
      - text: "After accounting for general giving and receiving, if one household in a pair gives more than expected, the other does too — a strong balance or reciprocity within dyads"
        correct: true
      - text: "Dyads have no variation"
    explanation: "**p. 467:** “This implies that pairs of households are balanced—if one household gives less than average (after accounting for generalized giving and receiving), then the other probably gives less as well ... there is an astonishing amount of balance. This could reflect reciprocity, adjusted for overall wealth levels. Or it could reflect types of relationships among households, like kin obligations.”"
  - id: srm-engineering
    prompt: "You have counts of RPC calls between pairs of microservices. Which question could a social relations model answer that raw call counts cannot?"
    options:
      - text: "Total traffic in the system"
      - text: "The latency of each call"
      - text: "Which language each service uses"
      - text: "Whether specific pairs have reciprocal traffic beyond what each service's general fan-out and popularity predict, and whether high-fan-out services are also highly called"
        correct: true
    explanation: "The SRM decomposes dyadic counts into sender effects, receiver effects and pair effects, with correlations at each level. That separates 'service A calls everyone a lot' from 'A and B have a special mutual relationship' — useful for spotting chatty pairs worth co-locating or merging."
---

A custom covariance structure for dyadic data: general giving and receiving, and dyad-specific balance.

## Learning objectives

- Explain why raw dyadic correlations mislead
- Specify a social relations model with household and dyad effects
- Interpret correlations between giving and receiving and within dyads

## In a nutshell

```text
y_AB ~ Poisson(λ_AB),   log λ_AB = α + g_A + r_B + d_AB
y_BA ~ Poisson(λ_BA),   log λ_BA = α + g_B + r_A + d_BA
[g_i, r_i]      ~ MVNormal(0, diag(σg, σr) R_gr diag(σg, σr))
[d_AB, d_BA]    ~ MVNormal(0, σd² R_d)        (shared σd)
```

| Quantity | Posterior | Meaning |
|---|---|---|
| ρgr | ≈ −0.4 | Generous households receive less |
| σg vs σr | 0.83 vs 0.42 | Giving varies more than receiving |
| ρd | ≈ 0.88 | Strong dyadic balance |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 14.4, pp. 462–467; Figures 14.8–14.9.
- Koster JM, Leckie G. Food sharing networks in lowland Nicaragua: an application of the social relations model to count data. *Social Networks* 2014.
- Kenny DA, La Voie L. The social relations model. *Advances in Experimental Social Psychology* 1984.
