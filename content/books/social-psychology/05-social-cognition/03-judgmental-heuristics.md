---
title: 'Mental Shortcuts: The Availability and Representativeness Heuristics'
questions:
- id: heuristics-definition
  prompt: What are judgmental heuristics?
  explanation: The glossary defines judgmental heuristics as “Mental shortcuts people use to make judgments quickly and efficiently” (glossary p. 501; section p. 120). Like caching or approximate algorithms in software, they trade accuracy for speed and are usually good enough. The interesting part is that their failures are systematic and predictable, so you can design around them.
  options:
  - text: Formal algorithms for statistical inference
  - text: Schemas about specific people
  - text: Mental shortcuts for quick, efficient judgments
    correct: true
  - text: Moral rules taught by culture
- id: availability
  prompt: After a well-publicized outage at a cloud provider, a team estimates that provider outages are much more frequent than its own historical data shows. Which heuristic is at work?
  explanation: The availability heuristic is “A mental rule of thumb whereby people base a judgment on the ease with which they can bring something to mind” (glossary p. 499; section p. 121). Ease of recall usually tracks frequency, but vivid, recent and heavily discussed events are easy to recall regardless of how common they are. A widely shared outage postmortem inflates estimates of outage risk; dramatic but rare security incidents can crowd out boring, common risks like expired certificates. Ground estimates in data where you have it.
  options:
  - text: Availability
    correct: true
  - text: Representativeness
  - text: Anchoring on the base rate
  - text: The overjustification effect
- id: schwarz-assertive
  prompt: In Schwarz et al. (1991), some people listed 6 times they were assertive and others listed 12. Who rated themselves as more assertive, and why?
  explanation: 'Listing 12 examples is hard; people who struggled concluded "if it''s this hard to think of examples, I must not be very assertive." Those listing 6 found it easy and concluded the opposite. The *ease* of retrieval, not the *number* of examples retrieved, drove the judgment. Applied to engineering: asking a team to list many risks can make the project seem safer ("we had trouble thinking of the last few") than asking for a few.'
  options:
  - text: Those who listed 6
    correct: true
  - text: Those who listed 12, because they had more examples
  - text: Both groups equally
  - text: Neither group, because self-ratings are unaffected by recall
- id: representativeness
  prompt: What is the representativeness heuristic?
  explanation: The glossary defines it as “A mental shortcut whereby people classify something according to how similar it is to a typical case” (glossary p. 503; section p. 123). If someone wears a hoodie, codes in Vim and talks about Rust, we judge them "typical startup engineer" by resemblance. Similarity is often informative, but the heuristic ignores how common each category actually is.
  options:
  - text: Judging by how easily something comes to mind
  - text: Choosing representatives for a group decision
  - text: Assuming a sample represents a population
  - text: Classifying by similarity to a typical case
    correct: true
- id: base-rate
  prompt: An anomaly-detection rule flags a login as "looks like credential stuffing" because it matches the attack pattern. True attacks are 1 in 10,000 logins, and the rule also matches 1% of normal logins. Most flagged logins are therefore...
  explanation: Base rate information is “Information about the frequency of members of different categories in the population” (glossary p. 499). The representativeness heuristic tempts us to judge only by resemblance to the typical attack and ignore how rare attacks are. Out of 10,000 logins, about 1 is an attack and about 100 normal logins match the rule, so roughly 99% of alerts are false positives. This is base-rate neglect, and it is why alert fatigue happens when detectors are tuned only to look like the bad case.
  options:
  - text: Normal logins
    correct: true
  - text: Attacks, because the pattern matches the attack profile
  - text: Impossible to determine without knowing the attacker
  - text: Exactly half attacks and half normal
- id: barnum
  prompt: An online "developer personality test" tells everyone "You value clean code but sometimes cut corners under pressure, and you prefer autonomy but appreciate guidance." Most people rate it as highly accurate. Why?
  explanation: The section "Personality Tests and the Representativeness Heuristic" (p. 125) explains that vague, generally true statements feel personally diagnostic because they match how we see ourselves; we do not ask how many other people would match too (the base rate). This is often called the Barnum effect. Validating any assessment, whether a personality quiz or a hiring rubric, requires showing it distinguishes people, not just that people like its output.
  options:
  - text: The test is highly valid
  - text: Vague statements match everyone's self-image
    correct: true
  - text: People rate all feedback as accurate
  - text: Because of the overjustification effect
- id: multi-heuristics
  prompt: Which of these are likely failures of the availability or representativeness heuristic? Select all that apply.
  type: multiple
  explanation: 'The first and third are availability: vivid, memorable events feel frequent. The second is representativeness: resemblance to a prototype replaces base rates. The fourth uses actual frequency data, which is exactly the corrective. Heuristics are not errors in themselves; they become errors when better data are available and ignored.'
  options:
  - text: Overestimating the risk of a dramatic zero-day after reading about one, while ignoring routine misconfigurations
    correct: true
  - text: Assuming a candidate who fits the "rock-star engineer" stereotype will perform better, regardless of how few such candidates actually succeed
    correct: true
  - text: Judging that a rare-but-vivid bug type is common because the team still talks about the one time it happened
    correct: true
  - text: Estimating error rates by querying a year of logs
- id: heuristics-why
  prompt: Why do people rely on heuristics despite their errors?
  explanation: 'Heuristics are efficient: they let us make many judgments a day with limited attention. The book''s framing in this chapter is of people as everyday theorists working with incomplete information. Errors come from applying shortcuts where their assumptions do not hold. The practical response is not to abolish intuition but to identify high-stakes decisions where slowing down, gathering base rates and checking alternatives is worth the cost.'
  options:
  - text: Because culture forbids careful thinking
  - text: Because heuristics are always more accurate than analysis
  - text: Because people are lazy and careless
  - text: They are fast and usually good enough
    correct: true
---
## Learning objectives

- Define judgmental heuristics and explain why people use them
- Explain the availability heuristic, including the Schwarz et al. assertiveness study
- Explain the representativeness heuristic and base-rate neglect
- Explain why vague personality descriptions feel accurate (the Barnum effect)

## Key ideas

**Judgmental heuristics** are mental shortcuts for quick, efficient judgments (p. 120). They usually work but fail predictably.

**Availability heuristic** (p. 121): judging by how easily something comes to mind. Schwarz et al. (1991): people who listed 6 examples of their own assertiveness rated themselves more assertive than people who listed 12, because 12 was hard to generate. Vivid, recent, widely reported events seem more frequent than they are.

**Representativeness heuristic** (p. 123): classifying something by how similar it is to a typical case. It leads people to neglect **base rate information** about how common categories are.

**Personality tests** (p. 125): vague, generally true descriptions feel personally accurate because they resemble our self-image; the representativeness heuristic makes them seem diagnostic.

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 5, pp. 120–126
- Tversky, A., & Kahneman, D. (1974). Judgment under uncertainty: Heuristics and biases. *Science*, 185, 1124–1131
- Schwarz, N., et al. (1991). Ease of retrieval as information. *Journal of Personality and Social Psychology*, 61, 195–202
