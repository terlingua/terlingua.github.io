---
title: "The Hidden Bit and Exact Decoding"
questions:
  - id: "binary64-precision-bits"
    type: "single"
    prompt: "binary64 stores a 52-bit fraction. How many bits of significand precision does a normal binary64 number have?"
    explanation: |-
      For normal numbers the significand is 1.F: one implicit leading 1 plus the 52 stored fraction bits, so the precision is p = 52 + 1 = 53 bits. IEEE 754-2019 lists binary64 with p = 53 (and binary32 with p = 24). 53 bits correspond to about 53 × log10(2) ≈ 15.95 decimal digits.

      "52" counts only the stored bits and forgets the hidden bit, which is free precision gained because a normalized binary significand always starts with 1.

      "54" double-counts: it adds the sign bit to the significand. The sign is a separate field and does not add precision.

      "64" is the total width of the format, which also contains the sign and the 11-bit exponent field.
    options:
      - text: "52"
      - text: "54"
      - text: "64"
      - text: "53"
        correct: true
  - id: "encode-minus-six-point-five"
    type: "single"
    prompt: "What is the binary32 bit pattern of `-6.5f`?"
    explanation: |-
      Step by step: 6.5 = 110.1 in binary = 1.101 × 2\^2. Sign S = 1. Exponent e = 2, stored E = 2 + 127 = 129 = `10000001`. Fraction = the bits after the leading 1: `101` followed by 20 zeros.

      Concatenate: `1 10000001 10100000000000000000000` → nibbles `1100 0000 1101 0000 0000 …` = `0xC0D00000`.

      "0x40D00000" is +6.5: the sign bit was left at 0.

      "0xC0E80000" stores the leading 1 in the fraction (`1101…`) instead of dropping it. Decoded, that pattern is 1.1101 × 2\^2 = −7.25.

      "0xC0500000" uses exponent 1 (E = 128) instead of 2, which decodes to −1.101 × 2\^1 = −3.25.
    options:
      - text: "0xC0D00000"
        correct: true
      - text: "0x40D00000"
      - text: "0xC0E80000"
      - text: "0xC0500000"
  - id: "decode-3e200000"
    type: "single"
    prompt: "Which value does the binary32 pattern `0x3E200000` encode?"
    explanation: |-
      Split `0x3E200000` = `0011 1110 0010 0000 …` into fields: sign 0; exponent `01111100` = 124; fraction `0100000…` = binary .01 = 0.25.

      Value = 1.25 × 2\^(124 − 127) = 1.25 × 2\^−3 = 1.25 / 8 = 0.15625.

      "0.125" ignores the fraction (it is 1.0 × 2\^−3), as if the pattern had no fraction bits set.

      "0.03125" ignores the hidden bit and treats the significand as 0.25: 0.25 × 2\^−3 = 0.03125.

      "0.3125" uses exponent −2 instead of −3, an off-by-one in the bias (subtracting 126). Its pattern would be `0x3EA00000`.
    options:
      - text: "0.125"
      - text: "0.15625"
        correct: true
      - text: "0.03125"
      - text: "0.3125"
  - id: "exact-value-of-float-pi"
    type: "single"
    prompt: "`(float)Math.PI` has the bit pattern `0x40490FDB`. What is the exact value that this pattern represents?"
    explanation: |-
      Every finite binary floating-point number is a dyadic rational, M × 2\^k with integer M, so its exact decimal expansion is finite. Here: sign 0, exponent `10000000` = 128 → e = 1, significand 1.F = 0xC90FDB / 2\^23 = 13176795 / 8388608. Value = 13176795 / 8388608 × 2 = 13176795 / 4194304 = 3.1415927410125732421875 exactly (22 digits after the decimal point, because the denominator is 2\^22).

      "3.14159265358979…, the value of π" is π itself; π is irrational and cannot be stored exactly in any binary format. The float is the nearest binary32 value to π, about 8.7 × 10\^−8 above it.

      "3.1415927, exactly" is the shortest decimal string that converts back to the same float. Printing routines show it, but it is a label for the float, not its value.

      "3.14159274101257, exactly" is the exact value cut off after 14 decimals. It is also not the stored value; the expansion continues to 3.1415927410125732421875.
    options:
      - text: "3.14159265358979…, the value of π"
      - text: "3.1415927, exactly"
      - text: "3.1415927410125732421875"
        correct: true
      - text: "3.14159274101257, exactly"
  - id: "exponent-mask-bug"
    type: "single"
    prompt: "This helper is meant to return the unbiased exponent of a normal `float`. What does `Exponent(2.0f)` return?\n\n```csharp\nstatic int Exponent(float x)\n{\n    int bits = BitConverter.SingleToInt32Bits(x);\n    return ((bits >> 23) & 0x7F) - 127;\n}\n```"
    explanation: |-
      The exponent field of binary32 is 8 bits wide (bits 30–23), so the mask must be `0xFF`. The code masks with `0x7F`, keeping only 7 bits and dropping the field's top bit.

      Trace for 2.0f: pattern `0x40000000`; `bits >> 23` = `0x80` = 128 (sign 0, field 128). `128 & 0x7F` = 0. Result 0 − 127 = −127.

      "1" is what a correct implementation (`& 0xFF`) returns: 128 − 127 = 1. The bug is hidden by testing only with 1.0f, whose field 127 = `0x7F` survives the narrow mask and gives the right answer 0.

      "128" forgets the subtraction of the bias and also ignores the mask.

      "0" is the result for 1.0f, not 2.0f.

      "-63" is what you get by misreading the shift as `>> 24`: `0x40000000 >> 24` = 64, `64 & 0x7F` = 64, 64 − 127 = −63. The field starts at bit 23, not on a byte boundary.
    options:
      - text: "1"
      - text: "128"
      - text: "-127"
        correct: true
      - text: "0"
      - text: "-63"
  - id: "largest-finite-binary32"
    type: "multiple"
    prompt: "Which statements about the binary32 pattern `0x7F7FFFFF` are correct? Select all that apply."
    explanation: |-
      Correct:

      - Its fields are sign 0, exponent `11111110` = 254 (the largest non-reserved value) and an all-ones fraction, so it is the largest finite binary32 value.
      - Its value is 1.11…1 (23 ones after the point) × 2\^(254 − 127) = (2 − 2\^−23) × 2\^127 = 2\^128 − 2\^104 ≈ 3.4028235 × 10\^38.
      - Adding 1 to the pattern gives `0x7F800000`: exponent all ones, fraction zero, which is +infinity. This is the bit-level view of overflow.

      Wrong:

      - "Its value is 2\^128 − 1" treats the significand as if it had 128 bits of precision. Only 24 bits exist, so the gap below 2\^128 is 2\^104, not 1.
      - "Its stored exponent is 255, the largest field value" confuses it with the reserved all-ones field used by infinities and NaNs; `0x7F7FFFFF` has `11111110` = 254.
    options:
      - text: "It is the largest finite binary32 value."
        correct: true
      - text: "Its value is 2\\^128 − 1."
      - text: "Its stored exponent is 255, the largest field value."
      - text: "Its value is (2 − 2\\^−23) × 2\\^127."
        correct: true
      - text: "Incrementing the pattern as an integer gives +infinity."
        correct: true
