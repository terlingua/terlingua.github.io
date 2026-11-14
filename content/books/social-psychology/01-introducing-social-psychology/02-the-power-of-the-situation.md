---
title: The Power of the Situation and the Importance of Construal
questions:
- id: fae-definition
  prompt: The book calls the fundamental attribution error a "formidable barrier" for social psychologists. What is it?
  explanation: 'The glossary defines the fundamental attribution error as “The tendency to overestimate the extent to which people’s behavior is due to internal, dispositional factors and to underestimate the role of situational factors” (p. 9, glossary p. 501). It is a barrier because people resist the core finding of social psychology, that situations are often more powerful than personalities. Writing off people who behave badly as "bad people" gives us a comforting but false sense that it could never happen to us. Chapter 6 covers the error in depth (why it happens: perceptual salience and the two-step attribution process).'
  options:
  - text: The tendency to overestimate how much other people agree with us
  - text: The tendency to explain everything by luck
  - text: The tendency to remember failures more than successes
  - text: Overestimating dispositional causes and underestimating situational ones
    correct: true
- id: blameless-postmortem
  prompt: After an outage caused by a junior engineer running a migration against production, the first draft of the postmortem says "the engineer was careless." A blameless-postmortem culture would push the team to ask which question instead?
  explanation: '"The engineer was careless" is a textbook fundamental attribution error: a dispositional explanation that ignores the situation (p. 9). The social psychological question is what features of the situation would lead most people to do the same thing: production credentials on a laptop, a runbook that does not distinguish environments, a deadline, a habit of everyone else doing it this way. Blameless postmortems institutionalize the book''s central lesson that “Aspects of the social situation that may seem minor can overwhelm the differences in people’s personalities” (p. 11). Fixing the situation changes the behavior of everyone, not just one person.'
  options:
  - text: Should the engineer be removed from the on-call rotation?
  - text: How can we make sure engineers feel guilty enough to be careful next time?
  - text: Which personality test should we use when hiring to screen out careless people?
  - text: What made this action easy and likely for anyone in that role?
    correct: true
- id: wall-street-game
  prompt: In the Wall Street Game versus Community Game study, students labelled by their dorm advisors as "cooperative" or "competitive" played an identical game under one of two names. What happened?
  explanation: 'Liberman, Samuels and Ross (2004) gave everyone the same Prisoner''s Dilemma–style game. Calling it the Community Game led about two thirds of players to cooperate on the first round; calling it the Wall Street Game led about two thirds to compete (pp. 10–11). The advisors'' trait labels predicted nothing: “The students labeled competitive were no more likely to adopt the competitive strategy than those who were labeled cooperative” (p. 11). The name changed the *construal*: it told players what kind of situation this was and what norms applied. In software terms, calling a quarterly process "performance stack ranking" versus "team retrospective" can change behavior more than who is in the room.'
  options:
  - text: Students labelled competitive defected in both versions; students labelled cooperative cooperated in both
  - text: The game's name changed cooperation; the personality labels did not
    correct: true
  - text: Neither the label nor the name had any effect
  - text: Only students labelled cooperative were affected by the game's name
- id: naming-the-game
  prompt: A platform team renames its shared incident channel from "#blame-board" to "#learning-review." Based on the Wall Street versus Community Game study, why might this be more than cosmetic?
  explanation: 'The name of the game study shows that a minor situational cue, a label, can determine whether people cooperate or compete because it shapes construal: “the way in which people perceive, comprehend, and interpret the social world” (p. 11, glossary p. 500). A label tells people which norms apply (punish versus learn). It is not magic, and the label must match how the channel is actually used, but labels are part of the situation, and situations beat personalities more often than intuition suggests.'
  options:
  - text: Because the label frames what kind of situation it is
    correct: true
  - text: Because channel names affect search indexing
  - text: It is purely cosmetic; only incentives change behavior
  - text: Because people with competitive personalities avoid channels with soft names
- id: construal-definition
  prompt: What is "construal"?
  explanation: The book defines construal as “The way in which people perceive, comprehend, and interpret the social world” (p. 11, glossary p. 500). Social psychologists argue that to understand behavior you need to know how people construe a situation, not just its objective features. The same Slack message ("can we talk?") can be construed as a friendly check-in or a warning of bad news, and the construal, not the literal text, drives the response.
  options:
  - text: The process of building a theory from data
  - text: A personality trait that determines how agreeable someone is
  - text: The objective, measurable properties of a situation
  - text: How people perceive, comprehend and interpret the social world
    correct: true
