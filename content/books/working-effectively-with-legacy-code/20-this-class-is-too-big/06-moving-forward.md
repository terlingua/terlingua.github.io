---
title: "Moving Forward — Strategy, Tactics and After Extract Class"
questions:
  - id: binge
    prompt: "What typically happens when teams go on a large refactoring binge?"
    options:
      - id: od253a9
        text: "Bugs disappear"
      - id: stability
        text: "System stability breaks down for a while, even with care and tests"
        correct: true
      - id: oe9b3ba
        text: "Nothing changes"
    explanation: "“In nearly every case that I’ve seen, when teams go on a large refactoring binge, system stability breaks down for a little while, even if they are being careful and writing tests as they go.”"
  - id: best-strategy
    prompt: "What is the best strategy for breaking down big classes?"
    options:
      - id: as-needed
        text: "Identify responsibilities, make sure the team understands them, then break the class down as needed"
        correct: true
      - id: od597f1
        text: "Stop feature work for a month"
      - id: o3abc22
        text: "Rewrite the class from scratch"
    explanation: "“The best approach to breaking down big classes is to identify the responsibilities, make sure that everyone else on the team understands them, and then break down the class on an as-needed basis.”"
  - id: start-level
    prompt: "In most legacy systems, at which level should you start applying SRP?"
    options:
      - id: o0707b4
        text: "The interface level, changing all clients first"
      - id: o97b5d2
        text: "The package level"
      - id: implementation
        text: "The implementation level — extract classes and delegate; it makes interface-level work easier later"
        correct: true
    explanation: "“the most that you can hope for in the beginning is to start to apply the SRP at the implementation level” and “introducing SRP at the implementation level makes it easier to introduce it at the interface level later.”"
  - id: tokenizer-tests
    prompt: "To extract `TermTokenizer` safely, how can you cover the private `nextTerm` and `hasMoreTerms`?"
    options:
      - id: o5863b1
        text: "You can't"
      - id: od913d4
        text: "Use reflection"
      - id: through-parser
        text: "Create a `RuleParser` in a harness and give it strings to evaluate — tests through it cover those methods"
        correct: true
    explanation: "“it might be just as easy to create a RuleParser in a test harness and give it a set of strings to evaluate. If we do that, we’ll have tests that cover hasMoreTerms and nextTerm, and we’ll be able to move them to a new class safely.”"
  - id: moving-prefix
    prompt: "Without tests, why extract method bodies into new methods prefixed `MOVING` rather than moving the original methods?"
    options:
      - id: o73eba0
        text: "The prefix makes code faster"
      - id: inheritance
        text: "Moving an overriding method silently redirects callers to the base-class version; new uniquely named methods avoid clashes"
        correct: true
      - id: oeedd6e
        text: "Compilers require uppercase prefixes"
    explanation: "“if you attempt to move a method that overrides another method. If you do, callers of the method on the original class will now call a method with the same name from a base class.” “The prefix is just a mechanical way of generating a new name and making sure that it doesn’t clash”."
  - id: no-lean
    prompt: "In step 5, why must you NOT Lean on the Compiler when checking variable uses?"
    options:
      - id: o4f3079
        text: "The compiler is too slow"
      - id: oa8a672
        text: "Variables can't be found by compilers"
      - id: shadowing
        text: "Shadowed variables — commenting out a shadowing declaration just makes the shadowed one visible, so the compiler won't find all uses"
        correct: true
    explanation: "“It is important not to Lean on the Compiler (315) in this step.” — “Commenting out the declaration of a shadowed variable just makes the one that it shadows visible.” Do a text search of the class and its subclasses instead."
  - id: steps-order
    prompt: "After moving variables and methods to the new class, how do you fix call sites and names?"
    options:
      - id: o96f486
        text: "Search and replace across the solution"
      - id: lean-later
        text: "Create an instance in the old class and Lean on the Compiler to redirect calls, then remove the `MOVING` prefixes, again leaning on the compiler"
        correct: true
      - id: o652557
        text: "Leave the prefixes permanently"
    explanation: "“Create an instance of the new class in the old class, and Lean on the Compiler (315) to find places where the moved methods have to be called on the instance” and “you can start to remove the MOVING prefix on all of the moved methods.”"
  - id: partner
    prompt: "What extra safeguard does Feathers recommend for the manual variable-search step?"
    options:
      - id: o9a606d
        text: "Run a profiler"
      - id: o99cf3e
        text: "Use a linter"
      - id: pair
        text: "Be very careful and do it with a partner"
        correct: true
    explanation: "“It is possible to make mistakes with this. Be very careful, and do it with a partner.”"
  - id: after
    prompt: "After Extract Class, what is the biggest danger?"
    options:
      - id: o50c462
        text: "Slow builds"
      - id: overambitious
        text: "Getting overambitious — the current structure works; move in a better direction rather than straight to the ideal"
        correct: true
      - id: o3b3e88
        text: "Too few classes"
    explanation: "“In practice, the biggest danger for teams doing this is getting overambitious.” — “move that not necessarily toward the ideal design, but at least in a better direction.”"
---
## Learning objectives

- Choose incremental strategy over refactoring binges
- Perform Extract Class without tests using the `MOVING` prefix procedure

## The MOVING procedure (summary)

1. Pick a responsibility.
2. Group the instance variables to move.
3. Extract bodies of whole methods into `MOVING*` methods.
4. Extract partial bodies into `MOVING*` methods.
5. Text-search class and subclasses for variable uses — no compiler leaning.
6. Move to the new class; instantiate it; lean on the compiler for call sites.
7. Drop the `MOVING` prefix.

## Connections to today

C# warns about member hiding (CS0108) and requires `new`/`override`, so some shadowing traps are
compiler-visible. Reflection- or string-based access (serialisers, `nameof`-free bindings) still
escapes the compiler — text search remains essential.
