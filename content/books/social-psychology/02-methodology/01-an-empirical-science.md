---
title: 'An Empirical Science: Questions, Hypotheses and Operationalization'
questions:
- id: hindsight-definition
  prompt: After an incident, everyone in the postmortem says "we all knew that config change was risky." Which bias does the book describe that fits this?
  explanation: 'The hindsight bias is “The tendency for people to overestimate, after knowing that something occurred, how much they could have predicted it before it occurred” (p. 22, glossary p. 501). The book raises it at the start of the methods chapter because social psychology findings often seem obvious once you hear them, which tempts people to dismiss the need for research. In postmortems the bias does damage twice: it makes the responders look negligent ("it was obvious") and hides the real uncertainty they faced. A useful antidote is to reconstruct what was known *at the time*, from logs and chat history, before discussing what should have happened.'
  options:
  - text: The hindsight bias
    correct: true
  - text: The bias blind spot
  - text: The planning fallacy
  - text: The overjustification effect
- id: predict-first
  prompt: Why does the book ask readers to predict the results of studies (the Try It! quiz on p. 23) before reading about them?
  explanation: 'Committing to a prediction before seeing the outcome is the only way to measure how predictable it really was. Many people who predict, for example, how many participants in Milgram''s study will deliver the maximum shock are far off, even though the result "makes sense" afterward. The same logic underlies pre-registration (later in this chapter) and writing down an A/B test''s expected effect before launching: a prediction recorded in advance cannot be bent by hindsight.'
  options:
  - text: Because readers' predictions are used as research data
  - text: Because the results are random and cannot be predicted anyway
  - text: Because predicting results is a form of informed consent
  - text: Because hindsight makes known results feel predictable
    correct: true
- id: question-sources
  prompt: Which of these are sources of research questions described in the book? Select all that apply.
  type: multiple
  explanation: 'The book describes inspiration from personal observations, from previous theories and research, and from cultural events (p. 24). Leon Festinger was dissatisfied with behaviorism''s account of attitude change, which led to cognitive dissonance theory. The 1964 murder of Kitty Genovese, and the report that 38 neighbors failed to help, led Latané and Darley to ask whether the *number* of witnesses reduces any one person''s likelihood of helping (pp. 24–25). Engineers do the same: an odd production observation, a theory that does not explain your system''s behavior, or an industry-wide incident all generate good questions.'
  options:
  - text: A random number generator that assigns topics
  - text: Major cultural events
    correct: true
  - text: Dissatisfaction with existing theories and previous research
    correct: true
  - text: Personal observations of everyday life
    correct: true
- id: genovese-reframe
  prompt: After the Kitty Genovese murder, many commentators asked what was wrong with New Yorkers. How did Latané and Darley reframe the question?
  explanation: 'Instead of a dispositional explanation, “Latané and Darley thought it would be more interesting and important to examine the social situation in which Genovese’s neighbors found themselves” (p. 25). Their hypothesis: the more people who witness an emergency, the less likely any one of them is to intervene, perhaps because each assumes someone else has acted. This is the chapter 1 lesson in action: resist the fundamental attribution error and look for the situational cause. (Later reporting found the "38 witnesses" story was exaggerated, but the bystander effect itself has been replicated many times; see chapter 11.)'
  options:
  - text: They asked which personality traits made New Yorkers apathetic
  - text: How the number of witnesses could reduce anyone's helping
    correct: true
  - text: They argued that helping behavior cannot be studied scientifically
  - text: They asked whether the newspaper report was accurate
- id: hypothesis-definition
  prompt: What is a hypothesis?
  explanation: 'The glossary defines a hypothesis as “A specific prediction about the answer to a research question” (p. 25, glossary p. 501). Specificity is what makes it testable: "bystanders influence helping" is vague; "people who believe they are the only witness will help more often than people who believe four others are present" can be confirmed or refuted. The engineering analogue is turning "the new cache will help" into "p95 latency on `/search` will drop by at least 20% at current traffic."'
  options:
  - text: A summary of results after a study is complete
  - text: A specific prediction answering a research question
    correct: true
  - text: A broad theory that explains everything in a domain
  - text: A description of a culture from the inside
- id: operationalization
  prompt: A team wants to study whether "psychological safety" improves software quality. They decide to measure safety by a five-item survey and quality by escaped defects per release. What is this step called?
  explanation: Operationalization is “The process of identifying how you will observe, measure, or manipulate a concept” (p. 25, glossary p. 502). It is harder than it sounds because abstract concepts (helping, aggression, quality, safety) can be measured many ways and each choice captures only part of the concept. Escaped defects miss maintainability; a survey may capture willingness to report rather than actual safety. Any conclusion is only as good as the operationalizations behind it, which is why the open science movement asks researchers to share their exact materials.
  options:
  - text: Debriefing
  - text: Meta-analysis
  - text: Random assignment
  - text: Operationalization
    correct: true
- id: theory-cycle
  prompt: How does the book describe the relationship between theory and research?
  explanation: 'The book describes theory-driven research as a cycle: a theory is developed, “specific hypotheses derived from that theory are tested; based on the results obtained, the theory is revised and new hypotheses are formulated” (p. 24). This mirrors iterative engineering: a model of your system (the theory) predicts what a change will do; you test, and the result updates the model. Lewin''s line that “There is nothing so practical as a good theory” (p. 39) makes the same point: good theory tells you where to look.'
  options:
  - text: Theory yields hypotheses; results revise the theory
    correct: true
  - text: Theory is unnecessary in applied research
  - text: Theories are proven true once and never tested again
  - text: Research is done first; theory is written afterward and never revised
---
## Learning objectives

- Explain the hindsight bias and why it makes social psychology findings look obvious
- Describe three sources of research questions: personal observation, previous theories and research, and cultural events
- Define a hypothesis and explain why a good one is specific and testable
- Define operationalization and explain why it is harder than it sounds

## Key ideas

**Hindsight bias**: after learning an outcome, people overestimate how predictable it was (p. 22). Findings look obvious *after* you hear them; the Try It! quiz on p. 23 asks you to predict first.

**Where questions come from** (p. 24): personal observations; previous theories and research (Festinger's dissatisfaction with behaviorism led to cognitive dissonance theory); cultural events (the 1964 murder of Kitty Genovese led Latané and Darley to study bystanders, pp. 24–25). Theory-driven research cycles: theory → hypotheses → test → revise theory.

**Hypothesis**: a specific prediction about the answer to a research question (p. 25). **Operationalization**: deciding how you will observe, measure or manipulate each concept (p. 25). "Helping" might become "left the cubicle to find the experimenter before the seizure ended."

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 2, pp. 22–25
- Darley, J. M., & Latané, B. (1968). Bystander intervention in emergencies: Diffusion of responsibility. *Journal of Personality and Social Psychology*, 8, 377–383
