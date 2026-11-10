---
title: xUnit.net v3 (and 4.x)
questions:
  - id: package
    prompt: Which package do you reference for a new xUnit.net v3 test project?
    options:
      - text: "`xunit` 2.9.x"
      - id: v3
        text: "`xunit.v3` (or a variant such as `xunit.v3.mtp-v2` that pins the MTP version)"
        correct: true
      - text: "`xunit.core.vstest`"
      - text: "`Microsoft.NET.Test.Sdk` only"
    explanation: v3 has new package ids. The `xunit` package id is the v2 line.
  - id: testcontext
    prompt: What does `TestContext.Current` provide in xUnit v3?
    type: multiple
    options:
      - id: token
        text: A `CancellationToken` for the current test
        correct: true
      - id: output
        text: Access to the test output helper and test metadata
        correct: true
      - id: key-value
        text: A `KeyValueStorage` and attachments for sharing data and artifacts
        correct: true
      - text: The production `IServiceProvider`
    explanation: It is ambient and available anywhere in the test call stack — no need to inject `ITestOutputHelper` just to get a token or log.
  - id: lifetime
    prompt: What changed about `IAsyncLifetime` in v3?
    options:
      - text: It was removed
      - text: It became synchronous
      - id: valuetask
        text: "`InitializeAsync` returns `ValueTask`, and the interface derives from `IAsyncDisposable` so cleanup is `DisposeAsync()`"
        correct: true
      - text: It now requires a constructor parameter
    explanation: Migrating from v2 means changing `Task InitializeAsync()` to `ValueTask InitializeAsync()` and `Task DisposeAsync()` to `ValueTask DisposeAsync()`.
  - id: dynamic-skip
    prompt: How do you skip a test at runtime when, say, Docker is not available?
    options:
      - text: Throw `SkipException` from a static helper (v2 style only)
      - id: assert-skip
        text: Call `Assert.Skip("reason")`, or use `Assert.SkipUnless(condition, "reason")` / `Assert.SkipWhen(...)`
        correct: true
      - text: Return early, which reports the test as skipped
      - text: Dynamic skipping is impossible
    explanation: Returning early reports a *pass*, which is a lie. v3 makes dynamic skipping first-class.
  - id: explicit
    prompt: "What does `[Fact(Explicit = true)]` do?"
    options:
      - text: The test must use explicit types instead of `var`
      - text: The test always runs first
      - id: only-when-asked
        text: The test runs only when explicitly requested (for example by name or with an "explicit" option), not in normal runs
        correct: true
      - text: The test is excluded from coverage
    explanation: Useful for slow diagnostics or tests against real external systems you run by hand.
  - id: assembly-fixture
    prompt: How do you share one expensive fixture (such as a database container) across every test class in an assembly in v3?
    options:
      - text: A static field initialized in each constructor
      - id: assembly-fixture
        text: "Declare `[assembly: AssemblyFixture(typeof(DatabaseFixture))]` and accept `DatabaseFixture` in test class constructors"
        correct: true
      - text: "`[Collection]` on every class is the only way"
      - text: It is not possible
    explanation: Assembly fixtures are new in v3. Collection fixtures still exist when you need a subset of classes to share something.
  - id: matrix
    prompt: What does `MatrixTheoryData<T1, T2>` produce from `[1, 2]` and `["a", "b"]`?
    options:
      - text: Two rows, `(1, "a")` and `(2, "b")`
      - text: One row containing both arrays
      - text: A compile error
      - id: product
        text: Four rows — the Cartesian product `(1,"a"), (1,"b"), (2,"a"), (2,"b")`
        correct: true
    explanation: Matrix data generates combinations, ideal for exhaustive small-space testing.
  - id: v4
    prompt: Which are notable in xUnit.net 4.0 (August 2026)?
    type: multiple
    options:
      - id: aot
        text: Native AOT support via new `xunit.v3.core.aot*` packages
        correct: true
      - id: parallel
        text: Full test-case-level parallelization with opt-outs at collection, class, method and data levels
        correct: true
      - id: dropped
        text: Removal of Microsoft Testing Platform v1 support and of official Mono support
        correct: true
      - text: A return to VSTest as the only runner
    explanation: "4.0 also obsoleted `CollectionBehavior` parallel properties in favor of `[assembly: Parallelization(...)]`, added generic attributes on .NET 8+, and a `strict` option on `Assert.All`."
