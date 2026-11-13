---
title: "5.1 Spurious Association"
questions:
  - id: waffles
    prompt: "Waffle House density is associated with divorce rate across US states. What is the book's explanation?"
    options:
      - text: "Divorced people eat more waffles"
      - text: "Waffle Houses cause stress, which causes divorce"
      - text: "Both are associated with the South for historical reasons; the correlation is spurious, created by a shared cause"
        correct: true
      - text: "The correlation is a statistical error"
    explanation: "**p. 123:** Waffle House began in Georgia and spread across the South; “the Southern United States has some of the highest divorce rates in the nation. So it’s probably just an accident of history that Waffle House and high divorce rates both occur in the South.” And: “In large data sets, every pair of variables has a statistically discernible non-zero correlation.” In telemetry with thousands of metrics, you will always find some metric correlated with your incident — often through a shared driver like time of day or deploy cadence."
  - id: reasons-multiple-regression
    prompt: "Which reasons for multiple regression does the chapter list?"
    type: multiple
    options:
      - text: "Statistical “control” for confounds"
        correct: true
      - text: "Multiple and complex causation"
        correct: true
      - text: "Interactions, where the importance of one variable depends on another"
        correct: true
      - text: "To guarantee causal estimates"
    explanation: "**p. 123–124:** “(1) Statistical “control” for confounds. A confound is something that misleads us about a causal influence ... (2) Multiple and complex causation ... since one cause can hide another, they must be measured simultaneously. (3) Interactions. The importance of one variable may depend upon another.” But the chapter warns: “multiple regression can be worse than useless, if we don’t know how to use it. Just adding variables to a model can do a lot of damage.” No method guarantees causal estimates."
  - id: confound-both-ways
    prompt: "Can confounds only create false associations?"
    options:
      - text: "Yes, confounds only make unimportant variables look important"
      - text: "Yes, confounds only affect experiments"
      - text: "No — they only reverse the sign of effects"
      - text: "No — they can hide important effects just as easily as they produce false ones"
        correct: true
    explanation: "**p. 123:** confounds “can hide important effects just as easily as they can produce false ones.” Section 5.2 shows a masking example. A real performance regression can be hidden in aggregate latency if the same release also shifted traffic toward a faster endpoint."
  - id: dag-definition
    prompt: "What does each word in *directed acyclic graph* mean?"
    options:
      - text: "Graph: nodes and connections; directed: arrows indicate the direction of causal influence; acyclic: causes do not eventually flow back on themselves"
        correct: true
      - text: "Directed: data flow forward in time; acyclic: no missing values; graph: a chart"
      - text: "Directed: supervised learning; acyclic: no loops in code; graph: network"
      - text: "Graph: a plot of residuals; directed: sorted; acyclic: no repeated values"
    explanation: "**p. 128:** “Graph means it is nodes and connections. Directed means the connections have arrows that indicate directions of causal influence. And acyclic means that causes do not eventually flow back on themselves.” And: “Unlike a statistical model, a DAG will tell you the consequences of intervening to change a variable. But only if the DAG is correct. There is no inference without assumption.” Engineers already draw DAGs: build graphs, service dependency maps, data-lineage graphs. A causal DAG is one where the arrows mean 'changing this changes that'."
  - id: total-effect
    prompt: "In the DAG A → M → D with A → D, what does a regression of D on A alone estimate?"
    options:
      - text: "Only the direct effect A → D"
      - text: "The total influence of A on D, through both the direct path and the indirect path via M"
        correct: true
      - text: "Only the indirect effect through M"
      - text: "Nothing interpretable"
    explanation: "**p. 129:** “Model m5.1, the regression of D on A, tells us only that the total influence of age at marriage is strongly negative with divorce rate. The “total” here means we have to account for every path from A to D.” An effect transmitted through an intermediate variable is called **mediation**. Example: a new CDN (A) lowers page latency (M), which raises conversion (D), and may also affect conversion directly via different caching rules. Regressing conversion on 'CDN enabled' alone gives the total effect."
  - id: listen-to-a
    prompt: "Why could M and D be associated even if M has no causal effect on D?"
    options:
      - text: "Because of measurement error"
      - text: "Because D causes M"
      - text: "Because both 'listen' to A: they share information from a common cause, so their association reflects A's influence"
        correct: true
      - text: "They could not; association implies causation here"
    explanation: "**p. 129:** “Because both M and D “listen” to A. They have information from A. So when you inspect the association between D and M, you pick up that common information that they both got from listening to A.” This is a **fork** (M ← A → D), the classic confound. Two microservices' error rates correlate because both listen to the same database — not because one causes the other."
  - id: testable-implications
    prompt: "What distinguishes the DAG M ← A → D from the DAG with A → D, A → M, M → D?"
    options:
      - text: "Nothing; they imply the same data"
      - text: "The second implies M ⊥ A"
      - text: "The first implies D ⊥ A"
      - text: "The first implies D ⊥ M | A: once you know A, M carries no further information about D; the second implies no such independence"
        correct: true
    explanation: "**p. 131:** “The testable implications of the first DAG are that all pairs of variables should be associated, whatever we condition on. The testable implications of the second DAG are that all pairs of variables should be associated, before conditioning on anything, but that D and M should be independent after conditioning on A.” (The book's 'first DAG' is the one with M → D; its 'second DAG' is M ← A → D — the reverse of the order in this question.) Conditional independencies are a DAG's testable implications. Multiple regression that conditions on A tests this one."
  - id: multiple-regression-questions
    prompt: "What descriptive question does a multiple regression of D on both M and A answer?"
    options:
      - text: "After I already know the other predictors, what additional value is there in knowing this one?"
        correct: true
      - text: "Which variable causes D"
      - text: "Whether the predictors are correlated"
      - text: "What would happen in a randomized experiment"
    explanation: "**p. 131:** “Is there any additional value in knowing a variable, once I already know all of the other predictor variables?” The coefficients are “(often opaque) answers to these questions. The questions above are descriptive, and the answers are also descriptive. It is only the derivation of the testable implications above that gives these descriptive results a causal meaning.” Without the DAG, a coefficient is just a conditional association."
  - id: divorce-result
    prompt: "In model m5.3 the coefficient on marriage rate moves to near zero while the coefficient on age at marriage barely changes. What is the conclusion?"
    options:
      - text: "Marriage rate is useless for prediction in any situation"
      - text: "Once age at marriage is known, marriage rate adds little; M is predictive but not causal, and the M–D association is spurious, created by A"
        correct: true
      - text: "Both have strong direct effects"
      - text: "Age at marriage is spurious"
    explanation: "**p. 134:** “Once we know median age at marriage for a State, there is little or no additional predictive power in also knowing the rate of marriage in that State.” But note: “if you didn’t have access to age-at-marriage data, then you’d definitely find value in knowing the marriage rate. M is predictive but not causal.” In ops: 'number of Slack messages in #incidents' predicts outage severity, but muting the channel will not shorten outages."
  - id: control-out-of-control
    prompt: "Why does the box “Control is out of control” call the phrase 'statistical control' sloppy?"
    options:
      - text: "Because statistical control is identical to experimental control"
      - text: "Because Bayesian models cannot control for anything"
      - text: "Because conditioning on a variable in a regression is quite different from controlling it in an experiment, and the language implies more than the model delivers"
        correct: true
      - text: "Because only randomization exists"
    explanation: "**p. 131:** “this is sloppy language, as it implies too much. Statistical control is quite different from experimental control.” Conditioning describes what the model learns about one variable within levels of another; it does not physically hold anything fixed. Chapter 6 shows that conditioning on the wrong variable can create bias. 'We controlled for region' in an analytics report deserves the same skepticism."
  - id: three-plots
    prompt: "Match each interpretive plot to its purpose."
    type: multiple
    options:
      - text: "Predictor residual plots: understand how the statistical model conditions, but not much else"
        correct: true
      - text: "Posterior prediction plots: check fit and assess predictions; not causal tools"
        correct: true
      - text: "Counterfactual plots: explore implied predictions of imaginary experiments that manipulate variables"
        correct: true
      - text: "Posterior prediction plots: estimate causal effects"
    explanation: "**p. 135:** “(1) Predictor residual plots ... useful for understanding the statistical model, but not much else. (2) Posterior prediction plots ... tools for checking fit and assessing predictions. They are not causal tools. (3) Counterfactual plots. These show the implied predictions for imaginary experiments. These plots allow you to explore the causal implications of manipulating one or more variables.”"
  - id: regression-to-mean
    prompt: "The posterior prediction plot shows the model under-predicts states with very high divorce and over-predicts states with very low divorce. What does the book say about this?"
    options:
      - text: "It shows the model is broken"
      - text: "It means the outcome should be log-transformed"
      - text: "It shows the priors are too wide"
      - text: "It is normal: regression is skeptical of extreme values and expects regression toward the mean; the interesting cases are points far from the diagonal, like Idaho and Utah"
        correct: true
    explanation: "**p. 139:** “This is what regression does—it is skeptical of extreme values, so it expects regression towards the mean.” Idaho and Utah sit far from the diagonal because of a variable the model lacks (religious composition). Outliers in posterior prediction plots point at missing variables. In SRE, the hosts your fleet model mispredicts most are the ones with an unrecorded difference — a different kernel, a noisy neighbor."
  - id: counterfactual-surgery
    prompt: "When simulating a counterfactual that manipulates M, what happens to the DAG?"
    options:
      - text: "All arrows into M are deleted, because when you control M, its usual causes no longer influence it"
        correct: true
      - text: "All arrows out of M are deleted"
      - text: "Nothing changes"
      - text: "M is removed from the graph"
    explanation: "**p. 143:** “when we manipulate some variable X, we break the causal influence of other variables on X. This is the same as saying we modify the DAG so that no arrows enter X ... It’s like a perfectly controlled experiment.” This is Pearl's *do*-operator. Feature flags are exactly this: forcing the flag on for a cohort cuts every arrow that normally decides who sees the feature, which is why flagged experiments estimate causal effects."
  - id: counterfactual-total
    prompt: "In Figure 5.6, manipulating A changes D through which paths?"
    options:
      - text: "Only A → D"
      - text: "Both A → D and A → M → D, because the simulation first updates M from A and then D from both"
        correct: true
      - text: "Neither; manipulation removes all paths"
      - text: "Only A → M → D"
    explanation: "**p. 142:** “This predicted trend in D includes both paths: A → D and A → M → D.” The counterfactual simulation runs the causal model forward in order: set A, simulate M given A, then simulate D given A and M. Simulation order follows the DAG's topological order — exactly how a build system evaluates targets."
  - id: simulate-dag
    prompt: "The box “Simulating the divorce example” generates `age`, then `mar` from `age`, then `div` from `age`. What is the point?"
    options:
      - text: "To increase sample size"
      - text: "To replace the real data"
      - text: "Every DAG implies a simulation; simulating data with known structure lets you check that your models recover the structure you built in"
        correct: true
      - text: "To compute p-values"
    explanation: "**p. 134:** “Every DAG implies a simulation, and such simulations can help us design models to correctly infer relationships among variables.” Fitting m5.1–m5.3 to this synthetic data reproduces the same pattern. It is the statistical equivalent of a fault-injection test: build a world where you know the truth, then check your analysis finds it."
