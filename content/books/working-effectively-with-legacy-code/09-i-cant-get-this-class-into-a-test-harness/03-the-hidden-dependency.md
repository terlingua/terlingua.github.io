---
title: "The Case of the Hidden Dependency"
questions:
  - id: what-hidden
    prompt: "What makes `mailing_list_dispatcher`'s dependency \"hidden\"?"
    options:
      - id: ctor-new
        text: "The constructor itself creates a `mail_service` with `new`, so callers can't see or replace it"
        correct: true
      - id: o96d8d7
        text: "The class has no constructor"
      - id: o7a1fa4
        text: "The dependency is in a private header"
    explanation: "“It allocates a mail_service object using new in the constructor initializer list.” and “The fundamental problem here is that the dependency on mail_service is hidden in the mailing_list_dispatcher constructor.”"
  - id: why-not-real
    prompt: "Why not just construct the real dispatcher in tests?"
    type: multiple
    options:
      - id: real-mail
        text: "Calling `send_message` would really send mail to people"
        correct: true
      - id: o211b1f
        text: "The constructor is private"
      - id: link
        text: "You'd have to link the mail libraries and configure the mail system"
        correct: true
    explanation: "“we’ll have to link to the mail libraries and configure the mail system to handle registrations. And if we use the send_message function in our tests, we’ll really be sending mail to people.”"
  - id: parameterize
    prompt: "What does Parameterize Constructor do?"
    options:
      - id: oc43c6c
        text: "Makes the constructor generic"
      - id: externalize
        text: "Externalizes a constructor dependency by passing it into the constructor"
        correct: true
      - id: o0907f5
        text: "Adds default values to every field"
    explanation: "“With this technique, we externalize a dependency that we have in a constructor by passing it into the constructor.”"
  - id: clients
    prompt: "A common objection is that all clients must change to pass the new parameter. How does Feathers avoid that?"
    options:
      - id: keep-old
        text: "Keep a constructor with the original signature that creates the real dependency and delegates"
        correct: true
      - id: oa64d01
        text: "Global search and replace"
      - id: o64009c
        text: "Make the parameter optional via reflection"
    explanation: "“Now we can supply a constructor that has the original signature. Tests can call the constructor parameterized by mail_service, and clients can call this one. They don’t need to know that anything has changed.”"
  - id: initialize-safe
    prompt: "Why is extracting the constructor body into `initialize` reasonably safe without tests?"
    options:
      - id: o7961e1
        text: "Compilers verify extracted methods"
      - id: ocdffaf
        text: "It doesn't change any code"
      - id: preserve
        text: "You can Preserve Signatures while doing it"
        correct: true
    explanation: "“Unlike most method extractions, this one is pretty safe to attempt without tests because we can Preserve Signatures (312) as we do it.”"
  - id: csharp-chaining
    prompt: "Why is this refactoring even easier in C# and Java than C++?"
    options:
      - id: o4fe502
        text: "They have no headers"
      - id: chain
        text: "Constructors can call other constructors (`: this(new MailService())`)"
        correct: true
      - id: oc2104d
        text: "They have garbage collection"
    explanation: "“This refactoring is even easier in languages such as C# and Java because we can call constructors from other constructors in those languages.”"
  - id: when-easy
    prompt: "When is Parameterize Constructor a very easy technique to apply?"
    options:
      - id: o590825
        text: "When the object has many construction dependencies of its own"
      - id: no-ctor-deps
        text: "When the object created in the constructor has no construction dependencies itself"
        correct: true
      - id: o75e730
        text: "When the class is a singleton"
    explanation: "“When an object is created in a constructor and it doesn’t have any construction dependencies itself, Parameterize Constructor is a very easy technique to apply.”"
---
## Learning objectives

- Recognise a hidden constructor dependency
- Apply Parameterize Constructor without breaking existing clients

## Connections to today

```csharp
public class MailingListDispatcher
{
    private readonly IMailService _service;

    public MailingListDispatcher() : this(new MailService()) { }

    public MailingListDispatcher(IMailService service)
    {
        _service = service;
    }
}
```

This "poor man's DI" constructor pair is a legitimate stepping stone. Once a composition root
exists, the parameterless constructor can be deleted.
