---
title: The Nature and Origin of Attitudes
questions:
- id: attitude-definition
  prompt: How does the book define attitudes?
  explanation: The glossary defines attitudes simply as “Evaluations of people, objects, and ideas” (glossary p. 499). They are enduring evaluations (liking or disliking) that can rest on beliefs, feelings or past behavior. "I like Kotlin," "I distrust microservices" and "I respect our SRE team" are all attitudes.
  options:
  - text: Beliefs that are always accurate
  - text: Emotions felt in the moment
  - text: Evaluations of people, objects and ideas
    correct: true
  - text: Behaviors repeated over time
- id: cognitive-affective
  prompt: Engineer A prefers Postgres after comparing benchmark results and feature tables. Engineer B prefers Rust because writing it "just feels elegant and right." Which kinds of attitudes are these?
  explanation: 'A cognitively based attitude is “An attitude based primarily on people’s beliefs about the properties of an attitude object”; an affectively based attitude is “An attitude based more on people’s feelings and values than on their beliefs about the nature of an attitude object” (glossary pp. 499–500). The distinction matters for persuasion (later sections): arguments about properties work better on cognitive attitudes; emotional appeals work better on affective ones. Arguing benchmarks with someone whose attitude is aesthetic often goes nowhere.'
  options:
  - text: A's is affectively based; B's is cognitively based
  - text: A's is cognitively based; B's is affectively based
    correct: true
  - text: Both are cognitively based
  - text: Both are behaviorally based
- id: classical-conditioning
  prompt: A team's on-call pager sound has been paired with stressful outages for months. Now engineers feel a jolt of dread whenever they hear any similar tone, even outside work. Which process created this response?
  explanation: Classical conditioning is “The phenomenon whereby a stimulus that elicits an emotional response is repeatedly paired with a neutral stimulus that does not, until the neutral stimulus takes on the emotional properties of the first stimulus” (glossary p. 499). The tone was neutral; paired with outages, it acquired their emotional charge. Affectively based attitudes toward tools, people and places often form this way, without anyone deciding anything.
  options:
  - text: Classical conditioning
    correct: true
  - text: Operant conditioning
  - text: Self-perception
  - text: Attitude inoculation
- id: operant-conditioning
  prompt: A junior developer who asks questions in team chat gets praised and fast help, so they ask more and come to like the team channel. Which process is this?
  explanation: Operant conditioning is “The phenomenon whereby behaviors we freely choose to perform become more or less frequent, depending on whether they are followed by a reward or punishment” (glossary p. 502). Rewarded behavior (asking) increases, and positive attitudes toward the setting follow. If questions were met with sarcasm, asking would decrease and the channel would be disliked. Team norms around question-asking are shaped this way.
  options:
  - text: Operant conditioning
    correct: true
  - text: The overjustification effect
  - text: Self-handicapping
  - text: Classical conditioning
- id: behavioral-attitude
  prompt: An engineer who was neutral about writing documentation notices they have been writing detailed READMEs for every project and concludes "I guess I like documenting." What kind of attitude is this?
  explanation: A behaviorally based attitude is “An attitude based on observations of how one behaves toward an object” (glossary p. 499). This is self-perception theory (chapter 4) applied to attitudes, and it works when initial attitudes are weak or ambiguous and the behavior was not obviously caused by external pressure (no one required the READMEs).
  options:
  - text: A behaviorally based attitude
    correct: true
  - text: A cognitively based attitude
  - text: An implicit attitude
  - text: An affectively based attitude formed by classical conditioning
- id: explicit-implicit
  prompt: What is the difference between explicit and implicit attitudes?
  explanation: 'Explicit attitudes are “Attitudes that we consciously endorse and can easily report”; implicit attitudes are “Attitudes that exist largely outside of conscious awareness” (glossary p. 500–501; section p. 171). The two can diverge: a manager may sincerely endorse equal treatment of all candidates (explicit) while holding implicit associations that favor some groups. Implicit attitudes tend to come from early and repeated experience and can be measured indirectly, for example with the Implicit Association Test (chapter 8).'
  options:
  - text: Implicit attitudes are always stronger
  - text: Explicit attitudes are conscious; implicit ones largely aren't
    correct: true
  - text: There is no difference
  - text: Explicit attitudes are about sex; implicit attitudes are about everything else
- id: multi-attitude-sources
  prompt: Which of these are sources of affectively based attitudes described in the chapter? Select all that apply.
  type: multiple
  explanation: Affectively based attitudes come from feelings and values rather than from an objective appraisal; sources include values, sensory or aesthetic reactions, and conditioning (section p. 169). Careful feature comparison produces cognitively based attitudes. The source predicts which persuasion strategy will work.
  options:
  - text: Careful comparison of documented features
  - text: Classical and operant conditioning
    correct: true
  - text: Sensory reactions, such as liking the look of a font
    correct: true
  - text: Values such as religious or moral beliefs
    correct: true
---
## Learning objectives

- Define attitudes
- Distinguish cognitively, affectively and behaviorally based attitudes
- Explain how classical and operant conditioning create affectively based attitudes
- Distinguish explicit and implicit attitudes and their different origins

## Key ideas

**Attitudes** are evaluations of people, objects and ideas (p. 166). They have cognitive, affective and behavioral components, and one usually dominates (pp. 168–170):

- **Cognitively based**: built on beliefs about properties (choosing a database from benchmarks)
- **Affectively based**: built on feelings and values (loving a language's aesthetics); often formed by **classical conditioning** (pairing with emotional stimuli) or **operant conditioning** (reward and punishment)
- **Behaviorally based**: inferred from one's own behavior, via self-perception (chapter 4), when initial attitudes are weak

**Explicit attitudes** are consciously endorsed and easily reported; **implicit attitudes** exist largely outside awareness, often formed by early or repeated experience, and are measured with tools like the IAT (p. 171).

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 7, pp. 166–173 (page ranges from the book's contents); glossary pp. 499–504
- Greenwald, A. G., McGhee, D. E., & Schwartz, J. L. K. (1998). Measuring individual differences in implicit cognition. *Journal of Personality and Social Psychology*, 74, 1464–1480
