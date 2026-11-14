---
title: 'Individual Behavior in a Group Setting: Facilitation, Loafing and Deindividuation'
questions:
- id: social-facilitation
  prompt: A senior engineer types faster and more accurately when pair programming on familiar CRUD code, but a junior engineer makes more mistakes when watched while learning a new framework. What explains both?
  explanation: Social facilitation is “When people are in the presence of others and their individual performance can be evaluated, the tendency to perform better on simple tasks and worse on complex tasks” (glossary p. 503; section p. 238). Zajonc explained that arousal strengthens the dominant, most likely response. For a well-practiced task the dominant response is correct; for a new task it is often wrong. Live-coding interviews can therefore underestimate candidates on unfamiliar problems.
  options:
  - text: Social facilitation
    correct: true
  - text: Group polarization
  - text: Social loafing
  - text: Deindividuation
- id: dominant-response
  prompt: According to Zajonc, why does arousal help some tasks and hurt others?
  explanation: 'The section on arousal and the dominant response (p. 240) explains the account. Zajonc even showed it in cockroaches, which ran a simple maze faster with an audience of other cockroaches and a complex maze slower. The practical rule: rehearse until the right action is the dominant one, then performance under pressure improves. That is why incident runbooks and game days matter: they make the correct responses dominant.'
  options:
  - text: Arousal always improves performance
  - text: Arousal only affects physical tasks
  - text: Arousal always hurts performance
  - text: Arousal strengthens the most practiced response
    correct: true
- id: why-arousal
  prompt: Why does the presence of others cause arousal, according to the chapter? Select all that apply.
  type: multiple
  explanation: The section "Why the Presence of Others Causes Arousal" (p. 240) lists alertness, evaluation apprehension and distraction. All three can operate at once. Screen-sharing your code to a large audience triggers evaluation apprehension and distraction, which is why debugging in front of many people feels harder than alone.
  options:
  - text: Others make us more alert and vigilant
    correct: true
  - text: We worry about how others are evaluating us (evaluation apprehension)
    correct: true
  - text: Others are distracting, creating conflict between attending to them and to the task
    correct: true
  - text: Others always help us perform the task
- id: social-loafing
  prompt: In a 12-person code review where nobody's comments are attributed and only the overall approval is recorded, reviewers skim and miss obvious bugs. Which phenomenon is this?
  explanation: 'Social loafing is “When people are in the presence of others and their individual performance cannot be evaluated, the tendency to perform worse on simple or unimportant tasks but better on complex or important tasks” (glossary p. 503; section p. 242). With many reviewers and no individual accountability, each feels less responsible ("someone else will catch it"). Fixes: fewer reviewers, named owners for specific areas, and making each reviewer''s contribution visible.'
  options:
  - text: Social facilitation
  - text: Social loafing
    correct: true
  - text: Deindividuation
  - text: Groupthink
- id: facilitation-vs-loafing
  prompt: What determines whether the presence of others leads to social facilitation or social loafing?
  explanation: 'The chapter''s integrative model (pp. 238–242) says evaluation is the key: identifiable performance raises arousal; pooled performance lowers it. Then task difficulty determines the outcome: higher arousal helps simple tasks and hurts complex ones; lower arousal hurts simple tasks and helps complex ones. Notice the glossary''s phrasing: loafers do *better* on complex tasks, because the anxiety is gone.'
  options:
  - text: The time of day
  - text: Whether individual performance can be evaluated
    correct: true
  - text: Whether the task is physical
  - text: The size of the room
- id: reduce-loafing
  prompt: Which change would most reduce social loafing on a team's shared on-call runbook maintenance?
  explanation: Research on social loafing (meta-analysis by Karau & Williams, 1993) shows it decreases when individual contributions are identifiable, the task matters to people, groups are smaller, and members care about the group. Named ownership and visible contributions restore evaluation. The chapter also notes loafing is stronger among men and in individualist cultures, and weaker in collectivist cultures.
  options:
  - text: Removing any deadlines
  - text: Making all contributions anonymous
  - text: Adding more people to the maintenance rotation with no ownership
  - text: Named owners with visible contributions
    correct: true
- id: deindividuation
  prompt: What is deindividuation?
  explanation: Deindividuation is “The loosening of normal constraints on behavior when people can’t be identified (such as when they are in a crowd)” (glossary p. 500; section p. 243). It makes people feel less accountable and increases obedience to the group's norms, whatever they are. Crowds can become violent, but anonymity can also increase prosocial behavior if the group's norm is prosocial.
  options:
  - text: Losing one's job
  - text: Separating from a group
  - text: Becoming more individualistic
  - text: Loosened restraint when people can't be identified
    correct: true
- id: deindividuation-online
  prompt: Why do anonymous comment sections and some issue trackers attract hostile comments that people would never make face to face?
  explanation: 'The section on deindividuation online (p. 243) applies the concept to digital settings: when people feel unidentifiable and distant from those affected, normal restraints weaken. Design choices matter: real identities or persistent reputations, visible community norms, and moderation make people feel identifiable and highlight prosocial norms. Code-of-conduct enforcement in open source projects partly works this way.'
  options:
  - text: Because of the overjustification effect
  - text: Online anonymity produces deindividuation
    correct: true
  - text: Because of social facilitation
  - text: Online users have more aggressive personalities
- id: deindividuation-norms
  prompt: Research shows deindividuation increases behavior consistent with what?
  explanation: Deindividuation makes people less guided by personal standards and more by the situation's norms (Postmes & Spears, 1998). A masked crowd at a riot becomes more violent; anonymous people in a context with charitable norms can become more generous. The design lesson is to make the right norms salient where people are anonymous.
  options:
  - text: The situation's norms
    correct: true
  - text: Random behavior
  - text: The person's private values
  - text: Only aggressive behavior
---
## Learning objectives

- Explain social facilitation and Zajonc's dominant-response account
- Explain why the presence of others causes arousal
- Explain social loafing and how to reduce it
- Explain the integrated model: evaluation plus arousal predicts facilitation versus loafing
- Explain deindividuation, including online

## Key ideas

**Social facilitation** (p. 238): when individual performance can be evaluated, the presence of others improves performance on simple tasks and impairs it on difficult tasks. Zajonc: others cause **arousal**, which strengthens the **dominant response** (correct on well-learned tasks, incorrect on new ones; p. 240). Why arousal (p. 240): others make us more alert, cause **evaluation apprehension**, and distract us.

**Social loafing** (p. 242): when individual performance cannot be evaluated, people relax: worse on simple tasks, but better on complex tasks (less anxiety). Loafing is reduced by identifiability, meaningful tasks, small groups and caring about the group; it is stronger in men and in individualist cultures.

**Deindividuation** (p. 243): loosening of normal constraints when people cannot be identified, as in crowds and masks. It increases behavior consistent with the group's norms, and occurs online, where anonymity and distance foster trolling and harassment (p. 243).

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 9, pp. 238–244
- Zajonc, R. B. (1965). Social facilitation. *Science*, 149, 269–274
- Karau, S. J., & Williams, K. D. (1993). Social loafing: A meta-analytic review. *Journal of Personality and Social Psychology*, 65, 681–706
- Postmes, T., & Spears, R. (1998). Deindividuation and antinormative behavior. *Psychological Bulletin*, 123, 238–259
