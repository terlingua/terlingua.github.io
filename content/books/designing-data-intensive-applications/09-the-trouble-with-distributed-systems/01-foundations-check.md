---
title: "Foundations Check — Before Chapter 9"
questions:
  - id: packet-switching
    prompt: "In a packet-switched network, what happens when many senders target one link at once?"
    options:
      - id: o78682a
        text: "Packets queue in switch buffers, and some may be dropped if the buffers fill"
        correct: true
      - id: o9af0da
        text: "Packets are delivered simultaneously"
      - id: o989056
        text: "The receiver rejects all packets"
      - id: o1cc53e
        text: "The network reserves bandwidth for each sender in advance"
    explanation: "Packet switching shares links dynamically; congestion shows up as queueing delay and drops."
  - id: udp
    prompt: "Which is true of UDP?"
    options:
      - id: o65b2a3
        text: "It does not retransmit lost packets or provide flow control"
        correct: true
      - id: o807276
        text: "It establishes a connection with a handshake"
      - id: of972dc
        text: "It guarantees in-order delivery"
      - id: oe0c106
        text: "It is always slower than TCP"
    explanation: "UDP is a thin datagram layer over IP; reliability, if needed, is up to the application."
  - id: context-switch
    prompt: "Why can a running thread be paused at any instruction?"
    options:
      - id: o5b267c
        text: "Pauses only happen during I/O"
      - id: o34422f
        text: "CPUs run one thread forever"
      - id: od32f9c
        text: "The OS scheduler can preempt it to run another thread"
        correct: true
      - id: oe18149
        text: "Threads only pause at function calls"
    explanation: "Preemptive multitasking lets the kernel interrupt a thread at almost any point."
  - id: gc
    prompt: "What is a \"stop-the-world\" garbage collection pause?"
    options:
      - id: o2fe3a5
        text: "A disk flush"
      - id: o59293c
        text: "The runtime halts all application threads while it collects garbage"
        correct: true
      - id: oa1f456
        text: "A database lock"
      - id: o566df9
        text: "A pause in network traffic"
    explanation: "Some collector phases require all mutator threads to stop. Modern collectors shrink, but rarely eliminate, these pauses."
  - id: utc
    prompt: "Why store timestamps in UTC?"
    options:
      - id: oa778e1
        text: "UTC is more precise than local time"
      - id: od2000b
        text: "Databases require UTC"
      - id: o851bf4
        text: "UTC has no daylight saving jumps, avoiding ambiguous or skipped local times"
        correct: true
      - id: o93dae8
        text: "UTC never has leap seconds"
    explanation: "Local time zones jump at DST transitions; UTC is monotonic apart from leap-second handling."
  - id: quartz
    prompt: "What makes a computer's hardware clock drift?"
    options:
      - id: oaf2d82
        text: "Cosmic rays reset it"
      - id: o1d6459
        text: "Its quartz oscillator runs slightly fast or slow, varying with temperature"
        correct: true
      - id: oda4911
        text: "NTP introduces drift"
      - id: od6090b
        text: "The OS adds random offsets"
    explanation: "Quartz frequency is never exact; drift is measured in parts per million."
  - id: majority-two
    prompt: "Why can there never be two disjoint majorities of the same set of nodes?"
    options:
      - id: o7923a6
        text: "Nodes can vote twice"
      - id: o5cdf7f
        text: "Each majority has more than half the nodes, so two of them would need more than all the nodes"
        correct: true
      - id: od69926
        text: "Majorities are chosen randomly"
      - id: o0bc6c0
        text: "Majorities always include the leader"
    explanation: "Two sets each larger than n/2 must overlap — the foundation of quorum safety."
---
Chapter 9 leans on networking basics (packets, TCP/UDP), operating-system scheduling, garbage collection
and clock sources. These questions are not drawn from the book's text.

## Learning objectives

- Recall packet switching, TCP and UDP behaviour
- Recall how threads are scheduled and why GC pauses happen
- Recall what NTP and UTC are
