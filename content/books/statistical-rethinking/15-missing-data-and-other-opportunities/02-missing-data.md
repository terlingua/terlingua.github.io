---
title: "15.2 Missing Data"
questions:
  - id: static-values
    prompt: "What does the book say about replacing missing values with a fixed value such as the mean or zero?"
    options:
      - text: "It is never warranted: the model will think it knows those values with certainty"
        correct: true
      - text: "It is the standard best practice"
      - text: "It is fine for small datasets"
      - text: "It is equivalent to imputation"
    explanation: "**p. 499:** “Replacing missing values with static values is never warranted—we do not know those values, and if you fix them, the model will think it knows them with certainty.” Filling missing latency samples with the mean understates variance and fakes precision; filling with zero inserts impossible values."
  - id: missing-meaningful
    prompt: "Why does the book say 'missing data are meaningful data'?"
    options:
      - text: "Because missing values should be deleted"
      - text: "Because the fact that a value is missing is itself an observation whose meaning depends on context — e.g. people may refuse to report income because it is low or high"
        correct: true
      - text: "Because missing values are always zero"
      - text: "Because they increase sample size"
    explanation: "From the box on p. 500: “The fact that a variable has an unobserved value is still an observation. It is data, just with a very special value ... If some people refuse to fill in their income, this may be associated with low (or high) income.” A missing heartbeat from a host is not 'no data'; it may be the most informative signal you have."
  - id: dogs-mcar
    prompt: "In the 'dog ate my homework' DAGs, when are missing values benign for estimating S → H?"
    options:
      - text: "When dogs eat homework from noisy homes"
      - text: "When dogs eat bad homework"
      - text: "When dogs eat homework completely at random (no arrow into D), so H is independent of D and missingness only reduces efficiency"
        correct: true
      - text: "Never"
    explanation: "Scenario (a) (p. 500–501): “H is independent of D (H ⊥⊥ D), because H∗ is a collider ... Since the missing values are completely random, missingness doesn’t necessarily change the overall distribution of homework scores. It removes data, and that makes estimation less efficient.” The book notes these scenarios “sometimes go by the unhelpful names (a) missing completely at random (MCAR), (b) and (c) missing at random (MAR), and (d) the impressively absurd missing not at random (MNAR)” (p. 503). Dropped telemetry from a random UDP packet loss is roughly MCAR."
  - id: dogs-mar
    prompt: "In scenario (b), dogs eat the homework of students who study a lot (S → D). Is the effect of S on H still estimable?"
    options:
      - text: "No, never"
      - text: "Only if H is binary"
      - text: "Only if you drop S"
      - text: "Yes, if you condition on S — missingness depends only on an observed variable, so conditioning closes the path, though imputation can still help efficiency and the functional form must be right"
        correct: true
    explanation: "When missingness is caused by an observed variable that is in the model (often called MAR, missing at random), conditioning on that variable makes H independent of D. The book notes complications from nonlinear relationships, but the key logic is the DAG: is there an open path from H to D after conditioning? If high-traffic hosts drop more metrics and you model traffic, you can recover the relationship."
  - id: dogs-mnar
    prompt: "In scenario (d), dogs prefer to eat bad homework (H → D). What is the consequence?"
    options:
      - text: "Missingness depends on the unobserved value itself; nothing simple fixes it without modeling the missingness mechanism, and the observed data are biased"
        correct: true
      - text: "No problem, missingness is random"
      - text: "Conditioning on S fixes it"
      - text: "It only reduces sample size"
    explanation: "When the outcome itself causes missingness (often called MNAR, missing not at random), observed values are a biased sample, and only a model of the missingness process — with strong assumptions — can help. Error logs that fail to write when the disk is full, or crash reports lost when the crash is severest, are this case."
  - id: dropping-can-help
    prompt: "In scenario (c), noisy homes (X) both lower homework quality and cause dogs to eat homework. Why can dropping the incomplete cases *improve* the estimate in one simulation?"
    options:
      - text: "Because dropping data always helps"
      - text: "Because the missing cases come from noisy houses, which cause omitted-variable bias through X; removing them happens to reduce that bias — but a different missingness rule can make things worse"
        correct: true
      - text: "Because X is observed"
      - text: "Because the model was misspecified"
    explanation: "**p. 503:** “The homework that is missing is from noisy houses. And it is noisy houses that mess up our estimate of bS, through omitted variable bias. So when we delete those houses from the data, the estimate actually gets better.” But: “this improvement is not a general property of missing data in such a DAG ... What happens under missingness depends upon the details of the functions in the full structural causal model.”"
  - id: imputation-idea
    prompt: "What is the key idea behind Bayesian imputation?"
    options:
      - text: "Replace NAs with draws from a uniform distribution"
      - text: "Use the median of observed values"
      - text: "A generative model contains information about unobserved variables: treat each missing value as a parameter with the same distribution the model assigns to the observed values, and infer it jointly"
        correct: true
      - text: "Duplicate the observed rows"
    explanation: "**p. 504:** “The key idea with imputation is that any generative model necessarily contains information about variables that have not been observed. Some data go missing, but the model stays the same.” For neocortex Bᵢ ~ Normal(ν, σB): **p. 506:** “when Bi is observed, then this line is a likelihood ... But when Bi is missing and therefore a parameter, that same line is interpreted as a prior.” One line, two roles."
  - id: imputation-benefit
    prompt: "Compared with complete-case analysis, why does the imputation model give narrower posteriors for the milk effects?"
    options:
      - text: "Because it invents data"
      - text: "Because the priors are tighter"
      - text: "Because imputed values have no uncertainty"
      - text: "Because it keeps the observed body-mass values (and outcomes) of the 12 species that complete-case analysis threw away"
        correct: true
    explanation: "**p. 508:** “The model that imputes the missing values ... has narrower marginal distributions for both effects. How could this happen? We used more information, the values of body mass that are not missing but are discarded” by the complete-case model. Each imputed value carries its own uncertainty, which propagates into the estimates."
  - id: impute-with-covariance
    prompt: "How does the improved imputation model use the relationship between neocortex and body mass?"
    options:
      - text: "It models (M, B) jointly as multivariate normal, so the observed body mass of a species with missing neocortex informs the imputed value"
        correct: true
      - text: "It ignores it"
      - text: "It imputes body mass instead"
      - text: "It sets neocortex equal to body mass"
    explanation: "**p. 510:** with (Mᵢ, Bᵢ) ~ MVNormal((μM, μB), S), “it will measure the correlation between M and B, using the observed cases, and then use that correlation to infer the missing B values.” Figure 15.6 shows imputations that preserve the positive association. Imputing a missing metric from correlated metrics that were observed."
  - id: multiple-imputation
    prompt: "How does the book describe multiple imputation?"
    options:
      - text: "As unrelated to Bayesian methods"
      - text: "As an approximation to full Bayesian imputation under a missing-at-random model, invented when full Bayesian imputation was impractical; full Bayesian imputation is now preferable"
        correct: true
      - text: "As always superior to Bayesian imputation"
      - text: "As a way to avoid modeling missingness"
    explanation: "From the box “Multiple imputations” (p. 511): “it actually has a Bayesian justification. But it was invented when Bayesian imputation on the desktop was impractical ... Multiple imputation is more limited than full Bayesian imputation, so now we just use the real thing.”"
  - id: moralizing-gods
    prompt: "In the Moralizing Gods data, 60% of the key exposure is missing, and missingness is strongly tied to writing. What does the book conclude?"
    options:
      - text: "Complete-case analysis is fine"
      - text: "Replacing NAs with zero is safe"
      - text: "Statistics cannot save this situation; any imputation needs very strong assumptions, complete cases would be biased, and the honest response is more research — and always report how missing data were handled"
        correct: true
      - text: "The hypothesis is proven"
    explanation: "**p. 515:** “This situation cannot be saved by statistics.” Replacing NAs with zero “implies a generative model in which any polity that believes in moralizing gods will never produce a missing value” (p. 516). And the box “Present details about missing data” (p. 516): “the minimum is to report the counts of missing values in each variable and what was done with them.” For postmortems: report how much telemetry was missing and why."
