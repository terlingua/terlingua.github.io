---
title: "NaN, Signed Zero and Infinity"
questions:
  - id: "nan-equality-javascript"
    type: "single"
    prompt: "What does this JavaScript line print?\n\n```javascript\nconst x = NaN;\nconsole.log(x === x, x !== x, [NaN].includes(NaN), [NaN].indexOf(NaN));\n```"
    explanation: |-
      IEEE 754 defines comparisons involving a NaN as *unordered*: `==`, `<`, `>`, `<=`, `>=` are false and `!=` is true, even when both operands are the same NaN. JavaScript's `===` and `!==` follow that rule, so `x === x` is `false` and `x !== x` is `true`.

      The two array methods use different equality algorithms defined by ECMAScript: `indexOf` uses strict equality (so it never finds NaN and returns `-1`), while `includes` uses SameValueZero, which treats NaN as equal to NaN (so it returns `true`).

      Output: `false true true -1`.

      "true false true 0" assumes a value always equals itself, which IEEE deliberately breaks for NaN.

      "false true false -1" assumes `includes` uses `===` like `indexOf`.

      "false true true 0" assumes `indexOf` and `includes` share one equality rule.
    options:
      - text: "true false true 0"
      - text: "false true true -1"
        correct: true
      - text: "false true false -1"
      - text: "false true true 0"
  - id: "sort-with-nan"
    type: "single"
    prompt: "What does Python return for `sorted([3.0, float('nan'), 1.0, 2.0])`?"
    explanation: |-
      `sorted` relies only on `<`, and it assumes `<` is a strict weak ordering. Every comparison involving NaN is false, which breaks that assumption, so the output is unspecified garbage rather than an error.

      Trace: CPython's sort first looks for an already-ordered run. It checks whether `nan < 3.0` (false, so the run is "ascending"), then `1.0 < nan` (false, still ascending), then `2.0 < 1.0` (false, still ascending). The whole list is taken as one ascending run and returned unchanged: `[3.0, nan, 1.0, 2.0]`.

      "[1.0, 2.0, 3.0, nan]" assumes NaN sorts last, which some APIs implement explicitly (for example `Array.Sort` on `double[]` in .NET uses a total order that puts NaN first), but a comparison-based sort using `<` does not.

      "[nan, 1.0, 2.0, 3.0]" likewise assumes a defined position for NaN.

      "It raises TypeError" confuses NaN with `None`; NaN is a float, so comparisons are allowed and simply return false.
    options:
      - text: "[1.0, 2.0, 3.0, nan]"
      - text: "[nan, 1.0, 2.0, 3.0]"
      - text: "[3.0, nan, 1.0, 2.0]"
        correct: true
      - text: "It raises TypeError."
  - id: "negative-zero-output"
    type: "single"
    prompt: "What does this JavaScript line print?\n\n```javascript\nconsole.log(0 === -0, 1 / -0, Object.is(0, -0));\n```"
    explanation: |-
      IEEE 754 has two zeros, +0 (`0x0000…`) and −0 (`0x8000…`). Comparison treats them as equal, so `0 === -0` is `true`. Division keeps the sign: 1 / (−0) is −infinity under the standard's sign rules for division by zero. `Object.is` implements SameValue, which distinguishes the two zeros, so it returns `false`.

      Output: `true -Infinity false`.

      "false -Infinity false" assumes equality compares bit patterns; IEEE comparison deliberately treats ±0 as equal.

      "true Infinity true" assumes −0 is just 0; the sign survives division.

      "true NaN false" confuses 1 / 0 with 0 / 0; only 0/0 (and similar indeterminate forms) produce NaN.
    options:
      - text: "false -Infinity false"
      - text: "true Infinity true"
      - text: "true NaN false"
      - text: "true -Infinity false"
        correct: true
  - id: "which-yields-nan"
    type: "single"
    prompt: "Under IEEE 754 default rules (no exceptions trapped), which expression produces NaN?"
    explanation: |-
      IEEE 754-2019 defines a set of *invalid operations* that have no meaningful result and return a quiet NaN: ∞ − ∞ (and ∞ + (−∞)), 0 × ∞, 0/0, ∞/∞, square root of a negative number, and similar. ∞ − ∞ is in that list.

      Trace the others:

      - 1 / 0 is *division by zero* for a finite non-zero dividend: the result is an exactly signed infinity, +∞ here.
      - ∞ + 1 is +∞: infinity absorbs any finite addend.
      - −1 / ∞ is −0: a finite number divided by infinity is a zero with the XOR of the operand signs.
    options:
      - text: "∞ − ∞"
        correct: true
      - text: "1 / 0"
      - text: "∞ + 1"
      - text: "−1 / ∞"
  - id: "division-by-zero-by-language"
    type: "multiple"
    prompt: "Which of these evaluate to a value **without** raising an exception? Select all that apply."
    explanation: |-
      IEEE 754's default for division by zero and invalid operations is to return ±∞ or NaN and set a status flag. Languages decide whether to expose that default or to raise. These are language-specific behaviors, not IEEE rules.

      Correct:

      - C# `1.0 / 0.0` follows IEEE defaults and yields `double.PositiveInfinity`.
      - JavaScript `1 / 0` yields `Infinity`; JavaScript numbers never raise on arithmetic.
      - Python `math.inf - math.inf` yields `nan`; Python returns IEEE results for operations on infinities.

      Wrong:

      - Python `1.0 / 0.0` raises `ZeroDivisionError: float division by zero`. Python checks for a zero divisor before dividing, even for floats.
      - C# integer division `1 / zero` with `int zero = 0` throws `DivideByZeroException`. Integers have no infinity, so there is no IEEE result to return.
    options:
      - text: "C# `1.0 / 0.0`"
        correct: true
      - text: "Python `1.0 / 0.0`"
      - text: "C# `1 / zero` where `int zero = 0`"
      - text: "JavaScript `1 / 0`"
        correct: true
      - text: "Python `math.inf - math.inf`"
        correct: true
  - id: "max-with-nan"
    type: "single"
    prompt: "What does this Python line print?\n\n```python\nnan = float(\"nan\")\nprint(max(nan, 1.0), max(1.0, nan))\n```"
    explanation: |-
      Python's built-in `max` keeps the first element and replaces it only when a later element compares greater. Comparisons with NaN are false.

      - `max(nan, 1.0)`: starts with `nan`; is `1.0 > nan`? False, so `nan` stays. Result `nan`.
      - `max(1.0, nan)`: starts with `1.0`; is `nan > 1.0`? False, so `1.0` stays. Result `1.0`.

      Output: `nan 1.0`. The result depends on argument order, which is why NaN-aware code must decide explicitly whether NaN wins (IEEE 754-2019 `maximum`) or loses (`maximumNumber`).

      "nan nan" assumes NaN always propagates, as IEEE 754-2019 `maximum` and .NET's `Math.Max` do; Python's comparison-based `max` does not.

      "1.0 1.0" assumes NaN is always ignored, as C's `fmax` does.

      "It raises ValueError" is wrong: comparing floats never raises in Python.
    options:
      - text: "nan nan"
      - text: "1.0 1.0"
      - text: "nan 1.0"
        correct: true
      - text: "It raises ValueError."
