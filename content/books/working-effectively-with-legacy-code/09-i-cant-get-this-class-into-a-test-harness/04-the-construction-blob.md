---
title: "The Case of the Construction Blob"
questions:
  - id: why-not-param
    prompt: "Why isn't Parameterize Constructor always the best choice for a construction blob?"
    type: multiple
    options:
      - id: chained
        text: "Objects created inside are used to create other objects, so moving creation out is unsafe without tests and burdens clients"
        correct: true
      - id: long-list
        text: "Many objects or globals created inside can lead to a very large parameter list"
        correct: true
      - id: oc14b66
        text: "Parameterized constructors are slower"
    explanation: "“we could end up with a very large parameter list. In worse situations, a constructor creates a few objects and then uses them to create other objects” and moving cursor creation out “isn’t very safe if we don’t have tests in place, and it could be a big burden on clients”."
  - id: cpp-virtual
    prompt: "Why can't Extract and Override Factory Method be used in a C++ constructor?"
    options:
      - id: o1dd615
        text: "C++ has no virtual functions"
      - id: no-dispatch
        text: "C++ doesn't resolve virtual calls in constructors to derived-class overrides"
        correct: true
      - id: ocbc249
        text: "Factory methods must be static in C++"
    explanation: "“C++ doesn’t allow calls to virtual functions in constructors to resolve to virtual functions defined in derived classes.”"
  - id: general-danger
    prompt: "Even in Java and C#, why is calling overridable methods from a constructor generally not a good idea?"
    options:
      - id: o4acb05
        text: "It disables garbage collection"
      - id: uninitialized
        text: "The override may access derived-class state before the base constructor has finished — uninitialized variables"
        correct: true
      - id: o9ed1e7
        text: "It is a syntax error"
    explanation: "“Functions in derived classes often assume that they can use variables from their base class. Until the constructor of the base class is completely finished, there is a chance that an overridden function that it calls can access an uninitialized variable.”"
  - id: supersede
    prompt: "What does Supersede Instance Variable do?"
    options:
      - id: oa63b98
        text: "Replaces a class with a subclass at link time"
      - id: setter
        text: "Adds a setter that swaps in another instance after the object is constructed"
        correct: true
      - id: odaea79
        text: "Makes a field static"
    explanation: "“We write a setter on the class that allows us to swap in another instance after we construct the object.”"
  - id: cpp-care
    prompt: "What extra care does Supersede Instance Variable need in C++?"
    options:
      - id: delete
        text: "You must delete the old object and understand what its destructor destroys, or risk subtle bugs"
        correct: true
      - id: o6adf97
        text: "None"
      - id: occ6482
        text: "You must make the field `const`"
    explanation: "“When we replace an object, we have to get rid of the old one.” — “we have to understand what the destructor does and whether it destroys anything that is passed to the object’s constructor.”"
  - id: production
    prompt: "What warning applies to superseding methods in garbage-collected languages?"
    options:
      - id: od14b2f
        text: "They are thread-safe by default"
      - id: not-production
        text: "Don't use them in production code; superseding objects that manage resources can cause serious resource problems"
        correct: true
      - id: oc43292
        text: "They leak memory immediately"
    explanation: "“But we should be very careful not to use the superseding method in production code. If the objects that we are superseding manage other resources, we can cause some serious resource problems.”"
  - id: preference
    prompt: "How does Feathers feel about Supersede Instance Variable?"
    options:
      - id: o332403
        text: "It's his first choice everywhere"
      - id: ob8f32f
        text: "He never uses it"
      - id: reluctant
        text: "He avoids it unless he can't — but uses it occasionally in C++ because factory methods can't be overridden in constructors there"
        correct: true
    explanation: "“I don’t like to use Supersede Instance Variable (404) unless I can’t avoid it. The potential for resource-management problems is too great. However, I do use it in C++ at times.”"
---
## Learning objectives

- Recognise a construction blob
- Choose between Extract and Override Factory Method and Supersede Instance Variable by language

## Connections to today

C# analyzers such as CA2214 ("Do not call overridable methods in constructors") encode the same
danger Feathers describes. In C#, prefer Parameterize Constructor or a factory delegate parameter
(`Func<IFocusWidget>`) over overriding in constructors.