---

Missingness is data too. Think causally about why values are missing, then impute with the model.

## Learning objectives

- Explain why static replacement is never warranted
- Classify missingness scenarios with DAGs (completely random, depends on observed, depends on the missing value)
- Impute missing values as parameters, using relationships among variables
- Recognize when missing data cannot be rescued

## In a nutshell

| Scenario | DAG | Consequence | Usual name |
|---|---|---|---|
| (a) Random dogs | D independent | Lost efficiency only | MCAR |
| (b) Dogs eat studious students' work | S → D | OK if S modelled | MAR |
| (c) Noisy homes | X → D, X → H | Depends on details | MAR |
| (d) Dogs eat bad homework | H → D | Biased; needs missingness model | MNAR |

```text
Kᵢ ~ Normal(μᵢ, σ);   μᵢ = α + βB Bᵢ + βM Mᵢ
(Mᵢ, Bᵢ) ~ MVNormal((μM, μB), S)     B observed → likelihood, missing → parameter
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 15.2, pp. 499–516; Figures 15.4–15.7.
- Rubin DB. Inference and missing data. *Biometrika* 1976.
- Little RJA, Rubin DB. *Statistical Analysis with Missing Data*, 3rd ed. Wiley, 2019.
- Whitehouse H et al. Complex societies precede moralizing gods throughout world history. *Nature* 2019 (later retracted); Beheim B et al., Treatment of missing data determined conclusions regarding moralizing gods, *Nature* 2021.
