---
title: "Replace Global Reference with Getter"
questions:
  - id: static-is-global
    prompt: "\"It's just a static method call, not a global!\" Why does Feathers count `Inventory.getInventory()` as a global?"
    options:
      - id: o850704
        text: "It isn't a global"
      - id: o3742d7
        text: "Because it's public"
      - id: class-global
        text: "In Java the class itself is a global object, and it must reference some state to do its work"
        correct: true
    explanation: "“For our purposes, that counts as a global. In Java, the class itself is a global object, and it seems that it must reference some state to be capable of doing its work”."
  - id: access
    prompt: "What access level should the new getter have?"
    options:
      - id: o84a892
        text: "Static"
      - id: overridable
        text: "Loose enough to override in a subclass — e.g. protected"
        correct: true
      - id: o47f908
        text: "Private"
    explanation: "“Note that we make it protected so that we can override it under test.” Step 2: “Make sure that the access protection of the method is loose enough for you to be able to override the getter in a subclass.”"
  - id: singleton
    prompt: "Because `Inventory` is a singleton, what else is needed to create `FakeInventory`?"
    options:
      - id: od6243e
        text: "Make it final"
      - id: protected-ctor
        text: "Make `Inventory`'s constructor protected rather than private"
        correct: true
      - id: of80a4a
        text: "Nothing"
    explanation: "“Because Inventory is a singleton, we have to make its constructor protected rather than private.”"
  - id: vs-call
    prompt: "When is this preferred over Extract and Override Call?"
    options:
      - id: o82d85d
        text: "When there is exactly one call"
      - id: o6e7b34
        text: "Never"
      - id: many-calls
        text: "When there are many calls against the same global"
        correct: true
    explanation: "From Extract and Override Call: “I tend to use it unless there are many different calls against the same global. If there are, I often use Replace Global Reference with Getter (399) instead.”"
  - id: steps
    prompt: "What are the four steps?"
    options:
      - id: od0d675
        text: "Make the global private → add a setter → reset in tearDown"
      - id: four
        text: "Identify the global → write an overridable getter → replace references with the getter → override it in a testing subclass"
        correct: true
      - id: of3aa8f
        text: "Delete the global → add a field → inject"
    explanation: "“1. Identify the global reference that you want to replace. 2. Write a getter for the global reference.” “3. Replace references to the global with calls to the getter. 4. Create a testing subclass and override the getter.”"
---
## Learning objectives

- Funnel global access through an overridable getter

## Connections to today

```csharp
protected virtual Inventory Inventory => Inventory.Current;
```

A virtual property is the C# getter. If `Inventory` is sealed or a static class, combine with Extract
Interface or Introduce Instance Delegator.
