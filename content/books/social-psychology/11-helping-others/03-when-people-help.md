---
title: 'Situational Determinants: Cities, Mobility and the Bystander Effect'
questions:
- id: urban-overload
  prompt: Why do people in big cities help strangers less often, according to the urban overload hypothesis?
  explanation: 'The urban overload hypothesis is “The theory that people living in cities are constantly bombarded with stimulation and that they keep to themselves to avoid being overwhelmed by it” (glossary p. 504; section p. 321). Supporting it, the immediate environment predicts helping better than where people grew up: a person raised in a small town helps less when in a dense city. Engineers in alert-heavy environments show a parallel: bombarded by notifications, they filter aggressively and miss the one that matters.'
  options:
  - text: Cities have fewer people in need
  - text: Rural people are paid to help
  - text: They screen out stimulation to avoid overload
    correct: true
  - text: City dwellers have less altruistic personalities
- id: residential-mobility
  prompt: What did research find about residential mobility and helping?
  explanation: 'The section on residential mobility (p. 322) reports that stable communities show more prosocial behavior, such as buying "critical habitat" license plates or supporting local businesses (Oishi et al., 2007). Teams with high turnover face the same problem: members who expect to leave soon invest less in shared infrastructure and documentation.'
  options:
  - text: Long-term residents help more
    correct: true
  - text: Long-term residents help less
  - text: People who move often help more
  - text: Mobility has no effect
- id: bystander-effect
  prompt: An alert fires in a shared channel with 80 engineers. Nobody acknowledges it for 20 minutes. When the same alert pages one named on-call engineer, it is acknowledged in two minutes. What explains the difference?
  explanation: The bystander effect is “The finding that the greater the number of bystanders who witness an emergency, the less likely any one of them is to help” (glossary p. 499; section p. 323). Latané and Darley's seizure study (chapter 2) showed helping fell from 85% with one witness to 31% with four others. A meta-analysis (Fischer et al., 2011) confirmed the effect, though it weakens in clearly dangerous emergencies. Paging a named owner is the standard engineering fix.
  options:
  - text: The bystander effect
    correct: true
  - text: Social facilitation
  - text: The overjustification effect
  - text: Group polarization
- id: five-steps
  prompt: What are the five steps in Latané and Darley's model of bystander intervention, in order?
  explanation: 'Each step is a point where helping can fail (sections pp. 323–328). It is a pipeline: a failure at any stage drops the request. Designing incident response is designing for each step: make problems noticeable (good alerts), unambiguous (clear severity), owned (named on-call), actionable (runbooks) and low-cost to act on (no blame for false alarms).'
  options:
  - text: Interpret the event, assume responsibility, notice it, implement help, know how
  - text: Feel empathy, notice the event, evaluate costs, help, debrief
  - text: Decide to help, notice the event, call for help, assume responsibility, help
  - text: Notice, interpret, take responsibility, know how, act
    correct: true
- id: good-samaritan
  prompt: In Darley and Batson's (1973) Good Samaritan study, seminary students walked to give a talk (some about the Good Samaritan parable) and passed a man slumped and groaning. What predicted whether they stopped?
  explanation: Only about 10% of students who were told they were late stopped, compared with about 63% of those who had plenty of time; the parable topic made little difference (section p. 324). Hurry prevented the first step, noticing. Engineers under deadline pressure likewise walk past a teammate's struggle in chat, or a suspicious log line, not out of callousness but because they never really notice.
  options:
  - text: Whether they were in a hurry
    correct: true
  - text: Whether their talk was about the Good Samaritan
  - text: Their religiosity scores
  - text: Their gender
- id: pluralistic-ignorance
  prompt: During a confusing incident, everyone on the bridge call stays calm and silent. Each engineer privately suspects something is badly wrong but concludes from others' calm that it isn't. What is this?
  explanation: Pluralistic ignorance is “The case in which people think that everyone else is interpreting a situation in the same way, when in fact they are not” (glossary p. 502; section p. 325). In Latané and Darley's smoke study, participants alone usually reported the smoke; those with calm confederates rarely did. Everyone looks to others for information (chapter 10's informational influence) while hiding their own concern. Asking "does anyone else think this looks bad?" can break the spell.
  options:
  - text: Social loafing
  - text: Diffusion of responsibility
  - text: Pluralistic ignorance
    correct: true
  - text: Groupthink
