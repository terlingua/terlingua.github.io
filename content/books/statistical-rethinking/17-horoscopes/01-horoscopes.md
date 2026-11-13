---
title: "Chapter 17: Horoscopes"
questions:
  - id: two-senses
    prompt: "In what two senses do statistics books resemble horoscopes?"
    type: multiple
    options:
      - text: "To remain plausibly correct, their advice must be very vague, because the targets are diverse"
        correct: true
      - text: "There are strong incentives to exaggerate the power and importance of the advice"
        correct: true
      - text: "Both are based on planetary motion"
      - text: "Both are always wrong"
    explanation: "**p. 553:** “First, in order to remain plausibly correct, they must remain tremendously vague ... It is easy to do better, once more detail is available. In the case of statistical analysis, it is typically only the scientist who can provide that detail, not the statistician.” And: “Second, there are strong incentives for both astrologers and statisticians to exaggerate the power and importance of their advice.” Generic engineering best-practice lists have the same flaw; the team that knows the system can do better."
  - id: not-additive
    prompt: "What does the book mean by 'scientific discovery is not an additive process'?"
    options:
      - text: "Science only multiplies"
      - text: "Data never add up"
      - text: "Statistics alone determines success"
      - text: "Weakness in one part (theory, measurement, analysis, publication) cannot be atoned by virtue in another; everything interacts"
        correct: true
    explanation: "**p. 553:** “Scientific discovery is not an additive process, in which sin in one part can be atoned by virtue in another. Everything interacts ... Statistical analysis can neither be uniquely credited with science’s success, nor can it be uniquely blamed for its failures and follies.” Reliability engineering is similar: perfect monitoring cannot compensate for no rollback plan."
  - id: mercury-rising
    prompt: "Which pieces of 'horoscopic advice' does the box “Mercury rising” offer?"
    type: multiple
    options:
      - text: "Thinking generatively — how the data could arise — solves many problems"
        correct: true
      - text: "All variables are measured with error, and conditioning on variables creates as many problems as it solves"
        correct: true
      - text: "There is no inference without assumption, but do not choose your assumptions for the sake of inference"
        correct: true
      - text: "Build complex models one piece at a time. Be critical. Be kind."
        correct: true
      - text: "Always report p < 0.05"
    explanation: "**p. 553:** “Thinking generatively—how the data could arise—solves many problems. Many statistical problems cannot be solved with statistics. All variables are measured with error. Conditioning on variables creates as many problems as it solves. There is no inference without assumption, but do not choose your assumptions for the sake of inference. Build complex models one piece at a time. Be critical. Be kind.” A one-paragraph summary of the whole book."
  - id: science-population
    prompt: "How does the book reconcile widespread false findings with the successes of science?"
    options:
      - text: "Science is a population-level process of variation and selective retention; most findings at any time can be false while science in the long run still works, like natural selection with mostly maladaptive variation"
        correct: true
      - text: "Most findings are true"
      - text: "Successes are luck"
      - text: "False findings are always fraud"
    explanation: "**p. 554:** “Science is a population-level process of variation and selective retention. It does not operate on individual hypotheses, but rather on populations of hypotheses ... So it’s entirely possible for most findings at any one point in time to be false but for science in the long term to still function.” Even Fermi 'discovered' elements that did not exist. Engineering organizations work the same way: many experiments fail, and the system improves if it retains what works and corrects errors."
  - id: dynamics-list
    prompt: "Which of these are among the book's list of pieces in the dynamics of scientific discovery?"
    type: multiple
    options:
      - text: "Quality of theory and predictions"
        correct: true
      - text: "Quality of measurement, including low signal-to-noise designs"
        correct: true
      - text: "Peer review, publication incentives and post-publication review"
        correct: true
      - text: "Replication and meta-analysis"
        correct: true
    explanation: "**p. 554–555** lists eight: theory and predictions, funding dynamics, measurement, data analysis, peer review, publication, post-publication peer review, and replication and meta-analysis. On analysis: “If you are not designing your analysis before you see the data, then your analysis may overfit the data in ways that regularization cannot reliably address.” On measurement: “Poor signal will not mean no findings, just unreliable ones.”"
  - id: openness
    prompt: "What aspect of science does the book say you personally control, and how should you practice it?"
    options:
      - text: "Peer review; be harsher"
      - text: "Openness: pre-plan the analysis as a mock analysis, register it publicly, analyze as planned, document and justify changes, and publish all data and scripts"
        correct: true
      - text: "Funding; apply for more grants"
      - text: "Publication; target top journals"
    explanation: "**p. 555:** “there is an aspect of science that you do personally control: openness. Pre-plan your research together with the statistical analysis ... Document it in the form of a mock analysis that you would not be ashamed to share with a colleague. Register it publicly ... Then collect the data. Then analyze the data as planned. If you must change the plan, that’s fine. But document the changes and justify them.” In engineering: write the experiment design doc, commit the analysis notebook before the test runs, and publish results with code."
  - id: paper-advertisement
    prompt: "What does the book say about the relationship between a paper and its data and analysis?"
    options:
      - text: "The paper is the product; data are optional"
      - text: "Data should be shared only on request"
      - text: "The data and its analysis are the scientific product; the paper is just an advertisement"
        correct: true
      - text: "Scripts should be private"
    explanation: "**p. 555:** “Do not provide scripts and data “on request,” but rather put them online so reviewers of your paper can access them without your interaction ... The data and its analysis are the scientific product. The paper is just an advertisement.” The engineering equivalent: the dashboard screenshot in a postmortem is the advertisement; the query, the data and the reproducible notebook are the product."
---

The book's short closing chapter: statistics is one part of a larger system, and openness is the part you
control.

## Learning objectives

- Explain the horoscope analogy and why statistical advice is necessarily vague
- Describe science as a population-level process that tolerates many false findings
- List the dynamics that shape scientific reliability
- Practice openness: pre-planned, documented, reproducible analysis

## In a nutshell

> Thinking generatively solves many problems. All variables are measured with error. Conditioning creates
> as many problems as it solves. No inference without assumption. Build complex models one piece at a time.
> Be critical. Be kind. (paraphrasing p. 553)

## An engineering checklist inspired by the chapter

1. Write the generative story and a DAG before touching the data.
2. Simulate fake data from it and check that your analysis recovers what you planted.
3. Commit the analysis plan (and code) before the experiment or migration runs.
4. Report the full posterior and predictions on the outcome scale, not a single 'significant' number.
5. Publish the queries, data extracts and notebooks with the decision document.

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Chapter 17, pp. 553–556.
- Horton R. Offline: What is medicine's 5 sigma? *The Lancet* 2015. [doi:10.1016/S0140-6736(15)60696-1](https://doi.org/10.1016/S0140-6736(15)60696-1)
- Open Science Collaboration. Estimating the reproducibility of psychological science. *Science* 2015. [doi:10.1126/science.aac4716](https://doi.org/10.1126/science.aac4716)
- Smaldino PE, McElreath R. The natural selection of bad science. *Royal Society Open Science* 2016. [doi:10.1098/rsos.160384](https://doi.org/10.1098/rsos.160384)
