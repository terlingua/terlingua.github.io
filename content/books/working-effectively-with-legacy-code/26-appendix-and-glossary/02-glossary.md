---
title: "Glossary"
questions:
  - id: change-vs-interception
    prompt: "What's the difference between a change point and an interception point?"
    options:
      - id: o1a7fdd
        text: "A change point is in tests; an interception point is in production"
      - id: diff
        text: "A change point is where you need to make a change; an interception point is where a test can be written to sense some condition"
        correct: true
      - id: o321edc
        text: "They are synonyms"
    explanation: "“change point A place in code where you need to make a change.” and “interception point A place where a test can be written to sense some condition in a piece of software.”"
  - id: coupling-count
    prompt: "According to the glossary, what is the coupling count of `int f(int a, int b, int c)`?"
    options:
      - id: oeccbc8
        text: "3"
      - id: four
        text: "4 — three parameters plus one for the return value"
        correct: true
      - id: oc4ca42
        text: "1"
    explanation: "“If there is no return value, it is the number of parameters. If there is, it is the number of parameters plus one.”"
  - id: unit-test-tension
    prompt: "The glossary defines a unit test as one that \"runs in less than 1/10th of a second\". Chapter 2 says a 1/10-second test is slow. How do you reconcile them?"
    options:
      - id: ceiling
        text: "The glossary sets an upper bound for calling something a unit test; Chapter 2 argues that near that bound it's already slow at scale, aiming for ~1/100th second"
        correct: true
      - id: oedac58
        text: "Chapter 2 only applies to C++"
      - id: o0872bb
        text: "They contradict and one must be wrong"
    explanation: "Glossary: “unit test A test that runs in less than 1/10th of a second and is small enough to help you localize problems when it fails.” Chapter 2: “A unit test that takes 1/10th of a second to run is a slow unit test.” and at 1/100th “Now we are talking about 5 to 10 minutes.”"
  - id: link-seam
    prompt: "What artifacts does the glossary say can be swapped at a link seam?"
    options:
      - id: o7cb2bd
        text: "Only database schemas"
      - id: artifacts
        text: "Production libraries, DLLs, assemblies or JAR files"
        correct: true
      - id: oc46871
        text: "Only source files"
    explanation: "“In compiled languages, you can replace production libraries, DLLs, assemblies, or JAR files with others during testing to get rid of dependencies or sense some condition”."
  - id: free-function
    prompt: "According to the glossary, which languages lack free functions?"
    options:
      - id: java-csharp
        text: "Java and C# (as of the book's writing)"
        correct: true
      - id: o514837
        text: "All OO languages"
      - id: o87399f
        text: "C and C++"
    explanation: "“Free functions don’t exist in Java and C#.” Modern C# top-level statements and local functions blur this slightly, but methods still live in types."
  - id: object-seam
    prompt: "How does the glossary describe how you usually exploit an object seam?"
    options:
      - id: o5abf03
        text: "Use preprocessor defines"
      - id: subclass
        text: "Subclass a production class and override methods"
        correct: true
      - id: obd0d1d
        text: "Change the classpath"
    explanation: "“object seam A place where you can vary behavior by replacing one object with another. In object-oriented languages, you usually do this by subclassing a class in your production code and overriding various methods of the class.”"
  - id: tdd-coverage
    prompt: "What does the glossary claim about code developed with TDD?"
    options:
      - id: o9b1be6
        text: "It needs no refactoring"
      - id: o75ff59
        text: "It has no bugs"
      - id: coverage
        text: "It has test coverage by default"
        correct: true
    explanation: "“Code developed using TDD has test coverage, by default.”"
  - id: sketches
    prompt: "Match the sketch to its use."
    options:
      - id: match
        text: "Effect sketch → deciding where to write tests; feature sketch → deciding how to break apart a large class"
        correct: true
      - id: o573b3c
        text: "Effect sketch → breaking apart classes; feature sketch → where to write tests"
      - id: of11526
        text: "Both → documentation for auditors"
    explanation: "“Effect sketches can be useful when you are trying to decide where to write tests.” “Feature sketches can be useful when you are trying to decide how to break apart a large class.”"
---
## Learning objectives

- Use the book's vocabulary precisely

## Quick reference

| Term | One-line meaning |
|------|------------------|
| Seam | Place to vary behaviour without editing there |
| Enabling point | Where you choose which behaviour a seam uses |
| Change point | Where you must change code |
| Interception point | Where a test can sense effects |
| Pinch point | Narrow interception point covering many changes |
| Characterization test | Pins current behaviour |
| Coupling count | Params (+1 if a return value) |
| Fake / mock | Impersonating collaborator / one that asserts internally |
| Testing subclass | Subclass made to allow testing |
