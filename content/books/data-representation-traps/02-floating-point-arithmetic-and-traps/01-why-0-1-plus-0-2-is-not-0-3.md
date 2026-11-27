---
title: "Why 0.1 + 0.2 Is Not 0.3"
questions:
  - id: "exact-binary-fractions"
    type: "multiple"
    prompt: "Which of these decimal literals are stored **exactly** as binary64 values? Select all that apply."
    explanation: |-
      A number has a finite binary expansion exactly when, written as a reduced fraction, its denominator is a power of two. (It must also fit in 53 significant bits, which these small values do.)

      Correct:

      - 0.375 = 3/8 = 0.011 in binary.
      - 0.5 = 1/2 = 0.1 in binary.
      - 2.25 = 9/4 = 10.01 in binary.

      Wrong:

      - 0.1 = 1/10; the denominator 10 = 2 × 5 has the factor 5, so the binary expansion repeats forever: 0.000110011001100… The literal is rounded to the nearest double.
      - 0.3 = 3/10; same factor 5, same infinite repetition. Having only one significant decimal digit says nothing about the binary expansion.
    options:
      - text: "0.1"
      - text: "0.375"
        correct: true
      - text: "0.5"
        correct: true
      - text: "2.25"
        correct: true
      - text: "0.3"
  - id: "stored-point-one-direction"
    type: "single"
    prompt: "How does the binary64 value stored for the literal `0.1` compare with the real number 0.1?"
    explanation: |-
      The literal is converted to the nearest binary64 value (IEEE 754 round-to-nearest). The infinite expansion 1.1001 1001 1001… × 2\^−4 must be cut after 52 fraction bits. The bits after the cut start `1001…`, which is more than half a unit, so the conversion rounds **up**. The stored value is 0.1000000000000000055511151231257827021181583404541015625, about 5.55 × 10\^−18 above 0.1.

      "It is exactly 0.1; only printing adds error" is backwards: printing shows the shortest string that maps back to the same double, which hides the error.

      "It is slightly less than 0.1" would be true if the conversion truncated (chopped) the expansion. IEEE conversion rounds to nearest.

      "It is exactly 0.1 in binary32 but not in binary64" is false: 0.1 is inexact in every binary format, and the binary32 value is 0.100000001490116119384765625.
    options:
      - text: "It is exactly 0.1; only printing adds error."
      - text: "It is slightly less than 0.1."
      - text: "It is exactly 0.1 in binary32 but not in binary64."
      - text: "It is slightly greater than 0.1."
        correct: true
  - id: "what-prints-point-three"
    type: "single"
    prompt: "What does this Python line print?\n\n```python\nprint(0.1 + 0.2 == 0.3, 0.1 + 0.2)\n```"
    explanation: |-
      `0.1`, `0.2` and `0.3` are each rounded to the nearest double. The exact sum of the first two doubles is 0.3000000000000000166533453693773481063544750213623046875, which rounds to the double 0.3000000000000000444089209850062616169452667236328125. The literal `0.3` is the different double 0.299999999999999988897769753748434595763683319091796875. They differ, so `==` is `False`, and Python's shortest round-trip printing of the sum needs 17 significant digits: `0.30000000000000004`.

      Output: `False 0.30000000000000004`.

      "True 0.3" assumes decimal arithmetic.

      "False 0.3" assumes printing rounds to a few digits; Python's `repr` prints the shortest string that converts back to the same double, and `0.3` would convert to a different double.

      "True 0.30000000000000004" is self-contradictory: if the two were equal, the sum would print as `0.3`.
    options:
      - text: "False 0.30000000000000004"
        correct: true
      - text: "True 0.3"
      - text: "False 0.3"
      - text: "True 0.30000000000000004"
  - id: "hex-pattern-of-point-one"
    type: "single"
    prompt: "What is the binary64 bit pattern of `0.1`?"
    explanation: |-
      0.1 = 1.6 × 2\^−4. Stored exponent: −4 + 1023 = 1019 = `0x3FB` in the top 12 bits (sign 0, exponent `01111111011`). The fraction is the binary expansion of 0.6 = 0.1001 1001 1001… → hex `999…`. Fifty-two fraction bits are 13 hex digits: `9999999999999` followed by more `9…` (binary `1001…`). The first discarded bit is 1 and further bits are non-zero, so the fraction rounds up from `…9999` to `…999A`. Pattern: `0x3FB999999999999A`.

      "0x3FB9999999999999" truncates instead of rounding to nearest; it is the double just below the stored value.

      "0x3FC999999999999A" has exponent `0x3FC` (−3): that is 0.2.

      "0x3DCCCCCD" is the 32-bit binary32 pattern of 0.1f, not a binary64 pattern.
    options:
      - text: "0x3FB9999999999999"
      - text: "0x3FB999999999999A"
        correct: true
      - text: "0x3FC999999999999A"
      - text: "0x3DCCCCCD"
  - id: "tie-in-point-one-plus-point-two"
    type: "single"
    prompt: "The exact sum of the doubles `0.1` and `0.2` lies **exactly halfway** between the doubles `0x3FD3333333333333` (≈ 0.29999999999999998890) and `0x3FD3333333333334` (≈ 0.30000000000000004441). Why does IEEE 754 addition return `0x3FD3333333333334`?"
    explanation: |-
      Default IEEE 754 rounding is *round to nearest, ties to even*: when the exact result is equidistant from two representable values, the one whose least significant significand bit is 0 is chosen. `…3334` ends in hex 4 = binary `0100` (last bit 0, even); `…3333` ends in binary `0011` (odd). So the tie goes to `0x3FD3333333333334`.

      Trace: the exact sum is 0.3000000000000000166533453693773481063544750213623046875. Distance down to `…3333`: 0.0000000000000000277555756156289135105907917022705078125. Distance up to `…3334`: the same, 2\^−55. A genuine tie.

      "Addition always rounds up" is not an IEEE rounding mode for default arithmetic; round-toward-positive exists but is not the default.

      "The upper value is closer to the exact sum" is false here; both distances are exactly 2\^−55.

      "Ties always round away from zero" describes ties-to-away, which IEEE 754-2019 defines for decimal formats and as an optional attribute, not as the binary default.
    options:
      - text: "Addition always rounds up."
      - text: "The upper value is closer to the exact sum."
      - text: "Ties go to the value whose last significand bit is 0."
        correct: true
      - text: "Ties always round away from zero."
  - id: "float-literal-versus-double"
    type: "single"
    prompt: "What does this C# line print?\n\n```csharp\nConsole.WriteLine((double)0.1f == 0.1);\n```"
    explanation: |-
      `0.1f` is rounded to the nearest binary32: 0.100000001490116119384765625 (24-bit precision). Widening to `double` is exact, so it stays that value. The literal `0.1` is rounded to the nearest binary64: 0.1000000000000000055511151231257827…. The two differ around the ninth significant digit, so the comparison is `False`.

      "True" assumes widening re-rounds toward the decimal 0.1; widening preserves the binary32 value exactly and cannot recover bits that were never stored.

      "It does not compile" is wrong: C# allows comparing a `double` with a `double`, and the cast is explicit.

      "It depends on the CPU" is wrong: IEEE 754 conversions and comparisons are deterministic on every conforming platform; .NET uses IEEE binary32 and binary64.
    options:
      - text: "True"
      - text: "It does not compile."
      - text: "It depends on the CPU."
      - text: "False"
        correct: true
