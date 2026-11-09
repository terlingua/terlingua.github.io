---
title: "Tackling Monsters with Automated Refactoring Support"
questions:
  - id: tool-only
    prompt: "When using a refactoring tool without tests on a monster method, what discipline does Feathers recommend?"
    options:
      - id: oc7a6d5
        text: "Mix tool and manual edits freely"
      - id: oe10c43
        text: "Only rename variables"
      - id: exclusive
        text: "Use the tool exclusively — avoid even reordering statements or splitting expressions by hand"
        correct: true
    explanation: "“it pays to make a series of changes solely with the tool and to avoid all other edits to the source.” “you should avoid even simple things, such as reordering statements and breaking apart expressions.”"
  - id: why
    prompt: "Why accept \"refactoring with one hand behind your back\"?"
    options:
      - id: separation
        text: "It cleanly separates changes known to be safe from changes that aren't"
        correct: true
      - id: o72aced
        text: "Tools require it"
      - id: obfc3ef
        text: "It is faster"
    explanation: "“it gives you a clean separation between changes that are known to be safe and changes that aren’t.”"
  - id: reorder
    prompt: "What auxiliary change do tools not analyse for safety, per the book?"
    options:
      - id: oe615d5
        text: "Renaming methods"
      - id: reordering
        text: "Reordering statements to group them for extraction"
        correct: true
      - id: o81ce01
        text: "Extracting methods"
    explanation: "“often we’re tempted to reorder statements to group them for extraction. No current tool does the analysis needed to see whether reordering can be done safely.”"
  - id: goals
    prompt: "What should your key goals be when extracting?"
    type: multiple
    options:
      - id: ob6c8f8
        text: "Minimise the number of methods"
      - id: separate-logic
        text: "Separate logic from awkward dependencies"
        correct: true
      - id: seams
        text: "Introduce seams that make it easier to get tests in place"
        correct: true
    explanation: "“1. To separate logic from awkward dependencies” and “2. To introduce seams that make it easier to get tests in place”."
  - id: hokey
    prompt: "The extracted names (`commoditiesAreReadyForUpdate`, `singleBrokerCommodity`) seem \"hokey\". Why keep them?"
    options:
      - id: o79dff2
        text: "Names don't matter"
      - id: o063e71
        text: "Tools can't rename"
      - id: start
        text: "They let code communicate at a higher level and introduce seams; you can rename later as things settle"
        correct: true
    explanation: "“They seem a little hokey, don’t they? But they are a good starting point. At the very least, they allow the code to communicate at a higher level, and they introduce seams that allow us to break dependencies.” “We can rename methods later as they settle into place.”"
  - id: which-move
    prompt: "For `CommoditySelectionPanel`, which methods should stay and which move?"
    options:
      - id: o2d55cf
        text: "Move everything into one new class"
      - id: display-stays
        text: "Keep `clearDisplay` and `displayCommodity` on the panel (it's a display); move `update` and `updateCommodities` to a `CommodityFilter`"
        correct: true
      - id: o7ea424
        text: "Move the display methods out and keep the filtering"
    explanation: "“it would be more appropriate to see if we can move update and updateCommodities to another class and leave clearDisplay and displayCommodity here so that we can take advantage of the fact that this class is a panel, a display.”"
  - id: coarse
    prompt: "What is the key thing to remember when using an automated tool on monsters?"
    options:
      - id: coarse-safe
        text: "You can do a lot of coarse work safely and handle the details once other tests are in place"
        correct: true
      - id: o13fafa
        text: "Get every detail perfect on the first pass"
      - id: o48eaf1
        text: "Never extract more than one method"
    explanation: "“you can do a lot of coarse work safely and handle the details after you get other tests in place. Don’t be too concerned about methods that seem like they don’t fit the class.”"
---
## Learning objectives

- Refactor with a tool exclusively to keep edits provably safe
- Choose extraction goals that create seams

## Connections to today

Roslyn refactorings are strong but still won't prove statement reordering safe. Committing after
each tool-only step gives an easy rollback point and a clean review diff.
