---
title: "Supersede Instance Variable"
questions:
  - id: pager
    prompt: "`TestingPager` overrides virtual `formConnection`, yet constructing it still talks to hardware. Why?"
    options:
      - id: oe6460e
        text: "The override is misspelled"
      - id: o3055d0
        text: "Virtual functions don't work in tests"
      - id: ctor-dispatch
        text: "In C++, the virtual call from the base constructor uses the base version, not the override"
        correct: true
    explanation: "“When we constructed the TestingPager, Page::formConnection was called during initialization because C++ did not allow the override in the constructor.” The override is used later in `sendMessage`."
  - id: why-rule
    prompt: "Why does C++ have this rule?"
    options:
      - id: unsafe
        text: "Calling an override from a base constructor could touch derived members not yet initialised (like `c` in class B)"
        correct: true
      - id: o0e668a
        text: "For performance"
      - id: o1c47bf
        text: "For binary compatibility"
    explanation: "“C++ has this rule because constructor calls to overridden virtual functions can be unsafe.” In the example, `c` “was never initialized because B’s constructor hasn’t been run yet.”"
  - id: which
    prompt: "In C++, when should you use Extract and Override Getter vs. Supersede Instance Variable?"
    options:
      - id: used-in-ctor
        text: "Getter if the object isn't used in the constructor; Supersede if it is used but must be replaced before another method is called"
        correct: true
      - id: o74b706
        text: "Always the getter"
      - id: o857174
        text: "Always supersede"
    explanation: "“If the object that you are replacing is not used in the constructor, you can use Extract and Override Getter (352) to break the dependency. If you do use the object but you need to make sure that you can replace it before another method is called, you can use Supersede Instance Variable.”"
  - id: poor-practice
    prompt: "Why are setters that change an object's base collaborators generally poor practice?"
    options:
      - id: history
        text: "They let clients drastically change behaviour during the object's life, so you must know its history to understand it"
        correct: true
      - id: oa39808
        text: "They break serialisation"
      - id: oda4990
        text: "They are slow"
    explanation: "“Those setters allow clients to drastically change the behavior of an object during its lifetime. When someone can make those changes, you have to know the history of that object to understand what happens when you call one of its methods.”"
  - id: supersede-word
    prompt: "What's the benefit of the unusual prefix \"supersede\"?"
    options:
      - id: obe33af
        text: "The compiler recognises it"
      - id: searchable
        text: "It's uncommon, so you can quickly search to make sure no production code uses these methods"
        correct: true
      - id: o9baf83
        text: "It's a C++ keyword"
    explanation: "“One nice thing about using the word supersede as the method prefix is that it is kind of fancy and uncommon. If you ever get concerned about whether people are using the superceding methods in production code, you can do a quick search”."
  - id: references
    prompt: "In step 3, what must you verify when the superseded variable is a reference?"
    options:
      - id: other-refs
        text: "That no other references in the class point to the same object — otherwise replacement may need more work"
        correct: true
      - id: oe9ee83
        text: "That it is null"
      - id: o1286a3
        text: "That it is const"
    explanation: "“If the variable is a reference, verify that there aren’t any other references in the class to the object it points to. If there are, you might have additional work to do in the superceding method”."
---
## Learning objectives

- Explain C++'s constructor-dispatch rule and its workaround

## Connections to today

C# behaves like Java here: overrides *do* run from base constructors, which is why analyzers warn
against it. Supersede Instance Variable is rarely needed in .NET; constructor injection is the norm.
