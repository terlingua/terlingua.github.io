---
title: Arguments, side effects and command–query separation
questions:
  - id: cqs
    prompt: What does command–query separation (CQS) recommend?
    options:
      - id: separate
        text: A method should either change state (command) or return information (query), but not both
        correct: true
      - text: Every method should be both a command and a query
      - text: Queries must be asynchronous
      - text: Commands must be stored in a database
    explanation: Bertrand Meyer's CQS makes code predictable — asking a question should never change the answer.
  - id: flag-argument
    prompt: Why are boolean flag arguments like `Render(true)` considered a smell?
    options:
      - id: two-things
        text: They signal that the function does two different things, and the call site is unreadable
        correct: true
      - text: Booleans use more memory than integers
      - text: C# does not allow boolean parameters on public methods
    explanation: Split into two well-named functions, or use an enum or named argument to make the call site self-explanatory.
  - id: hidden-side-effects
    prompt: A method named `CheckPassword` also initialises the user's session. Which problems does this cause?
    type: multiple
    options:
      - text: Callers may start a session accidentally just by validating a password
        correct: true
      - text: The name lies about what the method does
        correct: true
      - text: It creates a temporal coupling where checking must happen at a particular moment
        correct: true
      - text: It improves performance by doing two things at once
    explanation: Hidden side effects make names untrustworthy and couple behaviour to call order.
  - id: many-args
    prompt: A constructor takes eight primitive parameters. What is usually the best refactoring?
    options:
      - text: Make all of them optional
      - text: Replace them with a `Dictionary<string, object>`
      - id: parameter-object
        text: Group related parameters into meaningful types such as `Address` or `DateRange`
        correct: true
    explanation: Parameter objects give names to concepts and make invalid combinations harder to construct.
---
## Learning objectives

- Keep argument lists short and meaningful
- Eliminate hidden side effects
- Apply command–query separation

## Fewer, better arguments

```csharp
// Hard to call correctly
var unclear = new Booking("Ada", "Lovelace", "10 Downing St", "London", "SW1A 2AA",
                          new DateOnly(2026, 12, 1), new DateOnly(2026, 12, 5), 2);

// Concepts made explicit
var booking = new Booking(
    guest: new GuestName("Ada", "Lovelace"),
    address: new Address("10 Downing St", "London", "SW1A 2AA"),
    stay: new DateRange(new DateOnly(2026, 12, 1), new DateOnly(2026, 12, 5)),
    guests: 2);
```

## Flag arguments

```csharp
report.Render(true);                 // what does true mean?
report.RenderForPrint();             // two functions
report.Render(RenderMode.Print);     // or an enum
```

## Side effects

A side effect is a hidden change in state. Make effects visible in names or remove them:

```csharp
public bool CheckPassword(User user, string password)
{
    var ok = _hasher.Verify(user.PasswordHash, password);
    if (ok) _sessions.Initialize(user); // hidden!
    return ok;
}
```

Rename it `CheckPasswordAndStartSession`, or better, let the caller start the session explicitly.

## Command–query separation

```csharp
// Violates CQS: changes state and returns whether the key existed
public bool Set(string key, string value) { /* ... */ return true; }

// Separated
public bool Contains(string key) => _items.ContainsKey(key);
public void Set(string key, string value) => _items[key] = value;
```

Pragmatic exceptions exist — `Stack<T>.Pop` and `Dictionary.TryAdd` are deliberate, well-known
idioms — but your own APIs should default to CQS.
