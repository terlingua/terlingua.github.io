---
title: Automatic Thinking with Schemas, Accessibility and Priming
questions:
- id: automatic-vs-controlled
  prompt: Which pairing correctly describes the two modes of social thinking?
  explanation: 'The glossary defines automatic thinking as “Thinking that is nonconscious, unintentional, involuntary, and effortless” and controlled thinking as “Thinking that is conscious, intentional, voluntary, and effortful” (glossary pp. 499–500). Automatic thinking is not inherently bad; it lets us function without deliberating over everything. Engineers have the same split: pattern-matching a stack trace you have seen a hundred times versus methodically bisecting a novel bug. Errors arise when the automatic mode is applied to a situation it does not fit.'
  options:
  - text: 'Automatic: used only by experts; controlled: used only by novices'
  - text: 'Automatic: always wrong; controlled: always right'
  - text: 'Automatic: unconscious, effortless; controlled: conscious, effortful'
    correct: true
  - text: 'Automatic: conscious and effortful; controlled: unconscious and effortless'
- id: schemas
  prompt: What are schemas?
  explanation: 'The glossary defines schemas as “Mental structures people use to organize their knowledge about the social world that influence the information they notice, think about, and remember” (glossary p. 503; section p. 112). The database analogy is actually apt: a schema is a structure that tells you what fields to expect and fills defaults when values are missing. "Code review" activates a schema with roles, typical comments and outcomes. Schemas make processing fast and help with ambiguity, but defaults can be wrong, and a stereotype is a schema applied to a group.'
  options:
  - text: Emotions triggered by social situations
  - text: Database table definitions
  - text: Conscious rules people deliberately follow
  - text: Mental structures that organize social knowledge
    correct: true
- id: schema-gap-filling
  prompt: 'A new teammate sends a terse message: "Need to talk about your PR." Without other information, an engineer assumes they are in trouble. Which function of schemas does this illustrate?'
  explanation: 'Schemas are most influential when information is ambiguous: they supply the missing pieces. If this engineer''s schema for "need to talk" comes from experiences of criticism, they read the message as a threat. Usually gap-filling is helpful (it lets us make sense of partial information quickly), but it also means the same message can mean very different things to different readers, which is a construal effect from chapter 1.'
  options:
  - text: Schemas only operate when information is complete
  - text: Schemas fill gaps in ambiguous information
    correct: true
  - text: Schemas cause physiological arousal
  - text: Schemas make people ignore ambiguous information completely
- id: accessibility
  prompt: What is accessibility?
  explanation: Accessibility is “The extent to which schemas and concepts are at the forefront of people’s minds and are therefore likely to be used when making judgments about the social world” (glossary p. 499; section p. 114). Something can be accessible chronically (an on-call engineer who has seen many outages reads every latency blip as an incident) or temporarily, because it was recently activated. When a situation is ambiguous, the most accessible schema tends to be applied.
  options:
  - text: How easy a webpage is for people with disabilities to use
  - text: How readily a schema comes to mind and gets used
    correct: true
  - text: The accuracy of a schema
  - text: How available a person is to talk
- id: priming
  prompt: After reading a postmortem about a security breach, an engineer reviewing an unusual login pattern immediately interprets it as an attack. Which process best explains this?
  explanation: Priming is “The process by which recent experiences increase the accessibility of a schema, trait, concept, or goal” (glossary p. 503; section p. 114). In Higgins, Rholes and Jones (1977), participants first memorized words in a supposedly unrelated task, either "adventurous" or "reckless," then read about Donald, who sails across the Atlantic and skydives. Those primed with "reckless" rated him more negatively. The behavior was identical; the accessible concept changed its interpretation. Priming has influence only when the behavior is ambiguous and the primed concept is applicable to it.
  options:
  - text: Downward social comparison
  - text: Priming
    correct: true
  - text: The overjustification effect
  - text: Self-handicapping
