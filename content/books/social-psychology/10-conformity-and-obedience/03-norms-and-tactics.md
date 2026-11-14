---
title: 'Conformity Tactics: Injunctive, Descriptive and Dynamic Norms, and Compliance Techniques'
questions:
- id: injunctive-descriptive
  prompt: '"Most engineers on this team write tests for every PR" versus "Our team expects tests on every PR." Which norm types are these?'
  explanation: The glossary gives descriptive norms as “Information about how other people typically behave in a social setting” and injunctive norms as “Information about how people should behave in a social setting” (glossary pp. 500–501), and also as people's perceptions of how others “actually behave” versus “what behaviors are approved or disapproved of by others.” Descriptive norms tell you what is normal; injunctive norms tell you what is approved. Both influence behavior, and they can conflict.
  options:
  - text: Both are descriptive
  - text: Both are injunctive
  - text: Descriptive first, injunctive second
    correct: true
  - text: The first is injunctive; the second is descriptive
- id: littering
  prompt: In Cialdini and colleagues' littering studies, when were people most likely to litter?
  explanation: 'Seeing a confederate litter in a littered environment drew attention to the descriptive norm "people litter here," and littering increased. Seeing someone litter in a clean environment highlighted that it was unusual, and littering decreased. The broken-windows dynamic in codebases is similar: in a module full of TODO hacks, adding another feels normal; in a clean module, the first hack stands out.'
  options:
  - text: In an already littered place
    correct: true
  - text: In a clean environment after seeing someone litter
  - text: Only when they were alone
  - text: Never; nobody littered
- id: boomerang
  prompt: A company emails each team its cloud spend compared with the average team. Teams that spent less than average start spending more. What happened, and how could it be prevented?
  explanation: 'The section "Using Norms to Change Behavior: Beware the ''Boomerang Effect''" (p. 288) describes Schultz et al. (2007): households told they used less energy than their neighbors began using more. When a smiley face (injunctive approval) was added for low users, the boomerang disappeared. Comparative dashboards need both: "you''re below average, and that''s great."'
  options:
  - text: Social loafing; adding more teams to the comparison would fix it
  - text: The boomerang effect
    correct: true
  - text: Groupthink; removing the email would fix it
  - text: Reactance; making the message more forceful would fix it
- id: dynamic-norms
  prompt: Only a minority of engineers currently use the new secrets manager, but adoption has doubled every month. Which message is most likely to increase adoption, according to research on dynamic norms?
  explanation: Dynamic norms are “Information about how people’s behaviors have been changing over time” (glossary p. 500). When the current majority does not yet do the desired behavior, a static descriptive norm works against you ("most people don't"). Telling people the behavior is spreading signals where the norm is heading and that change is possible. Sparkman and Walton (2017) found dynamic norms increased choices like eating less meat.
  options:
  - text: Say nothing about others' behavior
  - text: '"Only a few engineers use the secrets manager"'
  - text: '"More engineers are switching every month"'
    correct: true
  - text: '"Most engineers don''t use the secrets manager yet"'
- id: foot-in-door
  prompt: A colleague first asks you to review a 10-line PR, which you do. A week later they ask you to co-own their service's on-call rotation, and you agree. Which tactic is this?
  explanation: 'The foot-in-the-door technique is a “Social influence strategy in which getting people to agree first to a small request makes them more likely to agree later to a second, larger request” (glossary p. 501; section p. 289). It works through self-perception (chapter 4): after helping, you see yourself as someone who helps this person. In Freedman and Fraser''s (1966) study, homeowners who first agreed to display a small safe-driving sign were far more likely to accept a large ugly billboard later. Watch for this in scope creep.'
  options:
  - text: The foot-in-the-door technique
    correct: true
  - text: Lowballing
  - text: Reactance
  - text: The door-in-the-face technique
