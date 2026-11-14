---
title: Rational choice theory and situational crime prevention
questions:
- id: why-scp
  prompt: Why, according to the book, did governments become enthusiastic about situational crime prevention?
  options:
  - text: Because it was cheaper to build prisons
  - text: Because it addressed the root causes of poverty
  - text: Because labelling theory recommended it
  - text: They lost patience with criminologists' failure to solve the rising crime figures and shifted spending toward pragmatic, evaluated solutions based on removing opportunities
    correct: true
  explanation: Situational measures "were energetically promoted as governments essentially lost patience with the failure of criminologists to solve the apparently never-ending explosion in the crime figures", and UK spending went "more to finding and evaluating pragmatic solutions to particular offences than to developing criminological theory" (p. 64). The glossary's term for this is "administrative criminology" (p. 502).
- id: opportunity-premise
  prompt: What is the central premise accepted by most government-backed crime prevention practitioners?
  options:
  - text: Crime is a social construction and needs no prevention
  - text: Crime can only be reduced through harsher sentences
  - text: Crime is an outcome of the opportunity to offend, and regardless of offender motivation, removing that opportunity reduces crime
    correct: true
  - text: Crime is caused by biological defects
  explanation: Practitioners "came to accept the central notion that crime is an outcome of the opportunity to offend. Regardless of offender motivation, removal of that opportunity, it is argued, will reduce the incidence of crime" (p. 64). Note the explicit indifference to motivation, which Hopkins Burke later challenges using hate crime.
- id: scp-match
  prompt: A payments team stops storing raw card numbers and keeps only tokens from a vault provider. Which situational technique from the chapter does this most resemble?
  options:
  - text: Specific deterrence
  - text: Target hardening
  - text: Natural surveillance
  - text: Target removal
    correct: true
  explanation: 'The book distinguishes protecting a target (hardening) from removing it: "If a target can be removed completely instead of simply being protected even more impressive results are possible", for example "the centralization of cash transactions and the issue of tokens for use with gas and electricity meters" (p. 64). Tokenization removes the valuable target from your systems entirely. Encryption-at-rest would be closer to "reducing their attraction to thieves", like Chip and PIN.'
- id: surveillance-types
  prompt: What is the difference between formal and natural surveillance in the chapter?
  options:
  - text: Formal surveillance is legal; natural surveillance is illegal
  - text: Formal surveillance uses cameras; natural surveillance uses dogs
  - text: Formal surveillance relies on agencies like police and private security whose observation deters; natural surveillance relies on ordinary people observing their environment as they go about their business
    correct: true
  - text: There is no difference
  explanation: Formal surveillance proponents argue "agencies - such as the police and private security companies - that engage in observation activities will deter offenders"; natural surveillance "is founded on the notion that, by observing their environment as they go about their everyday business, people can provide themselves with some protection" (p. 64). Code review and pairing are natural surveillance; a SOC is formal.
- id: becker
  prompt: How did Gary Becker (1968) model the decision to offend?
  options:
  - text: As a response to labelling
  - text: 'As straightforward economic choice: the person compares legitimate income opportunities and rewards with illegal rewards, the probability of arrest and the likely punishment'
    correct: true
  - text: As a moral failure caused by poor parenting
  - text: As a product of brain chemistry
  explanation: Early rational choice variants "compare the decision-making process adopted by offenders with straightforward economic choice. Thus, Gary Becker (1968) proposed that the potential offender calculates the legitimate opportunities of earning income available, the amount of reward they offer, the amounts offered by illegal methods, the probability of arrest and the likely punishment" (p. 65). It is an expected-utility model; later work relaxes the assumption of full information and careful weighing.
- id: bounded-rationality
  prompt: What is **bounded (limited) rationality** in Clarke and Cornish's rational choice theory?
  options:
  - text: Offenders will not always obtain all the facts needed for a wise decision and will not necessarily weigh carefully the information they have; crime is the outcome of choices, however hasty or ill-considered
    correct: true
  - text: Offenders are perfectly rational and fully informed
  - text: Offenders have no rationality at all
  - text: Rationality is limited to the wealthy
  explanation: 'Crime is "''the outcome of the offender''s choices or decisions, however hasty or ill-considered these might be'' (Clarke, 1987: 118). In other words, offenders invariably act in terms of a limited or bounded form of rationality. They will not always obtain all the facts... and the information available will not necessarily be weighed carefully" (p. 66; glossary p. 505). The concept, from Herbert Simon''s work on decision-making, keeps choice central without pretending people are perfect calculators.'
