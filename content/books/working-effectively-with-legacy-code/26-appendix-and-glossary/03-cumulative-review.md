---
title: "Cumulative Review — Scenarios Across the Book"
questions:
  - id: s1
    prompt: "A 3,000-line `InvoiceService` can't be constructed in tests (it opens a DB connection in its constructor). You must add a VAT rule used only when `Submit()` runs. What is the most reasonable first move?"
    options:
      - id: o8d2be0
        text: "Add the VAT code inline and test manually"
      - id: ofacc43
        text: "Rewrite the service with clean architecture"
      - id: oe7d0c2
        text: "Wrap the whole class in a decorator immediately"
      - id: try-then-parameterize
        text: "Write a construction test; if the DB connection is created inside, Parameterize Constructor (keeping the old constructor) and fake it"
        correct: true
    explanation: "Chapter 9: “The best way to see if you will have trouble instantiating a class in a test harness is to just try to do it.” For a dependency created in the constructor, “I like to use Parameterize Constructor (379) as often as I can.”"
  - id: s2
    prompt: "The same class truly can't be instantiated even after a day of effort, and the release is tomorrow. Which technique gets the VAT logic tested?"
    options:
      - id: sprout-class
        text: "Sprout Class — develop a `VatCalculator` test-first and call it from `Submit()`"
        correct: true
      - id: o83c1d6
        text: "Sprout Method on `InvoiceService`"
      - id: oe660df
        text: "Skip tests this once"
    explanation: "Chapter 6: Sprout Class fits when “there is just no way that you are going to be able to create objects of that class in a test harness in a reasonable amount of time”."
  - id: s3
    prompt: "You plan to refactor a pricing method but don't know what it does in edge cases. Requirements docs exist. Which tests should you write first?"
    options:
      - id: o06d687
        text: "No tests; refactor with an IDE only"
      - id: o06ecba
        text: "Tests from the requirements documents"
      - id: characterize
        text: "Characterization tests that pin actual behaviour, flagging suspicious results — not tests from the requirements"
        correct: true
    explanation: "Chapter 13: “In nearly every legacy system, what the system does is more important than what it is supposed to do.”"
  - id: s4
    prompt: "Your changes touch `Order`, `LineItem` and `Discount`, all consumed by `OrderSummary.Render()`. None are tested. Where do you start?"
    options:
      - id: o193573
        text: "Test only `Discount`"
      - id: pinch
        text: "Look for a pinch point — e.g. `OrderSummary.Render()` — and write covering tests there first"
        correct: true
      - id: oa2ba60
        text: "Break dependencies on all three classes individually first"
    explanation: "Chapter 12: “A pinch point is a narrowing in an effect sketch, a place where tests against a couple of methods can detect changes in many methods.” Higher-level tests are “a first step toward getting unit tests in place.”"
  - id: s5
    prompt: "A method calls static `TaxApi.Lookup(zip)` once. You need to sense the zip passed. Which catalog technique is the lightest fit?"
    options:
      - id: extract-override-call
        text: "Extract and Override Call"
        correct: true
      - id: o2af5f0
        text: "Link Substitution"
      - id: o8b9cce
        text: "Encapsulate Global References"
      - id: o49c274
        text: "Template Redefinition"
    explanation: "Chapter 25: Extract and Override Call “is an ideal way to break dependencies on global variables and static methods” when there's a single call; many calls suggest Replace Global Reference with Getter."
  - id: s6
    prompt: "A teammate proposes extracting an interface from a sealed SDK client with 60 methods so you can mock it. What does the book suggest considering?"
    options:
      - id: o90cf07
        text: "Mock all 60 methods"
      - id: o19ce8e
        text: "Unseal the SDK class"
      - id: adapt
        text: "Adapt Parameter / Skin and Wrap — a narrow interface that communicates your responsibility, implemented by a thin wrapper"
        correct: true
    explanation: "Chapter 25: “Move toward interfaces that communicate responsibilities rather than implementation details.” Chapter 10: when library classes are sealed, “isolate them behind some wrapper”."
  - id: s7
    prompt: "While refactoring with your IDE (no tests), you also \"quickly\" reorder two statements and rename a variable by hand. What does Chapter 22 say?"
    options:
      - id: o74d4e1
        text: "Only renaming is risky"
      - id: o3166fb
        text: "Fine, reordering is always safe"
      - id: tool-only
        text: "Don't — use the tool exclusively without tests; manual edits break the separation between known-safe and unknown changes"
        correct: true
    explanation: "“When doing automated refactoring without tests, use the tool exclusively.” and “you should avoid even simple things, such as reordering statements and breaking apart expressions.”"
  - id: s8
    prompt: "A characterization test reveals `CalculateRefund` rounds half-cents down, which looks wrong; the system has been in production for 8 years. What now?"
    options:
      - id: o5a2706
        text: "Fix it silently in the refactoring PR"
      - id: od01df3
        text: "Delete the test"
      - id: suspicious
        text: "Keep the test, mark it suspicious, and escalate — someone may depend on that behaviour"
        correct: true
    explanation: "Chapter 13: “If the system has been deployed, you need to examine the possibility that someone is depending on that behavior” and “mark it in the test code as suspicious and then escalate it.”"
  - id: s9
    prompt: "The team wants to stop all features for six weeks to \"refactor everything\". What does Chapter 20 suggest instead?"
    options:
      - id: o798eb4
        text: "Approve the six-week binge"
      - id: ob5dbb0
        text: "Never refactor"
      - id: as-needed
        text: "Identify responsibilities, make sure everyone understands them, then break classes down as needed while delivering features"
        correct: true
    explanation: "“when teams go on a large refactoring binge, system stability breaks down for a little while” and “The best approach to breaking down big classes is to identify the responsibilities, make sure that everyone else on the team understands them, and then break down the class on an as-needed basis.”"
  - id: s10
    prompt: "Which sequence best reflects the legacy code change algorithm applied to a new feature?"
    options:
      - id: o7011a9
        text: "Write the feature → add tests → refactor → find change points"
      - id: algorithm
        text: "Find where to change → find where to test → break dependencies conservatively → write characterization tests → TDD the change and refactor"
        correct: true
      - id: of2fbb4
        text: "Refactor the module → write tests → add the feature"
    explanation: "Chapter 2: “1. Identify change points. 2. Find test points. 3. Break dependencies. 4. Write tests. 5. Make changes and refactor.”"
  - id: s11
    prompt: "After two weeks, your team has wrapped and sprouted around a giant class repeatedly and people are tired of it. According to Chapter 6, what usually happens next — and is it good?"
    options:
      - id: tackle
        text: "Familiarity and tiredness lead the team to get the old class under test — a good outcome"
        correct: true
      - id: o979abf
        text: "The sprouts must be deleted"
      - id: oa25644
        text: "The giant class must be rewritten from scratch"
    explanation: "“After a while, people get tired of sidestepping the old carcasses, and they start to get them under test. Part of this is familiarity.” “The other part of it is sheer tiredness.”"
  - id: s12
    prompt: "Which principle ties the whole book together?"
    options:
      - id: feedback
        text: "Get fast, localized feedback (tests) around code before and while you change it, using small, conservative steps to get there"
        correct: true
      - id: o3259dd
        text: "Always rewrite legacy systems"
      - id: o1705a3
        text: "Design for testability up front and you'll never need these techniques"
    explanation: "From the Preface: “With tests, we can change the behavior of our code quickly and verifiably.” From Chapter 2: “Unit testing is one of the most important components in legacy code work.” And Chapter 25's techniques exist “to get tests in place.”"
---
## Learning objectives

- Combine techniques from different chapters to plan a realistic change

## How to use this review

Each scenario has more than one defensible answer in real life. The quiz picks the answer the book
argues for; the explanation points you back to the passage. If you disagree, write down the
trade-off — that's the conversation Feathers wants teams to have.
