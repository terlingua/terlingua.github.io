---
title: "The Case of the Onion Parameter"
questions:
  - id: hope
    prompt: "What keeps you from total despair when objects need objects that need objects?"
    options:
      - id: o0b0b6c
        text: "The garbage collector"
      - id: base-case
        text: "There must be at least one class that doesn't require other objects as arguments, or the system could never have compiled"
        correct: true
      - id: oe9dd03
        text: "Dependency injection containers"
    explanation: "“there has to be at least one class that doesn’t require objects of another class as arguments. If there isn’t, there is no way the system could ever have compiled.”"
  - id: approach
    prompt: "What is the way to handle an onion parameter?"
    options:
      - id: o549c60
        text: "Build the whole onion in every test"
      - id: what-needed
        text: "Ask what you really need; Pass Null if nothing, or Extract Interface/Implementer on the most immediate dependency for a fake"
        correct: true
      - id: o5a0995
        text: "Use a link seam"
    explanation: "“If we don’t need anything from them in the tests, we can Pass Null (111). If we just need some rudimentary behavior, we can use Extract Interface (362) or Extract Implementer (356) on the most immediate dependency”."
  - id: java-interface
    prompt: "`SchedulingTask`'s public methods all live in its base class `SerialTask`. In Java, must you extract an interface on `SerialTask` too?"
    options:
      - id: no-need
        text: "No — an interface for `SchedulingTask` can include methods inherited from `SerialTask`"
        correct: true
      - id: o7a3d65
        text: "It's impossible in Java"
      - id: o87647c
        text: "Yes, both classes need interfaces"
    explanation: "“Can we use Extract Interface (362) on SchedulingTask, or do we have to use it on SerialTask, too? In Java, we don’t. We can create an interface for SchedulingTask that includes methods from SerialTask also.”"
  - id: cpp-delegate
    prompt: "In C++, what extra code is needed so `SchedulingTask` isn't abstract after inheriting the pure virtual `run()` from the interface class?"
    options:
      - id: of80a4a
        text: "Nothing"
      - id: delegate
        text: "A `run()` in `SchedulingTask` that delegates to `SerialTask::run()`"
        correct: true
      - id: od17ab9
        text: "A virtual destructor only"
    explanation: "“we’d need to provide a body for run() in SchedulingTask, which delegates to the run() from SerialTask.” The code shows `virtual void run() { SerialTask::run(); }`."
  - id: general
    prompt: "What general lesson closes this case?"
    options:
      - id: interfaces-systematic
        text: "Wherever you can create interfaces or interface-like classes, you can use them systematically to break dependencies"
        correct: true
      - id: o617d81
        text: "Avoid inheritance entirely"
      - id: o723d4c
        text: "Prefer link seams in Java"
    explanation: "“In any language where we can create interfaces or classes that act like interfaces, we can systematically use them to break dependencies.”"
---
## Learning objectives

- Peel an onion parameter at the nearest layer only
- Extract an interface that includes inherited members

## Connections to today

Test data builders (`new SchedulingTaskBuilder().WithResolver(...).Build()`) and AutoFixture-style
tools hide onion construction in tests. They help readability, but Feathers' question still
applies first: what does the test really need?
