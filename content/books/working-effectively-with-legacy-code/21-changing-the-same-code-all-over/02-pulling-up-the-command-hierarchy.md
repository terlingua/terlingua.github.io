---
title: "Pulling Up the Command Hierarchy"
questions:
  - id: superclass
    prompt: "Why introduce a `Command` superclass?"
    options:
      - id: o89c544
        text: "Java requires a common base class"
      - id: share-writefield
        text: "Both classes are commands, and a superclass lets `writeField` be pulled up and used by both"
        correct: true
      - id: o24de40
        text: "To add a database dependency"
    explanation: "“Because both classes are commands, we can introduce a superclass for them called Command. When we have it, we can pull writeField up into the superclass so that it can be used in both commands”."
  - id: roughly-same
    prompt: "What rule does Feathers state for methods that look roughly the same?"
    options:
      - id: o9650b6
        text: "Leave them alone"
      - id: extract-diff
        text: "Extract the differences into other methods; often you can then make them identical and remove one"
        correct: true
      - id: ob59514
        text: "Merge them with a boolean flag"
    explanation: "“When two methods look roughly the same, extract the differences to other methods. When you do that, you can often make them exactly the same and get rid of one.”"
  - id: not-yet
    prompt: "Both `write` methods look identical after extracting `writeBody`. Why can't `write` be pulled up immediately?"
    options:
      - id: o4c1d40
        text: "Java forbids pulling up public methods"
      - id: data
        text: "They use class-specific data — `header`, `footer`, `commandChar` — which must first move up or be reached through methods"
        correct: true
      - id: o373d38
        text: "The tests would fail"
    explanation: "“Even though both writes look the same, they use data from their classes: header, footer, and commandChar. If we are going to try to make a single write method, it would have to call methods from the subclasses to get that data.”"
  - id: commandchar
    prompt: "How is the differing `commandChar` handled?"
    options:
      - id: abstract-getter
        text: "An abstract `getCommandChar()` on `Command`, overridden in each subclass"
        correct: true
      - id: o9bd78a
        text: "A static field in `Command`"
      - id: o5216ad
        text: "A constructor parameter for every client"
    explanation: "“One simple way of handling this is to introduce an abstract getter on the Command class” and “we can replace the commandChar variables on each subclass with a getCommandChar override”."
  - id: getsize
    prompt: "What difference is extracted from the two `getSize` methods?"
    options:
      - id: ocb25cc
        text: "The footer bytes"
      - id: o147984
        text: "The header length"
      - id: body-size
        text: "The size of the fields — `getBodySize()` — leaving the bookkeeping sizes identical"
        correct: true
    explanation: "“What if we extract what is computed differently: the size of the fields? We call the resulting method getBodySize().” Then getSize can move up."
  - id: zealous
    prompt: "What final generalisation removes the remaining duplication?"
    options:
      - id: ob10bf4
        text: "Reflection over subclass fields"
      - id: fields-list
        text: "A `fields` list in the base class that subclass constructors populate, used by generic `getBodySize` and `writeBody`"
        correct: true
      - id: ofc92ff
        text: "A code generator"
    explanation: "“If we declare a list in the base class, we can add to it in each subclass constructor” — “When we add to the fields list in each subclass, we can use the same code to get the body size”."
  - id: keep-subclasses
    prompt: "With subclasses now tiny shells, why keep them rather than static `Command.send` methods?"
    type: multiple
    options:
      - id: clients
        text: "Static methods would force client code — which constructs these objects in many places — to change"
        correct: true
      - id: o7e3aeb
        text: "Static methods are illegal in Java"
      - id: command-chars
        text: "Clients shouldn't have to track command chars"
        correct: true
    explanation: "“One thing is for sure: We do have to send two different command chars, and we don’t want the user to have to keep track of them.” and “that would force all of our client code to change.” — “Sure, the subclasses are pretty tiny, but does that really hurt anything? Not really.”"
  - id: rename
    prompt: "What small change \"should've been done earlier\"?"
    options:
      - id: o1028b1
        text: "Make `Command` final"
      - id: consistent-names
        text: "Rename `AddEmployeeCmd` to `AddEmployeeCommand` for consistency"
        correct: true
      - id: o6e03a5
        text: "Remove `LoginCommand`"
    explanation: "“We can rename AddEmployeeCmd to AddEmployeeCommand. That would make the names of the two subclasses consistent. We’re less likely to be wrong when we use names consistently.”"
  - id: abbreviations
    prompt: "What went wrong in the team that abbreviated \"manager\"?"
    options:
      - id: o565253
        text: "The compiler rejected abbreviations"
      - id: inconsistent
        text: "Inconsistent abbreviations (`Mgr`, `Mngr`) meant you usually had to look names up — Feathers guessed wrong over half the time"
        correct: true
      - id: o80f38c
        text: "Names were too long"
    explanation: "“some classes were named XXXXMgr, and others were named XXXXMngr.” — “More than 50 percent of the time, I was wrong when I attempted to guess which suffix was used for a particular class”."
---
## Learning objectives

- Pull shared behaviour into a base class step by step
- Use abstract hooks for differing data
- Judge when tiny subclasses are fine

## Connections to today

```csharp
public abstract class Command
{
    private static readonly byte[] Header = [0xde, 0xad];
    private static readonly byte[] Footer = [0xbe, 0xef];
    protected List<string> Fields { get; } = [];
    protected abstract byte CommandChar { get; }
    protected virtual void WriteBody(Stream output) { foreach (var f in Fields) WriteField(output, f); }
    private static void WriteField(Stream output, string field) { output.Write(Encoding.ASCII.GetBytes(field)); output.WriteByte(0); }
}
```

Today you might prefer composition (a `CommandWriter` plus data records) over inheritance, but the
duplication-driven discovery of "field" and "body" concepts is the same.
