---
title: "Notes and Sketching"
questions:
  - id: feels-like
    prompt: "Why do many people skip deliberate techniques for understanding code?"
    options:
      - id: not-working
        text: "Spending time trying to understand looks and feels suspiciously like not working"
        correct: true
      - id: o9c41a2
        text: "The techniques require expensive tools"
      - id: o82b085
        text: "Code is always self-explanatory"
    explanation: "“After all, spending time trying to understand something looks and feels suspiciously like not working.”"
  - id: how
    prompt: "How should you sketch while reading confusing code?"
    options:
      - id: oc0a167
        text: "Produce full UML class diagrams before reading further"
      - id: names-lines
        text: "Write the name of the last important thing you saw and the next one, and draw lines for relationships — no special notation needed"
        correct: true
      - id: of74e19
        text: "Generate call graphs with a tool only"
    explanation: "“Write down the name of the last important thing that you saw, and then write down the name of the next one. If you see a relationship between them, draw a line. These sketches don’t have to be full-blown UML diagrams”."
  - id: precision
    prompt: "Where does the precision in a sketch need to live?"
    options:
      - id: o70138d
        text: "In a formal notation so others can read it later"
      - id: o682a70
        text: "In a version-controlled diagram file"
      - id: not-paper
        text: "Not on the paper — the paper is a tool for conversation and memory"
        correct: true
    explanation: "“The precision doesn’t have to be on paper. The paper is just a tool to make conversation go easier and help us remember the concepts we’re discussing and learning.”"
  - id: infectious
    prompt: "How does Feathers suggest spreading sketching on a team?"
    options:
      - id: just-do-it
        text: "Don't push it as process — just sketch while explaining code to a partner; it's informal and infectious"
        correct: true
      - id: of241b3
        text: "Hold a training course"
      - id: o4a29f1
        text: "Make it mandatory in the definition of done"
    explanation: "“it is informal and infectious. If you find this technique useful, you don’t have to push for your team to make it part of its process. All you have to do is this: Wait until you are working with someone trying to understand some code, and then make a little sketch”."
  - id: mental-state
    prompt: "Besides seeing things differently, what else does sketching help with?"
    options:
      - id: state
        text: "Maintaining your mental state while understanding something complex"
        correct: true
      - id: o66b187
        text: "Making code compile faster"
      - id: of6c324
        text: "Replacing tests"
    explanation: "“It’s also a great way of maintaining our mental state when we are trying to understand something particularly complex.”"
---
## Learning objectives

- Use quick, informal sketches to understand and discuss code

## Connections to today

Whiteboard apps and Mermaid diagrams in pull requests are fine, but Feathers' point is speed and
conversation: a 30-second scribble while pairing beats a polished diagram nobody updates.
