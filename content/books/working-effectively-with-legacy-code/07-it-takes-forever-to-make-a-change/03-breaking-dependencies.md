---
title: "Breaking Dependencies and Build Dependencies"
questions:
  - id: first-step
    prompt: "In object-oriented code, what is often the first step toward faster feedback on a class?"
    options:
      - id: instantiate
        text: "Try to instantiate the classes you need in a test harness"
        correct: true
      - id: oc2879b
        text: "Split the solution into microservices"
      - id: oe12e43
        text: "Buy faster build machines"
    explanation: "“In object-oriented code, often the first step is to attempt to instantiate the classes that we need in a test harness.”"
  - id: find-deps
    prompt: "How do you discover which dependencies get in the way of building a cluster of classes quickly?"
    options:
      - id: oc463d9
        text: "Draw a full dependency graph first"
      - id: o1dcf52
        text: "Read the build logs"
      - id: try-harness
        text: "Just attempt to use the classes in a test harness; nearly every problem you hit is a dependency to break"
        correct: true
    explanation: "“Generally, that is rather easy: You just attempt to use the classes in a test harness. Nearly every problem that you run into will be the result of some dependency that you should break.”"
  - id: why-no-recompile
    prompt: "After Extract Implementer on `ConsultantSchedulerDB`, why doesn't `AddOpportunityFormHandler` recompile when `ConsultantSchedulerDBImpl` changes?"
    options:
      - id: o0a920e
        text: "The compiler caches everything"
      - id: interface
        text: "It depends only on the interface, which doesn't change unless you force it to"
        correct: true
      - id: o9c2b7e
        text: "The implementation moved to another language"
    explanation: "“it doesn’t directly depend on the code in ConsultantSchedulerDBImpl anymore.” — “unless we do something that forces us to change the ConsultantSchedulerDB interface, we won’t have to rebuild the AddOpportunityFormHandler class.”"
  - id: firewall
    prompt: "What does Feathers call the effect of extracting implementers for both collaborators?"
    options:
      - id: compilation-firewall
        text: "A \"compilation firewall\""
        correct: true
      - id: o3bedcd
        text: "A pinch point"
      - id: o04e829
        text: "An anti-corruption layer"
    explanation: "“In a way, we’ve put a \"compilation firewall\" in the code.”"
  - id: build-time-paradox
    prompt: "After splitting a cluster into new packages with interfaces, a full rebuild takes slightly longer. Has the refactoring failed?"
    options:
      - id: o92450b
        text: "Yes — builds must get faster in every measure"
      - id: average
        text: "No — total rebuild cost can rise while the average incremental build time drops dramatically"
        correct: true
      - id: o18445b
        text: "Yes — revert the packages"
    explanation: "“the overall cost of a rebuild of the entire system grows, but the average time for a build can decrease.” and “But the average time for a make, a build based on what needs to be recompiled, can go down dramatically.”"
  - id: dip
    prompt: "Why is it better to depend on interfaces or abstract classes than on concrete classes (Dependency Inversion Principle)?"
    options:
      - id: less-volatile
        text: "Interfaces change less often than the code behind them, so depending on them minimises massive recompilation"
        correct: true
      - id: o3380d6
        text: "Interfaces execute faster"
      - id: o791524
        text: "Concrete classes can't be tested"
    explanation: "“interfaces typically change far less often than the code behind them.” — “When you depend on less volatile things, you minimize the chance that particular changes will trigger massive recompilation.”"
  - id: other-half
    prompt: "Shielding `AddOpportunityFormHandler` from its dependencies is only half the issue. What is the other half?"
    options:
      - id: o5a58d5
        text: "Making its tests run in parallel"
      - id: dependents
        text: "Shielding code that depends on `AddOpportunityFormHandler`, via Extract Interface or Extract Implementer on it"
        correct: true
      - id: oa40489
        text: "Moving it into the database package"
    explanation: "“We can also make builds faster for code that depends on AddOpportunityFormHandler.” — “We can break that dependency also by using Extract Interface (362) or Extract Implementer (356) on AddOpportunityForm Handler.”"
  - id: cost
    prompt: "What price does Feathers accept for more interfaces and packages?"
    options:
      - id: conceptual
        text: "Some conceptual overhead — things can take a little longer to find, but are easy to work with once found"
        correct: true
      - id: oe8488a
        text: "Loss of type safety"
      - id: o5587fb
        text: "Runtime performance"
    explanation: "“There is some conceptual overhead in having more interfaces and packages. Is that a fair price to pay compared to the alternative? Yes.”"
  - id: once
    prompt: "Why is the effort of getting a cluster compiling separately worth it even if painful?"
    options:
      - id: ofc0379
        text: "It removes the need for tests"
      - id: od1ccc8
        text: "It is required by most compilers"
      - id: once-forever
        text: "You do it only once for that set of classes and reap the benefits forever"
        correct: true
    explanation: "“you have to do it only once for that set of classes; afterward, you get to reap the benefits forever.”"
  - id: cpp-implementer
    prompt: "In C++, why is Extract Implementer a little easier than Extract Interface for this purpose?"
    options:
      - id: o4c7cd5
        text: "It needs no new files"
      - id: oa53382
        text: "C++ has no interfaces at all"
      - id: names
        text: "You don't have to rename references everywhere, but you do have to change the places that create instances"
        correct: true
    explanation: "“In C++, Extract Implementer (356) is a little easier than Extract Interface (362). You don’t have to change the names of references all over the place, but you do have to change the places that create instances of the old class”."
---
## Learning objectives

- Use interfaces and packages to cut recompilation
- Explain why average build time matters more than full rebuild time
- Apply the Dependency Inversion Principle to build speed

## In a nutshell

Get the cluster into a harness, then extract interfaces at its boundaries — for what it uses and
for what uses it. Move the cluster into its own package. Full builds may get slightly slower;
everyday builds get dramatically faster.

## Connections to today

- In .NET, project boundaries are the compilation unit. A test project that references only an
  `OpportunityProcessing` project and an abstractions project builds in seconds; one that
  references the web host builds everything.
- Reference assemblies let MSBuild skip rebuilding dependents when only a referenced project's
  implementation (not its public surface) changes — a tooling version of the "compilation firewall".
- Interface-only "Abstractions" packages (for example `Microsoft.Extensions.Logging.Abstractions`)
  are this chapter's package structure applied at ecosystem scale.
