---
title: "Subnormals and Gradual Underflow"
questions:
  - id: "smallest-normal-binary32"
    type: "single"
    prompt: "What is the smallest positive **normal** binary32 value?"
    explanation: |-
      The smallest stored exponent for normal numbers is E = 1 (E = 0 is reserved), so the smallest normal value is 1.0 × 2\^(1 − 127) = 2\^−126 ≈ 1.1754944 × 10\^−38. Its pattern is `0x00800000`.

      "2\^−127" applies the bias to E = 0, which is not a normal exponent; E = 0 means zero or subnormal.

      "2\^−149" is the smallest positive *subnormal*, 2\^−126 × 2\^−23, not the smallest normal.

      "2\^−128" has no special meaning in binary32; it is an ordinary subnormal value (`0x00200000`).
    options:
      - text: "2\\^−126"
        correct: true
      - text: "2\\^−127"
      - text: "2\\^−149"
      - text: "2\\^−128"
  - id: "decode-pattern-one"
    type: "single"
    prompt: "Which value does the binary32 pattern `0x00000001` encode?"
    explanation: |-
      Fields: sign 0, E = 0, F = 1. E = 0 with F ≠ 0 is a subnormal: value = 0.F × 2\^(1 − bias) = (F / 2\^23) × 2\^−126 = 1 × 2\^−23 × 2\^−126 = 2\^−149 ≈ 1.4013 × 10\^−45. It is the smallest positive binary32 value.

      "2\^−150" uses 0 − bias = −127 as the exponent; subnormals use 1 − bias = −126, the same exponent as the smallest normals.

      "2\^−126" reads the pattern as if the hidden bit were 1; subnormals have an implicit leading 0, which is what lets them get smaller than 2\^−126.

      "1.0" reads the pattern as the integer 1; the bits are a float encoding, not an integer.
    options:
      - text: "2\\^−150"
      - text: "2\\^−149"
        correct: true
      - text: "2\\^−126"
      - text: "1.0"
  - id: "why-one-minus-bias"
    type: "single"
    prompt: "For stored exponent E = 0, IEEE 754 uses the exponent 1 − bias instead of 0 − bias, and a leading 0 instead of 1. Why?"
    explanation: |-
      With exponent 1 − bias, the largest subnormal (0.111…1 × 2\^−126 in binary32) and the smallest normal (1.000…0 × 2\^−126) are adjacent: both binades use the same spacing 2\^−149, so the number line has no gap and no overlap between them. This is what makes underflow gradual.

      "So that +0 and −0 can both be represented" is unrelated: signed zero comes from the separate sign bit.

      "Because hardware cannot represent an exponent of −127" is false; the choice is a definition in the standard, not a hardware limit.

      "To double the dynamic range of the format" overstates it: subnormals extend the range down by only t = 23 binades (from 2\^−126 to 2\^−149), and they do so with decreasing precision.
    options:
      - text: "So that +0 and −0 can both be represented"
      - text: "Because hardware cannot represent an exponent of −127"
      - text: "So that subnormals continue the normal spacing with no gap"
        correct: true
      - text: "To double the dynamic range of the format"
  - id: "what-prints-underflow"
    type: "single"
    prompt: "In Python (binary64 floats), `5e-324` is the smallest positive subnormal. What does this print?\n\n```python\ntiny = 5e-324\nprint(tiny / 2, tiny * 0.75)\n```"
    explanation: |-
      The only representable values near these results are 0 and `tiny` = 2\^−1074, so each result rounds to one of them under round-to-nearest, ties-to-even.

      - `tiny / 2` = 2\^−1075 is exactly halfway between 0 and 2\^−1074. A tie goes to the value with an even last significand bit; 0 has significand 0 (even) and 2\^−1074 has significand 1 (odd), so the result is 0.0.
      - `tiny * 0.75` = 0.75 × 2\^−1074 is closer to 2\^−1074 (distance 0.25 units) than to 0 (distance 0.75 units), so it rounds up to 5e-324.

      Output: `0.0 5e-324`.

      "2.5e-324 3.75e-324" assumes subnormals can hold any small value; there is nothing below 2\^−1074 except 0.

      "0.0 0.0" assumes anything smaller than `tiny` flushes to zero; IEEE 754 rounds to nearest.

      "5e-324 5e-324" assumes results round away from zero to avoid underflow; the default rounding mode is round-to-nearest, ties-to-even.
    options:
      - text: "2.5e-324 3.75e-324"
      - text: "0.0 0.0"
      - text: "5e-324 5e-324"
      - text: "0.0 5e-324"
        correct: true
  - id: "gradual-underflow-properties"
    type: "multiple"
    prompt: "With subnormals enabled (IEEE 754 default behavior), which statements are correct? Select all that apply."
    explanation: |-
      Correct:

      - For finite x and y, x − y \== 0 exactly when x \== y. If x and y are close enough for their difference to be tiny, that difference is a multiple of the smallest spacing (2\^−149 in binary32, 2\^−1074 in binary64) and is exactly representable as a subnormal or small normal; if they are not that close, the rounded difference is far from 0. Either way a non-zero exact difference never becomes 0.
      - Precision decreases as values shrink into the subnormal range: a subnormal with leading fraction bits zero has fewer significant bits (2\^−149 has exactly 1).
      - The smallest subnormal equals the smallest normal times 2\^−t: 2\^−126 × 2\^−23 = 2\^−149 in binary32.

      Wrong:

      - "Subnormals keep the full 24-bit precision of binary32" contradicts the leading-0 encoding: the implicit bit is 0, so leading zeros in F are lost precision.
      - "Flush-to-zero mode preserves the property that x − y \== 0 only when x \== y" is exactly backwards; FTZ replaces subnormal results with 0, which is how distinct x and y can produce a zero difference.
    options:
      - text: "The smallest subnormal equals the smallest normal times 2\\^−t."
        correct: true
      - text: "Subnormals keep the full 24-bit precision of binary32."
      - text: "Flush-to-zero mode preserves the property that x − y \\== 0 only when x \\== y."
      - text: "For finite x and y, x − y \\== 0 exactly when x \\== y."
        correct: true
      - text: "Values in the subnormal range have fewer significant bits as they get smaller."
        correct: true
  - id: "ftz-division-guard"
    type: "single"
    prompt: "A shader computes `if (a != b) r = 1.0 / (a - b);` in binary32 with flush-to-zero enabled. With `a = 1.5e-38` and `b = 1.2e-38` (both normal), what happens?"
    explanation: |-
      Both inputs are normal (above 2\^−126 ≈ 1.1755 × 10\^−38), so `a != b` is true. Their difference, about 3 × 10\^−39, is below the smallest normal and would be a subnormal. Flush-to-zero replaces that subnormal result with 0, so `1.0 / 0` gives +infinity even though the guard said the values differ. With IEEE default gradual underflow the difference would be a subnormal near 3 × 10\^−39 and the reciprocal about 3.3 × 10\^38, which is finite in binary32.

      "The guard prevents the division, so `r` is unchanged" assumes `!=` and subtraction see the same values. The comparison is exact; only the subtraction's result is flushed.

      "`r` is about `3.3e38`, the reciprocal of the difference" is what happens without FTZ.

      "The comparison throws because the inputs are subnormal" is wrong twice: the inputs are normal, and IEEE comparisons never throw.
    options:
      - text: "The guard prevents the division, so `r` is unchanged."
      - text: "`a - b` flushes to 0, so `r` becomes +infinity."
        correct: true
      - text: "`r` is about `3.3e38`, the reciprocal of the difference."
      - text: "The comparison throws because the inputs are subnormal."
