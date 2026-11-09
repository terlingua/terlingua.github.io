---
title: "Pull Up Feature"
questions:
  - id: when
    prompt: "When is Pull Up Feature the right tool?"
    options:
      - id: o4c7357
        text: "When every method uses the database"
      - id: o209d01
        text: "When the class is final"
      - id: unrelated
        text: "When a cluster of methods you need doesn't reference the bad dependencies at all, so you can lift them into an abstract superclass"
        correct: true
    explanation: "“Sometimes you have to work with a cluster of methods on a class, and the dependencies that keep you from instantiating the class are unrelated to the cluster.” “you can pull up the cluster of methods, the feature, into an abstract superclass.”"
  - id: alternatives
    prompt: "Why not Expose Static Method or Break Out Method Object for `getDeadtime`?"
    type: multiple
    options:
      - id: ofd6d2a
        text: "Static methods are forbidden in Java"
      - id: non-static
        text: "It uses many non-static features, so Expose Static Method doesn't fit"
        correct: true
      - id: small
        text: "It's small, and its dependencies on other fields and methods make a method object more work than warranted"
        correct: true
    explanation: "“We could try to use Expose Static Method (345), but we are using many non-static features of the Scheduler class. Break Out Method Object (330) is another possibility, but this is a rather small method, and those dependencies on other methods and fields of the class will make the work more involved”."
  - id: design
    prompt: "Is the resulting split between `Scheduler` and `SchedulingServices` good design?"
    options:
      - id: o99d6ee
        text: "Unacceptable"
      - id: o22b7c2
        text: "Ideal"
      - id: less-ideal
        text: "Less than ideal — features spread across two classes just for testing; a validator delegate would be better, later"
        correct: true
    explanation: "“From a design point of view, it is less than ideal. We’ve spread a set of features across two classes just to make it easier to test.” “A better factoring would be to have Scheduler delegate to some validator object”."
  - id: abstract
    prompt: "Why make the new superclass abstract?"
    options:
      - id: understandable
        text: "So every concrete class in the code is known to be used; uninstantiated concrete classes look like dead code"
        correct: true
      - id: o023033
        text: "Java requires it"
      - id: o0e668a
        text: "For performance"
    explanation: "“I like to make it abstract so that the code is easier to understand. It is great to be able to look at the code in an application and know that every concrete class is being used.”"
  - id: steps
    prompt: "Which steps reduce risk while pulling up?"
    type: multiple
    options:
      - id: o47e9b5
        text: "Delete the original class"
      - id: testing-sub
        text: "Create a testing subclass with methods to set it up"
        correct: true
      - id: missing
        text: "Copy each missing reference the compiler reports, preserving signatures"
        correct: true
      - id: copy-compile
        text: "Copy the methods to the superclass and compile"
        correct: true
    explanation: "“3. Copy the methods to the superclass and compile. 4. Copy each missing reference that the compiler alerts you about to the new superclass. Remember to Preserve Signatures (312)” and “5. … create a subclass for the abstract class and add whatever methods you need to be able to set it up in your tests.”"
---
## Learning objectives

- Lift a dependency-free cluster into a testable abstract superclass

## Connections to today

In C#, an `abstract class SchedulingServices` with `protected List<ScheduleItem> Items` and a
`TestingSchedulingServices` subclass in the test project is a direct translation. Plan to replace
the inheritance with a separate `DeadtimeCalculator` once tests exist.
