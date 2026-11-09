---
title: "Pinch Point Traps"
questions:
  - id: trap
    prompt: "What trap threatens unit tests for new code?"
    options:
      - id: odca9fb
        text: "They stop compiling"
      - id: mini-integration
        text: "They slowly grow into mini-integration tests that are big, bulky and slow"
        correct: true
      - id: obcf57d
        text: "They become too small"
    explanation: "“One way is to let unit tests slowly grow into mini-integration tests.” — “if we do this too often, we’ll end up with a lot of big, bulky unit tests that take forever to run.”"
  - id: new-code-rule
    prompt: "When writing unit tests for new code, what is the trick?"
    options:
      - id: o77b2c9
        text: "Only write integration tests"
      - id: oddfa32
        text: "Always use real collaborators"
      - id: independent
        text: "Test classes as independently as possible; break down classes when tests get too large; fake collaborators when needed"
        correct: true
    explanation: "“The trick when we are writing unit tests for new code is to test classes as independently as possible.” — “the job of a unit test isn’t to see how a cluster of objects behaves together, but rather how a single object behaves.”"
  - id: tables-turned
    prompt: "How are \"the tables turned\" for existing code?"
    options:
      - id: carve
        text: "It can pay to carve off a piece of the application with pinch-point tests, then write narrower unit tests as you touch classes"
        correct: true
      - id: o2d034c
        text: "Existing code should never be tested"
      - id: o714cf4
        text: "Existing code must only have unit tests"
    explanation: "“When we are writing tests for existing code, the tables are turned. Sometimes it pays to carve off a piece of an application and build it up with tests.”"
  - id: forest
    prompt: "What does the \"forest\" image say about the life cycle of pinch-point tests?"
    options:
      - id: delete-later
        text: "They stake out an area you own; over time you can delete them and let per-class tests support development"
        correct: true
      - id: o2520d3
        text: "They should be kept forever as the primary safety net"
      - id: ofebdb1
        text: "They should be deleted immediately"
    explanation: "“Tests at pinch points are kind of like walking several steps into a forest and drawing a line, saying \"I own all of this area.\"” — “Over time, you can delete the tests at the pinch point and let the tests for each class support your development work.”"
  - id: oasis
    prompt: "What does writing characterization tests at a pinch point create, and what's the warning?"
    options:
      - id: obf0e94
        text: "A permanent architecture layer"
      - id: odf942c
        text: "A performance improvement"
      - id: oasis-trap
        text: "A little oasis where work gets easier — but it could be a trap"
        correct: true
    explanation: "“You’ve made a little oasis in your application where the work has just gotten easier. But be careful—it could be a trap.”"
---
## Learning objectives

- Avoid letting pinch-point tests become the permanent suite
- Apply different rules to new code and existing code

## Connections to today

Large, slow "service tests" that nobody dares delete are the pinch-point trap in many .NET code
bases. Track their run time; when finer unit tests cover an area, retire the broad test.
