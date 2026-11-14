---
title: Causal Attribution and the Covariation Model
questions:
- id: attribution-theory
  prompt: What is attribution theory?
  explanation: The glossary defines attribution theory as “A description of the way in which people explain the causes of their own and other people’s behavior” (glossary p. 499; section p. 149). Fritz Heider argued that people are like naive scientists trying to explain behavior, and that the main distinction is between explanations in terms of the person and explanations in terms of the situation. Postmortems are formal attribution exercises.
  options:
  - text: A theory of persuasion
  - text: A theory about how groups make decisions
  - text: An account of how people explain behavior
    correct: true
  - text: A theory about how people credit authors in citations
- id: internal-external
  prompt: A teammate misses a standup. "She's disorganized" versus "her train was delayed." Which is which?
  explanation: 'An internal attribution is “The inference that a person is behaving in a certain way because of something about the person, such as attitude, character, or personality”; an external attribution is “The inference that a person is behaving a certain way because of something about the situation they are in, with the assumption that most people would respond the same way in that situation” (glossary pp. 500–501). The choice matters: internal attributions lead to judging people; external ones lead to fixing situations.'
  options:
  - text: Both are external
  - text: '"She''s disorganized" is external; "her train was delayed" is internal'
  - text: Both are internal
  - text: The first is internal; the second is external
    correct: true
- id: covariation
  prompt: What does the covariation model propose?
  explanation: 'The covariation model is “A theory that states that to form an attribution about what caused a person’s behavior, we note the pattern between when the behavior occurs and the presence or absence of possible causal factors” (glossary p. 500; section p. 149). Kelley modeled the perceiver as an analyst looking for what varies with what. Engineers do exactly this when debugging: does the error co-occur with this host, this user, this time of day?'
  options:
  - text: People infer causes from what covaries with the behavior
    correct: true
  - text: People avoid explaining behavior
  - text: People attribute behavior to whatever is most visible
  - text: People always attribute behavior to personality
- id: consensus
  prompt: Which question asks about consensus information?
  explanation: Consensus information is “The extent to which other people behave the same way toward the same stimulus as the actor does” (glossary p. 500). Distinctiveness information is “The extent to which a particular actor behaves in the same way toward different stimuli,” and consistency information is “The extent to which the behavior between one actor and one stimulus is the same across time and circumstances” (glossary p. 500). Research suggests people underuse consensus, which is one reason they over-attribute behavior to personality.
  options:
  - text: Do other people behave the same way toward this same stimulus?
    correct: true
  - text: Does this person behave the same way toward this stimulus at other times?
  - text: Does this person like the stimulus?
  - text: Does this person behave the same way toward other stimuli?
- id: flaky-test-internal
  prompt: A CI job fails. Only this one test fails (no other tests fail on the same commit), it fails on every developer's machine, and it fails on every run. Treating the test as the "actor" and the commit as the "stimulus," which conclusion fits the covariation model?
  explanation: 'Mapping Kelley''s logic onto debugging: low consensus (no other "actors" respond the same way to the stimulus), low distinctiveness (the test fails across different "stimuli" such as environments) and high consistency (it fails every time) point to something about the actor, here the test itself. This analogy is ours, not the book''s, but it shows the model is just a structured way of asking what covaries with the effect, the same reasoning used in systematic debugging.'
  options:
  - text: The cause is random, because consistency is low
  - text: The commit is the cause, because consensus is high
  - text: Something about the test itself is the cause
    correct: true
  - text: The CI runner is the cause
- id: covariation-external
  prompt: Pat yells at their manager. Everyone else also yells at this manager (high consensus). Pat doesn't yell at anyone else (high distinctiveness). Pat has yelled at this manager on many occasions (high consistency). What attribution does the covariation model predict?
  explanation: 'High consensus, high distinctiveness and high consistency together point to an external cause, the stimulus (section pp. 149–151). Compare: if nobody else yelled at the manager (low consensus), Pat yelled at everyone (low distinctiveness), and Pat always yelled at the manager (high consistency), the model predicts an internal attribution: Pat is the problem. When consistency is low, people attribute the behavior to unusual circumstances.'
  options:
  - text: 'External: something about the manager'
    correct: true
  - text: 'Internal: Pat is an angry person'
  - text: Something unusual about this particular day
  - text: No attribution can be made
- id: low-consistency
  prompt: A usually reliable engineer, who has never missed a deadline before, misses one this week. According to the covariation model, which attribution is most likely?
  explanation: When behavior is low in consistency, the person does not usually act this way in this situation, so people tend to attribute it to something special about the circumstances (an illness, a family emergency, an unexpected blocker) rather than to the person or the stimulus. This is a reasonable inference and a good default for one-off events. Unfortunately, as the next section shows, people often skip this reasoning and jump to personality.
  options:
  - text: 'An internal attribution: the engineer is unreliable'
  - text: An external attribution to the project itself
  - text: No attribution is possible
  - text: Something unusual about the circumstances this time
    correct: true
- id: covariation-limits
  prompt: Which limitations of the covariation model does research identify? Select all that apply.
  type: multiple
  explanation: 'People do use covariation, especially when they are motivated and have information, but real life rarely supplies all three kinds of data, and people under-weight how others behave. When time or attention is short, they default to quick, internal attributions, which leads into the fundamental attribution error. Good incident reviews supply the missing data deliberately: did other services fail? Did this service fail under other conditions? Has this happened before?'
  options:
  - text: People never use covariation information at all
  - text: People often make attributions quickly and automatically rather than through careful analysis
    correct: true
  - text: People tend to underuse consensus information
    correct: true
  - text: People often lack information about consensus, distinctiveness or consistency
    correct: true
---
## Learning objectives

- Define attribution theory and distinguish internal and external attributions
- Explain Kelley's covariation model and its three types of information
- Apply consensus, distinctiveness and consistency to predict internal versus external attributions

## Key ideas

**Attribution theory** (Heider, 1958; p. 149): how people explain the causes of their own and others' behavior. **Internal attribution**: something about the person; **external attribution**: something about the situation.

**Covariation model** (Kelley, 1967; p. 149): we note the pattern between when behavior occurs and the presence of possible causes, using:

- **Consensus**: do other people behave the same way toward this stimulus?
- **Distinctiveness**: does this person behave the same way toward other stimuli?
- **Consistency**: does this person behave the same way toward this stimulus across time and circumstances?

Low consensus + low distinctiveness + high consistency → internal attribution. High consensus + high distinctiveness + high consistency → external attribution. Low consistency → something unusual about the circumstances.

People do use covariation, but often lack information or underuse consensus.

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 6, pp. 148–152
- Heider, F. (1958). *The Psychology of Interpersonal Relations*. Wiley
- Kelley, H. H. (1967). Attribution theory in social psychology. *Nebraska Symposium on Motivation*, 15, 192–238
