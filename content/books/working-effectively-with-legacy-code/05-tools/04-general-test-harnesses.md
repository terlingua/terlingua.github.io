---
title: "General Test Harnesses — FIT and Fitnesse"
questions:
  - id: fit-idea
    prompt: "What is the core idea of FIT?"
    options:
      - id: tables
        text: "Documents with embedded HTML tables describing inputs and outputs can be run as tests"
        correct: true
      - id: o7b85f1
        text: "Generating unit tests from source code"
      - id: o772723
        text: "Recording GUI sessions and replaying them"
    explanation: "“If you can write documents about your system and embed tables within them that describe inputs and outputs for your system, and if those documents can be saved as HTML, the FIT framework can run them as tests.”"
  - id: fit-output
    prompt: "How does FIT report results?"
    options:
      - id: colored
        text: "It outputs the same HTML, with cells colored green for passing values and red for failing ones"
        correct: true
      - id: o5c6479
        text: "As an XML log file only"
      - id: ob78f12
        text: "It prints a pass count to the console"
    explanation: "“The output looks the same as the input, and all text and tables are preserved. However, the cells in the tables are colored green to indicate values that made a test pass and red to indicate values that caused a test to fail.”"
  - id: fit-communication
    prompt: "What does Feathers call one of FIT's very powerful properties?"
    options:
      - id: communication
        text: "Fostering communication between people who write software and people who specify what it should do"
        correct: true
      - id: od15cc7
        text: "Its ability to replace unit tests"
      - id: o96b955
        text: "Its speed compared with xUnit"
    explanation: "“One of the very powerful things about FIT is its capability to foster communication between people who write software and people who need to specify what it should do.”"
  - id: fitnesse
    prompt: "What is Fitnesse?"
    options:
      - id: o990139
        text: "A C++ unit testing harness"
      - id: oe9dfde
        text: "A commercial GUI testing tool"
      - id: wiki
        text: "FIT hosted in a wiki, with hierarchical pages of test tables run individually or as suites"
        correct: true
    explanation: "“Fitnesse is essentially FIT hosted in a wiki.” and “Fitnesse supports hierarchical web pages that define FIT tests.”"
  - id: xunit-vs-fit
    prompt: "Which harness does Chapter 5 consider the proper domain for testing several classes at a time?"
    options:
      - id: fit-fitnesse
        text: "FIT and Fitnesse"
        correct: true
      - id: ob35cfe
        text: "CppUnitLite"
      - id: o0c640b
        text: "xUnit only"
    explanation: "“They can be used to test several classes at a time, but that sort of work is more properly the domain of FIT and Fitnesse.”"
---
## Learning objectives

- Describe FIT's table-driven approach and its communication value
- Place general harnesses alongside unit-test harnesses

## In a nutshell

FIT and Fitnesse let non-programmers write executable specifications as tables. Tests written
before the feature exists fail until developers implement it.

## Connections to today

- Gherkin-based tools (Reqnroll, the community fork that succeeded SpecFlow in .NET) and
  example-mapping workshops carry the same idea: shared, executable examples.
- Approval/snapshot testing (for example Verify) is another general harness well suited to legacy
  code, covered in the Chapter 13 notes.
