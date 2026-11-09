---
title: "Characterizing Classes"
questions:
  - id: start
    prompt: "How do you start characterizing a class?"
    options:
      - id: o237056
        text: "Test every private field"
      - id: high-level
        text: "Figure out what it does at a high level, test the simplest thing you can imagine, then follow curiosity"
        correct: true
      - id: o068f12
        text: "Start with the longest method"
    explanation: "“The first thing to do is to try to figure out what the class does at a high level. We can write tests for the simplest thing that we can imagine it doing and then let our curiosity guide us from there.”"
  - id: heuristics
    prompt: "Which heuristics does Feathers give for characterizing classes?"
    type: multiple
    options:
      - id: go-wrong
        text: "List things that can go wrong and try to trigger them"
        correct: true
      - id: extremes
        text: "Think about extreme input values"
        correct: true
      - id: tangled
        text: "Look for tangled logic; use sensing variables to make sure you execute particular areas"
        correct: true
      - id: obd182e
        text: "Test only through the UI"
      - id: invariants
        text: "Write tests for invariants that should hold for the class's lifetime"
        correct: true
    explanation: "The four heuristics: “Look for tangled pieces of logic.” “make a list of the things that can go wrong.” “What happens at extreme values?” “Should any conditions be true at all times during the lifetime of the class? Often these are called invariants.”"
  - id: documentation
    prompt: "Characterization tests are documentation. How should you order them?"
    options:
      - id: o05602a
        text: "Alphabetically"
      - id: intent-first
        text: "Easy cases showing the class's main intent first, then cases highlighting its idiosyncrasies"
        correct: true
      - id: o617be1
        text: "Hardest cases first"
    explanation: "“Start with some easy cases that show the main intent of the class, and then move into cases that highlight its idiosyncrasies.”"
  - id: guided
    prompt: "Why do tests written while characterizing often turn out to suit the change you're about to make?"
    options:
      - id: o317a8c
        text: "Because test frameworks generate them"
      - id: guides-curiosity
        text: "The change you set out to make often guides your curiosity, consciously or not"
        correct: true
      - id: o8f24dc
        text: "Coincidence"
    explanation: "“Whether we think about it consciously or not, the change that we set out to make often guides our curiosity.”"
  - id: bugs-deployed
    prompt: "You find a bug while characterizing a system that's been in production for years. What should you consider before fixing it?"
    options:
      - id: depends
        text: "Whether someone depends on that behaviour — it may take analysis to fix without ripple effects"
        correct: true
      - id: oc6407f
        text: "Nothing; always fix immediately"
      - id: o95523a
        text: "Never fix bugs found during characterization"
    explanation: "“If the system has been deployed, you need to examine the possibility that someone is depending on that behavior, even though you see it as a bug.” If never deployed, “You should fix the bug.”"
  - id: bias
    prompt: "What is Feathers' bias about bugs found during characterization?"
    options:
      - id: o0a6c19
        text: "Log them and move on"
      - id: fix-soon
        text: "Fix clear errors as soon as found; mark suspected ones as suspicious in test code and escalate quickly"
        correct: true
      - id: oe18852
        text: "Ignore them until the refactoring is done"
    explanation: "“My bias is toward fixing bugs as soon as they are found. When behavior is clearly in error, it should be fixed. If you suspect that some behavior is wrong, mark it in the test code as suspicious and then escalate it.”"
  - id: proportion
    prompt: "According to the sidebar, legacy code bugs are usually in proportion to what?"
    options:
      - id: od9cb57
        text: "Its age"
      - id: o8788ab
        text: "Its line count"
      - id: understood
        text: "How little the code is understood"
        correct: true
    explanation: "“All legacy code has bugs, usually in direct proportion to how little it is understood.”"
---
## Learning objectives

- Apply the four characterization heuristics
- Organise characterization tests as readable documentation
- Decide how to handle discovered bugs

## Connections to today

- Name suspicious tests explicitly, e.g. `Rounds_half_down_SUSPICIOUS_see_ticket_123`, or tag them
  with a trait so they surface in reports.
- Snapshot tests are great for breadth but poor at intent. Add a few named, hand-written tests
  for the main intent at the top of the file, as Feathers suggests.
