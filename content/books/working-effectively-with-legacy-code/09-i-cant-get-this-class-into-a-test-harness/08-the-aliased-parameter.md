---
title: "The Case of the Aliased Parameter"
questions:
  - id: why-interface-fails
    prompt: "Why doesn't Extract Interface on `OriginationPermit` simply work?"
    options:
      - id: oa142ea
        text: "`OriginationPermit` is final"
      - id: field-type
        text: "The constructor stores it into a `Permit` field, and an `IOriginationPermit` can't be assigned to `Permit` — Java interfaces can't inherit from classes"
        correct: true
      - id: oead3b0
        text: "Interfaces can't have methods"
    explanation: "“We could create an interface for OriginationPermit, but that wouldn’t do us any good. We would have to assign an IOriginationPermit to a Permit field, and that won’t work. In Java, interfaces can’t inherit from classes.”"
  - id: interfaces-all-the-way
    prompt: "What is Feathers' verdict on creating interfaces all the way down (`IPermit`, `IFacilityPermit`, `IOriginationPermit`)?"
    options:
      - id: oc569ea
        text: "The ideal design"
      - id: o629011
        text: "Impossible in Java"
      - id: yuck
        text: "A ridiculous amount of work that clutters the design with near one-to-one class/interface pairs — acceptable only with your back against the wall"
        correct: true
    explanation: "“Yuck. That is a ridiculous amount of work” and “when we get to the point that we have nearly a one-to-one relationship between classes and interfaces, the design gets cluttered.”"
  - id: ask-why
    prompt: "Before severing a dependency with Extract Interface, what question pays off?"
    options:
      - id: od6850c
        text: "Is it in the same package?"
      - id: o391779
        text: "Who owns the class?"
      - id: why-bad
        text: "Why is the dependency bad — creation pain, a side effect, slowness? If only pieces are a problem, sever only those"
        correct: true
    explanation: "“Sometimes it pays to ask why the dependency is bad.” — “When we use Extract Interface (362), we can get past all of these issues, but we do it by brutally severing the connection to a class. If only pieces of a class are problems, we can take another approach and sever only the connection to them.”"
  - id: technique
    prompt: "Which technique isolates the database access in `OriginationPermit.validate()`?"
    options:
      - id: o2af5f0
        text: "Link Substitution"
      - id: subclass-override
        text: "Subclass and Override Method — a `FakeOriginationPermit` and test subclasses overriding `validate`"
        correct: true
      - id: o532668
        text: "Parameterize Constructor"
    explanation: "“Another strategy that we can use is Subclass and Override Method (401). We can make a class called FakeOriginationPermit that supplies methods that make it easy to change the validation flag.”"
  - id: local-class
    prompt: "What does the `AlwaysValidPermit` class defined inside the test method illustrate?"
    options:
      - id: o54dd94
        text: "A Java bug"
      - id: on-the-fly
        text: "Creating classes on the fly makes special cases very easy in tests, though Feathers avoids it in production"
        correct: true
      - id: oabaaa8
        text: "That tests should never define classes"
    explanation: "“In many languages, we can create classes \"on the fly\" like this in methods. Although I don’t like to do it often in production code, it is very convenient when we are testing.”"
  - id: lucky
    prompt: "Why was the team \"lucky\" in this case, and what if they weren't?"
    options:
      - id: isolated
        text: "The unwanted dependencies were isolated in `validate`; otherwise you must extract methods first (see Chapter 22)"
        correct: true
      - id: oee1a35
        text: "The DBA was on vacation"
      - id: o026e84
        text: "The class had tests already"
    explanation: "“We were lucky that the dependencies we didn’t like were isolated in that validate method. In worse cases, they are intermingled with logic that we need, and we have to extract methods first.”"
---
## Learning objectives

- Recognise when Extract Interface can't satisfy a class-typed field
- Sever only the problematic part with Subclass and Override Method

## Connections to today

C# can't define classes inside methods, but nested private classes in the test class, or a
`Func<bool>` hook passed to a fake, serve the same purpose.
