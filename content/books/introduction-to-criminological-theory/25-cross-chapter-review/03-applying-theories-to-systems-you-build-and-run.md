---
title: Applying theories to systems you build and run
questions:
- id: apply-ci
  prompt: A team's code-style violations drop sharply after they replace a monthly manual audit with an automated check that fails the build within a minute. Which idea best explains the improvement?
  options:
  - text: Lombroso's atavism
  - text: The dramatization of evil
  - text: 'Felson''s hot stove and the deterrence principles of certainty and celerity: quick, certain, minor consequences shape behaviour better than rare, delayed ones'
    correct: true
  - text: Merton's ritualism
  explanation: 'Felson: a real stove burns "quick, certain, but minor"; an imaginary one that burns rarely and later "will not be as effective" (p. 50). Deterrence theory emphasizes certainty and celerity over severity (pp. 60–1).'
- id: apply-tokens
  prompt: A company eliminates stored passwords by moving to passkeys, removing a valuable target from its databases. Which theory most directly supports this?
  options:
  - text: Psychodynamic theory
  - text: Peacemaking criminology
  - text: Situational crime prevention (target removal) within rational choice theory
    correct: true
  - text: Labelling theory
  explanation: '"If a target can be removed completely instead of simply being protected even more impressive results are possible" (p. 64). The approach takes offender motivation as given and changes the situation (Ekblom, p. 66).'
- id: apply-reorg
  prompt: After a merger, engineers from the acquired company keep deploying the way they always did and are written up for violations. Turnover rises and some begin deliberately ignoring policy. Which combination of theories best explains the sequence?
  options:
  - text: Garofalo's natural crimes
  - text: Routine activities followed by deterrence
  - text: Biological positivism followed by eugenics
  - text: Sellin's culture conflict (their conduct norms clash with the dominant culture's rules) followed by labelling and secondary deviance as stigmatizing reaction hardens oppositional identity
    correct: true
  explanation: Culture conflict arises "most typically, through migration" when "laws usually represent the rules or norms of the dominant culture" (p. 208); repeated stigmatizing reaction can produce secondary deviance (p. 198).
- id: apply-metrics
  prompt: Leadership demands a doubling of features shipped per quarter without extra staff. Teams start disabling flaky tests and inflating estimates. Which theory most directly explains the behaviour?
  options:
  - text: Hirschi's involvement
  - text: 'Merton''s anomie: innovation — accepting the goal while using illegitimate means because legitimate means are blocked'
    correct: true
  - text: Sheldon's body types
  - text: The carnival of crime
  explanation: Strain arises where everyone is pushed toward a goal while access to legitimate means is limited (p. 141); innovation is the predictable adaptation.
- id: apply-thrill
  prompt: A teenager who defaced a company's status page says they did it "to see if I could" and to impress friends online; they had no financial motive. Which perspective explains this best?
  options:
  - text: Bowlby's maternal deprivation
  - text: 'Cultural criminology: Katz''s seductions of crime and Presdee''s carnival, where transgression and status are the attraction'
    correct: true
  - text: Rusche and Kirchheimer's labour market theory
  - text: Becker's economic model of crime
  explanation: Katz describes crime whose causes "are constructed by the offenders themselves in ways that are compellingly seductive" (p. 508); Presdee includes "computer hacking" among carnival transgressions (p. 392).
- id: apply-layoffs
  prompt: During layoffs, insider incidents rise among remaining staff. Survivors report losing trusted colleagues, career prospects and any sense that their work matters. Which theories predict this? Select the best combination.
  options:
  - text: Becker's economic model alone
  - text: Garofalo and Quetelet
  - text: Hirschi's social bond (weakened attachment and commitment), Box's recession thesis and Tittle's control deficit
    correct: true
  - text: Lombroso and Goring
  explanation: Bonds weaken when attachment and stakes are lost (p. 298); Box showed recession "could lead to breakdown in social bonds and an increase in criminal activity" (p. 309); a control deficit breeds "resentment, envy and the loss of any stake" (p. 305).
