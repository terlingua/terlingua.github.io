---
title: "The Case of the Undetectable Side Effect"
questions:
  - id: why-pointless
    prompt: "Why would running `AccountDetailFrame.actionPerformed` in a test harness be pointless?"
    options:
      - id: no-sense
        text: "It opens windows, prompts for input and has no decent place to sense what it does"
        correct: true
      - id: oad8c7e
        text: "It throws immediately"
      - id: od4d48f
        text: "It is private"
    explanation: "“It would create a window, show it to us, prompt us for input, and then go on to display something in another window. There is no decent place to sense what this code does.”"
  - id: first-extract
    prompt: "What does extracting the whole body of `actionPerformed` into `performCommand(String)` achieve?"
    options:
      - id: no-actionevent
        text: "It separates the logic from any dependency on `ActionEvent`"
        correct: true
      - id: o7b3e15
        text: "It removes GUI calls"
      - id: oede099
        text: "It makes the code faster"
    explanation: "“If we extract the whole body of the method, we can separate ourselves from any dependency on the ActionEvent class.”"
  - id: naming
    prompt: "How should methods extracted around GUI calls be named?"
    options:
      - id: perspective
        text: "From the class's perspective — what they do or calculate for it — hiding the display components"
        correct: true
      - id: oe5d41a
        text: "After the widgets they touch"
      - id: o0b3aca
        text: "With a `Gui` prefix"
    explanation: "“we should take a look at what each piece of code does from the perspective of this class, or what it calculates for this class. In addition, we should not use names that deal with the display components.”"
  - id: cqs
    prompt: "What is Command/Query Separation?"
    options:
      - id: o19c647
        text: "Separate UI commands from business queries in different projects"
      - id: cqs-def
        text: "A method should be a command (modifies state, returns nothing) or a query (returns a value, no modification), not both"
        correct: true
      - id: o9198c2
        text: "Separate read and write databases"
    explanation: "“A method should be a command or a query, but not both. A command is a method that can modify the state of the object but that doesn’t return a value. A query is a method that returns a value but that does not modify the object.”"
  - id: cqs-why
    prompt: "What is the primary reason CQS matters?"
    options:
      - id: o9446a9
        text: "Performance"
      - id: communication
        text: "Communication — you can call a query repeatedly without inspecting it for side effects"
        correct: true
      - id: o638894
        text: "Thread safety"
    explanation: "“There are a number of reasons, but the most primary is communication. If a method is a query, we shouldn’t have to look at its body to discover whether we can use it several times in a row without causing some side effect.”"
  - id: testing-subclass
    prompt: "How does `TestingAccountDetailFrame` make `performCommand` testable?"
    options:
      - id: o9cc961
        text: "It uses reflection on private fields"
      - id: o9e718c
        text: "It mocks the whole GUI framework"
      - id: override
        text: "It overrides `setDescription`, `getAccountSymbol` and `setDisplayText` to do nothing, return a set symbol and record the text"
        correct: true
    explanation: "“After those extractions, we can Subclass and Override Method (401) and test whatever code is left in the performCommand method.” The subclass stores `displayText` and returns `accountSymbol`."
  - id: flinch
    prompt: "`setDescription` creates and shows a frame. Why accept such a nasty method for now?"
    options:
      - id: o6b4752
        text: "It's actually good design"
      - id: o126211
        text: "GUI code never needs refactoring"
      - id: safety-first
        text: "Coarse, conservative extractions are a decent first step; you can relocate frame creation once tests are in place"
        correct: true
    explanation: "“doing these coarse extractions is a decent first step. Afterward, we can see if we can relocate the frame creation to a better place.” and “Remember that it is okay to extract methods with poor names or poor structure to get tests in place. Safety first.”"
  - id: hazard
    prompt: "When you use a safe automated Extract Method tool, which part of the work becomes most hazardous?"
    options:
      - id: between
        text: "The manual editing you do between uses of the tool"
        correct: true
      - id: o122531
        text: "Naming the methods"
      - id: o79cff7
        text: "Running the tests"
    explanation: "“A good refactoring tool will only allow you to do an automated extract method refactoring when it is safe. However, that just makes the editing that we do between uses of the tool the most hazardous part of the work.”"
  - id: symbolsource
    prompt: "Why is the extracted class named `SymbolSource` rather than something like `DetailFrameReader`?"
    options:
      - id: oa55527
        text: "Because Java requires `Source` suffixes"
      - id: obf4775
        text: "To match a database table"
      - id: role
        text: "From `AccountDetailFrame`'s point of view its job is to get symbol information however it needs to — a role that could later become an interface"
        correct: true
    explanation: "“we named it SymbolSource here because, from the point of view of the AccountDetailFrame, its job is to get symbol information any way it needs to. I wouldn’t be surprised to see SymbolSource become an interface”."
---
## Learning objectives

- Extract GUI-dependent code into command and query methods
- Test remaining logic with a testing subclass
- Name extracted methods by role, not by widget

## Connections to today

- MVVM in WPF/MAUI and component logic split from markup in Blazor are this refactoring made
  architectural: logic lives in a class with no UI dependency, so tests call it directly.
- bUnit can render Blazor components in tests, but extracting the logic is still cheaper and
  faster than rendering.
