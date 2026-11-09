---
title: "Is It Worth It? Time Pressure and the Hump"
questions:
  - id: two-hours
    prompt: "You spent two hours writing tests and the change itself took 15 minutes. Why does Feathers say you cannot conclude the two hours were wasted?"
    type: multiple
    options:
      - id: find-errors
        text: "Tests also save time when you are trying to find an error"
        correct: true
      - id: unknown-without
        text: "You don't know how long the change would have taken without the tests"
        correct: true
      - id: o630503
        text: "Test-writing time is always billed to a different budget"
      - id: debug-time
        text: "You don't know how much debugging time a mistake would have cost"
        correct: true
    explanation: "“You don’t know how long that work might have taken you if you hadn’t written the tests. You also don’t know how much time it would’ve taken you to debug if you made a mistake” and “but also the amount of time tests save you when you are trying to find an error.”"
  - id: clustering
    prompt: "What observation makes the \"worst case\" (tests written for code nobody touches again) unlikely to be typical?"
    options:
      - id: cluster
        text: "Changes cluster in systems — if you change it today, a nearby change is likely soon"
        correct: true
      - id: ob4df74
        text: "Tests are always reused by other projects"
      - id: o18aaf4
        text: "Untouched code is deleted automatically"
    explanation: "“Typically, changes cluster in systems.” “If you are changing it today, chances are, you’ll have a change close by pretty soon.”"
  - id: experiment
    prompt: "What is the team experiment Feathers often starts with?"
    options:
      - id: ob74374
        text: "Pair every developer with a QA engineer"
      - id: no-change-without-tests
        text: "For an iteration, make no change without tests covering it; anyone who can't write a test calls a quick meeting to ask the group"
        correct: true
      - id: oa476fb
        text: "Freeze all changes for one iteration to write tests only"
      - id: o0ea2d6
        text: "Delete all failing tests and start over"
    explanation: "“For an iteration, we try to make no change to the code without having tests that cover the change. If anyone thinks that they can’t write a test, they have to call a quick meeting in which they ask the group whether it is possible to write the test.”"
  - id: hump
    prompt: "How do the beginnings of those iterations feel, and what eventually changes?"
    options:
      - id: terrible-then-better
        text: "Terrible at first; then people discover they are revisiting better code and changes get easier"
        correct: true
      - id: oa927c7
        text: "They feel great from day one"
      - id: o5277b4
        text: "They never improve, but management insists"
    explanation: "“The beginnings of those iterations are terrible.” and “But slowly, they start to discover that they are revisiting better code. Their changes are getting easier”."
  - id: house
    prompt: "\"Clients are clamoring — can we hack it inline in 10 places by 5:00?\" Which line from the sidebar captures Feathers' response?"
    options:
      - id: o3ca272
        text: "\"Ship it now; refactor in the next sprint.\""
      - id: house-opt
        text: "\"Code is your house, and you have to live in it.\""
        correct: true
      - id: o1043f2
        text: "\"Always say no to the boss.\""
    explanation: "“Remember, code is your house, and you have to live in it.” He notes the trap: “The hard part is actually going back and doing that testing and refactoring.”"
  - id: first-try
    prompt: "Under time pressure, what should you try *before* reaching for sprout and wrap techniques?"
    options:
      - id: o116224
        text: "Add logging and deploy"
      - id: instantiate
        text: "Try instantiating the class in a test harness — it may be easier than you think (Chapters 9 and 10)"
        correct: true
      - id: o975fcc
        text: "Rewrite the class"
    explanation: "“If you have to make a change to a class right now, try instantiating the class in a test harness.” and “Getting the code you are changing into a test harness might be easier than you think.”"
  - id: caution
    prompt: "What caution applies to every technique in this chapter?"
    options:
      - id: callers-untested
        text: "You add tested code, but unless you cover the code that calls it, you aren't testing its use"
        correct: true
      - id: oe5175b
        text: "They always make code uglier permanently"
      - id: o9a9e4b
        text: "They only work in Java"
    explanation: "“When you use them, you are adding tested code into your system, but unless you cover the code that calls it, you aren’t testing its use. Use caution.”"
---
## Learning objectives

- Argue for or against test investment under deadline pressure
- Describe the team experiment and "getting over the hump"
- Recognise when to try getting the class under test before sprouting

## In a nutshell

The cost of tests is visible; the savings are counterfactual. Changes cluster, so tests tend to
pay back quickly. Teams that commit to "no change without tests" feel pain at first and then do
not want to go back. When you really cannot get the class under test, write the change as new
code: sprout or wrap.

## Connections to today

- "We'll fix it in the next sprint" is the 5:00 emergency. Tracking the follow-up as a ticket
  rarely beats getting the change tested now.
- A useful team agreement: any PR touching legacy code must either add a test or explain in the
  description why it couldn't — the modern form of "call a quick meeting".