- id: not-apart
  prompt: What advantage does the book attribute to the bounded rationality approach compared with the predestined actor model?
  options:
  - text: It explains all crime by poverty
  - text: It proves that criminals are biologically different
  - text: It removes the need for any crime prevention
  - text: It avoids treating criminals as a category of humanity apart from law-abiding citizens
    correct: true
  explanation: Bounded rationality "is an approach that avoids the inherent tendency within the predestined actor model to treat criminals as a category of humanity apart from law-abiding citizens" (p. 66). From this perspective "crime is simply rational action performed by fairly ordinary people in response to particular pressure, opportunities and situational inducements". It echoes Beccaria's "despotic spirit... is in everyone".
- id: ekblom
  prompt: According to Ekblom, what does situational crime prevention try to change?
  options:
  - text: Society's distribution of wealth
  - text: The specific circumstances that influence an offender's decision or ability to commit particular crimes at given places and times, taking propensities as given
    correct: true
  - text: Children's early socialization
  - text: Offenders' underlying propensities and motives
  explanation: 'Ekblom: it "does not directly aim to change offenders'' propensities or motives for crime at all. It takes these as given and, proceeding from an analysis of the circumstances engendering particular crimes, it introduces specific changes to influence the offender''s decision or ability to commit these crimes at given places and times" (p. 66). It is a configuration change, not a people change.'
- id: decision-chain
  prompt: Bennett (1986) observes that an offence rarely results from a single decision. What follows for prevention?
  options:
  - text: Only sentencing matters
  - text: Prevention is impossible because decisions are random
  - text: Only the first decision matters
  - text: There is a series of decisions from the original choice to offend to the final choice of target, involving both dispositional and situational factors, so practitioners must decide when and where in the sequence to intervene
    correct: true
  explanation: '"A series of decisions will probably be made, starting with the original choice to offend... and ending with the final decision to act against a particular target. Therefore, both dispositional and situational factors are involved", and situational factors exert more influence nearer the event, with "clear implications for crime prevention practitioners in deciding when and where to intervene" (p. 66). Compare intervening at reconnaissance versus at exfiltration in an attack chain.'
- id: stolen-goods
  prompt: What does Sutton argue about stolen goods markets? Select all that apply.
  type: multiple
  options:
  - text: Demand is often stimulated by respectable people prepared to ask few questions in the right circumstances
    correct: true
  - text: Courts should be less tolerant of the local 'fence', reducing criminal opportunity
    correct: true
  - text: Their existence provides crucial motivation for theft
    correct: true
  - text: Eradicating them would certainly eliminate drug-motivated crime with no displacement
  explanation: Sutton "proposes that it is the existence of stolen goods markets that provides the crucial motivation for theft", notes demand "is - in at least many cases - stimulated by respectable people prepared to ask few questions", and suggests judges "be considerably less tolerant of the local 'fence'" (p. 67). The book cautions that addicts might simply be displaced to "other means of obtaining cash such as prostitution... or armed robbery".
- id: expressive
  prompt: Which motives for crime does the chapter note do not fit the purely economic, career-choice picture?
  options:
  - text: Expressive rewards such as peer approval, getting back at a target like a school, or alleviating boredom
    correct: true
  - text: Profit and income maximization
  - text: Calculating the odds of arrest
  - text: Comparing legitimate wages to illegal gains
  explanation: 'Some crime is "committed for expressive reward, to gain the approval of peers, to ''get back at'' a target (such as a school)... or simply to alleviate boredom" (p. 66). Cultural criminology (Chapter 20) builds an entire theory around the non-instrumental thrills of crime. Not all attackers are financially motivated either: defacement and griefing are expressive.'
---
## Learning objectives

- Explain why governments embraced situational crime prevention and opportunity reduction
- Describe target hardening, target removal, reducing target attraction and surveillance
- Contrast Becker's economic model with Clarke and Cornish's bounded rationality
- Explain the sequence-of-decisions idea and Sutton's work on stolen goods markets

