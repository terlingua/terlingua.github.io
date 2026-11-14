---
title: 'Eyewitness Testimony: Encoding, Storage and Retrieval'
questions:
- id: wrongful-convictions
  prompt: According to SPA 3, in about what share of DNA exoneration cases listed by the Innocence Project was the conviction based at least partly on faulty eyewitness testimony?
  explanation: The book reports that “In approximately 75% of these cases, the conviction was based, at least in part, on faulty eyewitness testimony” (p. 444) and that an erroneous eyewitness is one of the most common causes of wrongful conviction. Jurors find confident eyewitnesses very persuasive, which makes the problem worse. Engineers see a version of this when a confident colleague's memory of "what happened during the incident" contradicts the logs.
  options:
  - text: About 75%
    correct: true
  - text: About 5%
  - text: About 25%
  - text: About 100%
- id: three-stages
  prompt: Which pairing correctly defines the three stages of memory?
  explanation: The glossary defines encoding as “The process by which people attend to information in their environment and transform this sensory data into a mental representation,” storage as “The process by which people maintain in memory information they have encoded from the environment,” and retrieval as “The process by which people recall information stored in their memories” (glossary pp. 500, 503). The book stresses “our minds are not like video cameras” (p. 444). Errors at any stage corrupt testimony, just as bugs at ingest, storage or query corrupt a data pipeline.
  options:
  - text: 'Encoding: recalling; storage: forgetting; retrieval: perceiving'
  - text: Encoding and retrieval are the same process
  - text: Storage is when people describe events to the police
  - text: Encoding takes in, storage keeps, retrieval recalls
    correct: true
- id: own-race-bias
  prompt: What is own-race bias?
  explanation: Own-race bias is “The tendency for people to be better at recognizing faces of their own race than those of other races” (p. 445, glossary p. 502). It arises largely from greater experience with own-race faces and attention to identity-relevant features. It contributes to cross-race misidentifications in lineups. Face recognition systems trained mostly on one group show an analogous accuracy gap, which is an engineering ethics issue.
  options:
  - text: Remembering crimes by one's own race more vividly
  - text: Assuming people of one's own race are innocent
  - text: Preferring to hire people of one's own race
  - text: Recognizing own-race faces better than others
    correct: true
- id: attention
  prompt: In the study where viewers counted basketball passes, what happened when a person in a gorilla costume walked through the scene?
  explanation: The book reports that “Although it seems as if everyone would notice such a bizarre interruption, only half did” (p. 446). People encode only what they attend to. An eyewitness focused on a weapon may not encode the face. In debugging, engineers focused on one hypothesis may not "see" an obvious anomaly in the same dashboard.
  options:
  - text: Only about half of viewers noticed the gorilla
    correct: true
  - text: Viewers noticed the gorilla but forgot the passes
  - text: Everyone noticed immediately
  - text: Nobody noticed
- id: reconstructive-memory
  prompt: In Loftus's studies, people saw a car at a stop sign, then some were asked a question that mentioned a yield sign. What happened?
  explanation: Reconstructive memory is “The process whereby memories of an event become distorted by information encountered after the event occurred” (p. 447, glossary p. 503). The misleading question planted new information that merged with the original memory. Leading questions in incident reviews ("when you saw the database was overloaded...") can likewise reshape responders' memories. Ask open questions and check against logs.
  options:
  - text: The question had no effect
  - text: People forgot the car entirely
  - text: Everyone correctly remembered the stop sign
  - text: Many later "remembered" a yield sign
    correct: true
- id: source-monitoring
  prompt: An engineer is certain they read about a breaking API change in the official changelog, but they actually saw it speculated about on a forum. What kind of error is this?
  explanation: Source monitoring is “The process whereby people try to identify the source of their memories” (p. 448, glossary p. 503). In the Loftus studies, people who had both a stop sign and a yield sign in memory confused which came from the event and which from the question. Source errors are common with information from multiple channels (Slack, docs, meetings), which is why citing sources in decisions matters.
  options:
  - text: Post-identification feedback
  - text: A source monitoring error
    correct: true
  - text: Own-race bias
  - text: An encoding failure
- id: lineups
  prompt: Which practices does SPA 3 recommend to make police lineups more accurate? Select all that apply.
  type: multiple
  explanation: 'Table SPA-3–1 lists recommendations including blind administration, sequential presentation, warning that the culprit may not be present, and fillers who resemble the description (p. 449). Sequential presentation makes it “more difficult for witnesses to compare all the pictures and choose the one that most resembles the criminal” (p. 449), reducing best-guess errors. Telling witnesses they picked the suspect inflates confidence (next section). These are engineering-style safeguards: blinding, isolation of steps, and avoiding feedback leakage.'
  options:
  - text: Choosing fillers who match the witness's description
    correct: true
  - text: Telling witnesses the culprit may not be present
    correct: true
  - text: Telling witnesses when they have picked the suspect
  - text: The person running the lineup should not know who the suspect is
    correct: true
  - text: Presenting photos sequentially, one at a time, rather than all at once
    correct: true
---
## Learning objectives

- Explain why eyewitness errors cause many wrongful convictions
- Define encoding, storage and retrieval and the sources of error at each stage
- Explain own-race bias, change blindness and attention effects
- Explain reconstructive memory and source monitoring
- Describe recommended lineup procedures

## Key ideas

In about 75% of DNA exonerations listed by the Innocence Project, conviction was based at least partly on faulty eyewitness testimony (p. 444). Memory is not a video camera; errors arise at **encoding** (p. 444), **storage** (p. 447) and **retrieval** (p. 448).

**Encoding**: poor viewing conditions, seeing what we expect, attention narrowed (half of viewers missed a person in a gorilla suit, p. 446), and **own-race bias**: better recognition of faces of one's own race.

**Storage**: **reconstructive memory**: information encountered after the event distorts the memory (Loftus): a misleading question about a yield sign led people to "remember" a yield sign instead of a stop sign. **Source monitoring** errors: confusion about where a memory came from (p. 448).

**Retrieval** (pp. 448–449): lineups produce "best guess" errors. Recommendations (Table SPA-3–1): administrators who do not know who the suspect is, sequential presentation, instructions that the culprit may not be present, fillers who match the description, and recording confidence immediately.

## Sources

- Aronson et al., *Social Psychology* 12e, Social Psychology in Action 3, pp. 442–449
- Loftus, E. F. (1979). *Eyewitness Testimony*. Harvard University Press
- Meissner, C. A., & Brigham, J. C. (2001). Thirty years of investigating the own-race bias in memory for faces. *Psychology, Public Policy, and Law*, 7, 3–35
- Simons, D. J., & Chabris, C. F. (1999). Gorillas in our midst. *Perception*, 28, 1059–1074
- Wells, G. L., et al. (1998). Eyewitness identification procedures. *Law and Human Behavior*, 22, 603–647
