---
title: "Encapsulate Global References"
questions:
  - id: three-choices
    prompt: "With problematic global dependencies, what are your three choices?"
    type: multiple
    options:
      - id: act-differently
        text: "Make the globals act differently under test"
        correct: true
      - id: encapsulate
        text: "Encapsulate the globals to decouple further"
        correct: true
      - id: link
        text: "Link to different globals"
        correct: true
      - id: oce1dc5
        text: "Delete the globals"
    explanation: "“You can try to make the globals act differently under test, you can link to different globals, or you can encapsulate the globals so that you can decouple things further. The last option is called Encapsulate Global References.”"
  - id: bundle
    prompt: "What heuristic tells you globals belong in the same class?"
    options:
      - id: together
        text: "They're always used or modified near each other"
        correct: true
      - id: o2fdf65
        text: "They have the same type"
      - id: o80aee8
        text: "They're declared in the same file"
    explanation: "“If several globals are always used or are modified near each other, they belong in the same class.”"
  - id: naming
    prompt: "How should you approach naming the new class?"
    options:
      - id: oac9117
        text: "Always call it `Globals`"
      - id: o64e1ae
        text: "Use the name of the first global"
      - id: future-methods
        text: "Think about the methods that will eventually live on it; the name should be good but needn't be perfect — you can rename later"
        correct: true
    explanation: "“When naming a class, think about the methods that will eventually reside on it. The name should be good, but it doesn’t have to be perfect. Remember that you can always rename the class later.”"
  - id: why-public-data
    prompt: "Why move the globals into a class as public data rather than immediately moving logic onto a real `Frame` class?"
    options:
      - id: o1115c3
        text: "C++ forbids methods on such classes"
      - id: leave-logic
        text: "Without tests, leave logic alone; get separation via seams first, move behaviour later with tests"
        correct: true
      - id: oa5add7
        text: "Public data is better design"
    explanation: "“when we don’t have tests in place and we are trying to do the minimal work we need to get tests in place, it is best to leave logic alone as much as possible.” “Later, when we have more tests in place, we can move behavior from one class to another with impunity.”"
  - id: free-functions
    prompt: "For scattered free functions like `::GetOption`, why use an abstract `OptionSource` with virtual methods rather than static wrappers?"
    options:
      - id: of9bc0d
        text: "Static methods can't call free functions"
      - id: explicit-seams
        text: "Virtual methods on a parameterized class give explicit, manageable object seams; statics or free-function wrappers would need link or preprocessing seams"
        correct: true
      - id: o9c6947
        text: "Virtual methods are faster"
    explanation: "“neither of those approaches would have given us good seams. We would have had to use the link seam (36) or the preprocessing seam (33)” — “When we use the class and virtual function approach and parameterize the class, the seams that we have are explicit and easy to manage.”"
  - id: production-impl
    prompt: "What should each production method in `ProductionOptionSource` do?"
    options:
      - id: delegate
        text: "Nothing more than delegate to the global function"
        correct: true
      - id: ode0130
        text: "Cache results"
      - id: o54ebd9
        text: "Validate inputs"
    explanation: "“Each of the functions in the production code should do nothing more than delegate to a global function.”"
  - id: first-step-only
    prompt: "After references go through the global instance, what is still needed to use fakes?"
    options:
      - id: o319d41
        text: "Nothing; fakes work automatically"
      - id: o71f436
        text: "A link seam"
      - id: next
        text: "Introduce Static Setter, Parameterize Constructor, Parameterize Method or Replace Global Reference with Getter"
        correct: true
    explanation: "“Referencing a member of a class rather than a simple global is only the first step. Afterward, consider whether you should use Introduce Static Setter (372), or parameterize the code”."
---
## Learning objectives

- Encapsulate related globals or free functions behind a class
- Use the compiler to redirect references safely

## Steps

1. Identify the globals.
2. Create a class to reference them from.
3. Copy the globals in; handle initialisation.
4. Comment out the originals.
5. Declare a global instance of the new class.
6. Lean on the Compiler for unresolved references.
7. Prefix each with the instance name.
8. Add a seam for fakes (static setter, parameterize, getter).

## Connections to today

`static class AppSettings { public static string ConnectionString; }` is a modern global. Wrap it
in an `IAppSettings` (or `IOptions<T>`) instance first, then inject it.
