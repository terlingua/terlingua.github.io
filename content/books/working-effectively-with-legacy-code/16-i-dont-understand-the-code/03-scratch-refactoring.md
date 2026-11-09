---
title: "Scratch Refactoring"
questions:
  - id: definition
    prompt: "What is Scratch refactoring?"
    options:
      - id: o934731
        text: "Refactoring using only automated tools"
      - id: o7a0b8c
        text: "Refactoring in a separate branch and merging later"
      - id: throwaway
        text: "Check out the code, refactor freely without tests to understand it — and throw it away rather than checking it in"
        correct: true
    explanation: "“Forget about writing tests. Extract methods, move variables, refactor it whatever way you want to get a better understanding of it—just don’t check it in again. Throw that code away.”"
  - id: risks
    prompt: "What are the two risks of Scratch refactoring?"
    type: multiple
    options:
      - id: attached
        text: "You may get attached to the resulting structure and miss better insights later"
        correct: true
      - id: o65e51d
        text: "The scratch changes may be deployed accidentally"
      - id: false-view
        text: "A gross mistake may give you a false view of what the system does"
        correct: true
    explanation: "“The first risk is that we make some gross mistake when we refactor that leads us to think that the system is doing something that it isn’t.” “We could get so attached to the way that the code turns out that we start to think about it in those terms all the time.”"
  - id: why-not-same
    prompt: "Why might the real refactoring end up structured differently from the scratch one?"
    type: multiple
    options:
      - id: o201bcd
        text: "Refactoring tools produce random structure"
      - id: better-way
        text: "You may see a better way later"
        correct: true
      - id: code-changes
        text: "The code may change in the meantime, bringing different insights"
        correct: true
    explanation: "“We might see a better way of structuring the code later. Our code could change between now and then, and we might have different insights.”"
  - id: benefit
    prompt: "What psychological benefit does Scratch refactoring offer?"
    options:
      - id: oc9d037
        text: "It replaces the need for tests"
      - id: confidence
        text: "It convinces you that you understand the most important things — you feel there isn't something scary behind every corner"
        correct: true
      - id: o9f6d90
        text: "It improves code coverage"
    explanation: "“Scratch refactoring is a good way to convince yourself that you understand the most important things about the code” — “You feel reasonably confident that there isn’t something scary behind every corner”."
  - id: modern
    prompt: "Using git, which workflow best matches Scratch refactoring?"
    options:
      - id: o70c815
        text: "Refactor on main and revert if tests fail"
      - id: oe8290d
        text: "Squash the scratch commits into the feature PR"
      - id: discard
        text: "Create a throwaway branch or stash, refactor freely, then delete it and start the real work from the main branch"
        correct: true
    explanation: "The essence is “just don’t check it in again. Throw that code away.” A throwaway branch is the version-control equivalent."
---
## Learning objectives

- Use throwaway refactoring to learn code quickly
- Guard against false views and attachment

## Connections to today

Asking an AI assistant to "restructure this for readability" in a scratch branch is a fast scratch
refactoring — with the same two risks, amplified: the assistant may misread behaviour, and its
tidy structure may anchor you.
