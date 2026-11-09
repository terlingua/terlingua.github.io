---
title: "Summary — Sprouting Around the Carcasses"
questions:
  - id: biggest-obstacle
    prompt: "What does Feathers call the biggest obstacle to improvement in large code bases?"
    options:
      - id: existing-code
        text: "The existing code — and what it leads you to believe about whether improvement is worth it"
        correct: true
      - id: ofe4dbc
        text: "Management"
      - id: o6093d0
        text: "Lack of tooling"
    explanation: "“The biggest obstacle to improvement in large code bases is the existing code.” — “I’m talking about what that code leads you to believe.”"
  - id: consistent
    prompt: "What happens when small improvements are made consistently over a couple of months?"
    options:
      - id: obfc025
        text: "The code becomes slower"
      - id: o1f4cf6
        text: "Nothing measurable"
      - id: different
        text: "The system starts to look significantly different, and people feel the difference between good and bad code"
        correct: true
    explanation: "“if you consistently do these little improvements, your system will start to look significantly different over the course of a couple of months.”"
  - id: carcasses
    prompt: "Why do teams eventually get the old big classes under test after sprouting around them for a while?"
    type: multiple
    options:
      - id: tiredness
        text: "Sheer tiredness of side-stepping the \"trash in the living room\""
        correct: true
      - id: familiarity
        text: "Familiarity — repeatedly looking at the class makes it less scary"
        correct: true
      - id: ofd006b
        text: "The compiler requires it"
    explanation: "“Part of this is familiarity.” — “It gets less scary. The other part of it is sheer tiredness. You get tired of looking at the trash in your living room, and you want to take it out.”"
  - id: design-verdict
    prompt: "From a design point of view, how should you judge sprouted and wrapped code?"
    options:
      - id: mixed
        text: "Mixed — sometimes it separates responsibilities and moves toward better design, sometimes it exists only because the old class wasn't ready for tests"
        correct: true
      - id: o2e6c48
        text: "Always good design"
      - id: of8155e
        text: "Always bad design"
    explanation: "“In many cases, they allow us to put some distance between distinct new responsibilities and old ones.” “But in other cases, we know that the only reason we’ve created a class is because we wanted to write new code with tests”."
  - id: pick-technique
    prompt: "Scenario: a 6,000-line `OrderManager` can't be instantiated in tests. You must send a webhook every time `Submit()` is called, from 30 call sites. Which technique fits best?"
    options:
      - id: o448cc0
        text: "Inline the webhook call in all 30 callers"
      - id: wrap-class-decorator
        text: "Wrap Class as a decorator over an extracted interface"
        correct: true
      - id: oeae174
        text: "Sprout Method inside `OrderManager`"
    explanation: "The class can't be instantiated, so sprouting into it won't get tested; many callers need the behaviour, and “When there are many calls to the code you want to wrap, it often pays to move toward a decorator-ish wrapper.” Extract Interface makes the wrapper testable."
---
## Learning objectives

- Choose among the four techniques for a given scenario
- Explain how small, consistent improvements change a team's beliefs

## Decision guide

| Situation | Technique |
|-----------|-----------|
| Class is testable or nearly so; change is a distinct step | Sprout Method |
| Class can't be compiled or instantiated in a harness | Sprout Class |
| New behaviour must run before/after every call; method name should stay | Wrap Method |
| Same, but class is untestable or too large to grow | Wrap Class |
