---
title: "Automated Refactoring Tools"
questions:
  - id: fowler-def
    prompt: "According to the definition quoted from Fowler, which change is a refactoring?"
    options:
      - id: o6776ec
        text: "Adding a cache to speed up a query"
      - id: of72ece
        text: "Fixing an off-by-one error"
      - id: internal-structure
        text: "Changing internal structure to make code easier to understand and cheaper to modify, without changing existing behaviour"
        correct: true
      - id: o409ab3
        text: "Reformatting and renaming while also tweaking output text"
    explanation: "“refactoring (n.). A change made to the internal structure of software to make it easier to understand and cheaper to modify without changing its existing behavior.”"
  - id: inline-bug
    prompt: "A tool inlines `int v = getValue();` into a loop as `total += getValue();`. `getValue()` increments a field. What went wrong?"
    options:
      - id: o90ae8c
        text: "Nothing; inlining is always safe"
      - id: ten-times
        text: "The side effect now happens 10 times instead of once, so behaviour changed"
        correct: true
      - id: o51ddea
        text: "The field becomes final"
      - id: o8f863e
        text: "The loop no longer compiles"
    explanation: "“The variable was removed, but now the value of alpha is incremented 10 times rather than 1. This change clearly didn’t preserve behavior.”"
  - id: sanity-checks
    prompt: "What sanity checks does Feathers run on a new refactoring tool?"
    type: multiple
    options:
      - id: oeaa1e2
        text: "Check whether it can generate UML diagrams"
      - id: extract-paces
        text: "Put its Extract Method support through its paces"
        correct: true
      - id: existing-name
        text: "Extract a method using the name of a method that already exists in the class — is it flagged?"
        correct: true
      - id: base-name
        text: "Use the name of a method in a base class — does it detect accidental overriding?"
        correct: true
    explanation: "“When you attempt to extract a method and give it the name of a method that already exists in that class, does it flag that as an error? What if it is the name of a method in a base class—does the tool detect that?” and “the first thing that I do is put its support for extracting methods through its paces.”"
  - id: no-tests-ok
    prompt: "When can you assume a sequence of automated refactorings did not change behaviour without tests?"
    options:
      - id: o6e7b34
        text: "Never"
      - id: safe-chain
        text: "When the tool performs safe refactorings and you go from one automated refactoring to another without any other editing"
        correct: true
      - id: o16396e
        text: "Whenever the tool is popular"
    explanation: "“If your tool performs safe refactorings and you go from one automated refactoring to another without doing any other editing, you can assume that your edits haven’t changed behavior. However, this isn’t always the case.”"
  - id: untrusted-tool
    prompt: "You discover your IDE's Extract Method sometimes changes behaviour. What does Feathers advise?"
    options:
      - id: o928a04
        text: "Keep using it but review diffs more carefully"
      - id: manual-advice
        text: "Don't use the automated refactorings; follow the advice for working without a tool"
        correct: true
      - id: o6b1e84
        text: "Write a plugin to fix it first"
    explanation: "“If you discover that the ones supplied by your tool don’t preserve behavior, don’t use the automated refactorings. Follow the advice for cases in which you don’t have a refactoring tool—it will be safer.”"
  - id: csharp-2004
    prompt: "What was the state of C# refactoring tools when the book was written?"
    options:
      - id: under-dev
        text: "Under active development"
        correct: true
      - id: o9b3784
        text: "Mature and widely trusted"
      - id: o25c672
        text: "Non-existent and not planned"
    explanation: "“Tools for C# refactoring are under active development at the time of this writing.” Today Visual Studio, ReSharper and Rider ship many Roslyn-backed refactorings — but Feathers' sanity checks still apply."
---
## Learning objectives

- State what makes a change a refactoring
- Explain how a "safe" tool refactoring can still change behaviour
- Run quick sanity checks on a refactoring tool

## In a nutshell

Automated refactorings are huge time savers if — and only if — they preserve behaviour. Know
what your tool checks, test it on tricky cases, and have tests around code before relying on it.

## Connections to today

- Roslyn-based refactorings in Visual Studio and Rider are far more capable than 2004 tools, yet
  side effects, reflection, `dynamic`, source generators and serialisation by member name can all
  defeat a tool's analysis.
- AI "refactor this" suggestions are not refactorings in Fowler's sense unless something verifies
  behaviour preservation. Treat them as hand edits.
