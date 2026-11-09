---
title: "Text Redefinition"
questions:
  - id: what
    prompt: "What makes Text Redefinition possible in Ruby?"
    options:
      - id: open-classes
        text: "Interpreted code can redefine methods on the fly; re-opening a class and defining a method replaces the existing one"
        correct: true
      - id: o2ef052
        text: "Ruby compiles to C"
      - id: o3d95de
        text: "Ruby has macros"
    explanation: "“When they are interpreted, methods can be redefined on the fly.” “The Ruby interpreter doesn’t care whether there already is a definition of that method, if there is one; it just replaces it.”"
  - id: scope
    prompt: "Does the test redefine the entire `Account` class?"
    options:
      - id: one-method
        text: "No — only `report_deposit`"
        correct: true
      - id: o118205
        text: "Yes — the whole class is replaced"
    explanation: "“It’s important to note that we aren’t redefining the entire Account class here—just the report_deposit method.”"
  - id: downside
    prompt: "What is the downside of Text Redefinition in Ruby?"
    options:
      - id: persists
        text: "The new method replaces the old one until the program ends, which can surprise later tests"
        correct: true
      - id: o9e04a9
        text: "It's slow"
      - id: od2b03b
        text: "It requires a compiler"
    explanation: "“The new method replaces the old one until the program ends. This can cause some trouble if you forget that a particular method has been redefined by a previous test.”"
  - id: c
    prompt: "How is Text Redefinition done in C and C++?"
    options:
      - id: o292184
        text: "With templates"
      - id: preprocessor
        text: "With the preprocessor — see the preprocessing seam example in Chapter 4"
        correct: true
      - id: ob26757
        text: "It can't be"
    explanation: "“We can do Text Redefinition in C and C++ also, using the preprocessor.”"
  - id: steps
    prompt: "What are the steps in Ruby?"
    options:
      - id: obedc26
        text: "Delete the method → run tests"
      - id: ruby-steps
        text: "Identify the class → require its module at the top of the test file → provide alternative method definitions at the top of the test file"
        correct: true
      - id: oed7cda
        text: "Copy the class → rename it → edit the copy"
    explanation: "“1. Identify a class with definitions that you want to replace. 2. Add a require clause with the name of the module that contains that class to the top of the test source file. 3. Provide alternative definitions at the top of the test source file for each method that you want to replace.”"
---
## Learning objectives

- Understand redefinition-based seams in dynamic languages

## Connections to today

Monkey-patching in Python (`unittest.mock.patch`) and JavaScript module mocking (Jest) are modern
Text Redefinition. Prefer scoped patches that restore the original after each test — Feathers'
downside is exactly what scoping fixes.
