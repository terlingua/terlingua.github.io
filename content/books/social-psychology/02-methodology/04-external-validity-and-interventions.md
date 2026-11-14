---
title: 'Generalizing Results: External Validity, Replication and Intervention Science'
questions:
- id: external-validity
  prompt: What is external validity?
  explanation: 'The glossary defines external validity as “The extent to which the results of a study can be generalized to other situations and to other people” (p. 35, glossary p. 500). It has two parts: generalizing across situations (would a lab result hold in an office?) and generalizing across people (would a result from U.S. college students hold for engineers in Bangalore?). A load test that passes on a staging cluster with synthetic traffic has internal validity for that setup but uncertain external validity for production.'
  options:
  - text: Agreement between two observers
  - text: The reliability of a measurement instrument
  - text: How far results generalize to other situations and people
    correct: true
  - text: Making sure nothing but the independent variable affects the dependent variable
- id: psychological-realism
  prompt: The seizure study took place over an intercom in a lab, a situation people rarely face. Why does the book still argue it can generalize to real emergencies?
  explanation: 'Psychological realism is “The extent to which the psychological processes triggered in an experiment are similar to psychological processes that occur in everyday life” (p. 35, glossary p. 503). An experiment need not look like everyday life to tap the same mental processes. Participants believed the emergency was real, so they faced the same dilemma as real bystanders. A chaos-engineering game day is similar: the environment is staged, but if engineers experience real uncertainty and time pressure, their decision processes generalize to real incidents.'
  options:
  - text: Because it had high psychological realism
    correct: true
  - text: Because the participants were randomly selected from all humans
  - text: Because the results were statistically significant
  - text: Because it was a field experiment
- id: cover-story
  prompt: Why do researchers use a cover story?
  explanation: 'A cover story is “A description of the purpose of a study, given to participants, that is different from its true purpose and is used to maintain psychological realism” (p. 36, glossary p. 500). Latané and Darley said they were studying students'' personal problems; had participants known the study was about helping, they would have helped to look good. Cover stories involve deception, which the ethics section addresses: it must be justified and followed by a debriefing.'
  options:
  - text: To satisfy the institutional review board's requirement for deception
  - text: To keep psychological realism so people behave naturally
    correct: true
  - text: To hide the results from participants forever
  - text: To increase the sample size
- id: basic-dilemma
  prompt: What is the "basic dilemma of the social psychologist"?
  explanation: 'The glossary defines it as “The trade-off between internal and external validity in conducting research; it is very difficult to do one experiment that is both high in internal validity and generalizable to other situations and people” (p. 38, glossary p. 499). Lab studies maximize control; field experiments maximize realism but lose control. The book''s resolution is not to do it all in one study: run controlled lab studies to establish causation and field studies to show generality. Engineers do the same with microbenchmarks (control) and canary releases (realism).'
  options:
  - text: Whether to use correlational or observational methods
  - text: The choice between studying animals and humans
  - text: The trade-off between internal and external validity
    correct: true
  - text: Whether to publish results that do not reach significance
- id: field-experiment
  prompt: A company randomly assigns half its support agents to a new escalation script and compares customer satisfaction over a month in normal operations. What kind of study is this?
  explanation: Field experiments are “Experiments conducted in natural settings rather than in the laboratory” (p. 37, glossary p. 500). They keep random assignment, so causal inference is possible, while improving external validity because behavior is studied where it naturally occurs. The cost is less control over extraneous variables (seasonal traffic, a product outage in week two). Most product A/B tests are field experiments.
  options:
  - text: An archival analysis
  - text: A field experiment
    correct: true
  - text: A correlational survey
  - text: A laboratory experiment
- id: cross-cultural
  prompt: Why is cross-cultural research "not simply a matter of traveling to another culture, translating materials into the local language, and replicating a study there"?
  explanation: 'Cross-cultural research is “Research conducted with members of different cultures to see whether the psychological processes of interest are present in both cultures or whether they are specific to the culture in which people were raised” (p. 37, glossary p. 500). The book warns that “Researchers have to guard against imposing their own viewpoints and definitions learned from their culture onto another culture with which they are unfamiliar” and recommends working with researchers from the other culture (p. 37). A UX study localized only by translating the strings can fail the same way: the concept being measured may not map across cultures.'
  options:
  - text: Because translation is always impossible
  - text: Because cultures never differ in psychological processes
  - text: Researchers must not impose their own culture's definitions
    correct: true
  - text: Because random assignment is illegal in other countries
- id: replication-meta
  prompt: Which statements about replication and meta-analysis are correct? Select all that apply.
  type: multiple
  explanation: 'The book says “Replications are the ultimate test of an experiment’s external validity” (p. 38): only by repeating studies with different populations and settings can we know how far a result generalizes. Meta-analysis is “A statistical technique that averages the results of two or more studies to see if the effect of an independent variable is reliable” (p. 38, glossary p. 502), and the book''s example says one positive result out of 20 studies was probably an exception (p. 39). For engineers: one benchmark run proves little; repeated runs across machines, plus an aggregate, tell you whether the speed-up is real.'
  options:
  - text: A single significant study is sufficient to establish a finding
  - text: A meta-analysis statistically averages the results of two or more studies to see whether an effect is reliable
    correct: true
  - text: Replications are the ultimate test of an experiment's external validity
    correct: true
  - text: If an effect shows up in only 1 of 20 studies, a meta-analysis will suggest it is probably not reliable
    correct: true