## The ideas

**Opportunity reduction.** Governments "essentially lost patience with the failure of criminologists to solve the apparently never-ending explosion in the crime figures" (p. 64). From the late 1970s UK spending went to pragmatic, evaluated solutions rather than theory, and practitioners accepted that "crime is an outcome of the opportunity to offend. Regardless of offender motivation, removal of that opportunity, it is argued, will reduce the incidence of crime". The glossary calls this administrative criminology: "reducing the opportunity to offend by the creation and evaluation of usually small-scale situational crime prevention schemes" (p. 502).

**Techniques.**

- **Target hardening**: closing a door, anti-bandit screens, security fencing, armoured safes.
- **Target removal**: centralizing cash, tokens for gas and electricity meters.
- **Reducing attraction**: cards were more attractive to thieves before Chip and PIN.
- **Cheap wins**: street lighting that cut crime and paid for itself within a year (Painter and Farrington).
- **Surveillance**: formal (police, private security) and natural (people watching their surroundings in everyday life; positioning employees).

**Crime as a career choice.** Jim Phelan (1940): the robber "is a tradesman" choosing a trade with greater rewards and dangers; "no one speaks of hereditary test-piloting" (p. 64). Gary **Becker (1968)** modelled the offender comparing legitimate income, illegal rewards, arrest probability and punishment — straightforward economic choice. Some crime, though, is expressive: peer approval, revenge on a target such as a school, boredom.

**Bounded rationality.** Clarke and Cornish define crime as "the outcome of the offender's choices or decisions, however hasty or ill-considered these might be" (p. 66, Clarke 1987). Offenders act with "a limited or bounded form of rationality": they lack full information and do not weigh it carefully. The approach avoids treating criminals "as a category of humanity apart". Ekblom: it does not try to change offenders' propensities but "takes these as given" and changes the situation to influence "the offender's decision or ability" to offend at given places and times (p. 66). Crime is "rational action performed by fairly ordinary people in response to particular pressure, opportunities and situational inducements".

**A sequence of decisions.** Bennett (1986): an offence rarely follows one decision; there is a chain from the general choice to offend to the final target selection. Dispositional factors matter early, situational ones nearer the event — so practitioners can choose where in the chain to intervene.

**Motivation returns.** Early rational choice ignored motivation; later versions say actions appear rational to the offender given their knowledge and thinking. Sutton argues **stolen goods markets** motivate theft (heroin and crack users averaging £15,000 a year needed to steal), and that demand comes from "respectable people prepared to ask few questions" — happy to buy a boxed TV at a silly price, not one with a child's fingerprints on it (p. 67). Tougher action on fences would cut opportunity, though addicts might be displaced to other crimes.

## Software lens

Situational crime prevention is defence in depth without needing to understand attackers' souls. Target hardening is MFA and least privilege; target removal is not storing card numbers at all (tokenization); reducing attraction is encrypting data so a stolen disk is worthless; surveillance is logging and anomaly detection, and "natural surveillance" is pairing and code review. Bounded rationality fits attackers too: most are opportunistic and work with partial information, so raising effort a little deflects many. Bennett's decision chain is the kill chain: intervene early (reduce attack surface) or late (block the final exfiltration). Sutton's point maps to ransomware: payments and stolen-credential marketplaces create the demand that motivates the supply.

## Sources

- Hopkins Burke (2014), pp. 63–7.
- Becker, G. (1968) 'Crime and punishment: an economic approach', *Journal of Political Economy*, 76: 169–217.
- Cornish, D. and Clarke, R. (eds) (1986) *The Reasoning Criminal*. New York: Springer.
- Clarke, R. (1987) 'Rational choice theory and prison psychology', in McGurk et al. (eds) *Applying Psychology to Imprisonment*. London: HMSO, p. 118.
- Ekblom, P. (2001) 'Situational crime prevention', in McLaughlin and Muncie (eds) *The Sage Dictionary of Criminology*. London: Sage, p. 264.
- Bennett, T. (1986) 'Situational crime prevention from the offender's perspective', in Heal and Laycock (eds) *Situational Crime Prevention*. London: HMSO.
- Sutton, M. (1998) *Handling Stolen Goods and Theft*. Home Office Research Study 178.
