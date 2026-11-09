---
title: "Replace Function with Function Pointer"
questions:
  - id: why
    prompt: "Why use function pointers rather than Link Substitution or Definition Completion in C?"
    options:
      - id: o9431f5
        text: "Link substitution is impossible in C"
      - id: lighter
        text: "Those can be overkill and need nontrivial build changes; function pointers give seams that could also vary behaviour in production"
        correct: true
      - id: o35296d
        text: "Function pointers are always safer"
    explanation: "“You can use Link Substitution (377) or Definition Completion (337), but often they are overkill for smaller bouts of dependency breaking.” “the seams we get with Link Substitution are not the sort you’d want exploit to vary behavior in production code.”"
  - id: views
    prompt: "How do teams view function pointers?"
    options:
      - id: o5455ae
        text: "Universally safe"
      - id: o48d04b
        text: "Deprecated in C"
      - id: two-camps
        text: "Some see them as horribly unsafe (memory corruption); others as a useful tool used with care"
        correct: true
    explanation: "“On some teams, they are seen as horribly unsafe because it is possible to corrupt their contents and end up calling through some random area of memory. On other teams, they are seen as a useful tool, to be used with care.”"
  - id: steps
    prompt: "What is the step order?"
    options:
      - id: order
        text: "Find declarations → declare same-named function pointers → rename originals → initialise pointers to the old functions → build and rename the bodies"
        correct: true
      - id: od0a084
        text: "Delete functions → add pointers → rewrite callers"
      - id: o9dc9a7
        text: "Add macros → define pointers → link"
    explanation: "“1. Find the declarations” “2. Create function pointers with the same names before each function declaration.” “3. Rename the original function declarations” “4. Initialize the pointers to the addresses of the old functions in a C file.” “5. Run a build to find the bodies of the old functions. Rename them to the new function names.”"
  - id: callers
    prompt: "Why don't callers of `db_store` need editing?"
    options:
      - id: oa16623
        text: "The linker rewrites them"
      - id: same-name
        text: "The pointer has the same name, and C lets you call through a function pointer with ordinary call syntax"
        correct: true
      - id: o39e9e3
        text: "Callers are regenerated"
    explanation: "The technique declares “a function pointer with the same name” and initialises it with `db_store = db_store_production;`, so existing `db_store(...)` calls now go through the pointer."
  - id: upgrade
    prompt: "What does Feathers suggest if you're using this technique in C?"
    options:
      - id: cpp
        text: "Consider moving to C++ (mixed compilation) to gain its other seams"
        correct: true
      - id: oc9bfce
        text: "Rewrite in Java"
      - id: oeafe77
        text: "Stay with pure C"
    explanation: "“if you are using this technique in C, consider upgrading to C++ so that you can take advantage of all of the other seams that C++ provides you.”"
  - id: build-impact
    prompt: "What is a nice property of this technique regarding builds?"
    options:
      - id: compile-time
        text: "It happens entirely at compile time, with minimal impact on the build system"
        correct: true
      - id: o5e20f1
        text: "It needs a special linker"
      - id: odfcb56
        text: "It requires separate executables"
    explanation: "“One of the nice things about it is that it happens completely at compile time, so it has minimal impact on your build system.”"
---
## Learning objectives

- Turn a C function into a replaceable pointer without changing callers

## Connections to today

A `static Func<DateTime> Now = () => DateTime.UtcNow;` field in C# is the same idea — and carries
the same global-state caveat. Prefer an injected `TimeProvider`.
