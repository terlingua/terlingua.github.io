---
title: Test-driven development
questions:
  - id: cycle
    prompt: What is the order of the TDD cycle?
    options:
      - text: Green, red, refactor
      - id: red-green-refactor
        text: Red, green, refactor
        correct: true
      - text: Refactor, red, green
      - text: Write all tests, then write all code
    explanation: Write a failing test (red), make it pass with the simplest code (green), then improve the design with tests as a safety net (refactor).
  - id: doubles
    prompt: What is the difference between a stub and a mock?
    options:
      - id: stub-mock
        text: A stub supplies canned answers; a mock also verifies that expected interactions happened
        correct: true
      - text: There is no difference
      - text: Mocks are only used for databases
    explanation: Prefer stubs and fakes for most tests; use mocks when the interaction itself is the behaviour you care about.
  - id: benefits
    prompt: Which are benefits of TDD?
    type: multiple
    options:
      - text: It pushes toward small, decoupled units that are easy to test
        correct: true
      - text: It produces a regression suite as a side effect
        correct: true
      - text: It forces you to think about the API from the caller's point of view first
        correct: true
      - text: It guarantees the software has no bugs
    explanation: TDD improves design and gives confidence, but tests can only find bugs that someone thought to test.
---
## Learning objectives

- Practice the red–green–refactor cycle
- Choose between stubs, fakes and mocks
- Understand what TDD does and does not guarantee

## Red, green, refactor

1. **Red** — write a test for behaviour that does not exist yet; watch it fail
2. **Green** — write the simplest code that makes it pass
3. **Refactor** — clean up duplication and naming with the tests protecting you

```csharp
[Fact]
public void Empty_string_returns_zero() => Assert.Equal(0, StringCalculator.Add(""));
```

```csharp
public static class StringCalculator
{
    public static int Add(string numbers) => 0;
}
```

The next test forces real behaviour:

```csharp
[Theory]
[InlineData("1", 1)]
[InlineData("1,2", 3)]
[InlineData("4,5,6", 15)]
public void Sums_comma_separated_numbers(string input, int expected) =>
    Assert.Equal(expected, StringCalculator.Add(input));
```

```csharp
public static int Add(string numbers) =>
    string.IsNullOrEmpty(numbers) ? 0 : numbers.Split(',').Sum(int.Parse);
```

## Test doubles

| Double | Purpose |
|--------|---------|
| Dummy | Fills a parameter, never used |
| Stub | Returns canned answers |
| Fake | A working, simplified implementation (in-memory repository) |
| Spy | Records calls for later assertions |
| Mock | Pre-programmed with expectations it verifies |

Over-mocking couples tests to implementation details. A hand-written fake is often clearer:

```csharp
public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<int, Order> _orders = [];
    public Task AddAsync(Order order, CancellationToken ct) { _orders[order.Id] = order; return Task.CompletedTask; }
    public Task<Order?> FindAsync(int id, CancellationToken ct) => Task.FromResult(_orders.GetValueOrDefault(id));
}
```

## What TDD does not do

TDD does not prove correctness, and it does not replace integration tests, exploratory testing or
thinking. It is a design and feedback technique that leaves a useful test suite behind.
