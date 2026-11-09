---
title: "Internal Relationships and Feature Sketches"
questions:
  - id: lumping
    prompt: "What is heuristic"
    options:
      - id: o4d2bc0
        text: "Duplicate code"
      - id: oee2f59
        text: "Methods with the same return type"
      - id: lumps
        text: "\"Lumping\" — groups of instance variables used by some methods and not others"
        correct: true
    explanation: "“Look for relationships between instance variables and methods. Are certain instance variables used by some methods and not others?” and “Usually there is some sort of \"lumping\" in a class.”"
  - id: how-draw
    prompt: "How do you draw a feature sketch?"
    options:
      - id: o56f91d
        text: "A sequence diagram per method"
      - id: circles
        text: "Circles for each variable and method; lines from each method to the variables and methods it uses; usually skip constructors"
        correct: true
      - id: o69bc12
        text: "A UML class diagram"
    explanation: "“Next, we look at each method and put down a circle for it. Then we draw a line from each method circle to the circles for any instance variables and methods that it accesses or modifies. It’s usually okay to skip the constructors.”"
  - id: vs-effect
    prompt: "How do feature sketches differ from effect sketches?"
    options:
      - id: oc059c1
        text: "They are identical"
      - id: o50a377
        text: "Feature sketches are permanent documentation"
      - id: arrows
        text: "Arrows are reversed — feature sketches point to what is used; effect sketches point to what is impacted"
        correct: true
    explanation: "“The main difference is that the arrows are reversed.” “Feature sketches are great for mapping the internal structure of classes. Effect sketches (155) are great for reasoning forward from a point of change.”"
  - id: disposable
    prompt: "Should you keep feature sketches in version control?"
    options:
      - id: throw
        text: "No — they're disposable, drawn with a partner for about 10 minutes and thrown away"
        correct: true
      - id: o6a810d
        text: "Only for public classes"
      - id: o8c4c42
        text: "Yes, as design documentation"
    explanation: "“These sketches are disposable tools. They are the sort of thing that you sit down and draw up with a partner for about 10 minutes before you make your changes. Afterward you throw them away.”"
  - id: reservation-cluster
    prompt: "What cluster does the `Reservation` sketch reveal?"
    options:
      - id: of5700b
        text: "All methods use all variables equally"
      - id: cluster
        text: "`duration`, `dailyRate`, `date` and `customer` are used mainly by `getPrincipalFee`, `extend` and `extendForWeek`"
        correct: true
      - id: o1264c7
        text: "Only `fees` is used anywhere"
    explanation: "“The duration, dailyRate, date, and customer variables are used primarily by getPrincipalFee, extend, and extendForWeek.”"
  - id: flip
    prompt: "The big cluster lacks a good name (\"Reservation\" is taken). What alternative does Feathers explore?"
    options:
      - id: fee-calculator
        text: "Flip it — extract the other cluster as `FeeCalculator`, passing it the principal fee"
        correct: true
      - id: o06b7c5
        text: "Give up on extraction"
      - id: oc0ff3d
        text: "Name it `Reservation2`"
    explanation: "“We could flip things around. Instead of extracting all of the code in the big circle, we can extract the other code” — “We can call the class that we extract FeeCalculator.” Reservation then passes `getPrincipalFee()` into it."
  - id: name-test
    prompt: "What should you figure out before extracting a cluster?"
    options:
      - id: o514672
        text: "Whether it's in the same file"
      - id: o0931a2
        text: "Whether it has more than five methods"
      - id: distinct
        text: "Whether the new class has a good, distinct responsibility — can you name it?"
        correct: true
    explanation: "“The key thing to figure out before attempting this is whether this new class has a good, distinct responsibility. Can we come up with a name for it?”"
  - id: pinch
    prompt: "In a feature sketch, what is the small set of lines connecting two large clusters?"
    options:
      - id: oabfa88
        text: "A seam"
      - id: o0aa181
        text: "A cycle"
      - id: pinch-point
        text: "A pinch point"
        correct: true
    explanation: "“In feature sketches, we often see these connections as a small set of lines connecting larger clusters. I call this a pinch point (180)”."
  - id: circle-interface
    prompt: "When you circle groups of features in a sketch, what do the crossed lines represent?"
    options:
      - id: o0b6a6e
        text: "Bugs"
      - id: interface
        text: "The interface of a potential new class"
        correct: true
      - id: oab3917
        text: "Tests to delete"
    explanation: "“When you circle features, the lines that you cross can define the interface of a new class.”"
---
## Learning objectives

- Draw and read feature sketches
- Choose which cluster to extract, including flipping the extraction

## Connections to today

LCOM (lack of cohesion of methods) metrics in code-analysis tools quantify the lumping a feature
sketch shows visually. Use the metric to find candidates, the sketch to decide.
