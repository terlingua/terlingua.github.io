---
title: 'Self-Consistency: Cognitive Dissonance, Decisions and Effort'
questions:
- id: dissonance-definition
  prompt: Which definition matches the book's definition of cognitive dissonance?
  explanation: 'The glossary defines cognitive dissonance as “The discomfort that people feel when two cognitions (beliefs, attitudes) conflict, or when they behave in ways that threaten their self-esteem” (glossary p. 499; section p. 97). Aronson''s refinement, the second clause, is central: dissonance is most painful when behavior conflicts with seeing ourselves as good, competent and sensible. "I''m a careful engineer" plus "I shipped without tests and it broke production" is a classic dissonance pairing.'
  options:
  - text: Discomfort from conflicting cognitions or self-threatening acts
    correct: true
  - text: A disagreement between two people about facts
  - text: The tendency to see one's views as superior
  - text: The anxiety caused by uncertainty about the future
- id: three-ways
  prompt: An engineer believes "I care about security" but has been committing API keys to the repository. Which are ways to reduce the resulting dissonance? Select all that apply.
  type: multiple
  explanation: 'The book describes three routes: change your behavior to fit the cognition, justify the behavior by changing a conflicting cognition, or justify it by adding new cognitions (section p. 97). Only the first fixes the actual problem; the other two make you feel better while the keys stay in the repo. The chapter''s lesson is that people are not rational animals but rationalizing animals: dissonance reduction aims at feeling consistent, not at being right.'
  options:
  - text: 'Change the behavior: start using a secrets manager'
    correct: true
  - text: 'Change a cognition: decide that "these keys aren''t really sensitive"'
    correct: true
  - text: 'Add a new cognition: "everyone does it, and the repo is private anyway"'
    correct: true
  - text: Ignore the conflict without any change in belief or behavior, which leaves dissonance unchanged
- id: self-esteem-threat
  prompt: Why is dissonance strongest when behavior threatens our self-concept?
  explanation: 'Dissonance includes behaving “in ways that threaten their self-esteem” (glossary p. 499). Doing something foolish, cruel or incompetent clashes with the core belief "I''m a decent, capable person," and that is where people work hardest to justify themselves. It also explains why smart people defend bad decisions: the smarter you think you are, the more threatening it is to admit a mistake.'
  options:
  - text: Because dissonance only arises from conflicting facts
  - text: Because people see themselves as good and sensible
    correct: true
  - text: Because people with low self-esteem never feel dissonance
  - text: Because the self-concept is irrelevant to dissonance
- id: postdecision
  prompt: After a long debate, a team chooses PostgreSQL over a document database by a narrow margin. A month later, everyone on the team describes the document database as clearly inferior. What explains the shift?
  explanation: 'Postdecision dissonance is “Dissonance aroused after making a decision, typically reduced by enhancing the attractiveness of the chosen alternative and devaluating the rejected alternatives” (glossary p. 502; section p. 99). Brehm (1956) had women rate appliances, choose between two equally rated ones, and rate them again: the chosen one rose and the rejected one fell. It feels like learning, but no new information arrived. Keep the original decision record: it shows how close the call really was.'
  options:
  - text: New benchmark data showed the document database was worse
  - text: Group polarization during the original debate
  - text: Postdecision dissonance
    correct: true
  - text: The halo effect
- id: irrevocable
  prompt: Knox and Inkster (1968) interviewed bettors at a racetrack just before and just after they placed their bets. What did they find, and why?
  explanation: 'Once the bet is placed it cannot be undone, so people reduce dissonance by becoming more certain they chose well. The book uses this to show that irrevocability intensifies dissonance reduction. It cuts both ways: one-way-door decisions (deleting data, signing a long contract) lock in confidence that may not be justified, so evaluate them carefully *before* committing, when you can still think clearly.'
  options:
  - text: Bettors were more confident about their horse right after betting
    correct: true
  - text: There was no difference
  - text: Bettors were less confident after betting because they worried about losing
  - text: Only experienced bettors changed their confidence
- id: lowballing
  prompt: A vendor quotes a low price; after the team commits internally and starts integration, the vendor "discovers" extra fees. The team pays anyway. Which dissonance-related tactic is this?
  explanation: 'The book discusses lowballing as an application of postdecision dissonance: after customers commit, they build up reasons the choice was right, and those reasons remain when the attractive terms are removed. The commitment, plus the effort already invested in integration, makes reversing feel like admitting a mistake. A defense is to re-ask the question from scratch: "If we were choosing today, at this price, would we pick it?"'
  options:
  - text: The door-in-the-face technique
  - text: Lowballing
    correct: true
  - text: Self-handicapping
  - text: Attitude inoculation
- id: justification-of-effort
  prompt: Which definition matches the justification of effort?
  explanation: 'The glossary defines justification of effort as “The tendency for individuals to increase their liking for something they have worked hard to attain, especially if they have freely chosen to exert that effort” (glossary p. 501; section p. 100). Aronson and Mills (1959) found that a harder initiation led to liking a dull group more. "Freely chosen" matters: if you were forced, the force justifies the effort and no dissonance arises. Grueling, voluntary interview loops may make hires like the company more for having survived them.'
  options:
  - text: The tendency to justify others' effort by praising them
  - text: Liking something more because you freely worked hard for it
    correct: true
  - text: The tendency to reduce effort in groups
  - text: The tendency to work harder when others are watching
- id: effort-tooling
  prompt: Which situation is the justification of effort most likely to distort?
  explanation: 'The effect is strongest when effort is high and freely chosen. A self-built system that took two years of voluntary struggle invites justification: admitting it is worse than an off-the-shelf tool would imply the effort was wasted. A forced adoption provides external justification ("we had no choice"), so less dissonance arises. Bring in people who did not invest the effort to evaluate the system.'
  options:
  - text: Evaluating a tool that leadership forced the team to adopt
  - text: A home-grown build system the team fought for
    correct: true
  - text: Evaluating an API whose design nobody on the team was involved in
  - text: Evaluating a free SaaS tool adopted in an afternoon
---
## Learning objectives

- Define cognitive dissonance and explain the three ways to reduce it
- Explain why dissonance is strongest when behavior threatens self-esteem
- Explain postdecision dissonance and the role of irrevocability
- Explain the justification of effort

## Key ideas

**Cognitive dissonance** (Festinger, 1957; p. 97): discomfort when two cognitions conflict, or when behavior threatens self-esteem. Reduce it by (1) changing behavior, (2) changing a cognition to justify the behavior, or (3) adding new cognitions. People reduce dissonance in self-justifying ways, not necessarily rational ones.

**Decisions** (p. 99): after choosing between close alternatives, people experience **postdecision dissonance** and spread the alternatives apart: the chosen option looks better, the rejected worse (Brehm, 1956). The effect is stronger when decisions are irrevocable; bettors are more confident just after placing a bet (Knox & Inkster, 1968). Salespeople exploit this with lowballing.

**Justification of effort** (p. 100): people come to like what they suffered for, especially when the effort was freely chosen (Aronson & Mills, 1959).

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 4, pp. 97–102
- Festinger, L. (1957). *A Theory of Cognitive Dissonance*. Stanford University Press
- Brehm, J. W. (1956). Postdecision changes in the desirability of alternatives. *Journal of Abnormal and Social Psychology*, 52, 384–389
- Knox, R. E., & Inkster, J. A. (1968). Postdecision dissonance at post time. *Journal of Personality and Social Psychology*, 8, 319–323
