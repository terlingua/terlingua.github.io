---
title: "1.2 Statistical Rethinking"
questions:
  - id: falsification-impossible
    prompt: "The chapter gives two reasons why deductive falsification is impossible in nearly every scientific context. What are they?"
    type: multiple
    options:
      - text: "Hypotheses are not models"
        correct: true
      - text: "Computers are not fast enough"
      - text: "Measurement matters"
        correct: true
      - text: "Bayesian priors are subjective"
    explanation: "**p. 4:** “(1) Hypotheses are not models. The relations among hypotheses and different kinds of models are complex. Many models correspond to the same hypothesis, and many hypotheses correspond to a single model. This makes strict falsification impossible. (2) Measurement matters. Even when we think the data falsify a model, another observer will debate our methods and measures.” The first is a mapping problem (many-to-many between ideas and models); the second is an evidence problem (the data themselves are uncertain and contested)."
  - id: nhst-not-popper
    prompt: "Why does the book say ordinary null hypothesis significance testing (NHST) is *not* really Popperian falsification?"
    options:
      - text: "Because NHST uses p-values instead of confidence intervals"
      - text: "Because Popper rejected all statistics"
      - text: "Because NHST tries to falsify a null hypothesis, not the research hypothesis, so the falsification is aimed at something other than the explanatory model"
        correct: true
      - text: "Because NHST is Bayesian"
    explanation: "**p. 5:** “usually NHST is used to falsify a null hypothesis, not the actual research hypothesis. So the falsification is being done to something other than the explanatory model. This seems the reverse from Karl Popper’s philosophy.” Popper wanted scientists to expose their *own* theory to refutation. NHST rejects a straw man (zero effect) and then treats the research hypothesis as supported by default. In an A/B test, rejecting 'the new checkout has exactly zero effect' does not test *your* explanation of why it works."
  - id: three-layers
    prompt: "Figure 1.2 separates three layers. Which mapping is correct?"
    options:
      - text: "Hypotheses → statistical models → process models, each one-to-one"
      - text: "Process models and statistical models are the same thing"
      - text: "Each hypothesis implies exactly one statistical model"
      - text: "Hypotheses (vague ideas) → process models (detailed causal mechanisms) → statistical models (associations), with many-to-many relations at each step"
        correct: true
    explanation: "From the caption of Figure 1.2 (p. 6): “Hypotheses (H) are typically vague, and so correspond to more than one process model (P). Statistical evaluations of hypotheses rarely address process models directly. Instead, they rely upon statistical models (M), all of which reflect only some aspects of the process models. As a result, relations are multiple in both directions.” Software analogue: the hypothesis 'the cache is causing latency' corresponds to several mechanisms (eviction storms, cold starts, lock contention), and each predicts similar latency histograms. A latency histogram (statistical model) cannot by itself tell them apart."
  - id: neutral-evolution
    prompt: "In the neutral-evolution example, why can neither rejecting nor failing to reject the neutral null model tell you much?"
    options:
      - text: "Because the same power-law statistical model is predicted both by a neutral process model and by some selection models"
        correct: true
      - text: "Because the sample sizes were too small"
      - text: "Because neutral theory is known to be false"
      - text: "Because power laws cannot be fitted to data"
    explanation: "The neutral equilibrium model (P0A) and a fluctuating-selection model (P1B) both predict a power-law distribution of allele frequencies — the same statistical model MII. **p. 7:** “The null model is not unique to any process model nor hypothesis.” The fix: “Explicitly compare predictions of more than one model”, and look for a description of the data in which the processes *differ*, such as changes in allele frequency over time. The engineering version: if two root causes produce the same error rate, stop staring at error rate and find a signal where they diverge (timing, affected hosts, request shape)."
  - id: entropy-identification
    prompt: "Why do statistical models so often correspond to many different process models, according to the box on entropy and model identification?"
    options:
      - text: "Because scientists rarely collect enough data"
      - text: "Because the distributions they use (normal, binomial, Poisson) are maximum entropy distributions that arise from many different underlying processes"
        correct: true
      - text: "Because the exponential family is rarely used"
      - text: "Because likelihoods are arbitrary"
    explanation: "**p. 7:** “Nature loves them because nature loves entropy, and all of the exponential family distributions are maximum entropy distributions.” And: “one can no more infer evolutionary process from a power law than one can infer developmental process from the fact that height is normally distributed.” A normal distribution of response times tells you little about *why* — countless processes that add up many small effects produce it. The upside: we can still use these distributions productively without knowing the mechanism. Chapter 10 develops this formally."
  - id: modus-tollens
    prompt: "Falsification uses *modus tollens*: H implies D; D is not observed; therefore H is false. What does finding D tell you?"
    options:
      - text: "H is proven true"
      - text: "H is false"
      - text: "Nothing certain about H, because other hypotheses might also predict D"
        correct: true
      - text: "D was a measurement error"
    explanation: "**p. 7:** “finding D tells us nothing certain about H, because other hypotheses might also predict D.” This asymmetry is why a passing test suite does not prove code correct, while a single failing test can show it is wrong — Dijkstra's point that testing shows the presence of bugs, not their absence. But the chapter's next move is crucial: in practice even the failing case is uncertain, because the observation itself might be wrong."
  - id: observation-error
    prompt: "The faster-than-light neutrino episode (2011) illustrates which problem with falsification?"
    options:
      - text: "Hypotheses are vague"
      - text: "Relativity was falsified"
      - text: "Physicists do not use statistics"
      - text: "Observation error: an apparent falsification of a well-tested theory was more likely a measurement problem, and it turned out to be one"
        correct: true
    explanation: "The OPERA team reported neutrinos arriving slightly faster than light (p. 8). Instead of declaring relativity falsified, the community — and the team itself — suspected the measurement, and the anomaly was later traced to equipment faults (including a loose fiber-optic connection). The observation D was itself uncertain. The engineering lesson matches the on-call heuristic: when a dashboard shows something physically impossible (negative latency, 300% CPU on a single core), suspect the instrumentation before the theory."
  - id: continuous-hypotheses
    prompt: "Why does the chapter say that the black swan story oversimplifies most scientific hypotheses?"
    options:
      - text: "Because most hypotheses are quantitative — about degrees, such as how often or how much — so a single observation rarely falsifies them"
        correct: true
      - text: "Because swans are not a good biological model"
      - text: "Because one black swan is not enough"
      - text: "Because Popper never discussed swans"
    explanation: "“All swans are white” is a discrete claim, so one counterexample refutes it. But the chapter points out (p. 8) that “most hypotheses are quantitative, concerning degrees of existence, rather than discrete, concerning total presence or absence.” A claim like 'black swans are rare' or 'the new build reduces crash rate' is about a frequency or effect size; a single crash does not refute it. Comparing such hypotheses is “probabilistic, a matter of degree, not kind” (p. 9) — which is what Bayesian estimation is for."
  - id: consensual
    prompt: "What does the book mean when it says falsification is *consensual, not logical*?"
    options:
      - text: "Scientists vote on which data to keep"
      - text: "Given measurement error and continuous hypotheses, scientific communities argue toward consensus about what evidence means; it is a social process, not a mechanical deduction"
        correct: true
      - text: "Logic plays no role in science"
      - text: "A theory is false when a majority of papers say so"
    explanation: "**p. 10:** “falsification is always consensual, not logical. In light of the real problems of measurement error and the continuous nature of natural phenomena, scientific communities argue towards consensus about the meaning of evidence.” Postmortems work the same way: after an incident, engineers weigh logs, metrics and timelines, argue about which explanation fits best, and converge on a root cause narrative. Nobody runs a single deterministic test that 'proves' it."
  - id: what-instead
    prompt: "A product team asks only 'is the new search ranking significantly better?' In the spirit of this section, which reframing is most useful?"
    options:
      - text: "Lower the p-value threshold to 0.01"
      - text: "Run the test longer until it becomes significant"
      - text: "Write down two or more plausible mechanisms for how ranking affects behavior, derive what each predicts (for example, effects on query reformulation or dwell time), and estimate the quantities that tell them apart"
        correct: true
      - text: "Only report the observed difference with no uncertainty"
    explanation: "The section argues for building and comparing several *non-null* models rather than testing against a null. The summary on p. 17 says “we should learn to build and analyze multiple non-null models of natural phenomena.” For a product experiment, competing mechanisms ('better top result' versus 'fewer clicks needed because snippets answer the question') predict different side effects. Estimating those effects with uncertainty teaches you far more than one binary verdict, and avoids the trap of 'peeking until significant'."
