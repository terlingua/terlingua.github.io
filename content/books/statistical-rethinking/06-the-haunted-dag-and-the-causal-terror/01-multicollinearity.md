---
title: "6.1 Multicollinearity (and the Selection-Distortion Effect)"
questions:
  - id: newsworthy-trustworthy
    prompt: "A grant panel ranks 200 proposals by the sum of trustworthiness and newsworthiness (uncorrelated in all proposals) and funds the top 10%. Among funded proposals the two are strongly negatively correlated. Why?"
    options:
      - text: "Newsworthy topics attract worse researchers"
      - text: "Selection on a combination of two criteria induces a negative correlation between them among those selected"
        correct: true
      - text: "The simulation has a bug"
      - text: "Trustworthiness causes low newsworthiness"
    explanation: "**p. 161:** “all that is necessary for such a negative correlation to arise is that peer reviewers care about both newsworthiness and trustworthiness ... the act of selection itself is enough to make the most newsworthy studies the least trustworthy.” Among the selected, a low score on one criterion implies a high score on the other. In the simulation the correlation is −0.77. Hiring shows this too: if you hire on 'algorithms interview + system design interview', among hires the two scores will look negatively correlated even if they are unrelated in the applicant pool. This is the **selection-distortion effect**, and inside a regression it is called collider bias (Section 6.3)."
  - id: regression-oracle
    prompt: "How does the chapter describe regression as an oracle?"
    options:
      - text: "As infallible when the sample is large"
      - text: "As useless"
      - text: "As an oracle, but a cruel one that speaks in riddles and punishes bad questions"
        correct: true
      - text: "As an oracle only for prediction"
    explanation: "**p. 162:** “Regression is indeed an oracle, but a cruel one. It speaks in riddles and delights in punishing us for asking bad questions.” The chapter covers three hazards of adding variables without a causal model — multicollinearity, post-treatment bias and collider bias — and ends with a framework (the backdoor criterion) for deciding what to include. But “this framework does not do the most important step for us: It will not give us a valid model.”"
  - id: multicollinearity-definition
    prompt: "What does multicollinearity mean, and what is its typical symptom?"
    options:
      - text: "Missing data in predictors; estimates are biased"
      - text: "Predictors that are uncorrelated; estimates become too precise"
      - text: "An outcome with many categories; the model fails to fit"
      - text: "A strong association between predictors, conditional on the others in the model; the posterior suggests none is reliably associated with the outcome even if each truly is"
        correct: true
    explanation: "**p. 163:** “Multicollinearity means a very strong association between two or more predictor variables. The raw correlation isn’t what matters. Rather what matters is the association, conditional on the other variables in the model.” The symptom: wide, uncertain coefficients for variables that are each clearly predictive alone. And reassuringly: “there is nothing wrong with multicollinearity. The model will work fine for prediction. You will just be frustrated trying to understand it.”"
  - id: legs
    prompt: "Height is regressed on both left and right leg lengths. Each coefficient is wildly uncertain, yet their sum is precisely estimated near the true value. Why?"
    options:
      - text: "The two legs carry almost the same information, so μ effectively depends only on (βl + βr)x; many combinations give the same predictions, and only the sum is identified"
        correct: true
      - text: "The priors were wrong"
      - text: "The legs measure different things"
      - text: "quap failed to converge"
    explanation: "**p. 165:** “The parameters β1 and β2 cannot be pulled apart, because they never separately influence the mean µ. Only their sum, β1 + β2, influences µ.” The joint posterior is a narrow negatively correlated ridge (Figure 6.2). The model is answering the question you asked: “What is the value of knowing each leg’s length, after already knowing the other leg’s length?” (p. 164). The answer — almost none — is correct. Similar: regressing latency on both 'request bytes' and 'request kilobytes'."
  - id: cannot-answer
    prompt: "What does the book say is the right interpretation of the multicollinear posterior?"
    options:
      - text: "The posterior is wrong and should be discarded"
      - text: "The posterior isn't wrong; it tells you the question you asked cannot be answered with these data — which is a great thing for a model to say"
        correct: true
      - text: "Both variables are unimportant"
      - text: "One leg must be removed randomly"
    explanation: "**p. 166:** “The posterior distribution isn’t wrong, in such cases. It’s telling you that the question you asked cannot be answered with these data. And that’s a great thing for a model to say, that it cannot answer your question.” And: “if you are just interested in prediction, you’ll find that this leg model makes fine predictions.” Like a profiler that cannot attribute time between two functions that are always called together — the total is clear, the split is not identifiable from that trace."
  - id: milk-fat-lactose
    prompt: "Percent fat and percent lactose each strongly predict milk energy alone, but both coefficients shrink and widen when included together. What causal story does the book propose?"
    options:
      - text: "Fat causes lactose"
      - text: "Lactose causes energy and fat is spurious"
      - text: "An unobserved trade-off in milk density D determines both fat and lactose, which then determine energy; fat and lactose are nearly substitutes, forming one axis of variation"
        correct: true
      - text: "Energy causes both fat and lactose"
    explanation: "**p. 169:** “there is a core tradeoff in milk composition that mammal mothers must obey. If a species nurses often, then the milk tends to be watery and low in energy ... If instead a species nurses rarely, in short bouts, then the milk needs to be higher in energy.” The DAG is L ← D → F with both F and L → K, and D unobserved. Knowing D, or having a model that predicts it, “would be better than stumbling through regressions.”"
  - id: drop-correlated
    prompt: "Some fields teach analysts to inspect pairwise correlations and drop highly correlated predictors before fitting. What does the book say?"
    options:
      - text: "It is a good default practice"
      - text: "It is required for Bayesian models"
      - text: "It only works with standardized variables"
      - text: "It is a mistake: pairwise correlations are not the problem; conditional associations matter, and the right action depends on what causes the collinearity"
        correct: true
    explanation: "**p. 169:** “This is a mistake. Pairwise correlations are not the problem. It is the conditional associations—not correlations—that matter. And even then, the right thing to do will depend upon what is causing the collinearity. The associations within the data alone are not enough to decide what to do.” Dropping features by a correlation threshold is common in ML pipelines; it is fine for prediction hygiene but says nothing about causal importance."
  - id: identifiability
    prompt: "In a Bayesian model, what does it mean for a parameter to be weakly identified?"
    options:
      - text: "The data and model do not allow you to learn much about it, though a posterior always exists; the posterior may be close to the prior or a broad ridge"
        correct: true
      - text: "The parameter has no posterior"
      - text: "The parameter cannot have a prior"
      - text: "The parameter is known exactly"
    explanation: "The book notes (p. 169–170) that in a Bayesian model with proper priors a posterior always exists, so “it’s probably better to speak of weakly identified parameters in a Bayesian context.” And: “even when a DAG says a causal effect should be identifiable, it may not be statistically identifiable. We have to work just as hard at the statistics as we do at the design.” Comparing prior and posterior shows how much the data taught the model."
  - id: collinear-sd
    prompt: "In the simulation that adds a predictor correlated with percent fat, how does the posterior standard deviation of the fat slope change as correlation rises toward 1?"
    options:
      - text: "It decreases"
      - text: "It increases sharply, especially at very high correlations — and less so with informative priors"
        correct: true
      - text: "It oscillates"
      - text: "It stays constant"
    explanation: "The box “Simulating collinearity” (p. 170) shows the standard deviation of the slope rising as correlation increases, slowly at first and then steeply near 1. The code uses flat priors, which “does exaggerate the effect of collinear variables. When you use informative priors, the inflation in standard deviation can be much slower.” Regularizing priors are a practical remedy for unstable coefficients, the same reason ridge regression exists."
