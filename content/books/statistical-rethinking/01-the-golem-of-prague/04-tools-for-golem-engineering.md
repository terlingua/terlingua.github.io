---
title: "1.3 Tools for Golem Engineering"
questions:
  - id: four-tools
    prompt: "Which four tools does the book choose to teach?"
    options:
      - text: "t-tests, ANOVA, chi-square tests and regression"
      - text: "Maximum likelihood, bootstrapping, permutation tests and p-values"
      - text: "Machine learning, deep learning, reinforcement learning and causal forests"
      - text: "Bayesian data analysis, model comparison, multilevel models and graphical causal models"
        correct: true
    explanation: "**p. 10:** “These tools are: (1) Bayesian data analysis (2) Model comparison (3) Multilevel models (4) Graphical causal models”. They serve three purposes the book names just before: “designing inquiry, extracting information from data, and making predictions.” Bayesian analysis is the inference engine; model comparison guards against overfitting; multilevel models handle clustered data and partial pooling; DAGs handle causal reasoning. The rest of the book is these four tools, layered."
  - id: bayes-counting
    prompt: "How does the book describe Bayesian data analysis 'in modest terms'?"
    options:
      - text: "As counting the numbers of ways the data could happen, according to our assumptions"
        correct: true
      - text: "As subjective opinion dressed up as math"
      - text: "As a way to make p-values smaller"
      - text: "As a computational trick that requires MCMC"
    explanation: "**p. 10:** “Bayesian data analysis is no more than counting the numbers of ways the data could happen, according to our assumptions. Things that can happen more ways are more plausible. Probability theory is relevant because probability is just a calculus for counting.” Chapter 2 makes this literal with marbles. The modest framing matters: Bayes is not magic or ideology, it is bookkeeping of possibilities, and the assumptions doing the counting are on the page for anyone to criticize."
  - id: frequentist-imaginary
    prompt: "According to the book, what is frequentist uncertainty premised on?"
    options:
      - text: "The scientist's personal beliefs"
      - text: "Imaginary resampling of data: the distribution of results if the measurement were repeated many times"
        correct: true
      - text: "The posterior distribution"
      - text: "The prior distribution"
    explanation: "**p. 11:** “frequentist uncertainty being premised on imaginary resampling of data—if we were to repeat the measurement many many times, we would end up collecting a list of values that will have some pattern to it. It means also that parameters and models cannot have probability distributions, only measurements can.” That distribution of measurements is the *sampling distribution*. The book notes that the resampling “is never done, and in general it doesn’t even make sense” for one-off processes. In SRE terms: a frequentist interval for 'this quarter's incident rate' imagines rerunning the quarter many times; a Bayesian interval directly states what you believe about the rate given the quarter you actually had."
  - id: saturn-example
    prompt: "The book uses Galileo's blurry view of Saturn to argue that Bayesian probability is useful even when there is no repeated sampling. What is the point?"
    options:
      - text: "Galileo should have taken more photographs"
      - text: "Saturn's shape changes randomly"
      - text: "The uncertainty comes from the instrument (our information), not from variation in Saturn's shape, so it is naturally expressed as probability over the true shape"
        correct: true
      - text: "Telescopes produce sampling distributions"
    explanation: "Galileo's telescope showed Saturn as a blob with 'ears'. Repeating the observation with the same telescope would not resolve it; the uncertainty is about the *information channel*. **p. 11–12** argues this is where Bayesian probability shines, because uncertainty about a fixed quantity can be described directly. Developers meet this with low-resolution telemetry: if your metrics are 1-minute averages, sampling them a thousand times will not reveal a 50 ms spike. The fix is better information, and meanwhile the honest description is a probability distribution over what happened."
  - id: overfitting-paradox
    prompt: "What does the book mean by “Fitting is easy; prediction is hard”?"
    options:
      - text: "Fitting a model requires little code"
      - text: "Prediction always needs deep learning"
      - text: "Models cannot predict the future at all"
      - text: "More complex models always fit the sample better, but often predict new data worse, so fit to the sample is a bad guide to model quality"
        correct: true
    explanation: "**p. 13:** “the smarter the golem, the dumber its predictions. So if we wish to make good predictions, we cannot judge our models simply on how well they fit our data. Fitting is easy; prediction is hard.” Adding parameters can only improve in-sample fit — the model memorizes noise. This is the same as a capacity planner fitting a 9th-degree polynomial to last month's traffic: perfect on the past, wild for next week. Chapter 7 introduces cross-validation and information criteria to estimate out-of-sample accuracy."
  - id: cv-ic-uses
    prompt: "Which benefits does the book attribute to cross-validation and information criteria?"
    type: multiple
    options:
      - text: "They give useful expectations of predictive accuracy rather than merely fit to the sample"
        correct: true
      - text: "They give an estimate of the tendency of a model to overfit"
        correct: true
      - text: "They identify the true causal model"
      - text: "They help us spot highly influential observations"
        correct: true
    explanation: "**p. 13** lists three benefits: “First, they provide useful expectations of predictive accuracy, rather than merely fit to sample. So they compare models where it matters. Second, they give us an estimate of the tendency of a model to overfit. This will help us to understand how models and data interact, which in turn helps us to design better models.” Third (p. 14), they “help us to spot highly influential observations.” They do *not* find the true causal model; p. 16 warns that causally wrong models can predict better."
  - id: partial-pooling
    prompt: "What trick do multilevel models use to fight overfitting?"
    options:
      - text: "Partial pooling: sharing information across units so that each unit's estimate is informed by the others"
        correct: true
      - text: "Dropping the noisiest groups"
      - text: "Setting all priors to be flat"
      - text: "Averaging every group into one number"
    explanation: "**p. 14:** “Multilevel models actually do something about it. What they do is exploit an amazing trick known as partial pooling that pools information across units in the data in order to produce better estimates for all units.” Imagine error rates for 200 microservices, some with millions of requests and some with twelve. Treating each service alone (no pooling) gives absurd estimates for tiny ones (0% or 25%); treating them all as one (complete pooling) ignores real differences. Partial pooling shrinks small-sample services toward the fleet average in proportion to how little data they have. Chapter 13 develops it."
  - id: four-reasons-multilevel
    prompt: "Which of these are among the four typical reasons the book gives for using multilevel models?"
    type: multiple
    options:
      - text: "To adjust estimates for repeat sampling"
        correct: true
      - text: "To adjust estimates for imbalance in sampling"
        correct: true
      - text: "To study variation"
        correct: true
      - text: "To avoid averaging"
        correct: true
      - text: "To make models run faster"
    explanation: "**p. 15:** “(1) To adjust estimates for repeat sampling ... (2) To adjust estimates for imbalance in sampling ... (3) To study variation ... (4) To avoid averaging. Pre-averaging data to construct variables can be dangerous. Averaging removes variation, manufacturing false confidence.” In telemetry: many requests per user (repeat sampling), a few heavy users dominating volume (imbalance), interest in per-region differences (variation), and the habit of reporting averages of averages (pre-averaging). The book adds that “multilevel regression deserves to be the default approach” (p. 400)."
  - id: wind-branches
    prompt: "When the wind blows, branches sway. Why can't a statistical model alone decide whether wind causes swaying or swaying causes wind?"
    options:
      - text: "Because the data are too noisy"
      - text: "Because a statistical model only sees association, which is symmetric; deciding direction needs information from outside the data"
        correct: true
      - text: "Because wind is hard to measure"
      - text: "Because trees are not random"
    explanation: "**p. 16:** “a statistical model is never sufficient for inferring cause, because the statistical model makes no distinction between the wind causing the branches to sway and the branches causing the wind to blow. Facts outside the data are needed to decide which explanation is correct.” A correlation between 'pods restarted' and 'memory alerts' is the same correlation whichever way the arrow points. Your knowledge of the system (restarts happen *because* of OOM kills) supplies the arrow."
  - id: identification-problem
    prompt: "The book distinguishes two meanings of *prediction*. Which pairing is right?"
    options:
      - text: "Short-term versus long-term forecasting"
      - text: "Training-set prediction versus test-set prediction"
      - text: "Guessing what happens next as a passive observer, versus predicting the consequences of an intervention — the second needs correct causal identification"
        correct: true
      - text: "Bayesian versus frequentist prediction"
    explanation: "**p. 16:** “Successful prediction does not require correct causal identification. In fact, as you’ll see later in the book, predictions may actually improve when we use a model that is causally misleading.” But when you intervene — “Suppose we recruit many people to climb into the trees and sway the branches. Will it make wind? Not much.” A model that predicts churn from 'opened the cancel page' predicts well, but forcing users *away* from the cancel page will not reduce churn. Forecasting needs association; decisions need causation."
  - id: dag-role
    prompt: "What does a directed acyclic graph (DAG) give you, according to the chapter?"
    options:
      - text: "A detailed statistical model that can be fitted directly"
      - text: "Proof that your causal assumptions are true"
      - text: "A way to learn causal structure from data automatically"
      - text: "A heuristic causal model from which you can deduce which statistical models give valid causal inferences, assuming the DAG is true"
        correct: true
    explanation: "**p. 17:** “DAGs are heuristic—they are not detailed statistical models. But they allow us to deduce which statistical models can provide valid causal inferences, assuming the DAG is true.” And the uncomfortable part: “The terrible truth about statistical inference is that its validity relies upon information outside the data.” A DAG is like an architecture diagram: it does not run, but it tells you which components can influence which, so you know where to look and what to control for."
  - id: causal-salad
    prompt: "A data scientist adds every available column as a 'control' to a regression of revenue on a new feature, watches the coefficient move, and tells a causal story. What does the book call this, and what is the danger?"
    options:
      - text: "Causal salad: included variables can confound as easily as omitted ones, so a model that predicts well may still mislead about interventions"
        correct: true
      - text: "Feature engineering, which is always safe if the model predicts well"
      - text: "Regularization, which prevents bias"
      - text: "Multilevel modeling"
    explanation: "**p. 17:** “Causal salad means tossing various “control” variables into a statistical model, observing changes in estimates, and then telling a story about causation. Causal salad seems founded on the notion that only omitted variables can mislead us about causation. But included variables can just as easily confound us.” Chapter 6 shows how: controlling for a post-treatment variable or a collider can create or hide effects. If 'sessions per week' is caused by the feature and also drives revenue, controlling for it erases the very effect you want to measure."
