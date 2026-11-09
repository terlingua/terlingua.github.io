---
title: "Reasoning About Effects"
questions:
  - id: why-not-enough
    prompt: "Why isn't \"write tests for each method you change\" always enough in legacy code?"
    options:
      - id: o009cbb
        text: "Tests for changed methods are always wrong"
      - id: elsewhere
        text: "A change in one place can affect behaviour somewhere else, which you'd never know about without a test there"
        correct: true
      - id: oac0dcc
        text: "Methods can't be tested individually"
    explanation: "“A change in one place can affect behavior someplace else; unless we have a test in place, we might never know about it.”"
  - id: chain
    prompt: "If you change `SCALE_FACTOR` from 3 to 4 in `getBalancePoint()`, what is affected?"
    options:
      - id: o19b1eb
        text: "Only `getBalancePoint()`"
      - id: oddd32b
        text: "Every method in the program"
      - id: callers-chain
        text: "The method's result, and potentially its callers and their callers up to a system boundary — but not code that never calls it"
        correct: true
    explanation: "“It could also change the results of methods that call that method, and so on, all the way back to some system boundary. Despite this, many parts of the code won’t have different behavior.”"
  - id: cppclass-list
    prompt: "After a `CppClass` is created, which changes could affect its methods' results?"
    type: multiple
    options:
      - id: o9a87d1
        text: "Someone modifies the name string"
      - id: alter-element
        text: "Someone alters or replaces an object in the declarations list"
        correct: true
      - id: add-list
        text: "Someone adds elements to the declarations list passed to the constructor (it's held by reference)"
        correct: true
    explanation: "“Someone could add additional elements to the declarations list after passing it to the constructor. Because the list is held by reference” and “Someone can alter one of the objects held in the declarations list or replace one of its elements”. The name can't change because “in Java, String objects are immutable.”"
  - id: sketch-rule
    prompt: "What is the key rule for drawing an effect sketch?"
    options:
      - id: obe271e
        text: "One box per class"
      - id: bubbles
        text: "A bubble for each variable that can be affected and each method whose return value can change, with arrows to what changes because of them"
        correct: true
      - id: o22cc4d
        text: "Use full UML notation"
    explanation: "“The key is to have a separate bubble for each variable that can be affected and each method whose return value can change.” — “draw an arrow to everything whose value can change at runtime because of them.”"
  - id: goodness
    prompt: "What does Feathers propose as one measure of goodness in software?"
    options:
      - id: o76dd3e
        text: "High cyclomatic complexity"
      - id: oe78047
        text: "Few classes"
      - id: sum-simple
        text: "Complicated effects on the outside world are the sum of a much simpler set of effects in the code"
        correct: true
    explanation: "“one measure of goodness in software is that rather complicated effects on the outside world are the sum of a much simpler set of effects in the code.”"
  - id: classreader
    prompt: "Why can we conclude that a `CppClass`'s declarations won't change after creation?"
    options:
      - id: populated-before
        text: "The list is only populated in `matchVirtualDeclaration` during parsing, before construction, and `Declaration`s and tokens are immutable"
        correct: true
      - id: oe0fcf7
        text: "The list is declared `final`"
      - id: o92270f
        text: "Java lists are immutable"
    explanation: "“It looks like all of the changes that happen to this list happen before the CppClass object is created.” and “nothing else can change its state after it is created, so we can feel pretty comfortable saying that when a CppClass object is created, its declaration list and the list’s contents aren’t going to change.”"
  - id: forward-vs-backward
    prompt: "How does legacy-change reasoning differ from debugging?"
    options:
      - id: o814f17
        text: "Debugging needs no reasoning"
      - id: o2798b7
        text: "They're identical"
      - id: forward
        text: "Debugging reasons backward from a problem to its source; legacy change asks how a change could affect results — reasoning forward"
        correct: true
    explanation: "“we have a debugging problem and we have to reason backward from the problem to its source.” For legacy work: “If we make a particular change, how could it possibly affect the rest of the results of the program?” — “This involves reasoning forward from points of change.”"
---
## Learning objectives

- Trace a change's chain of effects to system boundaries
- Draw an effect sketch
- Use immutability to prune effect analysis

## Connections to today

- Feathers wished for an IDE that lists everything affected by selected code. "Find all
  references", call hierarchy and Roslyn-based dependency views go part way; none fully trace
  effects through mutable state.
- `readonly`, `init`-only properties, records and `IReadOnlyList<T>` turn many effect arrows into
  "can't happen", shrinking the sketch.
