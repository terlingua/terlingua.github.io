---
title: "Pinch Points"
questions:
  - id: definition
    prompt: "What is a pinch point?"
    options:
      - id: narrowing
        text: "A narrowing in an effect sketch where tests against a couple of methods can detect changes in many methods"
        correct: true
      - id: ofbf35e
        text: "A class with only one method"
      - id: o5ea03d
        text: "A performance bottleneck"
    explanation: "“A pinch point is a narrowing in an effect sketch, a place where tests against a couple of methods can detect changes in many methods.”"
  - id: determined-by
    prompt: "What determines pinch points?"
    options:
      - id: o1ff78a
        text: "The number of classes in the package"
      - id: change-points
        text: "The change points — a class with multiple clients may still have a good pinch point for a given set of changes"
        correct: true
      - id: oc89391
        text: "The class hierarchy depth"
    explanation: "“The key thing to remember about pinch points, though, is that they are determined by change points. A set of changes to a class might have a good pinch point even if the class has multiple clients.”"
  - id: needs-reorder
    prompt: "`InventoryControl` also calls `Item.needsReorder`. Does adding a `shippingCarrier` field change the pinch point?"
    options:
      - id: oc1a0dd
        text: "Yes — now there is no pinch point"
      - id: o0bcabc
        text: "Yes — `InventoryControl` becomes the pinch point"
      - id: no-change
        text: "No — the new field doesn't affect `needsReorder`, so `BillingStatement` is still the pinch point"
        correct: true
    explanation: "“Adding a shippingCarrier field to Item doesn’t impact the needsReorder method at all, so BillingStatement is still our pinch point”."
  - id: supplier
    prompt: "After adding supplier get/set used by both `InventoryControl` and `BillingStatement`, what is the pinch point?"
    options:
      - id: oa82663
        text: "`Item.getSupplier` alone"
      - id: two-methods
        text: "`run` and `makeStatement` together — two methods is still narrower than the eight things changed"
        correct: true
      - id: of183ce
        text: "There is none"
    explanation: "“taken together, the run method and the makeStatement method can be seen as a pinch point; together they are just two methods, and that is a narrower place to detect problems than the eight methods and variables that have to be touched”."
  - id: tangled
    prompt: "If an effect sketch looks like a large tangled tree with no pinch point, what should you do?"
    type: multiple
    options:
      - id: close
        text: "If none exist, write tests for individual changes as close as you can"
        correct: true
      - id: oc12cd4
        text: "Give up on testing"
      - id: fewer
        text: "Revisit change points — find pinch points for only one or two changes at a time"
        correct: true
    explanation: "“Maybe we are trying to do too much at once. Consider finding pinch points for only one or two changes at a time. If you can’t find a pinch point at all, just try to write tests for individual changes as close as you can.”"
  - id: key-question
    prompt: "To decide whether testing at one of two places is enough, what key question should you ask?"
    options:
      - id: oed7c5e
        text: "\"Which test runs faster?\""
      - id: o90b2dc
        text: "\"Which class is newer?\""
      - id: sense-it
        text: "\"If I break this method, will I be able to sense it in this place?\""
        correct: true
    explanation: "“The key question to ask is, \"If I break this method, will I be able to sense it in this place?\" The answer depends on how the method is used.”"
---
## Learning objectives

- Identify pinch points from effect sketches
- Handle cases with multiple or no pinch points

## In a nutshell

A pinch point is an interception point that sees many changes. It depends on *which* changes you
plan, so recompute it when the plan changes.
