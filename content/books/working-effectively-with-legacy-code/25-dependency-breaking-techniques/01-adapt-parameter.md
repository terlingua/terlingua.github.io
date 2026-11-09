---
title: "Adapt Parameter"
questions:
  - id: when
    prompt: "When should you use Adapt Parameter?"
    options:
      - id: o3fd5a0
        text: "Only for primitive parameters"
      - id: cant-extract
        text: "When you can't use Extract Interface on a parameter's class, or the parameter is difficult to fake"
        correct: true
      - id: o538301
        text: "Whenever a method has more than three parameters"
    explanation: "“Use Adapt Parameter when you can’t use Extract Interface (362) on a parameter’s class or when a parameter is difficult to fake.”"
  - id: servlet
    prompt: "Why not just Extract Interface from `HttpServletRequest`?"
    options:
      - id: standard
        text: "It's already a standard API interface with ~23 methods; you can't make it extend your narrower interface"
        correct: true
      - id: o0c51fa
        text: "It's a final class"
      - id: o01fa43
        text: "Java has no interfaces"
    explanation: "“we can’t extract an interface from another interface. In Java, we would need to have HttpServletRequest extend the one we are extracting, and we can’t modify a standard interface that way.”"
  - id: mock-lib
    prompt: "A mock library for `HttpServletRequest` would work. Why does Feathers prefer wrapping?"
    options:
      - id: o6421b0
        text: "Mock libraries are illegal"
      - id: o8411b5
        text: "Mocks are slower"
      - id: aftermath
        text: "Looking ahead, production code would stay tied to a wide API interface; a narrow `ParameterSource` breaks that dependency and communicates better"
        correct: true
    explanation: "“we will have done a lot of work to keep HttpServletRequest, an API interface, in place. Is there a way to make the code look better and make the dependency breaking easier?” — “We can wrap the parameter that is coming in and break our dependency on the API interface entirely.”"
  - id: responsibilities
    prompt: "What design guidance accompanies `ParameterSource`?"
    options:
      - id: o5ae3d5
        text: "Prefer abstract classes"
      - id: communicate
        text: "Move toward interfaces that communicate responsibilities rather than implementation details"
        correct: true
      - id: o83e9aa
        text: "Mirror the API exactly"
    explanation: "“Move toward interfaces that communicate responsibilities rather than implementation details. This makes code easier to read and easier to maintain.”"
  - id: preserve
    prompt: "Why does Adapt Parameter need extra care compared with most catalog techniques?"
    options:
      - id: no-preserve
        text: "It's one case where you don't Preserve Signatures; a simplified interface too different from the original can introduce subtle bugs"
        correct: true
      - id: o26160c
        text: "It always requires reflection"
      - id: o3f7faf
        text: "It deletes code"
    explanation: "“Adapt Parameter is one case in which we don’t Preserve Signatures (312). Use extra care.” and it “can be risky if the simplified interface that you are creating for the parameter class is too different from the parameter’s current interface.”"
  - id: bias
    prompt: "What should your bias be while adapting?"
    options:
      - id: ofb4100
        text: "The fewest lines of code"
      - id: o550e3d
        text: "The ideal design immediately"
      - id: confidence
        text: "Changes you feel confident in, not the best structure — that can come after tests (e.g. a Null Object later)"
        correct: true
    explanation: "“Your bias should be toward making changes that you feel more confident in rather than changes that give you the best structure. Those can come after your tests.”"
  - id: steps
    prompt: "Which is the correct order of Adapt Parameter steps?"
    options:
      - id: order
        text: "New interface → production implementer → fake implementer → simple test passing the fake → change the method → run the test"
        correct: true
      - id: oef121b
        text: "Change the method → write tests → create interface"
      - id: o4ca4af
        text: "Fake → method change → interface → production implementer"
    explanation: "“1. Create the new interface” “2. Create a production implementer” “3. Create a fake implementer” “4. Write a simple test case, passing the fake to the method.” “5. Make the changes you need to in the method” “6. Run your test”."
---
## Learning objectives

- Wrap a hard-to-fake parameter behind a narrow, responsibility-named interface

## Connections to today

`HttpContext`/`HttpRequest` in ASP.NET Core are abstract classes with `DefaultHttpContext`
available for tests, so adapting is less necessary. For other wide types (`IConfiguration`,
`ClaimsPrincipal`), a narrow `ITenantSource`-style interface still communicates better.
