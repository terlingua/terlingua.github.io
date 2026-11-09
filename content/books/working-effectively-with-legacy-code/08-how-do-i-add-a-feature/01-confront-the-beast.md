---
title: "Context — Confront the Beast"
questions:
  - id: hazards
    prompt: "Besides the obvious ones, which hazards of relying on sprouting and wrapping does Chapter 8 list?"
    type: multiple
    options:
      - id: o3abb54
        text: "Sprouted code cannot be tested"
      - id: fear
        text: "Fear and resignation"
        correct: true
      - id: duplication
        text: "New code may duplicate code in untested areas and fester"
        correct: true
      - id: no-better
        text: "Existing code isn't significantly modified, so it won't get better for a while"
        correct: true
    explanation: "“when we sprout or wrap, we don’t significantly modify the existing code, so it isn’t going to get any better for a while. Duplication is another hazard.” “The last hazards are fear and resignation”."
  - id: reminders
    prompt: "How does Feathers describe the sprouts and wraps left in code?"
    options:
      - id: o6ceebe
        text: "Badges of good design"
      - id: oeb6ea2
        text: "Temporary scaffolding that always disappears"
      - id: reminders-of-fear
        text: "Little reminders of fear, which gets in the way of good decision making"
        correct: true
    explanation: "“Fear gets in the way of good decision making. The sprouts and wraps left in the code are little reminders of it.”"
  - id: preference
    prompt: "What is Feathers' general preference when adding features?"
    options:
      - id: o55dbd0
        text: "Hide from risky code with wrappers"
      - id: o49fda3
        text: "Rewrite the module first"
      - id: confront
        text: "Confront the beast — get code under test and use this chapter's techniques"
        correct: true
    explanation: "“In general, it’s better to confront the beast than hide from it. If we can get code under test, we can use the techniques in this chapter to move forward in a good way.”"
  - id: late-duplication
    prompt: "Why is duplication especially dangerous when you sprout into untested areas?"
    options:
      - id: realize-late
        text: "You might not realise you are duplicating until you are far along"
        correct: true
      - id: o49844c
        text: "Duplication only matters in C++"
      - id: od6a259
        text: "Duplicate code doesn't compile"
    explanation: "“If the code that we add duplicates code that exists in the untested areas, it might just lie there and fester. Worse, we might not realize that we are going to have duplication until we get far along making our changes.”"
---
## Learning objectives

- List the hidden costs of sprouting and wrapping
- Explain why getting code under test is the better default

## Connections to today

Count the `*Helper`, `*V2` and `*New` classes in a code base: they are often sprouts that never got
folded back. A periodic "fold-back" task is a cheap way to repay that debt.