---

This section argues that the common goal of statistics in science — testing null hypotheses — rests on a
misreading of Karl Popper, and that falsification is far messier in practice than the textbook story.

## Learning objectives

- Explain why hypotheses, process models and statistical models are different things
- Explain why NHST is not Popperian falsification
- Describe how measurement error and quantitative hypotheses undermine strict falsification
- Explain why falsification is a social, consensual process

## In a nutshell

| Problem | What it means | Developer analogy |
|---|---|---|
| Hypotheses are not models | Many-to-many links between ideas, mechanisms and statistical patterns | Many root causes produce the same error-rate graph |
| Observation error | The evidence itself might be wrong | Broken instrumentation, clock skew |
| Continuous hypotheses | Claims are about how much, not whether | 'Reduces p99 latency' versus 'never exceeds 100 ms' |
| Consensual falsification | Communities argue to a conclusion | Postmortems |

## The three layers

```text
Hypothesis (vague)        Process model (mechanism)           Statistical model (pattern)
"Evolution is neutral" -> P0A neutral equilibrium        ->  MII  power law
                          P0B neutral non-equilibrium    ->  MI
"Selection matters"   -> P1A constant selection          ->  MIII
                          P1B fluctuating selection      ->  MII  power law (same as P0A!)
```

The practical rule from p. 7: compare the predictions of several process models, and when they agree on one
kind of data, look for another kind of data where they disagree.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 1.2, pp. 4–10; Figure 1.2, p. 6.
- Popper K. *The Logic of Scientific Discovery*. 1959.
- OPERA Collaboration. Measurement of the neutrino velocity with the OPERA detector in the CNGS beam. *JHEP* 2012. [doi:10.1007/JHEP10(2012)093](https://doi.org/10.1007/JHEP10(2012)093)
- Dijkstra EW. *Notes on Structured Programming*, 1970 — “Program testing can be used to show the presence of bugs, but never to show their absence!”
