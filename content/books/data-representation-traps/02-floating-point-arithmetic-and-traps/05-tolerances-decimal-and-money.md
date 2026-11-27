---
title: "Tolerances, Decimal Types and Money"
questions:
  - id: "isclose-output"
    type: "single"
    prompt: "What does this Python line print?\n\n```python\nimport math\nprint(math.isclose(0.1 + 0.2, 0.3), math.isclose(1e-12, 0.0))\n```"
    explanation: |-
      `math.isclose(a, b)` returns `abs(a - b) <= max(rel_tol * max(abs(a), abs(b)), abs_tol)` with defaults `rel_tol=1e-09` and `abs_tol=0.0`.

      - First call: |0.30000000000000004 − 0.3| ≈ 5.55 × 10\^−17, and the relative bound is 1e-9 × 0.3 = 3 × 10\^−10. 5.55e-17 ≤ 3e-10 → `True`.
      - Second call: the relative bound is 1e-9 × max(1e-12, 0) = 1e-21, and abs_tol is 0. The difference 1e-12 exceeds 1e-21 → `False`. A purely relative tolerance can never call a non-zero value "close to zero"; you must pass `abs_tol` for that.

      Output: `True False`.

      "False False" assumes `isclose` is exact equality.

      "True True" assumes a built-in absolute tolerance; the default `abs_tol` is 0.0.

      "False True" reverses both cases.
    options:
      - text: "False False"
      - text: "True True"
      - text: "True False"
        correct: true
      - text: "False True"
  - id: "absolute-epsilon-at-large-magnitude"
    type: "single"
    prompt: "A reconciliation job compares account balances around `1e9` (in currency units, as doubles) with `Math.Abs(a - b) < 1e-9`. What is the practical effect of this check?"
    explanation: |-
      1e9 lies in [2\^29, 2\^30), so the binary64 ULP there is 2\^(29 − 52) = 2\^−23 ≈ 1.19 × 10\^−7. Two *different* doubles near 1e9 always differ by at least that much, which is larger than 1e-9. So the test passes only when a \== b exactly: the "tolerance" does nothing.

      "It tolerates differences up to one billionth of a unit" ignores that no two distinct values at this magnitude are that close.

      "It is too loose and treats different balances as equal" has it backwards; 1e-9 is far below the spacing, not above it.

      "It throws for values above 2\^53" is wrong: nothing throws, and 1e9 is far below 2\^53 anyway. The fix is a tolerance scaled to the magnitude (relative tolerance) or, for money, exact integer or decimal arithmetic.
    options:
      - text: "It tolerates differences up to one billionth of a unit."
      - text: "It is too loose and treats different balances as equal."
      - text: "It throws for values above 2\\^53."
      - text: "It behaves exactly like `a == b` at this magnitude."
        correct: true
  - id: "csharp-decimal-output"
    type: "single"
    prompt: "What does this C# line print?\n\n```csharp\nConsole.WriteLine($\"{0.1m + 0.2m == 0.3m} {1m / 3m * 3m}\");\n```"
    explanation: |-
      C#'s `decimal` is a 128-bit *base-10* floating-point type: a 96-bit integer coefficient, a sign and a scale of 0–28 (a power of ten). 0.1, 0.2 and 0.3 are exact (coefficients 1, 2, 3 with scale 1), so the sum equals 0.3 exactly: `True`.

      But 1/3 is not a finite decimal fraction. `1m / 3m` rounds to 28 decimal places, 0.3333333333333333333333333333, and multiplying by 3 gives 0.9999999999999999999999999999. Decimal types remove *binary* representation error, not rounding error.

      Output: `True 0.9999999999999999999999999999`.

      "False 1" applies binary64 behavior to `decimal` and assumes decimal is exact for all fractions.

      "True 1" assumes decimal division is exact.

      "False 0.9999999999999999999999999999" assumes decimal also has the binary 0.1 + 0.2 problem.
    options:
      - text: "True 0.9999999999999999999999999999"
        correct: true
      - text: "False 1"
      - text: "True 1"
      - text: "False 0.9999999999999999999999999999"
  - id: "split-a-hundred-three-ways"
    type: "single"
    prompt: "An invoice of 100.00 (two decimal places) must be split into three shares that sum exactly to the total. Working in integer cents, which allocation is correct?"
    explanation: |-
      10000 cents ÷ 3 = 3333 remainder 1. Give every share 3333 cents and distribute the 1 leftover cent to one share (by a deterministic rule such as "first share" or "largest fractional remainder"): 3334 + 3333 + 3333 = 10000 cents → 33.34, 33.33, 33.33.

      "33.33, 33.33, 33.33" sums to 99.99 and silently loses a cent; rounding each share independently never guarantees the total.

      "33.34, 33.34, 33.34" comes from rounding each share upward (ceiling) from 33.333…, which overshoots the total to 100.02.

      "33.333333, 33.333333, 33.333334" uses sub-cent amounts that cannot be paid; the currency's minor unit is 0.01.
    options:
      - text: "33.33, 33.33, 33.33"
      - text: "33.34, 33.33, 33.33"
        correct: true
      - text: "33.34, 33.34, 33.34"
      - text: "33.333333, 33.333333, 33.333334"
  - id: "minor-unit-exponents"
    type: "multiple"
    prompt: "A payment system stores amounts as integers in each currency's minor unit, per the ISO 4217 currency table. Which statements are correct? Select all that apply."
    explanation: |-
      ISO 4217 assigns each currency a number of minor-unit digits (its decimal exponent). Integer minor units are only meaningful together with that exponent.

      Correct:

      - The Japanese yen (JPY) has 0 minor-unit digits: an integer amount of 1999 means ¥1999.
      - The Bahraini dinar (BHD) has 3 minor-unit digits: 1999 minor units means 1.999 BHD.
      - Converting an integer amount to a display value requires the currency's exponent; the integer alone is ambiguous.

      Wrong:

      - "Every currency has exactly two decimal places" is the most common assumption behind multi-currency bugs; JPY (0), BHD (3) and KWD (3) are counterexamples.
      - "1999 minor units always means 19.99 in the major unit" hard-codes an exponent of 2; it is ¥1999 in JPY and 1.999 in BHD.

      This is the one question in this section that depends on a published table that can change (currencies are occasionally redenominated); the arithmetic rule — amount = integer × 10\^−exponent — does not.
    options:
      - text: "JPY has no minor unit, so 1999 means 1999 yen."
        correct: true
      - text: "BHD uses three minor-unit digits."
        correct: true
      - text: "Every currency has exactly two decimal places."
      - text: "1999 minor units always means 19.99 in the major unit."
      - text: "Displaying an amount requires the currency's minor-unit exponent."
        correct: true
  - id: "round-half-even-money"
    type: "single"
    prompt: "What does this Python line print?\n\n```python\nfrom decimal import Decimal\nprint(round(2.675, 2), round(Decimal(\"2.675\"), 2))\n```"
    explanation: |-
      Two different effects:

      - `2.675` as a binary64 literal is 2.67499999999999982236431605997495353221893310546875 (below the decimal halfway point), so `round(2.675, 2)` correctly rounds *down* to `2.67`. It is not a tie at all.
      - `Decimal("2.675")` is exactly 2.675, a true tie between 2.67 and 2.68. Python's `round` on a `Decimal` uses the context's default rounding, ROUND_HALF_EVEN, which picks the even last digit: 2.68 (8 is even, 7 is odd).

      Output: `2.67 2.68`.

      "2.68 2.68" assumes the float literal is exactly 2.675.

      "2.67 2.67" assumes half-even would choose 7; the even neighbor is 8.

      "2.68 2.67" swaps the two effects.

      Banking and accounting rules differ (some require half-up), so money code should state its rounding mode explicitly, for example `Decimal.quantize(Decimal("0.01"), rounding=ROUND_HALF_UP)`.
    options:
      - text: "2.68 2.68"
      - text: "2.67 2.67"
      - text: "2.68 2.67"
      - text: "2.67 2.68"
        correct: true
