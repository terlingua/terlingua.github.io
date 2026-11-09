---
title: "Foundations Check — Before Chapter 8"
questions:
  - id: rollback
    prompt: "Inside `BEGIN; INSERT …; UPDATE …; ROLLBACK;`, what happens to the INSERT?"
    options:
      - id: ob16c9f
        text: "It is committed automatically"
      - id: o00bce2
        text: "It is undone along with the UPDATE"
        correct: true
      - id: ob79d87
        text: "It is kept because it ran first"
      - id: o78769d
        text: "It is moved to a log for later"
    explanation: "ROLLBACK discards every change made since BEGIN. This all-or-nothing property is what the book calls atomicity."
  - id: race
    prompt: "Two threads each do `x = x + 1` on a shared variable starting at 0, without synchronization. Which final values are possible?"
    options:
      - id: o81aac5
        text: "0, 1 or 2"
      - id: oc5ee17
        text: "Only 2"
      - id: oe2ccd3
        text: "Only 1"
      - id: oe63c19
        text: "1 or 2"
        correct: true
    explanation: "Each increment is read, add, write. If both read 0 before either writes, the result is 1 — a lost update."
  - id: deadlock
    prompt: "Thread A holds lock 1 and waits for lock 2; thread B holds lock 2 and waits for lock 1. What is this?"
    options:
      - id: o795156
        text: "Starvation"
      - id: o8a8a1a
        text: "Livelock"
      - id: o9624f9
        text: "Deadlock"
        correct: true
      - id: o5a0111
        text: "A race condition that resolves itself"
    explanation: "Each waits for the other forever. Databases detect such cycles and abort one transaction."
  - id: cas
    prompt: "What does compare-and-swap (CAS) do?"
    options:
      - id: oc44ca1
        text: "Locks a variable until the thread exits"
      - id: oc6fd00
        text: "Swaps two variables without a temporary"
      - id: o9e9d3d
        text: "Compares two strings and sorts them"
      - id: o61affd
        text: "Atomically sets a value only if it still equals an expected value"
        correct: true
    explanation: "CAS is the building block of lock-free algorithms; the chapter's conditional writes are its database equivalent."
  - id: mutex-rw
    prompt: "What does a readers–writer lock allow?"
    options:
      - id: o75f8c2
        text: "Many writers and one reader"
      - id: o38d396
        text: "Many concurrent readers or one exclusive writer"
        correct: true
      - id: o9aa224
        text: "Readers and writers simultaneously"
      - id: o32c95d
        text: "Only one reader at a time"
    explanation: "Shared mode for readers, exclusive mode for a writer — the lock modes used in two-phase locking."
  - id: autocommit
    prompt: "In most SQL databases, what happens to a single statement executed without BEGIN?"
    options:
      - id: o5652cb
        text: "It is queued until the next BEGIN"
      - id: oe2034a
        text: "It is never committed"
      - id: o2dc434
        text: "It runs without any atomicity"
      - id: oab7abf
        text: "It runs in its own implicit transaction and commits automatically (autocommit)"
        correct: true
    explanation: "Autocommit wraps each standalone statement in a transaction, so a single UPDATE is still atomic."
  - id: isolation-levels-sql
    prompt: "Which isolation levels does the SQL standard name?"
    options:
      - id: o16ffdb
        text: "Snapshot, MVCC, 2PL, SSI"
      - id: o2aaead
        text: "Read uncommitted, read committed, repeatable read, serializable"
        correct: true
      - id: oecf1c9
        text: "Eventual, causal, linearizable, strict"
      - id: o5bb7d0
        text: "Low, medium, high, maximum"
    explanation: "The standard's four levels date back to System R. The chapter explains why snapshot isolation does not fit neatly into them."
---
Chapter 8 assumes familiarity with SQL transactions and with concurrency basics from multithreaded
programming. These questions are not drawn from the book's text.

## Learning objectives

- Recall BEGIN/COMMIT/ROLLBACK semantics
- Recall race conditions, mutexes, deadlocks and compare-and-swap
