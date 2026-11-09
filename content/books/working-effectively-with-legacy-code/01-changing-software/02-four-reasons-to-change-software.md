---
title: "Four Reasons to Change Software"
questions:
  - id: four-reasons
    prompt: "Which are the four primary reasons for changing software that Chapter 1 lists?"
    type: multiple
    options:
      - id: design
        text: "Improving the design"
        correct: true
      - id: optimize
        text: "Optimizing resource usage"
        correct: true
      - id: feature
        text: "Adding a feature"
        correct: true
      - id: bug
        text: "Fixing a bug"
        correct: true
      - id: o6826c3
        text: "Upgrading dependencies"
    explanation: "“1. Adding a feature 2. Fixing a bug 3. Improving the design 4. Optimizing resource usage”"
  - id: logo-lesson
    prompt: "In the logo example, the customer calls the change a bug fix and developers call it a feature. What does Feathers say actually matters technically?"
    options:
      - id: ob5c0aa
        text: "How many lines of HTML change"
      - id: behavior
        text: "Whether we are adding new behaviour or changing existing behaviour"
        correct: true
      - id: o5ba18d
        text: "Whether the issue tracker labels it bug or feature"
      - id: o02c702
        text: "Which department pays for the work"
    explanation: "“this talk about bugfixing and feature addition masks something that is much more important to us technically: behavioral change. There is a big difference between adding new behavior and changing old behavior.”"
  - id: add-method
    prompt: "You add a new public method `ReplaceTrackListing` to a class but nothing calls it yet. Per the CDPlayer example, what has happened to the application's behaviour?"
    options:
      - id: neither
        text: "Neither — an uncalled method does not change behaviour"
        correct: true
      - id: o503cc9
        text: "Behaviour was changed"
      - id: odb130e
        text: "Behaviour was added"
      - id: o2fb280
        text: "Both — any edit changes behaviour"
    explanation: "“When we added that method, did we add new behavior to our application or change it? The answer is: neither. Adding a method doesn’t change behavior unless the method is called somehow.”"
  - id: button-subtle
    prompt: "Once a UI button calls the new method, why does Feathers say behaviour is *also* subtly changed, not just added?"
    options:
      - id: o40d279
        text: "Tests start failing automatically"
      - id: ocbca52
        text: "The compiler re-orders methods"
      - id: render
        text: "The UI renders differently and probably takes slightly longer to display"
        correct: true
      - id: oe82104
        text: "The old method is deleted"
    explanation: "“The UI will render differently with that new button. Chances are, the UI will take about a microsecond longer to display. It seems nearly impossible to add behavior without changing it to some degree.”"
  - id: modify-vs-add
    prompt: "Which heuristic does Chapter 1 offer programmers for telling adding from changing behaviour?"
    options:
      - id: o87eed8
        text: "Changes to HTML never count as behaviour changes"
      - id: oe511a6
        text: "Only changes visible to the user count"
      - id: modify-code
        text: "Modifying existing code may change behaviour; only adding code and calling it is often just adding behaviour"
        correct: true
      - id: o37ac54
        text: "Any change under 10 lines only adds behaviour"
    explanation: "“If we have to modify code (and HTML kind of counts as code), we could be changing behavior. If we are only adding code and calling it, we are often adding behavior.”"
  - id: refactor-def
    prompt: "What distinguishes refactoring from general clean-up in Feathers' description?"
    options:
      - id: small-steps-tests
        text: "A series of small structural modifications, supported by tests, with no intended functional change"
        correct: true
      - id: o53033b
        text: "Refactoring is only reformatting source"
      - id: o24aa3d
        text: "Refactoring includes rewriting large chunks from scratch"
      - id: o2dc27c
        text: "Refactoring always improves performance"
    explanation: "“Instead, we are making a series of small structural modifications, supported by tests to make the code easier to change.” and “there aren’t supposed to be any functional changes when you refactor”."
  - id: refactor-perf
    prompt: "A pure refactoring made a hot path 5% slower. Does that contradict Chapter 1's definition?"
    options:
      - id: o02e223
        text: "Yes — any observable difference means it was not a refactoring"
      - id: perf-allowed
        text: "No — functionality is held invariant, but structural changes can alter performance for better or worse"
        correct: true
      - id: o425c2e
        text: "Yes — refactoring must never touch hot paths"
    explanation: "The definition explicitly allows it: “(although behavior can change somewhat because the structural changes that you make can alter performance, for better or worse).”"
  - id: optimize-vs-refactor
    prompt: "Refactoring and optimisation both keep functionality the same. What differs?"
    options:
      - id: o7b4ff0
        text: "Optimisation changes functionality; refactoring does not"
      - id: oa0ca97
        text: "Nothing; they are the same activity"
      - id: ocfb812
        text: "Refactoring needs tests and optimisation does not"
      - id: something-else
        text: "The \"something else\" being changed — structure for refactoring, a resource such as time or memory for optimisation"
        correct: true
    explanation: "“In refactoring, the \"something else\" is program structure; we want to make it easier to maintain. In optimization, the \"something else\" is some resource used by the program, usually time or memory.”"
  - id: table-new-functionality
    prompt: "When the table splits functionality into \"new functionality\" and \"existing functionality\", which change types hold *existing* functionality invariant?"
    type: multiple
    options:
      - id: o835aed
        text: "Fixing a bug"
      - id: opt
        text: "Optimizing"
        correct: true
      - id: ref
        text: "Refactoring"
        correct: true
      - id: feat
        text: "Adding a feature"
        correct: true
    explanation: "“Adding features, refactoring, and optimizing all hold existing functionality invariant.” Bug fixing does change functionality, though “the changes are often very small compared to the amount of existing functionality that is not altered.”"
  - id: real-challenge
    prompt: "According to Chapter 1, what is the big deal about preserving behaviour?"
    options:
      - id: o4e7fff
        text: "Users rarely depend on existing behaviour"
      - id: o2d7918
        text: "Compilers cannot detect behavioural changes"
      - id: unknown-risk
        text: "We often do not know how much behaviour is at risk when we change code"
        correct: true
      - id: o094126
        text: "The amount of behaviour to preserve is large"
    explanation: "“The amount of behavior that we have to preserve is usually very large, but that isn’t the big deal. The big deal is that we often don’t know how much of that behavior is at risk when we make our changes.”"
---
Feathers reframes change around one question: are we *adding* behaviour, *changing* it, or
deliberately keeping it fixed while something else moves?

## Learning objectives

- List the four reasons to change software
- Distinguish adding behaviour from changing behaviour
- Compare refactoring and optimisation by what each holds invariant
- Explain why preserving behaviour is the central difficulty

## In a nutshell

| Change | Structure | New functionality | Existing functionality | Resource usage |
|--------|-----------|-------------------|------------------------|----------------|
| Add feature | changes | changes | — | — |
| Fix bug | changes | — | changes | — |
| Refactor | changes | — | — | — |
| Optimise | — | — | — | changes |

In every case the slice of behaviour that changes is small compared with what must stay the same.
The danger is not the size of what we preserve but that we rarely know which parts are at risk.

## Connections to today

- **Feature flags.** Shipping code behind a flag that is off is the CDPlayer case: the method
  exists but is not called, so behaviour is unchanged until the flag flips. The flip is the
  behavioural change and deserves its own tests.
- **Observable side effects.** In cloud systems "a microsecond longer" can become an SLO breach or
  a cost increase. Performance and resource usage are behaviour your users may depend on.
- **Semantic versioning.** A "patch" that changes existing behaviour is a breaking change in
  disguise — the same mislabelling as the bug-vs-feature debate.