- id: behaviorism-vs-gestalt
  prompt: Behaviorism and the Gestalt approach differ in what they treat as the cause of behavior. Which pairing is correct?
  explanation: 'Behaviorism is “A school of psychology maintaining that to understand human behavior, one need only consider the reinforcing properties of the environment” (p. 11, glossary p. 499). The Gestalt approach stresses “the importance of studying the subjective way in which an object appears in people’s minds rather than the objective, physical attributes of the object” (p. 12, glossary p. 501). Social psychology took the Gestalt side: two people can receive the same objective reward and construe it differently (chapter 4 shows how a reward can even reduce motivation). Kurt Lewin, a German Jewish émigré often called the founder of experimental social psychology, applied Gestalt ideas to the social world (p. 12).'
  options:
  - text: Both focus mainly on genes and hormones
  - text: 'Behaviorism: personality traits; Gestalt: social class'
  - text: 'Behaviorism: objective rewards; Gestalt: how things appear subjectively'
    correct: true
  - text: 'Behaviorism: subjective perception; Gestalt: rewards and punishments'
- id: naive-realism
  prompt: Two teams argue about whether to adopt microservices. Each side believes it is seeing the evidence objectively and that the other side is biased by its own interests. What does the book call the belief each side holds?
  explanation: 'Naïve realism is “The conviction that we perceive things ‘as they really are,’ underestimating how much we are interpreting or ‘spinning’ what we see” (p. 12, glossary p. 502). Lee Ross found that negotiations often run aground because each side assumes reasonable people see things its way, and “even when each side recognizes that the other side perceives the issues differently ... each thinks that the other side is biased” (p. 13). The fix is not to assume you are the neutral one: make construals explicit (write down assumptions, criteria and the evidence that would change your mind) so the disagreement is about interpretations rather than about who is biased.'
  options:
  - text: The overjustification effect
  - text: Groupthink
  - text: Naïve realism
    correct: true
  - text: Belief perseverance
- id: lewin-quote
  prompt: Which statement best captures why construal matters more than the objective situation?
  explanation: 'The duck/rabbit figure (p. 12) is the Gestalt illustration: the same lines can be seen two ways. Lewin extended this to social life: what matters is how the person perceives and interprets the situation. The Wall Street versus Community Game study shows the same thing in behavior. Chapter 2''s "wise interventions" are built on this idea: change how people interpret themselves or their situation and behavior changes in a self-sustaining way.'
  options:
  - text: Construal only matters for ambiguous optical illusions, not real social behavior
  - text: People respond to their interpretation, which can differ between people
    correct: true
  - text: Construal is fixed at birth and cannot be changed
  - text: People respond to the true probabilities of rewards in their environment
- id: multi-situational-forces
  prompt: An engineer who is normally meticulous skips the code review checklist during a release crunch. Which of the following are situational explanations of the kind the book says we tend to underweight? Select all that apply.
  type: multiple
  explanation: 'The first three options are features of the situation: incentives and praise (a reward structure), descriptive norms (what others are doing, chapter 10) and friction (small barriers, which the applied chapter on sustainability shows have big effects). "Secretly lazy" is a dispositional attribution. The fundamental attribution error predicts that observers will jump to that explanation and underestimate the other three (p. 9). The practical lesson: before attributing behavior to character, enumerate the situational forces that would push most people the same way.'
  options:
  - text: The release manager publicly praised whoever merged fastest that week
    correct: true
  - text: Everyone else on the team was skipping the checklist
    correct: true
  - text: The checklist lives in a wiki page that takes a minute to find
    correct: true
  - text: The engineer has always secretly been lazy
---
## Learning objectives

- Explain the fundamental attribution error and why it is a barrier to understanding behavior
- Describe the Wall Street Game versus Community Game study and what it shows about labels and situations
- Define construal and naïve realism, and explain why construal matters more than the objective situation
- Contrast behaviorism with the Gestalt approach and explain Kurt Lewin's contribution

## Key ideas

**Fundamental attribution error (FAE):** explaining behavior by personality while underestimating the situation (p. 9). Explaining the Jonestown deaths or a bad deploy as "flawed people" feels safe, because it implies it could never happen to us, which is exactly the false security the book warns about.

**Wall Street versus Community Game** (Liberman, Samuels & Ross, 2004; pp. 10–11): dorm advisors labelled students as cooperative or competitive. Students then played the same Prisoner's Dilemma–type game, called either the Wall Street Game or the Community Game. The *name of the game* predicted behavior strongly; the *personality label* did not.

**Construal** is how people perceive, comprehend and interpret the social world (p. 11). Behaviorism looked at the objective rewarding properties of situations; the Gestalt approach, brought to social psychology by Kurt Lewin, emphasizes the subjective way the situation appears in people's minds (p. 12).

**Naïve realism** is the conviction that we see things "as they really are" (p. 12). Lee Ross found it derails negotiations: each side thinks the other side is the biased one (p. 13).

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 1, pp. 9–13
- Liberman, V., Samuels, S. M., & Ross, L. (2004). The name of the game: Predictive power of reputations versus situational labels in determining Prisoner's Dilemma game moves. *Personality and Social Psychology Bulletin*, 30, 1175–1185
- Lewin, K. (1951). *Field theory in social science*. Harper
