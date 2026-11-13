---
title: "2.2 Building a Model"
questions:
  - id: design-loop
    prompt: "What are the three steps of the book's design loop for a simple Bayesian model?"
    options:
      - text: "Collect, clean, visualize"
      - text: "Data story, update, evaluate"
        correct: true
      - text: "Train, validate, deploy"
      - text: "Hypothesize, test, reject"
    explanation: "**p. 28:** “(1) Data story: Motivate the model by narrating how the data might arise. (2) Update: Educate your model by feeding it the data. (3) Evaluate: All statistical models require supervision, leading to model revision.” It is a loop, not a pipeline: evaluation sends you back to revise the story. Engineers will recognize the shape of a build–measure–learn loop or red–green–refactor: write down what you expect, run it, inspect the result, adjust."
  - id: data-story
    prompt: "Why does the book insist on narrating a *data story* before fitting anything?"
    options:
      - text: "Because the story must be published with the results"
      - text: "Because stories make the model run faster"
      - text: "Because a story that explains how each observation is generated can be translated directly into a probability model, and writing it down exposes ambiguities in vague hypotheses"
        correct: true
      - text: "Because priors come from stories and likelihoods come from data"
    explanation: "For the globe: “The true proportion of water covering the globe is p. A single toss of the globe has a probability p of producing a water (W) observation... Each toss of the globe is independent of the others.” (p. 28–29). Each sentence becomes part of the model. And p. 29 notes that when you try to state precisely how a vague hypothesis generates data, “many stories and resulting models will be consistent with the same vague hypothesis. Resolving that ambiguity often leads to important realizations and model revisions, before any model is fit to data.” This is the same benefit as writing a design doc before code: the act of specifying reveals what you had not decided."
  - id: updating-sequence
    prompt: "In Figure 2.5 the globe-tossing posterior is updated one toss at a time. What happens to the plausibility of values of p each time a W is observed?"
    options:
      - text: "All values of p get the same boost"
      - text: "Only p = 1 gets a boost"
      - text: "Nothing changes until the sample is complete"
      - text: "p = 0 becomes impossible, and plausibility shifts toward higher values of p"
        correct: true
    explanation: "Each W is likelier under larger p, so higher proportions gain relative plausibility, and p = 0 (no water) is eliminated outright, because it has zero ways to produce a W. Each L does the reverse and eliminates p = 1. After the sequence W L W W W L W L W, the curve peaks near 6/9 ≈ 0.67. The key idea is that the posterior after n observations is the prior for observation n + 1, exactly like folding events into running state."
  - id: order-irrelevant
    prompt: "Does the order of the tosses matter for the final posterior in the globe model?"
    options:
      - text: "No — given the independence assumption, any order of the same six W and three L gives the same posterior"
        correct: true
      - text: "Yes, later observations count more"
      - text: "Yes, early observations count more because they set the prior"
      - text: "Only if the prior is flat"
    explanation: "Because updating multiplies likelihoods, and multiplication is commutative, the final posterior is independent of order — *if* the model's assumption of independent tosses holds. The book uses this (p. 31) as an example of something to check: if the real data show trends with time, the model's insensitivity to order is hiding something. Analogy: summing request counts in any order gives the same total, but if your error rate is drifting during a rollout, a model that ignores order will miss the drift."
  - id: sample-size
    prompt: "What does the book say about Bayesian estimates and sample size?"
    options:
      - text: "Bayesian estimates require at least 30 observations"
      - text: "Bayesian estimates are valid for any sample size, though they depend on the prior, and more data still helps"
        correct: true
      - text: "Bayesian estimates are invalid for small samples"
      - text: "Sample size does not matter at all in Bayesian inference"
    explanation: "**p. 31:** “Bayesian estimates are valid for any sample size. This does not mean that more data isn’t helpful—it certainly is. Rather, the estimates have a clear and valid interpretation, no matter the sample size. But the price for this power is dependency upon the initial plausibilities, the prior.” With one toss you get a legitimate (very wide) posterior. This is useful for small-data engineering decisions — a new service with five incidents still gets an honest, wide estimate of its incident rate, rather than a method that refuses to answer."
  - id: certainty-no-guarantee
    prompt: "As you toss the globe more, the posterior becomes narrower. Which caution does the book attach to this?"
    options:
      - text: "Narrow posteriors are always wrong"
      - text: "Narrow posteriors mean you should stop collecting data"
      - text: "The model's certainty is no guarantee that the model is a good one"
        correct: true
      - text: "Posterior width does not depend on sample size"
    explanation: "**p. 31:** “First, the model’s certainty is no guarantee that the model is a good one.” A model is certain *conditional on its assumptions*. If tosses were not independent (say, a thrower who always catches the globe the same way), the model would still become very confident, but about the wrong thing. Same with dashboards: a tight confidence band from a million requests is precise, but if the metric itself is biased (for instance, timeouts not recorded as errors), you are precisely wrong."
  - id: supervise
    prompt: "What does the book mean by the second caution, to *supervise and critique* the model?"
    options:
      - text: "Have a statistician approve every model"
      - text: "Rerun the model with different random seeds"
      - text: "Test whether the model's assumptions are exactly true"
      - text: "Check the model's adequacy for its purpose by asking questions beyond those it was built to answer — for example, whether there are patterns the model ignores, such as trends in the order of data"
        correct: true
    explanation: "**p. 32:** “the objective is to check the model’s adequacy for some purpose. This usually means asking and answering additional questions, beyond those that originally constructed the model.” The book stresses that all models are false, so testing whether assumptions are *true* is pointless; the question is whether they are good enough here. It is a monitoring mindset: an SLO model of availability does not need to be true, but you should check whether its errors matter for the decision you are making."
  - id: false-but-useful
    prompt: "Why is it pointless, according to the book, to check whether a model's assumptions are literally true?"
    options:
      - text: "Because all models are false in the large world, yet a model with false assumptions can still produce useful estimates"
        correct: true
      - text: "Because assumptions cannot be checked"
      - text: "Because Bayesian models have no assumptions"
      - text: "Because only priors are assumptions"
    explanation: "p. 32 says a model may still produce a perfectly useful estimate despite false assumptions, “because models are essentially information processing machines”. The globe is not tossed by independent physics, Earth is not a perfect sphere, and the model still estimates the water fraction well. A load balancer's round-robin model assumes equal server capacity — false — yet works well enough until one server is much slower. Check adequacy for purpose, not truth."
---

The globe-tossing model: toss an inflatable globe, record whether your right index finger lands on water (W)
or land (L), and estimate the proportion of water p.

## Learning objectives

- Follow the data story → update → evaluate loop
- Explain how a story about data generation becomes a model
- Explain Bayesian updating one observation at a time and why order does not matter
- State the two cautions about model certainty and supervision

## In a nutshell

```text
Data:   W L W W W L W L W   (6 W, 3 L)
Story:  each toss lands on water with probability p, independently of other tosses
Update: start flat, multiply by p for each W and by (1 − p) for each L
Result: a posterior over p peaking near 0.67
```

- Bayesian estimates are valid at any sample size; the cost is dependence on the prior.
- Certainty is conditional on the model. Check adequacy for purpose, not literal truth.

## For engineers

The updating loop is the same shape as an event-sourced projection: state starts at a prior, each event
updates it, and the final state does not depend on event order only if events are independent and
commutative. When the real stream has ordering effects, the projection is wrong even though every step is
correct.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 2.2, pp. 28–32; Figure 2.5.