---

## Learning objectives

- Explain why normal binary floats store one bit fewer than their precision.
- Encode a decimal value with a short binary expansion into a binary32 pattern by hand.
- Decode any normal binary32 pattern to its exact value.
- Distinguish a float's exact value from the shortest string that prints it.

## The invariant

A binary significand in normalized scientific notation always begins with 1: any non-zero binary number can be shifted until its leading digit is 1, and in base 2 there is no other non-zero digit. IEEE 754 therefore does not store that digit. For a normal number (stored exponent E not all zeros and not all ones):

value = (−1)\^S × (1 + F / 2\^t) × 2\^(E − bias)

where F is the stored fraction read as an unsigned integer and t is its width (23 or 52). The format's precision p is t + 1: 24 bits for binary32 and 53 bits for binary64. That extra bit is called the hidden or implicit bit.

Two consequences matter in practice:

1. **Every finite float is an exact dyadic rational.** The value is an integer significand times a power of two, so it has a finite decimal expansion. A float never "is approximately 0.1"; it is exactly some other number that happens to be the nearest representable one.
2. **The spacing between neighbors is fixed within a binade.** Within [2\^e, 2\^(e+1)) consecutive floats differ by 2\^(e − p + 1). In binary32 just above 1.0 the step is 2\^−23; just above 2.0 it is 2\^−22.