---

## Learning objectives

- Apply IEEE 754 comparison rules to NaN, ±0 and ±∞.
- List the invalid operations that produce NaN and the operations that produce infinities or signed zeros.
- Explain how NaN breaks sorting, `min`/`max` and hash-based containers.
- Distinguish IEEE behavior from language-specific behavior such as raising on division by zero.

## The invariant

IEEE 754-2019 closes floating-point arithmetic over special values so that a computation always produces *some* result and records problems in status flags instead of halting:

| Operation | Result | Exception flag |
|---|---|---|
| finite ≠ 0 divided by ±0 | ±∞ (sign = XOR of operand signs) | division by zero |
| result too large | ±∞ (default rounding) | overflow |
| ∞ − ∞, 0 × ∞, 0/0, ∞/∞, √(negative) | quiet NaN | invalid |
| any arithmetic with a quiet NaN operand | quiet NaN (payload preserved where possible) | none |
| finite / ±∞ | ±0 | none |

Comparisons follow a separate rule: every pair of values is either less, equal, greater or **unordered**, and a pair is unordered exactly when at least one operand is NaN. For unordered pairs `<`, `<=`, `>`, `>=` and `==` are false and `!=` is true. Hence `x != x` is the portable NaN test, and +0 \== −0 is true even though the bits differ.

