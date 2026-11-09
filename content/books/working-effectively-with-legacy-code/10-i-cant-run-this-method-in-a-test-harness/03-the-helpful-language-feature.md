---
title: "The Case of the \"Helpful\" Language Feature"
questions:
  - id: obstacles
    prompt: "Why can't the test simply create `HttpPostedFile` and `HttpFileCollection` objects?"
    type: multiple
    options:
      - id: sealed
        text: "They are sealed, so they can't be subclassed"
        correct: true
      - id: no-ctor
        text: "No public constructors"
        correct: true
      - id: o112585
        text: "They are internal to the test assembly"
    explanation: "“First of all, the HttpPostedFile class doesn’t have a public constructor. Second, the class is sealed.” HttpFileCollection has “the same problems: no public constructors and no way to created derived classes.”"
  - id: why-sealed
    prompt: "Why do language designers provide `sealed`/`final`?"
    options:
      - id: of7138b
        text: "Performance only"
      - id: security
        text: "Security — so malicious subclasses of sensitive classes can't be passed around"
        correct: true
      - id: o1bb834
        text: "To make testing harder deliberately"
    explanation: "“If just anyone could create a subclass of HttpPostedFile or even a class such as String, they could write some malicious code and pass it around in code that uses those classes.”"
  - id: only-technique
    prompt: "Why is Adapt Parameter the only applicable technique for the collection parameter?"
    options:
      - id: ob5e986
        text: "The method is static"
      - id: oc352ab
        text: "Adapt Parameter is always best"
      - id: not-ours
        text: "The classes are library classes outside our control, so Extract Interface/Implementer can't be applied to them"
        correct: true
    explanation: "“We can’t use Extract Interface (362) or Extract Implementer (356); the HttpPostedFile and HttpFileCollection classes aren’t under our control”. “The only technique that we can use here is Adapt Parameter (326).”"
  - id: lucky
    prompt: "What made the collection problem easy?"
    options:
      - id: o0b225d
        text: "Reflection could create it"
      - id: iterate
        text: "The code only iterates the collection, and its unsealed superclass `NameObjectCollectionBase` can be subclassed"
        correct: true
      - id: o74e978
        text: "The collection had an interface already"
    explanation: "“We’re lucky, in this case, because the only thing that we do to the collection is iterate over it. Fortunately, the sealed HttpFileCollection class that our code uses has an unsealed superclass named NameObjectCollectionBase.”"
  - id: skin-wrap
    prompt: "How is the `HttpPostedFile` problem solved?"
    options:
      - id: o56274f
        text: "Subclassing `HttpPostedFile`"
      - id: wrapper
        text: "Skin and Wrap the API — an `IHttpPostedFile` interface, a production `HttpPostedFileWrapper` and a test `FakeHttpPostedFile`"
        correct: true
      - id: o2af5f0
        text: "Link Substitution"
    explanation: "“We can use Skin and Wrap the API (205) to get some separation between us and HttpPostedFile class. To do that, we extract an interface (IHttpPostedFile) and write a wrapper (HttpPostedFileWrapper)”."
  - id: real-fault
    prompt: "Where does Feathers place the real fault?"
    options:
      - id: o1c1b10
        text: "With language designers for adding `sealed`"
      - id: o48e2af
        text: "With test framework authors"
      - id: our-dependency
        text: "With us, for depending directly on libraries out of our control"
        correct: true
    explanation: "“But the real fault lies with us. When we depend directly on libraries that are out of our control, we are just asking for trouble.”"
  - id: price
    prompt: "What runtime cost does the wrapping approach add to production code?"
    options:
      - id: o05b6b3
        text: "A database round-trip per file"
      - id: o6adf97
        text: "None"
      - id: iterate-wrap
        text: "Iterating the original collection, wrapping each file and adding it to a new collection"
        correct: true
    explanation: "“we have to iterate the original HttpFileCollection in the production code, wrap each HttpPostedFile that it contains, and then add it to a new collection that we pass to the getKSRStreams method. That’s the price of security.”"
  - id: advice
    prompt: "What practical advice closes this case?"
    type: multiple
    options:
      - id: o315d93
        text: "Never use library classes"
      - id: isolate
        text: "Isolate library classes that use them behind wrappers for wiggle room"
        correct: true
      - id: sparingly
        text: "Use `sealed` and `final` sparingly"
        correct: true
    explanation: "“it is good to use mechanisms such as sealed and final sparingly. And when we need to use library classes that use them, it’s a good idea to isolate them behind some wrapper so that we have some wiggle room”."
---
## Learning objectives

- Work around sealed library types with Adapt Parameter and Skin and Wrap the API
- Explain why direct library dependencies are the real problem

## Connections to today

- Microsoft later shipped exactly this wrapper idea: `System.Web.Abstractions` added
  `HttpPostedFileBase` and an `HttpPostedFileWrapper` for ASP.NET MVC testability.
- ASP.NET Core went further: uploaded files arrive as `IFormFile`, an interface you can fake
  directly. Library design caught up with Feathers' complaint.
- Many .NET analyzers now recommend sealing classes for performance. That is fine for your own
  leaf types; just keep library-facing code behind your own abstractions.
