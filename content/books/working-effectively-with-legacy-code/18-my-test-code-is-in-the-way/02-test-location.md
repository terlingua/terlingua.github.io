---
title: "Test Location"
questions:
  - id: main-consideration
    prompt: "What is the main consideration when deciding whether tests live alongside production code?"
    options:
      - id: ofe03ad
        text: "Compiler speed"
      - id: deployment-size
        text: "Size constraints on deployment — e.g. commercial software running on someone else's computer"
        correct: true
      - id: o80e2dc
        text: "Code review policy"
    explanation: "“The main thing to consider is whether there are size constraints on your application’s deployment.”"
  - id: java-package
    prompt: "How can Java keep tests separate yet \"together\"?"
    options:
      - id: span
        text: "A package can span two directories (source and test), and IDEs can show both in one view"
        correct: true
      - id: o45aafa
        text: "By using inner classes"
      - id: ob33a00
        text: "Java requires tests in the same file"
    explanation: "“In Java, a package can span two different directories” and “Some IDEs actually show you classes in those two directories in the same view”."
  - id: tax
    prompt: "What happens if navigating between code and tests requires climbing directory structures?"
    options:
      - id: stop
        text: "It's like paying a tax; people stop writing tests and work goes slower"
        correct: true
      - id: o5b7ddb
        text: "Builds get faster"
      - id: oe0bbcc
        text: "Nothing important"
    explanation: "“If you have to navigate up and down directory structures to go back and forth between your code and its tests, it is like paying a tax as you work. People will just stop writing tests, and the work will go slower.”"
  - id: alternative
    prompt: "What alternative does Feathers mention for keeping tests near code without shipping them?"
    options:
      - id: o9cf58f
        text: "Encrypt the test binaries"
      - id: o223edf
        text: "Obfuscate the tests"
      - id: build-strip
        text: "Keep them together and use scripts or build settings to remove test code from the deployment"
        correct: true
    explanation: "“An alternative is to keep the production code and the test code in the same location but to use scripts or build settings to remove the test code from the deployment.”"
  - id: aesthetic
    prompt: "What warning does Feathers give about separating tests for aesthetic reasons?"
    options:
      - id: o4a4f14
        text: "Separation is always required"
      - id: good-reason
        text: "Make sure there's a good reason; aesthetic separation often makes navigation painful, and co-location soon feels normal"
        correct: true
      - id: o12a0e7
        text: "Aesthetics is the best reason"
    explanation: "“Above all, if you choose to separate test and production code, make sure it is for a good reason. Quite often teams separate the code for aesthetic reasons” and “After a period of time working that way, it just feels normal.”"
---
## Learning objectives

- Weigh deployment constraints against navigation ergonomics

## Connections to today

In .NET, separate test projects are the norm and `dotnet publish` never ships them, so deployment
size is rarely the driver. Navigation is the remaining concern: mirror folder structures, use
"Go to test" IDE commands, and keep test projects in the same solution.
