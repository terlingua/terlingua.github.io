---
title: "Seam Types — Preprocessing, Link and Object Seams"
questions:
  - id: preproc-enabling
    prompt: "In the `db_update` example, where is the seam and what is the enabling point?"
    options:
      - id: correct-pair
        text: "Seam — the text of the `db_update` call; enabling point — a preprocessor define named `TESTING`"
        correct: true
      - id: ocf1642
        text: "Seam — `localdefs.h`; enabling point — the linker"
      - id: o13f485
        text: "Seam — the `account_update` signature; enabling point — the makefile"
      - id: o8b3608
        text: "Seam — `MASTER_ACCOUNT`; enabling point — the database"
    explanation: "“we wanted to change the behavior at the text of the db_update call.” and “In this case, the enabling point is a preprocessor define named TESTING.”"
  - id: preproc-caution
    prompt: "Why does Feathers advise against excessive preprocessing in production code even though he likes the seams it gives?"
    type: multiple
    options:
      - id: several-programs
        text: "Conditional compilation forces you to maintain several programs in the same source"
        correct: true
      - id: clarity
        text: "It tends to decrease code clarity"
        correct: true
      - id: obscure-bugs
        text: "Macros are simple text replacement and can hide obscure bugs"
        correct: true
      - id: o633508
        text: "Preprocessors make code run slower"
    explanation: "“It’s not a good idea to use excessive preprocessing in production code because it tends to decrease code clarity.” Conditional directives “pretty much force you to maintain several different programs in the same source code” and “It is easy to create macros that hide terribly obscure bugs.”"
  - id: link-java
    prompt: "To supply a test version of `fit.Parse` in `FitFilter`, where is the seam and where is the enabling point?"
    options:
      - id: o52368f
        text: "Seam — `main`; enabling point — `argv`"
      - id: classpath
        text: "Seam — the `new Parse` call in `process`; enabling point — the classpath"
        correct: true
      - id: o0e71dc
        text: "Seam — the import statement; enabling point — the constructor"
    explanation: "“The seam is the new Parse call in the process method. The enabling point is the classpath.”"
  - id: tell-interface
    prompt: "Why is a graphics library an especially good candidate for stubbing at a link seam?"
    options:
      - id: tell
        text: "It is almost a pure \"tell\" interface, so empty stubs work; \"ask\" calls need meaningful return values"
        correct: true
      - id: ob019b0
        text: "Graphics libraries are always dynamically linked"
      - id: o6594d8
        text: "Graphics code is never tested"
    explanation: "“it is almost a pure \"tell\" interface. You issue calls to functions to tell them to do something, and you aren’t asking for much information back. Asking for information is difficult because the defaults often aren’t the right thing to return”."
  - id: link-sensing
    prompt: "How can a link seam be used for sensing rather than just separation?"
    options:
      - id: o01ba64
        text: "Read the linker map file"
      - id: o0796d1
        text: "It cannot; link seams only separate"
      - id: record
        text: "Have the stub functions record their calls in a data structure that the test inspects"
        correct: true
    explanation: "“Separation is often a reason to use a link seam. You can do sensing also; it just requires a little more work.” — “we could introduce some additional data structures to record calls”. Start simple: “allow it to get only as complicated as it needs to be”."
  - id: link-enabling
    prompt: "Where is the enabling point of a link seam always found, and what risk follows?"
    options:
      - id: ofe6a7d
        text: "In the test class; risk of slow tests"
      - id: outside
        text: "Outside the program text (build or deployment scripts), which makes link seams hard to notice"
        correct: true
      - id: o2c3532
        text: "In the method body; risk of merge conflicts"
    explanation: "“The enabling point for a link seam is always outside the program text. Sometimes it is in a build or a deployment script. This makes the use of link seams somewhat hard to notice.” The usage tip: “make sure that the difference between test and production environments is obvious.”"
  - id: not-a-seam
    prompt: "`Cell cell = new FormulaCell(this, \"A1\", \"=A2+A3\"); cell.Recalculate();` appears inside one method. Is `cell.Recalculate()` an object seam?"
    options:
      - id: o26a369
        text: "Yes — every virtual call is a seam"
      - id: oab1ee4
        text: "Yes — the enabling point is the `Cell` base class"
      - id: not-seam
        text: "No — there is no enabling point; the class is fixed by `new` in the same method"
        correct: true
    explanation: "“Is the call to Recalculate an object seam? No. There is no enabling point.” — “The class of the cell is decided when the object is created, and we can’t change it without modifying the method.”"
  - id: param-seam
    prompt: "After changing the signature to `buildMartSheet(Cell cell)`, where is the enabling point?"
    options:
      - id: arg-list
        text: "The argument list of `buildMartSheet`"
        correct: true
      - id: o5a082e
        text: "The `Cell` constructor"
      - id: o48c7e9
        text: "The `Recalculate` method body"
    explanation: "“In this example, the enabling point is the argument list of buildMartSheet.”"
  - id: static-seam
    prompt: "The call `Recalculate(cell)` goes to a private static method. Why does Feathers still call it a seam?"
    options:
      - id: o5ed91d
        text: "It is not a seam"
      - id: protected-virtual
        text: "Making the method protected and non-static lets a test subclass override it without editing `buildMartSheet`"
        correct: true
      - id: o8f3719
        text: "Static methods can be replaced at runtime by any test framework"
    explanation: "“Is the call to Recalculate in buildMartSheet a seam? Yes. We don’t have to edit buildMartSheet to change behavior at that call. If we delete the keyword static on Recalculate and make it a protected method instead of a private method, we can subclass and override it during test”."
  - id: three-seams
    prompt: "Which seam types are available at the `PostReceiveError` call in `CAsyncSslRec::Init`?"
    type: multiple
    options:
      - id: object
        text: "Object seam — a virtual member function overridden in a testing subclass"
        correct: true
      - id: o3a39bc
        text: "Reflection seam — swap the method via metadata"
      - id: link
        text: "Link seam — link to a library with a stub"
        correct: true
      - id: preproc
        text: "Preprocessing seam — define a macro under a preprocessor flag"
        correct: true
    explanation: "Feathers lists all three: “we can easily use the link seam there”, “So, we have a preprocessing seam there”, and “so we have an object seam there also.”"
  - id: which-best
    prompt: "Which seam type does Feathers recommend in object-oriented languages, and when should the others be used?"
    options:
      - id: o990778
        text: "Link seams always, because they need no code changes"
      - id: object-best
        text: "Object seams in general; reserve preprocessing and link seams for pervasive dependencies with no better alternative"
        correct: true
      - id: o13e3bf
        text: "Preprocessing seams, because they are most explicit"
    explanation: "“In general, object seams are the best choice in object-oriented languages.” and “I like to reserve preprocessing seams and link seams for cases where dependencies are pervasive and there are no better alternatives.” They are “not as explicit” and tests on them “can be hard to maintain.”"
  - id: csharp-virtual
    prompt: "In C#, methods are non-virtual unless marked `virtual`. A legacy class calls its own non-virtual `SendAlert()` method. What does this mean for the object seam technique shown with `Recalculate`?"
    options:
      - id: oa2dce3
        text: "Nothing — C# overrides non-virtual methods automatically in subclasses"
      - id: od48fde
        text: "Object seams are impossible in C#"
      - id: mark-virtual
        text: "You must make the method `virtual` (and accessible to a subclass) before a testing subclass can override it"
        correct: true
    explanation: "The book's Java example removes `static` and makes the method `protected` so it can be overridden: “If we delete the keyword static on Recalculate and make it a protected method instead of a private method, we can subclass and override it”. In C# you also need `virtual`, because `new` on a non-virtual method only hides it."
