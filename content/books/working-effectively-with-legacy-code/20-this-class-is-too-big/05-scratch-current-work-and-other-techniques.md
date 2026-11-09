---
title: "Scratch Refactoring, Current Work and Other Techniques"
questions:
  - id: scratch
    prompt: "What caution applies to heuristic"
    options:
      - id: o2a72ef
        text: "It requires full test coverage first"
      - id: artificial
        text: "It's artificial — what you see when you scratch isn't necessarily what you'll end up with"
        correct: true
      - id: oa7ce58
        text: "It must be committed"
    explanation: "“Just remember that it is an artificial exercise. The things you see when you \"scratch\" are not necessarily the things you’ll end up with when you refactor.”"
  - id: current-work
    prompt: "What is heuristic"
    options:
      - id: focus
        text: "Focus on the current work — if you're providing a different way of doing something, that may be a responsibility to extract and allow substitution for"
        correct: true
      - id: od8d01f
        text: "Ignore current work and plan the ideal design"
      - id: occc798
        text: "Refactor everything you can see"
    explanation: "“Pay attention to what you have to do right now. If you are providing a different way of doing anything, you might have identified a responsibility that you should extract and then allow substitution for.”"
  - id: overwhelmed
    prompt: "How does focusing on current changes prevent being overwhelmed?"
    options:
      - id: o036a78
        text: "It hides other responsibilities"
      - id: way-of-changing
        text: "Your current change tells you one particular way the software can change; that alone can mark new code as a separate responsibility"
        correct: true
      - id: o3412d5
        text: "It reduces code size"
    explanation: "“Remember that the changes you currently are making are telling you about some particular way that the software can change. Often just recognizing that way of changing is enough to see the new code you write as a separate responsibility.”"
  - id: get-better
    prompt: "How do you really get better at identifying responsibilities?"
    options:
      - id: obc06a2
        text: "Use more tools"
      - id: read
        text: "Read more — design-pattern books and, more importantly, other people's code"
        correct: true
      - id: o1dd5df
        text: "Memorise the heuristics"
    explanation: "“The way to really get better at identification is to read more. Read books about design patterns. More important, read other people’s code.”"
---
## Learning objectives

- Use scratch refactoring and current work as responsibility detectors
- Build long-term skill by reading code

## Connections to today

Reading well-structured open-source .NET code (for example ASP.NET Core or the .NET runtime
libraries) is a direct application of Feathers' advice: notice how class names and method names
correspond.
