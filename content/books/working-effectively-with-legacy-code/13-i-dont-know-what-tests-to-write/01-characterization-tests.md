---
title: "Characterization Tests"
questions:
  - id: bug-finding
    prompt: "Why does Feathers say bug finding in legacy code can be misdirected effort?"
    options:
      - id: o04f0c2
        text: "Legacy code has no bugs"
      - id: oafa26a
        text: "Bugs should be found only by QA"
      - id: prevent
        text: "Finding bugs is usually easy; the way to win is to concentrate on not putting bugs in to begin with"
        correct: true
    explanation: "“No, finding bugs in legacy code usually isn’t a problem. In terms of strategy, it can actually be misdirected effort.” — “The way to win is to concentrate effort on not putting bugs into code in the first place.”"
  - id: purpose
    prompt: "What should automated tests generally do, according to Chapter 13?"
    options:
      - id: o660926
        text: "Measure performance"
      - id: o83ea88
        text: "Find bugs on their first run"
      - id: specify-preserve
        text: "Specify a goal we want to fulfil, or preserve behaviour that is already there"
        correct: true
    explanation: "“In general, automated tests should specify a goal that we’d like to fulfill or attempt to preserve behavior that is already there. In the natural flow of development, tests that specify become tests that preserve.”"
  - id: requirements
    prompt: "Why not write tests from old requirements documents?"
    options:
      - id: actual-matters
        text: "What the system does is more important than what it's supposed to do; tests from assumptions put you back to bug finding"
        correct: true
      - id: o4c065a
        text: "Requirements documents are confidential"
      - id: oc42969
        text: "It is too slow"
    explanation: "“In nearly every legacy system, what the system does is more important than what it is supposed to do. If we write tests based on our assumption of what the system is supposed to do, we’re back to bug finding again.”"
  - id: definition
    prompt: "What is a characterization test?"
    options:
      - id: oc22c2f
        text: "A test derived from a specification"
      - id: o593571
        text: "A performance benchmark"
      - id: actual
        text: "A test that characterizes the actual behavior of a piece of code"
        correct: true
    explanation: "“A characterization test is a test that characterizes the actual behavior of a piece of code.” “The tests document the actual current behavior of the system.”"
  - id: algorithm
    prompt: "Put the characterization test algorithm in order."
    options:
      - id: o39983b
        text: "Write the expected value from the spec → run → fix code until it passes"
      - id: oed87c8
        text: "Run the code manually → copy output → write the test later"
      - id: correct-order
        text: "Use code in a harness → write an assertion you know will fail → let the failure tell you the behaviour → change the test to expect it → repeat"
        correct: true
    explanation: "“1. Use a piece of code in a test harness. 2. Write an assertion that you know will fail. 3. Let the failure tell you what the behavior is. 4. Change the test so that it expects the behavior that the code produces. 5. Repeat.”"
  - id: moral
    prompt: "\"If we just paste in what the code produces, and the code has a bug, isn't the test wrong?\" How does Feathers resolve this?"
    options:
      - id: oc72f3a
        text: "Characterization tests should be deleted after refactoring"
      - id: o443b64
        text: "Only paste values you've verified by hand"
      - id: no-authority
        text: "The tests have no moral authority; they're a mechanism to detect later differences from current behaviour, not a gold standard"
        correct: true
    explanation: "“They aren’t really tests written as a gold standard that the software must live up to.” — “They don’t have any moral authority; they just sit there documenting what pieces of the system really do.”"
  - id: unexpected
    prompt: "You characterize a method and the output looks wrong. What should you do with the test?"
    options:
      - id: oba2a25
        text: "Change the expected value to what you think is right"
      - id: suspicious
        text: "Keep it in the suite, mark it as suspicious, and find out the effect of fixing it"
        correct: true
      - id: o81f223
        text: "Delete it"
    explanation: "“It could be a bug. That doesn’t mean that we don’t include the test in our test suite; instead, we should mark it as suspicious and find out what the effect would be of fixing it.”"
  - id: not-black-box
    prompt: "How do you decide which cases to characterize, given the infinite number possible?"
    options:
      - id: curiosity-change
        text: "These aren't black-box tests — read the code, write tests until you understand it, then check whether they'd sense problems from your planned change"
        correct: true
      - id: odf70dc
        text: "Test only public getters"
      - id: od18f32
        text: "Random input generation until coverage is 100%"
    explanation: "“we aren’t writing black-box tests here. We are allowed to look at the code we are characterizing.” Then: “We think about the changes that we want to make in the code and try to figure out whether the tests that we have will sense any problems that we can cause.”"
  - id: no-confidence
    prompt: "If you can't get confidence that your tests will sense problems from a planned change, what then?"
    options:
      - id: different-way
        text: "Consider changing the software in a different way — perhaps do a piece of it first"
        correct: true
      - id: o7f9129
        text: "Make the change anyway and rely on QA"
      - id: o659001
        text: "Abandon the change"
    explanation: "“If we can’t feel that confidence, it’s safer to consider changing the software in a different way. Maybe we can do a piece of what we were considering first.”"
  - id: method-use
    prompt: "What is the Method Use Rule?"
    options:
      - id: oee9369
        text: "Only use methods with documentation"
      - id: rule
        text: "Before you use a method in a legacy system, check for tests; if there aren't any, write them"
        correct: true
      - id: ofb0115
        text: "Never call a method you didn't write"
    explanation: "“Before you use a method in a legacy system, check to see if there are tests for it. If there aren’t, write them.” This uses “tests as a medium of communication.”"
---
## Learning objectives

- Write characterization tests with the fail-first trick
- Explain why characterization tests carry no moral authority
- Decide when you have enough characterization

## In a nutshell

Assert something you know is wrong, read the failure, paste in the real value. Repeat until you
understand the code and your tests would notice the change you plan.

## Connections to today

```csharp
[Fact]
public Task Generates_page_for_base_row()
{
    var generator = new PageGenerator();
    generator.Assoc(RowMappings.GetRow(Page.BaseRow));
    return Verify(generator.Generate());
}
```

- Snapshot/approval tools such as **Verify** and **ApprovalTests** automate step 3–4: the first
  run writes a `.received` file, you approve it, and later runs compare against it. That is
  characterization testing at scale.
- Property-based testing (FsCheck, CsCheck) can explore inputs, but characterization still needs
  you to decide which recorded outputs matter for the change.
