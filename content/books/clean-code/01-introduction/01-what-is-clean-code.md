---
title: What is clean code?
questions:
  - id: read-write
    prompt: Why is readability usually more important than writing speed?
    options:
      - text: Compilers optimise readable code better
      - text: Readable code always uses fewer lines
      - text: Code reviews are only done on readable code
      - id: read-more
        text: Code is read far more often than it is written, so reading cost dominates
        correct: true
    explanation: Every change starts with reading and understanding existing code. Making that cheap pays off on every future change.
  - id: qualities
    prompt: Which qualities describe clean code?
    type: multiple
    options:
      - text: It does one thing and its intent is obvious
        correct: true
      - text: It is covered by tests that document its behaviour
        correct: true
      - text: It contains clever one-liners that only experts understand
      - text: It has minimal duplication
        correct: true
    explanation: Clarity, tests and low duplication are recurring themes. Cleverness that hides intent is the opposite of clean.
  - id: tech-debt
    prompt: What is "technical debt"?
    options:
      - id: future-cost
        text: The extra future cost created by choosing an expedient solution now
        correct: true
      - text: Money owed for software licences
      - text: The cost of hiring senior developers
    explanation: Ward Cunningham's metaphor — shortcuts accrue interest in the form of slower, riskier future changes.
---
"Clean code" is not a style guide. It is code whose **intent is obvious** to the next reader — who
is usually you, six months later.

## Learning objectives

- Describe the properties of clean code
- Explain why readability has economic value
- Understand technical debt as a trade-off, not a sin

## Same behaviour, different cost

```csharp
public List<int[]> Get(List<int[]> l)
{
    var r = new List<int[]>();
    foreach (var x in l) if (x[0] == 4) r.Add(x);
    return r;
}
```

```csharp
public IReadOnlyList<Cell> FlaggedCells(IEnumerable<Cell> board) =>
    board.Where(cell => cell.IsFlagged).ToList();
```

Both compile. Only the second tells you what it does. The magic number `4` became a named concept,
the anonymous `int[]` became a `Cell` type, and the name says *why* the method exists.

## Properties of clean code

- **Readable** — reads like well-written prose; names reveal intent
- **Focused** — each function, class and module does one thing
- **Tested** — behaviour is pinned down and documented by tests
- **Unsurprising** — it does what its name says and nothing else
- **Free of duplication** — each piece of knowledge lives in one place

## Technical debt

Ward Cunningham coined the debt metaphor: shipping a quick solution is like borrowing. Debt is
fine when deliberate and repaid. It becomes dangerous when nobody tracks it and the "interest" —
slower, riskier changes — grows until the team spends most of its time working around old
decisions.

> "Any fool can write code that a computer can understand. Good programmers write code that humans
> can understand." — Martin Fowler
