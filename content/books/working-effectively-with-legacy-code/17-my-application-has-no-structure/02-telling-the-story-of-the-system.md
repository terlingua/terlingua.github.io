---
title: "Telling the Story of the System"
questions:
  - id: how
    prompt: "How does \"telling the story of the system\" work?"
    options:
      - id: few-concepts
        text: "One person asks \"What is the architecture?\"; the other explains with only a few concepts, as if to someone who knows nothing, then adds the next most important things"
        correct: true
      - id: o5e87bf
        text: "An architect writes a 50-page document"
      - id: o666f8c
        text: "The team reads the code aloud"
    explanation: "“Then the other person tries to explain the architecture of the system using only a few concepts, maybe as few as two or three.” — “Next, you pick the next most important things to say about the system.”"
  - id: lying
    prompt: "The simplified story feels like lying. Why is it still valuable?"
    type: multiple
    options:
      - id: ideal
        text: "It shows what would have been ideal versus what exists as expediency"
        correct: true
      - id: important
        text: "It forces you to think about what's most important to communicate"
        correct: true
      - id: oc9d037
        text: "It replaces the need for tests"
      - id: roadmap
        text: "It serves as a roadmap for where to add features and makes the system less scary"
        correct: true
    explanation: "“it helps everyone understand what would’ve been ideal and what things are there as expediencies.” “it really forces you to think about what is important in the system.” “having a simple story of how a system works just serves as a roadmap”."
  - id: choose
    prompt: "How can the story guide a choice between two implementation options?"
    options:
      - id: obcee01
        text: "Prefer the newer technology"
      - id: ob2753d
        text: "Prefer the option with fewer lines"
      - id: less-lie
        text: "Prefer the option that makes the brief story feel like less of a lie"
        correct: true
    explanation: "“some changes fall more in line with the story. That is, they make the briefer story feel like less of a lie. If you have to choose between two ways of doing something, the story can be a good way to see which one will lead to an easier-tounderstand system.”"
  - id: junit-feature
    prompt: "To report tests that call no assertions, why does Feathers prefer having TestResult count assertions over adding `buildUsageReport` to `TestCase`?"
    options:
      - id: o82f38b
        text: "It runs faster"
      - id: still-true
        text: "Putting reporting on TestCase is a bolder \"lie of omission\"; counting assertions in TestResult keeps the story substantially true"
        correct: true
      - id: oe163dc
        text: "TestCase can't have new methods"
    explanation: "“The alternative, putting the responsibility for running a bunch of cases and building a report from them on TestCase, would be a bolder lie” — “Our first story is generalized a little bit more but at least it is still substantially true.”"
  - id: abstracting
    prompt: "What are you really doing when you simplify and rip away detail to describe a system?"
    options:
      - id: abstraction
        text: "Abstracting — which often reveals new abstractions"
        correct: true
      - id: o058a82
        text: "Hiding bugs"
      - id: o2e4f9d
        text: "Writing requirements"
    explanation: "“When we simplify and rip away detail to describe a system, we are really abstracting. Often when we force ourselves to communicate a very simple view of a system, we can find new abstractions.”"
  - id: bad
    prompt: "If a system isn't as simple as the simplest story about it, is it bad?"
    options:
      - id: no-guidance
        text: "No — systems get more complicated as they grow; the story gives guidance"
        correct: true
      - id: o8cfaca
        text: "Yes, the story must be expanded until exact"
      - id: o18de2a
        text: "Yes, it must be rewritten"
    explanation: "“If a system isn’t as simple as the simplest story we can tell about it, does that mean that it’s bad? No. Invariably, as systems grow, they get more complicated. The story gives us guidance.”"
  - id: errors-failures
    prompt: "In JUnit's terminology as described here, what's the difference between errors and failures?"
    options:
      - id: o4b17c2
        text: "Errors are compile errors"
      - id: diff
        text: "Failures are failed checks; errors are problems in the test that aren't explicitly checked for"
        correct: true
      - id: o35a364
        text: "They're the same"
    explanation: "“(Errors are problems that occur in the test that aren’t explicitly checked for. Failures are failed checks.)”"
---
## Learning objectives

- Run a "story of the system" session
- Use the story to choose between design alternatives

## Connections to today

Try the story with your own app: "Terlingua has two main pieces: a build-time content processor that
turns Markdown into JSON, and a Blazor WebAssembly app that reads that JSON." Then list the lies
you told — each one is a design conversation.
