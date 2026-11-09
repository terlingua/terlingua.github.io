---
title: "The Manual Refactoring Challenge and Introduce Sensing Variable"
questions:
  - id: errors
    prompt: "Which common mistakes can happen when extracting methods by hand?"
    type: multiple
    options:
      - id: wrong-params
        text: "Mistakes passing parameters or assigning return values, including wrong types"
        correct: true
      - id: forget-var
        text: "Forgetting to pass a variable and declaring it locally instead"
        correct: true
      - id: o37933b
        text: "Making the method too short"
      - id: hide-override
        text: "Naming the method so it hides or overrides a base-class method"
        correct: true
    explanation: "“1. We can forget to pass a variable into the extracted method.” “2. We could give the extracted method a name that hides or overrides a method with the same name in a base class.” “3. We could make a mistake when we pass in parameters or assign return values.”"
  - id: compiler-miss
    prompt: "When won't the compiler catch a forgotten variable?"
    options:
      - id: o6e7b34
        text: "Never"
      - id: same-name
        text: "When it has the same name as an instance variable"
        correct: true
      - id: o9dc1a9
        text: "When the method is private"
    explanation: "“Often the compiler tells us about the missing variable (unless it has the same name as an instance variable)”."
  - id: sensing-def
    prompt: "What is Introduce Sensing Variable?"
    options:
      - id: temp-var
        text: "Add a variable to the class to sense conditions in the method being refactored, then remove it afterward"
        correct: true
      - id: ob0620d
        text: "Add logging to production"
      - id: o0ff179
        text: "Use a debugger watch"
    explanation: "“Sometimes it helps to add a variable to a class and use it to sense conditions in the method that we want to refactor. When we’ve done the refactoring that we need to do, we can get rid of that variable”."
  - id: still-engineer
    prompt: "With `nodeAdded` in place, what work remains before extracting `isBasicChild`?"
    options:
      - id: o5bca95
        text: "Delete the condition"
      - id: of80a4a
        text: "Nothing"
      - id: inputs
        text: "Engineer inputs that drive the condition both ways and assert on the flag"
        correct: true
    explanation: "“With that variable in place, we still have to engineer the input to produce a case that covers that condition. When we do, we can extract that piece of logic, and our tests should still pass.”"
  - id: depth
    prompt: "Why didn't Feathers test every branch of the extracted condition?"
    options:
      - id: confident
        text: "He was confident about extracting the whole condition intact; the tests just verified it stayed on the code path"
        correct: true
      - id: o917d22
        text: "Conditions can't be tested"
      - id: odf688a
        text: "He forgot"
    explanation: "“I felt pretty confident that I could extract the entire body of the condition without introducing errors, so I didn’t test all of the logic of the condition. These tests just provided a quick way of checking to make sure that the condition was still part of the code path after the extraction.”"
  - id: lifetime
    prompt: "How long should sensing variables live?"
    options:
      - id: o95a64b
        text: "Remove after each single extraction"
      - id: session
        text: "Over a series of refactorings; delete them (and refactor or delete their tests) after the session"
        correct: true
      - id: o3508f9
        text: "Forever, as documentation"
    explanation: "“it is a good idea to keep them in the class over a series of refactorings and delete them only after your refactoring session.” Then “I end up deleting these tests or refactoring them so that they test the methods I extract”."
  - id: desnarl
    prompt: "How can sensing variables help with snarled methods?"
    options:
      - id: o6839db
        text: "They only work on bulleted methods"
      - id: progressive
        text: "Progressively extract conditionals or their bodies from deep nesting, sensing each new method until de-snarled"
        correct: true
      - id: ocf8478
        text: "They flatten code automatically"
    explanation: "“you can also use them to progressively de-snarl methods.” — “we can use sensing variables to extract conditional statements or extract bodies of conditional statements into new methods.”"
---
## Learning objectives

- Recognise the common manual-extraction mistakes
- Use sensing variables to pin a path before extracting

## Connections to today

```csharp
internal bool NodeAdded { get; private set; }
```

An `internal` sensing property plus `InternalsVisibleTo` keeps the temporary hook out of the public
API. Delete it at the end of the session.
