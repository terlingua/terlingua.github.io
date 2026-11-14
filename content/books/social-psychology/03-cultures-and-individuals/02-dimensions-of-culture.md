---
title: The Dimensions of Culture, Tight and Loose Cultures, and Subcultures
questions:
- id: power-distance
  prompt: In one engineering organization, junior developers implement architecture decisions without question; in another, they expect to be consulted. Which cultural dimension does this difference most resemble?
  explanation: Power distance is “The cultural dimension that explains how societies deal with the problem of inequality” (p. 59, glossary p. 502). In large power-distance societies, both high- and low-status people accept unequal power as a fact of life, and “subordinates should expect to follow the orders of leaders without much input.” In small power-distance societies, subordinates still acknowledge hierarchy but “expect to be consulted and to have input” (p. 59). For distributed teams, this predicts that the same open "any objections?" in a design review will draw comments from one office and silence from another, and silence there does not mean agreement.
  options:
  - text: Long-term vs. short-term orientation
  - text: Uncertainty avoidance
  - text: Indulgence vs. restraint
  - text: Power distance
    correct: true
- id: uncertainty-avoidance
  prompt: A company requires a change-advisory-board approval, a 40-item checklist and a formal sign-off for every deploy, and punishes deviations. Another lets teams "take it as it comes" and tolerates experimentation. Which dimension does this contrast map to most closely?
  explanation: 'Uncertainty avoidance is “The cultural dimension that explains how societies respond to ambiguity and uncertainty” (p. 59, glossary p. 504). In strong uncertainty-avoidance cultures, uncertainty “is experienced as a continuous threat that must be managed,” through strict codes and rules that punish deviation; in weak ones, uncertainty is accepted and people are encouraged to “take life as it comes” (pp. 59–60). The analogy is loose but useful: change-management culture is partly an organization''s answer to how threatening it finds uncertainty.'
  options:
  - text: Individualism vs. collectivism
  - text: Uncertainty avoidance
    correct: true
  - text: Power distance
  - text: Masculinity–femininity
- id: ind-coll-level
  prompt: Why does the book say it is a mistake for someone to describe themselves as "an individualist" in Hofstede's sense?
  explanation: 'The book stresses that “individualism and collectivism are not features of individuals” but “societal-level variables—they represent the degree to which people in society are integrated into groups” (p. 60). The glossary defines the dimension as “The cultural dimension that explains the degree to which people in society are integrated into groups” (glossary p. 501). The individual-level close cousins are independent and interdependent orientations. Mixing up levels is a classic analysis error: a property of the aggregate need not hold for each member, just as a service''s mean latency says little about any single request.'
  options:
  - text: Because only collectivism can be measured
  - text: Because the dimension describes societies, not individuals
    correct: true
  - text: Because nobody is individualistic
  - text: Because the dimension was abandoned in the 1990s
- id: squeaky-wheel
  prompt: Which pair of sayings does the book use to capture individualistic versus collectivistic cultures?
  explanation: 'In individualistic cultures people focus on "I," and “the squeaky wheel gets the grease” emphasizes speaking up and standing out to wield influence. In collectivistic cultures people focus on "we," and “the nail that sticks out gets hammered down” emphasizes fitting in (pp. 60–61). The multinational team study later in the chapter shows a cost of the squeaky-wheel norm: people who talk more get seen as more competent even when they are not.'
  options:
  - text: '"Absence makes the heart grow fonder" versus "out of sight, out of mind"'
  - text: '"Squeaky wheel gets the grease" vs. "nail gets hammered down"'
    correct: true
  - text: '"Birds of a feather flock together" versus "opposites attract"'
  - text: '"Time is money" versus "slow and steady wins the race"'