- id: door-in-face
  prompt: A manager first asks an engineer to lead a six-month platform rewrite, which the engineer declines. The manager then asks them to write a two-week design spike instead, and they agree. Which tactic is this?
  explanation: 'The door-in-the-face technique is a “Social influence strategy in which first asking people for a large request that they will probably refuse makes them more likely to agree later to a second, smaller request” (glossary p. 500; section p. 289). It works through the norm of reciprocity: the requester "conceded," so the target feels obliged to concede too. Cialdini et al. (1975) found students who refused to counsel delinquents for two years were much more likely to agree to chaperone them for a day. Anchored vendor quotes and initial scope estimates sometimes use this deliberately.'
  options:
  - text: The foot-in-the-door technique
  - text: Hypocrisy induction
  - text: The door-in-the-face technique
    correct: true
  - text: Attitude inoculation
- id: tactic-mechanisms
  prompt: Which statements correctly explain why these compliance techniques work? Select all that apply.
  type: multiple
  explanation: Foot-in-the-door relies on self-perception and consistency; door-in-the-face relies on reciprocal concessions. Neither involves new information about the request, which is why they are tactics rather than arguments. Recognizing them lets you evaluate the request on its own merits.
  options:
  - text: Both work without any change in the objective merits of the request
    correct: true
  - text: 'Door-in-the-face: a smaller request after a refusal feels like a concession, which people feel obliged to reciprocate'
    correct: true
  - text: Both work only when large rewards are offered
  - text: 'Foot-in-the-door: after complying with a small request, people see themselves as helpful and want to stay consistent'
    correct: true
- id: norms-engineering
  prompt: A team lead wants more engineers to write postmortems. Which message best combines the norm research?
  explanation: This message uses a dynamic norm (increasing adoption) and an injunctive norm (approval). The second and fourth options communicate a descriptive norm of not doing it, which suppresses the behavior. The third relies on threat, which can produce reactance (chapter 7) and only public compliance.
  options:
  - text: '"More teams write postmortems now, and we value it"'
    correct: true
  - text: '"Hardly anyone writes postmortems"'
  - text: '"You must write postmortems or face consequences"'
  - text: '"Writing postmortems is optional and few people bother"'
---
## Learning objectives

- Distinguish injunctive and descriptive norms and explain the littering studies
- Explain the boomerang effect and how to avoid it
- Explain dynamic norms
- Explain the foot-in-the-door and door-in-the-face techniques and why they work

## Key ideas

**Injunctive norms**: what people approve or disapprove of; **descriptive norms**: what people actually do (p. 287). Cialdini, Reno and Kallgren (1990): people littered more in a littered environment and when they saw someone else litter there; an injunctive cue (a confederate picking up litter) was especially effective.

**Boomerang effect** (p. 288): telling people the descriptive norm can push those *better* than average toward the average. Schultz et al. (2007): energy-use feedback made low users increase consumption, unless a smiley-face injunctive cue was added.

**Dynamic norms**: information about how behavior is changing over time; they can motivate change even when the current majority does not do it yet (Sparkman & Walton, 2017).

**Other tactics** (p. 289): **foot-in-the-door** (small request first, then larger) works through self-perception and consistency; **door-in-the-face** (large request refused, then smaller) works through reciprocal concessions.

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 10, pp. 286–292
- Cialdini, R. B., Reno, R. R., & Kallgren, C. A. (1990). A focus theory of normative conduct. *Journal of Personality and Social Psychology*, 58, 1015–1026
- Schultz, P. W., et al. (2007). The constructive, destructive, and reconstructive power of social norms. *Psychological Science*, 18, 429–434
- Sparkman, G., & Walton, G. M. (2017). Dynamic norms promote sustainable behavior. *Psychological Science*, 28, 1663–1674
- Freedman, J. L., & Fraser, S. C. (1966). Compliance without pressure. *Journal of Personality and Social Psychology*, 4, 195–202
- Cialdini, R. B., et al. (1975). Reciprocal concessions procedure for inducing compliance. *Journal of Personality and Social Psychology*, 31, 206–215
