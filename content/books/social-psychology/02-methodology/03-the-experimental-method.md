---
title: 'Answering Causal Questions: The Experimental Method'
questions:
- id: seizure-results
  prompt: In Latané and Darley's seizure study, what percentage of participants helped before the end of the seizure when they believed they were the only witness, compared with when they believed four others were present?
  explanation: 'When participants thought they were the only one who heard the victim, 85% helped; with one victim plus two other bystanders, 62%; with four other bystanders, only 31% (pp. 33–34). The number of bystanders participants *believed* were present strongly influenced helping. The book notes that group size is not the only cause, since some sole witnesses still failed to help and some in the large group did, but it is one important determinant. In engineering terms: an alert routed to a channel with forty people gets acknowledged later than the same alert paged to one named on-call engineer.'
  options:
  - text: About 50% in both conditions
  - text: About 30% versus 85%
  - text: 100% versus 0%
  - text: About 85% versus 31%
    correct: true
- id: iv-dv
  prompt: A team runs an experiment in which half of pull requests show an estimated review time and half do not, then measures how many hours each PR waits for its first review. Which is the independent variable?
  explanation: 'The independent variable is “The variable a researcher changes or varies to see if it has an effect on some other variable” (glossary p. 501); the dependent variable is “The variable a researcher measures to see if it is influenced by the independent variable” (glossary p. 500). Here the team varies whether the estimate is shown (IV) and measures waiting time (DV). In the seizure study the IV was the number of bystanders and the DV was helping. A handy rule: the dependent variable *depends* on the independent one, if the hypothesis is right.'
  options:
  - text: The programming language of each PR
  - text: The number of reviewers on the team
  - text: The hours until first review
  - text: Whether the review-time estimate is shown
    correct: true
- id: internal-validity
  prompt: The book says that keeping everything but the independent variable the same is called what?
  explanation: 'Internal validity means “Making sure that nothing besides the independent variable can affect the dependent variable; this is accomplished by controlling all extraneous variables and by randomly assigning people to different experimental conditions” (pp. 34, glossary p. 501). Latané and Darley prerecorded the other voices so every participant heard exactly the same emergency; only the number of supposed bystanders differed. In an A/B test, high internal validity means the two arms differ only in the feature: same traffic mix, same time window, same infrastructure.'
  options:
  - text: Internal validity
    correct: true
  - text: External validity
  - text: Operationalization
  - text: Psychological realism
- id: random-assignment
  prompt: Why does the book call random assignment to condition "the most important part of the experimental method"?
  explanation: Random assignment is “A process ensuring that all participants have an equal chance of taking part in any condition of an experiment; through random assignment, researchers can be relatively certain that differences in the participants’ personalities or backgrounds are distributed evenly across conditions” (p. 34, glossary p. 503). Without it, people who know about epilepsy might cluster in the sole-witness condition. Do not confuse it with random *selection*, which is about who gets into the study and affects external validity. Feature-flag systems that bucket users by hashing their ID are doing random assignment; they make the arms comparable but do not make your user base representative of the world.
  options:
  - text: Because it makes the experiment more realistic
  - text: It balances participants' differences across conditions
    correct: true
  - text: Because it guarantees the sample represents the whole population
  - text: Because it removes the need for a control group
- id: assignment-vs-selection
  prompt: Which statements correctly distinguish random assignment from random selection? Select all that apply.
  type: multiple
  explanation: Random assignment spreads individual differences evenly across conditions, which is what lets an experiment make causal claims (internal validity, p. 34). Random selection gives everyone in a population an equal chance of being sampled, which lets results generalize to that population (external validity, p. 29). An experiment run on volunteers from one company can have perfect random assignment and show causation *for that population* while telling you little about other populations.
  options:
  - text: Random assignment guarantees the results generalize to all people
  - text: Random selection is required for a study to show causation
  - text: Random selection decides who from a population is included and helps external validity
    correct: true
  - text: Random assignment decides which condition each participant is in and protects internal validity
    correct: true
- id: p-value
  prompt: An A/B test of a new checkout page reports p = .03 for the difference in conversion. Using the convention described in the book, how should this be read?
  explanation: 'The book defines the probability level (p-value) as a number that “tells researchers how likely it is that the results of their experiment occurred by chance and not because of the independent variable or variables” and says the convention is to consider results significant “if the probability level is less than 5 in 100” (p. 35, glossary p. 503). Strictly, a p-value is the probability of data at least this extreme *assuming no effect*; it is not the probability that the hypothesis is true, and it says nothing about how big or important the effect is. That is why the book stresses replication and meta-analysis: one significant result can be a fluke.'
  options:
  - text: Results this large would occur by chance only 3% of the time if there were no effect
    correct: true
  - text: There is a 97% chance the new page is better
  - text: The result is too small to matter
  - text: The effect size is 3%
- id: coin-chance
  prompt: Even with random assignment, why might two groups differ by chance?
  explanation: The book notes that if you randomly split 40 people into two groups, those who know most about epilepsy might by chance end up more in one group, “just as it is possible to get more heads than tails when you flip a coin 40 times” (p. 34). Statistics (p-values) quantify how likely the observed difference is under chance alone. Larger samples make such imbalances less likely, which is one reason small A/B tests produce noisy, unrepeatable "wins."
  options:
  - text: Because random assignment is always done incorrectly
  - text: They cannot; random assignment eliminates all chance differences
  - text: Chance can still put more of one kind of person in one group
    correct: true
  - text: Because participants choose their own condition
- id: confound
  prompt: A team compares error rates for a new deployment pipeline (used by volunteer teams) with the old pipeline (used by everyone else) and finds fewer errors with the new one. What threatens internal validity most?
  explanation: 'Because teams self-selected, any difference in errors could come from the kinds of teams that volunteer (more experienced, more invested) rather than the pipeline. This is exactly what random assignment prevents (p. 34). It also illustrates why the experimental method is the method of choice when you need causal answers: it “allows the experimenter to make causal inferences” (p. 31). If random assignment is impossible, at least compare like with like and treat the result as correlational.'
  options:
  - text: The p-value was not reported
  - text: Teams chose which pipeline to use
    correct: true
  - text: The study is not a field experiment
  - text: The error rate is the dependent variable
---
## Learning objectives

- Describe the Latané and Darley seizure study and what it shows about bystanders
- Define independent and dependent variables
- Define internal validity and explain why random assignment is the most important part of the experimental method
- Explain what a p-value is and the 5% convention

## Key ideas

**Seizure study** (Darley & Latané, 1968; pp. 32–34): participants talked over an intercom about college life and heard another "participant" have a seizure. When participants believed four other bystanders were present, 31% helped before the end of the seizure; with two others, 62%; when they thought they were the only witness, 85%.

**Independent variable (IV)**: what the researcher varies (number of bystanders). **Dependent variable (DV)**: what is measured (whether and how fast people help) (p. 32).

**Internal validity**: nothing besides the IV affects the DV, achieved by controlling extraneous variables and by **random assignment to condition** (p. 34), which spreads participants' personalities and backgrounds evenly across conditions. The voices were prerecorded so everyone heard exactly the same emergency.

**p-value** (p. 35): how likely the results would be if the IV had no effect. Convention: significant if p < .05.

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 2, pp. 31–35
- Darley, J. M., & Latané, B. (1968). Bystander intervention in emergencies. *Journal of Personality and Social Psychology*, 8, 377–383