---

## Learning objectives

- Decode stored exponent 0 as zero or a subnormal and compute subnormal values.
- State the smallest normal and smallest subnormal of binary32 and binary64.
- Explain why gradual underflow guarantees that distinct floats have a non-zero difference.
- Recognize the effect of flush-to-zero modes on correctness.

## The invariant

When the stored exponent E is 0, IEEE 754 changes the decoding rule:

value = (−1)\^S × 0.F × 2\^(1 − bias)

The implicit leading bit becomes 0 and the exponent is pinned at the smallest normal exponent (1 − bias). F = 0 gives ±0; F ≠ 0 gives a subnormal (older texts say "denormal").

| | binary32 | binary64 |
|---|---|---|
| Smallest normal | 2\^−126 ≈ 1.18 × 10\^−38 | 2\^−1022 ≈ 2.23 × 10\^−308 |
| Smallest subnormal | 2\^−149 ≈ 1.40 × 10\^−45 | 2\^−1074 ≈ 4.94 × 10\^−324 |
| Positive subnormals | 2\^23 − 1 | 2\^52 − 1 |

Because subnormals use the same exponent as the smallest binade of normals, the spacing near zero is uniform: 2\^−149 in binary32 from 0 all the way up to 2\^−125. This is *gradual underflow*, the design argued for by Kahan and explained in Goldberg's "What Every Computer Scientist Should Know About Floating-Point Arithmetic". It buys one important theorem: for finite x and y, x − y = 0 if and only if x = y. Code such as `if (x != y) z = 1 / (x - y)` is then safe from division by zero.

