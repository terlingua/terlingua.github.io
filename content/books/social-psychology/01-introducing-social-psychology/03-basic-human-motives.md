---
title: 'Where Construals Come From: Self-Esteem and Accuracy'
questions:
- id: two-motives
  prompt: According to the book, which two basic motives shape how people construe situations?
  explanation: 'The summary states that construal “is largely shaped by two basic human motives: the need to feel good about ourselves and the need to be accurate. At times these two motives tug in opposite directions” (p. 20). The interesting cases are the conflicts: an accurate view of a failed launch might show you made a bad call, which threatens your self-esteem, so you may construe the failure as bad luck or a weak team instead.'
  options:
  - text: Feeling good about ourselves and being accurate
    correct: true
  - text: The need for food and the need for safety
  - text: The need for power and the need for affiliation
  - text: The need for novelty and the need for consistency
- id: self-esteem-definition
  prompt: What does the book mean by self-esteem?
  explanation: 'The glossary defines self-esteem as “People’s evaluations of their own self-worth—that is, the extent to which they view themselves as good, competent, and decent” (p. 14, glossary p. 503). The book argues most people have a strong need to maintain it, and that this need can lead them to distort their perception of the world. Note the distinction: self-esteem is evaluative ("am I good?"), while the self-concept (chapter 4) is descriptive ("what am I like?").'
  options:
  - text: The accuracy of a person's beliefs about their abilities
  - text: A stable personality trait measured by IQ
  - text: How much other people respect a person
  - text: People's evaluation of their own worth as good, competent and decent
    correct: true
- id: failure-to-learn
  prompt: A tech lead's third rewrite of the billing service fails. They blame the vendor's API and the team's inexperience, and start a fourth rewrite with the same design. Which motive best explains the failure to learn?
  explanation: The book notes that people often distort the world to preserve self-esteem, and that “Acknowledging our deficiencies is difficult, even when the cost is failing to learn from our mistakes” (p. 15). Its example is someone who blames the ex-spouse for a divorce and then repeats the same problems in the next relationship. Externalizing blame protects the self but blocks accurate learning. Structures that separate the person's worth from the design's quality (design reviews that critique the proposal, not the author; blameless postmortems) make accuracy less threatening.
  options:
  - text: Social loafing
  - text: The need for novelty
  - text: The accuracy motive, because the vendor really is at fault
  - text: The self-esteem motive
    correct: true
- id: hazing
  prompt: In Aronson and Mills's classic study, women who went through a severe initiation to join a discussion group, which turned out to be dull, rated the group how, compared with women who went through a mild initiation or none?
  explanation: 'The book uses the example of Oscar, who loves his fraternity after a harsh hazing, and explains that personality psychologists might look for traits, but social psychologists point to self-justification: admitting you suffered for something worthless would threaten self-esteem, so you come to value it more (p. 15). Aronson and Mills (1959) tested this experimentally: severe initiation led to *higher* ratings of a deliberately dull group. Chapter 4 names the effect justification of effort: “The tendency for individuals to increase their liking for something they have worked hard to attain, especially if they have freely chosen to exert that effort” (glossary p. 501).'
  options:
  - text: As more interesting and valuable
    correct: true
  - text: They refused to rate it at all
  - text: As more boring, because they were angry about the initiation
  - text: Exactly the same as the others
- id: hard-won-framework
  prompt: A team spent eight painful months migrating to an in-house framework. A new hire points out that an off-the-shelf library does the same job better, and the team defends the framework fiercely. What does the suffering-and-self-justification research predict?
  explanation: 'The more people suffer to get something, the more they convince themselves it was worth it (p. 15). This is not stupidity; it is a self-esteem-protecting construal. Knowing the mechanism helps: frame the evaluation around future costs and benefits rather than past effort, and make it psychologically safe to say "we learned a lot, and now the right call is different." That reduces the self-esteem threat that drives the defense.'
  options:
  - text: Effort has no effect on how people evaluate outcomes
  - text: Their invested effort makes them value it more than it merits
    correct: true
  - text: The team will value the framework less because the pain reminds them of the cost
  - text: The team will readily switch because accuracy always wins
