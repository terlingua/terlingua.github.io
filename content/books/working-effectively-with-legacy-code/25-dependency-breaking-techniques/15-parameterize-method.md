---
title: "Parameterize Method"
questions:
  - id: what
    prompt: "What does Parameterize Method do?"
    options:
      - id: oc66720
        text: "Converts a method to a property"
      - id: odfd6b9
        text: "Adds generic type parameters"
      - id: pass-in
        text: "Passes an object a method creates internally in from the outside, keeping a forwarding method with the original signature"
        correct: true
    explanation: "“You have a method that creates an object internally, and you want to replace the object to sense or separate. Often the easiest way to do this is to pass the object from the outside.” Then: “We can use a little forwarding method that keeps the original signature intact”."
  - id: naming
    prompt: "Overloading `run()` and `run(TestResult*)` saves work, but what alternative naming does Feathers suggest to avoid confusion?"
    options:
      - id: type-in-name
        text: "Use the parameter type in the new name, e.g. `runWithTestResult`"
        correct: true
      - id: oca3845
        text: "Use `run2`"
      - id: o5b7d42
        text: "Prefix with `Test`"
    explanation: "“An alternative is to use the type of the parameter in the name of the new method. For instance, in this case, we could keep run() as the name of the original method but call the new method runWithTestResult(TestResult).”"
  - id: alternative
    prompt: "If exposing the new type at the interface would cause dependency problems, what does Feathers consider instead?"
    options:
      - id: o2af5f0
        text: "Link Substitution"
      - id: factory
        text: "Extract and Override Factory Method"
        correct: true
      - id: o49c6be
        text: "Text Redefinition"
    explanation: "“Parameterize Method can allow clients to become dependent on new types that were used in the class before but were not present at the interface. If I think that this will become an issue, I consider Extract and Override Factory Method (350) instead.”"
  - id: steps
    prompt: "What are the steps?"
    options:
      - id: o73b2fe
        text: "Make the method static → pass all fields"
      - id: ob8154e
        text: "Delete the method → add a new one → update callers"
      - id: steps-opt
        text: "Copy the method → add a parameter replacing the creation → make the copy's body call the parameterized method with the original creation expression"
        correct: true
    explanation: "“1. Identify the method that you want to replace and make a copy of it. 2. Add a parameter to the method for the object whose creation you are going to replace.” “3. Delete the body of the copied method and make a call to the parameterized method, using the object creation expression for the original object.”"
---
## Learning objectives

- Inject a method-local dependency while preserving the old call

## Connections to today

`Process(CancellationToken ct = default)` and `Save(IClock? clock = null)` are common C# forms.
Method injection suits per-call dependencies; constructor injection suits per-object ones.
