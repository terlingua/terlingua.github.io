---
title: 'Applied Research: Why Interventions Must Be Tested'
questions:
- id: why-control
  prompt: A company rolls out a mandatory "resilience workshop" for engineers after incidents and later surveys them; most say it helped. Why is that weak evidence, according to SPA 1?
  explanation: 'The book warns that “It is difficult to test the effectiveness of an intervention without a randomly assigned control group, and failing to conduct such tests can have serious consequences” (p. 409). People are often resilient on their own (SPA 2), so improvement after an intervention does not show the intervention caused it. Self-reports are vulnerable to effort justification (chapter 4) and demand effects. The fix is the same as for product features: randomize, compare, then scale.'
  options:
  - text: There was no control group
    correct: true
  - text: Because workshops never work
  - text: Because engineers cannot evaluate workshops
  - text: Surveys are always accurate
- id: cisd
  prompt: What did rigorous randomized studies find about Critical Incident Stress Debriefing (CISD)?
  explanation: McNally and colleagues concluded there is “no convincing evidence” that psychological debriefing prevents PTSD and that “some evidence suggests that it [CISD] may impede natural recovery,” recommending that “for scientific and ethical reasons, professionals should cease compulsory debriefing of trauma-exposed people” (pp. 409–410). In one study, burn victims randomly assigned to CISD had higher PTSD, anxiety and depression 13 months later (Carlier et al., 2000). "Seems like" and "really is" are not the same thing.
  options:
  - text: It had no effects of any kind
  - text: No convincing benefit, and possible harm
    correct: true
  - text: It dramatically reduced PTSD in all studies
  - text: It was never tested
- id: cisd-why-harm
  prompt: Why might CISD have done harm, according to the book?
  explanation: The book explains that right after a traumatic event “may not be the best time to focus on the event and discuss it with others,” that people “are often quite resilient when left alone,” and that “Forcing people to talk about and relive traumatic experiences may make them more likely to remember those experiences later” (p. 410). Later, with distance, writing about trauma can help (SPA 2). Timing and voluntariness matter, which is relevant to how organizations handle post-incident support for on-call staff.
  options:
  - text: Because talking about trauma is always harmful
  - text: Because participants lied
  - text: Because counselors were poorly paid
  - text: Reliving trauma right away may hinder recovery
    correct: true
- id: strengths
  prompt: What two strengths does the book say social psychologists bring to applied problems? Select all that apply.
  type: multiple
  explanation: 'The book lists both: “the field of social psychology is a rich source of theories about human behavior” and “social psychologists know how to perform rigorous experimental tests of these solutions to see if they work” (p. 410). Theory without testing produced CISD; testing without theory is slow trial and error.'
  options:
  - text: Expertise in performing rigorous experimental tests of whether solutions work
    correct: true
  - text: A rich source of theories about human behavior from which to design solutions
    correct: true
  - text: The ability to skip testing because theories are reliable
  - text: Access to unlimited funding
- id: engineering-parallel
  prompt: Which practice in software organizations best mirrors the lesson of the CISD story?
  explanation: The CISD story is about scaling an intuitive intervention without evidence and causing harm. The engineering equivalent is mandating a new process, tool or policy organization-wide without a control group. Holdouts, staged rollouts and pre-registered success metrics (chapter 2) let you learn whether the change actually helps before it becomes entrenched and protected by justification of effort.
  options:
  - text: Measuring only the people who chose to participate
  - text: Relying on testimonials from enthusiastic early adopters
  - text: Rolling out a process to everyone because it "seems obviously good"
  - text: A controlled rollout with a holdout
    correct: true
---
## Learning objectives

- Explain why social psychology addresses both basic and applied questions
- Explain why interventions need randomized tests, using Critical Incident Stress Debriefing as an example
- Explain the two strengths social psychologists bring to applied problems

## Key ideas

Social psychology addresses both basic and applied questions (p. 408). It is "difficult to test the effectiveness of an intervention without a randomly assigned control group" (p. 409).

**Critical Incident Stress Debriefing (CISD)** (pp. 409–410): bringing trauma-exposed people together soon after an event to describe experiences and emotions was widely adopted, including by more than 9,000 counselors after September 11, before rigorous tests. Randomized trials found no convincing evidence it prevents PTSD, and in one study burn victims who received CISD had *higher* PTSD rates 13 months later (Carlier et al., 2000). McNally et al. (2003) recommended ending compulsory debriefing.

**Social psychology to the rescue** (p. 410): the field supplies theories to design solutions and the experimental methods to test them (intervention tournaments, chapter 2).

## Sources

- Aronson et al., *Social Psychology* 12e, Social Psychology in Action 1, pp. 405–410
- McNally, R. J., Bryant, R. A., & Ehlers, A. (2003). Does early psychological intervention promote recovery from posttraumatic stress? *Psychological Science in the Public Interest*, 4, 45–79
- Carlier, I. V. E., et al. (2000). Psychological debriefing and the treatment of burn victims. Discussed p. 410
- Wilson, T. D. (2011). *Redirect: The Surprising New Science of Psychological Change*. Little, Brown
