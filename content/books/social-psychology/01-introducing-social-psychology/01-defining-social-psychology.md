---
title: Defining Social Psychology and Its Neighbors
questions:
- id: definition
  prompt: Which definition matches the book's definition of social psychology?
  explanation: 'The glossary defines social psychology as “The scientific study of the way in which people’s thoughts, feelings, and behaviors are influenced by the real or imagined presence of other people” (p. 4, glossary p. 503). Three parts matter. *Scientific*: claims are tested empirically, not settled by intuition. *Thoughts, feelings and behaviors*: all three are in scope, not just actions. *Real or imagined presence*: influence works even when others are only pictured, remembered or anticipated. The other options describe neighbouring fields: personality psychology (individual differences), sociology (groups and institutions) and evolutionary psychology (genetic factors shaped by selection).'
  options:
  - text: The study of how personality traits make individuals behave differently from one another
  - text: The study of how groups, institutions and societies shape the distribution of outcomes
  - text: The scientific study of how others, real or imagined, influence our thoughts, feelings and behavior
    correct: true
  - text: The study of how evolved genetic programs determine social behavior
- id: imagined-presence
  prompt: A developer working alone at 2 a.m. adds tests and renames variables because they picture how the senior reviewer will react to the pull request tomorrow. Does this count as social influence in the book's sense?
  explanation: 'The definition deliberately includes the *imagined* presence of other people (p. 4). Social influence is broader than direct pressure: we carry other people''s expectations, values and likely reactions around in our heads. The book also defines social influence as “The effect that the words, actions, or mere presence of other people has on our thoughts, feelings, attitudes, or behavior” (glossary p. 503). A developer anticipating a reviewer, a norm in a team''s `CONTRIBUTING.md`, or an imagined audience on a public repository all shape behavior without anyone in the room.'
  options:
  - text: No, social influence requires someone to be physically present
  - text: 'Yes'
    correct: true
  - text: No, this is an internal personality trait called conscientiousness
  - text: Only if the reviewer explicitly asked for tests beforehand
- id: proverbs
  prompt: Folk wisdom says both "birds of a feather flock together" and "opposites attract." What is the social psychologist's response to contradictory proverbs like these?
  explanation: 'The book argues that it is not enough to say all proverbs can be true; “Part of the job of the social psychologist is to do the research that specifies the conditions under which one or another is most likely to take place” (pp. 5–6). This is the difference between folk wisdom and science: common sense is often right *after the fact*, but it rarely tells you in advance which of two opposite predictions applies. (In chapter 13 the answer for attraction turns out to be that similarity predicts liking far better than complementarity.) Engineers will recognize the pattern: "keep services small" and "avoid distributed monoliths" are both true; the useful knowledge is when each applies.'
  options:
  - text: Conclude that common sense is always wrong and discard both
  - text: Treat both as true at all times, since proverbs summarize centuries of experience
  - text: Pick the proverb that matches personal experience
  - text: Research the conditions under which each one holds
    correct: true
- id: levels-of-analysis
  prompt: A team is debating why engineers in one company stay silent in incident reviews. Match the explanation to social psychology's level of analysis.
  explanation: Table 1–1 (p. 9) contrasts the disciplines. Biology and neuroscience look at genes, hormones and physiology (the cortisol option). Personality psychology looks at what makes individuals unique (introversion). Sociology works at the level of groups, organizations and societies (the institution option). Social psychology studies “the psychological processes people have in common” that make them susceptible to social influence, so its level of analysis is “the individual within a group, institution, or society” (p. 8). The correct option explains behavior by how ordinary people construe a social situation, which is the social psychological answer.
  options:
  - text: '"Large corporations as institutions reward conformity through their promotion structures"'
  - text: '"Cortisol spikes during stressful meetings suppress speech"'
  - text: '"Engineers who stay silent have introverted personalities"'
  - text: '"With the VP present and everyone silent, people read silence as the safe choice"'
    correct: true
- id: sociology-difference
  prompt: What most clearly separates social psychology from sociology, according to the book?
  explanation: 'The book says “in sociology, the level of analysis is the group, institution, or society at large, whereas the level of analysis in social psychology is the individual within a group, institution, or society” (p. 8). Both fields care about topics like prejudice and violence. A sociologist might ask how social class predicts outcomes across a population; a social psychologist asks what psychological processes in individuals (construal, motives, norms) explain how people in a given situation respond. The analogy for engineers: sociology studies the system''s emergent metrics; social psychology studies how each component reacts to its inputs.'
  options:
  - text: Sociologists never run experiments, while social psychologists always do
  - text: Social psychology only studies small laboratory groups of students
  - text: Sociology studies groups and institutions; social psychology the individual in them
    correct: true
  - text: Sociology studies prejudice while social psychology does not
