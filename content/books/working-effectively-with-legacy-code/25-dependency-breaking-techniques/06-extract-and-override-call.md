---
title: "Extract and Override Call"
questions:
  - id: what
    prompt: "What does Extract and Override Call do?"
    options:
      - id: o347703
        text: "Moves the call to another class"
      - id: o8c300f
        text: "Replaces all calls in the program with a fake"
      - id: wrap-call
        text: "Extracts a single problematic call (e.g. a static) into a protected method on the class, then overrides it in a testing subclass"
        correct: true
    explanation: "“One option is to extract the call to a new method and override it in a testing subclass. This is known as Extract and Override Call.”"
  - id: purposes
    prompt: "What can breaking a dependency on a single call achieve?"
    type: multiple
    options:
      - id: side-effects
        text: "Prevent odd side effects during tests"
        correct: true
      - id: occ9d05
        text: "Improve production performance"
      - id: sense
        text: "Sense values passed to the call"
        correct: true
    explanation: "“If we can break the dependency on a method call, we can prevent odd side effects in our testing or sense values that are passed to the call.”"
  - id: frequency
    prompt: "How often does Feathers use it, and when does he prefer something else?"
    options:
      - id: o23a434
        text: "Rarely; it's risky"
      - id: often
        text: "Very often — ideal for globals and statics — unless there are many calls against the same global, then Replace Global Reference with Getter"
        correct: true
      - id: o64c98c
        text: "Only in C++"
    explanation: "“Extract and Override Call is a very useful refactoring; I use it very often. It is an ideal way to break dependencies on global variables and static methods.” “If there are, I often use Replace Global Reference with Getter (399) instead.”"
  - id: steps
    prompt: "What's the essential manual procedure?"
    options:
      - id: oc477ef
        text: "Use reflection to swap it"
      - id: o6365f2
        text: "Rewrite the call from memory"
      - id: copy-sig
        text: "Copy the called method's signature (Preserve Signatures), create a method with it on the current class, move the call into it and call the new method"
        correct: true
    explanation: "“1. Identify the call that you want to extract. Find the declaration of its method. Copy its method signature” “2. Create a new method on the current class.” “3. Copy the call to the new method and replace the call with a call to the new method.”"
  - id: tool
    prompt: "With an automated refactoring tool, what is Extract and Override Call?"
    options:
      - id: o4f41f6
        text: "Still very risky"
      - id: trivial
        text: "Trivial — it's just Extract Method followed by an override"
        correct: true
      - id: o011d7b
        text: "Impossible"
    explanation: "“If you have an automated refactoring tool, Extract and Override Call is trivial. You can do it using the Extract Method (415) refactoring.”"
---
## Learning objectives

- Isolate a single static or global call behind an overridable method

## Connections to today

```csharp
public class PageLayout
{
    protected void RebindStyles() => _styles = FormStyles(_template, _id);
    protected virtual IList<Style> FormStyles(StyleTemplate template, int id) =>
        StyleMaster.FormStyles(template, id);
}
```

Remember `virtual` in C#. If the class is `sealed`, use a delegate parameter instead.
