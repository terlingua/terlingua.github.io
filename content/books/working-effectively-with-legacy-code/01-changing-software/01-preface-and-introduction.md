---
title: "Preface and Introduction — legacy code is code without tests"
questions:
  - id: definition
    prompt: "Feathers offers a \"strict\" definition of legacy code and then his own working definition. Which pairing is correct?"
    options:
      - id: o9acc7e
        text: "Strict — code older than five years; his — code without documentation"
      - id: correct-pair
        text: "Strict — code we got from someone else; his — code without tests"
        correct: true
      - id: o92e503
        text: "Strict — code without tests; his — code we got from someone else"
      - id: o889dd1
        text: "Strict — code written in an obsolete language; his — code nobody understands"
    explanation: "The Preface gives both: “Legacy code is code that we’ve gotten from someone else.” and later “To me, legacy code is simply code without tests.”"
  - id: clean-but-untested
    prompt: "A teammate says: \"Our new microservice is beautifully structured, so it can't be legacy code, even though we skipped the tests.\" Using the Preface's reasoning, what is the strongest reply?"
    options:
      - id: o9fca05
        text: "It becomes legacy only after it is deployed to production"
      - id: o6abe3b
        text: "It is legacy only once the original authors leave"
      - id: ob3b4b9
        text: "Agree — clean structure is what really matters"
      - id: no-verification
        text: "Without tests you cannot change its behaviour quickly and verifiably, so you cannot tell whether changes make it better or worse"
        correct: true
    explanation: "Feathers is explicit that structure alone is not enough: “It doesn’t matter how well written it is; it doesn’t matter how pretty or object-oriented or well-encapsulated it is.” and “Without them, we really don’t know if our code is getting better or worse.”"
  - id: why-definition-useful
    prompt: "Why does Feathers consider \"code without tests\" a *good working* definition rather than just a provocative one?"
    options:
      - id: oae820d
        text: "It avoids blaming the people who wrote the code"
      - id: o17b2f5
        text: "It lets managers measure legacy-ness with a coverage percentage"
      - id: points-to-solution
        text: "It points directly at a remedy — getting code under test"
        correct: true
      - id: o679265
        text: "It means any code with at least one test is no longer legacy"
    explanation: "He says of the definition: “It is a good working definition, and it points to a solution.” Note that it also decouples the term from authorship — “That definition of legacy code has nothing to do with who wrote it” — but the reason he gives for its usefulness is that it points to a solution."
  - id: book-not-about
    prompt: "Which statements match how the Preface describes the book's scope?"
    type: multiple
    options:
      - id: o743a74
        text: "It is primarily a catalogue of beautiful design patterns"
      - id: not-testing
        text: "It is not really a book about testing; it is about changing code with confidence"
        correct: true
      - id: small-examples
        text: "Examples are deliberately brief, but the techniques have been used on large code bases"
        correct: true
      - id: uglier-steps
        text: "Some steps may make code slightly uglier on the way to something better"
        correct: true
    explanation: "“This book is about being able to confidently make changes in any code base.” He warns “don’t be surprised if some of the steps you take to make changes involve making some code slightly uglier” and notes “The techniques here have been tested on substantially large pieces of code.”"
  - id: surgery-metaphor
    prompt: "In the surgery metaphor, what is the point of saying we should not let \"best\" be the enemy of \"better\"?"
    options:
      - id: o0b9461
        text: "Only the original authors can safely operate on the code"
      - id: of63345
        text: "Aesthetic judgement should never be suspended"
      - id: incremental
        text: "Take the system as it is, fix what is wrong now, and move it toward health in steps rather than demanding an ideal design first"
        correct: true
      - id: o804743
        text: "Legacy systems should be rewritten rather than patched"
    explanation: "“what we really need to do is take the patient as he is, fix what’s wrong, and move him to a healthier state.”"
  - id: read-first
    prompt: "The Introduction says Part II is organised as an FAQ. Which part does it ask you to read first to get the vocabulary for everything else?"
    options:
      - id: oe1d377
        text: "The Glossary only"
      - id: part-one
        text: "The introductory chapters, particularly Chapter 4, The Seam Model"
        correct: true
      - id: obfdd4d
        text: "Chapter 25 first, because the catalog is self-contained"
      - id: o677a6d
        text: "Part III, the dependency-breaking catalog"
    explanation: "“Please read the introductory chapters, particularly Chapter 4, The Seam Model. These chapters provide the context and nomenclature for all the techniques that follow.”"
  - id: part-three-special
    prompt: "What makes the refactorings in Part III special compared with ordinary refactorings?"
    options:
      - id: o0f2ec3
        text: "They are only applicable to C++"
      - id: without-tests
        text: "They are meant to be performed without tests, in order to get tests in place"
        correct: true
      - id: oc5fce9
        text: "They always improve the design"
      - id: obbf576
        text: "They require a refactoring browser"
    explanation: "“The refactorings in Dependency-Breaking Techniques are special in that they are meant to be done without tests, in the service of putting tests in place.”"
  - id: languages
    prompt: "You work in C#, which the book barely mentions. According to the Preface, should you skip the Java/C++/C examples?"
    options:
      - id: of012e2
        text: "Only read the C examples, because C# derives from C"
      - id: o75dd81
        text: "Yes — techniques are language-specific"
      - id: read-anyway
        text: "No — the three languages were chosen to span the range of legacy concerns, and many techniques carry over to other languages"
        correct: true
    explanation: "“Among them, these languages cover much of the spectrum of concerns that arise in legacy code. However, if the languages you use are not covered in the examples, take a look at them anyway.”"
---
The Preface and Introduction set the vocabulary for the whole book. The most important idea is a
redefinition: legacy code is not *old* code or *someone else's* code — it is code you cannot change
with fast, verifiable feedback.

## Learning objectives

- State the strict and the working definitions of legacy code and why they differ
- Explain why "clean but untested" code still carries legacy-code risk
- Navigate the book: Part I (mechanics), Part II (FAQ chapters), Part III (catalog)

## In a nutshell

- Code without tests is the working definition. Cleanliness helps, but without tests you cannot
  tell whether a change made things better or worse.
- The book is about *changing* code confidently, not about testing for its own sake or about
  pretty design.
- Expect some interim ugliness. Like surgery, legacy work leaves incisions that can heal once
  tests are in place.
- Part II chapters are named after the problems you face ("I can't get this class into a test
  harness"), and they cross-reference the Part III catalog heavily.

## Connections to today

- **AI-generated code.** Code produced quickly by an assistant and merged without tests fits
  Feathers' definition the day it lands. The definition is about feedback, not age.
- **Coverage gates.** A coverage threshold in CI is a crude proxy. Feathers' point is whether
  tests let you change *this* code *now* and know the outcome — a 90% figure built on tests that
  assert nothing does not meet that bar.
- **C# and .NET.** Most Java examples translate almost line-for-line to C#. C++-specific seams
  (preprocessor, link-time) have rough .NET analogues covered in the Chapter 4 notes.

## How to use this companion

Each section follows the book's headings. Read the book section, then take the quiz. Every
explanation quotes the passage the answer rests on, so you can check your reasoning without
switching back and forth.
