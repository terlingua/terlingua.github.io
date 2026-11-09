---
title: "Tools for Effect Reasoning — Know Your Language"
questions:
  - id: most-important
    prompt: "What is the most important tool for effect reasoning?"
    options:
      - id: o9c2caf
        text: "A static analysis tool"
      - id: language
        text: "Knowledge of your programming language — especially its \"firewalls\" that stop effect propagation"
        correct: true
      - id: oc37232
        text: "A debugger"
    explanation: "“The most important tool that we have in our arsenal is knowledge of our programming language. In every language, there are little \"firewalls,\" things that prevent effect propagation.”"
  - id: coordinate-diff
    prompt: "What is the subtle difference between the two `Coordinate` classes, and why does it matter?"
    options:
      - id: package-scope
        text: "The second's fields have package scope, so package clients and subclasses may access them directly — you must look beyond the class"
        correct: true
      - id: oc85fe0
        text: "The second has a different distance formula"
      - id: o79bf99
        text: "The second is abstract"
    explanation: "“In the first version of the class, the x and y variables were private. In the second, they had package scope.” — “clients in the package could be accessing the variables directly.”"
  - id: spot-bug
    prompt: "Look closely at both printed `Coordinate` constructors. What would a characterization test of `distance` reveal?"
    options:
      - id: o5b539b
        text: "`distance` returns negative values"
      - id: y-equals-x
        text: "The constructor assigns `this.y = x`, so `y` always equals `x`; a characterization test would pin that (surprising) behaviour"
        correct: true
      - id: o34c7c1
        text: "Nothing unusual"
    explanation: "Both listings contain “this.x = x; this.y = x;”. Whether it's a typo in the book or a real bug, a characterization test records what the code actually does — Chapter 13 says to document it and decide, not silently fix it."
  - id: mutable
    prompt: "Why can't you trust C++ `const` methods blindly?"
    options:
      - id: of72f62
        text: "`const` only applies to static methods"
      - id: o40fcde
        text: "`const` is ignored by compilers"
      - id: mutable-kw
        text: "Fields declared `mutable` can be modified inside `const` methods"
        correct: true
    explanation: "“In C++, when the keyword mutable is used in a declaration, it means that those variables can be modified in const methods.” and “Taking const to mean const in C++ without really checking can be dangerous.”"
  - id: csharp-firewall
    prompt: "In C#, which of these act as effect \"firewalls\" you can rely on when reasoning — and which can be circumvented in the way Feathers warns about?"
    options:
      - id: readonly-reflection
        text: "`private` and `readonly` fields narrow effects, but reflection or `UnsafeAccessor` can still reach them — so check before assuming"
        correct: true
      - id: o2b5324
        text: "`readonly` makes an object deeply immutable"
      - id: o523415
        text: "`internal` hides members from everything"
    explanation: "Feathers: “Taking const to mean const in C++ without really checking can be dangerous. The same holds true for other language constructs that can be circumvented.” In C#, a `readonly` reference field can still point to a mutable object."
---
## Learning objectives

- Use access modifiers and immutability as effect firewalls
- Know which language guarantees can be circumvented

## Know your language — C# edition

| Construct | Narrows effects? | Caveat |
|-----------|------------------|--------|
| `private` field | Yes | Reflection, `UnsafeAccessor`, `InternalsVisibleTo` for `internal` |
| `readonly` field | Reference can't change | Object it points to may be mutable |
| `record` / `init` | Shallow immutability | Collections inside may still mutate |
| `in` / `ref readonly` parameters | Caller's variable protected | Reference types' contents still mutable |
| `static` field | Widens effects | Shared across tests running in parallel |
