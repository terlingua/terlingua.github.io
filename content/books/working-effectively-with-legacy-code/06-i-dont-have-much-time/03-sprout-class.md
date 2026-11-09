---
title: "Sprout Class"
questions:
  - id: when-class
    prompt: "When is Sprout Method not enough, pushing you to Sprout Class?"
    options:
      - id: o3d3d2e
        text: "When the class is already small"
      - id: cannot-create
        text: "When you can't create objects of the source class in a test harness in a reasonable time"
        correct: true
      - id: oa4bd6d
        text: "When the change is a single line"
    explanation: "“there is just no way that you are going to be able to create objects of that class in a test harness in a reasonable amount of time, so there is no way to sprout a method and write tests for it on that class.”"
  - id: two-cases
    prompt: "Which two cases lead to Sprout Class?"
    type: multiple
    options:
      - id: harness
        text: "A small bit of functionality belongs in a class you can't get into a test harness"
        correct: true
      - id: new-responsibility
        text: "The change adds an entirely new responsibility to a class"
        correct: true
      - id: o59e6cb
        text: "You want to improve performance"
      - id: oad6c70
        text: "The class has too few methods"
    explanation: "“In one case, your changes lead you toward adding an entirely new responsibility to one of your classes.” “The other case … We have a small bit of functionality that we could place into an existing class, but we can’t get the class into a test harness.”"
  - id: ridiculous
    prompt: "A reviewer says `QuarterlyReportTableHeaderProducer` is \"a ridiculous tiny class that clutters the design\". What reframing does Feathers show?"
    options:
      - id: generator
        text: "Rename it a Generator with `generate()`, making it part of an existing concept that could share an `HTMLGenerator` interface"
        correct: true
      - id: o88b0d8
        text: "Make it a singleton"
      - id: o2276ea
        text: "Delete it and put the header inline"
    explanation: "“What if we’d named the class QuarterlyReportTableHeaderGenerator” — “Now the class is part of a concept that we’re familiar with.” They can then both implement `HTMLGenerator`."
  - id: new-concepts
    prompt: "What should you expect if a sprouted class does not fold into existing concepts?"
    options:
      - id: become-new
        text: "It may become a new concept; its role often looks different months later"
        correct: true
      - id: o6220b1
        text: "It must be removed in the next release"
      - id: o04edfa
        text: "It proves Sprout Class was wrong"
    explanation: "“Some sprouted classes never fold back into the main concepts in the application. Instead, they become new ones.” and “the decision to sprout a class often looks better in retrospect.”"
  - id: cpp-advantage
    prompt: "What extra advantage does Sprout Class have in C++?"
    options:
      - id: no-header-change
        text: "You don't modify existing headers; including the new header in the implementation file reduces compilation load over time"
        correct: true
      - id: o596f52
        text: "It enables reflection"
      - id: obf662b
        text: "It avoids the linker"
    explanation: "“In C++, Sprout Class has the added advantage that you don’t have to modify any existing header files to get your change in place.” and “This decreases the compilation load on the source class.”"
  - id: disadvantage
    prompt: "What is the key disadvantage of Sprout Class?"
    options:
      - id: conceptual
        text: "Conceptual complexity — work drifts out of key abstractions into other classes"
        correct: true
      - id: o8d6c56
        text: "It cannot be tested"
      - id: o9da359
        text: "Slower runtime"
    explanation: "“The key disadvantage of Sprout Class is conceptual complexity.” — “When you use Sprout Class, you start to gut the abstractions and do the bulk of the work in other classes.”"
  - id: tax-example
    prompt: "In tax software, some deductions are impossible at certain times of the year. Why might the date check become a sprouted class rather than code in `TaxCalculator`?"
    options:
      - id: of43022
        text: "Because `TaxCalculator` is sealed"
      - id: off-to-side
        text: "Checking dates is off to the side of `TaxCalculator`'s main responsibility, calculating tax"
        correct: true
      - id: o06fcfd
        text: "Date code can't live in a calculator for technical reasons"
    explanation: "“isn’t checking that off to the side of TaxCalculator’s main responsibility: calculating tax? Maybe it should be a new class.”"
  - id: constructor-args
    prompt: "In Sprout Class step 3, where do the source method's locals go?"
    options:
      - id: oa00e66
        text: "Into a configuration file"
      - id: ctor
        text: "Into the sprouted class's constructor arguments"
        correct: true
      - id: o3d81bb
        text: "Into static fields"
    explanation: "“Determine what local variables you need from the source method, and make them arguments to the classes’ constructor.”"
---
## Learning objectives

- Recognise when to escalate from Sprout Method to Sprout Class
- Name sprouted classes so they join existing concepts
- Explain the trade-off of conceptual complexity

## In a nutshell

When the source class can't even compile into a harness, put the new logic in a new class,
develop it test-first, and instantiate it from the old code. Start with an awkward name if you
must, but look for the concept it belongs to.

## Connections to today

- In .NET, a sprouted class is usually a small service in a new file, injected or `new`-ed at
  the call site. If the old class isn't using DI, a direct `new` is fine for now — that line is a
  future seam.
- Project-level build cost is the .NET analogue of the C++ header benefit: putting new logic in a
  separate, lightly referenced project can keep incremental builds and test runs fast.
