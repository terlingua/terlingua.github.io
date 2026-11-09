---
title: "Break Out Method Object"
questions:
  - id: when
    prompt: "When should you choose Break Out Method Object over Expose Static Method?"
    options:
      - id: o1a035b
        text: "When the method is tiny and pure"
      - id: oa5c85c
        text: "When the class is already testable"
      - id: large-instance
        text: "When the method is large or uses instance data and methods"
        correct: true
    explanation: "“If the method that you need to work with is small and doesn’t use instance data, use Expose Static Method (345)” — “if your method is large or does use instance data and methods, consider using Break Out Method Object.”"
  - id: ctor
    prompt: "What goes into the method object's constructor?"
    options:
      - id: obfb707
        text: "Only primitives"
      - id: o30aa33
        text: "Nothing; use setters"
      - id: ref-and-args
        text: "A reference to the original class (if needed) plus an exact copy of the method's arguments — Preserve Signatures"
        correct: true
    explanation: "“The arguments of the constructor should be a reference to the original class, and the arguments to the original method. We need to Preserve Signatures (312) on the latter.”"
  - id: same-position
    prompt: "\"We still accept a `GDIBrush` we can't instantiate — what good is this?\" How does the technique end up somewhere different?"
    options:
      - id: o445890
        text: "It doesn't; the technique fails here"
      - id: o5bf137
        text: "Make `GDIBrush` static"
      - id: extract-interface
        text: "Once code lives in `Renderer`, Extract Interface on `GDIBrush` (e.g. `PointRenderer`) breaks that dependency entirely"
        correct: true
    explanation: "“If we can’t instantiate GDIBrush in a test harness, we can use Extract Interface to break the dependency on GDIBrush completely.” — “we can call it PointRenderer because drawPoint is the method on GDIBrush that we really need access to”."
  - id: compile-errors
    prompt: "After copying the body into `Renderer::draw()`, what do compile errors tell you?"
    options:
      - id: o39c81c
        text: "Which tests to write"
      - id: oea5e48
        text: "That the technique failed"
      - id: dependencies
        text: "Where the code still uses members of the old class — add getters or make methods public so it compiles"
        correct: true
    explanation: "“If the draw() on Renderer has any references to instance variables or methods from GDIBrush, our compile will fail. To make it succeed, we can make getters for the variables and make the methods that it depends on public.”"
  - id: sick-feeling
    prompt: "Exposing private `drawPoint` publicly gives a \"sick feeling\". Why accept it?"
    options:
      - id: oed3f0c
        text: "Public methods are always fine"
      - id: oe32a8b
        text: "Encapsulation doesn't apply to C++"
      - id: not-end
        text: "It isn't the end — once tests exist, you'll refactor further (e.g. rethink PointRenderer as a class holding a GDIBrush)"
        correct: true
    explanation: "“You might have a sick feeling in the pit of your stomach because we’ve made details that were private in the original class public” — “The important thing to notice is that this isn’t really the end.”"
  - id: variations
    prompt: "Which variations of Break Out Method Object does the sidebar describe?"
    type: multiple
    options:
      - id: methods
        text: "It uses methods — the worst case, use Extract Interface between method object and original class"
        correct: true
      - id: no-ref
        text: "The method uses nothing from the original class — no reference needed"
        correct: true
      - id: data-only
        text: "It uses only data — consider a data-holding class passed as an argument"
        correct: true
      - id: o1a445c
        text: "It uses only static methods — use reflection"
    explanation: "“In the simplest case, the original method doesn’t use any instance variables or methods from the original class.” “In other cases, the method only uses data from the original class.” “The case that I show in this section is the worst case”."
  - id: locals
    prompt: "What extra testing advantage comes from moving the method into its own class?"
    options:
      - id: instance-vars
        text: "Local variables can become instance variables you can sense through"
        correct: true
      - id: o4b35cd
        text: "It compiles faster"
      - id: oca4623
        text: "It removes all dependencies automatically"
    explanation: "“Local variables in the old method can become instance variables in the new class. Often that makes it easier to break dependencies and move the code to a better state.”"
---
## Learning objectives

- Move a monster method into its own class safely
- Break the back-reference with Extract Interface

## Connections to today

Command handlers (`PlaceOrderHandler.Handle(command)`) in MediatR-style architectures are method
objects by another name — which is why they're so easy to test.
