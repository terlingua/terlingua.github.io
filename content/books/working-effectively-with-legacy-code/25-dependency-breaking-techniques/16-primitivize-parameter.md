---
title: "Primitivize Parameter"
questions:
  - id: when
    prompt: "When does Feathers resort to Primitivize Parameter?"
    options:
      - id: wall
        text: "When getting the class under test is ridiculously expensive and his back is against the wall"
        correct: true
      - id: oda89bd
        text: "When the class is easy to test"
      - id: o3b732d
        text: "As a first choice for any change"
    explanation: "“But sometimes the amount of work that you have to do to get a class under test is ridiculously large.” and “I don’t like to do this refactoring, but I will use it if my back is against the wall.”"
  - id: mechanism
    prompt: "How is `hasGapFor` made testable?"
    options:
      - id: o0f98e7
        text: "Sequence is mocked"
      - id: primitives
        text: "Logic is written in a free function on primitive data (vectors of durations), tested directly; `Sequence::hasGapFor` converts and delegates"
        correct: true
      - id: o600aec
        text: "A link seam replaces Event"
    explanation: "“We can write another method that will do the calculation on ints. When we have it, we can write hasGapFor and let it do its work by delegating to the other method.”"
  - id: horrible
    prompt: "Which \"horrible things\" does Feathers list about the result?"
    type: multiple
    options:
      - id: untested
        text: "Wrote untested code (`getDurationsCopy`)"
        correct: true
      - id: duplicate
        text: "Duplicated data"
        correct: true
      - id: prolonged
        text: "Prolonged the real problem of domain/infrastructure dependencies"
        correct: true
      - id: expose
        text: "Exposed Sequence's internal representation"
        correct: true
      - id: oa58aa7
        text: "Made the code run slower"
    explanation: "The list includes “Exposed the internal representation of Sequence.” “Written some untested code” “Duplicated data in the system.” and “Prolonged the problem.”"
  - id: better
    prompt: "Overall, what is better than Primitivize Parameter?"
    options:
      - id: o2af5f0
        text: "Link Substitution"
      - id: sprout-class
        text: "Adding the code to the original class, or Sprout Class to build new abstractions; Primitivize is often a predecessor to Sprout Class (e.g. a `GapFinder`)"
        correct: true
      - id: o349032
        text: "Nothing; it's ideal"
    explanation: "“Overall, it is better to add the new code to the original class or to use Sprout Class (63) to build up some new abstractions”. “Often it is a good predecessor to Sprout Class (63). To see this, imagine wrapping SequenceHasGapFor in a class called GapFinder.”"
  - id: condition
    prompt: "Under what condition only does Feathers use it?"
    options:
      - id: oa60b8a
        text: "When no one will review the code"
      - id: o7147f0
        text: "When the deadline is tomorrow"
      - id: later
        text: "When he's confident he'll take the time to bring the class under test later and fold the function back in"
        correct: true
    explanation: "“The only time I use Primitivize Parameter is when I feel confident that I will take the time to bring the class under test later.”"
---
## Learning objectives

- Test logic on a primitive representation when the domain class is untestable
- Recognise and plan to repay the debt it creates

## Connections to today

"Functional core" style — pure functions over plain data, like `SequenceHasGapFor(IReadOnlyList<int>,
IReadOnlyList<int>)` — is good design when chosen deliberately. The smell here is exposing internals
just to reach it.