---
## Learning objectives

- Set up and write tests with xUnit.net v3
- Use `TestContext`, async lifetimes, dynamic skips, explicit tests and assembly fixtures
- Know what changed in 4.0

## A complete small test project

```csharp
public sealed class DiscountCalculatorTests
{
    [Theory]
    [InlineData(99.99, 0)]
    [InlineData(100.00, 5)]
    [InlineData(1000.00, 100)]
    public void Discount_depends_on_order_total(decimal total, decimal expected)
    {
        var discount = DiscountCalculator.For(total);
        Assert.Equal(expected, discount);
    }

    [Fact]
    public async Task Discount_service_respects_cancellation()
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
        await cts.CancelAsync();

        var sut = new DiscountService(new SlowRulesStore());

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => sut.GetAsync("SPRING", cts.Token));
    }
}
```

## Strongly typed theory data

```csharp
public static TheoryData<string, bool> Emails => new()
{
    { "ada@example.org", true },
    { "not-an-email", false },
    { "", false },
};

[Theory]
[MemberData(nameof(Emails))]
public void Validates_email(string input, bool expected) =>
    Assert.Equal(expected, EmailAddress.TryParse(input, out _));

public static MatrixTheoryData<Currency, RoundingMode> Combinations =>
    new([Currency.Usd, Currency.Eur, Currency.Jpy], [RoundingMode.Bankers, RoundingMode.AwayFromZero]);
```

## Fixtures

```csharp
[assembly: AssemblyFixture(typeof(DatabaseFixture))]

public sealed class OrderQueriesTests(DatabaseFixture db)
{
    [Fact]
    public async Task Finds_open_orders()
    {
        var ct = TestContext.Current.CancellationToken;
        var queries = new OrderQueries(db.DataSource);
        var open = await queries.OpenForCustomerAsync(SeedData.CustomerWithTwoOpenOrders, ct);
        Assert.Equal(2, open.Count);
    }
}
```

## Dynamic skip and explicit tests

```csharp
[Fact]
public void Uses_gpu_acceleration()
{
    Assert.SkipUnless(Gpu.IsAvailable, "No GPU on this agent");
    Assert.True(ImagePipeline.Create().IsAccelerated);
}

[Fact(Explicit = true)]
public async Task Smoke_test_against_real_payment_sandbox()
{
    var client = PaymentSandbox.CreateClient();
    var result = await client.PingAsync(TestContext.Current.CancellationToken);
    Assert.True(result.Ok);
}
```

## Output without injection

```csharp
TestContext.Current.TestOutputHelper?.WriteLine($"Seeded {count} rows");
```

## Migrating from v2: the checklist

- Package `xunit` → `xunit.v3`; test project becomes `OutputType Exe`
- `IAsyncLifetime` methods return `ValueTask`
- Replace "return early" skips with `Assert.Skip*`
- Pass `TestContext.Current.CancellationToken` (analyzer xUnit1051 points out where)
- `xunit.runner.json` → `testconfig.json` under MTP
- Remove workarounds for features v3 now has (assembly fixtures, culture, explicit tests)

## Sources

- xUnit.net, "What's new in v3" — <https://xunit.net/docs/getting-started/v3/whats-new>
- xUnit.net, "Migrating from v2 to v3" — <https://xunit.net/docs/getting-started/v3/migration>
- xUnit.net, "Release notes 4.0.0" — <https://xunit.net/releases/v3/4.0.0>
- xUnit.net, "Running tests in parallel" — <https://xunit.net/docs/running-tests-in-parallel>