- id: priming-applicability
  prompt: In the Donald study, why didn't priming unrelated negative words (such as "disrespectful") make people see Donald as reckless?
  explanation: 'Priming is not a general mood effect. A concept must be applicable to the behavior: "reckless" and "adventurous" are both plausible readings of skydiving, so whichever is more accessible wins. "Disrespectful" does not describe skydiving at all, so priming it does not change the judgment. This limits how far priming effects reach and is a reason to be skeptical of claims that subtle primes broadly transform behavior; several such dramatic claims have failed to replicate.'
  options:
  - text: The primed concept must fit the behavior being judged
    correct: true
  - text: Because priming only works with positive words
  - text: Because negative words cannot be primed
  - text: Because the participants knew about the experiment
- id: belief-perseverance
  prompt: A team concludes a service is slow because of its ORM. Later, the benchmark that supported this is shown to be flawed. Many on the team still believe the ORM is the problem. What is this called?
  explanation: 'Belief perseverance is “The tendency to stick with an initial judgment even in the face of new information that should prompt us to reconsider” (glossary p. 499). In Ross, Lepper and Hubbard''s (1975) study, people given false feedback about their ability kept believing it even after being told it was fake, because they had generated their own explanations for it. Those explanations survive when the original evidence is gone. A useful countermeasure is to explicitly consider the opposite: ask people to explain why the ORM might *not* be the cause.'
  options:
  - text: Social facilitation
  - text: Belief perseverance
    correct: true
  - text: Out-group homogeneity
  - text: Hindsight bias
- id: multi-schema-tradeoffs
  prompt: Which statements about schemas are consistent with the chapter? Select all that apply.
  type: multiple
  explanation: 'Schemas are part of automatic thinking: they guide attention and memory, fill gaps, and speed up processing. The trade-off is bias toward schema-consistent information and away from what does not fit. Chapter 8 defines a stereotype as “A generalization about a group of people in which certain traits are assigned to virtually all members of the group” (glossary p. 503), a schema about groups with the same benefits and costs, amplified by its social consequences.'
  options:
  - text: Schemas let us process information efficiently and make sense of ambiguity
    correct: true
  - text: Schemas only operate when we are thinking carefully
  - text: Schemas can lead us to notice and remember information consistent with them and overlook inconsistent information
    correct: true
  - text: Stereotypes are schemas about groups
    correct: true
---
## Learning objectives

- Distinguish automatic from controlled thinking
- Define schemas and explain why they are useful and when they mislead
- Define accessibility and priming, and distinguish chronic from temporary accessibility
- Explain belief perseverance

## Key ideas

**Automatic thinking** is nonconscious, unintentional, involuntary and effortless; **controlled thinking** is conscious, intentional, voluntary and effortful (pp. 112, 129). Most everyday social thinking is automatic and usually good enough.

**Schemas** are mental structures that organize knowledge and guide what we notice, think about and remember (p. 112). They fill gaps in ambiguous information, which is efficient but can produce errors, especially when the schema is a stereotype.

**Accessibility** (p. 114): how much a schema is at the front of the mind. It can be **chronic** (from experience) or **temporary**, from **priming**: recent experience makes a schema more accessible. Higgins, Rholes and Jones (1977) primed "adventurous" or "reckless" and participants interpreted the same ambiguous person ("Donald") accordingly. Priming works only when the primed concept is applicable to the behavior.

**Belief perseverance**: people cling to an initial judgment even after the evidence for it is discredited (Ross, Lepper & Hubbard, 1975).

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 5, pp. 112–117 (page ranges from the book's contents); glossary pp. 499–504
- Higgins, E. T., Rholes, W. S., & Jones, C. R. (1977). Category accessibility and impression formation. *Journal of Experimental Social Psychology*, 13, 141–154
- Ross, L., Lepper, M. R., & Hubbard, M. (1975). Perseverance in self-perception and social perception. *Journal of Personality and Social Psychology*, 32, 880–892
