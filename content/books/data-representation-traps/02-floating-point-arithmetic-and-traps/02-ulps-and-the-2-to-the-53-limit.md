---
title: "ULPs and the 2^53 Limit"
questions:
  - id: "ulp-at-ten-to-sixteen"
    type: "single"
    prompt: "What is the gap between `1e16` and the next larger binary64 value?"
    explanation: |-
      The spacing (one ULP, unit in the last place) of binary64 in the binade [2\^e, 2\^(e+1)) is 2\^(e − 52). 1e16 lies between 2\^53 ≈ 9.007 × 10\^15 and 2\^54 ≈ 1.801 × 10\^16, so e = 53 and the ULP is 2\^(53 − 52) = 2. The next double after 1e16 is 10000000000000002.

      "1" is the spacing in [2\^52, 2\^53); it would apply to numbers below about 9.007 × 10\^15.

      "4" is the spacing in [2\^54, 2\^55), one binade too high.

      "2.22e-16" is the machine epsilon 2\^−52, the ULP of 1.0. The ULP is relative to the magnitude, so it grows with the value.
    options:
      - text: "1"
      - text: "4"
      - text: "2.22e-16"
      - text: "2"
        correct: true
  - id: "first-unrepresentable-integer"
    type: "single"
    prompt: "What is the smallest positive integer that binary64 **cannot** represent exactly?"
    explanation: |-
      binary64 has 53 bits of precision, so every integer with magnitude up to 2\^53 fits in the significand exactly. 2\^53 = 9007199254740992 itself is representable (1.0 × 2\^53). Above it the ULP becomes 2, so odd integers are skipped; the first is 2\^53 + 1 = 9007199254740993, which rounds (tie, to even) back to 9007199254740992.

      "9007199254740992" is 2\^53, which is exactly representable; JavaScript's `Number.MAX_SAFE_INTEGER` is 2\^53 − 1 because 2\^53 is the last integer whose successor is also exact, not because 2\^53 is unrepresentable.

      "4503599627370497" = 2\^52 + 1 is representable: in [2\^52, 2\^53) the ULP is exactly 1.

      "18446744073709551616" = 2\^64 is a power of two and therefore representable exactly; it is the 64-bit integer range, unrelated to the 53-bit significand.
    options:
      - text: "9007199254740993"
        correct: true
      - text: "9007199254740992"
      - text: "4503599627370497"
      - text: "18446744073709551616"
  - id: "float-counter-stalls"
    type: "single"
    prompt: "What does this C# snippet print?\n\n```csharp\nfloat count = 16_777_216f;\ncount += 1f;\nConsole.WriteLine(count == 16_777_216f);\n```"
    explanation: |-
      16,777,216 = 2\^24. binary32 has 24 bits of precision, so in [2\^24, 2\^25) the ULP is 2\^(24 − 23) = 2. The exact sum 2\^24 + 1 is halfway between 2\^24 and 2\^24 + 2; ties-to-even picks 2\^24 (significand ends in 0). `count` does not change, and the comparison prints `True`. A loop that counts events in a `float` silently stops counting at 16,777,216.

      "False" assumes every integer increment is exact, which only holds up to 2\^24 in binary32 (2\^53 in binary64).

      "It throws an OverflowException" confuses floats with checked integer arithmetic. 2\^24 is nowhere near the binary32 maximum of about 3.4 × 10\^38, and IEEE arithmetic does not throw.

      "It does not compile" is wrong: digit separators in float literals are valid C#.
    options:
      - text: "False"
      - text: "True"
        correct: true
      - text: "It throws an OverflowException."
      - text: "It does not compile."
  - id: "ulp-of-unix-seconds"
    type: "single"
    prompt: "A service stores the current Unix time in seconds (about `1.76e9`) as a binary64 `double`. What is the finest time step it can represent at that magnitude?"
    explanation: |-
      1.76 × 10\^9 lies in [2\^30, 2\^31) = [1,073,741,824, 2,147,483,648), so e = 30 and the ULP is 2\^(30 − 52) = 2\^−22 s ≈ 2.384 × 10\^−7 s ≈ 238 ns. Timestamps closer together than that collapse to the same double; microsecond values survive, nanosecond values do not.

      "About 0.22 ns" applies machine epsilon (2\^−52 ≈ 2.2 × 10\^−16) as an absolute step; the step must be scaled by the magnitude.

      "About 0.95 µs" is 2\^−20 s, the ULP two binades higher (values in [2\^32, 2\^33) seconds), which this timestamp has not reached.

      "128 seconds" is the ULP at this magnitude in **binary32** (2\^(30 − 23) = 2\^7), the right reason never to store epoch seconds in a `float`.
    options:
      - text: "About 0.22 ns"
      - text: "About 0.95 µs"
      - text: "About 238 ns"
        correct: true
      - text: "128 seconds"
  - id: "ulp-properties"
    type: "multiple"
    prompt: "Which statements about binary64 spacing are correct? Select all that apply."
    explanation: |-
      Correct:

      - The ULP doubles at every power of two: within [2\^e, 2\^(e+1)) it is constant 2\^(e − 52), and in the next binade it is 2\^(e − 51).
      - Relative spacing stays within a factor of two: ULP(x)/x is between 2\^−53 and 2\^−52 for normal x (largest just above a power of two, smallest just below the next).
      - In [2\^53, 2\^54) only even integers exist, because the ULP there is 2.

      Wrong:

      - "The ULP is the same at every magnitude" describes fixed-point arithmetic, not floating point. The absolute spacing at 1.0 is 2\^−52 and at 1e16 it is 2.
      - "Every 64-bit integer converts to a double exactly" ignores the 53-bit significand; above 2\^53, most 64-bit integers round.
    options:
      - text: "The ULP is the same at every magnitude."
      - text: "The ULP doubles at every power of two."
        correct: true
      - text: "Relative spacing ULP(x)/x stays between 2\\^−53 and 2\\^−52 for normal x."
        correct: true
      - text: "Between 2\\^53 and 2\\^54 only even integers are representable."
        correct: true
      - text: "Every 64-bit integer converts to a double exactly."
  - id: "json-large-id"
    type: "single"
    prompt: "An API returns `{\"id\": 9007199254740993}`. What does this JavaScript print?\n\n```javascript\nconsole.log(JSON.parse('{\"id\": 9007199254740993}').id);\n```"
    explanation: |-
      ECMAScript's `JSON.parse` turns every JSON number into a `Number`, which is IEEE 754 binary64. 9007199254740993 = 2\^53 + 1 is not representable; it is exactly halfway between 2\^53 and 2\^53 + 2, and ties-to-even picks 2\^53. The ID silently becomes 9007199254740992, which can be another record's ID.

      "9007199254740993" would need a 54-bit significand. JSON text itself (RFC 8259) has no size limit on numbers, but the JavaScript parser maps them to binary64.

      "9007199254740994" is the other neighbor; it loses the tie because its significand is odd.

      "It throws a SyntaxError" is wrong: the JSON is valid; precision loss is silent. The robust fixes are to send IDs as strings or to parse with a library that preserves big integers.
    options:
      - text: "9007199254740992"
        correct: true
      - text: "9007199254740993"
      - text: "9007199254740994"
      - text: "It throws a SyntaxError."
