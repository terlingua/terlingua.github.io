---
title: "We Feel Overwhelmed"
questions:
  - id: what-in-it
    prompt: "What does Feathers say makes legacy work worth it to a programmer?"
    options:
      - id: figure-out
        text: "Figuring out what is in it for you — ideally reconnecting with what's fun about programming"
        correct: true
      - id: of7756a
        text: "A rewrite opportunity"
      - id: o44ea90
        text: "Only the paycheck"
    explanation: "“one thing is going to make the job worth it to you as a programmer or not: figuring out what is in it for you.” — “it is still possible to connect with what is fun about programming.”"
  - id: greenfield
    prompt: "What typically happens to the green-field \"replacement system\" team?"
    options:
      - id: chasing
        text: "They must chase a constantly changing old system, fall behind, and face rising pressure — while the old system turns out to be critical"
        correct: true
      - id: o7dd8a3
        text: "They never start"
      - id: o0befef
        text: "They replace the old system within months"
    explanation: "“The green-field team has to do doubleduty, trying to replace a system that is constantly changing. As the months go by it becomes clearer that they are not going to be able to replace the old system”. “The grass isn’t really much greener in green-field development.”"
  - id: thrive
    prompt: "What helps you thrive in legacy code?"
    type: multiple
    options:
      - id: community
        text: "Connecting with the wider programming community"
        correct: true
      - id: people
        text: "Working with people you respect who know how to have fun at work"
        correct: true
      - id: motivation
        text: "Finding what motivates you"
        correct: true
      - id: o7900f6
        text: "Avoiding all legacy projects"
    explanation: "“The key to thriving in legacy code is finding what motivates you.” “there really isn’t much that can replace working in a good environment with people you respect who know how to have fun at work.” “Another thing that helps is to connect with the larger community.”"
  - id: attitude
    prompt: "Teams with millions of lines of legacy code vs. teams with better code who are dejected — what's the lesson?"
    options:
      - id: o553ba5
        text: "Better code always means happier teams"
      - id: oe0d949
        text: "Code size determines morale"
      - id: attitude-matters
        text: "The attitude we bring to the work is important"
        correct: true
    explanation: "“I’ve also seen teams with far better code bases who are dejected. The attitude we bring to the work is important.”"
  - id: tdd-outside
    prompt: "What personal practice does Feathers suggest?"
    options:
      - id: o1504fb
        text: "Take a long vacation"
      - id: o5fe533
        text: "Avoid programming outside work"
      - id: tdd-fun
        text: "TDD some code outside work and program for fun, to feel what fast-tested code is like"
        correct: true
    explanation: "“TDD some code outside of work. Program for fun a little bit. Start to feel the difference between the little projects you make and the big project at work.”"
  - id: morale
    prompt: "If team morale is low because of code quality, what does Feathers suggest trying?"
    options:
      - id: ugliest
        text: "Pick the ugliest, most obnoxious set of classes and get them under test together"
        correct: true
      - id: oa50f48
        text: "Start with the easiest classes"
      - id: of454d9
        text: "Freeze changes"
    explanation: "“Pick the ugliest most obnoxious set of classes in the project, and get them under test. When you’ve tackled the worst problem as a team, you’ll feel in control of your situation.”"
  - id: oases
    prompt: "What develops as you take control of the code base?"
    options:
      - id: od60bb1
        text: "A new architecture"
      - id: ofca8e9
        text: "Fewer features"
      - id: oasis
        text: "Oases of good code where work can really be enjoyable"
        correct: true
    explanation: "“As you start to take control of your code base, you’ll start to develop oases of good code. Work can really be enjoyable in them.”"
---
## Learning objectives

- Recognise the green-field illusion
- Choose concrete actions to rebuild team morale

## Connections to today

"Strangler fig" migrations (Fowler) are the healthier alternative to the big-bang replacement team:
the new system grows around the old one incrementally, guided by the same tests-first habits.