---
## Learning objectives

- Identify preprocessing, link and object seams and their enabling points
- Decide which seam type to use and why
- Spot calls that look like seams but are not

## In a nutshell

| Seam | Build step | Enabling point | Recommendation |
|------|------------|----------------|----------------|
| Preprocessing | Before compilation (C/C++) | Preprocessor define | Last resort; obscures code |
| Link | Linking / class loading | Build script, classpath | Last resort; easy to miss |
| Object | Runtime dispatch | Where the object is created or passed | Default choice in OO code |

A call is an object seam only if something outside the method can choose which implementation
runs. A `new` in the same method kills the seam.

## Connections to today

- **C# virtual-by-opt-in.** Unlike Java, C# methods are non-virtual by default and many classes
  are `sealed`. Interfaces and constructor injection are the usual way to create object seams.
- **Conditional compilation.** `#if DEBUG` and custom `DefineConstants` are .NET's preprocessing
  seam. The same warning applies: two programs in one file.
- **Runtime detours.** Microsoft Fakes shims (Visual Studio Enterprise) and similar tools rewrite
  calls at run time, much like a link seam whose enabling point is the test context. They help
  with untouchable statics, but the dependency remains in the design.
- **Dependency injection containers** are enabling points expressed as configuration. Keep the
  test/production difference obvious, as the link-seam usage tip advises.