- id: social-cognition-definition
  prompt: The book defines social cognition as...
  explanation: 'The glossary definition: “How people think about themselves and the social world; more specifically, how people select, interpret, remember, and use social information to make judgments and decisions” (p. 15, glossary p. 503). The accuracy motive views people as trying to understand the world correctly, but the summary adds that “individuals typically act on the basis of incomplete and inaccurately interpreted information” (p. 20). Chapter 5 is all about the shortcuts people use and where they fail.'
  options:
  - text: How people learn behaviors through rewards
  - text: The neurological basis of social behavior
  - text: The study of how groups make decisions
  - text: How people think about themselves and the social world
    correct: true
- id: motives-conflict
  prompt: In which situations are the self-esteem motive and the accuracy motive most likely to pull in opposite directions? Select all that apply.
  type: multiple
  explanation: The two motives conflict when the accurate conclusion would make you look bad to yourself (p. 13–15). Data about your own feature, feedback on your own work, and responsibility for your own mistakes all threaten self-esteem, so expect motivated reasoning there. The weather forecast carries no self-esteem stakes, so accuracy dominates. Engineering practices that pre-commit to success criteria before seeing the data (written hypotheses, pre-registered experiment metrics) help because they constrain motivated interpretation after the fact.
  options:
  - text: Reviewing a dashboard showing that the feature you championed reduced conversion
    correct: true
  - text: Deciding whether the outage you caused was due to your decision or to bad luck
    correct: true
  - text: Reading a performance review that rates your code quality below what you expected
    correct: true
  - text: Checking the weather forecast before a hike
- id: culture-identity
  prompt: Why does the book give culture and social identity a central place in the discussion of construal?
  explanation: 'The summary notes that an important question is “the extent to which social psychological laws are universal, as well as cultural differences in these laws and how they are expressed” (p. 20). Culture shapes construal itself: whether standing out is admirable or embarrassing, whether giving unsolicited advice is helpful or intrusive (chapter 3). Social identity, the part of the self-concept based on group memberships (glossary p. 503), affects how people interpret events involving their groups. For distributed engineering teams, this means the same review comment or meeting format can be construed very differently across offices.'
  options:
  - text: Because culture is only relevant to anthropologists
  - text: Because culture and group membership shape what people notice and value
    correct: true
  - text: Because culture determines personality at birth
  - text: Because cultural differences have been shown to be illusory
---
## Learning objectives

- Name the two basic motives that shape construal
- Explain how the self-esteem motive distorts perception, including suffering and self-justification
- Explain the social cognition (accuracy) motive and why it still produces errors
- Explain why culture and social identity are central to construal

## Key ideas

Two motives shape how we construe situations (p. 13): the **need to feel good about ourselves** (self-esteem) and the **need to be accurate** (social cognition). They usually point the same way, but when an accurate view would threaten self-esteem, people often distort reality to protect themselves (p. 14).

**Self-esteem** is people's evaluation of their own self-worth: how good, competent and decent they see themselves (p. 14). It explains why we blame circumstances for our failures and why we fail to learn from them.

**Suffering and self-justification** (p. 15): after a severe initiation (hazing), people like the group *more*, because admitting that they suffered for something worthless would threaten self-esteem. Aronson and Mills (1959) showed this experimentally; chapter 4 returns to it as the justification of effort.

**Social cognition** is how people select, interpret, remember and use social information to make judgments (p. 15). People try to be accurate but work with incomplete information and mental shortcuts.

**Culture and social identity** (pp. 16–18) shape both motives: what counts as "feeling good about yourself" and what counts as accurate differ across cultures and groups.

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 1, pp. 13–18
- Aronson, E., & Mills, J. (1959). The effect of severity of initiation on liking for a group. *Journal of Abnormal and Social Psychology*, 59, 177–181
