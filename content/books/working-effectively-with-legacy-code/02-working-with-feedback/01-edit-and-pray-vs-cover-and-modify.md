---
title: "Edit and Pray vs. Cover and Modify"
questions:
  - id: edit-pray-def
    prompt: "Which description best matches Edit and Pray?"
    options:
      - id: o827c65
        text: "Change code randomly until the build passes"
      - id: o2bc5ae
        text: "Ask QA to write tests after release"
      - id: plan-poke
        text: "Plan carefully, make the change, then run the system and poke around hoping nothing broke"
        correct: true
      - id: ode8154
        text: "Write tests first, then change the code"
    explanation: "“When you’re done, you run the system to see if the change was enabled, and then you poke around further to make sure that you didn’t break anything.”"
  - id: care-not-enough
    prompt: "Why is \"working with care\" not sufficient according to the butter-knife analogy?"
    options:
      - id: o135999
        text: "Care only matters for invasive changes"
      - id: tools
        text: "Safety is not solely a function of care; without the right tools and techniques, care does little"
        correct: true
      - id: oe9089e
        text: "Careful people work too slowly"
      - id: o4efc63
        text: "Surgeons do not need care"
    explanation: "“But safety isn’t solely a function of care.” and “Working with care doesn’t do much for you if you don’t use the right tools and techniques.”"
  - id: cover-meaning
    prompt: "In Cover and Modify, what does \"covering\" software mean?"
    options:
      - id: tests
        text: "Surrounding it with tests so bad changes are detected before they spread"
        correct: true
      - id: o5f17ba
        text: "Wrapping it in a façade class"
      - id: o38cf9c
        text: "Adding try/catch blocks around risky code"
      - id: o26b243
        text: "Documenting it thoroughly"
    explanation: "“Covering software means covering it with tests.” The cloak image: “to make sure that bad changes don’t leak out and infect the rest of our software.”"
  - id: detect-change
    prompt: "Which shift in the *purpose* of testing does Chapter 2 describe?"
    options:
      - id: o0f86ce
        text: "From automated testing to code review"
      - id: o0f2c02
        text: "From unit testing to manual testing"
      - id: o021b46
        text: "From regression testing to exploratory testing"
      - id: detect
        text: "From testing to attempt to show correctness, to testing to detect change"
        correct: true
    explanation: "“Testing done this way is really \"testing to attempt to show correctness.\"” versus “We can do \"testing to detect change.\"” — which in traditional terms is regression testing."
  - id: vise
    prompt: "What property of a \"software vise\" makes it valuable during change?"
    options:
      - id: o75db57
        text: "It guarantees the code is correct"
      - id: oc2aa26
        text: "It prevents anyone else from editing the file"
      - id: fixed
        text: "Behaviour is held fixed so you know you are changing only one piece of behaviour at a time"
        correct: true
      - id: o959174
        text: "It speeds up compilation"
    explanation: "“The behavior of the code is fixed in place. When we make changes, we can know that we are changing only one piece of behavior at a time.”"
  - id: regression-problem
    prompt: "What is the \"little problem\" with how regression testing is usually practised?"
    options:
      - id: ob3a8b6
        text: "It requires a dedicated QA team by law"
      - id: app-level
        text: "It is usually done only at the application interface, giving slow, coarse feedback"
        correct: true
      - id: o03f7e6
        text: "It is too fine-grained"
      - id: odb3da3
        text: "It never finds real bugs"
    explanation: "“Often when people practice it, they do it at the application interface.” and “It pays to do it at a finer-grained level.”"
  - id: scenarios-compare
    prompt: "Compare the two scenarios (overnight QA run vs. ~20 unit tests). Which advantages of the second scenario does the chapter illustrate?"
    type: multiple
    options:
      - id: no-blame
        text: "No need to figure out whose change caused an overnight failure"
        correct: true
      - id: oe04a13
        text: "It needed no tests to be written for the new behaviour"
      - id: refactor-safe
        text: "It made refactoring for understanding safe before making the real change"
        correct: true
      - id: minute
        text: "A logic inversion was caught and fixed in about a minute"
        correct: true
    explanation: "“we made a mistake and inverted the logic on a condition, but a test failed and we recovered in about a minute.” In the first scenario, “She’s not sure whether it was our changes”. Note that in the second scenario they still “added some tests to verify the new behavior.”"
---
Chapter 2 names the two ways changes get made and argues that only one of them gives real
feedback.

## Learning objectives

- Contrast Edit and Pray with Cover and Modify
- Explain "testing to detect change" and the software vise
- Argue why fine-grained regression feedback beats overnight runs

## In a nutshell

Edit and Pray substitutes care for feedback. Cover and Modify puts tests around the code first so
that every change is checked within minutes. Regression testing is the right idea; it is usually
done at too coarse a level and too infrequently.

## Connections to today

- **CI pipelines.** A PR check that runs in two minutes is the second scenario; a nightly
  end-to-end suite owned by another team is the first. Both have value, but only the fast one
  supports refactoring as you go.
- **`dotnet watch test`** and IDE live unit testing shorten the loop to seconds, which is the
  "minute, not overnight" argument taken further.
- **Production as the test.** Shipping and watching dashboards is Edit and Pray with better
  telemetry. Observability is valuable, but it detects breakage after users feel it.
