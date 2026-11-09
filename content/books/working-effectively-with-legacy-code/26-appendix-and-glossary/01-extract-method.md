---
title: "Appendix — Extract Method"
questions:
  - id: most-useful
    prompt: "Which refactoring does the appendix call \"perhaps the most useful\"?"
    options:
      - id: extract
        text: "Extract Method"
        correct: true
      - id: ofa73a9
        text: "Extract Interface"
      - id: o4537ff
        text: "Rename Class"
    explanation: "“Of all refactorings, Extract Method is perhaps the most useful.” (Chapter 8 separately calls Rename Class “the most powerful”.)"
  - id: prerequisite
    prompt: "What does the appendix's Extract Method procedure require first?"
    options:
      - id: tests
        text: "A set of tests that thoroughly exercise the method"
        correct: true
      - id: of547ab
        text: "A design document"
      - id: o0ab91b
        text: "A refactoring tool"
    explanation: "“When you want to extract a method, the first thing that you need is a set of tests. If you have tests that thoroughly exercise a method, you can extract methods from it using these steps”."
  - id: steps
    prompt: "Put the core of the procedure in order."
    options:
      - id: order
        text: "Comment out the code → create an empty named method → call it → copy code in → lean on the compiler for params/returns → adjust signature → run tests → delete commented code"
        correct: true
      - id: o84aed4
        text: "Delete the code → write the new method from memory → run tests"
      - id: o6ea50a
        text: "Copy code → run tests → name the method"
    explanation: "“1. Identify the code you want to extract, and comment it out. 2. Think of a name for the new method and create it as an empty method. 3. Place a call to the new method in the old method. 4. Copy the code” “5. Lean On the Compiler (315)” “6. Adjust the method declaration” “7. Run your tests. 8. Delete the commented-out code.”"
  - id: fix-compile
    prompt: "`getPremiumFee` first fails to compile because `result` and `amount` are undeclared. How is it fixed?"
    options:
      - id: o68c01f
        text: "Make `result` and `amount` fields"
      - id: return-param
        text: "Return the computed portion instead of adding to `result`, and pass `amount` as a parameter"
        correct: true
      - id: o95b47c
        text: "Declare them as locals initialised to 0"
    explanation: "“Because we are computing only a portion of the result, we can just return what we compute. We can also get hold of the amount if we make it a parameter to the method and add it to the call”."
  - id: declare-local-trap
    prompt: "Which Chapter 22 mistake would \"declare `amount` as a local initialised to 0\" represent?"
    options:
      - id: forgot-pass
        text: "Forgetting to pass a variable and declaring it locally instead — the compiler is satisfied but behaviour changes"
        correct: true
      - id: o43493f
        text: "Name hiding"
      - id: o6b8744
        text: "Wrong return type"
    explanation: "Chapter 22 lists: “We can forget to pass a variable into the extracted method.” — “we could just think that it needs to be a local variable and declare it in the new method.”"
  - id: why-comment
    prompt: "Why does Feathers like to comment out code he's extracting?"
    options:
      - id: revert
        text: "If a test fails, he can easily go back to what he had, get it passing, and try again"
        correct: true
      - id: o8963a0
        text: "For documentation"
      - id: oa955f5
        text: "Compilers require it"
    explanation: "“I like to comment out code that I am going to extract; that way, if I make a mistake and a test fails, I can easily go back to what I had, get the test to pass, and then try again.”"
  - id: uses
    prompt: "What uses of Extract Method does the appendix name for legacy work?"
    type: multiple
    options:
      - id: o562a67
        text: "Improve run-time speed"
      - id: resp
        text: "Separate responsibilities"
        correct: true
      - id: long
        text: "Break down long methods"
        correct: true
      - id: dup
        text: "Extract duplication"
        correct: true
    explanation: "“You can use it to extract duplication, separate responsibilities, and break down long methods.”"
---
## Learning objectives

- Perform Extract Method safely with tests and without a tool

## Connections to today

With a modern IDE, Extract Method is one keystroke, and the tool performs steps 2–6. The appendix
remains useful in Razor, SQL, PowerShell and YAML pipelines, where tool support is thinner.