- id: apply-shadow
  prompt: Most engineers in a company use unapproved tools, yet the security team publicly disciplines two engineers who had criticized management. Which concepts apply? Select the best combination.
  options:
  - text: Mechanical solidarity and maturational reform
  - text: Atavism and natural crimes
  - text: The schizophrenia of crime (near-universal rule-breaking alongside demands for rigorous enforcement) and conflict theory's insight that enforcement follows power
    correct: true
  - text: Defensible space and broken windows only
  explanation: Criminality "widespread to the virtual point of universality" coexists with demands for rigorous intervention (p. 508); conflict and radical theories show enforcement falling on those with least power or most nuisance value (pp. 225–6).
- id: apply-blameless
  prompt: After a serious mistake, an engineer is publicly thanked for leading the fix, the review names the decision and its impact without naming-and-shaming, and their manager pairs them with a mentor. Which theories underpin this practice? Select the best combination.
  options:
  - text: Right realist incapacitation and three strikes
  - text: Lombroso's typology
  - text: Rusche and Kirchheimer
  - text: Braithwaite's reintegrative shaming, labelling theory's warning about secondary deviance, and desistance theory's emphasis on hope, relationships and new identities
    correct: true
  explanation: Reintegrative shaming condemns the act while preserving membership (p. 508); stigmatization risks secondary deviance (p. 198); desistance requires hope, relationships and opportunities to practise new identities (p. 341).
- id: apply-risk-score
  prompt: A platform introduces a risk score that limits accounts based on predicted probability of abuse, with no specific violation required and no appeal. Over time the score is extended to more features and more user groups. Which perspectives describe this best?
  options:
  - text: Psychoanalysis
  - text: The new penology and actuarial justice, with net-widening, as described by Feeley and Simon and McCahill and Norris
    correct: true
  - text: Peacemaking criminology
  - text: Classical due process
  explanation: The new penology manages "groups assorted by levels of dangerousness based not on individualized suspicion, but on the probability" of offending (p. 506), and once surveillance is introduced "the concept of dangerousness is broadened" (p. 412).
- id: apply-square
  prompt: A security leader wants a single framework to plan a programme against account takeover that addresses attackers' incentives, users' account hygiene, community reporting and the security team's relationship with engineering. Which framework fits?
  options:
  - text: The left realist square of crime
    correct: true
  - text: Lombroso's typology
  - text: Concentric zone theory
  - text: Merton's five adaptations
  explanation: The square of crime requires intervention at the offender, victim, public and formal-system corners and their relationships (p. 349).
- id: apply-sat
  prompt: Most engineers never consider stealing source code; for a few contractors with grievances it is a live option, and for them visible auditing reduces attempts. Which theory explains this pattern best?
  options:
  - text: Biological positivism
  - text: Merton's ritualism
  - text: Situational action theory, with Wikström et al.'s finding that deterrence matters mainly for those who see crime as an action alternative
    correct: true
  - text: The Chicago School
  explanation: People who do not see crime as an alternative abstain "regardless of whether they assess the risk of getting caught as very high or very low", while those who do are influenced by "deterrence sensitivity" (p. 323).
- id: apply-values
  prompt: A team presents a "toxicity score" as an objective, neutral measurement. A reviewer notes that what counts as toxic is a normative decision and that affected communities were never consulted. Which idea from the final chapter is the reviewer applying?
  options:
  - text: Atavism
  - text: The technocratic model
  - text: The bloody code
  - text: The democratic model's recognition that no research is value-neutral, with public concern helping to direct research
    correct: true
  explanation: A democratic model "recognizes that no criminological research is value-neutral. 'Crime' itself is a normative concept" (p. 498), and involves exchange in which "public concern helps direct" research.
---
## Learning objectives

- Apply multiple criminological theories to realistic engineering, security and platform scenarios
- Choose the theory whose mechanism best fits the evidence
- Combine theories into an integrated, square-of-crime style response

## The ideas

Each question below describes a situation from software, DevOps, security or platform governance. More than one theory may be relevant; pick the one whose *mechanism* best fits the details given, and read the explanation for how others would add to it. A useful habit, drawn from left realism's square of crime and Wikström's SAT, is to ask four questions of any incident: what produced the motivated actor; what made the target suitable; what happened to guardians, handlers and community; and what did the formal system (rules, enforcement, labels) contribute?

## Software lens

The point of a theory is to tell you where to look and what to change. Deterrence says change the penalties and detection; situational prevention says change the target; control theory says strengthen bonds; labelling says change the reaction; critical theory says examine who wrote the rule; cultural criminology says understand what the act meant to the actor.

## Sources

- Hopkins Burke (2014), passim.
