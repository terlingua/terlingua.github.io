---
title: "Targeted Testing"
questions:
  - id: what-change
    prompt: "In the `FuelShare` refactoring, which code must a test definitely exercise?"
    options:
      - id: o1edad0
        text: "The `gallons < CORP_MIN` branch, which won't change"
      - id: o025fff
        text: "`lease.postReading`"
      - id: changed-line
        text: "The line that changes from `priceForGallons(gallons)` to a passed-in `totalPrice`"
        correct: true
    explanation: "Of the unchanged corp-minimum logic: “it is not strictly necessary.” But the line using `priceForGallons` “is going to change” — “That’s a small change, but it is a change nonetheless.”"
  - id: branch-check
    prompt: "When writing a test for a branch, what should you ask yourself?"
    options:
      - id: ofd101e
        text: "Is the branch the longest one?"
      - id: o51bfbc
        text: "Does the branch have a comment?"
      - id: other-way
        text: "Could this test pass any other way than by executing the branch? If unsure, use a sensing variable or debugger"
        correct: true
    explanation: "“When you write a test for a branch, ask yourself whether there is any other way that the test could pass, aside from executing that branch. If you are not sure, use a sensing variable (301) or the debugger to find out”."
  - id: truncation
    prompt: "With `CORP_MIN = 10` and `corpBase = 12.0`, a test adds 1 gallon and expects cost 12. The extracted method returns `long` instead of `double`. Why does the test still pass?"
    options:
      - id: ob0940b
        text: "The test isn't run"
      - id: o9c9468
        text: "Java rounds rather than truncates"
      - id: whole-number
        text: "The computed value is exactly 12.0, so truncation to an integer changes nothing — the conversion isn't really exercised"
        correct: true
    explanation: "“That test that we wrote still passes, even though we are silently truncating the value of cost when we return it. A conversion from double to int is being executed, but it isn’t really being fully exercised.”"
  - id: two-things
    prompt: "When refactoring, what two things must you generally check?"
    type: multiple
    options:
      - id: o0a5d55
        text: "Is it faster?"
      - id: connected
        text: "Is it connected correctly?"
        correct: true
      - id: exists
        text: "Does the behaviour exist after the refactoring?"
        correct: true
    explanation: "“When we refactor, we generally have to check for two things: Does the behavior exist after the refactoring, and is it connected correctly?”"
  - id: strategies
    prompt: "Which strategies help catch conversion problems?"
    type: multiple
    options:
      - id: oec3513
        text: "Use larger round-number inputs only"
      - id: smaller
        text: "Characterize a smaller chunk of code"
        correct: true
      - id: sensing
        text: "Use sensing variables to verify the path and conversions"
        correct: true
      - id: manual
        text: "Manually calculate expected values, checking each conversion for truncation"
        correct: true
      - id: debugger
        text: "Step through assignments with a debugger"
        correct: true
    explanation: "“One is to manually calculate the expected values” — “Another technique is to use a debugger” — “A third technique is to use sensing variables (301)” — “There is a fourth option also. We can just decide to characterize a smaller chunk of code.”"
  - id: valuable
    prompt: "Which characterization tests are the most valuable?"
    options:
      - id: o31b45e
        text: "Those with the most assertions"
      - id: path-conversions
        text: "Those that exercise a specific path and each conversion along it"
        correct: true
      - id: o46be4b
        text: "Sunny-day tests only"
    explanation: "“The most valuable characterization tests exercise a specific path and exercise each conversion along the path.”"
  - id: pick-input
    prompt: "Which single input would best expose an accidental double→long truncation in `1.2 * totalPrice`?"
    options:
      - id: fractional
        text: "`totalPrice = 7` (gives 8.4)"
        correct: true
      - id: obb1845
        text: "`totalPrice = 10` (gives 12.0)"
      - id: o1ac918
        text: "`totalPrice = 0`"
    explanation: "An input whose result has a fractional part forces the conversion to matter. The book's point: “Unless we take care to devise inputs that will force us to see that error, we’ll miss it.”"
  - id: tool-quirk
    prompt: "Extracting `x + y` (where `x` is a field) as a method, one tool generated `add(y)` instead of `add(x, y)`. Why?"
    options:
      - id: oa12802
        text: "The tool had a bug that drops parameters randomly"
      - id: o7825b9
        text: "Java forbids two-parameter methods"
      - id: instance-var
        text: "`x` is an instance variable, already available to any extracted method"
        correct: true
    explanation: "“at least one tool on the market will extract it as add(y) rather than add(x,y). Why? Because x is an instance variable and it is available to whatever methods we extract.”"
---
## Learning objectives

- Target tests at exactly the code you will change
- Pick inputs that exercise conversions
- Check existence and connection of moved behaviour

## Connections to today

Java lets `long cost; cost += 1.2 * totalPrice;` compile with a hidden narrowing conversion,
which is exactly the trap in the book. C# rejects that line without an explicit cast, so the
compiler catches more of these. The risk moves to casts you add while extracting —
`(long)(1.2 * totalPrice)` or `(int)amount` on a `decimal` — and those still need inputs with
fractional results to be exercised.