The hidden bit only works if zero has a separate encoding, since zero has no leading 1. That is one job of the reserved E = 0 field (see the next section).

**Encoding by hand** (for values with a short binary expansion):

1. Write the magnitude in binary.
2. Shift the binary point until one 1 is left of it; the number of places shifted left is e (negative if shifted right).
3. Store E = e + bias, then the bits after the point as F, padded with zeros.

**Decoding by hand:** split the pattern into sign, E and F, then compute (1 + F/2\^t) × 2\^(E − bias).

## Worked example

Encode −6.5 as binary32:

| Step | Result |
|---|---|
| Binary magnitude | 6.5 = 110.1 |
| Normalize | 1.101 × 2\^2 |
| Fields | S = 1, E = 2 + 127 = 129 = `10000001`, F = `101` then 20 zeros |
| Pattern | `1100 0000 1101 0000 …` = `0xC0D00000` |

Decode `0x40490FDB` (the binary32 nearest to π):

| Step | Result |
|---|---|
| Fields | S = 0, E = `10000000` = 128, F = `0x490FDB` |
| Significand | 1 + 0x490FDB / 2\^23 = 0xC90FDB / 2\^23 = 13176795 / 8388608 |
| Value | 13176795 / 8388608 × 2\^1 = 3.1415927410125732421875 |

```python
from decimal import Decimal
import struct
x = struct.unpack(">f", bytes.fromhex("40490fdb"))[0]
print(x)            # 3.1415927410125732  (binary64 repr of the widened float)
print(Decimal(x))   # 3.1415927410125732421875  (exact value)
```

`Decimal(x)` prints the exact value because the conversion from a binary float to `decimal.Decimal` is exact. The first line is a binary64 shortest string, because Python widened the binary32 value to a `float`, which is binary64.

## Where this shows up today

As of November 2026, the shortest-round-trip printing used by Python's `repr` (since Python 3.1), JavaScript's `Number.prototype.toString` and .NET's default `double.ToString()` (since .NET Core 3.0) hides exact values, which is why bugs that depend on them look mysterious. When a value matters to the last bit, print it exactly (`Decimal(x)` in Python, `x.ToString("G17")` in C# for 17 significant digits, or the hex pattern) before reasoning about it.

## Pitfalls checklist

- Forgetting to drop the leading 1 when encoding, or to add it back when decoding.
- Masking the binary32 exponent with 7 bits (`0x7F`) instead of 8 (`0xFF`); tests that use only 1.0 still pass.
- Believing that the printed string of a float is its value.
- Assuming precision is the number of stored fraction bits (23 or 52) rather than 24 or 53.
- Computing the largest float as 2\^128 − 1 or similar integer-style limits.

## Key takeaways

- Normal numbers have an implicit leading 1, so precision is 24 bits (binary32) or 53 bits (binary64).
- Value = (1 + F/2\^t) × 2\^(E − bias); encoding and decoding are mechanical.
- Every finite float is an exact dyadic rational with a finite decimal expansion.
- The largest finite binary32 is (2 − 2\^−23) × 2\^127; one more step in the bit pattern is infinity.