- id: diffusion
  prompt: What is diffusion of responsibility?
  explanation: Diffusion of responsibility is “The phenomenon wherein each bystander’s sense of responsibility to help decreases as the number of witnesses increases” (glossary p. 500; section p. 327). It is step 3 in the model. "Someone else will handle it" feels reasonable to each person, so no one does. GitHub issues with many watchers and no assignee are classic examples.
  options:
  - text: Each witness feels less responsible as witnesses grow
    correct: true
  - text: Assigning tasks to many people at once
  - text: Sharing on-call duties fairly
  - text: Spreading blame after an incident
- id: know-how
  prompt: An engineer notices a colleague's laptop showing a ransomware message, interprets it as an emergency, and feels responsible, but doesn't act because they don't know the procedure. Which step failed?
  explanation: The section on knowing how to help (p. 327) explains that even willing bystanders do not act if they lack the competence or the procedure. Security incident procedures that every employee knows ("disconnect from the network and call this number") remove this barrier. Training in first aid works the same way.
  options:
  - text: 'Step 2: interpreting'
  - text: 'Step 5: deciding to implement'
  - text: 'Step 1: noticing'
  - text: Step 4
    correct: true
- id: implement-help
  prompt: What can stop people at the final step, deciding to implement help?
  explanation: The section on deciding to implement help (p. 328) describes audience inhibition, the fear of looking foolish in front of others, and the costs of helping (time, danger, effort). Organizations that punish false alarms ("why did you page me for that?") raise those costs and teach people not to escalate. Rewarding reasonable escalations, even when they turn out to be minor, keeps step 5 open.
  options:
  - text: Fear of embarrassment and costs
    correct: true
  - text: They have not noticed the event
  - text: They don't know any procedure
  - text: They have too much responsibility
- id: online-diffusion
  prompt: Research on diffusion of responsibility online found what?
  explanation: 'The section on diffusion of responsibility and intervention online (p. 328) reports that the bystander effect appears in chat rooms and email: requests sent to many people get fewer replies. Personalizing the request ("@maria, can you look at this?") removes the ambiguity about who is responsible. Ticket systems with explicit assignees and rotation bots apply this finding.'
  options:
  - text: People respond more in larger online groups
  - text: Online settings eliminate the bystander effect
  - text: Naming individuals makes no difference
  - text: Larger groups respond less unless people are named
    correct: true
---
## Learning objectives

- Explain urban–rural differences and the urban overload hypothesis
- Explain how residential mobility affects helping
- Explain the bystander effect and Latané and Darley's five-step model
- Define pluralistic ignorance and diffusion of responsibility
- Explain diffusion of responsibility online

## Key ideas

**Urban versus rural** (p. 321): people help strangers more in small towns; the **urban overload hypothesis** says city dwellers are bombarded with stimulation and keep to themselves. Population density predicts helping better than where people grew up. **Residential mobility** (p. 322): people who have lived longer in a place help more (Oishi et al., 2007).

**Bystander effect** (p. 323): the more bystanders, the less likely any one is to help. Five steps:

1. **Notice** the event (p. 324): hurried seminary students in the Good Samaritan study (Darley & Batson, 1973) often walked past a groaning man.
2. **Interpret** it as an emergency (p. 325): **pluralistic ignorance**; in the smoke-filled room study, people with calm confederates rarely reported smoke.
3. **Assume responsibility** (p. 327): **diffusion of responsibility**.
4. **Know how to help** (p. 327).
5. **Decide to implement** help (p. 328): costs, audience inhibition, fear of embarrassment.

**Online** (p. 328): diffusion of responsibility occurs in chat rooms and email; larger groups respond less, but being addressed by name restores responsibility.

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 11, pp. 321–330
- Milgram, S. (1970). The experience of living in cities. *Science*, 167, 1461–1468
- Latané, B., & Darley, J. M. (1970). *The Unresponsive Bystander*. Appleton-Century-Crofts
- Darley, J. M., & Batson, C. D. (1973). "From Jerusalem to Jericho." *Journal of Personality and Social Psychology*, 27, 100–108
- Fischer, P., et al. (2011). The bystander effect: A meta-analytic review. *Psychological Bulletin*, 137, 517–537
