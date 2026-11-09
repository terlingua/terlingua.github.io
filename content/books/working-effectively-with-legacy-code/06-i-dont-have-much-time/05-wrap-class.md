---
title: "Wrap Class and the Decorator Pattern"
questions:
  - id: concept
    prompt: "What is the key idea of Wrap Class?"
    options:
      - id: of3801f
        text: "Copy the class and modify the copy"
      - id: new-behavior-not-in-class
        text: "Add new behaviour without adding it to an existing class, by putting it in another class that uses the original"
        correct: true
      - id: ob8c687
        text: "Subclass the original and override everything"
    explanation: "“The key to Wrap Class is that you are able to add new behavior into a system without adding it to an existing class.”"
  - id: cannot-instantiate
    prompt: "If you can't instantiate the original class in a harness, how do you make a wrapper testable?"
    options:
      - id: ocd3412
        text: "Make the wrapper static"
      - id: extract
        text: "Use Extract Implementer or Extract Interface on it and have the wrapper implement that interface"
        correct: true
      - id: oe4dec3
        text: "Skip tests for the wrapper"
    explanation: "“Often the easiest way to do this, if you can’t instantiate the original class in a test harness, is to use Extract Implementer (356) or Extract Interface (362) on it and have the wrapper implement that interface.”"
  - id: decorator-why
    prompt: "A `ToolController` needs alarms on raise/lower, logging on on/off and notifications on step, in varying combinations. Why is Decorator a better fit than subclasses?"
    options:
      - id: combinations
        text: "Subclassing every combination explodes; decorators compose behaviours at runtime"
        correct: true
      - id: o13568e
        text: "Decorators run faster"
      - id: o23698b
        text: "Subclasses cannot override methods"
    explanation: "“it won’t do to just create subclasses for each combination of things.” and “Decorator allows you to build up complex behaviors by composing objects at runtime.”"
  - id: basic-class
    prompt: "In a chain of decorators, what must exist at the core?"
    options:
      - id: oa72f8c
        text: "A static factory"
      - id: basic
        text: "At least one \"basic\" concrete class that actually performs the operations, like `ACMEController`"
        correct: true
      - id: oafdce7
        text: "Another decorator"
    explanation: "“When you are using the decorator pattern, you need to have at least one of these \"basic\" classes that you wrap around.”"
  - id: sparingly
    prompt: "Why should decorators be used sparingly?"
    options:
      - id: onion
        text: "Navigating decorators of decorators is like peeling an onion — necessary but it makes your eyes water"
        correct: true
      - id: o71b3d0
        text: "They require reflection"
      - id: o076c2d
        text: "They cannot be tested"
    explanation: "“Navigating through code that contains decorators that decorate other decorators is a lot like peeling away the layers of an onion. It is necessary work, but it does make your eyes water.”"
  - id: decorator-or-not
    prompt: "Logging is needed for `pay()` calls from exactly one place in the code. Which kind of wrapper fits best?"
    options:
      - id: oe9e42f
        text: "A global interceptor"
      - id: o1bb414
        text: "A decorator substituted for `Employee` everywhere"
      - id: non-decorator
        text: "A non-decorator class like `LoggingPayDispatcher`, created in that one place"
        correct: true
    explanation: "“When there are many calls to the code you want to wrap, it often pays to move toward a decorator-ish wrapper.” but “if the new behavior only has to happen in a couple of places, creating a wrapper that isn’t decorator-ish can be very useful.”"
  - id: threshold
    prompt: "Which two cases tip Feathers toward Wrap Class?"
    type: multiple
    options:
      - id: independent
        text: "The new behaviour is completely independent and you don't want to pollute the class with unrelated, low-level behaviour"
        correct: true
      - id: oe39156
        text: "The class has fewer than three methods"
      - id: too-large
        text: "The class is so large you can't stand to make it worse — wrapping puts a stake in the ground"
        correct: true
      - id: oc2efc7
        text: "The class has full test coverage"
    explanation: "“1. The behavior that I want to add is completely independent, and I don’t want to pollute the existing class with behavior that is low level or unrelated. 2. The class has grown so large that I really can’t stand to make it worse.”"
  - id: sprout-vs-wrap
    prompt: "When does Feathers use Sprout Method versus Wrap Method?"
    options:
      - id: o64afb6
        text: "Sprout when tests exist, Wrap when they don't"
      - id: algorithm-vs-importance
        text: "Sprout when the existing method communicates a clear algorithm; Wrap when the new feature is as important as the existing work"
        correct: true
      - id: o4fea59
        text: "Sprout for Java, Wrap for C++"
    explanation: "“I usually use Sprout Method when the code I have in the existing method communicates a clear algorithm to the reader. I move toward Wrap Method when I think that the new feature I’m adding is as important as the work that was there before.”"
---
## Learning objectives

- Implement Wrap Class as a decorator and as a non-decorator wrapper
- Decide between the two forms based on the number of call sites
- Recognise when Wrap Class's higher threshold is met

## In a nutshell

Wrap the existing class in a new one that does the new work and delegates to the old. Use a
decorator (same interface, transparent to callers) when many callers need the behaviour, and a
plain wrapper when only one or two places do.

## Connections to today

```csharp
public sealed class LoggingEmployee(IEmployee inner, ILogger<LoggingEmployee> log) : IEmployee
{
    public void Pay()
    {
        log.LogInformation("Paying {Employee}", inner.Name);
        inner.Pay();
    }

    public string Name => inner.Name;
}
```

- DI containers such as Scrutor's `Decorate<TService, TDecorator>()` register decorators without
  touching call sites.
- Resilience pipelines (Polly, `Microsoft.Extensions.Http.Resilience`) are decorator stacks.
  Feathers' onion warning applies when debugging them.
