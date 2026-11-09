---
title: "Delete Unused Code"
questions:
  - id: advice
    prompt: "What does Feathers advise about confusing code you can determine isn't used?"
    options:
      - id: delete
        text: "Delete it — it's only getting in your way"
        correct: true
      - id: o4b0f58
        text: "Comment it out"
      - id: o070309
        text: "Move it to a utility class"
    explanation: "“If the code you are looking at is confusing and you can determine that some of it isn’t used, delete it. It isn’t doing anything for you except getting in your way.”"
  - id: wasteful
    prompt: "How does he answer the objection that deleting code wastes someone's work?"
    options:
      - id: oec162c
        text: "The work was already wasted"
      - id: o6336f8
        text: "Keep a backup folder"
      - id: vcs
        text: "That's what version control is for — the code remains in earlier versions"
        correct: true
    explanation: "“Well, that is what version-control systems are for. That code will be in earlier versions.”"
  - id: determine
    prompt: "A public method in a shared library has no references in your solution. Is that enough to \"determine that it isn't used\"?"
    options:
      - id: oa05c8b
        text: "Yes, always"
      - id: o6d4842
        text: "Only if the IDE greys it out"
      - id: not-necessarily
        text: "Not necessarily — reflection, serialisation, other consumers of the library, or configuration may call it; check before deleting"
        correct: true
    explanation: "The book's condition is that “you can determine that some of it isn’t used”. Public API, reflection and dynamic dispatch make that determination harder; confirm before you delete."
---
## Learning objectives

- Delete dead code confidently using version control as the safety net

## Connections to today

IDE "unused member" analysis, `dotnet` trimming warnings and production telemetry (which code
paths actually run) help confirm code is dead. Feature-flag cleanup is a common, low-risk source
of deletions.
