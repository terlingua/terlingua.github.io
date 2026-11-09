---
title: "Risky Change"
questions:
  - id: three-questions
    prompt: "To mitigate risk, which three questions does Feathers say we must ask?"
    type: multiple
    options:
      - id: od57329
        text: "Who wrote this code originally?"
      - id: not-broken
        text: "How will we know that we haven't broken anything?"
        correct: true
      - id: correctly
        text: "How will we know that we've done them correctly?"
        correct: true
      - id: o6ea174
        text: "Can we postpone the change to the next release?"
      - id: what
        text: "What changes do we have to make?"
        correct: true
    explanation: "“1. What changes do we have to make? 2. How will we know that we’ve done them correctly? 3. How will we know that we haven’t broken anything?”"
  - id: inline-temptation
    prompt: "A developer pastes 40 new lines into an existing method instead of creating a method \"because it involves less editing and is safer\". What long-term consequence does Chapter 1 predict?"
    options:
      - id: grow
        text: "Existing methods and classes grow larger and harder to understand"
        correct: true
      - id: od3191b
        text: "The code becomes faster because there are fewer calls"
      - id: o135d26
        text: "Fewer bugs, because less code is touched"
      - id: o638417
        text: "Nothing — the approach is the recommended conservative policy"
    explanation: "“When we avoid creating new classes and methods, the existing ones grow larger and harder to understand.”"
  - id: rusty
    prompt: "Why does avoiding change make a team *worse* at change over time?"
    options:
      - id: o8b7ef8
        text: "Management stops funding refactoring"
      - id: o4e7764
        text: "Compilers become stricter"
      - id: oe29ccf
        text: "Tools stop supporting old code"
      - id: practice
        text: "People who rarely restructure code get rusty; doing it often makes it routine and teaches what can and cannot break"
        correct: true
    explanation: "“When people don’t make changes often they get rusty at it. Breaking down a big class into pieces can be pretty involved work unless you do it a couple of times a week. When you do, it becomes routine.”"
  - id: good-vs-bad-systems
    prompt: "Per Chapter 1, what distinguishes working in a good system from working in a bad one *after* you have spent time learning the area?"
    options:
      - id: o4c9ff8
        text: "Bad systems compile more slowly"
      - id: o9b1546
        text: "Good systems have more comments"
      - id: calm
        text: "In a good system you feel calm and confident about the change; in a bad one it feels like jumping off a cliff"
        correct: true
      - id: o047f8d
        text: "In a good system no learning time is needed"
    explanation: "“The difference between good systems and bad ones is that, in the good ones, you feel pretty calm after you’ve done that learning, and you are confident in the change you are about to make. In poorly structured code, the move from figuring things out to making changes feels like jumping off a cliff to avoid a tiger.”"
  - id: fear
    prompt: "What does Feathers name as the last consequence of avoiding change?"
    options:
      - id: o94f948
        text: "Higher hosting costs"
      - id: fear-opt
        text: "Fear — which often goes unnoticed until better techniques make it fade"
        correct: true
      - id: ob8959e
        text: "Loss of source control history"
      - id: o81a1a2
        text: "Slower compilers"
    explanation: "“The last consequence of avoiding change is fear.” and “Often they aren’t aware of how much fear they have until they learn better techniques and the fear starts to fade away.”"
  - id: try-harder
    prompt: "Your manager proposes doubling code-review time and adding a second reviewer to make legacy changes safe. What question does the chapter raise about this \"try harder\" alternative?"
    options:
      - id: oae8118
        text: "Whether reviews slow the build"
      - id: oa2a324
        text: "Whether reviewers are senior enough"
      - id: ocdd5cc
        text: "Whether reviews should be asynchronous"
      - id: know-right
        text: "Even after all that scrutiny, will anyone actually know they got it right?"
        correct: true
    explanation: "“Surely more time and scrutiny will make change safer. Or will it? After all of that scrutiny, will anyone know that they’ve gotten it right?” Chapter 2 answers with feedback from tests."
  - id: not-broke
    prompt: "\"If it's not broke, don't fix it\" is described as what kind of risk-management approach?"
    options:
      - id: o0f7262
        text: "An Extreme Programming practice"
      - id: conservative
        text: "A conservative approach of minimising the number of changes — which eventually catches up with you"
        correct: true
      - id: of58a5b
        text: "The recommended default for legacy systems"
    explanation: "“Most teams that I’ve worked with have tried to manage risk in a very conservative way. They minimize the number of changes that they make to the code base.” and “It’s tempting to think that we can minimize software problems by avoiding them, but, unfortunately, it always catches up with us.”"
---
Chapter 1 ends by naming the trap most teams fall into: managing risk by changing as little as
possible.

## Learning objectives

- Recite the three risk questions for any change
- Explain why minimising change makes code and teams worse over time
- Recognise fear as a hidden cost in legacy code bases

## In a nutshell

Avoiding change looks safe but has three costs: code grows into larger and harder units, people
lose practice at restructuring, and fear accumulates. "Trying harder" with more scrutiny still
leaves the third question — *how do we know we haven't broken anything?* — unanswered. The next
chapter answers it with feedback.

## Connections to today

- **Change-averse release processes.** Long freeze windows and change-advisory boards are
  organisational versions of "put the lines right here where I can see them". DORA research on
  delivery performance points the same way as Feathers: smaller, more frequent changes with fast
  feedback are safer than rare large ones.
- **Pull-request size.** Giant PRs are the review-time equivalent of a giant method. Many small,
  test-backed PRs keep reviewers calm for the same reason a good system keeps developers calm.

## Reflect

Pick a file in your code base that everyone avoids. Which of the three questions can you not
answer for a change there today?
