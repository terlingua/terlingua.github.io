---
title: "Extract Interface"
questions:
  - id: safety
    prompt: "Why is Extract Interface one of the safest dependency-breaking techniques in many languages?"
    options:
      - id: o966bac
        text: "It never changes any code"
      - id: compiler
        text: "If you get a step wrong, the compiler tells you immediately"
        correct: true
      - id: off85c4
        text: "It's done only in tests"
    explanation: "“In many languages, Extract Interface is a one of the safest dependency-breaking techniques. If you get a step wrong, the compiler tells you immediately, so there is very little chance of introducing a bug.”"
  - id: three-ways
    prompt: "What are the three ways to do Extract Interface?"
    type: multiple
    options:
      - id: paste
        text: "Cut/copy and paste several declarations at once — less safe, but practical with slow builds"
        correct: true
      - id: incremental
        text: "Incrementally, using the compiler to add each method"
        correct: true
      - id: o3c5cdf
        text: "Reflection-based generation at run time"
      - id: tool
        text: "Automated refactoring support"
        correct: true
    explanation: "“The first way is to use automated refactoring support”, “the second way … You can extract it incrementally” and “The third way of extracting an interface is to cut/copy and paste several methods from a class at once”."
  - id: steps
    prompt: "What is the incremental procedure?"
    options:
      - id: o84ed0c
        text: "Copy all public methods into the interface first"
      - id: o45e698
        text: "Change all clients first, then create the interface"
      - id: incremental-steps
        text: "Create an empty interface → make the class implement it → change the usage site's type → compile and add each method the compiler reports"
        correct: true
    explanation: "“1. Create a new interface with the name you’d like to use. Don’t add any methods to it yet.” “2. Make the class that you are extracting from implement the interface.” “3. Change the place where you want to use the object so that it uses the interface” “4. Compile the system and introduce a new method declaration on the interface for each method use that the compiler reports as an error.”"
  - id: partial
    prompt: "`TransactionRecorder` gets only `saveTransaction`, not `recordError`. Is the extraction incomplete?"
    options:
      - id: enough
        text: "No — extract only what the use site needs; complete interfaces can come incrementally later"
        correct: true
      - id: oceb5fc
        text: "Yes — it won't compile"
      - id: o21c751
        text: "Yes — interfaces must mirror all public methods"
    explanation: "“we didn’t need it for the test. Although it’s nice to have an interface that covers all of the public methods of a class, if we march down that road, we could end up doing much more work than we need”. “you don’t have to extract all of the public methods on the class you are extracting from.”"
  - id: naming-sidebar
    prompt: "What disadvantage of the I-prefix convention does the naming sidebar describe?"
    options:
      - id: lie
        text: "Code has to know whether it's dealing with an interface; reverting to a class becomes a pervasive rename or a subtle lie"
        correct: true
      - id: ob522e1
        text: "It is slower at run time"
      - id: oc2d94b
        text: "It breaks serialisation"
    explanation: "“The disadvantage is that you end up with a lot of code that has to know whether it is dealing with an interface.” “Removing the I if you want to go back to a regular class ends up being a pervasive change. If you don’t make the change, the name stays in the code as a subtle lie.”"
  - id: non-virtual
    prompt: "In C++, `BondRegistry::newFixedYield` is non-virtual and `PremiumRegistry` defines the same signature. What happens if you extract an interface making it virtual?"
    options:
      - id: behaviour-change
        text: "Calls through a `BondRegistry*` pointing at a `PremiumRegistry` now dispatch to the subclass — behaviour changes"
        correct: true
      - id: o61e069
        text: "It fails to compile"
      - id: oe9b3ba
        text: "Nothing changes"
    explanation: "“Before we extracted the interface, BondRegistry’s newFixedYield method was called because the compile-time type of the registry variable is BondRegistry. If we make newFixedYield virtual in the process of extracting the interface, we change the behavior.”"
  - id: csharp-safer
    prompt: "What does Feathers say about this non-virtual hazard in C#?"
    options:
      - id: o0c2783
        text: "C# has no interfaces"
      - id: o49e0ec
        text: "C# has exactly the same problem"
      - id: safer
        text: "C# is safer — adding an interface doesn't affect existing calls to non-virtual methods"
        correct: true
    explanation: "“In C#, things are a little safer because adding an interface does not affect existing calls to non-virtual methods.”"
  - id: remedy
    prompt: "If you need a non-virtual method through an interface on a class that has subclasses, what's best?"
    options:
      - id: o3e898d
        text: "Delete the subclass"
      - id: new-name
        text: "Add a new virtual method with a new name that delegates to the non-virtual (or static) method"
        correct: true
      - id: o61250d
        text: "Make the existing method virtual anyway"
    explanation: "“the best thing to do is add a new virtual method with a new name. That method can delegate to a non-virtual or even a static method.”"
---
## Learning objectives

- Extract an interface incrementally using the compiler
- Avoid the non-virtual method trap

## Connections to today

C# explicit interface implementation and the fact that interface dispatch never changes calls
made through the concrete type (Feathers' "C# is safer" note) make this the default .NET seam.
Default interface methods (C# 8+) can add members without breaking implementers, but don't use
them to smuggle logic into seams.