Consequences that hold in every language:

- **NaN poisons ordering.** Sorting, binary search, `min`/`max`, priority queues and B-tree keys assume a strict weak ordering; a single NaN can silently corrupt their results.
- **NaN breaks hash equality.** A container that uses `==` will never find a NaN key; one that uses a bitwise or "total order" equality will find only an identical payload.
- **Signed zero is observable.** Division, `atan2`, `copysign`, logarithms of zero and string formatting reveal the sign, even though `==` hides it.

IEEE 754-2019 also defines `totalOrder`, which orders −NaN < −∞ < … < −0 < +0 < … < +∞ < +NaN, and the operations `maximum`/`minimum` (NaN propagates) and `maximumNumber`/`minimumNumber` (NaN is ignored). Libraries choose among these; you must know which one you are calling.

## Worked example

```csharp
double nan = double.NaN;
Console.WriteLine(nan == nan);          // False  (IEEE comparison)
Console.WriteLine(nan.Equals(nan));     // True   (.NET Equals: reflexive for hashing)
Console.WriteLine(1.0 / -0.0);          // -Infinity (or -∞, depending on culture)
Console.WriteLine(Math.Max(nan, 1.0));  // NaN    (Math.Max propagates NaN)
```

```python
import math
nan = float("nan")
print(nan == nan, math.isnan(nan))       # False True
print(math.copysign(1.0, -0.0))          # -1.0
print({nan: 1}.get(float("nan")))        # None: a different NaN object, and nan != nan
```

The C# `Equals` result shows a deliberate language-level exception: .NET makes `Equals` reflexive for NaN so that dictionaries and `List.Contains` can find NaN, while `==` keeps IEEE semantics. Python's `dict` first checks object identity, so the *same* NaN object is found, but a freshly created NaN is not.

## Where this shows up today

As of November 2026, standard JSON (RFC 8259) still has no representation for NaN or infinities; Python's `json.dumps` emits the non-standard tokens `NaN` and `Infinity` unless `allow_nan=False`, while JavaScript's `JSON.stringify` writes `null`. Data pipelines that move floats between these runtimes regularly turn NaN into nulls or parse errors at the boundary.

## Pitfalls checklist

- Testing for NaN with `x == NaN`; use `isnan`, `Double.IsNaN`, `Number.isNaN` or `x != x`.
- Sorting or deduplicating data that may contain NaN without a NaN-aware comparator.
- Relying on argument order in `max`/`min` when NaN may appear.
- Assuming `0.0 == -0.0` means the two are interchangeable in division, formatting or `atan2`.
- Assuming every language returns ∞ for `1.0 / 0.0`; Python raises.

## Key takeaways

- NaN is unordered: all comparisons are false except `!=`, including NaN with itself.
- Invalid operations (∞ − ∞, 0 × ∞, 0/0) give NaN; division of a non-zero finite by zero gives a signed infinity.
- +0 and −0 compare equal but behave differently in division and sign-sensitive functions.
- Language and library rules (`Equals`, `includes`, `max`, raising on division) layer on top of IEEE; check which you are using.
