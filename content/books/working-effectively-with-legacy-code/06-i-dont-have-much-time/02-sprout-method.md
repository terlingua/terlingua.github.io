---
title: "Sprout Method"
questions:
  - id: inline-problems
    prompt: "What is wrong with adding the duplicate check directly inside the `postEntries` loop?"
    type: multiple
    options:
      - id: temp-attracts
        text: "The new temporary variable may attract more code into the method"
        correct: true
      - id: no-separation
        text: "No separation between new and old code, so it is hard to know it is right"
        correct: true
      - id: mingling
        text: "It mingles two operations — date posting and duplicate detection"
        correct: true
      - id: od25bcd
        text: "It fails to compile in Java"
    explanation: "“There isn’t any separation between the new code we’ve added and the old code.” “We’re mingling two operations here: date posting and duplicate entry detection.” “Temporaries aren’t necessarily bad, but sometimes they attract new code.”"
  - id: steps-order
    prompt: "What does Feathers like to do *before* even writing the sprouted method?"
    options:
      - id: oc93dc6
        text: "Rename the source method"
      - id: comment-call
        text: "Write down a call to the new method in context and comment it out"
        correct: true
      - id: o50dae9
        text: "Write the full method body without tests"
      - id: ob084ed
        text: "Extract an interface for the class"
    explanation: "“write down a call for a new method that will do the work involved and then comment it out. (I like to do this before I even write the method so that I can get a sense of what the method call will look like in context.)”"
  - id: locals
    prompt: "How does the sprouted method get the source method's local variables?"
    options:
      - id: of448e6
        text: "Via a global variable"
      - id: arguments
        text: "They become arguments to the call"
        correct: true
      - id: oe2a569
        text: "Through new instance fields"
    explanation: "“Determine what local variables you need from the source method, and make them arguments to the call.”"
  - id: when-use
    prompt: "When does Feathers recommend Sprout Method?"
    options:
      - id: distinct-or-untestable
        text: "Whenever the added code is a distinct piece of work, or you can't get tests around the method yet"
        correct: true
      - id: oc7d6be
        text: "Only for performance changes"
      - id: o8ae4ae
        text: "Only when the class is already fully tested"
    explanation: "“I recommend using Sprout Method whenever you can see the code that you are adding as a distinct piece of work or you can’t get tests around a method yet. It is far preferable to adding code inline.”"
  - id: cannot-instantiate
    prompt: "You want to sprout a method, but the class needs eight hard constructor arguments. Which fallbacks does the chapter offer?"
    type: multiple
    options:
      - id: oe85261
        text: "Use reflection to skip the constructor"
      - id: o3cbaa5
        text: "Copy the class into the test project"
      - id: pass-null
        text: "Pass Null for arguments the test doesn't exercise"
        correct: true
      - id: public-static
        text: "Make the sprout a public static method, passing instance variables as arguments"
        correct: true
    explanation: "“One alternative is to use Pass Null (111). When that won’t work, consider making the sprout a public static method. You might have to pass in instance variables of the source class as arguments”."
  - id: static-staging
    prompt: "How does Feathers view static methods created this way?"
    options:
      - id: staging
        text: "As a staging area — statics sharing variables may later become a new class, or move back once the class is under test"
        correct: true
      - id: o524213
        text: "As permanent design, never to be changed"
      - id: o5be8a2
        text: "As a code smell to be deleted immediately"
    explanation: "“I tend to look at static methods on classes as a staging area. Often after you have several statics and you notice that they share some of the same variables, you are able to see that you can make a new class”."
  - id: disadvantage
    prompt: "What is the main disadvantage of Sprout Method?"
    options:
      - id: of80df2
        text: "It makes methods longer than inline code"
      - id: obc0505
        text: "It requires a refactoring tool"
      - id: give-up
        text: "You are giving up on the source method and class for now, leaving the code in limbo"
        correct: true
    explanation: "“you essentially are saying that you are giving up on the source method and its class for the moment.” and “It leaves your code in limbo.”"
  - id: advantage
    prompt: "What are the key advantages?"
    type: multiple
    options:
      - id: oe05bea
        text: "The source method gets fully tested"
      - id: separates
        text: "New code is clearly separated from old code with a clean interface"
        correct: true
      - id: variables
        text: "You see all of the variables affected, making it easier to judge correctness in context"
        correct: true
    explanation: "“you are clearly separating new code from old code.” “You see all of the variables affected, and this can make it easier to determine whether the code is right in context.”"
  - id: grow-to-class
    prompt: "If more and more code ends up working with the unique entries, what evolution does Feathers sketch?"
    options:
      - id: new-class
        text: "Add more small methods, then introduce a class and shift those methods to it"
        correct: true
      - id: o93eeb0
        text: "Convert the list to a database table"
      - id: o58ca4f
        text: "Inline everything back into `postEntries`"
    explanation: "“If we end up with yet more code that needs to work with them, we can introduce a class and shift all of those new methods over to it.”"
---
## Learning objectives

- Apply the six Sprout Method steps
- Choose fallbacks when the source class cannot be instantiated
- Weigh the advantages and disadvantages

## In a nutshell

1. Find the change point.
2. Write a commented-out call to the new method.
3. Pass needed locals as arguments.
4. Capture a return value if needed.
5. Develop the sprout with TDD.
6. Uncomment the call.

## Connections to today

```csharp
public void PostEntries(IReadOnlyList<Entry> entries)
{
    var entriesToAdd = UniqueEntries(entries);
    foreach (var entry in entriesToAdd)
        entry.PostDate();
    _transactionBundle.ListManager.Add(entriesToAdd);
}

internal IReadOnlyList<Entry> UniqueEntries(IEnumerable<Entry> entries) =>
    entries.Where(e => !_transactionBundle.ListManager.HasEntry(e)).ToList();
```

- `internal` plus `InternalsVisibleTo` lets the test project call the sprout without making it
  public API.
- A sprouted `public static` helper in C# is easy to test and is often the seed of a later
  domain service.
