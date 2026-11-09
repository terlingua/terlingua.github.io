---
title: "Push Down Dependency"
questions:
  - id: when
    prompt: "When is Push Down Dependency preferable to Subclass and Override Method?"
    options:
      - id: o2a66ff
        text: "When the class has no methods"
      - id: pervasive
        text: "When problematic dependencies are pervasive rather than confined to a few method calls"
        correct: true
      - id: oe3c9df
        text: "When there's exactly one bad call"
    explanation: "“If the dependencies are contained in only a few method calls, you can use Subclass and Override Method (401)” — “But if the dependencies are pervasive, Subclass and Override Method might not work.”"
  - id: mechanism
    prompt: "How does Push Down Dependency work?"
    options:
      - id: o69caa1
        text: "Move logic to a static utility"
      - id: abstract-push
        text: "Make the current class abstract, push problematic dependencies down into a new production subclass, and create a testing subclass"
        correct: true
      - id: o0f32e2
        text: "Replace the class with an interface"
    explanation: "“When you use Push Down Dependency, you make your current class abstract. Then you create a subclass that will be your new production class, and you push down all the problematic dependencies into that class.”"
  - id: naming
    prompt: "How should the new production subclass be named?"
    options:
      - id: oc9c36d
        text: "With a `Production` prefix always"
      - id: environment
        text: "With a name that communicates the specific environment of the dependencies, e.g. `WindowsOffMarketTradeValidator`"
        correct: true
      - id: o37641f
        text: "With a version number"
    explanation: "“3. Create a new subclass with a name that communicates the specific environment of those dependencies.”"
  - id: next
    prompt: "Once `OffMarketTradeValidator` has tests, what's the next direction?"
    options:
      - id: o75af92
        text: "Keep the inheritance forever"
      - id: o5afd31
        text: "Remove the tests"
      - id: delegate-ui
        text: "Pull the retry logic up out of the Windows subclass and delegate the remaining UI calls to a new class holding the only UI dependencies"
        correct: true
    explanation: "“When we have tests for OffMarketTradeValidator, we can start to clean up the retry logic and pull it up from the WindowsOffMarketTradeValidator. When only the UI calls are left, we can move toward delegating them to a new class.”"
  - id: ideal
    prompt: "Is using inheritance this way ideal?"
    options:
      - id: o7895cc
        text: "It's never acceptable"
      - id: not-ideal
        text: "No, but it gets part of the logic under test"
        correct: true
      - id: odd99c1
        text: "Yes — it is the target design"
    explanation: "“Is using inheritance in this way ideal? No, but it helps us get part of the logic of a class under test.”"
---
## Learning objectives

- Isolate pervasive platform dependencies in a production subclass

## Connections to today

Platform-specific subclasses (`WindowsFileWatcher`, `LinuxFileWatcher`) are common in .NET too;
`OperatingSystem.IsWindows()` checks scattered through logic are the "before" picture.