---

## Learning objectives

- Decide whether a decimal literal is exactly representable in binary.
- Explain how a literal is rounded to the nearest float and in which direction.
- Trace `0.1 + 0.2` to the bit and explain why it differs from `0.3`.
- Apply round-to-nearest, ties-to-even.

## The invariant

A real number has a finite binary expansion only if it can be written as m / 2\^k. Decimal fractions whose reduced denominator contains a factor 5 (0.1 = 1/10, 0.2 = 1/5, 0.3 = 3/10) repeat forever in binary, the same way 1/3 repeats forever in decimal. A binary float keeps p significant bits (24 or 53), so such literals must be rounded.

IEEE 754-2019 requires every basic operation (+, −, ×, ÷, square root, fused multiply-add and conversions, including decimal-to-binary conversion of literals) to be **correctly rounded**: the result is the exact mathematical result rounded once, according to the rounding direction in force. The default for binary formats is **roundTiesToEven**:

1. If the exact result is representable, return it.
2. Otherwise return the nearer of the two neighboring floats.
3. If it is exactly halfway, return the neighbor whose last significand bit is 0.

Ties-to-even avoids the slow upward drift that ties-away-from-zero would cause when many halfway cases are rounded.

So `0.1 + 0.2` involves three roundings before the comparison: the literal `0.1`, the literal `0.2`, and the sum. The literal `0.3` is a fourth, independent rounding. Nothing guarantees that the rounded sum of two rounded values equals the rounding of the exact decimal sum.

## Worked example

Binary64 values, exact:

| Literal or expression | Hex pattern | Exact value |
|---|---|---|
| `0.1` | `0x3FB999999999999A` | 0.1000000000000000055511151231257827021181583404541015625 |
| `0.2` | `0x3FC999999999999A` | 0.200000000000000011102230246251565404236316680908203125 |
| exact sum of the two | — | 0.3000000000000000166533453693773481063544750213623046875 |
| `0.3` | `0x3FD3333333333333` | 0.299999999999999988897769753748434595763683319091796875 |
| `0.1 + 0.2` | `0x3FD3333333333334` | 0.3000000000000000444089209850062616169452667236328125 |

The exact sum sits exactly 2\^−55 from each neighbor. Ties-to-even picks `…334` (last bit 0). The result differs from `0.3` by one unit in the last place.

The same arithmetic happens in every language that uses binary64:

```javascript
console.log(0.1 + 0.2);          // 0.30000000000000004
console.log(0.1 + 0.2 === 0.3);  // false
console.log((0.1).toString(2));  // 0.0001100110011001100110011001100110011001100110011001101
```

The last line shows the stored binary fraction; the final `…1101` instead of `…1100` is the round-up.

```python
from decimal import Decimal
print(Decimal(0.1))        # 0.1000000000000000055511151231257827021181583404541015625
print(float.hex(0.1))      # 0x1.999999999999ap-4
```

## Where this shows up today

As of November 2026, JavaScript, Python, Java and C# all evaluate floating-point literals and arithmetic in IEEE 754 binary64 by default, so this example behaves identically across them. Spreadsheet engines and JSON parsers inherit the same behavior whenever they parse decimal text into binary doubles, which is why exported reports sometimes show values like 0.30000000000000004.

## Pitfalls checklist

- Assuming a short decimal literal is exact in binary.
- Comparing results of floating-point arithmetic with `==` against decimal literals.
- Mixing `float` and `double` values of "the same" literal and expecting equality.
- Believing that printing with fewer digits fixes the stored value.
- Assuming ties round away from zero, as taught in school, instead of to even.

## Key takeaways

- Only fractions with power-of-two denominators are exact in binary.
- Every IEEE operation, including literal parsing, is a single correct rounding.
- Default rounding is to nearest, ties to even.
- `0.1 + 0.2` and `0.3` are different doubles one ULP apart; the bug is the expectation, not the hardware.
