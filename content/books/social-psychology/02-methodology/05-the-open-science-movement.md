---
title: The Open Science Movement
questions:
- id: open-science-definition
  prompt: What is the open science movement?
  explanation: 'The glossary defines it as “A social movement originating partly in social psychology to reform scientific practices to improve the transparency, accessibility, and utility of science” (p. 41, glossary p. 502). Its core moves will feel familiar to anyone who works in open source: publish the inputs (materials and data), make the process reviewable (pre-registration), and let others verify by running it themselves (replication).'
  options:
  - text: A movement to replace experiments with surveys
  - text: A ban on deception in experiments
  - text: A campaign to make all journals free to publish in
  - text: A movement to make science more transparent, accessible and useful
    correct: true
- id: values-vs-pressures
  prompt: Why does the book say scientists' professional pressures can conflict with scientific values?
  explanation: The ideal is that knowledge is a common good and researchers do not care how a study turns out. But “social psychology is also a job” and “There is not enough grant money nor publication spots to go around,” which can lead researchers to be secretive or “very invested in how their studies come out” (p. 41). Engineers see the same misalignment when promotion criteria reward launching new systems over maintaining or verifying existing ones. Open science tries to redesign incentives so good practice is not a career sacrifice.
  options:
  - text: Because replication is illegal
  - text: Because journals require deception in every study
  - text: Because scientists are paid by the number of participants they test
  - text: Scarce grants and journal space reward secrecy and wanted results
    correct: true
- id: replication-numbers
  prompt: What did the large Open Science Collaboration (2015) replication project find for social psychology findings, according to the book?
  explanation: 'The book reports that “just under half the findings the teams attempted to replicate showed effects of a similar magnitude (Open Science Collaboration, 2015)” (p. 42). It also notes that before the movement “less than 1% of published studies in psychology were direct replications” (Makel et al., 2012) and that biomedical replications reported 20–25% (Prinz et al., 2011) and 11% (Begley & Ellis, 2012). The lesson is calibration: treat any single new finding, in psychology or in your own performance experiments, as provisional until it is repeated.'
  options:
  - text: No findings replicated
  - text: About 11% replicated
  - text: Every finding replicated perfectly
  - text: Just under half replicated at similar size
    correct: true
- id: nas-conclusion
  prompt: What did the National Academy of Sciences committee on reproducibility conclude, as summarized in the book?
  explanation: 'The committee “did not find evidence of a ‘replication crisis’ in science (including psychology) but reaffirmed and stressed the idea that researchers should openly share their data and research materials” and recommended that scientists “avoid overstating the implications of individual research findings” (p. 42). The book adds that it will flag findings that are new or contested. A good reading habit for this whole companion: ask how many times, with what samples, a result has been replicated.'
  options:
  - text: That only meta-analyses should be published
  - text: That replication is unnecessary if p is below .05
  - text: That psychology is in a unique crisis and should stop running experiments
  - text: No "crisis," but share data and don't overstate single findings
    correct: true
- id: weird
  prompt: What does "WEIRD" refer to in discussions of generalizability?
  explanation: 'The book says most research has been driven by researchers from “Western, Educated, Industrialized, Rich, and Democratic (‘WEIRD’) countries and thus is based on people from these countries, too” (p. 42). To claim a process is universal you must show it holds across diverse populations and situations. The engineering parallel: a model trained and evaluated only on traffic from one region or one device class may not generalize, however good its metrics look. Steven Roberts''s work extends the point to *who* conducts and evaluates research (p. 43).'
  options:
  - text: A type of wise intervention
  - text: Western, Educated, Industrialized, Rich and Democratic
    correct: true
  - text: A dimension of culture identified by Hofstede
  - text: A statistical test for weird outliers
- id: innovations
  prompt: Which of these are open science innovations the book describes? Select all that apply.
  type: multiple
  explanation: 'The book lists open access (paying to make papers free), pre-prints (posting the text of articles, which readers must evaluate because they may not be peer reviewed), pre-registration, open materials and open data (pp. 43–44). Publishing only significant results is the problem these practices address: it fills the literature with flukes. Each practice has an engineering twin: design docs written before implementation (pre-registration), reproducible build scripts (open materials) and shared benchmark datasets (open data).'
  options:
  - text: 'Open data: sharing data files when participants agree and confidentiality can be kept'
    correct: true
  - text: 'Open materials: sharing surveys, scripts and protocols'
    correct: true
  - text: 'Pre-registration: publishing hypotheses and planned methods before collecting data'
    correct: true
  - text: Pre-prints and open access publication
    correct: true
  - text: Publishing only statistically significant results
- id: preregistration-why
  prompt: Why does pre-registration give readers more confidence in a study's results?
  explanation: The book says pre-registration means researchers “publish their hypotheses and planned research methods before they collect any data,” which lets them “provide evidence that they truly predicted the results they found. If you learn that a study was pre-registered, then you can have more confidence that the results are robust” (p. 43). It protects against the hindsight bias and against trying many analyses and reporting the one that works. Declaring an A/B test's primary metric and stopping rule before launch does the same job.
  options:
  - text: Because pre-registration guarantees the results will replicate
  - text: It shows the predictions were made before seeing the data
    correct: true
  - text: Because it removes the need for a control condition
  - text: Because pre-registered studies always have larger samples
- id: preprint-caution
  prompt: What caution does the book give about pre-prints?
  explanation: Pre-prints “can be posted at any stage of the research or publication process, so they may reflect a paper highly vetted through peer review or the first draft of a paper,” so “the reader needs to evaluate the sources and final state of pre-prints, as pre-prints are self-published by scientists” (p. 43). They make knowledge more accessible but shift quality control onto the reader, much like reading an RFC draft versus a ratified standard.
  options:
  - text: They are always lower quality than journal articles
  - text: They are self-published, so readers must judge their status
    correct: true
  - text: They violate copyright and should be avoided
  - text: They cannot be cited
---
## Learning objectives

- Define the open science movement and explain the tension between scientific values and professional pressures
- Describe what large replication projects found and how the National Academies interpreted them
- Explain the WEIRD sampling problem and generalizability
- Describe open access, pre-prints, pre-registration, open materials and open data

## Key ideas

The **open science movement** aims to make science more transparent, accessible and useful (p. 41). Scientific ideals treat knowledge as a common good; the job (grants, publication pressure) pushes toward secrecy and investment in particular results.

**Replicability** (p. 42): before the movement fewer than 1% of published psychology studies were direct replications. The Open Science Collaboration (2015) found just under half of replicated findings showed effects of similar magnitude. Other fields fared worse (20–25% in some biomedical areas, 11% in another). The National Academies found no evidence of a "crisis" but urged openness and avoiding overstatement.

**Generalizability** (p. 42): most research comes from **WEIRD** (Western, Educated, Industrialized, Rich, Democratic) samples and researchers.

**Innovations** (pp. 43–44): open access, pre-prints, pre-registration, open materials, open data.

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 2, pp. 41–44
- Open Science Collaboration (2015). Estimating the reproducibility of psychological science. *Science*, 349, aac4716
- National Academies of Sciences, Engineering, and Medicine (2019). *Reproducibility and Replicability in Science*
- Muthukrishna, M., et al. (2020). Beyond WEIRD psychology. *Psychological Science*, 31, 678–701
