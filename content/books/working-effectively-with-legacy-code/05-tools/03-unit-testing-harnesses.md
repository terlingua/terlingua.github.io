---
title: "Unit-Testing Harnesses"
questions:
  - id: ui-tools
    prompt: "Why does Feathers distrust expensive tools that test through the GUI?"
    type: multiple
    options:
      - id: more-work
        text: "It is usually more work than teams are prepared to admit"
        correct: true
      - id: oc770e6
        text: "GUI tests cannot be automated"
      - id: volatile
        text: "UIs are volatile and far from the functionality being tested"
        correct: true
      - id: hard-diagnose
        text: "When UI-based tests fail, it can be hard to figure out why"
        correct: true
    explanation: "“It can be done, but it is usually more work than anyone on a team is prepared to admit.” “UIs are often volatile and too far from the functionality being tested. When UI-based tests fail, it can be hard to figure out why.”"
  - id: xunit-features
    prompt: "Which are the key features of xUnit listed in Chapter 5?"
    type: multiple
    options:
      - id: same-language
        text: "Tests are written in the language being developed in"
        correct: true
      - id: suites
        text: "Tests can be grouped into suites to run and rerun on demand"
        correct: true
      - id: ob60984
        text: "Tests must be unit tests; larger tests are rejected"
      - id: isolation
        text: "All tests run in isolation"
        correct: true
    explanation: "The three bullets: “It lets programmers write tests in the language they are developing in.” “All tests run in isolation.” “Tests can be grouped into suites so that they can be run and rerun on demand.” And “xUnit really doesn’t care how large or small a test is.”"
  - id: separate-objects
    prompt: "Why can two JUnit test methods in the same class not affect each other through instance fields?"
    options:
      - id: separate
        text: "The runner creates a completely separate test-class object for each test method"
        correct: true
      - id: ob395a3
        text: "JUnit resets fields with reflection"
      - id: oab48d7
        text: "Fields are always static in JUnit"
    explanation: "“It creates a completely separate object for each one of those test methods.” and “The key thing is that we have a completely separate object for each method. There is no way that they can affect each other.”"
  - id: setup-why
    prompt: "Why does JUnit use `setUp` rather than the constructor to build fixtures?"
    type: multiple
    options:
      - id: resources
        text: "Many test objects are created up front; delaying allocation until needed saves resources"
        correct: true
      - id: o5e6bae
        text: "Constructors are not allowed in test classes"
      - id: report
        text: "Running setup later lets problems be detected and reported"
        correct: true
    explanation: "“By placing code in setUp to create what we need just when we need it, we save quite a bit on resources. In addition, by delaying setUp, we can also run it at a time when we can detect and report any problems that might happen during setup.”"
  - id: cppunitlite-why
    prompt: "Why did Feathers write CppUnitLite instead of continuing with his JUnit-like CppUnit?"
    options:
      - id: o6c55d4
        text: "JUnit's licence prohibited a port"
      - id: no-reflection
        text: "C++ lacks reflection, so tests had to be registered manually; a smaller macro-based design was easier and more portable"
        correct: true
      - id: ode0c4b
        text: "C++ already had reflection"
    explanation: "“The primary issue was C++’s lack of reflection.” He found it “could be quite a bit smaller, easier to use, and far more portable if it used some C idioms and only a bare subset of the C++ language.”"
  - id: nunit-diff
    prompt: "What is the one significant difference between NUnit and JUnit noted in the book?"
    options:
      - id: of52ce0
        text: "NUnit runs only VB.NET tests"
      - id: attributes
        text: "NUnit uses attributes to mark test classes and methods"
        correct: true
      - id: o7bff7c
        text: "NUnit does not support assertions"
    explanation: "“NUnit is very close in operation to JUnit. The one significant difference is that it uses attributes to mark test methods and test classes.”"
  - id: xunit-net
    prompt: "In xUnit.net (the .NET framework named after the family), a fresh instance of the test class is created per test and the constructor plays the role of `setUp`. How does this relate to the JUnit behaviour Feathers describes?"
    options:
      - id: same-isolation
        text: "It keeps the same isolation idea — a separate object per test — while moving fixture setup into the constructor"
        correct: true
      - id: oa28d59
        text: "It abandons test isolation entirely"
      - id: o4b2e73
        text: "It shares one instance across all tests in the class"
    explanation: "The isolation principle is the same one the book highlights: “we have a completely separate object for each method. There is no way that they can affect each other.” Construction cost matters less because xUnit.net constructs instances as each test runs."
---
## Learning objectives

- Explain the xUnit design and its isolation guarantee
- Justify `setUp`/`tearDown` in JUnit's design
- Map the book's harnesses to today's .NET frameworks

## In a nutshell

Feathers found free xUnit harnesses more effective than costly GUI-driven tools. xUnit's power is
simplicity: tests in your language, isolated, grouped and rerunnable.

## Connections to today

- .NET has three mainstream xUnit-family frameworks: xUnit.net, NUnit and MSTest. All mark tests
  with attributes, as the book's NUnit example shows.
- Microsoft.Testing.Platform turns test projects into executables, echoing CppUnitLite's
  "just compile and run" simplicity.
- Playwright and similar tools have made UI tests far more reliable, but Feathers' caution about
  distance from the logic still applies to legacy work.
