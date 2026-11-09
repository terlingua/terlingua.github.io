---
title: "Strategy"
questions:
  - id: skeletonize
    prompt: "What is skeletonizing?"
    options:
      - id: ob25511
        text: "Extract the condition and body together"
      - id: separate
        text: "Extract the condition and the body into separate methods, leaving only control structure and delegations"
        correct: true
      - id: od852a8
        text: "Delete all comments"
    explanation: "“If you extract the condition and the body to two different methods, you are in a better position to reorganize the logic of the method later” — “all that is left in the method is a skeleton: the control structure and delegations to other methods.”"
  - id: sequences
    prompt: "What is \"finding sequences\"?"
    options:
      - id: ocb4a3b
        text: "Finding loops"
      - id: together
        text: "Extract the condition and body together, to reveal the method as a clear sequence of operations"
        correct: true
      - id: ofd935c
        text: "Sorting methods alphabetically"
    explanation: "“If you extract the condition and the body to the same method, you are in a better position to identify a common sequence of operations”."
  - id: which
    prompt: "Feathers admits his advice conflicts. How does he choose?"
    options:
      - id: o99729e
        text: "Flip a coin"
      - id: o38e5ae
        text: "Always skeletonize"
      - id: depends
        text: "Skeletonize when control structure will need refactoring (snarled methods); find sequences when an overarching sequence clarifies (bulleted methods)"
        correct: true
    explanation: "“I skeletonize when I feel that the control structure will need to be refactored after it is clarified. I attempt to find sequences when I feel that identifying an overarching sequence will make the code clearer. Bulleted methods lean me toward finding sequences, and snarled methods lean me toward skeletonizing”."
  - id: current-class
    prompt: "A chunk wants to be called `recalculateOrder` and clearly belongs on `Order`. What should you do first?"
    options:
      - id: awkward
        text: "Extract it to the current class with the awkward name; move it later when the direction is clear"
        correct: true
      - id: odb13a7
        text: "Extract it directly into `Order`"
      - id: o9a6dfa
        text: "Leave it inline"
    explanation: "“Although it is tempting to extract directly to another class, don’t do it. Use the awkward name first.” — “extracting to the current class moves us forward and it is less error prone.”"
  - id: name-hint
    prompt: "What strong indication suggests extracted code belongs on another class?"
    options:
      - id: o567303
        text: "It's longer than 10 lines"
      - id: o2ea621
        text: "It has a loop"
      - id: variable-name
        text: "You're tempted to name it after one of the variables it uses"
        correct: true
    explanation: "“If you look at a piece of code and you are tempted to use the name of one of the variables you are using in it, chances are good that the code belongs on the class of that variable.”"
  - id: small
    prompt: "Why extract small pieces first rather than large chunks?"
    options:
      - id: details
        text: "Small extractions change how you see the method; big chunks are less safe and easier to miss details that make the code work"
        correct: true
      - id: o9a90cb
        text: "Tools only extract small pieces"
      - id: o45f6b5
        text: "Small methods are faster"
    explanation: "“After you extract more pieces, you’ll probably see the original method in a different way.” “Too often that isn’t as easy as it looks; it isn’t as safe. It’s easier to miss the details, and the details are what make the code work.”"
  - id: redo
    prompt: "Are extractions you later undo wasted effort?"
    options:
      - id: o0a872f
        text: "Only if tests fail"
      - id: o6c7201
        text: "Yes — undone work is waste"
      - id: insight
        text: "No — they gave insight into the old design and a better way forward"
        correct: true
    explanation: "“Sometimes the best way to move forward is to undo an extraction or two and re-extract. When you do this, it doesn’t mean that the first extractions were wasted effort. They gave you something very important: insight into old design and into a better way of moving forward.”"
---
## Learning objectives

- Choose between skeletonizing and finding sequences
- Extract to the current class first, small pieces first, and be ready to redo

## Connections to today

Version control makes "be prepared to redo" cheap: commit each extraction, and use interactive
rebase or a fresh branch to try a different slicing.