---

The chapter opens with a puzzle about selection, then starts its tour of what goes wrong when variables are
added to a regression without a causal model.

## Learning objectives

- Explain the selection-distortion effect
- Define multicollinearity and recognize its symptoms
- Explain why only combinations of collinear parameters are identified
- Avoid the pairwise-correlation filtering heuristic

## In a nutshell

| Hazard (this chapter) | Cause | Symptom |
|---|---|---|
| Multicollinearity | Predictors carry the same information | Wide, unstable individual coefficients; good predictions |
| Post-treatment bias | Conditioning on a consequence of treatment | Treatment effect masked (or invented) |
| Collider bias | Conditioning on a common consequence | Spurious associations |

```text
μᵢ = α + βl xᵢ + βr xᵢ  =  α + (βl + βr) xᵢ    → only βl + βr is identified
```

## Selection distortion in C#

```csharp
var rng = new Random(1914);
var proposals = Enumerable.Range(0, 200)
    .Select(_ => (Nw: StandardNormal(rng), Tw: StandardNormal(rng)))
    .ToArray();
var cutoff = proposals.Select(p => p.Nw + p.Tw).OrderDescending().ElementAt(19);
var funded = proposals.Where(p => p.Nw + p.Tw >= cutoff).ToArray();
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 6 opening and Section 6.1, pp. 161–170; Figures 6.1–6.3.
- Berkson J. Limitations of the application of fourfold table analysis to hospital data. *Biometrics Bulletin* 1946 — the original description of selection-induced correlation.
