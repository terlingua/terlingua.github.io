---
title: "The Case of the Irritating Global Dependency"
questions:
  - id: frameworks
    prompt: "What distinction does Feathers draw between frameworks and \"old-style\" reuse?"
    options:
      - id: ob2cb3f
        text: "Frameworks are always open source"
      - id: use-us
        text: "Frameworks mostly use our code; old-style reuse is pulling in classes we use directly — which requires classes be independently compilable"
        correct: true
      - id: o756ff8
        text: "Old-style reuse is obsolete"
    explanation: "“many of them are not really things that we use; they are things that use our code.” He doubts old-style reuse “if we can’t pull a random class out of an average application and compile it independently in a test harness without doing a lot of work”."
  - id: why-not-param
    prompt: "Why not just Parameterize Constructor for `PermitRepository.getInstance()` in `Facility`?"
    options:
      - id: widespread
        text: "The same line appears in 10 other classes, in constructors, regular methods and static methods"
        correct: true
      - id: oe266e6
        text: "Constructors can't take repositories"
      - id: o619dc5
        text: "The repository is final"
    explanation: "“There are 10 other classes that have roughly the same line of code. It sits in constructors, regular methods, and static methods.”"
  - id: opacity
    prompt: "What is the \"opacity\" problem with globals?"
    options:
      - id: cant-tell
        text: "You can't tell from a piece of code what it can affect, because it may read or modify state declared elsewhere"
        correct: true
      - id: o14520f
        text: "They can't be serialised"
      - id: o00b3cf
        text: "They are slow to access"
    explanation: "“When we look at a piece of code, it is nice to be able to know what it can affect.” With globals, “We can look at the use of a class such as Account and not have a clue whether it is accessing or modifying variables declared someplace else”."
  - id: mini-app
    prompt: "Why does a singleton conflict with testing?"
    options:
      - id: o46ea06
        text: "Test frameworks forbid static fields"
      - id: isolation
        text: "Each test should be a totally isolated mini-application, but a singleton forbids more than one instance"
        correct: true
      - id: o7e9bfb
        text: "Singletons can't have methods"
    explanation: "“each test in a suite of tests should be a mini-application, in a way: It should be totally isolated from the other tests. So, to run code containing singletons in a test harness, we have to relax the singleton property.”"
  - id: static-setter
    prompt: "What is the first step of Introduce Static Setter?"
    options:
      - id: o9576d1
        text: "Delete `getInstance`"
      - id: o95d1d6
        text: "Make every caller take a parameter"
      - id: set-testing
        text: "Add a static method (`setTestingInstance`) that replaces the static instance"
        correct: true
    explanation: "“The first step is to add a new static method to the singleton class. The method allows us to replace the static instance in the singleton. We’ll call it setTestingInstance.”"
  - id: reset
    prompt: "When is a `resetForTesting()` method enough, and when is Introduce Static Setter better?"
    options:
      - id: o0a46e1
        text: "Neither works with singletons"
      - id: public-methods
        text: "Reset works if public methods let you set up any state needed; if not, or if external resources affect state, use the static setter with a subclass"
        correct: true
      - id: od9d1bb
        text: "Reset is always better"
    explanation: "“This scheme works well when the public methods on the singleton allow you to set up the singleton’s state every way you need to during testing. If the singleton doesn’t have those public methods or uses some external resources that affect its state, Introduce Static Setter (372) is the better choice.”"
  - id: why-singletons
    prompt: "Besides the three \"real\" reasons for a single instance, why do people often create singletons?"
    options:
      - id: global
        text: "Because they want a global variable and passing it around feels too painful"
        correct: true
      - id: o3e36ef
        text: "Because the language requires it"
      - id: oda490f
        text: "For thread safety"
    explanation: "“Often people create singletons because they want to have a global variable. They feel that it would be too painful to pass the variable around to the places where it is needed.”"
  - id: protection
    prompt: "If you relax the singleton property, what other protections can you add?"
    type: multiple
    options:
      - id: build-check
        text: "A build check that `setTestingInstance` isn't called from non-test code"
        correct: true
      - id: team-rule
        text: "A team rule that there is one instance"
        correct: true
      - id: runtime
        text: "A runtime check that raises an alarm if it is called"
        correct: true
      - id: od12d81
        text: "Making the class abstract"
    explanation: "“We could add a check to our build system in which we search through all the source files to make sure that setTestingInstance is not called by non-testing code. We can do the same thing with runtime checks.” and “we can rely on a team rule.”"
  - id: protected-ctor
    prompt: "When using a `TestingPermitRepository` subclass, how can you preserve part of the singleton property?"
    options:
      - id: protected
        text: "Make the base constructor protected rather than public"
        correct: true
      - id: o5bd2b1
        text: "Seal the base class"
      - id: occ24fa
        text: "Make the subclass a singleton too"
    explanation: "“we can make the constructor of PermitRepository protected rather than public. That will prevent the creation of more than one PermitRepository, although it does allow us to create subclasses.”"
  - id: doesnt-fix
    prompt: "What does Introduce Static Setter not do?"
    options:
      - id: o3f8fa7
        text: "It doesn't compile in Java"
      - id: global-remains
        text: "It doesn't do much to get past the global dependencies themselves — Parameterize Method/Constructor do that"
        correct: true
      - id: o5d0a31
        text: "It doesn't allow tests"
    explanation: "“This is a technique that we can use to get tests in place despite extensive global dependencies. Unfortunately, it doesn’t do much to get past the global dependencies.”"
  - id: global-exercise
    prompt: "When you search a large application for a global variable, what do you usually find?"
    options:
      - id: not-globally-used
        text: "It is globally accessible but used in relatively few places"
        correct: true
      - id: oa51603
        text: "It is used in every class"
      - id: of7e07c
        text: "It is never used"
    explanation: "“In most cases, variables that are global are globally accessible, but they really aren’t globally used.” If it truly is used everywhere, “it means there isn’t any layering in your code.”"
---
## Learning objectives

- Explain why singletons hinder isolated tests
- Apply Introduce Static Setter, `resetForTesting` and testing subclasses
- Plan the longer-term move away from a global

## Connections to today

- Static `DbContext` accessors, `HttpContext.Current`, `ConfigurationManager.AppSettings` and
  service locators are common .NET globals. `IHttpContextAccessor`, `IOptions<T>` and constructor
  injection are the long-term fixes; a static setter is the short-term bridge.
- xUnit runs test classes in parallel by default. A static testing instance shared across tests
  can make results flaky unless those tests share a non-parallel collection.