---

## Learning objectives

- Compute the ULP of a binary64 or binary32 value from its magnitude.
- State the largest range of exactly representable consecutive integers in each format.
- Predict when increments, counters and timestamps stop changing.
- Explain why large integer IDs lose precision when parsed as binary64.

## The invariant

Within a binade [2\^e, 2\^(e+1)) a binary format with precision p has evenly spaced values. The gap between neighbors is the **unit in the last place**:

ULP(x) = 2\^(e − p + 1), i.e. 2\^(e − 52) for binary64 and 2\^(e − 23) for binary32.

Consequences:

- **Absolute spacing grows with magnitude; relative spacing does not.** ULP(x)/x stays between 2\^−p and 2\^(1−p). The worst-case relative rounding error of one correctly rounded operation is half an ULP, the *unit roundoff* u = 2\^−53 for binary64.
- **Integers are exact up to 2\^p.** Every integer with |n| ≤ 2\^53 is a binary64 value (2\^24 for binary32). Above that, the ULP is 2, then 4, then 8, doubling at each power of two; most integers round.
- **Adding something smaller than half an ULP does nothing.** If |y| < ULP(x)/2, then x + y rounds back to x; at exactly half an ULP the tie goes to the even neighbor.

| Magnitude | binary64 ULP | binary32 ULP |
|---|---|---|
| 1.0 | 2\^−52 ≈ 2.2 × 10\^−16 | 2\^−23 ≈ 1.2 × 10\^−7 |
| 2\^24 = 16,777,216 | 2\^−28 | 2 |
| 1.76 × 10\^9 (Unix seconds) | 2\^−22 ≈ 238 ns | 128 s |
| 2\^53 ≈ 9.0 × 10\^15 | 2 | 2\^30 |
| 1 × 10\^16 | 2 | 2\^30 |

This is why 64-bit integer IDs, nanosecond timestamps and large monetary totals in minor units do not belong in a `double`, and why a `float` cannot even count past 16,777,216.

## Worked example

Find what happens to 2\^53 + 1 and 2\^53 + 3 in binary64. Near 2\^53 the ULP is 2:

| Exact value | Neighbors | Distance | Result |
|---|---|---|---|
| 2\^53 + 1 | 2\^53 and 2\^53 + 2 | tie | 2\^53 (significand 2\^52, even) |
| 2\^53 + 3 | 2\^53 + 2 and 2\^53 + 4 | tie | 2\^53 + 4 (significand 2\^52 + 2, even) |

```python
import math
print(int(float(2**53 + 1)))   # 9007199254740992
print(int(float(2**53 + 3)))   # 9007199254740996
print(math.ulp(1e16))          # 2.0
print(math.ulp(1.76e9))        # 2.384185791015625e-07
```

`math.ulp` (Python 3.9 and later) returns the spacing directly. In C#, `Math.BitIncrement(x) - x` gives the same number, and in JavaScript `Number.EPSILON` is the ULP of 1.0 only.

## Where this shows up today

As of November 2026, JSON (RFC 8259) still places no limit on number size, while JavaScript's `JSON.parse` converts every number to binary64. The I-JSON profile (RFC 7493) therefore advises keeping integers within ±(2\^53 − 1). Public APIs that use 64-bit identifiers commonly send them as strings for this reason, and JavaScript clients that need exact integers use `BigInt` or a big-number JSON parser.

## Pitfalls checklist

- Using a fixed absolute epsilon such as `1e-9` across magnitudes from 1 to 10\^16.
- Counting, summing cents, or accumulating nanoseconds in a `float` or `double` past the exact-integer limit.
- Treating `Number.MAX_SAFE_INTEGER` (2\^53 − 1) as the largest representable number rather than the largest safe consecutive integer.
- Storing epoch seconds in binary32 (128-second steps today).
- Round-tripping 64-bit database IDs through a JavaScript `Number`.

## Key takeaways

- ULP(x) = 2\^(e − 52) in binary64; it doubles at every power of two.
- Integers are exact only up to 2\^53 (binary64) and 2\^24 (binary32).
- Adding less than half an ULP changes nothing; long-running accumulators stall.
- Large IDs and fine-grained timestamps need integers or strings, not binary floats.
