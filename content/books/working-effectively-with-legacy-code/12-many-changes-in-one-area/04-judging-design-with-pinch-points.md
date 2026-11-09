---
title: "Judging Design with Pinch Points"
questions:
  - id: encapsulation
    prompt: "What is a pinch point, really, from a design point of view?"
    options:
      - id: o9e0882
        text: "A deployment unit"
      - id: boundary
        text: "A natural encapsulation boundary — a narrow funnel for all effects of a large piece of code"
        correct: true
      - id: oa5d17e
        text: "A sign of bad design"
    explanation: "“A pinch point is a natural encapsulation boundary. When you find a pinch point, you’ve found a narrow funnel for all of the effects of a large piece of code.”"
  - id: definition-enc
    prompt: "How does Feathers phrase \"pretty much the definition of encapsulation\"?"
    options:
      - id: internals
        text: "We don't have to care about the internals, but when we do, we don't have to look at the externals to understand them"
        correct: true
      - id: o188e15
        text: "Every class has an interface"
      - id: oc58ef5
        text: "All fields are private"
    explanation: "“This is pretty much the definition of encapsulation: We don’t have to care about the internals, but when we do, we don’t have to look at the externals to understand them.”"
  - id: tokenizer
    prompt: "In the `Parser` example, what does the effect sketch reveal?"
    options:
      - id: o41d405
        text: "`Parser` should be a singleton"
      - id: o4f3cd1
        text: "`parseExpression` should be private"
      - id: hidden-class
        text: "`getToken` and `hasMoreTokens` hide `stringToParse` and `currentPosition` from `parseExpression` — a `Tokenizer` class waiting to be extracted"
        correct: true
    explanation: "“parseExpression depends on getToken and hasMoreTokens, but it doesn’t directly depend on stringToParse or currentPosition” — “We can extract those methods and fields to a class called Tokenizer and end up with a simpler Parser class.”"
  - id: exercise
    prompt: "In the suggested exercise, why forget the names of the bubbles?"
    options:
      - id: od98837
        text: "Names are always wrong in legacy code"
      - id: ob39601
        text: "To make the sketch smaller"
      - id: grouping
        text: "To see how bubbles group into natural boundaries without existing names biasing you; then name the cluster"
        correct: true
    explanation: "“create an effect sketch for changes in a large class and forget about the names of the bubbles. Just look at how they are grouped. Are there any natural encapsulation boundaries?”"
  - id: team
    prompt: "Why does Feathers suggest doing this naming exercise with teammates?"
    options:
      - id: obf3147
        text: "To share blame"
      - id: common-view
        text: "Naming discussions help the team develop a common view of what the system is and can become"
        correct: true
      - id: o9428ce
        text: "Pairing is required for refactoring"
    explanation: "“The discussions that you have about naming have benefits far beyond the work that you are currently doing. They help you and your team develop a common view of what the system is and what it can become.”"
---
## Learning objectives

- Read pinch points as encapsulation boundaries
- Discover hidden classes from effect-sketch clusters

## Connections to today

Pinch points often mark good module or service boundaries. Before splitting a monolith, look for
pinch points: if you can't find one, a network boundary will make things worse, not better.
