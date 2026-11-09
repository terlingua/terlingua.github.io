---
title: "The Liskov Substitution Principle and Normalized Hierarchies"
questions:
  - id: square
    prompt: "`Rectangle r = new Square(); r.setWidth(3); r.setHeight(4);` — if `Square` keeps itself square, what does a client expecting rectangle semantics observe?"
    options:
      - id: sixteen
        text: "Area 16 — a surprise"
        correct: true
      - id: o8b9538
        text: "Area 12, as expected"
      - id: o731b58
        text: "A compile error"
    explanation: "“Anyone who expects that all rectangles will have an area of 12 when their width is set to 3 and their height is set to 4 is in for a surprise. They’d get 16 instead.”"
  - id: lsp-def
    prompt: "What does LSP require?"
    options:
      - id: oa4484b
        text: "Subclasses must add at least one method"
      - id: substitutable
        text: "Objects of subclasses must be substitutable for objects of their superclasses; clients shouldn't need to know they have a subclass"
        correct: true
      - id: o3cbad9
        text: "Base classes must be abstract"
    explanation: "“Objects of subclasses should be substitutable for objects of their superclasses throughout our code.” and “clients of a class should be able to use objects of a subclass without having to know that they are objects of a subclass.”"
  - id: mechanical
    prompt: "Can LSP violations be avoided mechanically?"
    options:
      - id: o73a052
        text: "Yes, by sealing all classes"
      - id: ofc8561
        text: "Yes, with a static analyser"
      - id: depends-clients
        text: "No — conformance depends on the clients and what they expect"
        correct: true
    explanation: "“There aren’t any mechanical ways to completely avoid LSP violations. Whether a class is LSP conformant depends upon the clients that it has and what they expect.”"
  - id: rules
    prompt: "Which rules of thumb help avoid LSP violations?"
    type: multiple
    options:
      - id: ofc511d
        text: "Always make every method virtual"
      - id: call-base
        text: "If you do, try to call the overridden method from the override"
        correct: true
      - id: o3a1432
        text: "Never use inheritance"
      - id: avoid-concrete
        text: "Whenever possible, avoid overriding concrete methods"
        correct: true
    explanation: "“1. Whenever possible, avoid overriding concrete methods. 2.If you do, see if you can call the method you are overriding in the overriding method.”"
  - id: confusion
    prompt: "Why does overriding concrete `getFromAddress` make code confusing?"
    options:
      - id: misread
        text: "A reader sees a `MessageForwarder` reference, reads its `getFromAddress`, and doesn't realise an `AnonymousMessageForwarder`'s version runs"
        correct: true
      - id: odb4f97
        text: "It slows down method dispatch"
      - id: ocfd271
        text: "It prevents unit testing"
    explanation: "“Someone can notice a MessageForwarder reference in code, take a look at the MessageFowarder class, and think that the code it has for getFromAddress is executed. They might have no idea that the reference is pointing to an AnonymousMessageForwarder”."
  - id: normalized
    prompt: "What defines a normalized hierarchy?"
    options:
      - id: o0dfebb
        text: "Every hierarchy has exactly two levels"
      - id: o3a93d2
        text: "Every class is in its own namespace"
      - id: one-impl
        text: "No class has more than one implementation of a method — no overrides of inherited concrete methods"
        correct: true
    explanation: "“In a normalized hierarchy, no class has more than one implementation of a method. In other words, none of the classes has a method that overrides a concrete method it inherited from a superclass.”"
  - id: normalize-forwarder
    prompt: "How would you normalise the `MessageForwarder` hierarchy if you keep inheritance?"
    options:
      - id: o32848e
        text: "Make `getFromAddress` static"
      - id: abstract
        text: "Make `MessageForwarder` abstract with an abstract `getFromAddress`, implemented by `AddressPreservingForwarder` and `AnonymousForwarder`"
        correct: true
      - id: ob8d9fa
        text: "Merge all subclasses into one"
    explanation: "“we could have made MessageForwarder abstract, given it an abstract method for getFromAddress, and let the subclasses provide concrete bodies.” Figure 8.7 shows `AddressPreservingForwarder` and `AnonymousForwarder`."
  - id: always
    prompt: "Must hierarchies always be normalized?"
    options:
      - id: occasionally
        text: "No — a few concrete overrides don't hurt if they don't violate LSP; think about distance from normalized form periodically"
        correct: true
      - id: oa05c8b
        text: "Yes, always"
      - id: o8f0a28
        text: "No, it never matters"
    explanation: "“A few concrete overrides every once in a while don’t hurt, as long as it doesn’t cause a Liskov substitution principle violation. However, it’s good to think about how far classes are from normalized form every once in a while”."
---
## Learning objectives

- Diagnose an LSP violation from client expectations
- Apply the two rules of thumb for overriding
- Normalise a hierarchy

## In a nutshell

Programming by Difference often overrides concrete methods — the classic route to LSP surprises.
Keep the override short-lived, or normalise: abstract base, one implementation per method.

## Connections to today

- C#'s `sealed override`, `abstract` members and the `virtual`-opt-in default nudge toward
  normalized hierarchies.
- Records with `with` expressions sidestep the Rectangle/Square mutator problem by not having
  setters.