---

## Learning objectives

- Choose between absolute, relative and ULP-based tolerances for float comparison.
- Explain what decimal floating-point types fix and what they do not.
- Represent money as integer minor units and allocate totals without losing cents.
- Select and state a rounding mode explicitly for financial calculations.

## The invariant

Because every float is the rounded image of a real value, equality is the wrong question for computed results. The right question is "are these within the error I expect?", and the error scales with magnitude:

- **Relative tolerance**: |a − b| ≤ r · max(|a|, |b|). Matches the floating-point error model; meaningless near zero.
- **Absolute tolerance**: |a − b| ≤ t. Right near zero; useless when t is below the ULP of the operands (it degenerates to `==`) or meaningless when t is huge relative to them.
- **Combined**: pass if either bound holds. Python's `math.isclose(a, b, rel_tol=…, abs_tol=…)` and NumPy's `isclose` both combine the two, with different defaults.
- **ULP distance**: count representable values between a and b using their ordered bit patterns; useful in numerical test suites.

Money is different: it is not a measurement but a count of minor units, and the law or contract defines the exact result. Two exact representations exist:

1. **Integers in minor units** (cents, fils, yen) together with the currency's ISO 4217 exponent. Addition and subtraction are exact; multiplication by rates and division require an explicit rounding step and an allocation rule for remainders.
2. **Decimal floating point** (C# `decimal`, Python `decimal.Decimal`, Java `BigDecimal`, SQL `DECIMAL(p, s)`). Base-10 representation makes decimal literals exact, but division, interest and currency conversion still round; the rounding mode must be chosen.

IEEE 754-2008 and 2019 also define decimal interchange formats (decimal64, decimal128); language decimal types implement the same idea with their own layouts, so "decimal" does not imply one standard format.

Rounding modes that matter for money: round-half-even (default in IEEE arithmetic and in Python's `decimal`, and the default of .NET's `Math.Round` and `decimal.Round`) and round-half-up/away-from-zero (common in invoicing and tax rules). They differ only on exact ties, which are frequent with decimal inputs.

## Worked example

Allocate 100.00 across three cost centers in integer cents:

| Step | Value |
|---|---|
| Total in minor units | 10000 |
| Base share | 10000 // 3 = 3333 |
| Remainder | 10000 − 3 × 3333 = 1 |
| Allocation | 3334, 3333, 3333 (one leftover cent to the first share) |
| Check | 3334 + 3333 + 3333 = 10000 |

```python
def allocate(total_minor: int, parts: int) -> list[int]:
    base, rem = divmod(total_minor, parts)
    return [base + (1 if i < rem else 0) for i in range(parts)]

print(allocate(10000, 3))   # [3334, 3333, 3333]
```

```csharp
Console.WriteLine(Math.Round(2.5));                                  // 2   (half-even default)
Console.WriteLine(Math.Round(2.5, MidpointRounding.AwayFromZero));   // 3
Console.WriteLine(0.1m + 0.2m == 0.3m);                              // True
```

## Where this shows up today

As of November 2026, the payment APIs that dominate online commerce accept amounts as integers in the currency's smallest unit and publish per-currency exponent tables, which is the integer-minor-units design above. Bugs still appear when code assumes two decimal places for every currency or parses amounts from JSON into binary doubles before converting.

## Pitfalls checklist

- Using one absolute epsilon for all magnitudes, or a relative one when comparing to zero.
- Storing money in `float`/`double`, or converting through them on the way to `decimal`.
- Rounding each share separately and losing or creating cents on the total.
- Hard-coding two decimal places for every currency.
- Leaving the rounding mode implicit; half-even and half-up differ on every exact tie.

## Key takeaways

- Compare computed floats with relative and/or absolute tolerances scaled to the problem.
- Decimal types make decimal literals exact; they do not make division exact.
- Represent money as integer minor units plus the currency exponent, and allocate remainders explicitly.
- Always name the rounding mode in money code.