---

Multiple regression can expose spurious associations — when used together with a causal model.

## Learning objectives

- Explain why correlations are everywhere and what a confound is
- Draw and read a DAG; distinguish total, direct and indirect effects
- Derive testable implications (conditional independencies) and test them with regression
- Use residual, posterior prediction and counterfactual plots for their proper purposes

## In a nutshell

```text
Dᵢ ~ Normal(μᵢ, σ)
μᵢ = α + βM Mᵢ + βA Aᵢ
α ~ Normal(0, 0.2);  βM, βA ~ Normal(0, 0.5);  σ ~ Exponential(1)
```

| DAG | Implied independence | Regression result | Verdict |
|---|---|---|---|
| A → M → D, A → D | None | — | Inconsistent with m5.3 |
| M ← A → D | D ⊥ M \| A | βM ≈ 0 given A | Consistent |

## For engineers

- **Correlation is cheap.** Across thousands of metrics, something always correlates with your incident.
- **A DAG is a design doc for an analysis.** It says what to condition on and why.
- **Manipulation deletes incoming arrows.** Feature flags and randomized rollouts are interventions.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 5 opening and Section 5.1, pp. 123–144; Figures 5.1–5.7.
- Pearl J, Glymour M, Jewell NP. *Causal Inference in Statistics: A Primer*. Wiley, 2016.
- Textor J et al. Robust causal inference using directed acyclic graphs: the R package 'dagitty'. *International Journal of Epidemiology* 2016. [doi:10.1093/ije/dyw341](https://doi.org/10.1093/ije/dyw341)
