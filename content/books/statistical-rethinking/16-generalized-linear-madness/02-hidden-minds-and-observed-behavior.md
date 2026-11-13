---
title: "16.2 Hidden Minds and Observed Behavior"
questions:
  - id: behavior-vs-strategy
    prompt: "45% of children chose the majority color. Why can't we conclude that 45% follow a majority strategy?"
    options:
      - text: "Because several different strategies (random choice, follow-first) also produce majority choices sometimes; observed behavior is a mixture over hidden strategies"
        correct: true
      - text: "Because the sample is small"
      - text: "Because children lie"
      - text: "Because colors are categorical"
    explanation: "Each strategy implies a probability vector over the three choices; random choice picks the majority color one third of the time, and follow-first picks it whenever the majority was shown first. **p. 535:** “Recall that 45% of the sample chose the majority color. But the posterior distribution is consistent with somewhere between 20% and 30% of children following the majority copying strategy ... This is what hidden state models can do for us—prevent us from confusing behavior with strategy.”"
  - id: five-strategies
    prompt: "Which strategies does the model consider?"
    type: multiple
    options:
      - text: "Follow the majority"
        correct: true
      - text: "Follow the minority, or be a maverick (choose the undemonstrated color)"
        correct: true
      - text: "Random choice"
        correct: true
      - text: "Follow the first demonstrated color"
        correct: true
      - text: "Always choose blue"
    explanation: "**p. 533:** “(1) Follow the Majority ... (2) Follow the Minority ... (3) Maverick ... (4) Random ... (5) Follow First.” “Each strategy entails a vector of three probabilities, one for each choice. For example, Random is [1/3, 1/3, 1/3].” Follow First depends on presentation order."
  - id: mixture-likelihood
    prompt: "How is the probability of a child's choice computed when the strategy is unknown?"
    options:
      - text: "Use only the most likely strategy"
      - text: "Average over strategies: Pr(yᵢ) = Σₛ pₛ Pr(yᵢ | s), with strategy frequencies p given a Dirichlet prior"
        correct: true
      - text: "Assume all children use the majority strategy"
      - text: "Fit five separate models"
    explanation: "**p. 534:** Pr(yᵢ) = Σₛ pₛ Pr(yᵢ | s). “This expression is a mixture ... Sometimes you’ll read that this marginalizes out the unknown strategy.” p ~ Dirichlet(4, 4, 4, 4, 4). Same marginalization as the sneaking cat of Chapter 15."
  - id: state-space-ops
    prompt: "A load balancer's logs show which backend each request went to. You suspect a mix of policies (round-robin, least-connections, sticky sessions) across clients. How does this section help?"
    options:
      - text: "Count the most common backend and call it the policy"
      - text: "Assume one policy for all"
      - text: "Define the choice probabilities each policy implies for each request, then fit a mixture estimating the share of requests governed by each policy"
        correct: true
      - text: "Fit a linear regression of backend ID"
    explanation: "Hidden strategies produce overlapping observable behavior. Writing down what each strategy predicts, then estimating mixing proportions, separates them — preventing you from confusing a behavior frequency with the prevalence of a policy. The model can be extended so strategy probabilities vary by client type, as the book suggests for age or gender (p. 535)."
  - id: extend-strategies
    prompt: "How can the strategy model be extended?"
    options:
      - text: "It cannot be extended"
      - text: "By adding more colors only"
      - text: "By removing the Dirichlet prior"
      - text: "By letting strategy probabilities depend on predictors like age, or by modelling strategies over time — a state space model"
        correct: true
    explanation: "**p. 535:** “This model can be extended to allow the probabilities of each strategy to vary by age, gender, or anything else.” The book notes that models of hidden states evolving over time are state space models, and the same marginalization logic applies (see also p. 521)."
---

Behavior is not strategy. Mixture models over hidden strategies recover what minds might be doing.

## Learning objectives

- Distinguish observed behavior from latent strategy
- Write each strategy as a probability vector over outcomes
- Build and interpret a mixture over strategies with a Dirichlet prior

## In a nutshell

```text
yᵢ ~ Categorical(θᵢ)
θᵢ,ⱼ = Σₛ pₛ Pr(j | s, majority_firstᵢ)
p ~ Dirichlet(4, 4, 4, 4, 4)
```

| Strategy | Pr(unchosen, majority, minority) |
|---|---|
| Majority | (0, 1, 0) |
| Minority | (0, 0, 1) |
| Maverick | (1, 0, 0) |
| Random | (1/3, 1/3, 1/3) |
| Follow first | (0, 1, 0) or (0, 0, 1) by order |

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 16.2, pp. 531–536.
- van Leeuwen EJC et al. The development of human social learning across seven societies. *Nature Communications* 2018.
