---
title: "The Concorde Era: 3,038 to 85,900 (pp. 160–163)"
questions:
  - id: avoid-cutting-planes
    prompt: >-
      Applegate, Bixby, Chvátal and Cook initially tried to *avoid* cutting planes. What happened?
    explanation: >-
      “We initially attempted to avoid the cutting-plane method, adopting the motto that if other people have
      tried something then it was not for us. It didn’t take long to realize this was a big mistake and by
      1989 we were immersed in the details of branch-and-cut” (p. 160). Their first code, “Subtour,” was meant
      only to compute subtour bounds for comparison and grew into a full solver. Novelty for its own sake
      lost to the proven method.
    options:
      - text: "They succeeded without cuts"
      - text: "They quickly found it was a mistake and adopted branch-and-cut"
        correct: true
      - text: "They used only heuristics"
      - text: "They switched to quantum computing"
  - id: cycle-stealing
    prompt: >-
      How did the team get computing power at Bellcore for the 3,038-city solution (1992)?
    explanation: >-
      About fifty desktop workstations ran their parallel code when owners were away. Applegate’s monitor
      checked for mouse or keyboard input several times a second and “disappeared before the command’s
      results could be delivered” if a user looked (p. 160). They competed for idle cycles with Arjen Lenstra’s
      RSA-129 factoring effort. This was opportunistic distributed computing, similar in spirit to spot
      instances and preemptible VMs: cheap capacity that disappears on demand, so the work must be
      interruptible.
    options:
      - text: "They computed by hand"
      - text: "They rented a supercomputer"
      - text: "Parallel cycle-stealing on idle desktop workstations, yielding instantly to returning users"
        correct: true
      - text: "Cloud GPUs"
  - id: rewrite
    prompt: >-
      After 7,397 cities (1994), why did the team throw away “Subtour” and write Concorde from scratch?
    explanation: >-
      “The Subtour code had grown in spurts as we discovered new techniques and it did not capture well our
      global view of TSP computation. Faced with this fact, we did the only sensible thing and threw it away”
      (p. 161). The rewrite added methods aimed at larger instances, notably **local cuts**: shrink clusters to
      get very small graphs and use an expensive LP-based method to find cuts there (p. 161). It is a rare
      case of a successful full rewrite: the team understood the domain deeply, had a clear target
      architecture, and the result scaled from 7,397 to 85,900 cities.
    options:
      - text: "It was too fast"
      - text: "They lost the source code"
      - text: "It had a license problem"
      - text: "Its accreted design no longer reflected their understanding; a clean architecture enabled methods for much larger instances"
        correct: true
  - id: local-cuts
    prompt: >-
      What are Concorde's “local cuts”?
    explanation: >-
      “A ‘local cuts’ separation routine based on shrinking clusters of cities to obtain very small graphs,
      allowing us to apply a time-consuming LP-based method to obtain cutting planes” (p. 161). On a tiny
      shrunken graph you can afford an exact search for violated inequalities that need not belong to any
      named family. Those cuts are then lifted back to the full instance. Reduce to a small core, solve that
      core exactly, and map the answer back.
    options:
      - text: "Cuts specific to one city"
      - text: "Shrink to tiny graphs, find violated inequalities there with an expensive exact LP method, lift back"
        correct: true
      - text: "Gomory cuts on local variables"
      - text: "Removing local edges"
  - id: record-list
    prompt: >-
      Which Concorde-era optimal solutions are correctly matched to their year?
    type: multiple
    explanation: >-
      3,038 drilling (1992), 4,461 East Germany (1993), 7,397 circuit (1994), 13,509 USA towns of 500 or more
      people (1998), 15,112 Germany (2001), 24,978 Sweden (2004, 84.8 CPU-years on 96 dual-processor
      machines at Georgia Tech), and 85,900 chip (run Feb 2005 to Apr 2006) (pp. 160–162). The text on p. 161
      says “24,979 cities in Sweden,” a typo for the 24,978 given elsewhere (p. 13).
    options:
      - text: "13,509 USA cities — 1998"
        correct: true
      - text: "24,978 Sweden — 2004"
        correct: true
      - text: "85,900 chip — 2006"
        correct: true
      - text: "3,038 drilling — 1975"
  - id: tour-from-helsgaun
    prompt: >-
      The 85,900-city optimal tour came from where, and how was optimality established?
    explanation: >-
      Concorde’s LP bound rose steadily (figure 8.10) until it was “within 0.001% below the length of the
      best-known tour found in 2004 by Keld Helsgaun. This was close enough for a short branch-and-cut search
      to close the remaining gap and prove that Helsgaun’s tour was indeed optimal” (p. 163). The heuristic
      (LKH) supplied the tour and the exact machinery supplied the proof, which is the division of labor this
      companion keeps returning to. Espinoza–Goycoolea’s domino-parity module made the bound reachable
      (p. 162).
    options:
      - text: "Concorde found the tour itself by branching"
      - text: "Helsgaun's LKH tour from 2004; Concorde raised the bound to within 0.001% and branched to prove it"
        correct: true
      - text: "A genetic algorithm found and proved it"
      - text: "It is not proven"
  - id: certificate-85900
    prompt: >-
      The computation took 136 CPU-years. How can anyone trust the claim?
    explanation: >-
      “In 2009 we published a computer code and data set that together certify the optimality of the
      85,900-city tour. The data consists of the cutting planes and dual LP solutions for each subproblem in
      the branch-and-cut search. The computer code, a relatively svelte 6,646 lines in the C programming
      language, runs through the subproblems and establishes that the dual LP solutions provide bounds to
      allow each of them to be pruned” (p. 163). Verifying is far cheaper than solving, and a small,
      auditable checker replaces trust in the huge solver.
    options:
      - text: "By re-running 136 years of computation"
      - text: "A published certificate (cuts plus dual solutions per subproblem) and a small checker that verifies every pruning"
        correct: true
      - text: "Peer reviewers trusted the authors"
      - text: "Helsgaun's reputation"
---
## Learning objectives

- Describe how Concorde was engineered and run.
- Explain certification of a massive computation.

## Timeline (pp. 160–163)

- 1988–89: tried to avoid cuts, then adopted branch-and-cut. Wrote **Subtour**.
- 1992: **3,038** cities on idle Bellcore workstations, competing with RSA-129 factoring for cycles.
- 1993: 4,461. 1994: 7,397. Then **threw Subtour away** and wrote **Concorde**, adding local cuts.
- 1998: **13,509** USA. 2001: 15,112 Germany. 2004: **24,978** Sweden (84.8 CPU-years).
- 2005–06: **85,900** chip, with Letchford/Espinoza–Goycoolea domino-parity cuts. Helsgaun’s 2004 tour proved optimal. **136 CPU-years**.
- 2009: published **certificate** with a 6,646-line checker.

## In software terms

- **Preemptible compute:** design long jobs to checkpoint and yield, as cycle stealing did and as spot fleets require.
- **Rewrite only with a clear model.** It worked here because the team knew exactly what they wanted.
- **Verifiable results:** ship certificates and small checkers for expensive computations.

## Sources

- Cook (2012), pp. 160–163.
- Applegate, D. L., R. E. Bixby, V. Chvátal, W. J. Cook. *The Traveling Salesman Problem: A Computational Study*. Princeton, 2006.
- Applegate, D. L., et al. “Certification of an optimal TSP tour through 85,900 cities.” *Oper. Res. Letters* 37 (2009), 11–15.
