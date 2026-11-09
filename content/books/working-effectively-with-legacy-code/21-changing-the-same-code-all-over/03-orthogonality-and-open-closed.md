---
title: "Orthogonality and the Open/Closed Principle"
questions:
  - id: new-command
    prompt: "After the refactoring, how do you add a new command?"
    options:
      - id: o950b5c
        text: "Copy `LoginCommand`"
      - id: o69b176
        text: "Edit `Command.write`"
      - id: subclass
        text: "Subclass `Command` — instead of copy-pasting another command and changing variables"
        correct: true
    explanation: "“What happens when we need to add a new command? Well, we can just subclass Command and create it.” Copying “is error prone”."
  - id: aggregate
    prompt: "How is a command that nests other commands handled?"
    options:
      - id: odfa881
        text: "It can't be supported"
      - id: override-body
        text: "Subclass `Command` and override `writeBody` — everything else just works"
        correct: true
      - id: o529768
        text: "Rewrite `Command`"
    explanation: "“We can do that easily by subclassing Command and overriding its writeBody method” — “Everything else just works.”"
  - id: orthogonality
    prompt: "What is orthogonality, per this chapter?"
    options:
      - id: o91d330
        text: "Code at right angles on screen"
      - id: one-place
        text: "Independence — there is exactly one place to go to change a given behaviour, one knob per behaviour"
        correct: true
      - id: o262d40
        text: "Using interfaces everywhere"
    explanation: "“Orthogonality is a fancy word for independence. If you want to change existing behavior in your code and there is exactly one place you have to go to make that change, you’ve got orthogonality.”"
  - id: terminator
    prompt: "If fields needed a `0x01` terminator instead of `0x00`, how many places change in the refactored design?"
    options:
      - id: one
        text: "One — `writeField`"
        correct: true
      - id: ob2bf49
        text: "One per command class"
      - id: o13d3a1
        text: "One per field"
    explanation: "“In the original design, if we had to use a 0x01 terminator for fields rather than a 0x00 terminator, we would have had to go through the code and make that change in many places.” Now “we can edit or override writeField if we want to change how fields are written”."
  - id: creative
    prompt: "What was the only really creative part of this refactoring?"
    options:
      - id: naming
        text: "Coming up with names for the new methods — concepts like field and body that were implicit in the code"
        correct: true
      - id: o908ec5
        text: "Designing the class hierarchy up front"
      - id: of0fca7
        text: "Choosing the byte values"
    explanation: "“The only creative thing that we’ve really done is come up with names for the new methods. The original code didn’t have the concept of a field or a command body, but in a way, the concept was there in the code.”"
  - id: emerge
    prompt: "What startling thing do you discover when you remove duplication zealously?"
    options:
      - id: designs-emerge
        text: "Designs emerge — many knobs just happen without planning"
        correct: true
      - id: odac67a
        text: "Code always gets longer"
      - id: o30f2da
        text: "Tests become unnecessary"
    explanation: "“One of the startling things that you discover when you start removing duplication zealously is that designs emerge. You don’t have to plan most of the knobs in your application; they just happen.”"
  - id: ocp
    prompt: "How does the book state the Open/Closed Principle?"
    options:
      - id: ob2d76c
        text: "Classes should be open source"
      - id: ocp-def
        text: "Code should be open for extension but closed to modification — good design means little code changes to add features"
        correct: true
      - id: o0d07b7
        text: "Methods should be public and final"
    explanation: "“The idea behind it is that code should be open for extension but closed to modification. What does that mean? It means that when we have good design, we just don’t have to change code much to add new features.”"
  - id: dup-ocp
    prompt: "What connection between duplication removal and OCP does the sidebar draw?"
    options:
      - id: oa4d69c
        text: "OCP requires duplication"
      - id: naturally
        text: "Removing duplication often makes code naturally fall in line with OCP"
        correct: true
      - id: o3914d8
        text: "They conflict"
    explanation: "“When we remove duplication, our code often naturally starts to fall in line with the Open/Closed Principle.”"
---
## Learning objectives

- Explain orthogonality as "one knob per behaviour"
- Connect duplication removal to emergent design and OCP

## Connections to today

"Rule of three" and DRY are often applied mechanically. Feathers' framing is more useful: the goal
is one knob per behaviour, and the evidence is how many places a likely change would touch.