- id: us-japan
  prompt: In the "Open Science, Open Questions" box, what happened to the assumption that the United States is individualistic and Japan collectivistic?
  explanation: 'By the early 2000s, “Some researchers found evidence that people in the U.S. were more collectivistic than people in Japan (Oyserman et al., 2002),” and others struggled to find reliable East–West differences (Heine et al., 2002). Researchers realized that how the constructs are measured matters, and that collectivism involves not just harmony but conflict within close groups and how it is managed (p. 61). It is a good example of the open science attitude: a famous "fact" gets reopened when measurement improves.'
  options:
  - text: It was confirmed by every study
  - text: It was proven that culture does not exist
  - text: Later evidence complicated it, and it remains an open question
    correct: true
  - text: Japan was found to be the most individualistic country in the world
- id: masculinity-femininity
  prompt: What does the masculinity–femininity dimension describe?
  explanation: 'The glossary defines it as “The cultural dimension that explains the distribution of emotional roles between men and women in society” (p. 61, glossary p. 502). In "feminine" cultures, roles are balanced: both men and women are expected to be modest and caring, and both parents attend to children''s emotional needs. In "masculine" cultures, roles are more rigid: men are expected to be assertive and ambitious, and deviations are more taboo (p. 61). The book notes the dimension was developed assuming gender as binary.'
  options:
  - text: How emotional roles are divided between men and women
    correct: true
  - text: How masculine or feminine individuals see themselves
  - text: The ratio of men to women in the workforce
  - text: Whether a society has a male or female head of state
- id: long-term
  prompt: A society encourages saving, persistence and adapting for future rewards, and attributes success to effort. Which pole of which dimension is this?
  explanation: Long-term vs. short-term orientation is “The cultural dimension that explains how people in society orient their attention and values toward the present and past relative to the future” (p. 61, glossary p. 502). Long-term cultures value perseverance, saving and investing, and believe “success is believed to stem from effort and failure from a lack of effort.” Short-term cultures focus on the here and now, spend and consume, and attribute more to luck (pp. 61–62). Engineering orgs show the same split between investing in tech debt reduction and shipping for this quarter.
  options:
  - text: Long-term orientation
    correct: true
  - text: Indulgence
  - text: Short-term orientation
  - text: Low uncertainty avoidance
- id: indulgence
  prompt: Which statement describes an indulgent (versus restrained) society?
  explanation: Indulgence vs. restraint is “The cultural dimension that explains how free people in a society are to indulge themselves in things for the sake of personal gratification” (p. 62, glossary p. 501). Indulgent societies let people “roam relatively freely so that they can enjoy life and have fun,” value leisure and expression, and place little emphasis on strict order; restrained societies use strict norms and controls to regulate gratification (p. 62).
  options:
  - text: It saves heavily for the future
  - text: It has large differences in power between groups
  - text: It values leisure and free expression and enforces little order
    correct: true
  - text: It heavily polices citizens and has strict norms about gratification
- id: tight-loose-origin
  prompt: According to Gelfand and colleagues, why do some cultures become tight?
  explanation: Tight cultures are “Cultures that have strict rules about how people should behave, and harsh punishments when people deviate from those rules”; loose cultures are “Cultures that have weaker rules and are much more permissive” (pp. 62–63, glossary pp. 502–504). Research found that groups facing many chronic threats to their existence tended to become tight, because rules make it easy to coordinate and respond to threats; groups that rarely faced threats could afford weaker norms (p. 63). Teams running safety-critical systems (payments, medical devices) predictably become tighter than teams running internal tools.
  options:
  - text: Because they have more democratic institutions
  - text: Because they are wealthier
  - text: Because their languages have more rules
  - text: Chronic threats required strict rules to coordinate responses
    correct: true
- id: tight-loose-tradeoff
  prompt: Which of these does the book associate with tight (rather than loose) cultures? Select all that apply.
  type: multiple
  explanation: 'Tight cultures emphasize order: they monitor citizens, expect conformity, prefer strong leaders, and have lower debt, alcoholism and obesity (Gelfand, 2018; p. 63). Loose cultures emphasize openness: they welcome new and creative perspectives, change more readily and express less prejudice toward people who differ from dominant groups (p. 63). The book stresses that “one way of being is not inherently better than the other” and that extremes of either are the most dysfunctional (Harrington et al., 2015).'
  options:
  - text: Closer monitoring of citizens and lower crime
    correct: true
  - text: More uniformity and expectation to conform
    correct: true
  - text: Lower rates of debt, alcoholism and obesity
    correct: true
  - text: Greater openness to new and creative perspectives and less prejudice toward outsiders
