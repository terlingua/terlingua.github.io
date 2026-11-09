---
title: "First Steps — Is It Worth It, and Where to Start"
questions:
  - id: not-grand
    prompt: "How does knowing refactoring change your view of duplication spread across a system?"
    options:
      - id: of10095
        text: "It is never worth removing"
      - id: ob535a0
        text: "It requires a dedicated rewrite project"
      - id: small-chunks
        text: "Removing it needn't be a grand reengineering effort; you can do it in small chunks as you work"
        correct: true
    explanation: "“You know that removing duplication doesn’t have to be a grand effort such as in reengineering or re-architecting. It’s something that you can do in small chunks as you do your work.”"
  - id: tests-first
    prompt: "Before removing duplication from `AddEmployeeCmd` and `LoginCommand`, what is needed?"
    options:
      - id: o4b32c2
        text: "Management approval"
      - id: od51a2a
        text: "A UML diagram"
      - id: tests
        text: "A set of tests to run after each refactoring"
        correct: true
    explanation: "“The first thing that we need is a set of tests that we’ll run after each refactoring. We’ll cut them out of the description here for brevity, but remember that they are there.”"
  - id: overthinking
    prompt: "What does Feathers realise when he steps back to see the full scope of duplication?"
    options:
      - id: oc6874e
        text: "The duplication is too big to tackle"
      - id: o748aa6
        text: "A new architecture is needed first"
      - id: start-small
        text: "He's over-thinking it — removing small pieces helps and makes larger duplication easier to see"
        correct: true
    explanation: "“Then I realize that I’m really over-thinking it. Removing small pieces of duplication helps, and it makes it easier to see larger areas of duplication later.”"
  - id: writefield
    prompt: "What is the first piece of duplication extracted?"
    options:
      - id: oe880fc
        text: "The constructor"
      - id: string-null
        text: "Writing a string followed by a terminating null — `writeField`"
        correct: true
      - id: o839328
        text: "The header bytes"
    explanation: "“When we write out a string, we also write a terminating null character (0x00).” — “Create a method named writeField that accepts a string and an output stream.”"
  - id: where-start
    prompt: "`c() { a(); a(); b(); a(); b(); b(); }` can be grouped as `aa(); b(); a(); bb();` or `a(); ab(); ab(); b();`. How does Feathers choose?"
    options:
      - id: ocf96cf
        text: "Always group identical calls"
      - id: o40403a
        text: "Always the grouping with fewer calls"
      - id: names
        text: "It doesn't matter much structurally; pick the grouping whose name makes more sense in context"
        correct: true
    explanation: "“The truth is, it doesn’t make much difference structurally.” — “I decide by paying attention to the names that I would use.”"
  - id: reversible
    prompt: "Why is choosing the \"wrong\" grouping not a big risk?"
    options:
      - id: not-final
        text: "Both groupings are better than the original and can be refactored into each other later — these aren't final decisions"
        correct: true
      - id: oe48b48
        text: "Tests will reject the wrong one"
      - id: ob78dc8
        text: "The compiler optimises it away"
    explanation: "“Both groupings are better than what we had, and we can refactor them into the other grouping, if we need to. These aren’t final decisions.”"
  - id: constructor-bug
    prompt: "The printed `AddEmployeeCmd` constructor takes `(name, address, yearlySalary)` but assigns `this.city = city; this.state = state;`. What does that tell a careful reader about the listing?"
    options:
      - id: o310bc0
        text: "Java infers missing parameters"
      - id: inconsistent
        text: "The parameters don't include `city` and `state`, so as printed the listing is inconsistent — the kind of slip tests (and a compiler) would expose"
        correct: true
      - id: o678f17
        text: "It's correct; city and state are static"
    explanation: "The listing shows “public AddEmployeeCmd(String name, String address, int yearlySalary)” followed by “this.city = city; this.state = state;”. Treat it as an illustrative slip; the refactoring lesson is unaffected."
---
## Learning objectives

- Start duplication removal small and let structure emerge
- Choose between alternative groupings by naming

## Connections to today

IDE "Find duplicate code" (Rider) and clone detection in static analysis spot candidates, but
Feathers' process is manual and test-backed: one tiny extraction, run tests, repeat.
