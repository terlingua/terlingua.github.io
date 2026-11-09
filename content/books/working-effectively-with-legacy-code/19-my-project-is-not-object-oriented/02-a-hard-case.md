---
title: "A Hard Case — Link and Preprocessing Seams in C"
questions:
  - id: link-fake
    prompt: "How can a link seam stop `ksr_notify` from notifying a third-party system during tests?"
    options:
      - id: oee6df8
        text: "Set an environment variable"
      - id: fake-lib
        text: "Build a library of fake functions with the same names (an empty `ksr_notify`) and link to it in tests"
        correct: true
      - id: o452a7b
        text: "Comment out the call"
    explanation: "“we can make a library that contains fakes: functions that have the same names as the original functions but that don’t really do what they are intended to do.”"
  - id: when-link
    prompt: "When is a library of link-seam fakes a good strategy, and when is it tedious?"
    options:
      - id: odeb889
        text: "Always good"
      - id: of06587
        text: "Always tedious"
      - id: peripheral
        text: "Good when the library's calls are peripheral; tedious when you need to sense through them or vary return values per test"
        correct: true
    explanation: "“If there are a lot of functions in the ksr library and we consider their calls to be sort of peripheral to the main logic of the system” it makes sense to use fakes. But “if we want to sense through those functions or we want to vary some of the values that they return, using link seams (36) isn’t as nice; it’s actually pretty tedious.”"
  - id: one-definition
    prompt: "Why is varying a link-seam fake's behaviour between tests messy?"
    options:
      - id: oe28ae7
        text: "C forbids function bodies in tests"
      - id: oe45b39
        text: "The linker caches results"
      - id: one-per-exe
        text: "Substitution happens at link time, so there's only one definition per executable; tests must set conditions that drive code in the fake's body"
        correct: true
    explanation: "“Because the substitution happens at link time, we can provide only one function definition for each executable that we build. If we want a fake ksr_notify function to behave one way in one test and another way in another test, we have to put code in the body and set up conditions in the test”."
  - id: preproc
    prompt: "How does the preprocessing approach remove the `ksr_notify` call in tests?"
    options:
      - id: o9fd2a9
        text: "It renames the function"
      - id: define-away
        text: "`#define ksr_notify(code,packet)` defines the call out of existence when `TESTING` is defined"
        correct: true
      - id: o0bb5db
        text: "It wraps it in a try/catch"
    explanation: "“we have a preprocessing define, TESTING, that defines the call to ksr_notify out of existence when we are testing.”"
  - id: include-files
    prompt: "Why move the tests into `testscanner.tst` and the defines into `scannertestdefs.h`?"
    options:
      - id: o3f5edb
        text: "To speed up linking"
      - id: oce2c89
        text: "The compiler requires separate files"
      - id: clarity
        text: "Mixing tests and source in one file hurts navigation; includes keep the production file close to its normal form"
        correct: true
    explanation: "“Mixing tests and source into a file like this isn’t really the clearest thing we can do. Often it makes code harder to navigate.” With includes, “the code looks reasonably close to what it would look like without the testing infrastructure.”"
  - id: macro-safety
    prompt: "Why aren't macros too dangerous here despite being easily misused?"
    options:
      - id: odec4d3
        text: "Modern compilers check macros"
      - id: o099b10
        text: "Macros are type-safe"
      - id: test-only
        text: "As long as rampant macro use is restricted to code that runs under test, production code isn't affected"
        correct: true
    explanation: "“As long as we restrict rampant usage of macros to code that runs under test, we don’t have to be too concerned that we’ll misuse macros in ways that will affect the production code.”"
  - id: other-languages
    prompt: "For procedural languages without a preprocessor, what's the general approach?"
    options:
      - id: o868e00
        text: "Use object seams"
      - id: link-large
        text: "Use the link seam and try to get larger areas of code under test"
        correct: true
      - id: ob077c2
        text: "Give up on unit tests"
    explanation: "“C is one of the few mainstream languages that have a macro preprocessor. In general, to break dependencies in other procedural languages, we have to use the link seam (36) and attempt to get larger areas of code under test.”"
---
## Learning objectives

- Choose between link-seam fakes and preprocessor defines for C
- Keep test scaffolding out of production code paths

## Connections to today

C test frameworks such as Unity/CMock and the linker's `--wrap` option (GNU ld) industrialise these
seams. Embedded teams still use them daily.
