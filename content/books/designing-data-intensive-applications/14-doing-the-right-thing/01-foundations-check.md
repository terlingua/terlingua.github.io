---
title: "Foundations Check — Before Chapter 14"
questions:
  - id: proxy
    prompt: "A model never sees a protected attribute but uses postal code, which correlates strongly with it. What can happen?"
    options:
      - id: obbb161
        text: "It cannot discriminate because the attribute was removed"
      - id: ob907e5
        text: "The model can still discriminate on the protected attribute via the proxy"
        correct: true
      - id: ofd3f49
        text: "Postal codes are random"
      - id: o65571a
        text: "The model becomes more accurate for everyone equally"
    explanation: "Removing a column does not remove correlated information; this is the proxy problem the chapter discusses."
  - id: base-rate
    prompt: "A test for a rare condition (1 in 1,000) is 99% accurate. A random person tests positive. Roughly what is the chance they have the condition?"
    options:
      - id: o4b4278
        text: "About 9%"
        correct: true
      - id: oc9cac3
        text: "1%"
      - id: oe00104
        text: "99%"
      - id: o938ff1
        text: "50%"
    explanation: "Out of 100,000 people, about 99 true positives versus about 999 false positives: 99 / 1,098 ≈ 9%. Rare-event predictors flag many innocent people."
  - id: individual-vs-pop
    prompt: "What does an accurate population-level probability tell you about one individual?"
    options:
      - id: o3f1b82
        text: "Exactly what will happen to them"
      - id: of60dbb
        text: "Their protected attributes"
      - id: o7b3942
        text: "Little — individual outcomes can differ widely from the average"
        correct: true
      - id: oc2c518
        text: "Nothing useful at all for groups"
    explanation: "Statistics describe distributions; the chapter makes the same point with life expectancy."
  - id: pseudonymization
    prompt: "Is pseudonymized data (names replaced by IDs) automatically anonymous?"
    options:
      - id: o214187
        text: "Only in the EU"
      - id: o953d70
        text: "Yes, always"
      - id: oe3a0e8
        text: "Only if encrypted"
      - id: oce3433
        text: "No — it can often be re-identified by combining it with other data"
        correct: true
    explanation: "Under GDPR, pseudonymized data is still personal data because re-identification is possible."
  - id: feedback
    prompt: "What is a self-reinforcing feedback loop in a decision system?"
    options:
      - id: o6f2e49
        text: "The system's decisions change the data it later learns from, amplifying its own effects"
        correct: true
      - id: ocff9fb
        text: "A network retry storm only"
      - id: o075fd9
        text: "A UI refresh loop"
      - id: o3fbd3e
        text: "A compiler optimization"
    explanation: "Predictions that alter outcomes (e.g., denying credit) can produce data that confirms the prediction."
---
Chapter 14 assumes some statistical literacy about prediction and a basic sense of privacy law. These
questions are not drawn from the book's text.

## Learning objectives

- Reason about base rates, proxies and individual versus population predictions
- Recall basic data protection concepts
