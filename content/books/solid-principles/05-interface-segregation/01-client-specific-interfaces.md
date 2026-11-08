---
title: Client-specific interfaces
questions:
  - id: definition
    prompt: What does the Interface Segregation Principle state?
    options:
      - text: Every class must implement exactly one interface
      - id: clients
        text: Clients should not be forced to depend on methods they do not use
        correct: true
      - text: Interfaces must live in a separate assembly from their implementations
      - text: Interfaces should never inherit from other interfaces
    explanation: Fat interfaces couple clients to members they never call, so changes to those members ripple into unrelated clients.
  - id: who-owns
    prompt: Who should an interface be shaped around?
    options:
      - text: The class that implements it
      - text: The database schema
      - id: client
        text: The client that uses it
        correct: true
    explanation: Interfaces describe what a client needs. One implementation can satisfy several client-shaped interfaces.
  - id: symptoms
    prompt: Which are symptoms of a fat interface?
    type: multiple
    options:
      - text: Implementations throw `NotImplementedException` for several members
        correct: true
      - text: Test doubles must stub many methods the test never touches
        correct: true
      - text: Most clients call only a small subset of the members
        correct: true
      - text: The interface has a descriptive name
    explanation: Unused members show up as stubs, throwing implementations and clients that only use a slice of the interface.
---
## Learning objectives

- State the interface segregation principle
- Split a fat interface into role interfaces
- Let one class implement several small interfaces

## A fat interface

```csharp
public interface IMultiFunctionDevice
{
    void Print(Document d);
    void Scan(Document d);
    void Fax(Document d);
    void Staple(Document d);
}

public sealed class BasicPrinter : IMultiFunctionDevice
{
    public void Print(Document d) { /* ... */ }
    public void Scan(Document d)   => throw new NotImplementedException();
    public void Fax(Document d)    => throw new NotImplementedException();
    public void Staple(Document d) => throw new NotImplementedException();
}
```

`BasicPrinter` is forced to lie, and every client that only prints still recompiles and redeploys
whenever the fax signature changes.

## Segregated interfaces

```csharp
public interface IPrinter { void Print(Document d); }
public interface IScanner { void Scan(Document d); }
public interface IFax     { void Fax(Document d); }

public sealed class BasicPrinter : IPrinter { public void Print(Document d) { } }
public sealed class OfficeMachine : IPrinter, IScanner, IFax
{
    public void Print(Document d) { }
    public void Scan(Document d) { }
    public void Fax(Document d) { }
}
```

The same `OfficeMachine` instance can be registered under several interfaces:

```csharp
builder.Services.AddSingleton<OfficeMachine>();
builder.Services.AddSingleton<IPrinter>(sp => sp.GetRequiredService<OfficeMachine>());
builder.Services.AddSingleton<IScanner>(sp => sp.GetRequiredService<OfficeMachine>());
```

## ISP and LSP

Fat interfaces tend to cause LSP violations, because implementations throw for members they cannot
support. Segregation removes the temptation: if you cannot fax, you simply do not implement `IFax`.
