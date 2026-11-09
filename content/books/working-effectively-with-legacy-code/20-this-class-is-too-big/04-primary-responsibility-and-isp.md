---
title: "The Primary Responsibility, SRP Levels and ISP"
questions:
  - id: one-sentence
    prompt: "What is heuristic"
    options:
      - id: o91e562
        text: "Read the class's comments"
      - id: sentence
        text: "Try to describe the class's responsibility in a single sentence; see if one clause matters most"
        correct: true
      - id: ob9fa04
        text: "Count the methods"
    explanation: "“Try to describe the responsibility of the class in a single sentence.” — “Is there any one thing that seems more important than anything else? If there is, you might have found the key responsibility of the class.”"
  - id: two-levels
    prompt: "At which two levels can SRP be violated, and which matters most?"
    options:
      - id: impl
        text: "Interface and implementation levels; the implementation level matters most"
        correct: true
      - id: o10ecfe
        text: "Compile and run time; run time matters most"
      - id: o34a175
        text: "Package and method levels; package matters most"
    explanation: "“It can be violated at the interface level and at the implementation level.” “The SRP violation that we care most about is violation at the implementation level.”"
  - id: facade
    prompt: "`ScheduledJob` still exposes many methods but delegates to `JobPool`, `ScheduledJobView` and `ResourceAcquistion`. How should you view it?"
    options:
      - id: o14065a
        text: "As worse than before"
      - id: o3cd34c
        text: "As fully SRP-compliant"
      - id: facade-opt
        text: "As a facade — SRP is still violated at the interface but much better at the implementation level"
        correct: true
    explanation: "“If it delegates, we don’t have a large monolithic class; we just have a class that is a facade” and “The Single Responsibility Principle is still violated at the interface level, but at the implementation level things are a bit better.”"
  - id: isp
    prompt: "What does the Interface Segregation Principle propose for a large class?"
    options:
      - id: o2660fa
        text: "Make all methods static"
      - id: client-interfaces
        text: "Create interfaces for groups of methods particular clients use and have the class implement them"
        correct: true
      - id: o51aac7
        text: "Split the class randomly"
    explanation: "“If we create an interface for each of these groupings and have the large class implement those interfaces, each client can see the big class through that particular interface.”"
  - id: isp-benefits
    prompt: "Which benefits of ISP does the sidebar name?"
    type: multiple
    options:
      - id: ofc0379
        text: "It removes the need for tests"
      - id: hide
        text: "It helps hide information"
        correct: true
      - id: deps
        text: "It decreases dependency"
        correct: true
      - id: recompile
        text: "Clients no longer recompile whenever the large class does"
        correct: true
    explanation: "“This helps us hide information and also decreases dependency in the system. The clients no longer have to recompile whenever the large class does.”"
  - id: invert-delegation
    prompt: "In Figure 20.14, what changes about delegation?"
    options:
      - id: of80a4a
        text: "Nothing"
      - id: o8ec67e
        text: "Both classes are merged"
      - id: controller-delegates
        text: "Instead of `ScheduledJob` delegating to a `JobController`, a `StandardJobController` delegates to `ScheduledJob`, whittling down its interface"
        correct: true
    explanation: "“Instead of having ScheduledJob delegate to a JobController, we’ve made a JobController delegate to ScheduledJob.” “Notice that ScheduledJob no longer has the methods that are on JobController.”"
  - id: tougher
    prompt: "Why is that refactoring \"nearly always tougher than it sounds\"?"
    type: multiple
    options:
      - id: expose
        text: "You may need to expose more methods on the original class for the new front to work"
        correct: true
      - id: o4d02cf
        text: "Interfaces are slow"
      - id: clients
        text: "Client code must change, which needs tests around those clients"
        correct: true
    explanation: "“Often to do it, you have to expose more methods in the public interface of the original class” and “Client code now has to be changed to use the new class rather than the old one; to do that safely, you need to have tests around those clients.”"
---
## Learning objectives

- Distinguish interface-level from implementation-level SRP violations
- Apply ISP with client-specific interfaces

## Connections to today

```csharp
public interface IJobController
{
    void Run();
    void Pause();
    void Resume();
    bool IsRunning { get; }
}
```

Registering one implementation under several narrow interfaces in a DI container is ISP without
changing the class; later, move implementations behind those interfaces into separate classes.
