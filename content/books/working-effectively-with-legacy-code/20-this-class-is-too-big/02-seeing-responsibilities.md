---
title: "Seeing Responsibilities — Group Methods, Hidden Methods, Decisions"
questions:
  - id: method-grouping
    prompt: "What questions drive method grouping?"
    options:
      - id: o44602c
        text: "\"Who wrote it?\" and \"When?\""
      - id: why-here
        text: "\"Why is this method here?\" and \"What is it doing for the class?\""
        correct: true
      - id: o244211
        text: "\"How long is it?\""
    explanation: "“The key questions I asked were \"Why is this method here?\" and \"What is it doing for the class?\" Then I grouped them into lists”."
  - id: design-skill
    prompt: "Why does Feathers say legacy code offers *more* opportunity for design skill than new features?"
    options:
      - id: oe876d7
        text: "New features need no design"
      - id: real-context
        text: "You can see the code that will be affected, so trade-offs and structure are judged in a real context"
        correct: true
      - id: od0222f
        text: "Legacy code is simpler"
    explanation: "“It is easier to talk about design tradeoffs when you can see the code that will be affected, and it is also easier to see whether structure is appropriate in a given context because the context is real and right in front of us.”"
  - id: not-all
    prompt: "When grouping methods, must you categorise every method into new classes?"
    options:
      - id: o188550
        text: "Yes, all methods must be categorised"
      - id: o4089b5
        text: "Only private ones"
      - id: some
        text: "No — find some that share an off-to-the-side responsibility, then extract when you next modify one"
        correct: true
    explanation: "“you don’t have to categorize all of the names into new classes.” — “Wait until you have to modify one of the methods you’ve categorized, and then decide whether you want to extract a class at that point.”"
  - id: poster
    prompt: "How can method grouping become a team exercise?"
    options:
      - id: o2e3c15
        text: "Assign one person to decide"
      - id: posters
        text: "Post method lists of major classes in the team room; members mark groupings over time and discuss"
        correct: true
      - id: o3dda09
        text: "Generate it with a tool and archive it"
    explanation: "“Put up poster boards in your team room with lists of the method names for each of your major classes. Team members can mark up the posters over time, showing different groupings of methods.”"
  - id: hidden
    prompt: "What does a class with many private and protected methods often indicate?"
    options:
      - id: obba16d
        text: "A need for reflection"
      - id: oc1d1f2
        text: "Good encapsulation"
      - id: dying
        text: "Another class in the class dying to get out"
        correct: true
    explanation: "“If a class has many of them, it often indicates that there is another class in the class dying to get out.”"
  - id: tokenizer-public
    prompt: "Making `nextTerm` and `hasMoreTerms` public on `RuleParser` feels odd. Why is it fine on a `TermTokenizer`?"
    options:
      - id: still-private
        text: "They are the tokenizer's natural interface, and `RuleParser` still uses the tokenizer privately — encapsulation isn't lost"
        correct: true
      - id: od36940
        text: "Tokenizers don't need encapsulation"
      - id: oed3f0c
        text: "Public methods are always fine"
    explanation: "“it is far less odd—and, actually, perfectly fine—to make them public methods on a TermTokenizer class. This doesn’t make RuleParser any less encapsulated.”"
  - id: decisions
    prompt: "What is heuristic"
    options:
      - id: o012b66
        text: "Conditional statements only"
      - id: made
        text: "Decisions already made and hard-coded — e.g. talking to a particular database — that you can imagine changing"
        correct: true
      - id: o6c5e1b
        text: "Decisions you are about to make"
    explanation: "“Look for decisions—not decisions that you are making in the code, but decisions that you’ve already made. Is there some way of doing something (talking to a database, talking to another set of objects, and so on) that seems hard-coded?”"
  - id: extract-first
    prompt: "Why do some extract-method refactoring before settling on classes to extract?"
    options:
      - id: o6dc1d6
        text: "To increase method count for metrics"
      - id: levels
        text: "Method names don't tell the whole story — methods like `updateScreen()` mix levels of abstraction; extracting reveals groupings and encapsulates resources"
        correct: true
      - id: o1e2f2d
        text: "Classes can only be extracted from small methods"
    explanation: "“Often big classes house methods that do many things at many different levels of abstraction.” “For this reason, it pays to do a little extract method refactoring before really settling on classes to extract.”"
---
## Learning objectives

- Apply heuristics 1–3 to a large class
- Explain why public methods on an extracted class don't break encapsulation

## Connections to today

IDE "file structure" views list members with access modifiers — a ready-made method list for
grouping. Regions (`#region`) often mark responsibilities that want to be classes.