- id: basic-vs-applied
  prompt: A researcher studies how feeling in control affects health in rats purely out of curiosity. Decades later, the findings help design better nursing-home routines. What does this illustrate?
  explanation: 'Basic research consists of “Studies that are designed to find the best answer to the question of why people behave as they do and that are conducted purely for reasons of intellectual curiosity”; applied research consists of “Studies designed to solve a particular social problem” (p. 39, glossary p. 499). The book''s example: research with dogs, rats and fish on control led to techniques that improved the health of nursing home residents (Langer & Rodin, 1976; Schulz, 1976), discussed in the health chapter. Lewin''s motto, “There is nothing so practical as a good theory” (p. 39), is the reason: to fix a problem you must understand the process that causes it.'
  options:
  - text: The line between basic and applied research is fuzzy
    correct: true
  - text: Animal research is irrelevant to humans
  - text: Applied research is more valuable than basic research
  - text: Basic and applied research never interact
- id: intervention-tournament
  prompt: A platform team has five ideas for reducing flaky tests (quarantine, retries, ownership labels, weekly reports, a leaderboard). Rather than trying them one at a time, it randomly assigns teams to one of the five or to a control group and measures the same outcome. What is this design called?
  explanation: An intervention tournament is “An experimental design whereby two or more interventions are compared to a single control condition and compared on the same outcomes to most quickly identify interventions that work and are most effective” (p. 40, glossary p. 502). The traditional approach bets on one intervention, tests it, and tweaks it if it fails. Tournaments find *which* interventions work, which work best, and whether some work better for certain groups, all at once. The book mentions tournaments on reducing partisan animosity (Voelkel et al., 2024). It is the social science version of a multi-arm experiment.
  options:
  - text: A meta-analysis
  - text: An intervention tournament
    correct: true
  - text: A wise intervention
  - text: An ethnography
- id: wise-intervention
  prompt: Employees at a financial firm were taught that stress can enhance performance. Days later they reported less stress, better health and better performance. What kind of intervention is this?
  explanation: Wise interventions are “Interventions designed to alter the meaning that people ascribe to themselves or their environment to elicit self-sustaining changes in behavior” (p. 40, glossary p. 504). The stress-mindset study (Crum et al., 2013) changed how employees construed stress rather than changing their workload. The premise is that a changed meaning keeps working after the intervention ends. Telling new hires that struggling with a large codebase is normal and temporary, rather than a sign they do not belong, is the same kind of intervention.
  options:
  - text: A reward-based behaviorist intervention
  - text: A wise intervention
    correct: true
  - text: A cover story
  - text: An intervention tournament
- id: intervention-limits
  prompt: What does the book identify as the biggest limitation of intervention science?
  explanation: 'The book says the biggest limitation is that “researchers are working with real people, trying to solve real problems, and the real world introduces real constraints” (p. 40). Community groups may object to randomly assigning members to a control group or to some uses of their data. Engineers face the same constraint when a customer will not accept being in the control arm of a reliability fix: sometimes ethics and trust outrank experimental purity.'
  options:
  - text: Interventions cannot be measured
  - text: Real communities impose real constraints
    correct: true
  - text: Interventions never work outside the lab
  - text: Interventions can only be studied in laboratories
---
## Learning objectives

- Define external validity and psychological realism, and explain why cover stories are used
- Explain generalizing across people and why cross-cultural research is not just translation
- Describe field experiments and the basic dilemma of the social psychologist
- Explain replication and meta-analysis
- Distinguish basic and applied research and describe intervention tournaments and wise interventions

## Key ideas

**External validity**: whether results generalize to other situations and people (p. 35). Generalizing across situations depends on **psychological realism** (do the processes match everyday life?) more than on surface realism; **cover stories** maintain it (p. 36). Generalizing across people needs random selection or replication with different populations; **cross-cultural research** must avoid imposing one culture's definitions (p. 37).

**Field experiments** happen in natural settings (p. 37). **Basic dilemma of the social psychologist**: the trade-off between internal and external validity (p. 38); resolve it with several studies, not one.

**Replications** are the ultimate test of external validity; **meta-analysis** averages results across studies (pp. 38–39).

**Intervention science** applies knowledge to real problems (p. 39). **Basic research** seeks why; **applied research** solves a problem; Lewin: “There is nothing so practical as a good theory.” **Intervention tournaments** compare several interventions against one control (p. 40). **Wise interventions** change the meaning people give to themselves or their situation (p. 40), such as teaching employees that stress can enhance performance (Crum et al., 2013).

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 2, pp. 35–40
- Aronson, E., & Carlsmith, J. M. (1968). Experimentation in social psychology. In *Handbook of Social Psychology*
- Hameiri, B., et al. (2022). Intervention tournaments. Discussed p. 40
- Walton, G. M., & Wilson, T. D. (2018). Wise interventions. *Psychological Review*, 125, 617–655
- Crum, A. J., Salovey, P., & Achor, S. (2013). Rethinking stress. *Journal of Personality and Social Psychology*, 104, 716–733
