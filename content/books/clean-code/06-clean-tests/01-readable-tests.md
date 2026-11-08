---
title: Readable, trustworthy tests
questions:
  - id: aaa
    prompt: What does the Arrange–Act–Assert pattern describe?
    options:
      - id: structure
        text: Setting up state, performing the single behaviour under test, then verifying the outcome
        correct: true
      - text: The order in which test projects are built
      - text: Three separate test classes for each production class
      - text: A naming convention for test files
    explanation: Clear sections make it obvious what is being tested and what is expected.
  - id: first
    prompt: The FIRST acronym says tests should be Fast, Independent, Repeatable, Self-validating and…
    options:
      - text: Fully mocked
      - text: Fragile
      - id: timely
        text: Timely
        correct: true
    explanation: Timely means tests are written close to — ideally before — the production code.
  - id: smells
    prompt: Which are test smells?
    type: multiple
    options:
      - text: Tests that depend on the order in which they run
        correct: true
      - text: Tests that call `DateTime.Now` and fail around midnight
        correct: true
      - text: Assertions on private implementation details rather than observable behaviour
        correct: true
      - text: A test name that describes the scenario and expected result
    explanation: Order dependence, hidden time dependence and implementation-coupled assertions make tests flaky or brittle. Descriptive names are good.
---
## Learning objectives

- Structure tests with Arrange–Act–Assert
- Name tests after behaviour
- Make tests deterministic

## A clean test

```csharp
public class ShoppingCartTests
{
    [Fact]
    public void Adding_the_same_product_twice_increases_the_quantity()
    {
        // Arrange
        var cart = new ShoppingCart();
        var book = new Product("SKU-1", 12.50m);

        // Act
        cart.Add(book);
        cart.Add(book);

        // Assert
        var line = Assert.Single(cart.Lines);
        Assert.Equal(2, line.Quantity);
    }
}
```

The name reads as a specification. One behaviour per test, and the assertion checks an observable
outcome.

## FIRST

| Letter | Meaning |
|--------|---------|
| Fast | Run in milliseconds so they run constantly |
| Independent | No shared state; any order works |
| Repeatable | Same result on every machine, every time |
| Self-validating | Pass or fail, no manual inspection |
| Timely | Written alongside (or before) the code |

## Controlling time

```csharp
public sealed class TrialService(TimeProvider time)
{
    public bool IsExpired(DateTimeOffset startedAt) => time.GetUtcNow() - startedAt > TimeSpan.FromDays(14);
}

[Fact]
public void Trial_expires_after_fourteen_days()
{
    var time = new FakeTimeProvider(new DateTimeOffset(2026, 1, 15, 0, 0, 0, TimeSpan.Zero));
    var service = new TrialService(time);

    Assert.True(service.IsExpired(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)));
}
```

`FakeTimeProvider` comes from the `Microsoft.Extensions.TimeProvider.Testing` package and lets
tests control the clock.

## Parameterised tests

```csharp
[Theory]
[InlineData("", false)]
[InlineData("short", false)]
[InlineData("correct-horse-battery", true)]
public void Password_strength(string password, bool expected) =>
    Assert.Equal(expected, PasswordPolicy.IsStrong(password));
```
