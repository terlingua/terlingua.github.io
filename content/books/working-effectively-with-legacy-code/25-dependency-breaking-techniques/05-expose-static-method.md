---
title: "Expose Static Method"
questions:
  - id: when
    prompt: "When can you use Expose Static Method?"
    options:
      - id: o6dcb36
        text: "When the class is easy to instantiate"
      - id: no-instance
        text: "When a method doesn't use instance data or methods, so it can be tested without instantiating the class"
        correct: true
      - id: od31310
        text: "When the method is virtual"
    explanation: "“If you have a method that doesn’t use instance data or methods, you can turn it into a static method. When it is static, you can get it under test without having to instantiate the class.”"
  - id: why-not-move
    prompt: "`validate` mostly uses `Packet` methods — why not move it onto `Packet` now?"
    options:
      - id: risk
        text: "Moving isn't the least risky step without tool support (you can't Preserve Signatures); get tests in place first"
        correct: true
      - id: oa0cb3f
        text: "Packet is final"
      - id: o1fd8a5
        text: "Methods can't be moved in Java"
    explanation: "“moving the method isn’t the least risky thing we can do right now; we definitely won’t be able to Preserve Signatures (312). If you don’t have automated support to move methods, often it is better to get some tests in place first.”"
  - id: encapsulation
    prompt: "Is making `validate` public static a breach of encapsulation?"
    options:
      - id: not-really
        text: "Not really — the static area isn't really part of the class; a static can't touch private instance data"
        correct: true
      - id: o64c98c
        text: "Only in C++"
      - id: oa05c8b
        text: "Yes, always"
    explanation: "“Encapsulation is a great thing for classes, but the static area of a class isn’t really part of the class.” “When a method is static, you know that it doesn’t access any of the private data of the class; it is just a utility method.”"
  - id: printed-logic
    prompt: "Compare the printed `validate` and the extracted `validatePacket`. The original throws when originator equals \"MIA\" OR length > MAX OR the checksum is NOT valid. What does the printed extraction do?"
    options:
      - id: inverted
        text: "It uses `== \"MIA\"`, `<= MAX_LENGTH` and `hasValidCheckSum()` without `!` — two conditions are inverted, so it isn't behaviour-preserving as printed"
        correct: true
      - id: oea81a8
        text: "It is identical"
      - id: o6db293
        text: "It only changes the exception type"
    explanation: "The printed static reads “packet.getOriginator() == \"MIA\" || packet.getLength() <= MAX_LENGTH || packet.hasValidCheckSum()”. That's why step 1 is “Write a test that accesses the method” and why Preserve Signatures means copying the body verbatim."
  - id: string-equality
    prompt: "In Java, what subtle problem does replacing `.equals(\"MIA\")` with `== \"MIA\"` introduce?"
    options:
      - id: reference
        text: "`==` compares references, so equal strings that aren't the same object fail the check"
        correct: true
      - id: o5f36d4
        text: "It throws NullPointerException"
      - id: o6adf97
        text: "None"
    explanation: "The original uses “packet.getOriginator().equals( \"MIA\")”; the printed extraction uses “packet.getOriginator() == \"MIA\"”. In Java that changes semantics — another reason to copy, not retype, during extraction."
  - id: restrict
    prompt: "How can you limit misuse of an exposed static?"
    options:
      - id: visibility
        text: "Use package/internal visibility, or make it protected and access it through a testing subclass"
        correct: true
      - id: oc3edcf
        text: "Add a comment"
      - id: o13ce2c
        text: "Mark it obsolete"
    explanation: "“In languages such as Java and C#, which have package or internal visibility, you can restrict access to the static or make it protected and access it through a testing subclass.”"
  - id: staging
    prompt: "What does Feathers suggest for any method that uses no instance data?"
    options:
      - id: make-static
        text: "Make it static to make it noticeable until you figure out what class it really belongs on"
        correct: true
      - id: o81f223
        text: "Delete it"
      - id: o069e73
        text: "Make it virtual"
    explanation: "“If you see a method that doesn’t use any instance data, it is a good idea to make it static to make it noticeable until you figure out what class it really belongs on.”"
---
## Learning objectives

- Expose instance-free logic as a static for testing
- Copy bodies verbatim and test immediately

## Connections to today

Roslyn analyzer CA1822 ("Mark members as static") finds candidates automatically. `internal
static` plus `InternalsVisibleTo` keeps the exposure test-only.
