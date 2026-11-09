---
title: "Lag Time"
questions:
  - id: definition
    prompt: "What is lag time?"
    options:
      - id: change-to-feedback
        text: "The time between making a change and getting real feedback about it"
        correct: true
      - id: o4db63a
        text: "The time a build server waits in a queue"
      - id: o35861d
        text: "Network latency to production"
    explanation: "“Lag time is the amount of time that passes between a change that you make and the moment that you get real feedback about the change.”"
  - id: rover
    prompt: "What does the Mars rover analogy illustrate?"
    options:
      - id: bundle
        text: "Long feedback delays push us to bundle many changes and move slowly — and even slower when we hit an obstacle"
        correct: true
      - id: o964538
        text: "That remote work is inefficient"
      - id: oca0c41
        text: "That autonomous software is dangerous"
    explanation: "“What we try to do instead is bundle a bunch of changes and make them all at once so that we don’t have to build too often.” and “If we hit an obstacle, we go even slower.”"
  - id: target
    prompt: "What feedback target does Feathers say is achievable in most mainstream languages?"
    options:
      - id: obb8c38
        text: "Under one hour"
      - id: o4eae6c
        text: "Under five minutes"
      - id: ten-seconds
        text: "Recompile and run tests on the code you're working on in under 10 seconds (under five if motivated)"
        correct: true
    explanation: "“you can always break dependencies in a way that lets you recompile and run tests against whatever code you are working on in less than 10 seconds. If a team is really motivated, its members can get it down to less than five seconds, in most cases.”"
  - id: requirement
    prompt: "What structural property makes this possible?"
    options:
      - id: separate-compile
        text: "Every class or module can be compiled separately and in its own test harness"
        correct: true
      - id: ob99670
        text: "A faster CPU"
      - id: oe59ba5
        text: "A single large project file"
    explanation: "“You should be able to compile every class or module in your system separately from the others and in its own test harness.”"
  - id: mental
    prompt: "How does compressing the time between steps change the quality of mental work?"
    type: multiple
    options:
      - id: o2f2cb7
        text: "You need to plan much more between steps"
      - id: concentration
        text: "Concentration is more intense"
        correct: true
      - id: mistakes
        text: "Mistakes are noticed and corrected much faster"
        correct: true
      - id: driving
        text: "Work becomes more like driving than waiting at a bus stop"
        correct: true
    explanation: "“Our work becomes more like driving than like waiting at a bus stop. Our concentration is more intense” and “Most important, the amount of time that it takes us to notice and correct mistakes is much smaller.”"
  - id: impediment
    prompt: "For compiled languages, what is the main impediment to near-instant feedback?"
    options:
      - id: o67b06d
        text: "Slow test frameworks"
      - id: dependency
        text: "Dependency — needing to compile something you don't care about to compile what you do"
        correct: true
      - id: o3d1997
        text: "Static typing"
    explanation: "“For the rest of us, who work in compiled languages, the main impediment is dependency, the need to compile something that we don’t care about just because we want to compile something else.”"
---
## Learning objectives

- Define lag time and its behavioural consequences
- State Feathers' feedback target and what it requires structurally

## Connections to today

- `dotnet watch test`, hot reload and incremental builds make sub-10-second loops realistic —
  if the test project doesn't drag in the whole solution.
- CI queue time is lag time too. A PR check that takes 40 minutes encourages bundling changes
  into big PRs, which is the rover problem at team scale.
