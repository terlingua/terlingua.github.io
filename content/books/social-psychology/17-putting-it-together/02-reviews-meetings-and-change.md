---
title: Code Review, Meetings, Hiring and Change
questions:
- id: review-conformity
  prompt: In a design review, the first three senior engineers approve. A fourth engineer sees a flaw but approves too. Which process is most likely, and which practice would most reduce it?
  explanation: Asch's studies showed people conform to a unanimous majority even when the answer is clear, and that conformity drops sharply when responses are private or one ally dissents (chapter 10). Independent written reviews remove the visible majority; inviting dissent supplies the ally.
  options:
  - text: Normative influence; independent written reviews
    correct: true
  - text: Reactance; banning approvals
  - text: Social loafing; making the review anonymous after discussion
  - text: Informational influence; adding more seniors to the review
- id: review-hidden-profile
  prompt: An architecture decision meeting spends an hour on the trade-offs everyone already knows. The one engineer who knows the vendor is being acquired never mentions it, and the team picks that vendor. Which chapter 9 concept fits best?
  explanation: Groups tend to discuss shared information and neglect what only one member knows (Stasser and Titus's hidden profiles, chapter 9). Asking each member "what do you know that others might not?" and assigning explicit areas of expertise (transactive memory) surface unique information.
  options:
  - text: Idiosyncrasy credits
  - text: Social facilitation
  - text: The jigsaw effect
  - text: The failure to share unique information
    correct: true
- id: meeting-groupthink
  prompt: A tight-knit leadership group under deadline pressure, with a CTO who announced a preferred option at the start, unanimously approves a risky rewrite while privately some members had doubts. Which concept and remedy fit?
  explanation: Groupthink is “A kind of decision process in which maintaining group cohesiveness and solidarity is more important than considering the facts in a realistic manner” (glossary p. 501). The antecedents here, cohesion, stress and a directive leader, match Janis's model (chapter 9), and so do the remedies.
  options:
  - text: Equity theory; pay the dissenters more
  - text: Groupthink; impartial leader and outside dissent
    correct: true
  - text: Social loafing; more people in the meeting
  - text: The mere exposure effect; more meetings
- id: adoption-tpb
  prompt: A platform team wants every service to adopt structured logging. Survey data show engineers like the idea, but few adopt it because the library setup is painful and their tech leads don't mention it. Which two components of the theory of planned behavior should the team target?
  explanation: The theory of planned behavior says intentions depend on “attitudes toward specific behaviors, subjective norms, and perceived behavioral control” (glossary p. 504; chapter 7). Attitudes are already positive; the gaps are ease and social expectations. SPA 1's "removing small barriers" shows how much convenience matters.
  options:
  - text: Implicit attitudes and classical conditioning
  - text: General attitudes and personality traits
  - text: Perceived control and subjective norms
    correct: true
  - text: Reactance and inoculation
- id: adoption-norms
  prompt: Only 30% of teams use the new deploy pipeline, but adoption has tripled this quarter. Which announcement is most likely to increase adoption?
  explanation: A dynamic norm (“Information about how people’s behaviors have been changing over time,” glossary p. 500) plus injunctive approval avoids the boomerang of announcing that most people don't do it (chapter 10). A threat risks reactance (chapter 7) and only public compliance.
  options:
  - text: '"Most teams still haven''t switched to the new pipeline"'
  - text: '"Switch now or your deploys will be blocked"'
  - text: '"Adoption tripled this quarter; thank you"'
    correct: true
  - text: '"The new pipeline is optional and few teams use it"'
- id: adoption-elm
  prompt: The same migration proposal is presented to the team that owns the affected service and to a broad all-hands audience. According to the elaboration likelihood model, what should differ?
  explanation: Personal relevance increases motivation to process arguments (Petty, Cacioppo & Goldman, chapter 7). The people affected will scrutinize the arguments; others will rely on cues. Attitudes formed centrally last longer and predict behavior better, so invest in the arguments for the people who have to live with the change.
  options:
  - text: The owning team will be persuaded by slogans
  - text: Both audiences process identically
  - text: Owners need arguments; others rely on cues
    correct: true
  - text: The all-hands audience will scrutinize every benchmark
- id: hiring-halo
  prompt: An interviewer is impressed by a candidate's confident presentation and famous former employer, and rates their coding exercise highly despite several bugs. Which concept fits?
  explanation: The halo effect is “A cognitive bias by which we tend to assume that an individual with one positive characteristic also possesses other (even unrelated) positive characteristics” (glossary p. 501; chapter 13). Structured rubrics scored independently for each dimension reduce it, as do work-sample evaluations against predefined criteria.
  options:
  - text: The bystander effect
  - text: Out-group homogeneity
  - text: Self-handicapping
  - text: The halo effect
    correct: true
- id: hiring-sfp
  prompt: A hiring manager decides early that a candidate is weak, interrupts them, asks harder follow-ups and gives fewer hints than usual. The candidate performs poorly. Which study from chapter 8 does this most resemble?
  explanation: Word, Zanna and Cooper (1974) showed interviewers' cold, rushed behavior caused applicants to perform worse, a self-fulfilling prophecy (chapters 5 and 8). Standardizing interviewer behavior, with the same prompts and hints for everyone, prevents expectations from creating their own evidence.
  options:
  - text: The Robbers Cave study
  - text: Sherif's autokinetic study
  - text: Festinger and Carlsmith's $1/$20 study
  - text: Word, Zanna and Cooper's interview study
    correct: true
- id: review-dissonance
  prompt: After spending six months building an internal feature-flag system, a team rejects evidence that an open source alternative is better. Which two chapter 4 concepts best explain the resistance?
  explanation: Freely chosen, effortful investment increases liking for the result (“The tendency for individuals to increase their liking for something they have worked hard to attain,” glossary p. 501), and after a choice people enhance the chosen option and devalue alternatives (postdecision dissonance). Having people without the investment evaluate, and framing the switch as learning rather than failure, reduces the threat.
  options:
  - text: Effort justification and postdecision dissonance
    correct: true
  - text: Own-race bias and source monitoring
  - text: Social facilitation and deindividuation
  - text: The two-factor theory and catharsis
- id: multi-capstone
  prompt: Which statements capture the book's central lessons as they apply to engineering organizations? Select all that apply.
  type: multiple
  explanation: 'These four lessons run through the book: the power of the situation and construal (chapter 1), the experimental method and the cautionary CISD story (chapter 2 and SPA 1), and structural interventions like the jigsaw classroom, named on-call owners and removing small barriers (chapters 8, 11 and SPA 1).'
  options:
  - text: Situations, processes and norms often shape behavior more than individual personalities do
    correct: true
  - text: Changing structures, such as interdependence, ownership and ease of action, is often more effective than exhortation
    correct: true
  - text: Intuitions about what will work should be tested with controlled comparisons before scaling
    correct: true
  - text: How people construe a situation matters more than its objective features
    correct: true
---
## Learning objectives

- Identify conformity, groupthink, polarization and unshared information in technical decisions
- Identify attitude-change and norm-based levers for adopting new practices
- Identify first-impression, halo and stereotype effects in hiring and evaluation

## Key ideas

Technical decisions are group decisions, subject to normative conformity (chapter 10), groupthink and unshared information (chapter 9). Adopting new practices is attitude and behavior change: the theory of planned behavior and the ELM (chapter 7), norms and compliance techniques (chapter 10), dissonance (chapter 4), and small barriers (SPA 1). Hiring and evaluation invite thin-slicing, primacy, halo effects, stereotypes and self-fulfilling prophecies (chapters 5, 6, 8 and 13).

## Sources

- Aronson et al., *Social Psychology* 12e, chs. 4–10, 13 and SPA 1; glossary pp. 499–504
