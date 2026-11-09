---
title: "Hyperaware Editing"
questions:
  - id: fatigue
    prompt: "How is code different from physical building materials, according to the chapter opening?"
    options:
      - id: never-breaks
        text: "Code doesn't fatigue; if you leave it alone it never breaks — faults come from someone editing it"
        correct: true
      - id: oc64342
        text: "Code breaks randomly over time"
      - id: o0aa8a7
        text: "Code wears out with use"
    explanation: "“Code is different. If you leave it alone, it never breaks.” — “the only way it gets a fault is for someone to edit it.”"
  - id: keystrokes
    prompt: "Into which two categories can every keystroke be classified?"
    options:
      - id: of5e30d
        text: "Code or comments"
      - id: oa2c4ea
        text: "Fast or slow"
      - id: behaviour
        text: "It either changes the behaviour of the software or it doesn't"
        correct: true
    explanation: "“When we sit down at a keyboard, we can classify every keystroke that we make into one of two categories. The keystroke either changes the behavior of the software or it doesn’t.”"
  - id: string-literal
    prompt: "Typing inside a string literal — does it change behaviour?"
    options:
      - id: o68eec4
        text: "Always"
      - id: usually
        text: "Most of the time, yes — unless the literal is in code that is never called"
        correct: true
      - id: o6e7b34
        text: "Never"
    explanation: "“Typing text in a string literal? That does, most of the time. If the string literal is in code that is never called, behavior won’t change.”"
  - id: numeric
    prompt: "Is changing a numeric literal used in an expression a refactoring?"
    options:
      - id: o4b79ff
        text: "Yes, it's a micro-refactoring"
      - id: oc33665
        text: "Only if the number gets smaller"
      - id: functional
        text: "No — it's a functional change, and you should know that when you type it"
        correct: true
    explanation: "“Changing a numeric literal in an expression that is used in your code isn’t refactoring; it’s a functional change, and it’s important to know that when you are typing.”"
  - id: fosters
    prompt: "Which practices foster hyperaware editing?"
    type: multiple
    options:
      - id: obff78d
        text: "Longer code reviews after the fact"
      - id: tests
        text: "Tests (especially TDD with sub-second runs)"
        correct: true
      - id: pairing
        text: "Pair programming"
        correct: true
    explanation: "“Tests foster hyperaware editing. Pair programming does also.” and “When you can get your code into a test harness and run tests against it in less than a second, you can run the tests whenever you need to”."
  - id: exhausting
    prompt: "Is hyperaware editing exhausting?"
    options:
      - id: o538b0d
        text: "Only for juniors"
      - id: od3460f
        text: "Yes, it should be used sparingly"
      - id: flow
        text: "Not frustrating — it's a flow state; what's tiring is working without feedback and fearing silent breakage"
        correct: true
    explanation: "“Hyperaware editing is a flow state” and “Personally, I get far more tired when I’m not getting any feedback.”"
  - id: prediction
    prompt: "Feathers predicted an IDE that runs a chosen set of tests at every keystroke. What category of modern tooling fulfils this?"
    options:
      - id: continuous
        text: "Continuous/live unit testing tools that rerun affected tests as you edit"
        correct: true
      - id: o661ece
        text: "Nightly builds"
      - id: ob1438f
        text: "Syntax highlighting"
    explanation: "He wrote: “someone will soon develop an IDE that allows you to specify a set of tests that will run at every keystroke.” and “Edit-triggered testing is the next step.” Tools such as Visual Studio Live Unit Testing, NCrunch and `dotnet watch test` are versions of this."
---
## Learning objectives

- Classify edits as behaviour-changing or not
- Use fast feedback to stay aware of each change

## Connections to today

AI completions insert many keystrokes at once. Hyperaware editing means reading each suggestion as
if you typed it: which parts change behaviour?
