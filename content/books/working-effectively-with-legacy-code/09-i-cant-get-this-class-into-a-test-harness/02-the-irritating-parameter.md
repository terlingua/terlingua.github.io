---
title: "The Case of the Irritating Parameter"
questions:
  - id: just-try
    prompt: "What is the best way to find out whether a class will be hard to instantiate in a test?"
    options:
      - id: o5e4a65
        text: "Ask its original author"
      - id: try
        text: "Write a test that tries to construct it and let the compiler tell you what you need"
        correct: true
      - id: oc16b35
        text: "Draw its dependency graph"
    explanation: "“The best way to see if you will have trouble instantiating a class in a test harness is to just try to do it. Write a test case and attempt to create an object in it. The compiler will tell you what you need to make it really work.”"
  - id: construction-test
    prompt: "What is distinctive about a \"construction test\"?"
    options:
      - id: no-assert
        text: "It usually has no assertion; it is later deleted or renamed into a real test"
        correct: true
      - id: oc48dee
        text: "It asserts every field value"
      - id: o696829
        text: "It runs only in CI"
    explanation: "“When I write one, I usually don’t put an assertion in it. I just try to create the object. Later, when I’m finally able to construct an object in the test harness, I usually get rid of the test or rename it”."
  - id: which-irritating
    prompt: "`CreditValidator` needs an `RGHConnection` and a `CreditMaster`. Which is the irritating parameter and why?"
    options:
      - id: rgh
        text: "`RGHConnection` — connecting to the server is slow and the server isn't always up"
        correct: true
      - id: o5fa9a7
        text: "Both equally"
      - id: o30e329
        text: "`CreditMaster` — it reads a file"
    explanation: "“establishing RGHConnections to the server in a test is not a good idea. It takes a long time, and the server isn’t always up. On the other hand, the CreditMaster is not really a problem.” It loads quickly and “the file is read-only”."
  - id: fake-rules
    prompt: "`FakeConnection` has empty methods, returns null and exposes a public field. Is that acceptable?"
    options:
      - id: o5f3241
        text: "Only if marked deprecated"
      - id: different-rules
        text: "Yes — rules differ for test-enabling classes, though test code should still be clean and easy to change"
        correct: true
      - id: oa9d736
        text: "No — test code must follow production rules exactly"
    explanation: "“The rules are different for classes that we use to make testing possible.” The sidebar adds: “Test code doesn’t have to live up to the same standards as production code.” but “test code should be clean. It should be easy to understand and change.”"
  - id: duplication
    prompt: "What should be done about the first three lines duplicated across `testNoSuccess` and `testAllPassed100Percent`?"
    options:
      - id: setup
        text: "Extract them into the test class's `setUp()`"
        correct: true
      - id: ocba256
        text: "Merge both tests into one"
      - id: o11a375
        text: "Nothing; test duplication never matters"
    explanation: "“The first three lines are duplicated. They should be extracted and placed in a common place, the setUp() method for this test class.”"
  - id: pass-null-why
    prompt: "Why is Pass Null safe in Java and C# tests?"
    options:
      - id: o60cf7e
        text: "The compiler forbids null dereferences"
      - id: oa6f39c
        text: "Null is replaced by a default object"
      - id: exception
        text: "If the parameter is used, the runtime throws and the harness catches it, so you find out quickly"
        correct: true
    explanation: "“If the parameter is used in the course of your test execution, the code will throw an exception and the test harness will catch the exception.”"
  - id: pass-null-cpp
    prompt: "Why is Pass Null risky in C and C++?"
    options:
      - id: silent
        text: "Unless the runtime detects null pointers, tests may crash mysteriously or be silently wrong and corrupt memory"
        correct: true
      - id: oc83e35
        text: "It is slower"
      - id: od3f255
        text: "Those languages have no null"
    explanation: "“If it doesn’t, you’ll just end up with tests that will crash mysteriously, if you are lucky. If you are unlucky, your tests will just be silently and hopelessly wrong. They will corrupt memory as they run, and you’ll never know.”"
  - id: production-null
    prompt: "What is Feathers' rule about null in *production* code?"
    options:
      - id: dont
        text: "Don't pass null unless you have no choice; consider a different protocol such as the Null Object Pattern"
        correct: true
      - id: ob95f45
        text: "Pass null freely; tests will catch it"
      - id: o82eafa
        text: "Always return null for missing data"
    explanation: "“Don’t pass null in production code unless you have no other choice.” and “Consider using the Null Object Pattern instead.”"
  - id: null-object-bug
    prompt: "With `NullEmployee` returned for unknown IDs, a loop does `e.pay(); employeesPaid++;`. What's the problem?"
    options:
      - id: o5fa1b6
        text: "`pay()` throws on null employees"
      - id: count-wrong
        text: "The count includes null employees, so it is wrong — null objects fit only when the client needn't care whether the operation succeeded"
        correct: true
      - id: o0b98f7
        text: "There is no problem"
    explanation: "“If any of the returned employees are null employees, the count will be wrong.” and “Null objects are useful specifically when a client doesn’t have to care whether an operation is successful.”"
  - id: third-way
    prompt: "Besides Pass Null and Extract Interface, what other approach does the chapter suggest for `RGHConnection`, and what's the caveat?"
    options:
      - id: o9e0e68
        text: "Make `RGHConnection` a singleton"
      - id: subclass
        text: "Subclass and Override Method on `connect()` if the constructor uses it — making sure you don't alter the behaviour you want to test"
        correct: true
      - id: o5a0995
        text: "Use a link seam"
    explanation: "“If the constructor of RGHConnection uses its connect method to form a connection, we could break the dependency by overriding connect() in a testing subclass.” — “but we have to be sure that we aren’t altering the behavior we want to test when we use it.”"
  - id: reaction
    prompt: "What does it suggest if your reaction to `new CreditValidator(connection, null, \"a\")` is \"fine, we do that all the time\"?"
    options:
      - id: nasty
        text: "You probably have a pretty nasty system with null checks and conditionals everywhere"
        correct: true
      - id: of71bf7
        text: "Your tests are too strict"
      - id: o599070
        text: "Your system is well designed"
    explanation: "“chances are, you’ve got a pretty nasty system on your hands. You probably have checks for null all over the place and a lot of conditional code”."
---
## Learning objectives

- Use a construction test to discover dependencies
- Choose among Extract Interface, Pass Null and Subclass and Override Method
- Keep Pass Null out of production code

## Connections to today

- C# nullable reference types (`<Nullable>enable</Nullable>`) flag `null!` in tests — that's
  fine; the warning is about production code, which matches Feathers' rule.
- `null!` or a `default!` argument is the C# spelling of Pass Null. A `NullLogger<T>.Instance`
  from `Microsoft.Extensions.Logging.Abstractions` is a ready-made Null Object.