- id: extremes
  prompt: What did research on the extremes of tightness and looseness find?
  explanation: 'Cultures at either extreme “tend to be the most dysfunctional,” with “higher blood pressures as well as lower psychological well-being” (Harrington et al., 2015; p. 63). Too loose, and people feel there is little control or ability to coordinate; too tight, and they feel repressed and lacking autonomy (Gelfand, 2021). The engineering lesson is familiar: no process causes chaos, and too much process causes paralysis; the useful question is how much tightness the threats you actually face justify.'
  options:
  - text: There is no difference between extremes and moderate levels
  - text: The tightest cultures are the happiest
  - text: Both extremes are linked to worse health and well-being
    correct: true
  - text: The loosest cultures are the healthiest
- id: subcultures-class
  prompt: Within the United States, how do working-class and middle- or upper-class subcultures differ, according to the book, and why?
  explanation: 'Subcultures are “Smaller cultural groups within larger societies” (p. 63, glossary p. 503). The book reports that working-class Americans tend to have stricter norms and socialize children with stronger rules, and are more interdependent, thinking about fitting in, whereas upper-class people try to stand out (Stephens et al., 2017; p. 64). The explanation parallels tight–loose theory: groups facing threats of “hard living” adopt stricter rules, while wealth lets people move through life more freely (p. 65). U.S. states also vary, with the South tighter and the Northeast and West looser (Harrington & Gelfand, 2014).'
  options:
  - text: There are no class differences in cultural orientation
  - text: Working-class Americans are more individualistic because they rely only on themselves
  - text: Upper-class Americans have stricter norms because they have more to lose
  - text: Working-class people face more threats and have stricter norms
    correct: true
---
## Learning objectives

- Name and define Hofstede's six dimensions of culture
- Explain why individualism–collectivism is a societal, not individual, variable and why the U.S.–Japan comparison became an open question
- Explain tight and loose cultures, the threats that produce them, and the trade-off between order and openness
- Explain subcultures, including regional and social-class differences in the United States

## Key ideas

| Dimension | About | Example |
|---|---|---|
| Power distance | How societies deal with inequality | Large: subordinates follow without input |
| Uncertainty avoidance | Response to ambiguity | Strong: strict codes and punishments |
| Individualism vs. collectivism | How integrated people are into groups | "Squeaky wheel gets the grease" vs. "nail that sticks out gets hammered down" |
| Masculinity–femininity | Distribution of emotional roles between men and women | Masculine: rigid gender roles |
| Long-term vs. short-term orientation | Attention to the future vs. present and past | Long-term: saving, persistence |
| Indulgence vs. restraint | Freedom to pursue gratification | Indulgent: leisure valued, lenient norms |

(Table 3–1, p. 62)

**Tight cultures** have strict rules and harsh punishments for deviance; **loose cultures** are more permissive (pp. 62–63). Chronic threats produce tightness (Gelfand). Tightness brings order (lower debt, alcoholism, obesity) but less openness; looseness brings creativity and tolerance but less coordination. Extremes in either direction are the most dysfunctional (Harrington et al., 2015).

**Subcultures** (pp. 63–65): U.S. states vary in tightness (Southern tighter; Northeast and West looser). Working-class Americans have stricter norms and more interdependent orientations than middle- and upper-class Americans, largely because they face more threats such as debt, unemployment and crime.

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 3, pp. 59–65
- Hofstede, G. (2011). Dimensionalizing cultures. *Online Readings in Psychology and Culture*, 2(1)
- Gelfand, M. J., et al. (2011). Differences between tight and loose cultures. *Science*, 332, 1100–1104
- Harrington, J. R., & Gelfand, M. J. (2014). Tightness–looseness across the 50 United States. *PNAS*, 111, 7990–7995