- id: evolutionary
  prompt: What is the main limitation the book raises about evolutionary psychology?
  explanation: 'The glossary defines evolutionary psychology as “The attempt to explain social behavior in terms of genetic factors that have evolved over time according to the principles of natural selection” (p. 6, glossary p. 500). The book values it for generating novel hypotheses but notes that such hypotheses are often “impossible to test with the experimental method. And just because hypotheses sound plausible does not mean they are true” (p. 7). Its example: the classic story that giraffes evolved long necks to reach high leaves may be wrong; some scientists think long necks evolved in males for fighting over mates (Simmons & Scheepers, 1996). Engineers know the same trap as the "just-so story" postmortem: a neat causal narrative that fits the facts but was never tested.'
  options:
  - text: It ignores genes entirely
  - text: It only applies to animals, not humans
  - text: Its plausible explanations are often hard to test
    correct: true
  - text: It has been proven false by modern genetics
- id: personality-vs-social
  prompt: Which question is a personality psychologist more likely to ask than a social psychologist?
  explanation: Personality psychologists study “the characteristics that make individuals unique and different from one another” (Table 1–1, p. 9). Social psychologists study what most people have in common, and how situations (the rotation's design, the number of others present, how a situation is construed) shape behavior. Both are legitimate; the book's argument is that people systematically overweight the personality explanation and underweight the situation. The next sections show that small situational changes often swamp trait differences.
  options:
  - text: How does the number of people in an alert channel change any one person's likelihood of responding?
  - text: What traits make some engineers consistently more likely to volunteer for on-call than others?
    correct: true
  - text: What features of an on-call rotation make most engineers more likely to respond quickly?
  - text: How does how an engineer interprets an ambiguous alert affect whether they act?
- id: multi-what-is-in-scope
  prompt: Which of these are within the scope of social psychology as the book defines it? Select all that apply.
  type: multiple
  explanation: The definition covers *thoughts, feelings and behaviors* influenced by others (p. 4), so feelings after a review, helping in a crowd (chapter 11's bystander effect) and belief change in groups (chapter 9's group polarization, chapter 10's conformity) are all in scope. Pure pharmacology is biology's level of analysis. Social psychologists may borrow biological measures (heart rate, cortisol, brain imaging) but the questions they ask are about social influence.
  options:
  - text: How an engineer's mood changes after reading a harsh code-review comment
    correct: true
  - text: Whether people help a colleague whose laptop is on fire in a crowded office
    correct: true
  - text: The molecular mechanism by which caffeine blocks adenosine receptors
  - text: How a remote worker's beliefs shift after a team discussion
    correct: true
---
## Learning objectives

- Define social psychology precisely, including the phrase "real or imagined presence of other people"
- Explain why social psychology is an empirical science rather than folk wisdom or philosophy
- Distinguish social psychology's level of analysis from biology, personality psychology and sociology
- Explain what evolutionary psychology contributes and why its claims are hard to test

## Key ideas

**Social psychology** is the scientific study of how people's thoughts, feelings and behaviors are influenced by the real or imagined presence of others (book p. 4). "Imagined" matters: a developer who writes cleaner code because they *picture* the reviewer is under social influence even when nobody is watching.

**Science, not proverbs.** Folk wisdom contains contradictory proverbs ("birds of a feather flock together" versus "opposites attract"; "absence makes the heart grow fonder" versus "out of sight, out of mind"). The social psychologist's job is to find out *under what conditions* each one holds (pp. 5–6), using empirical methods, especially experiments.

**Levels of analysis** (Table 1–1, p. 9):

| Discipline | Level of analysis |
|---|---|
| Biology and neuroscience | Genes, hormones, physiological processes |
| Personality psychology | Characteristics that make individuals unique |
| Social psychology | Psychological processes people have in common that make them susceptible to social influence; the individual in a social situation |
| Sociology | Groups, organizations, societies |

**Evolutionary psychology** explains social behavior with genetic factors shaped by natural selection (p. 6). It generates interesting hypotheses but many of its explanations are hard to test experimentally; the book's giraffe-neck example shows how a plausible story can be wrong (p. 7).

## Sources

- Aronson, Lewis, Page-Gould, Sommers & Wilson, *Social Psychology*, 12th ed. (Pearson, 2027), ch. 1, pp. 4–9; glossary pp. 499–504
- Simmons, R. E., & Scheepers, L. (1996). Winning by a neck: Sexual selection in the evolution of giraffe. *American Naturalist*, 148, 771–786
