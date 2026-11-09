---
title: "Parameterize Constructor"
questions:
  - id: what
    prompt: "What does Parameterize Constructor do?"
    options:
      - id: externalize
        text: "Externalizes an object created in a constructor so callers pass it in"
        correct: true
      - id: o52b4ec
        text: "Removes all constructors"
      - id: o2bc6df
        text: "Adds generics to a class"
    explanation: "“If you are creating an object in a constructor, often the easiest way to replace it is to externalize its creation, create the object outside the class, and make clients pass it into the constructor as a parameter.”"
  - id: clients
    prompt: "Does Parameterize Constructor force all clients to change?"
    options:
      - id: keep-original
        text: "No — keep a constructor with the original signature that creates the real object and chains to the new one"
        correct: true
      - id: o24416c
        text: "Only in Java"
      - id: oa05c8b
        text: "Yes, always"
    explanation: "“One reason people don’t often think of this technique is that they assume that it forces all clients to pass an additional argument. However, you can write a constructor that keeps the original signature around”."
  - id: order
    prompt: "What is the step order in the MailChecker example?"
    options:
      - id: o5d107c
        text: "Add a setter → call it after construction"
      - id: oe0c0c8
        text: "Delete the old constructor → add the new one → fix callers"
      - id: steps
        text: "Copy the constructor → add the parameter and assign it (removing `new`) → make the original constructor call the new one with `new MailReceiver()`"
        correct: true
    explanation: "“We make a copy of the constructor”, “Then we add a parameter to it for the MailReceiver”, “Next we assign that parameter to the instance variable, getting rid of the new expression.” Finally “we go back to the original constructor and remove its body, replacing it with a call to the new constructor.”"
  - id: downside
    prompt: "What is the one downside?"
    options:
      - id: of7c8dd
        text: "It slows object creation"
      - id: deps
        text: "It opens the door to further dependencies on the parameter's class across the system — usually a small concern"
        correct: true
      - id: oc2d94b
        text: "It breaks serialisation"
    explanation: "“When we add a new parameter to a constructor, we are opening the door to further dependencies on the parameter’s class.” “However, in general, that is a rather small concern.”"
  - id: default-args
    prompt: "Why does Feathers rarely use default arguments for this in C++?"
    options:
      - id: header
        text: "The header must then include the parameter type's header instead of using a forward declaration"
        correct: true
      - id: o6b2e21
        text: "Default arguments don't compile"
      - id: of01a4f
        text: "They're slower"
    explanation: "“The header file containing this class declaration has to include the header for EquipmentDispatcher. If it wasn’t for the constructor call, we might have been able to use a forward declaration”."
  - id: no-chaining
    prompt: "What if your language can't call one constructor from another?"
    options:
      - id: o986828
        text: "Use a static factory only"
      - id: extract
        text: "Extract duplication among the constructors into a new method"
        correct: true
      - id: o87c404
        text: "The technique can't be used"
    explanation: "“If you can’t call a constructor from another constructor in your language, you may have to extract any duplication among the constructors to a new method.”"
---
## Learning objectives

- Parameterize a constructor without changing existing clients

## Connections to today

C# optional parameters (`IMailReceiver? receiver = null` then `receiver ?? new MailReceiver()`)
avoid the C++ header problem, but a chained constructor stays clearer. Once a composition root
exists, remove the parameterless constructor.
