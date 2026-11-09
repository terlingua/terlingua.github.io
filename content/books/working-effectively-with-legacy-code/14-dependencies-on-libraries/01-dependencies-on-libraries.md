---
title: "Dependencies on Libraries Are Killing Me"
questions:
  - id: royalties
    prompt: "What cautionary tale illustrates over-reliance on a library?"
    options:
      - id: o8c5bd6
        text: "A library was deprecated"
      - id: o0cf147
        text: "A library had a security bug"
      - id: vendor
        text: "A vendor raised royalties so high the app couldn't make money, and switching would have amounted to a rewrite"
        correct: true
    explanation: "“a vendor raised royalties so high that the application couldn’t make money in the marketplace. The team couldn’t easily use another vendor’s library because separating out the calls to the original vendor’s code would’ve amounted to a rewrite.”"
  - id: prophecy
    prompt: "Why avoid littering direct calls to library classes even if you'll \"never change them\"?"
    options:
      - id: ob94931
        text: "Libraries are always slower"
      - id: o0d02e4
        text: "Licences forbid it"
      - id: self-fulfilling
        text: "That belief can become a self-fulfilling prophecy"
        correct: true
    explanation: "“Avoid littering direct calls to library classes in your code. You might think that you’ll never change them, but that can become a self-fulfilling prophecy.”"
  - id: seam
    prompt: "What is every hard-coded use of a library class, from a testing perspective?"
    options:
      - id: o3e62f9
        text: "An enabling point"
      - id: o3bedcd
        text: "A pinch point"
      - id: missed-seam
        text: "A place where you could have had a seam"
        correct: true
    explanation: "“Every hard-coded use of a library class is a place where you could have had a seam.”"
  - id: thin-wrapper
    prompt: "When library classes are final/sealed or have non-virtual key methods, what's often the best option?"
    options:
      - id: o7af488
        text: "Use reflection to mock them"
      - id: wrapper
        text: "Write a thin wrapper over the classes you need to separate"
        correct: true
      - id: o5661b1
        text: "Fork the library"
    explanation: "“In these cases, sometimes the best thing you can do is write a thin wrapper over the classes that you need to separate out.”"
  - id: designers-mistake
    prompt: "What mistake does Feathers attribute to library designers who enforce design constraints with language features?"
    options:
      - id: of6f847
        text: "Using interfaces"
      - id: o54fc69
        text: "Making libraries too small"
      - id: production-and-test
        text: "Forgetting that good code runs in both production and test environments"
        correct: true
    explanation: "“Library designers who use language features to enforce design constraints are often making a mistake. They forget that good code runs in production and test environments.”"
  - id: once
    prompt: "What is the \"once dilemma\"?"
    options:
      - id: o739799
        text: "Tests that pass only once"
      - id: o2ede34
        text: "Code that can only run once"
      - id: single-instance
        text: "A library assumes only one instance of a class exists, making fakes hard — wrapping may be the only choice"
        correct: true
    explanation: "“If library assumes that there is going to be only one instance of a class in a system, it can make the use of fake objects difficult.” — “Sometimes wrapping the singleton is the only choice available to you.”"
  - id: override-dilemma
    prompt: "What is the \"restricted override dilemma\"?"
    options:
      - id: o65f5be
        text: "Overriding is forbidden in C#"
      - id: o629961
        text: "Too many overrides cause LSP violations"
      - id: non-virtual
        text: "Making methods non-virtual has design value but makes it hard to introduce sensing and separation"
        correct: true
    explanation: "“From a design perspective, there is some value in making some methods non-virtual.” But “it is hard to deny that this practice makes it hard to introduce sensing and separation in code bases.”"
  - id: convention
    prompt: "What alternative does Feathers suggest to restrictive language features?"
    options:
      - id: pretend
        text: "A coding convention — pretend a public method is non-virtual in production, and override it selectively in tests"
        correct: true
      - id: od79575
        text: "Use only static methods"
      - id: o287a9d
        text: "Avoid classes entirely"
    explanation: "“You can do very well just pretending that a public method is non-virtual in production code. If you do that, you can override it selectively in test and get the best of both worlds.” and “Sometimes using a coding convention is just as good as using a restrictive language feature.”"
  - id: csharp-default
    prompt: "Which category in Feathers' description does C# fall into?"
    options:
      - id: o903662
        text: "Virtual by default, can be made non-virtual"
      - id: oe4bbcd
        text: "All methods are virtual"
      - id: explicit
        text: "You have to explicitly make methods virtual"
        correct: true
    explanation: "Feathers describes three kinds: “In some OO languages, all methods are virtual. In others, they are virtual by default, but they can be made non-virtual. In others, you have to explicitly make them virtual.” C# (like C++) is in the last group; Java is in the second."
---
## Learning objectives

- Recognise the cost of scattering library calls
- Explain the once and restricted-override dilemmas
- Choose wrappers and conventions to keep seams available

## Connections to today

- Cloud SDKs (Azure, AWS), payment providers and LLM APIs are today's high-risk libraries. A thin
  internal interface (`IBlobStore`, `IPaymentGateway`, `IChatModel`) keeps vendor churn and pricing
  changes from becoming rewrites.
- Many modern SDKs now ship mockable abstractions (for example, Azure SDK clients have virtual
  methods and protected constructors for mocking). Check before wrapping — but wrapping still buys
  you vocabulary and isolation.
