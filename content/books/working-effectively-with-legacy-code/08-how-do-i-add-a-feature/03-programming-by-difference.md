---
title: "Programming by Difference"
questions:
  - id: definition
    prompt: "What is Programming by Difference?"
    options:
      - id: o2f9ace
        text: "Comparing diffs between branches"
      - id: ob9661c
        text: "Writing only the code that differs from a specification"
      - id: inheritance
        text: "Using inheritance to introduce a feature without modifying a class directly, then deciding how to integrate it"
        correct: true
    explanation: "“We can use inheritance to introduce features without modifying a class directly. After we’ve added the feature, we can figure out exactly how we really want the feature integrated.”"
  - id: history
    prompt: "Why is it acceptable to use inheritance this way, given it fell out of favour in the 1990s?"
    options:
      - id: ob660b8
        text: "It isn't acceptable"
      - id: o292d2a
        text: "Inheritance problems were solved by Java"
      - id: temporary
        text: "Using inheritance initially doesn't mean keeping it; with tests you can move to other structures"
        correct: true
    explanation: "“But just because we use inheritance initially doesn’t mean that we have to keep it in place. With the help of the tests, we can move easily to other structures if the inheritance becomes problematic.”"
  - id: protected
    prompt: "What minimal change to `MessageForwarder` enabled `AnonymousMessageForwarder`?"
    options:
      - id: o17facb
        text: "Adding a static factory"
      - id: ofbaedb
        text: "Making the class final"
      - id: protected-opt
        text: "Making `getFromAddress` protected instead of private, then overriding it"
        correct: true
    explanation: "“Here we’ve made the getFromAddress method protected in MessageForwarder rather than private. Then we overrode it in AnonymousMessageForwarder.”"
  - id: catch
    prompt: "What's the catch with using Programming by Difference repeatedly?"
    options:
      - id: one-at-a-time
        text: "Features in distinct subclasses can only be had one at a time — e.g. no forwarder that is both anonymous and off-list"
        correct: true
      - id: o5b8dda
        text: "Subclasses run slower"
      - id: o7702b1
        text: "Tests can't instantiate subclasses"
    explanation: "“If we put features into distinct subclasses, we can only have one of those features at a time.”"
  - id: way-out
    prompt: "How does Feathers get out of the bind before adding off-list recipients?"
    options:
      - id: o2dd571
        text: "Multiple inheritance"
      - id: refactor-to-config
        text: "Refactor toward a configuration option, using the existing anonymous test to verify behaviour is preserved"
        correct: true
      - id: oc7d833
        text: "Copy the class twice"
    explanation: "“One way is to stop before adding the offlist recipients feature and refactor so that it can go in cleanly. Luckily, we have that test in place that we wrote earlier.” He then makes anonymity a configuration property."
  - id: verify-override
    prompt: "How did Feathers check that `MessageForwarder` alone handled the anonymous case?"
    options:
      - id: o7886ef
        text: "Code review"
      - id: comment-out
        text: "Commented out the override in `AnonymousMessageForwarder` and saw the tests still pass"
        correct: true
      - id: o7ea25d
        text: "Manual testing in a mail client"
    explanation: "“We can verify this by commenting out the override of getFrom in AnonymousMessageForwarder and seeing if the tests pass” — “Sure enough, they do.”"
  - id: srp
    prompt: "Folding anonymous and off-list features into `MessageForwarder` — is that bad under the Single Responsibility Principle?"
    options:
      - id: depends
        text: "It can be; it depends on how large and tangled the code for that responsibility gets"
        correct: true
      - id: o68eec4
        text: "Always"
      - id: o6e7b34
        text: "Never"
    explanation: "“Is this bad in light of the Single Responsibility Principle (246)? It can be. It depends on how large the code related to a responsibility gets and how tangled it is with the rest of the code.”"
  - id: mailing-list
    prompt: "Why was `MailingConfiguration` renamed to `MailingList`?"
    options:
      - id: obf4775
        text: "To match a database table"
      - id: o14ea6a
        text: "Because Java forbids the word Configuration"
      - id: active
        text: "It now actively builds and modifies data (from address, recipients), unlike a passive configuration"
        correct: true
    explanation: "“A configuration is usually a rather passive thing. This class actively builds and modifies data for MessageFowarders at their request.” — “the name MailingList might be a good fit.”"
  - id: rename-class
    prompt: "Which refactoring does Feathers call \"the most powerful\"?"
    options:
      - id: oe83a9e
        text: "Extract Method"
      - id: o18bae1
        text: "Move Method"
      - id: rename
        text: "Rename Class"
        correct: true
    explanation: "“There are many powerful refactorings, but Rename Class is the most powerful. It changes the way people see code and lets them notice possibilities that they might not have considered before.”"
---
## Learning objectives

- Add a feature by subclassing, pin it with a test, then refactor away the subclass
- Recognise the "one feature per subclass" trap
- Evolve configuration data into a behavioural class

## In a nutshell

Subclass, override, pass the test. Then use that test to move the behaviour into a better place —
configuration, then a class with real responsibilities, then a good name.

## Connections to today

- In C#, overriding requires the base member to be `virtual`, and `protected` is usually needed —
  the same two edits as Java's `private` → `protected`.
- Options objects (`IOptions<MailingOptions>`) are the modern properties collection. When the
  option starts driving behaviour, it is time for a `MailingList`-style class.
