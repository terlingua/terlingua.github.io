---
title: Incidents, On-Call and Postmortems
questions:
- id: incident-silence
  prompt: During an outage, an alert has fired in a channel with 60 engineers for 15 minutes, and everyone is reacting with emoji but nobody has claimed it. Which two processes from chapter 11 best explain this?
  explanation: 'Many witnesses lower each person''s sense of responsibility (“each bystander’s sense of responsibility to help decreases as the number of witnesses increases,” glossary p. 500), and calm reactions from others lead each person to conclude it''s not serious (pluralistic ignorance, “people think that everyone else is interpreting a situation in the same way, when in fact they are not,” glossary p. 502). Latané and Darley''s seizure study (chapter 2) and smoke study (chapter 11) show both. The fix is structural: route alerts to one named owner, and teach people to name a person when asking for help.'
  options:
  - text: Out-group homogeneity and ethnocentrism
  - text: Social facilitation and the halo effect
  - text: Postdecision dissonance and lowballing
  - text: Diffusion of responsibility and pluralistic ignorance
    correct: true
- id: incident-novice
  prompt: A new on-call engineer freezes during their first major incident and makes simple mistakes in commands they know well. Which concept best explains this, and what helps?
  explanation: Social facilitation is, when individual performance can be evaluated, “the tendency to perform better on simple tasks and worse on complex tasks” (glossary p. 503; chapter 9). For a novice, an incident is a complex, novel task under arousal. Practicing until the right actions are well learned changes what the dominant response is. Pairing new on-call engineers with a buddy also reduces evaluation apprehension.
  options:
  - text: 'Belief perseverance: they need new evidence'
  - text: 'Social loafing: they need more people watching'
  - text: Social facilitation; rehearse until correct responses dominate
    correct: true
  - text: 'The overjustification effect: they need fewer rewards'
- id: postmortem-attribution
  prompt: 'A postmortem draft concludes "root cause: engineer error." Which chapter 6 concepts should the reviewers check for, and what practice counters them?'
  explanation: The engineer who acted is salient, and observers overestimate dispositional causes (“The tendency to overestimate the extent to which people’s behavior is due to internal, dispositional factors,” glossary p. 501). Storms's videotape study shows perspective drives attribution, so rebuilding the actor's view of the situation helps. Asking for consensus information ("would others have done the same?") is the covariation model's antidote.
  options:
  - text: The mere exposure effect; showing the engineer more dashboards
  - text: The FAE and salience; rebuild the engineer's view
    correct: true
  - text: The halo effect; praising the engineer's other work
  - text: Social comparison; comparing the engineer with others
- id: postmortem-hindsight
  prompt: In a postmortem, everyone says "the warning signs were obvious." Which bias should facilitators name, and how can they counter it?
  explanation: The hindsight bias is “The tendency for people to overestimate, after knowing that something occurred, how much they could have predicted it before it occurred” (glossary p. 501; chapter 2). Signs look obvious only once you know which signals mattered. Timelines built from data available at the time show the real ambiguity responders faced.
  options:
  - text: Hindsight bias; rebuild what was known then
    correct: true
  - text: Group polarization; split into subgroups
  - text: Reactance; avoid telling people what to do
  - text: The planning fallacy; add buffers to estimates
- id: incident-obedience
  prompt: During an incident, a director orders the team to restart the primary database despite an engineer's warning that it could cause data loss. The engineer complies. Which factors from Milgram's research made compliance more likely, and which would have made refusal more likely?
  explanation: Milgram's variations showed obedience falls when peers refuse, when authorities are not present, and when people feel personally responsible (chapter 10). Fast escalation also keeps people on the "obey" track (adhering to the wrong norm). Incident roles that give anyone the right to call a stop, and a culture that backs the first dissenter, put the protective conditions in place.
  options:
  - text: Authority and pressure raised it; a peer's refusal would lower it
    correct: true
  - text: 'More likely: a peer refusing first; less likely: the director''s presence'
  - text: Compliance is determined only by personality
  - text: Time pressure always reduces obedience
- id: incident-stress
  prompt: An SRE team has been on a heavy rotation for months with back-to-back incidents and no recovery time. What does SPA 2 predict, and what change helps most?
  explanation: SPA 2 notes “The issue is when acute stress turns into chronic stress” and “it is not the amount of stress that matters for health so much as the chance to recover in between stressors” (p. 441). Physiological toughening comes from intermittent stressors with recovery. Rotation design, post-incident time off and alert hygiene are health interventions, not just productivity ones.
  options:
  - text: Only adding more alerts will help
  - text: Constant stress builds toughness; keep the rotation as is
  - text: Chronic stress; guarantee recovery time
    correct: true
  - text: Stress has no effect on health
- id: incident-story
  prompt: Two postmortem drafts contain the same facts. One is a chronological narrative; the other is a bulleted list of findings in random order. Readers find the first far more convincing. Which SPA 3 concept explains this?
  explanation: The story model is “The theory that jurors try to fit the evidence they hear at trial into a coherent story” (glossary p. 503); Pennington and Hastie found story-order presentation dramatically increased persuasion (SPA 3, p. 456). Narrative postmortems are more persuasive, which is good for clarity and a risk if the tidy story is wrong. Keep alternative hypotheses visible.
  options:
  - text: Own-race bias
  - text: The overjustification effect
  - text: The story model
    correct: true
  - text: Social loafing
- id: multi-incident
  prompt: Which practices directly apply the book's research to incident response? Select all that apply.
  type: multiple
  explanation: 'Each practice addresses a documented process: diffusion of responsibility (chapter 11), social facilitation (chapter 9), hindsight bias and the fundamental attribution error (chapters 2 and 6), and obedience (chapter 10).'
  options:
  - text: Paging a single named on-call owner rather than broadcasting to a large channel
    correct: true
  - text: Granting anyone the explicit right to halt a risky action
    correct: true
  - text: Running game days so correct responses become well learned
    correct: true
  - text: Writing timelines from data available at the time before judging decisions
    correct: true
---
## Learning objectives

- Recognize which social psychological processes operate during incidents
- Choose the concept that best explains a failure and the intervention that best addresses it
- Connect attribution, bystander, conformity, obedience and stress research to on-call practice

## Key ideas

Incidents compress many of the book's processes into a few hours: ambiguity (informational influence, chapter 10), many witnesses (bystander effect, chapter 11), salient individuals (fundamental attribution error, chapter 6), authority under pressure (obedience, chapter 10), arousal (social facilitation, chapter 9) and stress and recovery (SPA 2). Postmortems add hindsight bias (chapter 2), self-serving attributions and blaming the victim (chapters 6 and 8), and the story model (SPA 3).

## Sources

- Aronson et al., *Social Psychology* 12e, chs. 2, 6, 9, 10, 11 and SPA 2–3; glossary pp. 499–504
- Dekker, S. (2014). *The Field Guide to Understanding "Human Error"* (3rd ed.). Ashgate