---

The chapter's toolkit: four ideas that let you build, criticize and compare golems instead of choosing one
from a flowchart.

## Learning objectives

- Name the four tools and the job each does
- Contrast Bayesian and frequentist meanings of probability
- Explain the overfitting paradox and the role of cross-validation and information criteria
- List the four reasons for multilevel models
- Distinguish forecasting from causal prediction, and explain what a DAG adds

## In a nutshell

| Tool | Problem it solves | Chapters |
|---|---|---|
| Bayesian data analysis | Turning assumptions plus data into a distribution of plausible values | 2–4, 9 |
| Model comparison | Choosing models by expected out-of-sample accuracy, spotting overfitting and influential points | 7 |
| Multilevel models | Clustered, imbalanced data; studying variation; partial pooling | 13–14 |
| Graphical causal models | Deciding which variables to include for a causal question | 5–6, 14–15 |

## Two kinds of prediction

```text
Forecast:     "Given what I observe, what will happen?"       -> associations are enough
Intervention: "If I change X, what happens to Y?"             -> needs a causal model
```

Cross-validation and information criteria answer the first question. Only a causal model, such as a DAG,
answers the second.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 1.3, pp. 10–17.
- Pearl J, Mackenzie D. *The Book of Why*. Basic Books, 2018.
- Gelman A, Hill J. *Data Analysis Using Regression and Multilevel/Hierarchical Models*. Cambridge University Press, 2007.
