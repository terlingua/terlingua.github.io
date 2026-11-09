---
title: "Extract Implementer"
questions:
  - id: why
    prompt: "When does Feathers use Extract Implementer instead of Extract Interface?"
    options:
      - id: o2a66ff
        text: "When the class has no methods"
      - id: ob49aa2
        text: "When performance matters"
      - id: name
        text: "When the class's name is perfect for the interface and there are no tools to rename the class and extract an interface"
        correct: true
    explanation: "“When the name of a class is perfect for the name of an interface and I don’t have automated refactoring tools, I use Extract Implementer to get the separation I need.”"
  - id: i-prefix
    prompt: "Why does Feathers usually avoid simply prefixing \"I\" to make an interface name?"
    options:
      - id: o93d9d6
        text: "The compiler forbids it"
      - id: inconsistent
        text: "Unless it's already the code base convention, a mix of I-prefixed and unprefixed types makes you guess wrong half the time"
        correct: true
      - id: ob32a4c
        text: "It's a Java-only convention"
    explanation: "“One thing that I usually stop short of is putting an \"I\" prefix on the name of the class to make a name for the new interface, unless it is already the convention in the code base.” “Half of the time that you type the name of a type, you’ll be wrong.”"
  - id: mechanism
    prompt: "What is the core move of Extract Implementer?"
    options:
      - id: o54c53e
        text: "Make the class static"
      - id: o645986
        text: "Create an interface with an I-prefix"
      - id: push-down
        text: "Copy the class as a new production implementer (e.g. `ProductionModelNode`), then strip the original down to an interface of abstract public methods"
        correct: true
    explanation: "“To extract an implementer of a class, we turn the class into an interface by subclassing it and pushing all of its concrete methods down into that subclass.”"
  - id: steps
    prompt: "Which steps belong to Extract Implementer?"
    type: multiple
    options:
      - id: copy
        text: "Copy the class declaration under a new name (e.g. prefix `Production`)"
        correct: true
      - id: strip
        text: "Delete non-public methods and all variables from the source class and make remaining public methods abstract"
        correct: true
      - id: includes
        text: "Remove unnecessary imports/includes by deleting each and recompiling"
        correct: true
      - id: creation
        text: "Compile the system to find creations of the old class and create the production class instead"
        correct: true
    explanation: "The steps include “Make a copy of the source class’s declaration. Give it a different name.” “Turn the source class into an interface by deleting all non-public methods and all variables.” “Make all of the remaining public methods abstract.” “Examine all imports or file inclusions” and “Compile the rest of the system to find all of the places where instances of the source class were created.”"
  - id: dependency
    prompt: "Does replacing `new ModelNode` with `new ProductionModelNode` improve the dependency situation?"
    options:
      - id: not-really
        text: "Not really — it's one concrete class for another; consider whether a factory can reduce dependencies further"
        correct: true
      - id: o2bbd3a
        text: "It makes it worse"
      - id: oc5d3e7
        text: "Yes, dramatically"
    explanation: "“we’re replacing the creation of objects of one concrete class with objects of another, so we aren’t really making our overall dependency situation better. However, it’s good to take a look at those areas of object creation and try to figure out whether a factory can be used”."
  - id: hierarchy
    prompt: "When the class sits in a hierarchy (superclass and subclass), what does Feathers advise?"
    options:
      - id: consider-interface
        text: "Consider Extract Interface with different names — it's a far more direct refactoring"
        correct: true
      - id: o7281d4
        text: "Always use Extract Implementer"
      - id: o2c9a33
        text: "Flatten the hierarchy first"
    explanation: "“When you have a class embedded in a hierarchy like this, you really have to consider whether you are better off using Extract Interface (362) and picking different names for your interfaces. It is a far more direct refactoring.”"
  - id: cpp-virtual
    prompt: "In C++, what extra check does step 3 require?"
    options:
      - id: non-virtual-overrides
        text: "Ensure none of the methods made abstract are overridden by non-virtual methods"
        correct: true
      - id: oe893df
        text: "Make every method static"
      - id: o353690
        text: "Remove all destructors"
    explanation: "“If you are working in C++, make sure that none of the methods that you make abstract are overridden by non-virtual methods.”"
---
## Learning objectives

- Turn a well-named class into an interface plus a production implementer

## Connections to today

In C#, the `I` prefix is the platform convention, so Extract Interface (`IModelNode`) is normal and
Extract Implementer is rarely needed — Feathers' own rule ("unless it is already the convention")
applies.
