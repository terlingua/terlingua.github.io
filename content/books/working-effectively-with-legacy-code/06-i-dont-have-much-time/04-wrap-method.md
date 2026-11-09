---
title: "Wrap Method"
questions:
  - id: temporal
    prompt: "What is \"temporal coupling\" and why is it a problem?"
    options:
      - id: o8c2ede
        text: "Tests that must run in order"
      - id: same-time
        text: "Grouping code only because it must happen at the same time; the weak relationship makes later separation hard"
        correct: true
      - id: o6044f3
        text: "Code that depends on the system clock"
    explanation: "“Back in the early days of programming, this was named temporal coupling” — “When you group things together just because they have to happen at the same time, the relationship between them isn’t very strong.”"
  - id: form-one
    prompt: "In the first form of Wrap Method, what happens to `pay()`?"
    options:
      - id: o301786
        text: "It is made virtual and overridden"
      - id: rename
        text: "The old body is renamed (`dispatchPayment`) and a new `pay()` calls the logging and then the old code"
        correct: true
      - id: o0675c3
        text: "It is deleted and callers are updated"
    explanation: "“I’ve renamed pay() as dispatchPayment() and made it private. Next, I created a new pay method that calls it.” Clients “don’t have to know or care about the change.”"
  - id: form-two
    prompt: "When would you use the second form (`makeLoggedPayment`)?"
    options:
      - id: o8c2008
        text: "When the method is static"
      - id: explicit
        text: "When you want a new method no one calls yet, so the new behaviour is explicit and callers choose"
        correct: true
      - id: o2742c7
        text: "When all existing callers must get the new behaviour"
    explanation: "“There is another form of Wrap Method that we can use when we just want to add a new method, a method that no one calls yet.” and “Now users have the option of paying in either way.”"
  - id: limitation
    prompt: "What constraint does Wrap Method impose on the new feature — and does Feathers mind?"
    options:
      - id: oc5c8e0
        text: "It must be asynchronous"
      - id: before-after
        text: "It must happen before or after the old feature, not intertwined — and that is actually fine"
        correct: true
      - id: o5a38de
        text: "It must be in a different class — which is bad"
    explanation: "“the new feature that you add can’t be intertwined with the logic of the old feature. It has to be something that you do either before or after the old feature. Wait, did I say that is bad? Actually, it isn’t.”"
  - id: real-downside
    prompt: "What is the more real downside?"
    options:
      - id: o8f4aec
        text: "It breaks binary compatibility"
      - id: names
        text: "You must invent a new name for the old code, which can lead to poor names"
        correct: true
      - id: o5d25f8
        text: "Performance overhead of an extra call"
    explanation: "“The second (and more real) downside is that you have to make up a new name for the old code that you had in the method.” and “The primary disadvantage of Wrap Method is that it can lead to poor names.”"
  - id: preserve-sig
    prompt: "Which Chapter 23 practice does step 2 of the first form explicitly remind you to use?"
    options:
      - id: oc3c97b
        text: "Lean on the Compiler"
      - id: preserve
        text: "Preserve Signatures"
        correct: true
      - id: o0ab531
        text: "Single-Goal Editing"
    explanation: "“rename the method and then create a new method with the same name and signature as the old method. Remember to Preserve Signatures (312) as you do this.”"
  - id: size
    prompt: "How does Wrap Method compare with the sprout techniques regarding existing method size?"
    options:
      - id: oc059c1
        text: "They are identical"
      - id: no-growth
        text: "Sprouts add at least one line to existing methods; Wrap Method does not increase their size"
        correct: true
      - id: oc05b90
        text: "It grows methods more than sprouting does"
    explanation: "“Sprout Method and Sprout Class add code to existing methods and make them longer by at least one line, but Wrap Method does not increase the size of existing methods.”"
  - id: better-names
    prompt: "With tests in place, how would Feathers improve `dispatchPayment`, which also calculates pay?"
    options:
      - id: calculate
        text: "Extract `calculatePay()` so `pay()` reads log → calculate → dispatch"
        correct: true
      - id: oc18fba
        text: "Rename it to `doEverything()`"
      - id: o17f730
        text: "Merge logging into it"
    explanation: "“I’d extract the first part of dispatchPayment() into its own method named calculatePay()” — yielding `logPayment(); Money amount = calculatePay(); dispatchPayment(amount);`."
---
## Learning objectives

- Apply both forms of Wrap Method
- Explain temporal coupling
- Choose between Sprout Method and Wrap Method

## In a nutshell

Rename the old method, create a new one with the original signature that calls the new tested
behaviour and the old code. Or add a new explicitly named method that does both. New behaviour
must come before or after the old — never interleaved.

## Connections to today

```csharp
public void Pay()
{
    LogPayment();
    DispatchPayment();
}

private void DispatchPayment() { }
private void LogPayment() { }
```

- ASP.NET Core middleware, MediatR pipeline behaviours and `DelegatingHandler`s are
  architecture-level "wrap" mechanisms for before/after behaviour.
- Watch for temporal coupling in event handlers: "on order placed, also do X" accumulates exactly
  the way Feathers describes.
