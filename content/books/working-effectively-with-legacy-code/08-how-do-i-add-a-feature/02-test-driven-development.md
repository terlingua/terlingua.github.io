---
title: "Test-Driven Development (TDD)"
questions:
  - id: algorithm
    prompt: "What is the five-step TDD algorithm given in Chapter 8?"
    options:
      - id: o333d5e
        text: "Write code; write tests; refactor; commit; repeat"
      - id: o20b767
        text: "Design, code, test, debug, release"
      - id: o8e710b
        text: "Write all tests; make all compile; make all pass"
      - id: tdd-steps
        text: "Write a failing test case; get it to compile; make it pass; remove duplication; repeat"
        correct: true
    explanation: "“1. Write a failing test case. 2. Get it to compile. 3. Make it pass. 4. Remove duplication. 5. Repeat.”"
  - id: nan
    prompt: "Why does the first stub of `firstMomentAbout` return `Double.NaN`?"
    options:
      - id: obc7589
        text: "Java requires NaN for new methods"
      - id: fail-first
        text: "To make the test compile but fail — NaN definitely isn't the expected -0.5"
        correct: true
      - id: o4a0f26
        text: "NaN is the correct answer for empty data"
    explanation: "“We want the test to fail, so we have it return the double value NaN (which definitely is not the expected value of -0.5).”"
  - id: step-size
    prompt: "Feathers notes the code written for the first passing test is unusually large. When are such large steps acceptable?"
    options:
      - id: occa698
        text: "Never in TDD"
      - id: certain
        text: "When you are certain of the algorithm you need"
        correct: true
      - id: od32498
        text: "When tests are slow"
    explanation: "“Typically, steps are much smaller, although they can be this large if you are certain of the algorithm you need to use.”"
  - id: exception-test
    prompt: "The exception test calls `firstMomentAbout(0.0)` inside `try`, then `fail(\"expected InvalidBasisException\")`, and catches only `InvalidBasisException`. Reasoning from the code itself, when does this test pass?"
    options:
      - id: ob31efe
        text: "When any exception is thrown"
      - id: o9ffd9c
        text: "When no exception is thrown"
      - id: only-ibe
        text: "Only when `InvalidBasisException` is thrown"
        correct: true
      - id: o6e7b34
        text: "Never"
    explanation: "If nothing is thrown, `fail(...)` fails the test; any other exception escapes the `catch` and errors the test. The surrounding prose reads “It fails if an InvalidBasisException isn’t thrown, and it passes if no exceptions are thrown or any other exception is thrown.” — the second clause does not match the code, so trust the code. At first it fails because “an ArithmeticException is thrown when we divide by 0”."
  - id: compiler-help
    prompt: "After adding `throws InvalidBasisException` to the signature, what drives the next code change?"
    options:
      - id: compiler
        text: "The compiler errors, which say the exception must actually be thrown"
        correct: true
      - id: o36f38e
        text: "A runtime failure in production"
      - id: of3af05
        text: "A code reviewer"
    explanation: "“The compiler errors tell us that we have to actually throw the exception if it is listed in the declaration, so we go ahead and write the code.”"
  - id: generalize-later
    prompt: "Seeing that the nth moment is a general pattern, why not immediately write a general method taking N?"
    type: multiple
    options:
      - id: oec4f13
        text: "Java cannot raise numbers to a power"
      - id: burden
        text: "It would burden callers with supplying N, and clients shouldn't supply arbitrary N"
        correct: true
      - id: finish
        text: "The only job right now is to make it compile; generalise later if still wanted"
        correct: true
    explanation: "“it would burden the callers with the need to supply an N value, and we don’t want to allow clients to supply an arbitrary value for N.” and “Our only job right now is to make it compile. We can generalize later if we find that we still want to.”"
  - id: copy-paste
    prompt: "Why is copying `firstMomentAbout` into `secondMomentAbout` described as powerful in legacy code?"
    options:
      - id: o4f1248
        text: "Duplicated code runs faster"
      - id: side-by-side
        text: "Modifications are easier to understand side by side with the old code, and you can remove duplication later — or discard the attempt"
        correct: true
      - id: o64e385
        text: "Copying is faster to type"
    explanation: "“Often when we want to add features to particularly awful code, it’s easier to understand our modifications if we put them in some new place and can see them side by side with the old code. We can remove duplication later”."
  - id: dup-removal
    prompt: "What makes the final \"remove duplication\" step essential?"
    options:
      - id: oe693ca
        text: "The compiler rejects duplicates"
      - id: burden
        text: "Without it, brute-force copying causes trouble and a maintenance burden; tests make removal easy"
        correct: true
      - id: oeb8032
        text: "It is optional"
    explanation: "“if we don’t remove the duplication afterward, we are just causing trouble and making a maintenance burden. On the other hand, if we have tests in place, we are able to remove duplication easily.”"
  - id: legacy-step-zero
    prompt: "How is the TDD algorithm extended for legacy code?"
    type: multiple
    options:
      - id: od515d1
        text: "Skip the \"remove duplication\" step"
      - id: oa52cc8
        text: "Write all tests after the change"
      - id: dont-change
        text: "While making it pass, try not to change existing code"
        correct: true
      - id: step-zero
        text: "Step 0: get the class you want to change under test"
        correct: true
    explanation: "“0. Get the class you want to change under test.” and “3. Make it pass. (Try not to change existing code as you do this.)”"
  - id: one-thing
    prompt: "Why is TDD's focus on one thing at a time especially valuable in legacy code?"
    options:
      - id: separate-activities
        text: "You are either writing code or refactoring, never both — so new code can be written independently, then duplication with old code refactored away"
        correct: true
      - id: of7b6a8
        text: "It avoids the need for any refactoring"
      - id: o86cbca
        text: "It lets you skip compiling"
    explanation: "“We are either writing code or refactoring; we are never doing both at once.” and “After we have written some new code, we can refactor to remove any duplication between it and the old code.” (The printed sidebar says “write new code independently of new code”, which reads as a slip for old code.)"
---
## Learning objectives

- Run the TDD cycle, including the legacy step 0
- Use copy-then-deduplicate safely
- Reason about what a test actually asserts, not what prose says

## In a nutshell

Write the test for the method you wish existed, make it compile with a failing stub, make it
pass, then remove duplication. In legacy code, first get the class under test and avoid changing
old code while making the new test pass.

## Connections to today

```csharp
[Fact]
public void First_moment_about_point()
{
    var calc = new InstrumentCalculator();
    calc.AddElement(1.0);
    calc.AddElement(2.0);
    Assert.Equal(-0.5, calc.FirstMomentAbout(2.0), precision: 10);
}

[Fact]
public void Throws_with_no_elements() =>
    Assert.Throws<InvalidBasisException>(() => new InstrumentCalculator().FirstMomentAbout(0.0));
```

`Assert.Throws<T>` replaces the try/fail/catch idiom and removes the ambiguity discussed in the
quiz.
