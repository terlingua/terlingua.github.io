---
title: "A Model of Computation (pp. 169–171)"
questions:
  - id: why-model
    prompt: >-
      Why does the P vs NP question need a formal model of computation?
    explanation: >-
      “To have meaning, mathematical statements must be precise … Here we need to be precise in what we mean
      by an algorithm, that is, what does it mean to be computable?” (p. 169). Hilbert’s Entscheidungsproblem
      forced the issue, and Gödel, Church and Turing answered it. Without a fixed model, “polynomial number
      of steps” has no definite meaning.
    options:
      - text: "It doesn't; any intuition will do"
      - text: "Because “polynomial-time algorithm” must be defined precisely to be provable or refutable"
        correct: true
      - text: "Because computers are unreliable"
      - text: "For patent reasons"
  - id: turing-machine
    prompt: >-
      What are the components of a Turing machine?
    explanation: >-
      “Turing’s machine has a tape for holding symbols, a head that moves along the tape reading and writing
      symbols in individual cells, and a controller to guide the read/write head. The machine also has a
      finite set of states, with two special states being initial and halt. The controller is actually a table”
      giving, for (state, symbol), a symbol to write, a direction to move and the next state (p. 169). A
      machine is fully described by its transition table.
    options:
      - text: "A tape, a read/write head, a finite set of states, and a transition table"
        correct: true
      - text: "A neural network"
      - text: "A stack and a queue"
      - text: "A CPU, RAM and disk"
  - id: parity-machine
    prompt: >-
      The parity Turing machine of figure 9.1 has states initial, odd, even and halt. Reading a 1 in state
      *odd*, what does it do?
    explanation: >-
      “If the machine is in state odd and we read the symbol 1, then we write a blank symbol in the cell, move
      one cell to the right, and change the state to even” (p. 170). At the blank after the input it writes 0
      (even) or 1 (odd) and halts. It is a finite-state machine with a tape, the same idea as a lexer or
      protocol state machine.
    options:
      - text: "Halts"
      - text: "Writes 0, moves right, stays odd"
      - text: "Writes 1, moves left, stays odd"
      - text: "Writes blank, moves right, switches to even"
        correct: true
  - id: church-turing
    prompt: >-
      What is the Church–Turing Thesis?
    explanation: >-
      “The working assumption that we can equate algorithms and Turing machines is known as the Church-Turing
      Thesis” (p. 170). It is a thesis, not a theorem: “so far, Turing machines have been able to handle
      everything that has been thrown at them. If something is computable on a modern-day computer, then a
      lightning-fast Turing machine could also carry out the computation” (p. 170). Multi-tape machines add
      speed but not power, since one tape can simulate many with polynomial slowdown.
    options:
      - text: "A theorem that quantum computers are faster"
      - text: "A claim that Turing machines are physical devices"
      - text: "A proven theorem that P ≠ NP"
      - text: "The working assumption that “algorithm” means “something a Turing machine can do”"
        correct: true
  - id: universal
    prompt: >-
      What was Turing's Universal Machine, and why does it matter for software?
    explanation: >-
      “One can design a Universal Turing machine capable of simulating every Turing machine … The concept is
      thus to include the table as part of the input on the tape, just like we include a program on a modern
      computer” (p. 171). Shoes do one job, while an iPhone runs apps nobody imagined when it was built. This
      is the stored-program idea behind every interpreter, VM, container runtime and emulator.
    options:
      - text: "A machine that solves the TSP"
      - text: "A faster tape"
      - text: "A machine with infinite states"
      - text: "One machine that runs any other machine given its description as input — the stored-program idea"
        correct: true
---
## Learning objectives

- Describe a Turing machine and its transition table.
- State the Church–Turing Thesis and the universal machine idea.

## Summary

- Hilbert asked for a decision procedure for provability. Gödel, Church and Turing formalized computation (p. 169).
- **Turing machine:** tape, head, finite states (initial and halt), transition table: (state, symbol) → (write, move, next state) (pp. 169–170).
- **Parity example:** states *initial / odd / even* flip on each 1 and write the answer at the end (figure 9.1).
- Multi-tape machines are more convenient and only polynomially faster (p. 170).
- **Church–Turing Thesis:** algorithm = Turing machine (pp. 170–171).
- **Universal machine:** the program is data on the tape. This is the basis of general-purpose computers (p. 171).

```csharp
enum State { Initial, Odd, Even, Halt }

static int Parity(string bits)
{
    var state = State.Initial;
    foreach (var b in bits)
        state = (state, b) switch
        {
            (State.Odd, '1') => State.Even,
            (State.Odd, _) => State.Odd,
            (_, '1') => State.Odd,
            _ => State.Even,
        };
    return state == State.Odd ? 1 : 0;
}
```

## In software terms

Polynomial-time is robust across models, from a single-tape machine to a modern CPU, because the simulations cost only polynomial overhead (p. 174). That is why complexity classes do not depend on your language or hardware.

## Sources

- Cook (2012), pp. 169–171.
- Turing, A. M. “On computable numbers, with an application to the Entscheidungsproblem.” *Proc. London Math. Soc.* 42 (1936), 230–265.
