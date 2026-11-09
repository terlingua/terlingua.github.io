---
title: "Listing Markup"
questions:
  - id: uses
    prompt: "Which activities does listing markup support?"
    type: multiple
    options:
      - id: structure
        text: "Understanding method structure — line up blocks"
        correct: true
      - id: responsibilities
        text: "Separating responsibilities — group related lines with symbols or colours"
        correct: true
      - id: effects
        text: "Understanding the effects of a change — mark affected variables and calls repeatedly"
        correct: true
      - id: extract
        text: "Extracting methods — circle code and note its coupling count"
        correct: true
    explanation: "The four headings are Separating Responsibilities, Understanding Method Structure, Extract Methods (“circle code that you’d like to extract. Annotate it with its coupling count”) and Understand the Effects of a Change."
  - id: inside-out
    prompt: "How do you line up blocks in a C-family long method?"
    options:
      - id: o908cb4
        text: "Outside in — start from the method's outer braces"
      - id: off2d01
        text: "Re-indent with the IDE only"
      - id: inside-out-opt
        text: "Inside out — read to the first closing brace, mark it and its matching opener, then continue to the next closing brace"
        correct: true
    explanation: "“The easiest way to line up blocks is inside out.” — “just start reading from the top of the listing past each opening brace until you get to the first closing brace. Mark that one and then go back and mark the one that matches it.”"
  - id: effects-markup
    prompt: "How is markup for effects different from an effect sketch?"
    options:
      - id: on-code
        text: "You mark changed lines, then each variable and call they affect, repeating to see propagation — directly on the listing"
        correct: true
      - id: oea81a8
        text: "It is identical"
      - id: oc3ba9c
        text: "It only marks comments"
    explanation: "“instead of making an effect sketch (155), put a mark next to the code lines that you are going to change. Then put a mark next to each variable whose value can change as a result of that change and every method call that could be affected.”"
  - id: first-step
    prompt: "What is the literal first step of listing markup?"
    options:
      - id: print
        text: "Print the code you want to work with"
        correct: true
      - id: o368917
        text: "Run the tests"
      - id: o4e5ab4
        text: "Open a diff tool"
    explanation: "“The first step is to print the code that you want to work with.”"
---
## Learning objectives

- Apply listing markup to four comprehension goals

## Connections to today

Editors now offer brace matching, sticky scroll, code folding and bracket-pair colouring — digital
versions of lining up blocks. Code-review comments on a long method serve as shared markup.