The price is precision. A subnormal with k leading zero bits in F has only 23 − k significant bits (binary32), so relative error grows as values approach zero. Some hardware also handles subnormal operands or results slowly, through microcode assists that can make each affected operation tens to hundreds of times slower on certain x86 cores.

That is why processors offer non-standard modes. **Flush-to-zero (FTZ)** replaces subnormal results with zero, and **denormals-are-zero (DAZ)** treats subnormal inputs as zero. On x86 these are bits in the SSE/AVX control register MXCSR; on Arm they are controlled by the FZ bit of the floating-point control register. Compilers' fast-math options may enable them (GCC's `-ffast-math` on x86 links startup code that sets FTZ and DAZ). With these modes on, the x − y theorem no longer holds.

## Worked example

Decode `0x00000003` (binary32): S = 0, E = 0, F = 3 → 3 × 2\^−23 × 2\^−126 = 3 × 2\^−149 ≈ 4.2 × 10\^−45. It has 2 significant bits.

Trace the underflow theorem with a = 1.5 × 10\^−38 and b = 1.2 × 10\^−38 in binary32. Both are normal. The exact difference is about 3.0 × 10\^−39, below 2\^−126:

| Mode | Result of a − b | 1 / (a − b) |
|---|---|---|
| IEEE default (gradual underflow) | subnormal ≈ 3.0 × 10\^−39 | ≈ 3.3 × 10\^38 (finite) |
| FTZ | 0 | +infinity |

In binary64, Python shows the floor directly:

```python
import sys
print(sys.float_info.min)  # 2.2250738585072014e-308  (smallest normal)
print(5e-324)              # 5e-324  (smallest subnormal, 2**-1074)
print(5e-324 / 2)          # 0.0  (halfway case rounds to even: zero)
```

## Where this shows up today

As of November 2026, flush-to-zero is common wherever throughput beats strict IEEE behavior: GPU shader pipelines, audio DSP code (where decaying filter states would otherwise drift into subnormals and spike CPU use) and machine-learning kernels. Low-precision formats used in ML make the subnormal range a large fraction of all values, so whether a kernel flushes subnormals measurably changes results.

## Pitfalls checklist

- Writing the subnormal exponent as 0 − bias (−127 or −1023) instead of 1 − bias.
- Assuming the smallest float is the smallest *normal* (`float.Epsilon` in C# is actually the smallest subnormal 2\^−149, unlike C's `FLT_EPSILON`).
- Relying on `x != y` to guard a division when a library or compiler flag may enable FTZ.
- Ignoring the precision loss near zero when computing relative errors of tiny values.
- Benchmarking numeric code without checking for subnormal slowdowns.

## Key takeaways

- E = 0 means 0.F × 2\^(1 − bias): zero when F = 0, a subnormal otherwise.
- binary32 bottoms out at 2\^−149, binary64 at 2\^−1074.
- Gradual underflow guarantees x − y = 0 ⇔ x = y; FTZ/DAZ break that guarantee.
- Subnormals trade precision (and sometimes speed) for a gap-free number line around zero.
