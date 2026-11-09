---
title: "Reasoning Forward"
questions:
  - id: invert
    prompt: "When writing characterization tests, how is the reasoning inverted?"
    options:
      - id: oea062d
        text: "Start from the database schema"
      - id: downstream
        text: "Look at a set of objects and figure out what will change downstream if they stop working"
        correct: true
      - id: o9fff71
        text: "Start from the UI and work inward"
    explanation: "“When we are writing characterization tests (186), we invert this process. We look at a set of objects and try to figure out what will change downstream if they stop working.”"
  - id: change-points
    prompt: "What are the change points for the `InMemoryDirectory` change?"
    type: multiple
    options:
      - id: o7e28e0
        text: "`getElement`"
      - id: add
        text: "`addElement` (functionality added)"
        correct: true
      - id: generate
        text: "`generateIndex` (functionality removed)"
        correct: true
    explanation: "“We need to remove functionality from generateIndex and add functionality to addElement. When we’ve identified those as the points of change, we can start to sketch effects.”"
  - id: why-not-addelement
    prompt: "Why isn't `addElement` counted as something affected by the `elements` collection?"
    options:
      - id: behaves-same
        text: "It behaves the same regardless of the collection's contents, so no user of it can be impacted"
        correct: true
      - id: o5bf7ba
        text: "It is never called"
      - id: od4d48f
        text: "It is private"
    explanation: "“we don’t need to count that because addElement behaves the same way, regardless of what we do to the elements collection: No user of addElements can be impacted by anything we do to the elements collection”."
  - id: sense-at
    prompt: "Where can users of `InMemoryDirectory` sense the effects of the change?"
    options:
      - id: oddf3ad
        text: "Through the constructor"
      - id: count-get
        text: "Only through `getElementCount` and `getElement` (plus `getText` on returned elements)"
        correct: true
      - id: o63a2c8
        text: "Only through `generateIndex`"
    explanation: "“The only way that users of the InMemoryDirectory class can sense effects is through the getElementCount and getElement methods.” For index text: “we can call getText on an element returned by getElement.”"
  - id: subclasses
    prompt: "What could invalidate the effect sketch if fields weren't private?"
    options:
      - id: subclass-access
        text: "Subclasses or other code with access could modify the data in ways not in the sketch"
        correct: true
      - id: o5fd7d4
        text: "Garbage collection"
      - id: of80a4a
        text: "Nothing"
    explanation: "“If any data in InMemoryDirectory is public, protected, or packagescoped, a method in a subclass could modify it in ways that we won’t know about.” The tip: “If your class has a superclass or subclasses, there might be other clients that you haven’t considered.”"
  - id: double-index
    prompt: "What existing quirk of `InMemoryDirectory` must characterization tests be careful about?"
    options:
      - id: ocae644
        text: "`getElement` throws on missing names"
      - id: twice
        text: "Calling `generateIndex` twice creates two index elements, the second listing the first"
        correct: true
      - id: o869182
        text: "`addElement` rejects duplicate names"
    explanation: "“If we call generateIndex twice, we end up with two index elements (the second one created actually lists the first one as an element of the directory).” The application only uses it in a constrained way, so tests should reflect that usage."
  - id: first-step
    prompt: "According to the summary of this example, what is the first step in finding places to test?"
    options:
      - id: o83194f
        text: "Measuring coverage"
      - id: o1492ea
        text: "Writing the new feature"
      - id: detect
        text: "Figuring out where change can be detected — what the effects of the change are"
        correct: true
    explanation: "“We need to find places to test, and the first step is figuring out where change can be detected: what the effects of the change are.”"
---
## Learning objectives

- Identify change points and sketch forward to observable outputs
- Include collaborator classes and subclasses in the sketch

## Connections to today

```csharp
[Fact]
public void Generated_index_lists_existing_elements()
{
    var dir = new InMemoryDirectory();
    dir.AddElement(new Element("a"));
    dir.AddElement(new Element("b"));
    dir.GenerateIndex();
    Assert.Equal("a\nb\n", dir.GetElement("index")!.Text);
    Assert.Equal(3, dir.ElementCount);
}
```
